using Models;

namespace Abstractions;

/// <summary>
/// Represents communication with Apache Kafka.
/// </summary>
public interface IKafkaServiceClient
{
    /// <summary>
    /// Gets list of Kafka topics.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Topic> RequestTopicsList();

    /// <summary>
    /// Deletes Kafka topic
    /// </summary>
    public Task DeleteTopicAsync(Topic topic);
}
