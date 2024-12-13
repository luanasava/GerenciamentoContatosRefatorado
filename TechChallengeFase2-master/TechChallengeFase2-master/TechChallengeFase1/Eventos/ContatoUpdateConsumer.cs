using Core;
using MassTransit;
using TechChallengeFase1.Services;
using TechChallengeFase1.Services.Interfaces;

namespace TechChallengeFase3.Consumer.Eventos
{
    public class ContatoUpdateConsumer : IConsumer<Contato>
    {
        private readonly IContatoService _contatoService;

        public ContatoUpdateConsumer(IContatoService contatoService)
        {
            _contatoService=contatoService;
        }

        public Task Consume(ConsumeContext<Contato> context)
        {
            _contatoService.AtualizarContato(context.Message);

            return Task.CompletedTask;
        }
    }
}
