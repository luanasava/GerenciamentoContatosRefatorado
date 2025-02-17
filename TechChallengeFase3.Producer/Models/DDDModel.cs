using System.Text.Json.Serialization;

namespace TechChallengeFase3.Models
{
    public class DDDModel
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("state")]
        public string Estado { get; set; }

        [JsonPropertyName("cities")]
        public List<string> Cidades { get; set; }


        [JsonPropertyName("neighborhood")]
        public string Regiao { get; set; }

        [JsonPropertyName("street")]
        public string Rua { get; set; }

        [JsonPropertyName("service")]
        public string Servico { get; set; }
    }
}
