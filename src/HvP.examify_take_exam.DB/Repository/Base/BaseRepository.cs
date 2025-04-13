using Microsoft.EntityFrameworkCore;
using HvP.Database.DBContexts;
using HvP.examify_take_exam.DB.Entities;
using HvP.examify_take_exam.DB.Repository.Cache;
using HvP.examify_take_exam.Common.Cache;
using HvP.examify_take_exam.Common.Exceptions;
using HvP.examify_take_exam.Common.Constants.Errors;

namespace HvP.examify_take_exam.DB.Repository.Base
{
    public class BaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly CommonDBContext _dbContext;
        private ICacheRepository _repositoryCache;

        public BaseRepository(CommonDBContext dbContext, ICache cache)
        {
            this._dbContext = dbContext;
            this._repositoryCache = new CacheRepository(cache);
        }

        public virtual string GetEntityName()
        {
            return nameof(TEntity);
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
        public virtual async Task<TEntity?> GetByIdAsync(long id, bool isCache = false, bool isTracking = false)
        {
            string funcName = nameof(GetByIdAsync);
            try
            {
                string cacheKey = $"{this.GetEntityName()}_{funcName}:{id}";
                TEntity? entity = null;
                if (!isTracking && isCache)
                {
                    entity = await this._repositoryCache.GetDataAsync<TEntity>(cacheKey, null);
                }

                if (entity == null)
                {
                    DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
                    IQueryable<TEntity> source = (isTracking ? dbSet.AsQueryable() : dbSet.AsNoTracking().AsQueryable());
                    entity = await source.Where((TEntity entity) => entity.DeletedFlag != true && entity.Id == id).FirstOrDefaultAsync();
                    if (entity != null)
                    {
                        this._repositoryCache.SetData<TEntity>(this.GetEntityName(), cacheKey, entity);
                    }
                }
                return entity;
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

                this._repositoryCache.ClearByEntity(GetEntityName());

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

                this._repositoryCache.ClearByEntity(GetEntityName());

                return entity;
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrQueryDatabase, ex.Message);
            }
        }
    }
}