using TechChallengeFase3.Models.DTO;
using TechChallengeFase3.Models.Entity;

namespace TechChallengeFase3.Producer.Services.Interfaces
{
    public interface IConsumerService
    {
        Task<Contatos> ConsultarContatos(int ddd, int numero);

        Task<List<ContatosDTO>> ConsultarContatoPorDDD(int ddd);

    }
}
