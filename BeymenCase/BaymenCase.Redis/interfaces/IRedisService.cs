using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BaymenCase.Redis.interfaces
{
	public interface IRedisService
	{
		bool IsCacheActive { get; }
		bool AddOrUpdate(string key, object item, TimeSpan? cacheTime);
		bool AddIfNotExist(string key, object item, TimeSpan? cacheTime);
		Task<bool> AddOrUpdateAsync(string key, object item, TimeSpan? cacheTime);
		Task<bool> AddIfNotExistAsync(string key, object item, TimeSpan? cacheTime);
		void CacheString(string key, string item, TimeSpan? cacheTime);
		T RetrieveItemFromCache<T>(string key);
		Task <T> RetrieveItemFromCacheAsync<T>(string key);
		bool ControlItem(string key);
		void PublishMessageToChannel(string channelName, string message);

		void SubscribeToChannel(string channelName, Action<string, string> messageAction);
		List<string> GetKeyListByPattern(string pattern);
	}
}
