using Core;
using MassTransit;
using TechChallengeFase1.Services;
using TechChallengeFase1.Services.Interfaces;

namespace TechChallengeFase3.Consumer.Eventos
{
    public class ContatoDeleteConsumer : IConsumer<ContatoExclusaoDTO>
    {
        private readonly IContatoService _contatoService;

        public ContatoDeleteConsumer(IContatoService contatoService)
        {
            _contatoService=contatoService;
        }

        public Task Consume(ConsumeContext<ContatoExclusaoDTO> context)
        {
            _contatoService.ExcluirContato(context.Message.DDD, context.Message.Telefone);

            return Task.CompletedTask;
        }
    }
}
