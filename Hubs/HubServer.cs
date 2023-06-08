using AutoServiceMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AutoServiceMVC.Hubs
{
    public class HubServer : Hub
    {
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
    }
}
