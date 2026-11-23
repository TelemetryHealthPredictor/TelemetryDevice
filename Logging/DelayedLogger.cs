namespace TelemetryDeviceV1.Logging
{
    public class DelayedLogger : ILoggerTD
    {
        private readonly Dictionary<string, int> _messageCounts;

        public DelayedLogger()
        {
            _messageCounts = [];
        }

        public void Log(string message)
        {
            _messageCounts.TryAdd(message, 0);
            _messageCounts[message]++;

            if (_messageCounts[message] % 100 == 0)
            {
                Console.WriteLine($"{message} -> {_messageCounts[message]}");
            }
        }
    }
}
