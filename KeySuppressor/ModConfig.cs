using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace KeySuppressor
{
    public enum SuppressMode
    {
        DoNotSuppress,
        Suppress,
        SuppressOnlyInMenu,
        SuppressOnlyWhenPlayerFree,
        SuppressOnlyWhenPlayerCanMove
    }

    public static class SuppressModeExtensions
    {
        public static string[] Names = Enum.GetValues(typeof(SuppressMode))
            .Cast<SuppressMode>()
            .ToList()
            .Select(x => ToString(x))
            .ToArray();

        public static string ToString(this SuppressMode mode)
        {
            return mode switch
            {
                SuppressMode.DoNotSuppress => "DoNotSuppress",
                SuppressMode.Suppress => "Suppress",
                SuppressMode.SuppressOnlyInMenu => "SuppressOnlyInMenu",
                SuppressMode.SuppressOnlyWhenPlayerFree => "SuppressOnlyWhenPlayerFree",
                SuppressMode.SuppressOnlyWhenPlayerCanMove => "SuppressOnlyWhenPlayerCanMove",
                _ => throw new System.Exception("Invalid mode!")
            };
        }

        public static SuppressMode ToMode(this string mode)
        {
            return mode switch
            {
                "DoNotSuppress" => SuppressMode.DoNotSuppress,
                "Suppress" => SuppressMode.Suppress,
                "SuppressOnlyInMenu" => SuppressMode.SuppressOnlyInMenu,
                "SuppressOnlyWhenPlayerFree" => SuppressMode.SuppressOnlyWhenPlayerFree,
                "SuppressOnlyWhenPlayerCanMove" => SuppressMode.SuppressOnlyWhenPlayerCanMove,
                _ => throw new System.Exception("Invalid mode!")
            };
        }
    }

    class ModConfig
    {

        public Dictionary<SButton, SuppressMode> SuppressedKeys { get; set; } = new Dictionary<SButton, SuppressMode>
        {
            { SButton.DPadUp,               SuppressMode.DoNotSuppress },
            { SButton.DPadDown,             SuppressMode.DoNotSuppress },
            { SButton.DPadLeft,             SuppressMode.DoNotSuppress },
            { SButton.DPadRight,            SuppressMode.DoNotSuppress },
            { SButton.ControllerA,          SuppressMode.DoNotSuppress },
            { SButton.ControllerB,          SuppressMode.DoNotSuppress },
            { SButton.ControllerX,          SuppressMode.DoNotSuppress },
            { SButton.ControllerY,          SuppressMode.DoNotSuppress },
            { SButton.LeftStick,            SuppressMode.DoNotSuppress },
            { SButton.LeftThumbstickUp,     SuppressMode.DoNotSuppress },
            { SButton.LeftThumbstickDown,   SuppressMode.DoNotSuppress },
            { SButton.LeftThumbstickLeft,   SuppressMode.DoNotSuppress },
            { SButton.LeftThumbstickRight,  SuppressMode.DoNotSuppress },
            { SButton.RightStick,           SuppressMode.DoNotSuppress },
            { SButton.RightThumbstickUp,    SuppressMode.DoNotSuppress },
            { SButton.RightThumbstickDown,  SuppressMode.DoNotSuppress },
            { SButton.RightThumbstickLeft,  SuppressMode.DoNotSuppress },
            { SButton.RightThumbstickRight, SuppressMode.DoNotSuppress },
            { SButton.LeftShoulder,         SuppressMode.DoNotSuppress },
            { SButton.LeftTrigger,          SuppressMode.DoNotSuppress },
            { SButton.RightShoulder,        SuppressMode.DoNotSuppress },
            { SButton.RightTrigger,         SuppressMode.DoNotSuppress },
            { SButton.ControllerBack,       SuppressMode.DoNotSuppress },
            { SButton.ControllerStart,      SuppressMode.DoNotSuppress },
            { SButton.BigButton,            SuppressMode.DoNotSuppress }
        };

        public KeybindList EmoteMenuKey { get; set; } = KeybindList.Parse("None");
        public KeybindList QuestMenuKey { get; set; } = KeybindList.Parse("None");
        public KeybindList MapMenuKey { get; set; } = KeybindList.Parse("None");
        public KeybindList CraftingMenuKey { get; set; } = KeybindList.Parse("None");

        public bool InstantEmoteMenu = false;
    }
}
