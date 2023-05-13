using Microsoft.AspNetCore.Mvc.Rendering;

namespace AvvaMobile.Core.Redis;

public interface IRedisClient
{
    Task<bool> IsExists(string key);

    Task<bool> Set(string key, string value);
    
    Task<bool> Set(string key, string value, TimeSpan expiry);
    
    Task<string> Get_String(string key);

    Task<T> Get_Deserialized<T>(string key);
    Task<bool> Set_Serialized(string key, object value);
    
    Task<List<SelectListItem>> Get_SelectListItems(string key);
    Task<bool> Set_SelectListItems(string key, List<SelectListItem> value);
    
    Task Add_List(string key, List<string> values);
    Task Add_List(string key, List<int> values);
    Task Add_List(string key, List<bool> values);
    Task Add_List(string key, List<long> values);
}