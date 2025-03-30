using HvP.Database.DBContexts;
using HvP.examify_take_exam.DB.Cache;
using HvP.examify_take_exam.DB.Entities;
using HvP.examify_take_exam.DB.Models;
using HvP.examify_take_exam.DB.Repository;

namespace HvP.examify_take_exam.Services
{
    public class TakeExamService : ITakeExamService
    {
        private CommonDBContext _dbContext;
        private ICache _cache;
        private TakeExamRepository _repositoryImp;

        public TakeExamService(CommonDBContext dbContext, ICache cache, ILogger<TakeExamService> logger)
        {
            this._dbContext = dbContext;
            this._cache = cache;
            this._repositoryImp = new TakeExamRepository(dbContext, cache);
        }

        public async Task<object> GetData(long id)
        {
            try
            {
                var rs = await this._repositoryImp.GetByIdAsync(id, true);

                return rs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> CreateAsync(CreateTakeExamModel objData)
        {
            try
            {
                TakeExamEntity entity = objData.PlainToEntity();

                var rs = await this._repositoryImp.InsertAsync(entity);

                return rs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
