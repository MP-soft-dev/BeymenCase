using BaymenCase.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Application.Repositories
{
	public interface IAppKeyRepository
	{
		Task<List<AppKeyItem>> GetAllAsync(string appName=null);
		Task<bool> AddOrUpdateKeyAsync(AppKeyItem addItem, bool isNotExist);

		

		IEnumerable<AppKeyItem> GetAll(string appName = null);
		bool AddOrUpdateKey(AppKeyItem addItem, bool isNotExist);

		


	}
}
