using StackExchange.Redis;
using System.Text.Json;

namespace HvP.examify_take_exam.Common.Cache
{
    public class RedisCache : ICache
    {
        private static object _lockObj = new object();
        private IDatabase _database;
        private IConnectionMultiplexer _redis;

        public RedisCache(IConnectionMultiplexer redis)
        {
            if (redis != null)
            {
                _database = redis.GetDatabase();
                _redis = redis;
            }
        }

        public bool SetByKey<T>(string key, T value, long? ttl = 3600)
        {
            try
            {
                if (_database == null)
                {
                    return false;
                }

                lock (_lockObj)
                {
                    var jsonData = JsonSerializer.Serialize(value);
                    _database.StringSet(key, jsonData, TimeSpan.FromSeconds(ttl!.Value));

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<T> GetByKeyAsync<T>(string key, T notfoundValue)
        {
            try
            {
                if (_database == null)
                {
                    return notfoundValue;
                }

                RedisValue value = await _database.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                {
                    return notfoundValue;
                }

                return JsonSerializer.Deserialize<T>(value.ToString()) ?? notfoundValue;
            }
            catch (Exception ex)
            {
                return notfoundValue;
            }
        }

        public async Task<bool> ClearByKeyAsync(string key)
        {
            try
            {
                if (_database == null)
                {
                    return false;
                }

                var rs = await _database.KeyDeleteAsync(key);
                return rs;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> FlushAllAsync()
        {
            try
            {
                if (_database == null)
                {
                    return false;
                }

                await _database.ExecuteAsync("FLUSHDB");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
