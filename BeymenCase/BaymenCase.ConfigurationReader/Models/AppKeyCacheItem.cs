using BaymenCase.ConfigurationReader.Constant;
using BaymenCase.Redis.interfaces;
using BaymenCase.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaymenCase.ConfigurationReader.Models
{
	internal class AppKeyCacheItem
	{
		private AppKeyItem _data = null;
		private readonly IRedisService _cacheService;
		private readonly string _cacheKey;
		public readonly string Key;
		private readonly string _seperator = ConfigurationReaderConstant.CacheSeperator;

		public AppKeyCacheItem(IRedisService cacheService, string cacheKey, string appName,AppKeyItem data)
		{
			_cacheService = cacheService;
			_cacheKey = $"{_seperator}_{appName}_{cacheKey}";
			Key = cacheKey;
			_data = data;
			cacheService.SubscribeToChannel(_cacheKey, new Action<string, string>((key, message) => { if (message == "refresh") _data = GetValueFromCache(); }));
		}
		protected AppKeyItem GetValueFromCache()
		{
			try
			{
				if (!_cacheService.IsCacheActive)
					return _data;

				var tmpData = _cacheService.RetrieveItemFromCache<AppKeyItem>(_cacheKey);
				if (tmpData == null)
				{
					return _data;
				}

				return tmpData;
			}
			catch (Exception ex)
			{
				//log
				return _data;
			}
		}
		public T Get<T>()
		{

			if ( _data is null)
				_data = GetValueFromCache();
			
			if (!_data.IsActive) throw new Exception("key is not active");//or return null
			return (T)Convert.ChangeType(_data.Value, typeof(T));

		}
		public AppKeyItem Refresh()
		{

			if (_cacheService.IsCacheActive)
			{
				try
				{
					this._data = GetValueFromCache();
					
				}
				catch (Exception ex)
				{
					//log
				}
				
			}

			return _data;
		}
		public void TriggerRefreshFromMessageService()
		{
			Refresh();

			try
			{
				_cacheService.PublishMessageToChannel(_cacheKey, "refresh");
			}
			catch (Exception ex)
			{
				//log
			}
		}

	}
}
