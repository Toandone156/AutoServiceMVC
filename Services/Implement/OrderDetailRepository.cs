using AutoServiceBE.Models;
using AutoServiceMVC.Data;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
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

        public async Task<StatusMessage> Add(OrderDetail entity)
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
            dynamic orderDetails =  await _context.OrderDetails
                    .Include(o => o.Order)
                    .Include(o => o.Product)
                    .AsNoTracking()
                    .ToListAsync();

            var result = PaginatedList<OrderDetail>.Create(orderDetails, page, Page_Size);

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

        public async Task<StatusMessage> Update(OrderDetail entity)
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
                od => od.OrderID == entity.OrderID && od.ProductID == entity.ProductID);

            if (orderDetail == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.Update<OrderDetail>(entity);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetByOrderID(int orderID)
        {
            if (orderID == null)
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
                .Where(c => c.OrderID == orderID)
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
    }
}
