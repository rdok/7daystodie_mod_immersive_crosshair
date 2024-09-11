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
            _logger.Debug("Loading Patch: Requesting component of type " + typeof(T).Name);

            if (typeof(T) != typeof(INGuiWdwInGameHUD))
            {
                _logger.Debug(
                    "Loading Patch: Type is not IGuiWdwInGameHUD, proceeding with generic component retrieval.");
                var component = _localPlayerUI.GetComponentInChildren<T>();

                _logger.Debug(component == null
                    ? "Loading Patch: Component of type " + typeof(T).Name + " not found."
                    : "Loading Patch: Component of type " + typeof(T).Name + " found.");

                return component;
            }

            _logger.Debug("Loading Patch: Type is IGuiWdwInGameHUD, attempting to retrieve NGuiWdwInGameHUD.");
            var nGuiComponent = _localPlayerUI.GetComponentInChildren<NGuiWdwInGameHUD>();

            _logger.Debug(nGuiComponent == null
                ? "Loading Patch: NGuiWdwInGameHUD component not found."
                : "Loading Patch: NGuiWdwInGameHUD component found with type: " + nGuiComponent.GetType().Name);

            if (nGuiComponent != null)
            {
                _logger.Debug("Loading Patch: Wrapping NGuiWdwInGameHUD with IGuiWdwInGameHUD interface.");
                return (T)(object)new NInGuiWdwInGameHUDAdapter(nGuiComponent);
            }

            _logger.Debug("Loading Patch: Failed to wrap NGuiWdwInGameHUD. Returning null.");
            return default;
        }

        public XUi xui => _localPlayerUI.xui;
        public IGUIWindowManager windowManager => new GUIWindowManagerSeam(_localPlayerUI.windowManager);
    }
}