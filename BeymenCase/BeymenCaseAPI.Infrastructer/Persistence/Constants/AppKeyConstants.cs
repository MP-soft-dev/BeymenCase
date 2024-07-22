using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Infrastructer.Persistence.Constants
{
	internal class AppKeyConstants
	{
		public static TimeSpan AppKeyExpired = TimeSpan.MaxValue;
		public const string AppKeyPrefix = "appkey";
	}
}
