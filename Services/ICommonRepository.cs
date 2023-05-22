using AutoServiceMVC.Models.System;

namespace AutoServiceMVC.Services
{
    public interface ICommonRepository<T>
    {
        public Task<StatusMessage> GetAllAsync();
        public Task<StatusMessage> GetWithPaginatedAsync(string? search, int page);
        public Task<StatusMessage> GetByIdAsync(int? id);
        public Task<StatusMessage> CreateAsync(T? entity);
        public Task<StatusMessage> UpdateAsync(T? entity);
        public Task<StatusMessage> DeleteByIdAsync(int? id);
    }
}
