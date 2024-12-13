
using Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TechChallengeFase1.Services.Interfaces;
using TechChallengeFase3.Controllers.Producer;
using TechChallengeFase3.Producer.Services.Interfaces;


namespace TechChallenge.Teste
{
    public class ContatoControllerTest_Adicionar
    {
        private readonly Mock<IBus> _mockBusMassTransit;

        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConsumerService> _mockConsumerService;
        private readonly Mock<IContatoService> _mockContatoService;

        private readonly Mock<IBrasilApiService> _mockBrasilApiService;
        private readonly ContatoController _controller;

        public ContatoControllerTest_Adicionar()
        {
            _mockContatoService = new Mock<IContatoService>();
            _mockBrasilApiService = new Mock<IBrasilApiService>();
            _mockConsumerService = new Mock<IConsumerService>();

            var mockSendEndpoint = new Mock<ISendEndpoint>();
            mockSendEndpoint.Setup(endpoint => endpoint.Send(It.IsAny<Contato>(), default))
                        .Returns(Task.CompletedTask);


            _mockBusMassTransit = new Mock<IBus>();
            _mockBusMassTransit.Setup(bus => bus.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(mockSendEndpoint.Object);

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["MassTransit:FilaCreate"]).Returns("FilaCreate");
            _mockConfiguration.Setup(config => config["MassTransit:Servidor"]).Returns("localhost");
            _mockConfiguration.Setup(config => config["MassTransit:Usuario"]).Returns("guest");
            _mockConfiguration.Setup(config => config["MassTransit:Senha"]).Returns("guest");



            _controller = new ContatoController(_mockBusMassTransit.Object, _mockConfiguration.Object, _mockConsumerService.Object);

        }

        [Fact]

        public void AdicionarContato_ContatoValido()
        {
            //Arrange
            var contato = new Contato
            {
                Nome = "fiap",
                DDD = 11,
                Telefone = 999999999,
                Email = "teste@teste.com"
            };
            _controller.ModelState.Clear();


            //Act
            var result = (CreatedResult)_controller.adicionarContato(contato).Result;

            //Assert

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal($"/api/Contato/{contato}", createdResult.Location);
            Assert.Equal(contato, createdResult.Value);
        }

        [Fact]
        public void AdicionarContato_ContatoInvalido()
        {
            //Arrange
            var contato = new Contato
            {
                Nome = "",
                DDD = 111,
                Telefone = 12128,
                Email = "açsdkçlkasdç"
            };
            _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório");

            //Act
            var result = (BadRequestObjectResult)_controller.adicionarContato(contato).Result;

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

    }
}