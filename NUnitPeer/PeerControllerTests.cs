using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net.Http.Json;
using System.Net;
using System;

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
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Inject the handler or client into your application code
            _client = new HttpClient(mockHandler.Object);
            _logger = new Mock<ILogger<PeerController>>().Object;
            var configuration = new ConfigurationBuilder()
               .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure it uses the test directory
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            string url = configuration.GetValue<string>("AppConfig:Url") ?? string.Empty;
            _client.BaseAddress = new Uri(url);
            _fizzBuzz = new FizzBuzz(_client);
        }

        [Test]
        public async Task Test1()
        {
            // Arrange
            var controller = new PeerController(_logger, _fizzBuzz);

            //// Act
            var rslt = await controller.FizzBuzzTestAsync();
            var value = rslt.Value;
            //// Assert
            Assert.IsNotNull(rslt);
            Assert.That(value, Does.StartWith("Result:"));
        }
    }
}