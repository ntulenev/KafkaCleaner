using Confluent.Kafka;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Models;
using Abstractions;
using Logic.Configuration;

namespace Logic;

/// <summary>
/// Class that responsible for communication with Kafka.
/// </summary>
public class KafkaServiceClient : IKafkaServiceClient
{
    /// <summary>
    /// Creates <see cref="KafkaServiceClient"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="config">Settings.</param>
    /// <param name="config">Kafka admin client.</param>
    public KafkaServiceClient(ILogger<KafkaServiceClient> logger,
                              IOptions<KafkaServiceClientConfiguration> config,
                              IAdminClient adminClient)
    {
        if (config is null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        if (adminClient is null)
        {
            throw new ArgumentNullException(nameof(adminClient));
        }

        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        _config = config.Value;
        _adminClient = adminClient;
        _logger = logger;

        _logger.LogInformation("Instance created.");
    }

    /// <inheritdoc/>
    public IEnumerable<Topic> RequestTopicsList()
    {
        try
        {
            var metadata = _adminClient.GetMetadata(_config.MetadataTimeout);
            var topicsMetadata = metadata.Topics;

            return metadata.Topics.Select(a => a.Topic)
                                  .Where(x => !_config.ReservedTopics.Contains(x))
                                  .OrderBy(x => x)
                                  .Select(x => new Topic(x))
                                  .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on loading topics.");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteTopicAsync(Topic topic)
    {
        if (topic is null)
        {
            throw new ArgumentNullException(nameof(topic));
        }

        if (_config.ReservedTopics.Contains(topic.Name))
        {
            throw new InvalidOperationException($"Unable to delete reserved topic - {topic.Name}.");
        }

        try
        {
            _logger.LogInformation("Trying to remove topic {topic}.", topic.Name);

            await _adminClient.DeleteTopicsAsync([topic.Name]);

            _logger.LogInformation("Topic {topic} removed.", topic.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on removing topic {topic}", topic.Name);
            throw;
        }
    }

    private readonly KafkaServiceClientConfiguration _config;
    private readonly IAdminClient _adminClient;
    private readonly ILogger<KafkaServiceClient> _logger;
}
