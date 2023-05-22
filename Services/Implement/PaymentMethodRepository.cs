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
    public class PaymentMethodRepository : ICommonRepository<PaymentMethod>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public PaymentMethodRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(PaymentMethod? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<PaymentMethod>(entity);
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

            var paymentMethod = await _context.PaymentMethods.AsNoTracking().FirstOrDefaultAsync(c => c.PaymentMethodId == id);

            if(paymentMethod == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic paymentMethods = null;

            if (!String.IsNullOrEmpty(search))
            {
                paymentMethods = await _context.PaymentMethods
                    .AsNoTracking()
                    .Where(u => u.PaymentMethodName.Contains(search))
                    .ToListAsync();
            }
            else
            {
                paymentMethods = await _context.PaymentMethods
                    .AsNoTracking()
                    .ToListAsync();
            }

            var result = PaginatedList<PaymentMethod>.Create(paymentMethods, page, Page_Size);

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

            var paymentMethod = await _context.PaymentMethods
                .AsNoTracking()
                .AsNoTracking().FirstOrDefaultAsync(c => c.PaymentMethodId == id);
            if(paymentMethod == null)
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
                Data = paymentMethod
            };
        }

        public async Task<StatusMessage> UpdateAsync(PaymentMethod? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var paymentMethod = await _context.PaymentMethods.FirstOrDefaultAsync(c => c.PaymentMethodId == entity.PaymentMethodId);
            if(paymentMethod == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            paymentMethod.PaymentMethodName = entity.PaymentMethodName;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {

            var result = await _context.PaymentMethods
                .AsNoTracking()
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
