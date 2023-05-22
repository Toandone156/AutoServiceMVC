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
    public class ProductFeedbackRepository : ICommonRepository<ProductFeedback>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public ProductFeedbackRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(ProductFeedback? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<ProductFeedback>(entity);
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

            var productFeedback = await _context.ProductFeedbacks.AsNoTracking().FirstOrDefaultAsync(c => c.ProductFeedbackId == id);

            if(productFeedback == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.ProductFeedbacks.Remove(productFeedback);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic productFeedbacks = null;

            if (!String.IsNullOrEmpty(search))
            {
                productFeedbacks = await _context.ProductFeedbacks
                    .Include(p => p.Product)
                    .Include(p => p.User)
                    .AsNoTracking()
                    .Where(u => u.Comment.Contains(search))
                    .ToListAsync();
            }
            else
            {
                productFeedbacks = await _context.ProductFeedbacks
                    .Include(p => p.Product)
                    .Include(p => p.User)
                    .AsNoTracking()
                    .ToListAsync();
            }

            var result = PaginatedList<ProductFeedback>.Create(productFeedbacks, page, Page_Size);

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

            var productFeedback = await _context.ProductFeedbacks
                .Include(p => p.Product)
                .Include(p => p.User)
                .AsNoTracking()
                .AsNoTracking().FirstOrDefaultAsync(c => c.ProductFeedbackId == id);

            if(productFeedback == null)
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
                Data = productFeedback
            };
        }

        public async Task<StatusMessage> UpdateAsync(ProductFeedback? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var productFeedback = await _context.ProductFeedbacks.FirstOrDefaultAsync(c => c.ProductFeedbackId == entity.ProductFeedbackId);
            if(productFeedback == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            productFeedback.Comment = entity.Comment;
            productFeedback.Image = entity.Image;
            productFeedback.Rating = entity.Rating;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.ProductFeedbacks
                    .Include(p => p.Product)
                    .Include(p => p.User)
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
