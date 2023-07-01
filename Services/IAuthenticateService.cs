using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;

namespace AutoServiceMVC.Services
{
    public interface IAuthenticateService<T>
    {
        public Task<StatusMessage> ValidateLoginAsync(Login login);
        public Task<StatusMessage> RegisterAsync(Register register);
        public Task<StatusMessage> CheckEmailAndUsernameAsync(string email, string? username);
    }
}
