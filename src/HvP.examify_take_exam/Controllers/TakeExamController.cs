using Microsoft.AspNetCore.Mvc;
using HvP.examify_take_exam.Services;
using HvP.examify_take_exam.DB.Models;
using HvP.examify_take_exam.Common.Logger;
using HvP.examify_take_exam.Common.Exceptions;
using HvP.examify_take_exam.Common.Constants.Errors;
using HvP.examify_take_exam.Common.Extentions;

namespace HvP.examify_take_exam.Controllers
{
    [ApiController]
    [Route("api/v1/take-exam")]
    public class TakeExamController : ControllerBase
    {
        private readonly ILoggerService<TakeExamController> _logger;
        private ITakeExamService _serviceImp;

        public TakeExamController(ILoggerService<TakeExamController> logger, ITakeExamService service)
        {
            this._logger = logger;
            this._serviceImp = service;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get([FromQuery] long id)
        {
            // TODO: Add logger
            this._logger.LogInformation("Test Logger Controller");

            try
            {
                var rs = await this._serviceImp.GetData(id);
                return this.ResponseSuccess(rs);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ErrorMsg.ErrGetEnvConfig(""), ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] CreateTakeExamModel data)
        {
            this._logger.LogVerbose("Create Logger Controller");
            try
            {
                var rs = await this._serviceImp.CreateAsync(data);
                return this.ResponseSuccess(rs);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ErrorMsg.ErrGetEnvConfig(""), ex.Message);
            }
        }
    }
}
