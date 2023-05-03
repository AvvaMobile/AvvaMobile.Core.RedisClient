using StackExchange.Redis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvvaMobile.Core.Redis;

public class RedisClient : IRedisClient
{
    private static ConnectionMultiplexer _redis;

    public RedisClient(string host, int port, string user, string password)
    {
        _redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { { host, port } },
                User = user,
                Password = password
            });
    }

    public async Task<bool> IsExists(string key)
    {
        return await _redis.GetDatabase().KeyExistsAsync(key);
    }

    #region String Methods
    public async Task<string> String_Get(string key)
    {
        return await _redis.GetDatabase().StringGetAsync(key);
    }

    public async Task<bool> String_Set(string key, string value)
    {
        return await _redis.GetDatabase().StringSetAsync(key, value);
    }

    public async Task<int> Int_Get(string key)
    {
        var val = await String_Get(key);
        return int.Parse(val);
    }

    public async Task<bool> Int_Set(string key, int value)
    {
        return await String_Set(key, value.ToString());
    }
    #endregion

    #region Generic Methods
    public async Task<T> Deserialize_Get<T>(string key)
    {
        return JsonSerializer.Deserialize<T>(await _redis.GetDatabase().StringGetAsync(key));
    }

    public async Task<bool> Serialize_Set(string key, object value)
    {
        return await String_Set(key, JsonSerializer.Serialize(value));
    }
    #endregion

    #region SelectListItems Methods
    public async Task<List<SelectListItem>> List_SelectListItem_Get(string key)
    {
        return await Deserialize_Get<List<SelectListItem>>(key);
    }

    public async Task<bool> List_SelectListItem_Set(string key, List<SelectListItem> value)
    {
        return await Serialize_Set(key, JsonSerializer.Serialize(value));
    }
    #endregion

    #region List Methods
    public async Task List_Add(string key, List<string> values)
    {
        foreach (var value in values)
        {
            await _redis.GetDatabase().ListRightPushAsync(key, value, When.NotExists);
        }
    }

    public async Task List_Add(string key, List<int> values)
    {
        await List_Add(key, values.Select(x => x.ToString()).ToList());
    }

    public async Task List_Add(string key, List<bool> values)
    {
        await List_Add(key, values.Select(x => x.ToString()).ToList());
    }

    public async Task List_Add(string key, List<long> values)
    {
        await List_Add(key, values.Select(x => x.ToString()).ToList());
    }
    #endregion
}