using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using ProyectXAPI.Models.DTO;

namespace ProyectXAPILibrary.Controller
{
    public abstract class Controller
    {
        protected HttpClient client { get; private set; }
        protected JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public Controller(HttpClient client)
        {
            this.client = client;
        }
        private ResponseDTO<TValue>? DeserializeResponse<TValue>(string responseDTO)
        {
            return JsonSerializer.Deserialize<ResponseDTO<TValue>>(responseDTO, serializerOptions);
        }
        private async Task<ResponseDTO<TValue>?> DeserializeResponseAsync<TValue>(HttpResponseMessage responseContent)
        {
            string responseDTO = await responseContent.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ResponseDTO<TValue>>(responseDTO, serializerOptions);
        }
        protected async Task<HttpResponseMessage> SendRequest(HttpRequestMessage requestMessage,object bodyContent)
        {
            requestMessage.Content = new StringContent(JsonSerializer.Serialize(bodyContent, serializerOptions), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await this.client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request)
        {
            HttpResponseMessage response = await this.client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }
        protected ResponseDTO<T> DeserializeResponse<T>(HttpResponseMessage responseContent)
        {
            return DeserializeResponseAsync<T>(responseContent).GetAwaiter().GetResult() ?? new ResponseDTO<T>()
            {
                IsSuccess = false
            };
        }
    }
}
