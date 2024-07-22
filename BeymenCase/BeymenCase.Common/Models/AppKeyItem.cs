using System;
using System.Collections.Generic;
using System.Text;

namespace BaymenCase.Common.Models
{
	public class AppKeyItem
	{
		public Guid? ID { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
		public bool IsActive { get; set; }
		public string ApplicationName { get; set; }
	}
}
