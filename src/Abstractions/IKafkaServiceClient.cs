using Models;

namespace Abstractions;

/// <summary>
/// Represents communication with Apache Kafka.
/// </summary>
public interface IKafkaServiceClient
{
    /// <summary>
    /// Gets list of kafka tipics.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Topic> RequestTopicsList();

    /// <summary>
    /// Deletes kafka topic
    /// </summary>
    public Task DeleteTopicAsync(Topic topic);
}
