using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Humanizer.On;

namespace AutoServiceMVC.Services.Implement
{
    public class UserRepository : IAuthenticateService<User>, ICommonRepository<User>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;
        private readonly IHashPassword _hash;

        public UserRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor, IHashPassword hash) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
            _hash = hash;
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.Users
                    .ToListAsync();

            if (result == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.LIST_EMPTY
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = result
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic users = null;

            if (!String.IsNullOrEmpty(search))
            {
                users = await _context.Users
                    .Where(u => u.FullName.Contains(search))
                    .ToListAsync();
            }
            else
            {
                users = await _context.Users
                    .ToListAsync();
            }

            var result = PaginatedList<User>.Create(users, page, Page_Size);

            if (result == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.LIST_EMPTY
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = result
            };
        }

        async Task<StatusMessage> ICommonRepository<User>.CreateAsync(User? entity)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED,
                Data = null
            };
        }

        async Task<StatusMessage> ICommonRepository<User>.DeleteByIdAsync(int? id)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED,
                Data = null
            };
        }

        async Task<StatusMessage> ICommonRepository<User>.GetByIdAsync(int? id)
        {
            var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.UserId == id);

            if(user == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND,
                    Data = null
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = user
            };
        }

        async Task<StatusMessage> IAuthenticateService<User>.RegisterAsync(Register register)
        {
            if (register.Password != register.AgainPassword)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Password was not match"
                };
            }

            var user = new User()
            {
                Username = register.Username,
                HashPassword = _hash.GetHashPassword(register.Password),
                FullName = register.FullName,
                Email = register.Email
            };

            await _context.AddAsync<User>(user);
            await _context.SaveChangesAsync();
            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Register success",
                Data = user
            };
        }

        async Task<StatusMessage> ICommonRepository<User>.UpdateAsync(User? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == entity.UserId);

            if(user == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            user.FullName = entity.FullName;
            user.Email = entity.Email;
            user.Point = entity.Point;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS,
                Data = user
            };
        }

        async Task<StatusMessage> IAuthenticateService<User>.ValidateLoginAsync(Login login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
            if(user == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Username is wrong"
                };
            }
            
            if(user.HashPassword != _hash.GetHashPassword(login.Password))
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Password is wrong"
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Username and Password is valid",
                Data = user
            };
        }
    }
}
