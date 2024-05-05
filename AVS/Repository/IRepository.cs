namespace AVS.Repository
{
    public interface IRepository <T> 
    {

        Task<T?> GetById(Guid id);
        Task Add(T model);
        Task Update(T model);
        Task DeleteById(Guid id);
    }
}
