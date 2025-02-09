using HvP.examify_take_exam.DB.Constants.Errors;
using HvP.examify_take_exam.DB.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HvP.examify_take_exam.service.Controllers
{
    [ApiController]
    [Route("api/v1/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            //throw new Exception("Vi du ex");
            throw new BadRequestException(ErrorMsg.ErrBadRequest);
        }
    }
}
