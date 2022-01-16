using System;
using Microsoft.AspNetCore.Mvc;

namespace _360o.Server.Web.Controllers.Home
{
    [Route("web/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public Task<OnInitializeResponse> OnInitializeAsync()
        {
            return Task.FromResult(new OnInitializeResponse());
        }
    }
}

