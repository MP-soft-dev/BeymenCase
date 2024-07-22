using BaymenCase.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Infrastructer.Persistence.Helpers
{
	internal class AppKeyHelper
	{
		public static string GetStorageKeyByAppKeyItem(AppKeyItem item)
			=> $"appkey_{item.ApplicationName}_{item.Name}";

	}
}
