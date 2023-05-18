using AutoServiceBE.Models;
using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AutoServiceMVC.Services.Implement
{
    public class UserRepository : IAuthenticateService<User>, ICommonRepository<User>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public UserRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> GetAll(string? search, int page)
        {
            dynamic users = null;

            if (!String.IsNullOrEmpty(search))
            {
                users = await _context.Users
                    .Include(u => u.UserType)
                    .AsNoTracking()
                    .Where(u => u.FullName.Contains(search))
                    .ToListAsync();
            }
            else
            {
                users = await _context.Users
                    .Include(u => u.UserType)
                    .AsNoTracking()
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

        async Task<StatusMessage> ICommonRepository<User>.Add(User entity)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED,
                Data = null
            };
        }

        async Task<StatusMessage> ICommonRepository<User>.DeleteById(int id)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED,
                Data = null
            };
        }

        async Task<StatusMessage> ICommonRepository<User>.GetById(int id)
        {
            var user = await _context.Users
                    .Include(u => u.UserType)
                    .AsNoTracking()
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

        async Task<StatusMessage> IAuthenticateService<User>.Register(Register register)
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
                HashPassword = register.Password,
                FullName = register.FullName,
                Email = register.Email,
                PhoneNum = register.PhoneNum
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

        async Task<StatusMessage> ICommonRepository<User>.Update(User entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var user = _context.Users.FirstOrDefaultAsync(u => u.UserId == u.UserId);
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS,
                Data = user
            };
        }

        async Task<StatusMessage> IAuthenticateService<User>.ValidateLogin(Login login)
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
            
            if(user.HashPassword != login.Password)
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
