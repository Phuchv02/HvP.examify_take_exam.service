using HvP.examify_take_exam.Common.Cache;

namespace HvP.examify_take_exam.DB.Repository.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private static object _lockObj = new object();
        private const string _keyListEntityCacheKey = "_listEntityCacheKey";
        private const string _keyListKeyClearWithAnyChange = "_listKeyClearWithAnyChange";
        private ICache _cache;

        public CacheRepository(ICache cache)
        {
            this._cache = cache;
        }

        private Dictionary<string, List<string>> GetListEntityCacheKey()
        {
            Dictionary<string, List<string>>? dict = new();
            dict = this._cache.GetByKeyAsync(_keyListEntityCacheKey, dict).Result;
            return dict;
        }

        private void SetListEntityCacheKey(Dictionary<string, List<string>> dict)
        {
            this._cache.SetByKey(_keyListEntityCacheKey, dict);
        }

        private List<string> GetListKeyClearWithAnyChange()
        {
            List<string> list = new();
            list = this._cache.GetByKeyAsync(_keyListKeyClearWithAnyChange, list).Result;
            return list;
        }

        private void SetListKeyClearWithAnyChange(List<string> list)
        {
            this._cache.SetByKey(_keyListKeyClearWithAnyChange, list);
        }

        public void AddEntityCacheKey<T>(string entityName, string cacheKey)
        {
            Dictionary<string, List<string>>? _listEntityCacheKey = GetListEntityCacheKey();

            if (_listEntityCacheKey.ContainsKey(cacheKey))
            {
                if (!_listEntityCacheKey[entityName].Any(key => key == cacheKey))
                {
                    _listEntityCacheKey[entityName].Add(cacheKey);
                }
            }
            else
            {
                _listEntityCacheKey.Add(entityName, new List<string> { cacheKey });
            }

            SetListEntityCacheKey(_listEntityCacheKey);
        }

        public bool SetData<T>(string entityName, string cacheKey, T data, bool clearWithChanged = false, long? ttl = 3600)
        {
            try
            {
                Dictionary<string, List<string>> _listEntityCacheKey = GetListEntityCacheKey();
                List<string> _listKeyClearWithAnyChange = GetListKeyClearWithAnyChange();

                lock (_lockObj)
                {
                    if (_listEntityCacheKey.ContainsKey(entityName))
                    {
                        if (!_listEntityCacheKey[entityName].Any(key => key == cacheKey))
                        {
                            _listEntityCacheKey[entityName].Add(cacheKey);
                        }
                    }
                    else
                    {
                        _listEntityCacheKey.Add(entityName, new List<string> { cacheKey });
                    }

                    if (clearWithChanged && !_listKeyClearWithAnyChange.Any(key => key == cacheKey))
                    {
                        _listKeyClearWithAnyChange.Add(cacheKey);
                    }

                    _cache.SetByKey<T>(cacheKey, data, ttl);
                    SetListEntityCacheKey(_listEntityCacheKey);
                    SetListKeyClearWithAnyChange(_listKeyClearWithAnyChange);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<T> GetDataAsync<T>(string key, T notfoundValue)
        {
            return await _cache.GetByKeyAsync<T>(key, notfoundValue);
        }

        public bool ClearByEntity(string entityName)
        {
            try
            {
                Dictionary<string, List<string>>? _listEntityCacheKey = GetListEntityCacheKey();
                List<string> _listKeyClearWithAnyChange = GetListKeyClearWithAnyChange();

                if (_listEntityCacheKey.ContainsKey(entityName))
                {
                    foreach (string key in _listEntityCacheKey[entityName])
                    {
                        _ = this._cache.ClearByKeyAsync(key);
                    }
                }

                foreach (string key in _listKeyClearWithAnyChange)
                {
                    _ = this._cache.ClearByKeyAsync(key);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> FlushAllAsync()
        {
            return await this._cache.FlushAllAsync();
        }
    }
}
