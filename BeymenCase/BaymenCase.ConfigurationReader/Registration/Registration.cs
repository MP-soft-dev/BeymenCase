using BaymenCase.ConfigurationReader.Concrete;
using BaymenCase.ConfigurationReader.Interfaces;
using BaymenCase.Redis.interfaces;
using BeymenCaseAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCase.ConfigurationReader.Registration
{
	public static class Registration
	{
		public static IServiceCollection ConfigurationReaderRegistration(this IServiceCollection services)
		{
			var appName = ConfigurationManager.AppSettings["AppName"]?.ToString() ?? throw new SettingsPropertyNotFoundException("Appname not found");
			var refreshPeriodMS = ConfigurationManager.AppSettings["AppKeysRefreshPeriodMS"]?.ToString() ?? throw new SettingsPropertyNotFoundException("AppKeysRefreshPeriodMS not found");
			services.AddSingleton<IConfigurationReader>(s=>new ConfigurationReaderService(appName,int.Parse(refreshPeriodMS),s.GetRequiredService<IAppKeyService>(),s.GetRequiredService<IRedisService>()));
			return services;
		}
	}
}
