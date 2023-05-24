using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Humanizer.On;

namespace AutoServiceMVC.Services.Implement
{
    public class TableRepository : ICommonRepository<Table>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public TableRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor) 
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(Table? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<Table>(entity);
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

            var table = await _context.Tables.AsNoTracking().FirstOrDefaultAsync(c => c.TableId == id);

            if(table == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic tables = null;

            if (!String.IsNullOrEmpty(search))
            {
                tables = await _context.Tables
                    .AsNoTracking()
                    .Where(u => u.TableName.Contains(search))
                    .ToListAsync();
            }
            else
            {
                tables = await _context.Tables
                    .AsNoTracking()
                    .ToListAsync();
            }

            var result = PaginatedList<Table>.Create(tables, page, Page_Size);

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

            var table = await _context.Tables
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.TableId == id);
            if(table == null)
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
                Data = table
            };
        }

        public async Task<StatusMessage> UpdateAsync(Table? entity)
        {
            if(entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var table = await _context.Tables.FirstOrDefaultAsync(c => c.TableId == entity.TableId);
            if(table == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            table.TableName = entity.TableName;
            table.TableCode = entity.TableCode;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.Tables
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
