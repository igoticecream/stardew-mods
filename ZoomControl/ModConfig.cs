using StardewModdingAPI.Utilities;

namespace ZoomControl
{
    internal class ModConfig
    {
        public float ZoomLevel { get; set; } = 1.0f;
        public KeybindList ZoomLevelKey { get; set; } = KeybindList.Parse("LeftShift");
        public float UiScale { get; set; } = 1.0f;
        public KeybindList UiScaleKey { get; set; } = KeybindList.Parse("LeftControl");
    }
}
