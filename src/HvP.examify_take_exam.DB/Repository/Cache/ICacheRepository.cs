namespace HvP.examify_take_exam.DB.Repository.Cache
{
    public interface ICacheRepository
    {
        public void AddEntityCacheKey<T>(string entityName, string cacheKey);
        public bool SetData<T>(string entityName, string cacheKey, T data, bool clearWithChanged = false, long? ttl = 3600);
        public Task<T> GetDataAsync<T>(string key, T notfoundValue);
        public bool ClearByEntity(string entityName);
        public Task<bool> FlushAllAsync();
    }
}