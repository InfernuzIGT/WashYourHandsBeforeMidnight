using System.Collections;
using System.Collections.Generic;
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
    Switch = 4
}

public enum INPUT_ACTION
{
    None = 0,
    Interaction = 1,
    // TODO Mariano: Complete
}

public static class UniversalFunctions
{

    #region Input System

    public static bool ContainsDeviceName(string name, InputDevice device)
    {
        return device.displayName.ToLower().Contains(name.ToLower());
    }

    public static DEVICE GetStartDevice()
    {
        // Joystick
        var joystick = Joystick.current;
        if (joystick != null)
        {
            PrintStartDevice(DEVICE.Generic, joystick);
            return DEVICE.Generic;
        }

        // Switch
        var switchpro = SwitchProControllerHID.current;
        if (switchpro != null)
        {
            PrintStartDevice(DEVICE.Switch, switchpro);
            return DEVICE.Switch;
        }

        // PS4
        var dualshock = DualShockGamepad.current;
        if (dualshock != null)
        {
            PrintStartDevice(DEVICE.PS4, dualshock, "DualShockGamepad");
            return DEVICE.PS4;
        }
        dualshock = DualShock3GamepadHID.current;
        if (dualshock != null)
        {
            PrintStartDevice(DEVICE.PS4, dualshock, "DualShock3GamepadHID");
            return DEVICE.PS4;
        }
        dualshock = DualShock4GamepadHID.current;
        if (dualshock != null)
        {
            PrintStartDevice(DEVICE.PS4, dualshock, "DualShock4GamepadHID");
            return DEVICE.PS4;
        }

        // Xbox
        var xbox = XInputController.current;
        if (xbox != null)
        {
            PrintStartDevice(DEVICE.XboxOne, xbox, "XInputController");
            return DEVICE.XboxOne;
        }
        xbox = XInputControllerWindows.current;
        if (xbox != null)
        {
            PrintStartDevice(DEVICE.XboxOne, xbox, "XInputControllerWindows");
            return DEVICE.XboxOne;
        }

        PrintStartDevice(DEVICE.PC);
        return DEVICE.PC;
    }

    public static DEVICE GetCurrentDevice()
    {
        for (int i = 0; i < InputSystem.devices.Count; i++)
        {
            if (ContainsDeviceName("usb joystick", InputSystem.devices[i]) ||
                ContainsDeviceName("joystick", InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.Generic, InputSystem.devices[i]);
                return DEVICE.Generic;
            }
            if (ContainsDeviceName("switch pro controller", InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.Switch, InputSystem.devices[i]);
                return DEVICE.Switch;
            }
            if (ContainsDeviceName("ps4 controller", InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.PS4, InputSystem.devices[i]);
                return DEVICE.PS4;
            }
            if (ContainsDeviceName("xbox controller", InputSystem.devices[i]))
            {
                PrintCurrentDevice(DEVICE.XboxOne, InputSystem.devices[i]);
                return DEVICE.XboxOne;
            }
            if (ContainsDeviceName("keyboard", InputSystem.devices[i]))
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
        if (ContainsDeviceName("usb joystick", device) ||
            ContainsDeviceName("joystick", device))
        {
            PrintCurrentDevice(DEVICE.Generic, device);
            return DEVICE.Generic;
        }
        if (ContainsDeviceName("switch pro controller", device))
        {
            PrintCurrentDevice(DEVICE.Switch, device);
            return DEVICE.Switch;
        }
        if (ContainsDeviceName("ps4 controller", device))
        {
            PrintCurrentDevice(DEVICE.PS4, device);
            return DEVICE.PS4;
        }

        if (ContainsDeviceName("xbox controller", device))
        {
            PrintCurrentDevice(DEVICE.XboxOne, device);
            return DEVICE.XboxOne;
        }
        if (ContainsDeviceName("keyboard", device))
        {
            PrintCurrentDevice(DEVICE.PC);
            return DEVICE.PC;
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't detect device: {device}");
        return DEVICE.PC;
    }

    public static void DeviceRebind(InputActions inputAction, DEVICE device)
    {
        inputAction.bindingMask = InputBinding.MaskByGroup(device.ToString());
    }

    private static void PrintStartDevice(DEVICE device, InputDevice data = null, string description = null)
    {

#if UNITY_EDITOR

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

    #endregion

}