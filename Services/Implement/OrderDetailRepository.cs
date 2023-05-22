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
    public class OrderDetailRepository : ICommonRepository<OrderDetail>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public OrderDetailRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(OrderDetail? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<OrderDetail>(entity);
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
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> GetByIdAsync(int? id)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> UpdateAsync(OrderDetail? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(
                od => od.OrderId == entity.OrderId && od.ProductId == entity.ProductId);

            if (orderDetail == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            orderDetail.Quantity = entity.Quantity;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetByOrderIdAsync(int? orderId)
        {
            if (orderId == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var orderDetails = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .AsNoTracking()
                .Where(c => c.OrderId == orderId)
                .ToListAsync();

            if (orderDetails == null)
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
                Data = orderDetails
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.OrderDetails
                    .Include(o => o.Order)
                    .Include(o => o.Product)
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

        public async Task<StatusMessage> DeleteByEntityAsync(OrderDetail entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            _context.OrderDetails.Remove(entity);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }
    }
}
