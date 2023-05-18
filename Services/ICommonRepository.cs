using AutoServiceMVC.Models.System;

namespace AutoServiceMVC.Services
{
    public interface ICommonRepository<T>
    {
        public Task<StatusMessage> GetAll(string? search, int page);
        public Task<StatusMessage> GetById(int id);
        public Task<StatusMessage> Add(T entity);
        public Task<StatusMessage> Update(T entity);
        public Task<StatusMessage> DeleteById(int id);
    }
}
