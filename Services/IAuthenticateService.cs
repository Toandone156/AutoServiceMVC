using AutoServiceMVC.Models;
using AutoServiceMVC.Models.System;

namespace AutoServiceMVC.Services
{
    public interface IAuthenticateService<T>
    {
        public Task<StatusMessage> ValidateLogin(Login login);
        public Task<StatusMessage> Register(Register register);
    }
}
