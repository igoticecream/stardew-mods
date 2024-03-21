using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace LocalZoom
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig config = new();

        public override void Entry(IModHelper helper)
        {
            // Load mod config
            this.config = helper.ReadConfig<ModConfig>();

            // Subscribe to the events
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.Saved += this.OnSaved;
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            // Change the zoom level and UI scale when the game is loaded
            this.ChangeZoomLevel(this.config.zoomLevel);
            this.ChangeUIScale(this.config.uiScale);
        }

        private void OnSaved(object? sender, SavedEventArgs e)
        {
            // Get current save zoom level and UI scale
            this.config.zoomLevel = Game1.options.zoomLevel;
            this.config.uiScale = Game1.options.uiScale;

            // Save configuration
            this.Helper.WriteConfig(this.config);
        }

        private void ChangeZoomLevel(float zoomLevel)
        {
            // Clamp the value within the range of Options.minZoom to Options.maxZoom
            Game1.options.desiredBaseZoomLevel = Math.Min(Options.maxZoom, Math.Max(Options.minZoom, zoomLevel));
        }

        private void ChangeUIScale(float uiScale)
        {
            // Clamp the value within the range of Options.minUIZoom to Options.maxUIZoom
            Game1.options.desiredUIScale = Math.Min(Options.maxUIZoom, Math.Max(Options.minUIZoom, uiScale));
        }
    }
}
