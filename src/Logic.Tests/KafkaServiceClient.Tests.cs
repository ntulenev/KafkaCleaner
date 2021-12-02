using Confluent.Kafka;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

using FluentAssertions;

using Logic.Configuration;
using Models;

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


        [Fact(DisplayName = "KafkaServiceClient can request topic.")]
        [Trait("Category", "Unit")]
        public void KafkaServiceClientCanRequestTopics()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
            var optinosMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
            var clientMock = new Mock<IAdminClient>();
            var topic1 = "topic 1";
            var topicReserved = "topic reserved";
            var topic3 = "topic 3";
            var timeout = TimeSpan.FromSeconds(42);
            optinosMock.Setup(x => x.Value).Returns(new KafkaServiceClientConfiguration
            {
                ReservedTopics = new List<string>
                {
                    topicReserved
                },
                MetadataTimeout = timeout
            });
            var brokerTopics = new List<TopicMetadata>()
            {
               new TopicMetadata(topic1,new List<PartitionMetadata>(),null!),
               new TopicMetadata(topicReserved,new List<PartitionMetadata>(),null!),
               new TopicMetadata(topic3,new List<PartitionMetadata>(),null!),
            };
            var meta = new Metadata(new List<BrokerMetadata>(), brokerTopics, 1, string.Empty);
            clientMock.Setup(x => x.GetMetadata(timeout)).Returns(meta);
            var client = new KafkaServiceClient(loggerMock.Object, optinosMock.Object, clientMock.Object);

            // Act
            var result = client.RequestTopicsList();

            // Assert
            result.Should().Equal(new Topic(topic1), new Topic(topic3));
        }

        [Fact(DisplayName = "KafkaServiceClient can't delete null topic.")]
        [Trait("Category", "Unit")]
        public async Task KafkaServiceClientCantDeleteNullTopic()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
            var optinosMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
            var clientMock = new Mock<IAdminClient>();
            optinosMock.Setup(x => x.Value).Returns(new KafkaServiceClientConfiguration
            {
            });
            var client = new KafkaServiceClient(loggerMock.Object, optinosMock.Object, clientMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                                                        await client.DeleteTopicAsync(null!).ConfigureAwait(false)
                                                       ).ConfigureAwait(false);

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "KafkaServiceClient can't delete reserved topic.")]
        [Trait("Category", "Unit")]
        public async Task KafkaServiceClientCantDeleteReservedTopic()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
            var optinosMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
            var clientMock = new Mock<IAdminClient>();
            var topicName = "reserved";
            optinosMock.Setup(x => x.Value).Returns(new KafkaServiceClientConfiguration
            {
                ReservedTopics = new List<string>()
                {
                  topicName
                }

            });
            var client = new KafkaServiceClient(loggerMock.Object, optinosMock.Object, clientMock.Object);

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                                                        await client.DeleteTopicAsync(new Topic(topicName)).ConfigureAwait(false)
                                                       ).ConfigureAwait(false);

            // Assert
            exception.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }
    }
}
