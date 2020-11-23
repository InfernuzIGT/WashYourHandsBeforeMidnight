namespace UnityEngine.InputSystem
{
    public static class Devices
    {
        public static class NativeName
        {
            // Playstation
            public const string DualShockGamepad = "DualShockGamepad";
            public const string DualShock3GamepadHID = "DualShock3GamepadHID";
            public const string DualShock4GamepadHID = "DualShock4GamepadHID";

            // Xbox
            public const string XInputController = "XInputController";
            public const string XInputControllerWindows = "XInputControllerWindows";

            // Nintendo
            public const string SwitchProControllerHID = "SwitchProControllerHID";
            
            // PC
            public const string Keyboard = "Keyboard";
        }

        public static class Name
        {
            public const string PS4 = "ps4";
            public const string XBOX = "xbox";
            public const string JOYSTICK = "joystick";
            public const string SWITCH = "switch";
            public const string KEYBOARD = "keyboard";
        }

    }
}