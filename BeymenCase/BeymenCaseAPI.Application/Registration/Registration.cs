using BeymenCase.Common.Registration;
using BeymenCase.Redis.Registration;
using BeymenCaseAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Application.Registration
{
	public static class Registration
	{
		public static IServiceCollection ApplicationRegistration(this IServiceCollection services)
		{
			services.CommonRegistration().RedisRegistration();
			services.AddSingleton<IAppKeyService, AppKeyService>();
			return services;
		}
	}
}
