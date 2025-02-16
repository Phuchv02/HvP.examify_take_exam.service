using HvP.Database.DBContexts;
using HvP.examify_take_exam.DB.Repository;

namespace HvP.examify_take_exam.Services
{
    public class TakeExamService : ITakeExamService
    {
        private CommonDBContext _dbContext;
        private TakeExamRepository _repositoryImp;

        public TakeExamService(CommonDBContext dbContext, ILogger<TakeExamService> logger)
        {
            this._dbContext = dbContext;
            this._repositoryImp = new TakeExamRepository(dbContext);
        }

        public async Task<object> GetData()
        {
            try
            {
                var rs = await this._repositoryImp.GetByIdAsync(1);

                var x = 2;
                return rs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
