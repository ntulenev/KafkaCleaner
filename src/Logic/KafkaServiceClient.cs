using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Confluent.Kafka;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Models;
using Abstractions;

namespace Logic
{
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
        public KafkaServiceClient(ILogger<KafkaServiceClient> logger, IOptions<KafkaServiceClientConfiguration> config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _config = config.Value;

            _logger = logger;

            _kafkaConfig = new AdminClientConfig
            {
                BootstrapServers = string.Join(",", _config.BootstrapServers)
            };

            _logger.LogInformation("Instance created.");
        }

        /// <inheritdoc/>
        public IEnumerable<Topic> RequestTopicsList()
        {
            try
            {
                using var adminClient = new AdminClientBuilder(_kafkaConfig).Build();
                var metadata = adminClient.GetMetadata(_config.MetadataTimeout);
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
            _logger.LogInformation("Trying to remove topic {topic}.", topic.Name);

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
                using var adminClient = new AdminClientBuilder(_kafkaConfig).Build();
                await adminClient.DeleteTopicsAsync(new[] { topic.Name });

                _logger.LogInformation("Topic {topic} removed.", topic.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on removing topic {topic}", topic.Name);
                throw;
            }
        }

        private readonly KafkaServiceClientConfiguration _config;
        private readonly AdminClientConfig _kafkaConfig;
        private readonly ILogger<KafkaServiceClient> _logger;
    }
}
