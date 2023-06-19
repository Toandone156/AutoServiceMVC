using AutoServiceMVC.Models;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AutoServiceMVC.Hubs
{
    public class HubServer : Hub
    {
        private readonly IChatbotService _chatbot;

        public HubServer(IChatbotService chatbot) 
        { 
            _chatbot = chatbot;
        }
        public async Task JoinRoom(string type)
        {
            if(type == "user")
            {
                if (Context.User.Identity.IsAuthenticated && Context.User.Identity.AuthenticationType == "User_Scheme")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.FindFirstValue("Id"));
                }
            }else if (type == "employee")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Employee");
            }
        }

        public async Task AskChatbot(string message)
        {
            string answer = await _chatbot.AskMessage(message);
            await Clients.Caller.SendAsync("ChatbotAnswer", answer);
        }
    }
}
