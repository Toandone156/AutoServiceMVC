﻿using AutoServiceBE.Models;
using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Printing;

namespace AutoServiceMVC.Services.Implement
{
    public class EmployeeRepository : IAuthenticateService<Employee>, ICommonRepository<Employee>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;

        public EmployeeRepository(AppDbContext context, IOptionsMonitor<AppSettings> monitor)
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
        }

        public async Task<StatusMessage> GetAll(string? search, int page)
        {
            dynamic employees = null;

            if (!String.IsNullOrEmpty(search))
            {
                employees = await _context.Employees
                    .Include(e => e.Role)
                    .AsNoTracking()
                    .Where(u => u.FullName.Contains(search))
                    .ToListAsync();
            }
            else
            {
                employees = await _context.Employees
                    .Include(e => e.Role)
                    .AsNoTracking()
                    .ToListAsync();
            }

            var result = PaginatedList<Employee>.Create(employees, page, Page_Size);

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

        async Task<StatusMessage> ICommonRepository<Employee>.Add(Employee entity)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        async Task<StatusMessage> ICommonRepository<Employee>.DeleteById(int id)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        async Task<StatusMessage> ICommonRepository<Employee>.GetById(int id)
        {
            var Employee = await _context.Employees
                    .Include(e => e.Role)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.EmployeeID == id);

            if (Employee == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND,
                    Data = null
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.GET_SUCCESS,
                Data = Employee
            };
        }

        async Task<StatusMessage> IAuthenticateService<Employee>.Register(Register register)
        {
            if (register.Password != register.AgainPassword)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Password was not match"
                };
            }

            var employee = new Employee()
            {
                Username = register.Username,
                HashPassword = register.Password,
                FullName = register.FullName,
                Email = register.Email,
                PhoneNum = register.PhoneNum,
                RoleID = register.Role?.RoleId ?? 0
            };

            await _context.AddAsync<Employee>(employee);
            await _context.SaveChangesAsync();
            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Register success",
                Data = employee
            };
        }

        async Task<StatusMessage> ICommonRepository<Employee>.Update(Employee entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var Employee = _context.Employees.FirstOrDefaultAsync(u => u.EmployeeID == u.EmployeeID);
            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();
            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS,
                Data = Employee
            };
        }

        async Task<StatusMessage> IAuthenticateService<Employee>.ValidateLogin(Login login)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(u => u.Username == login.Username);
            if (employee == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Username is wrong"
                };
            }
            
            if (employee.HashPassword != login.Password)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Password is wrong"
                };
            }
            
            if ((employee.EndDate ?? DateTime.Now) <= DateTime.Now)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Employee is not work from now"
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Employeename and Password is valid",
                Data = employee
            };
        }
    }
}
