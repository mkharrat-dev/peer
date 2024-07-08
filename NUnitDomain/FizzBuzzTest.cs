using Domain;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;

namespace NUnitDomain
{
    public class Tests
    {
        HttpClient _client;
        IFizzBuzz _fizzBuzz;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure it uses the test directory
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            string url = configuration.GetSection("AppConfig").GetSection("Url").Value ?? string.Empty;

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
            _client.BaseAddress = new Uri(url);
            _fizzBuzz = new FizzBuzz(_client);
        }

        [Test]
        public async Task CalculateResultAsyncTest()
        {
            // Act
            var result = await _fizzBuzz.CalculateResultAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "Invalid data");
        }
    }
}