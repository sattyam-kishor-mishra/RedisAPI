using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;
        private IDatabase _db {get; set;}

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }
        public void CreatePlatform(Platform platform)
        {
            if(platform == null)
            {
                throw new ArgumentOutOfRangeException(nameof(platform));
            }           

            var serializedPlatform = JsonSerializer.Serialize(platform);

            _db.StringSet(platform.Id, serializedPlatform);
            _db.SetAdd("PlatformSet", serializedPlatform);
            
        }

        public Platform? GetPlatformById(string id)
        {            
            var platform = _db.StringGet(id);
            
            if(!string.IsNullOrEmpty(platform))
            {
                return JsonSerializer.Deserialize<Platform>(platform!);
            }

            return null;
        }

        public IEnumerable<Platform?>? GetPlatforms()
        {
            var platformMember = _db.SetMembers("PlatformSet");

            if(platformMember.Length > 0)
            {
                var obj = Array.ConvertAll(platformMember, val => JsonSerializer.Deserialize<Platform>(val!)).ToList();
                return obj;
            }

            return null!;
        }
    }
}