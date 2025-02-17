using Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFase3.Producer.Services.Interfaces;

namespace TechChallengeFase3.Controllers.Producer
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        private readonly IConsumerService _consumerService;

        public ContatoController(IBus bus, IConfiguration configuration, IConsumerService consumerService)
        {  
                _bus = bus;
                _configuration = configuration;
                _consumerService = consumerService;
        }
        

        [HttpGet("{DDD}/{Numero}")]
        public IActionResult obterContato(int DDD, int Numero)
        {
            try
            {
                return Ok(_consumerService.ConsultarContatos(DDD, Numero));
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> adicionarContato(Contato contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var filaCreate = _configuration["MassTransit:FilaCreate"] ?? string.Empty;
                    var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{filaCreate}"));
                    await endpoint.Send(contato);

                    return Created($"/api/Contato/{contato}", contato);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro: {ex.Message}");
            }
        }
        [HttpPut]
        public async Task<IActionResult> atualizarContato(Contato contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var filaUpdate = _configuration["MassTransit:FilaUpdate"] ?? string.Empty;
                    var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{filaUpdate}"));
                    await endpoint.Send(contato);                  

                    return Ok(contato);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro: {ex.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> excluirContato(int DDD, int Numero)
        {
            try
            {
                var filaDelete = _configuration["MassTransit:FilaDelete"] ?? string.Empty;
                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{filaDelete}"));

                ContatoExclusaoDTO contato = new ContatoExclusaoDTO { DDD = DDD, Telefone = Numero };
                await endpoint.Send(contato);

                return Ok($"Contato ({DDD}) {Numero} excluído com sucesso");
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro: {ex.Message}");
            }
        }
        [HttpGet("{DDD}")]
        public IActionResult obterContatoRegiao(int DDD)
        {
            try
            {               
                return Ok(_consumerService.ConsultarContatoPorDDD(DDD));
            }
            catch (Exception ex)
            {

                throw new Exception($"Erro: {ex.Message}");
            }
        }
    }
}
