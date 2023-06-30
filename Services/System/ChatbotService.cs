using AutoServiceMVC.Data;
using AutoServiceMVC.Hubs;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AutoServiceMVC.Services.System
{
    public interface IChatbotService
    {
        Task<string> AskMessage(string message);
    }

    public class ChatbotService : IChatbotService
    {
        private readonly string _apiPath = "https://api.openai.com/v1/chat/completions";
        private readonly string _apiKey;
        private readonly string _model = "gpt-3.5-turbo-0613";
        private readonly List<BotMessage> messages = new List<BotMessage>();
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<HubServer> _hub;

        public ChatbotService(
            IServiceScopeFactory scopeFactory,
            IHubContext<HubServer> hub,
            IOptionsMonitor<AppSettings> monitor)
        {
            _scopeFactory = scopeFactory;
            _hub = hub;
            _apiKey = monitor.CurrentValue.ApiKey;
            InitMessage();
        }

        public async Task InitMessage()
        {
            messages.Add(new BotMessage
            {
                role = "system",
                content = "You are a customer service chatbot for SelfCoffee shop, an innovative contactless order and payment shop." +
                "Our shop is use qr code or table code for access to table and order product. And customers can payment by qr or bank account via VNPAY."
            });

            StringBuilder menuInfo = new StringBuilder();

            using(var scope = _scopeFactory.CreateScope())
            {
                var product = scope.ServiceProvider.GetRequiredService<ICommonRepository<Product>>();

                StatusMessage statusMessage = await product.GetAllAsync();
                var products = statusMessage.Data as List<Product>;

                products.ForEach(p =>
                {
                    menuInfo.Append($"Id:{p.ProductId},Name:{p.ProductName},Price:{p.Price};");
                });
            }

            messages.Add(new BotMessage
            {
                role = "system",
                content = "Menu information:" + menuInfo.ToString() + ". Do not send id for customers."
            });

            messages.Add(new BotMessage
            {
                role = "system",
                content = "Your primary role is to engage in natural and helpful conversations with customers, assisting them with their orders. " +
                "You should provide menu options, gather customer preferences, invoke the appropriate functions, and guide customers through the ordering and payment process." +
                "Remember to provide clear and concise information, handle errors gracefully, and offer assistance in a friendly manner. " +
                "If customer want order more product that already in cart, you can invoke function with new quantity." +
                "You do not listed all products in shop, just provice products list if customers need it and product not more than 10 products to opimize response speed." +
                "You do need use html format to in your response. Use <ol> and <li> to list product and make product name is bold style."
            });
        }

        public async Task<string> AskMessage(string message)
        {
            if(messages.Last() is ResponseFunctionMessage)
            {
                var jsonFunction_Call = JsonConvert.SerializeObject((messages.Last() as ResponseFunctionMessage).function_call);
                dynamic function_call = JsonConvert.DeserializeObject(jsonFunction_Call);

                messages.Add(new RequestFunctionMessage()
                {
                    role = "function",
                    name = function_call.name,
                    content = message
                });
            }
            else
            {
                messages.Add(new BotMessage { role = "user", content = message });
            }


            var functions = new List<dynamic>()
            {
                new
                {
                    name = "add_to_cart",
                    description = "Add product to cart to prepare payment.",
                    parameters = new
                    {
                        type = "object",
                        properties = new
                        {
                            orderdetails = new {
                                type = "array",
                                items = new
                                {
                                    type = "object",
                                    properties = new
                                    {
                                        id = new
                                        {
                                            type = "string",
                                            description = "Write id of product that customer want to order"
                                        },
                                        quantity = new
                                        {
                                            type = "string",
                                            description = "Quantity of product that user want to order"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var api = new
            {
                model = _model,
                messages = messages,
                functions = functions,
                format = "html"
            };

            var japi = JsonConvert.SerializeObject(api);

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var content = new StringContent(japi.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage res = await client.PostAsync(_apiPath, content);

            string jsonString = await res.Content.ReadAsStringAsync();
            dynamic response = JsonConvert.DeserializeObject(jsonString);

            var responseMessage = (response.choices)[0].message;

            BotMessage m = new BotMessage();

            if(responseMessage.function_call != null)
            {
                m = new ResponseFunctionMessage()
                {
                    role = responseMessage.role,
                    content = "null",
                    function_call = responseMessage.function_call
                };
            }
            else
            {
                m = new BotMessage()
                {
                    role = responseMessage.role,
                    content = responseMessage.content
                };
            }

            messages.Add(m);

            return JsonConvert.SerializeObject(m);
        }
    }
}
