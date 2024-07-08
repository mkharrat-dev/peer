using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

namespace NUnitPeer
{
    public class PeerControllerTests
    {

        private ILogger<PeerController> _logger;
        private IFizzBuzz _fizzBuzz;
        private HttpClient _client;
        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<PeerController>>().Object;
            var configuration = new ConfigurationBuilder()
               .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure it uses the test directory
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            string url = configuration.GetValue<string>("AppConfig:Url") ?? string.Empty;

            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);


            mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{'key':'value'}"),
            });
            // Inject the handler or client into your application code
            _client = new HttpClient(mockHandler.Object);
            _client.BaseAddress = new Uri( url );
            _fizzBuzz = new FizzBuzz(_client);
        }

        [Test]
        public async Task Test1()
        {
            // Arrange
            var controller = new PeerController(_logger, _fizzBuzz);

            // Act
            var result = await controller.FizzBuzzTestAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Result: Invalid data", okResult.Value);
        }
    }
}