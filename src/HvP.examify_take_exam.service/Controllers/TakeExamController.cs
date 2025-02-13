using Microsoft.AspNetCore.Mvc;
using HvP.examify_take_exam.DB.Constants.Errors;
using HvP.examify_take_exam.DB.Exceptions;
using HvP.examify_take_exam.service.Services;

namespace HvP.examify_take_exam.service.Controllers
{
    [ApiController]
    [Route("api/v1/take-exam")]
    public class TakeExamController : ControllerBase
    {
        public ILogger _logger;
        private TakeExamService _serviceImp;

        public TakeExamController(ILogger<TakeExamController> logger, TakeExamService service)
        {
            this._logger = logger;
            this._serviceImp = service;
        }

        [HttpGet]
        [Route("get")]
        public IActionResult Test()
        {
            try
            {
                long? num = null;
                var num2 = num.Value;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ErrorMsg.ErrBadRequest, ex);
            }

            throw new BadRequestException(ErrorMsg.ErrBadRequest);
        }
    }
}
