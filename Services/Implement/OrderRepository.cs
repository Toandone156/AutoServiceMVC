using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AutoServiceMVC.Services.Implement
{
    public class OrderRepository : ICommonRepository<Order>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public OrderRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor)
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(Order? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<Order>(entity);
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

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();


            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic orders = await _context.Orders
                
                .ToListAsync();

            var result = PaginatedList<Order>.Create(orders, page, Page_Size);

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
            if (id == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(c => c.OrderId == id);

            if (order == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            order.Status = await GetRecentOrderStatus(order.OrderId);

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = order
            };
        }

        public async Task<StatusMessage> GetByUserIdAsync(int? userId)
        {
            if (userId == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();

            foreach (var order in orders)
            {
                order.Status = await GetRecentOrderStatus(order.OrderId);
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = orders
            };
        }

        public async Task<StatusMessage> UpdateAsync(Order? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderId == entity.OrderId);

            if (order == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            //order.Note = entity.Note;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.Orders
                .ToListAsync();

            foreach(var order in result)
            {
                order.Status = await GetRecentOrderStatus(order.OrderId);
            }

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

        async Task<Status?> GetRecentOrderStatus(int orderId)
        {
            var recentStatus = await _context.OrderStatus
                .Where(s => s.OrderId == orderId)
                .OrderByDescending(s => s.StatusId)
                .FirstOrDefaultAsync();

            return recentStatus?.Status;
        }
    }
}
