using System;
using Microsoft.AspNetCore.Mvc;

namespace _360o.Server.Web.Controllers.Home
{
	[Route("web/[controller]")]
	[ApiController]
	public class HomeController : ControllerBase
	{
		[HttpGet]
		public async Task<OnInitializeResponse> OnInitializeAsync()
        {
			return new OnInitializeResponse();
        }
	}
}

