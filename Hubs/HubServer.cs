using AutoServiceMVC.Models;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Security.Claims;

namespace AutoServiceMVC.Hubs
{
    public class HubServer : Hub
    {
        private readonly IChatbotService _chatbot;
        private readonly ICookieService _cookie;

        public HubServer(
            IChatbotService chatbot,
            ICookieService cookie) 
        { 
            _chatbot = chatbot;
            _cookie = cookie;
        }
        public async Task JoinRoom(string type)
        {
            if(type == "user")
            {
                if (Context.User.Identity.IsAuthenticated && Context.User.Identity.AuthenticationType == "User_Scheme")
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, Context.User.FindFirstValue("Id"));
                }
                else
                {
                    var guestOrder = _cookie.GetCookie(Context.GetHttpContext(), "guest_order");
                    var orderIdList = string.IsNullOrEmpty(guestOrder) ? new List<string>() : guestOrder.Split(",").ToList();

                    foreach (var orderId in orderIdList)
                    {
                        Groups.AddToGroupAsync(Context.ConnectionId, orderId);
                    }
                }
            }
            else if (type == "employee")
            {
                 Groups.AddToGroupAsync(Context.ConnectionId, "Employee");
            }
            
        }

        public async Task AskChatbot(string message)
        {
            string answer = await _chatbot.AskMessage(message);
            Clients.Caller.SendAsync("ChatbotAnswer", answer);
        }

        public async Task CallStaff(string tablename)
        {
            Clients.Group("Employee").SendAsync("ReceiveNoti", $"Customer call at {tablename}");
        }
    }
}
