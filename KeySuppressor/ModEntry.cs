using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace KeySuppressor
{
    public class ModEntry : Mod
    {

        #region Properties
        private ModConfig Config = new();
        private readonly int EmoteMenuShowTime = Game1.emoteMenuShowTime;
        #endregion

        #region Entry
        public override void Entry(IModHelper helper)
        {
            // Load mod config
            this.Config = this.Helper.ReadConfig<ModConfig>();

            // Subscribe to the events
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Input.ButtonsChanged += this.SuppressKeys;
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            if (!Context.IsPlayerFree)
                return;

            if (this.Config.EmoteMenuKey.JustPressed())
                Game1.activeClickableMenu = new EmoteMenu();

            if (this.Config.QuestMenuKey.JustPressed())
                Game1.activeClickableMenu = new QuestLog();

            if (this.Config.MapMenuKey.JustPressed())
                Game1.activeClickableMenu = new GameMenu(GameMenu.mapTab);

            if (this.Config.CraftingMenuKey.JustPressed())
                Game1.activeClickableMenu = new GameMenu(GameMenu.craftingTab);

            if (this.Config.SpecialOrdersMenuKey.JustPressed())
                Game1.activeClickableMenu = new SpecialOrdersBoard();
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            Game1.emoteMenuShowTime = this.Config.InstantEmoteMenu ? 1 : this.EmoteMenuShowTime;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // Get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetGenericModConfigMenuApi(this.Monitor);

            configMenu?.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            configMenu?.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "DPad Buttons"
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "DPad Up",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.DPadUp]),
                setValue: value => this.Config.SuppressedKeys[SButton.DPadUp] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "DPad Down",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.DPadDown]),
                setValue: value => this.Config.SuppressedKeys[SButton.DPadDown] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "DPad Left",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.DPadLeft]),
                setValue: value => this.Config.SuppressedKeys[SButton.DPadLeft] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "DPad Right",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.DPadRight]),
                setValue: value => this.Config.SuppressedKeys[SButton.DPadRight] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Face Buttons"
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "A",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.ControllerA]),
                setValue: value => this.Config.SuppressedKeys[SButton.ControllerA] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "B",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.ControllerB]),
                setValue: value => this.Config.SuppressedKeys[SButton.ControllerB] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "X",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.ControllerX]),
                setValue: value => this.Config.SuppressedKeys[SButton.ControllerX] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Y",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.ControllerY]),
                setValue: value => this.Config.SuppressedKeys[SButton.ControllerY] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Analog Thumbsticks"
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Left Stick Button",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.LeftStick]),
                setValue: value => this.Config.SuppressedKeys[SButton.LeftStick] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Left Thumbstick Up",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.LeftThumbstickUp]),
                setValue: value => this.Config.SuppressedKeys[SButton.LeftThumbstickUp] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Left Thumbstick Down",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.LeftThumbstickDown]),
                setValue: value => this.Config.SuppressedKeys[SButton.LeftThumbstickDown] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Left Thumbstick Left",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.LeftThumbstickLeft]),
                setValue: value => this.Config.SuppressedKeys[SButton.LeftThumbstickLeft] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Left Thumbstick Right",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.LeftThumbstickRight]),
                setValue: value => this.Config.SuppressedKeys[SButton.LeftThumbstickRight] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Right Stick Button",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.RightStick]),
                setValue: value => this.Config.SuppressedKeys[SButton.RightStick] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Right Thumbstick Up",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.RightThumbstickUp]),
                setValue: value => this.Config.SuppressedKeys[SButton.RightThumbstickUp] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Right Thumbstick Down",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.RightThumbstickDown]),
                setValue: value => this.Config.SuppressedKeys[SButton.RightThumbstickDown] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Right Thumbstick Left",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.RightThumbstickLeft]),
                setValue: value => this.Config.SuppressedKeys[SButton.RightThumbstickLeft] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Right Thumbstick Right",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.RightThumbstickRight]),
                setValue: value => this.Config.SuppressedKeys[SButton.RightThumbstickRight] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Trigger/Shoulder Buttons"
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Left Trigger",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.LeftTrigger]),
                setValue: value => this.Config.SuppressedKeys[SButton.LeftTrigger] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Left Shoulder",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.LeftShoulder]),
                setValue: value => this.Config.SuppressedKeys[SButton.LeftShoulder] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Right Trigger",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.RightTrigger]),
                setValue: value => this.Config.SuppressedKeys[SButton.RightTrigger] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Right Shoulder",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.RightShoulder]),
                setValue: value => this.Config.SuppressedKeys[SButton.RightShoulder] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Menu Buttons"
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Start",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.ControllerStart]),
                setValue: value => this.Config.SuppressedKeys[SButton.ControllerStart] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Back",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.ControllerBack]),
                setValue: value => this.Config.SuppressedKeys[SButton.ControllerBack] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            configMenu?.AddTextOption(
                mod: this.ModManifest,
                name: () => "Big Button",
                getValue: () => SuppressModeExtensions.ToString(this.Config.SuppressedKeys[SButton.BigButton]),
                setValue: value => this.Config.SuppressedKeys[SButton.BigButton] = SuppressModeExtensions.ToMode(value),
                allowedValues: SuppressModeExtensions.Names
            );

            // ------------
            // Keybinds
            // ------------
            configMenu?.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Keybinds"
            );

            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.EmoteMenuKey,
                (KeybindList val) => this.Config.EmoteMenuKey = val,
                () => "Emote",
                () => "Placeholder"
            );
            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.QuestMenuKey,
                (KeybindList val) => this.Config.QuestMenuKey = val,
                () => "Quest",
                () => "Placeholder"
            );
            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.MapMenuKey,
                (KeybindList val) => this.Config.MapMenuKey = val,
                () => "Map",
                () => "Placeholder"
            );
            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.CraftingMenuKey,
                (KeybindList val) => this.Config.CraftingMenuKey = val,
                () => "Crafting",
                () => "Placeholder"
            );
            configMenu?.AddKeybindList(
                mod: this.ModManifest,
                () => this.Config.SpecialOrdersMenuKey,
                (KeybindList val) => this.Config.SpecialOrdersMenuKey = val,
                () => "Special Orders",
                () => "Placeholder"
            );

            // ------------
            // Misc
            // ------------
            configMenu?.AddSectionTitle(
                mod: this.ModManifest,
                text: () => "Misc"
            );

            configMenu?.AddBoolOption(
                mod: this.ModManifest,
                () => this.Config.InstantEmoteMenu,
                (bool val) => this.Config.InstantEmoteMenu = val,
                () => "Instant Emote Menu",
                () => "Placeholder"
            );
        }
        #endregion

        // Set very low (even lower than low) event priority so that this event is handled after all other mods have handled that event. That way the keys are only suppressed for the base game functionality.
        [EventPriority(EventPriority.Low - 10000)]
        private void SuppressKeys(object? sender, ButtonsChangedEventArgs e)
        {
            foreach (var keyValue in this.Config.SuppressedKeys)
            {
                switch (keyValue.Value)
                {
                    case SuppressMode.SuppressOnlyInMenu when Context.IsWorldReady && Game1.activeClickableMenu != null:
                    case SuppressMode.SuppressOnlyWhenPlayerFree when Context.IsPlayerFree:
                    case SuppressMode.SuppressOnlyWhenPlayerCanMove when Context.CanPlayerMove:
                    case SuppressMode.Suppress:
                        this.Helper.Input.SuppressActiveKeybinds(KeybindList.ForSingle(keyValue.Key));
                        break;
                }
            }
        }
    }
}
