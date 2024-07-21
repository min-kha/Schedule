using Newtonsoft.Json;
using System.Text;

namespace ScheduleWeb.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content);
    }

    public async Task<T?> PostAsync<T>(string url, object data)
    {
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public async Task<T?> PostFileAsync<T>(string url, IFormFile file, Dictionary<string, string>? additionalData = null)
    {
        using var content = new MultipartFormDataContent();

        // Thêm file vào content
        using var fileStream = file.OpenReadStream();
        content.Add(new StreamContent(fileStream), "file", file.FileName);

        // Thêm dữ liệu bổ sung (nếu có)
        if (additionalData != null)
        {
            foreach (var kvp in additionalData)
            {
                content.Add(new StringContent(kvp.Value), kvp.Key);
            }
        }

        // Gửi yêu cầu
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Lỗi: {response.StatusCode} - {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }



    public async Task<T?> PutAsync<T>(string url, object data)
    {
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public async Task DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}");
        }
    }
}