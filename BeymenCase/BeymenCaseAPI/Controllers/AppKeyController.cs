using BaymenCase.Common.Models;
using BaymenCase.ConfigurationReader.Interfaces;
using BeymenCaseAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class AppKeyController : ControllerBase
	{

		private readonly ILogger<AppKeyController> _logger;
		private readonly IAppKeyService _appKeyService;
		private readonly IConfigurationReader _configurationReader;

		public AppKeyController(ILogger<AppKeyController> logger, IAppKeyService appKeyService, IConfigurationReader configurationReader)
		{
			_logger = logger;
			_appKeyService = appKeyService;
			_configurationReader = configurationReader;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var result = _appKeyService.GetAllKeys();
			return result is null || result.Count() == 0 ? NotFound() : Ok(result);
		}
		[HttpPost]
		public IActionResult GetKeyFromConfigurationReader(string key)
		{
			return Ok(_configurationReader.GetValue<string>(key));
		}
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AppKeyItem appKeyItem)
		{
			var result = false;

			if (appKeyItem == null) throw new ArgumentNullException(nameof(appKeyItem));

			result = await _appKeyService.AddOrUpdateKeyAsync(appKeyItem, true);

			return result ? Ok(appKeyItem) : BadRequest();


		}
		[HttpPut()]
		public async Task<IActionResult> Update([FromBody] AppKeyItem appKeyItem)
		{
			var result = await _appKeyService.AddOrUpdateKeyAsync(appKeyItem, false);

			return result ? Ok(appKeyItem) : BadRequest();
		}
	}
}
