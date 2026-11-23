namespace TelemetryDeviceV1.Logging
{
    public class CountingLogger : ILoggerTD
    {
        private readonly Dictionary<string, int> _messageCounts;

        public CountingLogger()
        {
            _messageCounts = new Dictionary<string, int>();
        }

        public void Log(string message)
        {
            _messageCounts.TryAdd(message, 0);
            _messageCounts[message]++;

            Console.WriteLine($"{message} - {_messageCounts[message]}");
        }
    }
}
