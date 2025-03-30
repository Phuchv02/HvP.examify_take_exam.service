using Microsoft.AspNetCore.Mvc;
using HvP.examify_take_exam.DB.Constants.Errors;
using HvP.examify_take_exam.DB.Exceptions;
using HvP.examify_take_exam.Services;
using HvP.examify_take_exam.DB.Extentions;
using HvP.examify_take_exam.DB.Models;

namespace HvP.examify_take_exam.Controllers
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
        public async Task<IActionResult> Get([FromQuery] long id)
        {
            // TODO: Add logger

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
