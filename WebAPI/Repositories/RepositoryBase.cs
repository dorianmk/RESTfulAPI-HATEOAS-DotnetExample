using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebAPI.Interfaces;

namespace WebAPI.Repositories
{
    public abstract class RepositoryBase<TId, TEntity> : IRepository<TId, TEntity, ResultType>
        where TEntity : class, IIdentifiable<TId>
        where TId : struct
    {
        protected readonly DbContext _dbContext;

        protected RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetById(TId id)
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<ResultType> Create(TEntity entity)
        {
            var existingEntity = await GetById(entity.Id);

            if (existingEntity != null)
                return ResultType.AlreadyExists;

            Validate(entity);
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return ResultType.Ok;
        }

        public async Task<ResultType> Delete(TId id)
        {
            var existingEntity = await GetById(id);

            if (existingEntity == null)
                return ResultType.NotFound;

            _dbContext.Set<TEntity>().Remove(existingEntity);
            await _dbContext.SaveChangesAsync();

            return ResultType.Ok;
        }

        public async Task<ResultType> Update(TEntity entity)
        {
            var existingEntity = await GetById(entity.Id);

            if (existingEntity == null)
                return ResultType.NotFound;

            Validate(entity);
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();

            return ResultType.Ok;
        }

        private void Validate(TEntity entity)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(entity, null, null);

            if (Validator.TryValidateObject(entity, validationContext, results, true) == false)
            {
                var aggregatedExceptions = new AggregateException(results.Select(x => new ValidationException(x.ErrorMessage)));
                throw new ValidationException(string.Empty, aggregatedExceptions);
            }
        }
    }
}
