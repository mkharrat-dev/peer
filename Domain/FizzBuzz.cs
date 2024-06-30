using Peer;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Domain
{
    public class FizzBuzz : IFizzBuzz
    {
        private readonly HttpClient _client;
        private readonly IAppConfig _config;

        public FizzBuzz(HttpClient client, IAppConfig config)
        {
            _client = client;
            _config = config;
        }
        public async Task<string> CalculateResultAsync()
        {
            try
            {
                var response = await _client.GetAsync(_config.Url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var value = content.Split(":");

                if (value.Length == 2)
                {
                    if (int.TryParse(value[0], out int firstNumber) && int.TryParse(value[1], out int secondNumber))
                    {
                        return (firstNumber * secondNumber).ToString();
                    }
                }
                else if (value.Length == 3)
                {
                    if (int.TryParse(value[0], out int A) && int.TryParse(value[1], out int B) && int.TryParse(value[2], out int C))
                    {
                        var result = string.Empty;
                        if (C % A == 0) result += "FIZZ";
                        if (C % B == 0) result += "BUZZ";
                        return result;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                // Log exception or handle it as needed
                return $"Request error: {e.Message}";
            }
            catch (Exception e)
            {
                // Log exception or handle it as needed
                return $"Error: {e.Message}";
            }

            return "Invalid data";
        }
    }
}
