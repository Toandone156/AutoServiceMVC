using AutoServiceBE.Models;
using AutoServiceMVC.Data;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Printing;

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

        public async Task<StatusMessage> Add(Coupon entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
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

        public async Task<StatusMessage> DeleteById(int id)
        {
            if (id == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponID == id);

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
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAll(string? search, int page)
        {
            dynamic coupons = null;

            if (!String.IsNullOrEmpty(search))
            {
                coupons = await _context.Coupons
                    .Include(c => c.Creator)
                    .Include(c => c.UserType)
                    .AsNoTracking()
                    .Where(u => u.CouponCode.Contains(search))
                    .ToListAsync();
            }
            else
            {
                coupons = await _context.Coupons
                    .Include(c => c.Creator)
                    .Include(c => c.UserType)
                    .AsNoTracking()
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

        public async Task<StatusMessage> GetById(int id)
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
                .Include(c => c.Creator)
                .Include(c => c.UserType)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CouponID == id);
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

        public async Task<StatusMessage> Update(Coupon entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            } 

            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponID == entity.CouponID);
            if(coupon == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.Coupons.Update(entity);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetByCode(string code)
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
                .Include(c => c.Creator)
                .Include(c => c.UserType)
                .AsNoTracking()
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
    }
}
