namespace AutoServiceMVC.Services
{
    public interface CommonRepository<T>
    {
        public List<T> GetAll();
        public T GetById(int id);
        public T Add(T entity);
        public T Update(T entity);
        public T DeleteById(int id);
    }
}
