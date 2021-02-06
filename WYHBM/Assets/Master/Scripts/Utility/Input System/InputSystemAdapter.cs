using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

// The names must match with Controls Schemes!
public enum DEVICE
{
    PC = 0,
    Generic = 1,
    PS4 = 2,
    XboxOne = 3,
    Switch = 4,
    Steam = 5,
    Xbox360 = 6,
    PS3 = 7
}

public enum INPUT_ACTION
{
    None = 0,
    Interact = 1,
    Attack = 2,
    Jump = 3,
    Crouch = 4,
    Pause = 5,
    Options = 6,
    Select = 7,
    Back = 8,
    Move = 9,
    Look = 10,
    Zoom = 11,
    // TODO Mariano: Complete
}

public enum RUMBLE_TYPE
{
    None = 0,
    Options = 1,
    Attack = 2,
    Hit = 3,
}

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
        public const string Mouse = "Mouse";
    }

    public static class Name
    {
        public const string PS4 = "ps4";
        public const string XBOX = "xbox";
        public const string JOYSTICK = "joystick";
        public const string SWITCH = "switch";
        public const string KEYBOARD = "keyboard";
        public const string MOUSE = "mouse";
    }

}

public static class InputSystemAdapter
{

    public static bool printInfo = false;

    public static bool ContainsDeviceName(string name, InputDevice device)
    {
        return device.displayName.ToLower().Contains(name);
    }

    public static DEVICE GetStartDevice()
    {
        // Playstation
        var dualshock = DualShockGamepad.current;
        if (dualshock != null)
        {
            PrintStartDevice(DEVICE.PS4, dualshock, Devices.NativeName.DualShockGamepad);
            return DEVICE.PS4;
        }
        dualshock = DualShock3GamepadHID.current;
        if (dualshock != null)
        {
            PrintStartDevice(DEVICE.PS4, dualshock, Devices.NativeName.DualShock3GamepadHID);
            return DEVICE.PS4;
        }
        dualshock = DualShock4GamepadHID.current;
        if (dualshock != null)
        {
            PrintStartDevice(DEVICE.PS4, dualshock, Devices.NativeName.DualShock4GamepadHID);
            return DEVICE.PS4;
        }

        // Xbox
        var xbox = XInputController.current;
        if (xbox != null)
        {
            PrintStartDevice(DEVICE.XboxOne, xbox, Devices.NativeName.XInputController);
            return DEVICE.XboxOne;
        }
        xbox = XInputControllerWindows.current;
        if (xbox != null)
        {
            PrintStartDevice(DEVICE.XboxOne, xbox, Devices.NativeName.XInputControllerWindows);
            return DEVICE.XboxOne;
        }

        // Nintendo
        var switchpro = SwitchProControllerHID.current;
        if (switchpro != null)
        {
            PrintStartDevice(DEVICE.Switch, switchpro);
            return DEVICE.Switch;
        }

        // Generic
        var joystick = Joystick.current;
        if (joystick != null)
        {
            PrintStartDevice(DEVICE.Generic, joystick);
            return DEVICE.Generic;
        }

        PrintStartDevice(DEVICE.PC);
        return DEVICE.PC;
    }

    public static Gamepad GetCurrentGamepad()
    {
        return Gamepad.current;
    }

    public static DEVICE GetCurrentDevice()
    {
        for (int i = 0; i < InputSystem.devices.Count; i++)
        {
            if (ContainsDeviceName(Devices.Name.PS4, InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.PS4, InputSystem.devices[i]);
                return DEVICE.PS4;
            }
            if (ContainsDeviceName(Devices.Name.XBOX, InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.XboxOne, InputSystem.devices[i]);
                return DEVICE.XboxOne;
            }

            if (ContainsDeviceName(Devices.Name.SWITCH, InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.Switch, InputSystem.devices[i]);
                return DEVICE.Switch;
            }
            if (ContainsDeviceName(Devices.Name.JOYSTICK, InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.Generic, InputSystem.devices[i]);
                return DEVICE.Generic;
            }
            if (ContainsDeviceName(Devices.Name.KEYBOARD, InputSystem.devices[i]) ||
                ContainsDeviceName(Devices.Name.MOUSE, InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.PC);
                return DEVICE.PC;
            }
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't detect device: NULL");
        return DEVICE.PC;
    }

    public static DEVICE GetCurrentDevice(InputDevice device)
    {
        switch (device.name)
        {
            case Devices.NativeName.DualShockGamepad:
            case Devices.NativeName.DualShock3GamepadHID:
            case Devices.NativeName.DualShock4GamepadHID:
                PrintCurrentDevice(DEVICE.PS4, device);
                return DEVICE.PS4;

            case Devices.NativeName.XInputController:
            case Devices.NativeName.XInputControllerWindows:
                PrintCurrentDevice(DEVICE.XboxOne, device);
                return DEVICE.XboxOne;

            case Devices.NativeName.SwitchProControllerHID:
                PrintCurrentDevice(DEVICE.Switch, device);
                return DEVICE.Switch;

            case Devices.NativeName.Keyboard:
            case Devices.NativeName.Mouse:
                PrintCurrentDevice(DEVICE.PC);
                return DEVICE.PC;
        }
        if (ContainsDeviceName(Devices.Name.PS4, device))
        {
            PrintCurrentDevice(DEVICE.PS4, device);
            return DEVICE.PS4;
        }
        if (ContainsDeviceName(Devices.Name.XBOX, device))
        {
            PrintCurrentDevice(DEVICE.XboxOne, device);
            return DEVICE.XboxOne;
        }
        if (ContainsDeviceName(Devices.Name.SWITCH, device))
        {
            PrintCurrentDevice(DEVICE.Switch, device);
            return DEVICE.Switch;
        }
        if (ContainsDeviceName(Devices.Name.JOYSTICK, device))
        {
            PrintCurrentDevice(DEVICE.Generic, device);
            return DEVICE.Generic;
        }
        if (ContainsDeviceName(Devices.Name.KEYBOARD, device) ||
            ContainsDeviceName(Devices.Name.MOUSE, device))
        {
            PrintCurrentDevice(DEVICE.PC);
            return DEVICE.PC;
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't detect device: {device}");
        return DEVICE.PC;
    }

    public static void DeviceRebind(DEVICE device, CustomInputAction inputAction)
    {
        inputAction.bindingMask = InputBinding.MaskByGroup(device.ToString());

        if (!printInfo)return;

        string color = "orange";
        Debug.Log($"<color={color}><b> Device Rebind: </b></color> {device.ToString()}");
    }

    private static void PrintStartDevice(DEVICE device, InputDevice data = null, string description = null)
    {

#if UNITY_EDITOR

        if (!printInfo)return;

        string color = "white";

        if (data == null)
        {
            Debug.Log($"<color={color}><b> Start Device: </b></color> {device.ToString()}");
        }
        else
        {
            if (description == null)
            {
                Debug.Log($"<color={color}><b> Start Device: </b></color> {device.ToString()} [{data.displayName}]");
            }
            else
            {
                Debug.Log($"<color={color}><b> Start Device: </b></color> {device.ToString()} - {description} [{data.displayName}]");
            }
        }

#endif

    }

    public static void PrintCurrentDevice(DEVICE device, InputDevice data = null)
    {

#if UNITY_EDITOR

        if (!printInfo)return;

        string color = "green";

        if (data == null)
        {
            Debug.Log($"<color={color}><b> Current Device: </b></color> {device.ToString()}");
        }
        else
        {
            Debug.Log($"<color={color}><b> Current Device: </b></color> {device.ToString()} [{data.displayName}]");
        }

#endif

    }

}