using PacketDotNet;
using SharpPcap;
using TelemetryDeviceV1.Deserialization;
namespace TelemetryDeviceV1.Dataflow.Stages
{
    public class BuilderStage : IBuilderStage
    {
        private static readonly int syncBytesCount = IcdConstants.SyncBytes.Length;

        public byte[] Build(RawCapture capture)
        {
            Packet packet = Packet.ParsePacket(capture.LinkLayerType, capture.Data);

            UdpPacket udp = packet.Extract<UdpPacket>();

            if (udp == null || udp.PayloadData == null || udp.PayloadData.Length < syncBytesCount)
            {
                return null!;
            }

            for (int i = 0; i < syncBytesCount; i++)
            {
                if (IcdConstants.SyncBytes[i] != udp.PayloadData[i])
                {
                    return null!;
                }
            }
            return udp.PayloadData.Skip(syncBytesCount).ToArray();
        }
    }
}
