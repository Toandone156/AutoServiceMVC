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
    public class PointTradingRepository : ICommonRepository<PointTrading>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public PointTradingRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(PointTrading? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<PointTrading>(entity);
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

            var pointtrading = await _context.PointTrading.AsNoTracking().FirstOrDefaultAsync(c => c.PointTradingId == id);

            if(pointtrading == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.PointTrading.Remove(pointtrading);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic pointtradings = null;

            if (!String.IsNullOrEmpty(search))
            {
                pointtradings = await _context.PointTrading
                    .Include(p => p.User)
                    .AsNoTracking()
                    .Where(u => u.TradeDescription.Contains(search))
                    .ToListAsync();
            }
            else
            {
                pointtradings = await _context.PointTrading
                    .Include(p => p.User)
                    .AsNoTracking()
                    .ToListAsync();
            }

            var result = PaginatedList<PointTrading>.Create(pointtradings, page, Page_Size);

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

            var pointtrading = await _context.PointTrading
                .Include(p => p.User)
                .AsNoTracking()
                .AsNoTracking().FirstOrDefaultAsync(c => c.PointTradingId == id);
            if(pointtrading == null)
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
                Data = pointtrading
            };
        }

        public async Task<StatusMessage> UpdateAsync(PointTrading? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var pointtrading = await _context.PointTrading.FirstOrDefaultAsync(c => c.PointTradingId == entity.PointTradingId);
            if(pointtrading == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            pointtrading.TradeDescription = entity.TradeDescription;
            pointtrading.Point = entity.Point;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {

            var result = await _context.PointTrading
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
