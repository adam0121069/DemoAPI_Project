using System.Text;
using System.Text.Json;
using CharlieApi.Models;

namespace CharlieApi.Service
{
    public class QuestionAnswerService
    {
        private const string ServiceLink = "http://localhost:66/qa";
        private readonly HttpClient _httpClient;

        public QuestionAnswerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAnswerAsync(string question)
        {
            try
            {
                var requestPayload = new { question };
                var jsonPayload = JsonSerializer.Serialize(requestPayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ServiceLink, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<AnswerResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                return result?.Answer ?? "No answer found.";
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the exception
                return $"Request error: {ex.Message}";
            }
            catch (JsonException ex)
            {
                // Log or handle the exception
                return $"JSON parsing error: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return $"An unexpected error occurred: {ex.Message}";
            }
        }
    }
}
