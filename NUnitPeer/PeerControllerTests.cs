using Castle.Core.Configuration;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Peer;

namespace NUnitPeer
{
    public class PeerControllerTests
    {

        private ILogger<PeerController> _logger;
        private IFizzBuzz _fizzBuzz;
        private HttpClient _client;
        private IAppConfig _config;
        [SetUp]
        public void Setup()
        {
            _client = new Mock<HttpClient>().Object;
            _logger = new Mock<ILogger<PeerController>>().Object;
            var configuration = new ConfigurationBuilder()
               .SetBasePath(TestContext.CurrentContext.TestDirectory) // Ensure it uses the test directory
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
            var appConfigSection = configuration.GetSection("AppConfig");
            _config = appConfigSection.Get<AppConfig>();
            _fizzBuzz = new FizzBuzz(_client, _config);
        }

        [Test]
        public void Test1()
        { 
            // Arrange
            var controller = new PeerController(_logger, _fizzBuzz);

            //// Act
            var response = controller.FizzBuzzTestAsync();

            //// Assert
            Assert.Pass();
        }
    }
}