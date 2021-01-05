// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input System/CustomInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CustomInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CustomInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CustomInputAction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""a350cff5-9b17-41ce-a559-a4c742828970"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4f5254e5-949c-4ff4-b7d7-b1ee28cdb2dc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a6308b3b-88f0-4626-9ab6-dfaaa9a15719"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""82ecff7d-a405-4943-b24a-59db54305473"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""fa6bbc11-47dd-4731-809e-cb87e6cf8e85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b5cd95ac-3ed0-4021-a9e5-121f96b3740b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""de34b2f2-f181-4fbc-84e5-a8c0e322c014"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Options"",
                    ""type"": ""Button"",
                    ""id"": ""633f90f4-f7e4-453c-b8a4-07e6a44bc181"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""DebugMode"",
                    ""type"": ""Button"",
                    ""id"": ""68b6befc-e8bc-4370-9652-08060962ccd4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset Camera"",
                    ""type"": ""Button"",
                    ""id"": ""9377efe5-7dc2-4b98-828f-b930ce4e4e58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""13740944-d7b5-4e20-b7be-3979b0465244"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Listen Mode"",
                    ""type"": ""Button"",
                    ""id"": ""18725fcf-6b4e-447b-a558-5631689a2271"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""bde9c4a2-d848-4e90-bed7-b36f49141a85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""473533e6-cb34-4519-a511-fbd17754ce92"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0fd54b73-b5e4-4a2f-acc9-7f251226c0c8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""85463805-5472-45db-bfbf-0b9280ce7a6f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ba78813b-9420-4c01-a024-cc97ab849d79"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f79a223b-c5c0-4c34-bb0b-308af1a2e9d2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""USB Joystick"",
                    ""id"": ""09aeb8e1-d7ff-435b-9b11-f9cf8363ec65"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a5df94b9-9399-47e8-973e-2b382e4454a0"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""569b2ace-ba78-40d4-bb5e-67e5869a87aa"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""87d14744-3182-4234-ae97-044605cb36d9"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""de2a8144-a131-45aa-adc2-6c99353b0646"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""PS4 Controller"",
                    ""id"": ""4e3b2bb9-2592-4c24-9abc-588444351b90"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""26baf961-8423-455c-a140-a5a46e6833ad"",
                    ""path"": ""<DualShockGamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""48735338-d159-42b1-aa30-2688a78d9e7a"",
                    ""path"": ""<DualShockGamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""83118d3e-4fc5-4d94-a404-338c9cb10ed3"",
                    ""path"": ""<DualShockGamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7aa01dc0-eb21-4304-b6c7-c42faca7e247"",
                    ""path"": ""<DualShockGamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Xbox Controller"",
                    ""id"": ""2bb6518a-02f5-4666-9536-909ff2919c17"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4b7f4972-b8f1-4343-9eb4-28c8f10c3dd0"",
                    ""path"": ""<XInputController>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ea85ce34-37d2-475d-b1e7-d1be8524762b"",
                    ""path"": ""<XInputController>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""82a405de-cfdf-488f-b8ae-5fb87bf8d43d"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6016905d-3649-4204-ac75-7653ab30080d"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c2f3478d-6bad-4fc9-97e1-07e77c6fff21"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f262f04-a06d-4ad4-b636-2a12d6d1e505"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af49d97e-c699-48e2-aae1-ab5e556bfe1e"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42464276-cdf1-4a68-b46a-8566cbe81551"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cce47fb-c4e6-4bbb-ab22-6f1be92fe3fd"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""016c03da-a614-4168-a571-8bf1a2d170ee"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa133e45-50a3-4f66-ba6e-c072af588608"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5796334b-a810-406b-b2ab-b3dcb22c1b2d"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""458dd9be-82c2-4f1e-b649-f3bd63ba885d"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a5fc1e5-514b-43d2-bb02-dffbe02956a5"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2696d2a4-91ed-4cba-9d62-5d16f68fdec2"",
                    ""path"": ""<XInputController>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3793e74-88fe-4de5-b16e-7c8d85c44ff7"",
                    ""path"": ""<DualShockGamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""238cedf1-f9f6-4182-8958-3bffcc3e86e7"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""DebugMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fd8880c-3dea-4bfa-bab7-d470bd419e5b"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9fe4221-a45d-413d-a109-9300c7b763e9"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a4769ad-8faa-4e42-8474-6093467da0ae"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""214e9d52-4b22-4a76-9518-7ae136a6c335"",
                    ""path"": ""<DualShockGamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0737cc52-a2c0-4714-8acd-981824b15c4d"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4c444fa-609d-432e-b930-d8f2076f723f"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3af25df2-d732-4183-a3aa-b632461c1310"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63b3a897-a779-4746-9299-80d458e7666c"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1298364-4bb6-492a-9d9d-151a806e247b"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13ae3cd1-cd31-438c-a915-8d65e5a14ce5"",
                    ""path"": ""<DualShockGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c18acb8c-7cd6-4f77-8cd4-f6621f11bdb8"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba71543c-275c-497b-97f9-6db972c30428"",
                    ""path"": ""<DualShockGamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Reset Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3156c280-359b-44b1-875f-dc0d8c25124a"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Reset Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97e20cf1-18cb-46d5-8bf6-b77f663279c0"",
                    ""path"": ""<XInputController>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Reset Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea79e5bc-e06d-45d0-a6d0-2267e75f6e40"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1580cd3-6131-43c1-a6c6-b2decfaae453"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfd22d73-0018-4c41-9083-51cd92d12fb2"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec789987-d2dc-4c47-8b1e-a297e0aa382a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Listen Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90a62602-f723-4905-b7e9-7c250684eab4"",
                    ""path"": ""<DualShockGamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Listen Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""236f1119-2462-49ea-ba2f-e821fea50b8e"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Listen Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""738586c6-571e-4bf3-988e-ed30c35f961d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b6dcb32-95e9-4923-aebe-9e73b70e7f0f"",
                    ""path"": ""<DualShockGamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""072885bd-ea72-4617-b3dd-4c2740bb6867"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""6bd2b6e8-87f8-4abb-b33e-3f8e53508025"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7d59fd98-47ee-4bc6-8d04-ff14aa15efe7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""40edcc8c-94c0-4c7c-9a19-93b66e5f386e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""8d4f8569-026d-4aad-af30-71a059f458a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""a18fe46d-6512-4d49-8eb9-4e5b672cbb3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset Camera"",
                    ""type"": ""Button"",
                    ""id"": ""0190e7d9-ff14-4f6d-945d-e0635b23418c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""USB Joystick"",
                    ""id"": ""f640f169-098f-45b6-aaf9-263541383830"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""03c780ea-f360-4817-9d18-a899ddba5e98"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8475efa8-9afa-41be-af5e-5131329f2f12"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/hat/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8d955d95-093a-4ebf-9d99-216795102546"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b9f65e55-201c-4a7e-ac58-2f202d32c7b7"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/hat/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""78eee479-321f-4721-b074-5122212bf912"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""926605e0-1cc7-4673-86a6-fcd4602121cb"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/hat/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9c8feced-9f9c-45f9-bab3-af11bfe0cd3c"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""05edd1c2-ba4a-46ac-8e8a-1b4cb3ce4952"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/hat/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Generic"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""ac042ccc-4846-4f81-8487-412d52ff13d7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""04c05ec1-d6f0-44ed-ba98-5a94318836a4"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;PS4;Switch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""13546525-ca20-495e-93cc-054c01310f77"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;Switch;PS4"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""00c30244-d261-4f12-9600-ff0aac329659"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;Switch;PS4"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3622cba8-96cc-403d-956b-30d7f7cd5f42"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;PS4;Switch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""dd9af845-d0d4-4cbc-9338-e6850749cace"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;PS4;Switch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""84367974-208f-4c5c-918d-b8ea510169b6"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;PS4;Switch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""70e529c8-6f93-4fa6-8892-305d76428ccb"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;PS4;Switch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a9697ccb-3a03-448c-9d37-8e67987416ac"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;XboxOne;PS4;Switch"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""b83400e1-8e44-45ce-85bf-efdf955f05a9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""52d7a9f8-e5db-46f8-a81c-ad299fe2343b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""61ea371f-914e-49fb-acb8-9f750c342138"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""242c25e9-f430-4971-b5bd-f15df8efca67"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e74c484e-f5b8-455c-8d99-a6606e2c7621"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e61a2808-9017-435e-b70b-a59e9f9c2ee9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2a1b1181-1677-4121-8cdc-79a342b8f727"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b4111b65-aa91-4165-881a-d41e76e20e49"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""037abdc8-13eb-42d6-9542-5cc7699bb87e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;PC"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""03433bc2-a8c8-42b1-b7b5-ba24aa62c61f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9134c747-4b25-4d6c-bcea-74c1608efada"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efc5e97c-0f75-4e36-aad5-b7b345254607"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1299f41-e512-4e43-9e46-72b12a4d9449"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15bba2ce-4dc7-48ae-942c-da2824b8973b"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd2e6a43-3e15-46e7-a968-5694f49deba6"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2982a9d-689c-4853-b9cf-31a9f3df6fd3"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f3b461e-c3d0-4ea0-bafa-334568edd142"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7be171d8-a33d-4e79-8a00-d1fc3746469f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3643abb8-1159-41ef-aa1a-5b4a39b3d272"",
                    ""path"": ""<DualShockGamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69051904-3d5c-4463-a0f2-daf8846c0063"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7dafd5a-c1c7-4e69-bb67-5cf34666ab05"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1c96c9f-bd0d-42b3-809e-4caa939138d7"",
                    ""path"": ""<DualShockGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4c9879c-6515-4891-9ddc-f81dafe56ac8"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""396e6411-c04d-4f2a-bb40-01f2f710651e"",
                    ""path"": ""<DualShockGamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Reset Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a0fb1a6-099e-4753-a0ff-225714cc6634"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Reset Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6793d932-bd12-40b0-87ee-e3071063cb20"",
                    ""path"": ""<XInputController>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxOne"",
                    ""action"": ""Reset Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Generic"",
            ""bindingGroup"": ""Generic"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PS4"",
            ""bindingGroup"": ""PS4"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XboxOne"",
            ""bindingGroup"": ""XboxOne"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Switch"",
            ""bindingGroup"": ""Switch"",
            ""devices"": [
                {
                    ""devicePath"": ""<SwitchProControllerHID>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Interaction = m_Player.FindAction("Interaction", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_Options = m_Player.FindAction("Options", throwIfNotFound: true);
        m_Player_DebugMode = m_Player.FindAction("DebugMode", throwIfNotFound: true);
        m_Player_ResetCamera = m_Player.FindAction("Reset Camera", throwIfNotFound: true);
        m_Player_Crouch = m_Player.FindAction("Crouch", throwIfNotFound: true);
        m_Player_ListenMode = m_Player.FindAction("Listen Mode", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Look = m_UI.FindAction("Look", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_ResetCamera = m_UI.FindAction("Reset Camera", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Interaction;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_Options;
    private readonly InputAction m_Player_DebugMode;
    private readonly InputAction m_Player_ResetCamera;
    private readonly InputAction m_Player_Crouch;
    private readonly InputAction m_Player_ListenMode;
    private readonly InputAction m_Player_Run;
    public struct PlayerActions
    {
        private @CustomInputAction m_Wrapper;
        public PlayerActions(@CustomInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Interaction => m_Wrapper.m_Player_Interaction;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputAction @Options => m_Wrapper.m_Player_Options;
        public InputAction @DebugMode => m_Wrapper.m_Player_DebugMode;
        public InputAction @ResetCamera => m_Wrapper.m_Player_ResetCamera;
        public InputAction @Crouch => m_Wrapper.m_Player_Crouch;
        public InputAction @ListenMode => m_Wrapper.m_Player_ListenMode;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Interaction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteraction;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Options.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOptions;
                @Options.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOptions;
                @Options.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOptions;
                @DebugMode.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDebugMode;
                @DebugMode.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDebugMode;
                @DebugMode.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDebugMode;
                @ResetCamera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetCamera;
                @ResetCamera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetCamera;
                @ResetCamera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnResetCamera;
                @Crouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCrouch;
                @ListenMode.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnListenMode;
                @ListenMode.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnListenMode;
                @ListenMode.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnListenMode;
                @Run.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Options.started += instance.OnOptions;
                @Options.performed += instance.OnOptions;
                @Options.canceled += instance.OnOptions;
                @DebugMode.started += instance.OnDebugMode;
                @DebugMode.performed += instance.OnDebugMode;
                @DebugMode.canceled += instance.OnDebugMode;
                @ResetCamera.started += instance.OnResetCamera;
                @ResetCamera.performed += instance.OnResetCamera;
                @ResetCamera.canceled += instance.OnResetCamera;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @ListenMode.started += instance.OnListenMode;
                @ListenMode.performed += instance.OnListenMode;
                @ListenMode.canceled += instance.OnListenMode;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Look;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_ResetCamera;
    public struct UIActions
    {
        private @CustomInputAction m_Wrapper;
        public UIActions(@CustomInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_UI_Look;
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @ResetCamera => m_Wrapper.m_UI_ResetCamera;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_UIActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnLook;
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @ResetCamera.started -= m_Wrapper.m_UIActionsCallbackInterface.OnResetCamera;
                @ResetCamera.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnResetCamera;
                @ResetCamera.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnResetCamera;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @ResetCamera.started += instance.OnResetCamera;
                @ResetCamera.performed += instance.OnResetCamera;
                @ResetCamera.canceled += instance.OnResetCamera;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    private int m_GenericSchemeIndex = -1;
    public InputControlScheme GenericScheme
    {
        get
        {
            if (m_GenericSchemeIndex == -1) m_GenericSchemeIndex = asset.FindControlSchemeIndex("Generic");
            return asset.controlSchemes[m_GenericSchemeIndex];
        }
    }
    private int m_PS4SchemeIndex = -1;
    public InputControlScheme PS4Scheme
    {
        get
        {
            if (m_PS4SchemeIndex == -1) m_PS4SchemeIndex = asset.FindControlSchemeIndex("PS4");
            return asset.controlSchemes[m_PS4SchemeIndex];
        }
    }
    private int m_XboxOneSchemeIndex = -1;
    public InputControlScheme XboxOneScheme
    {
        get
        {
            if (m_XboxOneSchemeIndex == -1) m_XboxOneSchemeIndex = asset.FindControlSchemeIndex("XboxOne");
            return asset.controlSchemes[m_XboxOneSchemeIndex];
        }
    }
    private int m_SwitchSchemeIndex = -1;
    public InputControlScheme SwitchScheme
    {
        get
        {
            if (m_SwitchSchemeIndex == -1) m_SwitchSchemeIndex = asset.FindControlSchemeIndex("Switch");
            return asset.controlSchemes[m_SwitchSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnOptions(InputAction.CallbackContext context);
        void OnDebugMode(InputAction.CallbackContext context);
        void OnResetCamera(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnListenMode(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnResetCamera(InputAction.CallbackContext context);
    }
}
