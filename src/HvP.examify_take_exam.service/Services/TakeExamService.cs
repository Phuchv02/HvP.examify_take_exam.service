using HvP.Database.DBContexts;
using HvP.examify_take_exam.DB.Repository;

namespace HvP.examify_take_exam.service.Services
{
    public class TakeExamService
    {
        private CommonDBContext _dbContext;
        private TakeExamRepository _takeExamRepository;

        public TakeExamService(CommonDBContext dbContext, ILogger<TakeExamService> logger)
        {
            this._dbContext = dbContext;
            this._takeExamRepository = new TakeExamRepository(dbContext);
        }

    }
}
