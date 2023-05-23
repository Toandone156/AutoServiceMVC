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
    public class ServiceFeedbackRepository : ICommonRepository<ServiceFeedback>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public ServiceFeedbackRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(ServiceFeedback? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<ServiceFeedback>(entity);
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

            var serviceFeedback = await _context.ServiceFeedbacks.AsNoTracking().FirstOrDefaultAsync(c => c.ServiceFeedbackId == id);

            if(serviceFeedback == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.ServiceFeedbacks.Remove(serviceFeedback);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic serviceFeedbacks = null;

            if (!String.IsNullOrEmpty(search))
            {
                serviceFeedbacks = await _context.ServiceFeedbacks
                    .Include(p => p.User)
                    .AsNoTracking()
                    .Where(u => u.Comment.Contains(search))
                    .ToListAsync();
            }
            else
            {
                serviceFeedbacks = await _context.ServiceFeedbacks
                    .Include(p => p.User)
                    .AsNoTracking()
                    .ToListAsync();
            }

            var result = PaginatedList<ServiceFeedback>.Create(serviceFeedbacks, page, Page_Size);

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

            var serviceFeedback = await _context.ServiceFeedbacks
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ServiceFeedbackId == id);
            if(serviceFeedback == null)
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
                Data = serviceFeedback
            };
        }

        public async Task<StatusMessage> UpdateAsync(ServiceFeedback? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var serviceFeedback = await _context.ServiceFeedbacks.FirstOrDefaultAsync(c => c.ServiceFeedbackId == entity.ServiceFeedbackId);
            if(serviceFeedback == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            serviceFeedback.Comment = entity.Comment;
            serviceFeedback.Image = entity.Image;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.ServiceFeedbacks
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
