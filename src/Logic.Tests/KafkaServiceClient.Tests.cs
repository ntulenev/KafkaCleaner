using Confluent.Kafka;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

using FluentAssertions;

using Logic.Configuration;
using Models;

namespace Logic.Tests;

public class KafkaServiceClientTests
{
    [Fact(DisplayName = "KafkaServiceClient can be created.")]
    [Trait("Category", "Unit")]
    public void KafkaServiceClientCanBeCreatedWithValidParams()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
        var optionsMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
        var clientMock = new Mock<IAdminClient>();

        // Act
        var exception = Record.Exception(() => new KafkaServiceClient(loggerMock.Object, optionsMock.Object, clientMock.Object));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "KafkaServiceClient cant be created with null config.")]
    [Trait("Category", "Unit")]
    public void KafkaServiceClientCantBeCreatedWithNullConfig()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
        var optionsMock = (IOptions<KafkaServiceClientConfiguration>)null!;
        var clientMock = new Mock<IAdminClient>();

        // Act
        var exception = Record.Exception(() => new KafkaServiceClient(loggerMock.Object, optionsMock, clientMock.Object));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "KafkaServiceClient cant be created with null admin tool.")]
    [Trait("Category", "Unit")]
    public void KafkaServiceClientCantBeCreatedWithNullAdminTool()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
        var optionsMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
        var clientMock = (IAdminClient)null!;

        // Act
        var exception = Record.Exception(() => new KafkaServiceClient(loggerMock.Object, optionsMock.Object, clientMock));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "KafkaServiceClient cant be created with null logger.")]
    [Trait("Category", "Unit")]
    public void KafkaServiceClientCantBeCreatedWithNullLogger()
    {
        // Arrange
        var loggerMock = (ILogger<KafkaServiceClient>)null!;
        var optionsMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
        var clientMock = new Mock<IAdminClient>();

        // Act
        var exception = Record.Exception(() => new KafkaServiceClient(loggerMock, optionsMock.Object, clientMock.Object));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }


    [Fact(DisplayName = "KafkaServiceClient can request topic.")]
    [Trait("Category", "Unit")]
    public void KafkaServiceClientCanRequestTopics()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
        var optionsMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
        var clientMock = new Mock<IAdminClient>();
        var topic1 = "topic 1";
        var topicReserved = "topic reserved";
        var topic3 = "topic 3";
        var timeout = TimeSpan.FromSeconds(42);
        optionsMock.Setup(x => x.Value).Returns(new KafkaServiceClientConfiguration
        {
            ReservedTopics =
            [
                topicReserved
            ],
            MetadataTimeout = timeout
        });
        var brokerTopics = new List<TopicMetadata>()
        {
           new(topic1,[],null!),
           new(topicReserved,[],null!),
           new(topic3,[],null!),
        };
        var meta = new Metadata([], brokerTopics, 1, string.Empty);
        clientMock.Setup(x => x.GetMetadata(timeout)).Returns(meta);
        var client = new KafkaServiceClient(loggerMock.Object, optionsMock.Object, clientMock.Object);

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
        var optionsMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
        var clientMock = new Mock<IAdminClient>();
        optionsMock.Setup(x => x.Value).Returns(new KafkaServiceClientConfiguration
        {
        });
        var client = new KafkaServiceClient(loggerMock.Object, optionsMock.Object, clientMock.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
                                                    await client.DeleteTopicAsync(null!).ConfigureAwait(false)
                                                   );

        // Assert
        exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "KafkaServiceClient can't delete reserved topic.")]
    [Trait("Category", "Unit")]
    public async Task KafkaServiceClientCantDeleteReservedTopic()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
        var optionsMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
        var clientMock = new Mock<IAdminClient>();
        var topicName = "reserved";
        optionsMock.Setup(x => x.Value).Returns(new KafkaServiceClientConfiguration
        {
            ReservedTopics =
            [
              topicName
            ]

        });
        var client = new KafkaServiceClient(loggerMock.Object, optionsMock.Object, clientMock.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
                                                    await client.DeleteTopicAsync(new Topic(topicName)).ConfigureAwait(false)
                                                   );

        // Assert
        exception.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "KafkaServiceClient can delete topic.")]
    [Trait("Category", "Unit")]
    public async Task KafkaServiceClientCanDeleteTopic()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<KafkaServiceClient>>();
        var optionsMock = new Mock<IOptions<KafkaServiceClientConfiguration>>();
        var clientMock = new Mock<IAdminClient>(MockBehavior.Strict);
        var reservedTopic = "reserved";
        var targetTopic = "target";
        clientMock.Setup(x =>
                x.DeleteTopicsAsync(It.Is<IEnumerable<string>>(a => a.Single() == targetTopic),
                                    null!))
            .Returns(Task.CompletedTask);
        optionsMock.Setup(x => x.Value).Returns(new KafkaServiceClientConfiguration
        {
            ReservedTopics =
            [
              reservedTopic
            ]

        });
        var client = new KafkaServiceClient(loggerMock.Object, optionsMock.Object, clientMock.Object);

        // Act
        await client.DeleteTopicAsync(new Topic(targetTopic));


        // Assert
        clientMock.Verify(x => x.DeleteTopicsAsync(It.Is<IEnumerable<string>>(a => a.Single() == targetTopic),
                                                   null!), Times.Once);
    }
}
