using ParameterDataLib;
using TelemetryDeviceV1.Dataflow.Stages.Helpers;
using TelemetryDeviceV1.ICD;
using TelemetryDeviceV1.Logging;

namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class DecoderStage : IDecoderStage
    {
        private readonly IcdDeserializer _deserializer;

        private readonly CountingLogger _logger;

        public DecoderStage(IcdDeserializer deserializer, CountingLogger logger)
        {
            _deserializer = deserializer;

            _logger = logger;
        }
        
        public IEnumerable<List<ParameterData>> Decode(byte[] data)
        {
            if (data is null)
            {
                return [];
            }

            PacketData packetData = new(data);

            List<ParameterData> result = [];

            foreach (IcdParameter icdParam in _deserializer.CorrelatorGroups[packetData.CorrelatorByte])
            {
                ParameterData paramData = packetData.GetParameterData(icdParam);

                if (paramData.Value > icdParam.Max || paramData.Value < icdParam.Min)
                {
                    if 
                    (
                        paramData.Name == "ABRK"
                        || paramData.Name == "AIL_1"
                        || paramData.Name == "AIL_2"
                        || paramData.Name == "SPL_1"
                        || paramData.Name == "SPL_2"
                        || paramData.Name == "LONP"
                    )
                    {
                        _logger.Log("Parameter not in ICD range");
                        _logger.Log(paramData.Name);
                        Console.WriteLine($"Value = {paramData.Value}");
                    }

                    continue;
                }

                result.Add(paramData);
            }

            return [result];
        }
    }
}
