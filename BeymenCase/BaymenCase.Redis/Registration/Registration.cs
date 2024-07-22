using BaymenCase.Redis.interfaces;
using BaymenCase.Redis.Services;
using BeymenCase.Common.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCase.Redis.Registration
{
	public static class Registration
	{
		public static IServiceCollection RedisRegistration(this IServiceCollection services)
		{
			services.AddSingleton<IRedisService>(s=>new RedisCacheService(new BaymenCase.Redis.Models.CacheServiceConfiguration
			{
				CacheConfiguration = ConfigurationManager.AppSettings["RedisConnStr"]?.ToString(),
				IsCacheActive = true
			},
			s.GetRequiredService<ISerializerService>()));
			return services;
		}
	}
}
