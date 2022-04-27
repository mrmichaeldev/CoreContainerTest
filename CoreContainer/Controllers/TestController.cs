using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using CoreContainer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreContainer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> log;
        private readonly ITestBusiness testBusiness;

        public TestController(ILogger<TestController> log, ITestBusiness testBusiness)
        {
            this.testBusiness = testBusiness;
            this.log = log;
        }

        public Result Transform(long id)
        {
            try
            {
                testBusiness.BusinessTransform(id, HttpContext.RequestAborted);
                return new Result().WithSuccess();
            }
            catch(Exception exc)
            {
                var message = "Could not perform transform action";
                log.LogError(exc, message);

                return new Result().WithError(message);
            }
        }
    }
}
