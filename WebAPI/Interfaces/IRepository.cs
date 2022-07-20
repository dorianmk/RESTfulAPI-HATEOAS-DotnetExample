namespace WebAPI.Interfaces
{

    public interface IRepository<TId, TEntity, TResult>
        where TEntity : IIdentifiable<TId>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> GetById(TId id);
        Task<TResult> Create(TEntity entity);
        Task<TResult> Delete(TId id);
        Task<TResult> Update(TEntity entity);

    }
}
