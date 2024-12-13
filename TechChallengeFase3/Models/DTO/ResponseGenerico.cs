using System.Dynamic;
using System.Net;

namespace TechChallengeFase3.Models.DTO
{
    public class ResponseGenerico<T> where T : class
    {
            public HttpStatusCode CodigoHttp { get; set; }
            public T? DadosRetorno { get; set; }
            public ExpandoObject? ErroRetorno { get; set; }
    }
}
