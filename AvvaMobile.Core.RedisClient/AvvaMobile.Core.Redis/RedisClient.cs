using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    #region Set Methods
    public async Task<bool> Set(string key, string value)
    {
        return await _redis.GetDatabase().StringSetAsync(key, value);
    }

    public async Task<bool> Set(string key, string value, TimeSpan expiry)
    {
        return await _redis.GetDatabase().StringSetAsync(key, value, expiry);
    }
    #endregion

    #region Get Methods
    public async Task<string> Get_String(string key)
    {
        return await _redis.GetDatabase().StringGetAsync(key);
    }
    #endregion

    #region Generic Methods
    public async Task<T> Get_Deserialized<T>(string key)
    {
        string value = await _redis.GetDatabase().StringGetAsync(key);

        return string.IsNullOrWhiteSpace(value) ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task<bool> Set_Serialized(string key, object value)
    {
        return await Set(key, JsonSerializer.Serialize(value));
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
            await _redis.GetDatabase().ListRightPushAsync(key, value, When.NotExists);
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