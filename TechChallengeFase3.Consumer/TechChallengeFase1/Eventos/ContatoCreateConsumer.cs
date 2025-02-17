using Core;
using MassTransit;
using TechChallengeFase1.Services;
using TechChallengeFase1.Services.Interfaces;

namespace TechChallengeFase3.Consumer.Eventos
{
    public class ContatoCreateConsumer : IConsumer<Contato>
    {
        private readonly IContatoService _contatoService;

        public ContatoCreateConsumer(IContatoService contatoService)
        {
            _contatoService=contatoService;
        }

        public Task Consume(ConsumeContext<Contato> context)
        {
            _contatoService.AdicionarContato(context.Message);

            return Task.CompletedTask;
        }
    }
}
