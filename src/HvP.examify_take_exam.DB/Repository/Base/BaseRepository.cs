using Microsoft.EntityFrameworkCore;
using HvP.Database.DBContexts;
using HvP.examify_take_exam.DB.Constants.Errors;
using HvP.examify_take_exam.DB.Entities;
using HvP.examify_take_exam.DB.Exceptions;

namespace HvP.examify_take_exam.DB.Repository.Base
{
    public class BaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly CommonDBContext _dbContext;

        public BaseRepository(CommonDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public virtual string GetRepositoryName()
        {
            return nameof(BaseRepository<TEntity>);
        }

        /// <summary>
        /// GetByIdAsync method in BaseRepository
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="isTracking">Track queryable objects</param>
        /// <returns>
        /// A queryable Entity object.
        /// </returns>
        /// <exception cref="BaseException"></exception>
        public virtual async Task<TEntity?> GetByIdAsync(long id, bool isTracking = false)
        {
            try
            {
                DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
                IQueryable<TEntity> source = (isTracking ? dbSet.AsQueryable() : dbSet.AsNoTracking().AsQueryable());
                return await source.Where((TEntity entity) => entity.DeletedFlag != true && entity.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrQueryDatabase, ex.Message);
            }
        }

        /// <summary>
        /// InsertAsync method in BaseRepository
        /// </summary>
        /// <param name="id">The entity</param>
        /// <param name="userId">Id of the executing User</param>
        /// <returns>
        /// An Entity object is inserted.
        /// </returns>
        /// <exception cref="BaseException"></exception>
        public virtual async Task<TEntity> InsertAsync(TEntity entity, long? userId = null)
        {
            try
            {
                if (userId.HasValue)
                {
                    entity.CreatedBy = userId;
                }

                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = entity.CreatedAt;
                entity.DeletedFlag = false;
                entity = _dbContext.Set<TEntity>().Add(entity).Entity;
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrQueryDatabase, ex.Message);
            }
        }

        /// <summary>
        /// UpdateAsync method in BaseRepository
        /// </summary>
        /// <param name="id">The entity</param>
        /// <param name="userId">Id of the executing User</param>
        /// <returns>
        /// An Entity object is upserted.
        /// </returns>
        /// <exception cref="BaseException"></exception>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, long? userId = null)
        {
            try
            {
                if (userId.HasValue)
                {
                    entity.UpdatedBy = userId;
                }

                entity.UpdatedAt = DateTime.UtcNow;
                _dbContext.Set<TEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrQueryDatabase, ex.Message);
            }
        }
    }
}