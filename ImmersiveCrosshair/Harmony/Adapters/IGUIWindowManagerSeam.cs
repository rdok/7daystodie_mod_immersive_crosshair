using ImmersiveCrosshair.Harmony.Interfaces;

namespace ImmersiveCrosshair.Harmony.Seams
{
    public class GUIWindowManagerSeam : IGUIWindowManager
    {
        private readonly GUIWindowManager _windowManager;

        public GUIWindowManagerSeam(GUIWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public bool IsWindowOpen(string wdwID)
        {
            return _windowManager.IsWindowOpen(wdwID);
        }
    }
}