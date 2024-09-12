using System;

namespace ImmersiveCrosshair.Harmony
{
    public class Logger : ILogger
    {
        private const string LogFormat = "[ImmersiveCrosshair]: {0}";

        public void Info(string message)
        {
            LogMessage(Log.Out, message);
        }

        public void Debug(string message)
        {
#if DEBUG
            Info(message);
#endif
        }

        public void Warn(string message)
        {
#if DEBUG
            LogMessage(Log.Warning, message);
#endif
        }

        public void Error(string message)
        {
#if DEBUG
            LogMessage(Log.Error, message);
#endif
        }

        private static void LogMessage(Action<string> logAction, string message)
        {
            logAction(string.Format(LogFormat, message));
        }
    }
}