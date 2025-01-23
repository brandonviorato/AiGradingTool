using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace AiGradingTool
{
    public class OllamaApi
    {
        private readonly HttpClient _httpClient;
        private string url = "http://localhost:11434/api/generate";
        private string model = "mistral";
        private string message = "hello there!";
        private bool stream = false; // waits for entire response to generate before sending

        public OllamaApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // this method sets the message to send to the AI
        public void SetMessage(string message)
        {
            this.message = message;
        }

        // this method sets the request data (LLM model, message, etc.)
        private string setRequestData(string model, string message, bool stream)
        {
            // set body
            var requestData = new
            {
                model = model,
                prompt = message,
                stream = stream
            };

            // serialize jsonBody
            string jsonBody = JsonSerializer.Serialize(requestData);

            return jsonBody;
        }

        // this method is what makes the api calls
        public async Task<T?> GetAsync<T>()
        {   
            // set up the request
            StringContent content = new StringContent(setRequestData(this.model, this.message, this.stream), Encoding.UTF8, "application/json");
            
            // make api call and await response
            HttpResponseMessage response = await _httpClient.PostAsync(this.url, content);
            
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseBody);
        }

    }
}
