using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPublisher.Publisher
{
    using System.Diagnostics;
    using Confluent.Kafka;

    public class KafkaPublisher: IDisposable
    {
        private readonly IProducer<Null, string>? _p;

        private readonly string _topicName;

        public KafkaPublisher(string topicName)
        {
            _topicName = topicName; 

            var bootstrapServers = "localhost:29092"; //"localhost:9092";
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            _p = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task Publish(string message)
        {
            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            {
                try
                {
                    Debug.Assert(_p != null, nameof(_p) + " != null");
                    var dr = await _p.ProduceAsync(_topicName, new Message<Null, string> { Value = message });
                    Debug.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }

        private bool _disposed = false;

        public void Dispose()
        {
            if (_disposed) return;
            _p?.Dispose();
        }
    }
}
