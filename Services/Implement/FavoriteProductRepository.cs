using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoServiceMVC.Services.Implement
{
    public class FavoriteProductRepository : ICommonRepository<FavoriteProduct>
    {
        private AppDbContext _context;

        public FavoriteProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StatusMessage> CreateAsync(FavoriteProduct? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<FavoriteProduct>(entity);
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

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.FavoriteProducts
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

        public async Task<StatusMessage> GetByIdAsync(int? id)
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

        public async Task<StatusMessage> UpdateAsync(FavoriteProduct? entity)
        {
            return new StatusMessage()
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        public async Task<StatusMessage> GetByConditions(Predicate<FavoriteProduct> conditions)
        {
            var result = _context.FavoriteProducts.ToList().Where(fp => conditions(fp));

            if (result.IsNullOrEmpty())
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

        public async Task<StatusMessage> DeleteByEntityAsync(FavoriteProduct entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            _context.FavoriteProducts.Remove(entity);
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }
    }
}
