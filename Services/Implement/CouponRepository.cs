using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Printing;
using static Humanizer.On;

namespace AutoServiceMVC.Services.Implement
{
    public class CouponRepository : ICommonRepository<Coupon>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public CouponRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(Coupon? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            if(entity.Quantity != null)
            {
                entity.Remain = entity.Quantity;
            }

            await _context.AddAsync<Coupon>(entity);
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
            if (id == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponId == id);

            if(coupon == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic coupons = null;

            if (!String.IsNullOrEmpty(search))
            {
                coupons = await _context.Coupons
                    .Where(u => u.CouponCode.Contains(search))
                    .ToListAsync();
            }
            else
            {
                coupons = await _context.Coupons
                    .ToListAsync();
            }

            var result = PaginatedList<Coupon>.Create(coupons, page, Page_Size);

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

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.CouponId == id);
            if(coupon == null)
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
                Data = coupon
            };
        }

        public async Task<StatusMessage> UpdateAsync(Coupon? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            } 

            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponId == entity.CouponId);
            if(coupon == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            coupon.CouponCode = entity.CouponCode;
            coupon.DiscountPercentage = entity.DiscountPercentage;
            coupon.DiscountValue = entity.DiscountValue;
            coupon.MinimumOrderAmount = entity.MinimumOrderAmount;
            coupon.MaximumDiscountAmount = entity.MaximumDiscountAmount;
            coupon.Quantity = entity.Quantity;
            coupon.PointAmount = entity.PointAmount;
            coupon.UserTypeId = entity.UserTypeId;
            coupon.StartAt = entity.StartAt;
            coupon.EndAt = entity.EndAt;

            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetByCodeAsync(string code)
        {
            if (code == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.CouponCode == code);

            if (coupon == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Code was not found"
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = coupon
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {

            var result = await _context.Coupons
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
