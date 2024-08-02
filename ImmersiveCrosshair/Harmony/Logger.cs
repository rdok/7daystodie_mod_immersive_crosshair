namespace ImmersiveCrosshair.Harmony
{
    public class Logger : ILogger
    {
        public void Info(string message)
        {
            Log.Out(message);
        }
    }

    public interface ILogger
    {
        void Info(string message);
    }
}