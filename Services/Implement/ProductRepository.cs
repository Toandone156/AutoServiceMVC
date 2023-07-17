﻿using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Printing;
using System.Linq.Expressions;
using static Humanizer.On;

namespace AutoServiceMVC.Services.Implement
{
    public class ProductRepository : ICommonRepository<Product>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public ProductRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor)
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> CreateAsync(Product? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            await _context.AddAsync<Product>(entity);
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

            var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == id);

            if (product == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            product.IsAvailable = false;
            await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.DELETE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic products = null;

            if (!String.IsNullOrEmpty(search))
            {
                products = await _context.Products
                    .Where(u => u.ProductName.Contains(search))
                    .ToListAsync();
            }
            else
            {
                products = await _context.Products
                    .ToListAsync();
            }

            var result = PaginatedList<Product>.Create(products, page, Page_Size);

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

            var product = await _context.Products
                .FirstOrDefaultAsync(c => c.ProductId == id);

            if (product == null)
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
                Data = product
            };
        }

        public async Task<StatusMessage> UpdateAsync(Product? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == entity.ProductId && c.IsAvailable);

            if (product == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            product.Price = entity.Price;
            product.ProductName = entity.ProductName;
            product.ProductDescription = entity.ProductDescription;
            product.ProductImage = entity.ProductImage;
            product.IsAvailable = entity.IsAvailable;
            product.IsInStock = entity.IsInStock;
            product.CategoryId = entity.CategoryId;

			await _context.SaveChangesAsync();

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS
            };
        }

        public async Task<StatusMessage> GetAllAsync()
        {
            var result = await _context.Products
                    .Where(p => p.IsAvailable)
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

        public async Task<StatusMessage> GetByCondition(Expression<Func<Product, bool>> condition)
        {
            var result = await _context.Products
                    .Where(condition)
                    .Where(p => p.IsAvailable)
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
