# KafkaCleaner
Helper utility for cleaning data in Apache Kafka. 

![Application](App.png)

Allows to ignore system or reserved topics.

```yaml
{
  "KafkaServiceClientConfiguration": {
    "MetadataTimeout": "00:00:05",
    "BootstrapServers": [
      "kafka:9092"
    ],
    "ReservedTopics": [
      "__consumer_offsets"
    ]
  }
}
```
