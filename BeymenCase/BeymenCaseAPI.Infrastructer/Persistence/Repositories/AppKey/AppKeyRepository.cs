using BaymenCase.Common.Models;
using BaymenCase.Redis.interfaces;
using BeymenCaseAPI.Application.Repositories;
using BeymenCaseAPI.Infrastructer.Persistence.Constants;
using BeymenCaseAPI.Infrastructer.Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Infrastructer.Persistence.Repositories.AppKey
{
	public class AppKeyRepository : IAppKeyRepository
	{
		private readonly IRedisService _redisService;

		public AppKeyRepository(IRedisService redisService)
		{
			_redisService = redisService;
		}

		public bool AddOrUpdateKey(AppKeyItem addItem, bool isNotExist)
		{
			string key = AppKeyHelper.GetStorageKeyByAppKeyItem(addItem);
			if (isNotExist)
				return _redisService.AddIfNotExist(key, addItem, AppKeyConstants.AppKeyExpired);
			return _redisService.AddOrUpdate(key, addItem, AppKeyConstants.AppKeyExpired);
		}

		public async Task<bool> AddOrUpdateKeyAsync(AppKeyItem addItem, bool isNotExist)
		{
			string key = AppKeyHelper.GetStorageKeyByAppKeyItem(addItem);
			if (isNotExist)
				return await _redisService.AddIfNotExistAsync(key, addItem, AppKeyConstants.AppKeyExpired);
			return await _redisService.AddOrUpdateAsync(key, addItem, AppKeyConstants.AppKeyExpired);
		}



		public IEnumerable<AppKeyItem> GetAll(string appName = null)
		{
			var keys = _redisService.GetKeyListByPattern($"{AppKeyConstants.AppKeyPrefix}{(appName is null ? "" : $"_{appName}")}*");
			List<AppKeyItem> result = new List<AppKeyItem>();
			if ((!keys?.Any() ?? true))
				throw new KeyNotFoundException("app keys not found");


			result.AddRange(
				keys.Select(key =>
					  _redisService.RetrieveItemFromCache<AppKeyItem>(key)
				));
			return result;
		}

		public Task<List<AppKeyItem>> GetAllAsync(string appName = null)
		{
			var keys = _redisService.GetKeyListByPattern($"{AppKeyConstants.AppKeyPrefix}{(appName is null ? "" : $"_{appName}")}*");
			List<AppKeyItem> result = new List<AppKeyItem>();
			if (keys is null || keys.Count == 0)
				return Task.FromResult(result);



			keys.ForEach(async key =>
			{
				var val = await _redisService.RetrieveItemFromCacheAsync<AppKeyItem>(key);
				result.Add(val);
			});
			
			return Task.FromResult(result);
		}
	}
}
