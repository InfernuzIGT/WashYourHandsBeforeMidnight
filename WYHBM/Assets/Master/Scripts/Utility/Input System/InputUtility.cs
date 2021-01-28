using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

public enum DEVICE
{
    PC = 0,
    Generic = 1,
    PS4 = 2,
    XboxOne = 3,
    SwitchJoyCon = 4,
    SwitchPro = 5,
    Steam = 6,
    Xbox360 = 7,
    PS3 = 8
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

public static class InputUtility
{

    public static bool debugMode = false;

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
            PrintStartDevice(DEVICE.SwitchJoyCon, switchpro);
            return DEVICE.SwitchJoyCon;
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
                PrintCurrentDevice(DEVICE.SwitchJoyCon, InputSystem.devices[i]);
                return DEVICE.SwitchJoyCon;
            }
            if (ContainsDeviceName(Devices.Name.JOYSTICK, InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.Generic, InputSystem.devices[i]);
                return DEVICE.Generic;
            }
            if (ContainsDeviceName(Devices.Name.KEYBOARD, InputSystem.devices[i]))
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
                PrintCurrentDevice(DEVICE.SwitchJoyCon, device);
                return DEVICE.SwitchJoyCon;

            case Devices.NativeName.Keyboard:
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
            PrintCurrentDevice(DEVICE.SwitchJoyCon, device);
            return DEVICE.SwitchJoyCon;
        }
        if (ContainsDeviceName(Devices.Name.JOYSTICK, device))
        {
            PrintCurrentDevice(DEVICE.Generic, device);
            return DEVICE.Generic;
        }

        if (ContainsDeviceName(Devices.Name.KEYBOARD, device))
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
    }
    
    private static void PrintStartDevice(DEVICE device, InputDevice data = null, string description = null)
    {

#if UNITY_EDITOR

        if (!debugMode)return;

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

        if (!debugMode)return;

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