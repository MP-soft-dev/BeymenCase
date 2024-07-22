using BaymenCase.Redis.interfaces;
using BaymenCase.Redis.Models;
using BeymenCase.Common.Serializer;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaymenCase.Redis.Services
{
	public class RedisCacheService : IRedisService
	{
		private readonly CacheServiceConfiguration _configurationOptions;
		private readonly ISerializerService _serializerService;

		public RedisCacheService(CacheServiceConfiguration configurationOptions, ISerializerService serializerService)
		{
			_configurationOptions = configurationOptions;
			_serializerService = serializerService;
		}


		private ConnectionMultiplexer _lazyConnection;
		private bool _initCompleted = false;

		public event EventHandler<ConnectionFailedEventArgs> ConnectionFailed;
		public event EventHandler<ConnectionFailedEventArgs> ConnectionRestored;

		public bool IsCacheActive =>
			_configurationOptions != null && (_initCompleted ? _lazyConnection != null ? _lazyConnection.IsConnected : false : true);

		private ConnectionMultiplexer _TryConnect()
		{
			try
			{
				var connection = ConnectionMultiplexer.Connect(_configurationOptions.CacheConfiguration);

				if (connection != null)
				{
					connection.ConnectionFailed += ConnectionFailed;

					connection.ConnectionRestored += ConnectionRestored;
				}

				_initCompleted = true;

				return connection;
			}
			catch (Exception ex)
			{
				_configurationOptions.IsCacheActive = false;

				return null;
			}
		}
		private ConnectionMultiplexer _Connection
		{
			get
			{
				try
				{
					if (_lazyConnection != null && _lazyConnection.IsConnected)
						return _lazyConnection;
					else
					{
						_lazyConnection = _TryConnect();
						return _lazyConnection;
					}
				}
				catch (Exception ex)
				{

					return null;
				}

			}
		}
		private IDatabase _Cache
		{
			get
			{
				return _Connection.GetDatabase(0);
			}
		}

		public bool AddOrUpdate(string key, object item, TimeSpan? cacheTime)
		{
			if (IsCacheActive)
				return _Cache.StringSet(key, _serializerService.SerializeItem(item), cacheTime);
			throw new Exception("Redis is not active");
		}
		public bool AddIfNotExist(string key, object item, TimeSpan? cacheTime)
		{
			if (IsCacheActive)
				return _Cache.StringSet(key, _serializerService.SerializeItem(item), cacheTime, when: When.NotExists);
			throw new Exception("Redis is not active");
		}

		public void CacheString(string key, string item, TimeSpan? cacheTime)
		{
			if (IsCacheActive)
				_Cache.StringSet(key, item, cacheTime);
			throw new Exception("Redis is not active");
		}

		public bool ControlItem(string key)
		{
			if (IsCacheActive)
			{
				return _Cache.KeyExists(key);
			}

			throw new Exception("Redis is not active");
		}

		public void PublishMessageToChannel(string channelName, string message)
		{
			_Cache.Publish(channelName, message);
		}

		public T RetrieveItemFromCache<T>(string key)
		{
			if (!IsCacheActive)
				return GetDefault<T>();

			try
			{
				return _serializerService.DeserializeItem<T>(_Cache.StringGet(key));
			}
			catch (Exception)
			{
				return GetDefault<T>();
			}
		}

		public T GetDefault<T>()
		{
			var type = typeof(T);

			if (type.IsValueType)
			{
				return (T)Activator.CreateInstance(type);
			}

			return (T)(object)null;
		}
		public List<string> GetKeyListByPattern(string pattern)
		{
			if (IsCacheActive)
			{
				return _Connection.GetServer(_Connection.Configuration).Keys(pattern: pattern).Select(x => (string)x).ToList();
			}
			throw new Exception("Redis is not active");
		}

		public void SubscribeToChannel(string channelName, Action<string, string> messageAction)
		{
			Action<RedisChannel, RedisValue> redisAction = new Action<RedisChannel, RedisValue>((key, message) => { messageAction(key, message); });
			var sub = _Connection.GetSubscriber();
			sub.Subscribe(channelName, redisAction);
		}

		public async Task<T> RetrieveItemFromCacheAsync<T>(string key)
		{
			if (!IsCacheActive)
				return GetDefault<T>();

			try
			{
				return _serializerService.DeserializeItem<T>(await _Cache.StringGetAsync(key));
			}
			catch (Exception)
			{
				return GetDefault<T>();
			}
		}

		public async Task<bool> AddIfNotExistAsync(string key, object item, TimeSpan? cacheTime)
		{
			if (IsCacheActive)
				return await _Cache.StringSetAsync(key, _serializerService.SerializeItem(item), cacheTime, when: When.NotExists);
			throw new Exception("Redis is not active");
		}

		public async Task<bool> AddOrUpdateAsync(string key, object item, TimeSpan? cacheTime)
		{
			if (IsCacheActive)
				return await _Cache.StringSetAsync(key, _serializerService.SerializeItem(item), cacheTime);
			throw new Exception("Redis is not active");
		}
	}
}
