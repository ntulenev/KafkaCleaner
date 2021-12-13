# KafkaCleaner
Helper utility for delete topics in Apache Kafka. 

![Application](App.png)

Allows to ignore system or reserved topics.

```yaml
{
 "BootstrapConfiguration": {
    "BootstrapServers": [
      "kafka:9092"
    ],
    "Username": "user123",
    "Password": "pwd123",
    "SecurityProtocol": "SaslPlaintext",
    "SASLMechanism": "ScramSha512"
  },
  "KafkaServiceClientConfiguration": {
    "MetadataTimeout": "00:00:05",
    "ReservedTopics": [
      "__consumer_offsets"
    ]
  }
}
```

| Parameter name | Description   |
| -------------- | ------------- |
| BootstrapServers | List of kafka cluster servers, like "kafka-test:9092"  |
| Username | SASL username (optional)  |
| Password | SASL password (optional)  |
| SecurityProtocol | Protocol used to communicate with brokers (Plaintext,Ssl,SaslPlaintext,SaslSsl) (optional)  |
| SASLMechanism | SASL mechanism to use for authentication (Gssapi,Plain,ScramSha256,ScramSha512,OAuthBearer) (optional)  |

Also this application could be considered as example of how to use Microsoft DI and Serilog in Windows Forms App.
