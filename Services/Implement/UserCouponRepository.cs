using AutoServiceBE.Models;
using AutoServiceMVC.Data;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
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

        public async Task<StatusMessage> Add(UserCoupon entity)
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
        
        public async Task<StatusMessage> DeleteById(int id)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> GetAll(string? search, int page)
        {
            dynamic userCoupons =  await _context.UserCoupons
                    .Include(uc => uc.User)
                    .Include(uc => uc.Coupon)
                    .AsNoTracking()
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

        public async Task<StatusMessage> GetById(int id)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> Update(UserCoupon entity)
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
                uc => uc.UserID == entity.UserID && uc.CouponID == entity.CouponID && uc.IsUsed == false);

            if (userCoupon == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.Update<UserCoupon>(userCoupon);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetByUserID(int userID)
        {
            if (userID == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var userCoupons = await _context.UserCoupons
                .Include(p => p.User)
                .Include(p => p.Coupon)
                .AsNoTracking()
                .Where(c => c.UserID == userID)
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
    }
}
