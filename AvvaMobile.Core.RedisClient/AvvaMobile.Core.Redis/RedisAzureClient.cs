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

    #region Set Methods
    public async Task<bool> Set(string key, string value)
    {
        return await _cache.StringSetAsync(key, value);
    }

    public async Task<bool> Set(string key, string value, TimeSpan expiry)
    {
        return await _cache.StringSetAsync(key, value, expiry);
    }
    #endregion

    #region Get Methods
    public async Task<string> Get_String(string key)
    {
        return await _cache.StringGetAsync(key);
    }
    #endregion

    #region Clear Methods
    public async Task Clear(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }

    public async Task Remove(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }

    public async Task ClearAll()
    {
        var endpoints = _redisConn.GetEndPoints();
        foreach (var endpoint in endpoints)
        {
            var server = _redisConn.GetServer(endpoint);
            await server.FlushDatabaseAsync();
        }
    }

    #endregion

    #region Generic Methods
    public async Task<T> Get_Deserialized<T>(string key)
    {
        string value = await _cache.StringGetAsync(key);
        return string.IsNullOrWhiteSpace(value) ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task<bool> Set_Serialized(string key, object value)
    {
        return await Set(key, JsonSerializer.Serialize(value));
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class
    {
        var result = await Get_String(key);
        if (string.IsNullOrEmpty(result))
        {
            result = JsonSerializer.Serialize(await action());
            await _cache.StringSetAsync(key, result);
        }
        return JsonSerializer.Deserialize<T>(result);
    }
    #endregion

    #region SelectListItems Methods
    public async Task<List<SelectListItem>> Get_SelectListItems(string key)
    {
        return await Get_Deserialized<List<SelectListItem>>(key);
    }

    public async Task<bool> Set_SelectListItems(string key, List<SelectListItem> value)
    {
        return await Set_Serialized(key, value);
    }
    #endregion

    #region List Methods
    public async Task Add_List(string key, List<string> values)
    {
        foreach (var value in values)
        {
            await _cache.ListRightPushAsync(key, value, When.NotExists);
        }
    }

    public async Task Add_List(string key, List<int> values)
    {
        await Add_List(key, values.Select(x => x.ToString()).ToList());
    }

    public async Task Add_List(string key, List<bool> values)
    {
        await Add_List(key, values.Select(x => x.ToString()).ToList());
    }

    public async Task Add_List(string key, List<long> values)
    {
        await Add_List(key, values.Select(x => x.ToString()).ToList());
    }
    #endregion
}