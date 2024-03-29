using GenericModConfigMenu;
using Microsoft.Xna.Framework.Input;
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
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Input.MouseWheelScrolled += this.OnMouseWheelScrolled;
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
                (float val) => { this.Config.ZoomLevel = val; UpdateZoomLevel(this.Config.ZoomLevel); },
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

            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.ZoomLevelResetKey,
                (KeybindList val) => this.Config.ZoomLevelResetKey = val,
                () => this.Helper.Translation.Get("settings.zoomlevelresetkey.name"),
                () => this.Helper.Translation.Get("settings.zoomlevelresetkey.tooltip")
            );

            configMenu?.AddNumberOption(
                mod: this.ModManifest,
                () => this.Config.UiScale,
                (float val) => { this.Config.UiScale = val; UpdateUIScale(this.Config.UiScale); },
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

            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.UiScaleResetKey,
                (KeybindList val) => this.Config.UiScaleResetKey = val,
                () => this.Helper.Translation.Get("settings.uiscaleresetkey.name"),
                () => this.Helper.Translation.Get("settings.uiscaleresetkey.tooltip")
            );
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            // Ignore if player is not free to act
            if (!Context.IsPlayerFree)
                return;

            if (this.Config.ZoomLevelResetKey.JustPressed())
                UpdateZoomLevel(this.Config.ZoomLevel);

            if (this.Config.UiScaleResetKey.JustPressed())
                UpdateUIScale(this.Config.UiScale);
        }

        private void OnMouseWheelScrolled(object? sender, MouseWheelScrolledEventArgs e)
        {
            // Ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;
            // Ignore if player is not free to act
            if (!Context.IsPlayerFree)
                return;

            if (this.Config.ZoomLevelKey.IsDown())
            {
                if (e.Delta > 0)
                {
                    UpdateZoomLevel((float)Math.Round(Game1.options.zoomLevel + INTERVAL, 2));

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? -1 : 1;
                }
                else if (e.Delta < 0)
                {
                    UpdateZoomLevel((float)Math.Round(Game1.options.zoomLevel - INTERVAL, 2));

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? 1 : -1;
                }
            }
            if (this.Config.UiScaleKey.IsDown())
            {
                if (e.Delta > 0)
                {
                    UpdateUIScale((float)Math.Round(Game1.options.uiScale + INTERVAL, 2));

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? -1 : 1;
                }
                else if (e.Delta < 0)
                {
                    UpdateUIScale((float)Math.Round(Game1.options.uiScale - INTERVAL, 2));

                    if (!(Game1.player.UsingTool && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is FishingRod fishingRod) || (!fishingRod.isReeling && !fishingRod.pullingOutOfWater))))
                        Game1.player.CurrentToolIndex += Game1.options.invertScrollDirection ? 1 : -1;
                }
            }
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            // Change the zoom level and UI scale when the game is loaded
            UpdateZoomLevel(this.Config.ZoomLevel);
            UpdateUIScale(this.Config.UiScale);

            // General
            Game1.options.hideToolHitLocationWhenInMotion = false;
            Game1.options.pauseWhenOutOfFocus = false;
            Game1.options.showAdvancedCraftingInformation = true;
            Game1.options.runButton = new InputButton[1] { new(Keys.LeftControl) };
        }

        private static void UpdateZoomLevel(float value)
        {
            // Clamp the value within the range of Options.minZoom to Options.maxZoom
            value = (float)Math.Round(Math.Min(Options.maxZoom, Math.Max(Options.minZoom, value)), 2); // or RoundUp?

            // Update game if possible
            if (Context.IsWorldReady)
                Game1.options.desiredBaseZoomLevel = value;
        }

        private static void UpdateUIScale(float value)
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
