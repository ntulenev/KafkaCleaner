using Confluent.Kafka;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

using FluentAssertions;

using Logic.Configuration;

namespace Logic.Tests
{
    public class KafkaServiceClientTests
    {
        [Fact(DisplayName = "KafkaServiceClient can be created.")]
        [Trait("Category", "Unit")]
        public void KafkaServiceClientCanBeCreatedWithValidParams()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
            var optinosMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
            var clientMock = new Mock<IAdminClient>();
            // Act
            var exception = Record.Exception(() => new KafkaServiceClient(loggerMock.Object, optinosMock.Object, clientMock.Object));

            // Assert
            exception.Should().BeNull();
        }

        [Fact(DisplayName = "KafkaServiceClient cant be created with null config.")]
        [Trait("Category", "Unit")]
        public void KafkaServiceClientCantBeCreatedWithNullConfig()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
            var optinosMock = (IOptions<KafkaServiceClientConfiguration>)null!;
            var clientMock = new Mock<IAdminClient>();
            // Act
            var exception = Record.Exception(() => new KafkaServiceClient(loggerMock.Object, optinosMock, clientMock.Object));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "KafkaServiceClient cant be created with null admin tool.")]
        [Trait("Category", "Unit")]
        public void KafkaServiceClientCantBeCreatedWithNullAdminTool()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
            var optinosMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
            var clientMock = (IAdminClient)null!;
            // Act
            var exception = Record.Exception(() => new KafkaServiceClient(loggerMock.Object, optinosMock.Object, clientMock));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "KafkaServiceClient cant be created with null logger.")]
        [Trait("Category", "Unit")]
        public void KafkaServiceClientCantBeCreatedWithNullLogger()
        {
            // Arrange
            var loggerMock = (ILogger<KafkaServiceClient>)null!;
            var optinosMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
            var clientMock = new Mock<IAdminClient>();
            // Act
            var exception = Record.Exception(() => new KafkaServiceClient(loggerMock, optinosMock.Object, clientMock.Object));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }
    }
}
