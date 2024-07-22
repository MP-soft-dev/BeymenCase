using System;
using System.Collections.Generic;
using System.Text;

namespace BaymenCase.Redis.Models
{
	public class CacheServiceConfiguration
	{
		public string CacheConfiguration { get; set; }
		public bool IsCacheActive { get; set; } = true;
	}
}
