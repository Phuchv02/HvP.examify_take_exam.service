using HvP.Database.DBContexts;
using HvP.examify_take_exam.Common.Cache;
using HvP.examify_take_exam.DB.Entities;
using HvP.examify_take_exam.DB.Repository.Base;

namespace HvP.examify_take_exam.DB.Repository
{
    public class TakeExamRepository : BaseRepository<TakeExamEntity>
    {
        public TakeExamRepository(CommonDBContext dBContext, ICache cache) : base(dBContext, cache) { }

        public override string GetEntityName()
        {
            return nameof(TakeExamEntity);
        }
    }
}
