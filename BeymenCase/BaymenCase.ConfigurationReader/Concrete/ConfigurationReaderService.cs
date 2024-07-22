using BaymenCase.Common.Models;
using BaymenCase.ConfigurationReader.Constant;
using BaymenCase.ConfigurationReader.Interfaces;
using BaymenCase.ConfigurationReader.Models;
using BaymenCase.Redis.interfaces;
using BaymenCase.Redis.Services;
using BeymenCase.Common.Serializer;
using BeymenCaseAPI.Application.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BaymenCase.ConfigurationReader.Concrete
{
	public class ConfigurationReaderService : IConfigurationReader, IDisposable
	{
		private readonly string _applicationName;
		private readonly int _refreshTimerIntervalInMs;
		private readonly IAppKeyService _appKeyService;
		private readonly IRedisService _redisService;
		private  ConcurrentBag<AppKeyCacheItem> _appKeys =new ConcurrentBag<AppKeyCacheItem>();
		private System.Timers.Timer _timer;
		public ConfigurationReaderService(string applicationName, int refreshTimerIntervalInMs, IAppKeyService appKeyService, IRedisService redisService)
		{
			_applicationName = applicationName;
			_refreshTimerIntervalInMs = refreshTimerIntervalInMs;
			_appKeyService = appKeyService;
			_redisService = redisService;
			_redisService.SubscribeToChannel("refreshallkeys", (key, message) => { if(message.Equals("all")) LoadAppKeys().ConfigureAwait(false).GetAwaiter().GetResult(); });
			LoadAppKeys().ConfigureAwait(false).GetAwaiter().GetResult();

			_timer = new System.Timers.Timer(double.Parse(_refreshTimerIntervalInMs.ToString()));
			_timer.AutoReset = true;
			_timer.Enabled = true;
			_timer.Elapsed += LoadAppKeysEvent;
			_timer.Start();
		}
		private async void LoadAppKeysEvent(Object source, ElapsedEventArgs e)
		{
			await LoadAppKeys();
		}
		private async Task LoadAppKeys()
		{
			_appKeys.Clear();
			var values = _appKeyService.GetAllKeys(_applicationName);
			foreach ( var value in values )
			{
				_appKeys.Add(new AppKeyCacheItem(_redisService, value.Name, _applicationName, value));
			}
			

		}
		public T GetValue<T>(string key)
		{
			var appKey = _appKeys.Where(x => x.Key == key).FirstOrDefault();
			if (appKey is null) throw new KeyNotFoundException("Key not found");//or return null
			return appKey.Get<T>();

		}

		public void Dispose()
		{
			_timer.Stop();
			_timer.Dispose();
		}
	}
}
