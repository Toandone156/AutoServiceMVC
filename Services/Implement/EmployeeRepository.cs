using AutoServiceMVC.Data;
using AutoServiceMVC.Models;
using AutoServiceMVC.Models.Constants;
using AutoServiceMVC.Models.System;
using AutoServiceMVC.Services.System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing.Printing;
using System.Security.Policy;
using static Humanizer.On;

namespace AutoServiceMVC.Services.Implement
{
    public class EmployeeRepository : IAuthenticateService<Employee>, ICommonRepository<Employee>
    {
        private readonly AppDbContext _context;
        private readonly int Page_Size;
        private readonly IHashPassword _hash;

        public EmployeeRepository(AppDbContext context, 
            IOptionsMonitor<AppSettings> monitor, 
            IHashPassword hash)
        {
            _context = context;
            Page_Size = monitor.CurrentValue.PageSize;
            _hash = hash;
        }

        public async Task<StatusMessage> GetAllAsync()
        {

            var result = await _context.Employees
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

        public async Task<StatusMessage> GetWithPaginatedAsync(string? search, int page)
        {
            dynamic employees = null;

            if (!String.IsNullOrEmpty(search))
            {
                employees = await _context.Employees
                    .Where(u => u.FullName.Contains(search))
                    .ToListAsync();
            }
            else
            {
                employees = await _context.Employees
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

        async Task<StatusMessage> ICommonRepository<Employee>.CreateAsync(Employee? entity)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        async Task<StatusMessage> ICommonRepository<Employee>.DeleteByIdAsync(int? id)
        {
            return new StatusMessage
            {
                IsSuccess = false,
                Message = Message.METHOD_NOT_DEFINED
            };
        }

        async Task<StatusMessage> ICommonRepository<Employee>.GetByIdAsync(int?  id)
        {
            var Employee = await _context.Employees
                    .SingleOrDefaultAsync(u => u.EmployeeId == id);

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

        async Task<StatusMessage> IAuthenticateService<Employee>.RegisterAsync(Register register)
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
                HashPassword = _hash.GetHashPassword(register.Password),
                FullName = register.FullName,
                Email = register.Email,
                RoleId = register.RoleId ?? 2
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

        async Task<StatusMessage> ICommonRepository<Employee>.UpdateAsync(Employee? entity)
        {
            if (entity == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.INPUT_EMPTY
                };
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(u => u.EmployeeId == entity.EmployeeId);

            if(employee == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = Message.ID_NOT_FOUND
                };
            }

            employee.FullName = entity.FullName;
            employee.Email = entity.Email;
            employee.RoleId = entity.RoleId;
            employee.EndDate = entity.EndDate;
            employee.HashPassword = entity.HashPassword;

            await _context.SaveChangesAsync();
            return new StatusMessage()
            {
                IsSuccess = true,
                Message = Message.UPDATE_SUCCESS,
                Data = entity
            };
        }

        async Task<StatusMessage> IAuthenticateService<Employee>.ValidateLoginAsync(Login login)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.Username == login.Username &&
                                        u.HashPassword == _hash.GetHashPassword(login.Password));
            if (employee == null)
            {
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Username or password is wrong"
                };
            }
            
            if (employee.EndDate != null && employee.EndDate < DateTime.Now)
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
        public async Task<StatusMessage> CheckEmailAndUsername(Employee? entity)
        {
            if (_context.Employees.Any(x => (x.Email == entity.Email) || (x.Username == entity.Username))){
                return new StatusMessage()
                {
                    IsSuccess = false,
                    Message = "Success",
                    Data = true
                };
            }

            return new StatusMessage()
            {
                IsSuccess = false,
                Message = "Success",
                Data = false
            };
        }

        public async Task<StatusMessage> CheckEmailAndUsernameAsync(string? email, string? username)
        {
            if (_context.Employees.Any(x => (x.Email == email) || (x.Username == username)))
            {
                return new StatusMessage()
                {
                    IsSuccess = true,
                    Message = "Success",
                    Data = true
                };
            }

            return new StatusMessage()
            {
                IsSuccess = true,
                Message = "Success",
                Data = false
            };
        }
    }
}
