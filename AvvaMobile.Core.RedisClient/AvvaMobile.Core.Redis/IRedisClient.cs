using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvvaMobile.Core.Redis;

public interface IRedisClient
{
    Task<bool> IsExists(string key);

    Task<string> String_Get(string key);
    Task<bool> String_Set(string key, string value);

    Task<int> Int_Get(string key);
    Task<bool> Int_Set(string key, int value);

    Task<T> Deserialize_Get<T>(string key);
    Task<bool> Serialize_Set(string key, object value);
    
    Task<List<SelectListItem>> List_SelectListItem_Get(string key);
    Task<bool> List_SelectListItem_Set(string key, List<SelectListItem> value);

    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class;
    Task Clear(string key);
    void ClearAll();

    Task List_Add(string key, List<string> values);
    Task List_Add(string key, List<int> values);
    Task List_Add(string key, List<bool> values);
    Task List_Add(string key, List<long> values);
}