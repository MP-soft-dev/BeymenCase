using BeymenCase.Common.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCase.Common.Registration
{
	public static class Registration
	{
		public static IServiceCollection CommonRegistration(this IServiceCollection services)
		{
			services.AddSingleton<ISerializerService, NewtonsoftSerializerManager>();
			return services;
		}
	}
}
