using AutoServiceBE.Models;
using AutoServiceMVC.Data;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Printing;

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

        public async Task<StatusMessage> Add(ServiceFeedback entity)
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

            var serviceFeedback = await _context.ServiceFeedbacks.FirstOrDefaultAsync(c => c.ServiceFeedbackId == id);

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
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAll(string? search, int page)
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

        public async Task<StatusMessage> Update(ServiceFeedback entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var serviceFeedback = await _context.ServiceFeedbacks.FirstOrDefaultAsync(c => c.ServiceFeedbackId == c.ServiceFeedbackId);
            if(serviceFeedback == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.ServiceFeedbacks.Update(entity);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }
    }
}
