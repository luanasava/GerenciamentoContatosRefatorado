using Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallengeFase1.Models.Entity;
using TechChallengeFase1.Services.Interfaces;
using TechChallengeFase3.Controllers.Producer;
using TechChallengeFase3.Producer.Services.Interfaces;

namespace TechChallenge.Teste
{
    public class ContatoControllerTest_Excluir
    {
        private readonly Mock<IBus> _mockBusMassTransit;

        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConsumerService> _mockConsumerService;
        private readonly Mock<ISendEndpoint> _mockSendEndpoint;
        private readonly Mock<IContatoService> _mockContatoService;

        private readonly Mock<IBrasilApiService> _mockBrasilApiService;
        private readonly ContatoController _controller;

        public ContatoControllerTest_Excluir()
        {
            _mockContatoService = new Mock<IContatoService>();
            _mockBrasilApiService = new Mock<IBrasilApiService>();
            _mockConsumerService = new Mock<IConsumerService>();

            _mockSendEndpoint = new Mock<ISendEndpoint>();
            _mockSendEndpoint.Setup(endpoint => endpoint.Send(It.IsAny<Contato>(), default))
                        .Returns(Task.CompletedTask);


            _mockBusMassTransit = new Mock<IBus>();
            _mockBusMassTransit.Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(_mockSendEndpoint.Object);

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["MassTransit:FilaDelete"]).Returns("FilaDelete");
            _mockConfiguration.Setup(config => config["MassTransit:Servidor"]).Returns("localhost");
            _mockConfiguration.Setup(config => config["MassTransit:Usuario"]).Returns("guest");
            _mockConfiguration.Setup(config => config["MassTransit:Senha"]).Returns("guest");



            _controller = new ContatoController(_mockBusMassTransit.Object, _mockConfiguration.Object, _mockConsumerService.Object);

        }

        [Fact]
        public void ExcluirContato_RetornaOK()
        {
            //Arrange
            int DDD = 11;
            int Numero = 999999999;

            //Act
            var result = (OkObjectResult)_controller.excluirContato(DDD, Numero).Result;

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Contato ({DDD}) {Numero} excluído com sucesso", okResult.Value);
        } 

        [Fact]
        public async Task ExcluirContato_Excecao()
        {
             int DDD = 11;
             int Numero = 999999999;

            _mockBusMassTransit
                .Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>()))
                .ReturnsAsync(_mockSendEndpoint.Object);

            _mockSendEndpoint
               .Setup(endpoint => endpoint.Send(It.IsAny<ContatoExclusaoDTO>(), default))
               .Throws(new Exception("Erro simulado"));

            var controller = new ContatoController(_mockBusMassTransit.Object, _mockConfiguration.Object, _mockConsumerService.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => controller.excluirContato(DDD, Numero));
            Assert.Equal("Erro: Erro simulado", exception.Message);
        }
    }
}
