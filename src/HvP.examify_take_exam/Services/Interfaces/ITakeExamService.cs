using HvP.examify_take_exam.DB.Models;

namespace HvP.examify_take_exam.Services
{
    public interface ITakeExamService
    {
        public Task<object> GetData(long id);

        public Task<object> CreateAsync(CreateTakeExamModel objData);
    }
}
