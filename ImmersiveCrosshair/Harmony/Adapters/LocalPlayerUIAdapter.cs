using System;
using ImmersiveCrosshair.Harmony.Interfaces;
using ImmersiveCrosshair.Harmony.Seams;

namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class LocalPlayerUIAdapter : ILocalPlayerUI
    {
        private readonly LocalPlayerUI _localPlayerUI;
        private readonly ILogger _logger = new Logger();

        public LocalPlayerUIAdapter(LocalPlayerUI playerUI)
        {
            _localPlayerUI =
                playerUI ?? throw new ArgumentNullException(nameof(playerUI));
        }

        public T GetComponentInChildren<T>()
        {
            if (typeof(T) != typeof(INGuiWdwInGameHUD))
            {
                var component = _localPlayerUI.GetComponentInChildren<T>();
                return component;
            }

            var nGuiComponent = _localPlayerUI.GetComponentInChildren<NGuiWdwInGameHUD>();

            if (nGuiComponent != null)
            {
                return (T)(object)new NInGuiWdwInGameHUDAdapter(nGuiComponent);
            }

            _logger.Debug("Loading Patch: Failed to wrap NGuiWdwInGameHUD. Returning null.");
            return default;
        }

        public XUi xui => _localPlayerUI.xui;
        public IGUIWindowManager windowManager => new GUIWindowManagerSeam(_localPlayerUI.windowManager);
    }
}