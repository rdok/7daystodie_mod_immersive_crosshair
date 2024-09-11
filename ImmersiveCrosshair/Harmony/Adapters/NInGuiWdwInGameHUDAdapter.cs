using System;
using System.Linq;
using UnityEngine;

namespace ImmersiveCrosshair.Harmony.Adapters
{
    public class NInGuiWdwInGameHUDAdapter : INGuiWdwInGameHUD
    {
        private readonly NGuiWdwInGameHUD _nGuiWdwInGameHUD;

        public NInGuiWdwInGameHUDAdapter(NGuiWdwInGameHUD nGuiWdwInGameHUD)
        {
            _nGuiWdwInGameHUD = nGuiWdwInGameHUD;
        }

        public bool showCrosshair
        {
            get => _nGuiWdwInGameHUD.showCrosshair;
            set => _nGuiWdwInGameHUD.showCrosshair = value;
        }
    }
}