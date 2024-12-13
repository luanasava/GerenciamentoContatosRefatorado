using Core;
using TechChallengeFase1.Models.Entity;

namespace TechChallengeFase1.Services.Interfaces
{
    public interface IContatoService
    {
        public void AdicionarContato(Contato contato);
        public void AtualizarContato(Contato contato);
        public void ExcluirContato(int DDD, int Numero);
        public Contato ConsultarContato(int DDD, int Numero);
        public List<Contato> ConsultarContatoPorDDD(int ddd);
    }
}
