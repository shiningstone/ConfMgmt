
using System.Collections.Generic;
using System.Diagnostics;

namespace Utils
{
    public class TaskWatch
    {
        private static Dictionary<string, Stopwatch> watches = new Dictionary<string, Stopwatch>();
        private static Logger _defLog = new Logger("TaskWatch");
        private static Logger _dbgLog = new Logger("DbgTaskWatch");

        public static void Start(string taskName)
        {
            watches[taskName] = Stopwatch.StartNew();
        }
        public static int Stop(string taskName, Logger log = null)
        {
            watches[taskName].Stop();

            var logger = log != null ? log : new Logger("TaskWatch");
            logger.Debug($"{taskName} : {watches[taskName].ElapsedMilliseconds:F3}");

            return (int)watches[taskName].ElapsedMilliseconds;
        }

        public static int DbgStop(string taskName, Logger log = null)
        {
            var logger = log != null ? log : _dbgLog;
            return Stop(taskName, logger);
        }
    }
}
