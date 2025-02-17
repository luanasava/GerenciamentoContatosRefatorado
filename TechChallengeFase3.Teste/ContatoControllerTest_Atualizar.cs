using Core;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallengeFase1.Models.Entity;
using TechChallengeFase1.Services.Interfaces;
using TechChallengeFase3.Controllers.Producer;
using TechChallengeFase3.Producer.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechChallenge.Teste
{
    public class ContatoControllerTest_Atualizar
    {
        private readonly Mock<IBus> _mockBusMassTransit;

        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConsumerService> _mockConsumerService;
        private readonly Mock<IContatoService> _mockContatoService;
        private readonly Mock<ISendEndpoint> _mockSendEndpoint;


        private readonly Mock<IBrasilApiService> _mockBrasilApiService;
        private readonly ContatoController _controller;

        public ContatoControllerTest_Atualizar()
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
            _mockConfiguration.Setup(config => config["MassTransit:FilaUpdate"]).Returns("FilaUpdate");
            _mockConfiguration.Setup(config => config["MassTransit:Servidor"]).Returns("localhost");
            _mockConfiguration.Setup(config => config["MassTransit:Usuario"]).Returns("guest");
            _mockConfiguration.Setup(config => config["MassTransit:Senha"]).Returns("guest");

            _controller = new ContatoController(_mockBusMassTransit.Object, _mockConfiguration.Object, _mockConsumerService.Object);

        }

        [Fact]

        public void AtualizarContato_RetornaOk()
        {
            //Arrange
            var contato = new Contato
            {
                Nome = "FiapAtualizado",
                DDD = 44,
                Telefone = 888888888,
                Email = "email@atualizado.com"
            };

            //Act
            var result = (OkObjectResult)_controller.atualizarContato(contato).Result;

            //Assert

            var retornaOk = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contato, retornaOk.Value);

        }


        [Fact]
        public async Task AtualizarContato_Excecao()
        {
            //Arrange
            var contato = new Contato
            {
                Nome = "",
                DDD = 555,
                Telefone = 98765,
                Email = "emailInvalido"
            };

            _mockBusMassTransit
                .Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>()))
                .ReturnsAsync(_mockSendEndpoint.Object);

            _mockSendEndpoint
               .Setup(endpoint => endpoint.Send(It.IsAny<Contato>(), default))
               .Throws(new Exception("Erro simulado"));

            var controller = new ContatoController(_mockBusMassTransit.Object, _mockConfiguration.Object, _mockConsumerService.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => controller.atualizarContato(contato));
            Assert.Equal("Erro: Erro simulado", exception.Message);
        }
    }
}
