using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Humanizer.On;

namespace AutoServiceMVC.Services.Implement
{
    public class RoleRepository : ICommonRepository<Role>
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<StatusMessage> CreateAsync(Role? entity)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> DeleteByIdAsync(int? id)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> GetByIdAsync(int? id)
        {
            if(id == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var table = await _context.Roles
                
                .FirstOrDefaultAsync(c => c.RoleId == id);

            if(table == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = table
            };
        }

        public async Task<StatusMessage> UpdateAsync(Role? entity)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.Roles
                
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
    }
}
