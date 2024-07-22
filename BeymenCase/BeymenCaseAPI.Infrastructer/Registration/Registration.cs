using BeymenCaseAPI.Application.Repositories;
using BeymenCaseAPI.Infrastructer.Persistence.Repositories.AppKey;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Infrastructer.Registration
{
	public static class Registration
	{
		public static IServiceCollection InfraRegistration(this IServiceCollection services)
		{
			services.AddSingleton<IAppKeyRepository, AppKeyRepository>();
			return services;
		}
	}
}
