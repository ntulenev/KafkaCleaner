namespace Logic.Configuration
{
    /// <summary>
    /// bootstrap servers config
    /// </summary>
    public class BootstrapConfiguration
    {
        /// <summary>
        /// List of bootstrap servers
        /// </summary>
        public List<string> BootstrapServers { get; set; } = default!;
    }
}
