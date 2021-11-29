namespace Logic.Configuration
{
    /// <summary>
    /// Configuration for <see cref="KafkaServiceClient"/>.
    /// </summary>
    public class KafkaServiceClientConfiguration
    {
        /// <summary>
        /// List of bootstrap servers
        /// </summary>
        public List<string> BootstrapServers { get; set; } = default!;

        /// <summary>
        /// List of reserved topics (Hided and unable to be deleted).
        /// </summary>
        public List<string> ReservedTopics { get; set; } = default!;

        /// <summary>
        /// Timout for metadata request.
        /// </summary>
        public TimeSpan MetadataTimeout { get; set; }
    }
}
