using System.Text.Json;
using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.Logging;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class KafkaStage : IKafkaStage
    {
        private readonly TelemetryProducer _producer;

        private readonly ILoggerTD _logger;

        public KafkaStage(TelemetryProducer producer, DelayedLogger logger)
        {
            _producer = producer;

            _logger = logger;
        }

        public void FlushProducer()
        {
            _producer.Flush();
        }

        public void Transmit(List<ParameterData> parameterValues)
        {
            string json = JsonSerializer.Serialize(parameterValues);

            _producer.SendTelemetryAsync(json);
        }
    }
}