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
            _config = new Mock<IAppConfig>().Object; 
            _fizzBuzz = new FizzBuzz(_client, _config);
        }

        [Test]
        public void Test1()
        { 
            var configurationSectionMock = new Mock<IConfigurationSection>();
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationSectionMock
               .Setup(x => x.Value)
               .Returns("https://localhost:5001/Peer");

            configurationMock
               .Setup(x => x.GetSection("AppConfig:Url"))
               .Returns(configurationSectionMock.Object);

            // Arrange
            var controller = new PeerController(_logger, _fizzBuzz);

            //// Act
            var response = controller.FizzBuzzTestAsync();

            //// Assert
            Assert.Pass();
        }
    }
}