using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Tools;

namespace ZoomControl
{
    internal sealed class ModEntry : Mod
    {
        private const float INTERVAL = 0.05f;

        private ModConfig Config = new();

        public override void Entry(IModHelper helper)
        {
            // Load mod config
            this.Config = this.Helper.ReadConfig<ModConfig>();

            // Subscribe to the events
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.GameLoop.Saved += this.OnSaved;
            helper.Events.Input.MouseWheelScrolled += this.OnMouseWheelScrolled;
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // Get current save zoom level and UI scale
            this.Config.ZoomLevel = Game1.options.zoomLevel;
            this.Config.UiScale = Game1.options.uiScale;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // Get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetGenericModConfigMenuApi(this.Monitor);

            // Register mod
            configMenu?.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            // Add some config options
            configMenu?.AddNumberOption(
                mod: this.ModManifest,
                () => this.Config.ZoomLevel,
                (float val) => { this.Config.ZoomLevel = val; this.UpdateZoomLevel(val); },
                () => this.Helper.Translation.Get("settings.zoomlevel.name"),
                () => this.Helper.Translation.Get("settings.zoomlevel.tooltip"),
                min: Options.minZoom,
                max: Options.maxZoom,
                interval: INTERVAL,
                FormatPercentage
            );

            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.ZoomLevelKey,
                (KeybindList val) => this.Config.ZoomLevelKey = val,
                () => this.Helper.Translation.Get("settings.zoomlevelkey.name"),
                () => this.Helper.Translation.Get("settings.zoomlevelkey.tooltip")
            );

            configMenu?.AddNumberOption(
                mod: this.ModManifest,
                () => this.Config.UiScale,
                (float val) => { this.Config.UiScale = val; this.UpdateUIScale(val); },
                () => this.Helper.Translation.Get("settings.uiscale.name"),
                () => this.Helper.Translation.Get("settings.uiscale.tooltip"),
                min: Options.minUIZoom,
                max: Options.maxUIZoom,
                interval: INTERVAL,
                FormatPercentage
            );

            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.UiScaleKey,
                (KeybindList val) => this.Config.UiScaleKey = val,
                () => this.Helper.Translation.Get("settings.uiscalekey.name"),
                () => this.Helper.Translation.Get("settings.uiscalekey.tooltip")
            );
        }

        private void OnMouseWheelScrolled(object? sender, MouseWheelScrolledEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            // Ignore if player is not free to act
            if (!Context.IsPlayerFree)
                return;

            //Game1.exitActiveMenu();

            if (this.Config.ZoomLevelKey.IsDown())
            {
                if (e.Delta > 0)
                {
                    this.UpdateZoomLevelBy(+INTERVAL);

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? -1 : 1;
                }
                else if (e.Delta < 0)
                {
                    this.UpdateZoomLevelBy(-INTERVAL);

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? 1 : -1;
                }
            }
            if (this.Config.UiScaleKey.IsDown())
            {
                if (e.Delta > 0)
                {
                    this.UpdateUIScaleBy(+INTERVAL);

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? -1 : 1;
                }
                else if (e.Delta < 0)
                {
                    this.UpdateUIScaleBy(-INTERVAL);

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? 1 : -1;
                }
            }
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            // Change the zoom level and UI scale when the game is loaded
            this.UpdateZoomLevel(this.Config.ZoomLevel);
            this.UpdateUIScale(this.Config.UiScale);
        }

        private void OnSaved(object? sender, SavedEventArgs e)
        {
            // Save configuration
            this.Helper.WriteConfig(this.Config);
        }

        private void UpdateZoomLevelBy(float amount)
        {
            this.UpdateZoomLevel((float)Math.Round(Game1.options.zoomLevel + amount, 2));
        }

        private void UpdateZoomLevel(float value)
        {
            // Clamp the value within the range of Options.minZoom to Options.maxZoom
            value = (float)Math.Round(Math.Min(Options.maxZoom, Math.Max(Options.minZoom, value)), 2); // or RoundUp?

            // Update game if possible
            if (Context.IsWorldReady)
                Game1.options.desiredBaseZoomLevel = value;
        }

        private void UpdateUIScaleBy(float amount)
        {
            this.UpdateUIScale((float)Math.Round(Game1.options.uiScale + amount, 2));
        }

        private void UpdateUIScale(float value)
        {
            // Clamp the value within the range of Options.minUIZoom to Options.maxUIZoom
            value = (float)Math.Round(Math.Min(Options.maxUIZoom, Math.Max(Options.minUIZoom, value)), 2); // or RoundUp?

            // Update game if possible
            if (Context.IsWorldReady)
                Game1.options.desiredUIScale = value;
        }

        private static string FormatPercentage(float val)
        {
            return $"{val:0.#%}";
        }
    }
}
