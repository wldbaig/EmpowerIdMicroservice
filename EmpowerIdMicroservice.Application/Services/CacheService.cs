using StackExchange.Redis;
using System.Text.Json;

namespace EmpowerIdMicroservice.Application.Services
{
    public interface ICacheService
    {
        T GetData<T>(string key);

        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

        object RemoveData(string key);
    }

    public class CacheService : ICacheService
    {
        private IDatabase _cachedb;

        public CacheService()
        {
            // TODO : Riminder get from appsettings

            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cachedb = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _cachedb.StringGet(key);

            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public object RemoveData(string key)
        {
            var exist = _cachedb.KeyExists(key);

            if(exist)
                return _cachedb.KeyDelete(key);

            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

            var isSet = _cachedb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);

            return isSet;
        }
    }
}
