using System.Dynamic;
using System.Text.Json;
using TechChallengeFase3.Models.DTO;
using TechChallengeFase3.Models;
using TechChallengeFase3.Models.Entity;
using TechChallengeFase3.Producer.Services.Interfaces;

namespace TechChallengeFase3.Producer.Services
{
    public class ConsumerService : IConsumerService
    {
        public async Task<Contatos> ConsultarContatos(int ddd, int numero)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://0.0.0.0:5261/{ddd}/{numero}");

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                var contentResp = await response.Content.ReadAsStringAsync();
                var objResponse = JsonSerializer.Deserialize<Contatos>(contentResp);

                return objResponse;
            }           
        }

        public async Task<List<ContatosDTO>> ConsultarContatoPorDDD(int ddd)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://0.0.0.0:5261/{ddd}");

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                var contentResp = await response.Content.ReadAsStringAsync();
                var objResponse = JsonSerializer.Deserialize<List<ContatosDTO>>(contentResp);

                return objResponse;
            }
        }
    }
}
