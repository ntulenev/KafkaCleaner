using System;
using System.Collections.Generic;

namespace Logic
{
    public class KafkaServiceClientConfiguration
    {
        public List<string> BootstrapServers { get; set; } = default!;
        public List<string> ReservedTopics { get; set; } = default!;
        public TimeSpan MetadataTimeout { get; set; }
    }
}
