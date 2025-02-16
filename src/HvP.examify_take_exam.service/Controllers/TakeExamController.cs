using Microsoft.AspNetCore.Mvc;
using HvP.examify_take_exam.DB.Constants.Errors;
using HvP.examify_take_exam.DB.Exceptions;
using HvP.examify_take_exam.service.Services;
using HvP.examify_take_exam.DB.Extentions;

namespace HvP.examify_take_exam.service.Controllers
{
    [ApiController]
    [Route("api/v1/take-exam")]
    public class TakeExamController : ControllerBase
    {
        public ILogger _logger;
        private ITakeExamService _serviceImp;

        public TakeExamController(ILogger<TakeExamController> logger, ITakeExamService service)
        {
            this._logger = logger;
            this._serviceImp = service;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Test()
        {
            var rs = await this._serviceImp.GetData();

            return this.ResponseSuccess(rs);
        }
    }
}
