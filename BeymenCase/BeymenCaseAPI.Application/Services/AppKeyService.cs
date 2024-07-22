using BaymenCase.Common.Models;
using BaymenCase.Redis.interfaces;
using BeymenCaseAPI.Application.Repositories;
using BeymenCaseAPI.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCaseAPI.Application.Services
{
	internal class AppKeyService : IAppKeyService
	{
		private readonly IAppKeyRepository _appKeyRepository;
		private readonly IRedisService _redisService;

		public AppKeyService(IAppKeyRepository appKeyRepository, IRedisService redisService)
		{
			_appKeyRepository = appKeyRepository;
			_redisService = redisService;
		}

		public bool AddOrUpdateKey(AppKeyItem keyItem, bool isNotExist)
		{
			var result = _appKeyRepository.AddOrUpdateKey(keyItem, isNotExist);
			if (!result && isNotExist) throw new Exception($"Key already exist key:appKey_{keyItem.ApplicationName}_{keyItem.Name}");
			return result;

		}

		public async Task<bool> AddOrUpdateKeyAsync(AppKeyItem keyItem, bool isNotExist)
		{
			if (!isNotExist)
			{
				keyItem.ID = Guid.NewGuid();

			}
			var result = await _appKeyRepository.AddOrUpdateKeyAsync(keyItem, isNotExist);
			if (!result && isNotExist) throw new Exception($"Key already exist key:appKey_{keyItem.ApplicationName}_{keyItem.Name}");
			PublishChannelMessage($"appKey_{keyItem.ApplicationName}_{keyItem.Name}", isNotExist);
			return result;

		}

		private void PublishChannelMessage(string key, bool isNotExist)
		{
			if (isNotExist)
				_redisService.PublishMessageToChannel("refreshallkeys", "all");
			else
				_redisService.PublishMessageToChannel(key, "refresh");
		}

		public IEnumerable<AppKeyItem> GetAllKeys(string appName = null)
		=> _appKeyRepository.GetAll(appName);

		public async Task<IEnumerable<AppKeyItem>> GetAllKeysAsync(string appName = null)
		=> await _appKeyRepository.GetAllAsync(appName);
	}
}
