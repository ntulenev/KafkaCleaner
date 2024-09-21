namespace Logic.Configuration;

/// <summary>
/// Configuration for <see cref="KafkaServiceClient"/>.
/// </summary>
public class KafkaServiceClientConfiguration
{
    /// <summary>
    /// List of reserved topics (Hided and unable to be deleted).
    /// </summary>
    public List<string> ReservedTopics { get; set; } = default!;

    /// <summary>
    /// Timeout for metadata request.
    /// </summary>
    public TimeSpan MetadataTimeout { get; set; }
}
