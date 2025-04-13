namespace HvP.examify_take_exam.Common.Cache
{
    public interface ICache
    {
        public bool SetByKey<T>(string key, T data, long? ttl = 3600);
        public Task<T> GetByKeyAsync<T>(string key, T notfoundValue);
        public Task<bool> ClearByKeyAsync(string key);
        public Task<bool> FlushAllAsync();
    }
}
