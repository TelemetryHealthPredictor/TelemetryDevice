using System.Numerics;
using TelemetryDeviceV1.ICD.Enums;

namespace TelemetryDeviceV1.Dataflow.Stages.Helpers
{
    public static class BitMaskService
    {
        private const int _floatSize = 8;
        private const int _byteBits = 8;

        public static double GetFloat64(byte[] data, byte[] mask)
        {
            ulong extractedBits = 0;

            for (int i = 0; i < data.Length; i++)
            {
                byte dataByte = data[i];
                byte maskByte = mask[i];

                for (int bit = 7; bit >= 0; bit--)
                {
                    if (((maskByte >> bit) & 1) == 1)
                    {
                        extractedBits = (extractedBits << 1) | (uint)((dataByte >> bit) & 1);
                    }
                }
            }

            return BitConverter.UInt64BitsToDouble(extractedBits);
        }

        public static int GetInt32(byte[] data, byte[] mask, ParameterDataType type)
        {
            int lowBitsEnd = 0;

            while (((mask[mask.Length - 1] >> lowBitsEnd) & 1) == 0)
            {
                lowBitsEnd++;
            }

            bool unsigned = true;
            if (type == ParameterDataType.Int16 || type == ParameterDataType.Int32)
            {
                unsigned = false;
            }

            BigInteger iData = new BigInteger(data, isBigEndian: true, isUnsigned: unsigned);
            BigInteger iMask = new BigInteger(mask, isBigEndian: true, isUnsigned: unsigned);

            BigInteger masked = iData & iMask;
            BigInteger result = masked >> lowBitsEnd;

            return checked((int)(result));
        }
    }
}
