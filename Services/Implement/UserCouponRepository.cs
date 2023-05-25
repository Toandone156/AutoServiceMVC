using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Printing;

namespace AutoServiceMVC.Services.Implement
{
    public class UserCouponRepository : ICommonRepository<UserCoupon>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public UserCouponRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(UserCoupon? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<UserCoupon>(entity);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.ADD_SUCCESS,
                Data = entity
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
            dynamic userCoupons =  await _context.UserCoupons
                    .ToListAsync();

            var result = PaginatedList<UserCoupon>.Create(userCoupons, page, Page_Size);

            if (result == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.LIST_EMPTY
                };
            }
            else
            {
                return new StatusMessage()
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = result
                };
            }
        }

        public async Task<StatusMessage> GetByIdAsync(int? id)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> UpdateAsync(UserCoupon? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var userCoupon = await _context.UserCoupons.FirstOrDefaultAsync(
                uc => uc.UserId == entity.UserId && uc.CouponId == entity.CouponId && uc.IsUsed == false);

            if (userCoupon == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            userCoupon.IsUsed = entity.IsUsed;
            userCoupon.ExpireAt = entity.ExpireAt;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetByUserIdAsync(int userId)
        {
            var userCoupons = await _context.UserCoupons
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (userCoupons == null)
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
                Data = userCoupons
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.UserCoupons
                    .ToListAsync();

            if (result == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.LIST_EMPTY
                };
            }
            else
            {
                return new StatusMessage()
                {
                    IsSuccess = true,
                    Message = Message.GET_SUCCESS,
                    Data = result
                };
            }
        }

        public async Task<StatusMessage> DeleteByEntityAsync(UserCoupon entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            _context.UserCoupons.Remove(entity);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }
    }
}
