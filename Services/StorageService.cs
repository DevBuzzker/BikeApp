using Microsoft.JSInterop;
using System.Text.Json;

namespace EBikeTracker.Services;

public class StorageService(IJSRuntime js)
{
    private readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

    public async Task SetAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await js.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", key);
        if (string.IsNullOrEmpty(json)) return default;
        try { return JsonSerializer.Deserialize<T>(json, _opts); }
        catch { return default; }
    }

    public async Task RemoveAsync(string key) =>
        await js.InvokeVoidAsync("localStorage.removeItem", key);
}
