using StackExchange.Redis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvvaMobile.Core.Redis;

public class RedisAzureClient : IRedisAzureClient
{
    private static ConnectionMultiplexer _redisConn;
    private readonly IDatabase _cache;
    public RedisAzureClient(string connectionString)
    {
        _redisConn = ConnectionMultiplexer.Connect(connectionString);
        _cache = _redisConn.GetDatabase();
    }

    public async Task<bool> IsExists(string key)
    {
        return await _cache.KeyExistsAsync(key);
    }

    #region String Methods
    public async Task<string> String_Get(string key)
    {
        return await _cache.StringGetAsync(key);
    }

    public async Task<bool> String_Set(string key, string value)
    {
        return await _cache.StringSetAsync(key, value);
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

    #region Clear Methods
    public async Task Clear(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }

    public void ClearAll()
    {
        var endpoints = _redisConn.GetEndPoints(true);
        foreach (var endpoint in endpoints)
        {
            var server = _redisConn.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }
    #endregion

    #region Generic Methods
    public async Task<T> Deserialize_Get<T>(string key)
    {
        return JsonSerializer.Deserialize<T>(await String_Get(key));
    }

    public async Task<bool> Serialize_Set(string key, object value)
    {
        return await String_Set(key, JsonSerializer.Serialize(value));
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class
    {
        var result = await String_Get(key);
        if (string.IsNullOrEmpty(result))
        {
            result = JsonSerializer.Serialize(await action());
            await _cache.StringSetAsync(key, result);
        }
        return JsonSerializer.Deserialize<T>(result);
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
            await _cache.ListRightPushAsync(key, value, When.NotExists);
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