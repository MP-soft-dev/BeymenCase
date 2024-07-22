using BaymenCase.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Application.Services
{
	public interface IAppKeyService
	{
		Task<IEnumerable<AppKeyItem>> GetAllKeysAsync(string appName = null);
		IEnumerable<AppKeyItem> GetAllKeys(string appName = null);
		
		Task<bool> AddOrUpdateKeyAsync(AppKeyItem keyItem, bool isNotExist);
		bool AddOrUpdateKey(AppKeyItem keyItem, bool isNotExist);
	


	}

}
