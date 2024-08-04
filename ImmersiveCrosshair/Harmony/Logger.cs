namespace ImmersiveCrosshair.Harmony
{
    public class Logger : ILogger
    {
        public void Info(string message)
        {
#if DEBUG
            Log.Out("[ImmersiveCrosshair]: " + message);
#endif
        }

        public void Warn(string message)
        {
#if DEBUG
            Log.Warning("[ImmersiveCrosshair]: " + message);
#endif
        }

        public void Error(string message)
        {
#if DEBUG
            Log.Error("[ImmersiveCrosshair]: " + message);
#endif
        }
    }

    public interface ILogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}