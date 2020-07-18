// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input System/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Action Player"",
            ""id"": ""a350cff5-9b17-41ce-a559-a4c742828970"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a6308b3b-88f0-4626-9ab6-dfaaa9a15719"",
                    ""expectedControlType"": ""Axis"",
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
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""82ecff7d-a405-4943-b24a-59db54305473"",
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
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""897e73f1-673e-4aa0-b3e8-e16ef5cfd8dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Walk"",
                    ""type"": ""Button"",
                    ""id"": ""944f967c-42b7-4db1-b8ed-61a3e61deb95"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""e64e27ee-0069-4c36-9f37-91ec8085aba3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""c65b17de-0041-49b0-9ae5-5f583fe51bf8"",
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
                    ""id"": ""00875ec4-af5d-4681-a605-0ada57ddc738"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3f757795-c021-48dd-bfde-b592c399a667"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""96ccaa57-fc38-47c3-beed-58c7ca72806d"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4bd58e91-1e57-46ab-b287-20791566d001"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Analog"",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fd8880c-3dea-4bfa-bab7-d470bd419e5b"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cce47fb-c4e6-4bbb-ab22-6f1be92fe3fd"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""876b4795-9610-43c2-9a57-e6baae6a52a0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bb3ca19-9e1e-4a1b-8507-a05e89d33c88"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""732a0010-6ee1-40d3-993d-5dee01770c62"",
                    ""path"": ""<Keyboard>/rightShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f534bfa5-3d3b-43d3-8e37-13fdf4dfac6d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Action Player
        m_ActionPlayer = asset.FindActionMap("Action Player", throwIfNotFound: true);
        m_ActionPlayer_Move = m_ActionPlayer.FindAction("Move", throwIfNotFound: true);
        m_ActionPlayer_Jump = m_ActionPlayer.FindAction("Jump", throwIfNotFound: true);
        m_ActionPlayer_Interaction = m_ActionPlayer.FindAction("Interaction", throwIfNotFound: true);
        m_ActionPlayer_Pause = m_ActionPlayer.FindAction("Pause", throwIfNotFound: true);
        m_ActionPlayer_Click = m_ActionPlayer.FindAction("Click", throwIfNotFound: true);
        m_ActionPlayer_Walk = m_ActionPlayer.FindAction("Walk", throwIfNotFound: true);
        m_ActionPlayer_MousePosition = m_ActionPlayer.FindAction("MousePosition", throwIfNotFound: true);
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

    // Action Player
    private readonly InputActionMap m_ActionPlayer;
    private IActionPlayerActions m_ActionPlayerActionsCallbackInterface;
    private readonly InputAction m_ActionPlayer_Move;
    private readonly InputAction m_ActionPlayer_Jump;
    private readonly InputAction m_ActionPlayer_Interaction;
    private readonly InputAction m_ActionPlayer_Pause;
    private readonly InputAction m_ActionPlayer_Click;
    private readonly InputAction m_ActionPlayer_Walk;
    private readonly InputAction m_ActionPlayer_MousePosition;
    public struct ActionPlayerActions
    {
        private @InputActions m_Wrapper;
        public ActionPlayerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_ActionPlayer_Move;
        public InputAction @Jump => m_Wrapper.m_ActionPlayer_Jump;
        public InputAction @Interaction => m_Wrapper.m_ActionPlayer_Interaction;
        public InputAction @Pause => m_Wrapper.m_ActionPlayer_Pause;
        public InputAction @Click => m_Wrapper.m_ActionPlayer_Click;
        public InputAction @Walk => m_Wrapper.m_ActionPlayer_Walk;
        public InputAction @MousePosition => m_Wrapper.m_ActionPlayer_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_ActionPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionPlayerActions set) { return set.Get(); }
        public void SetCallbacks(IActionPlayerActions instance)
        {
            if (m_Wrapper.m_ActionPlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnJump;
                @Interaction.started -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnInteraction;
                @Pause.started -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnPause;
                @Click.started -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnClick;
                @Walk.started -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnWalk;
                @MousePosition.started -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_ActionPlayerActionsCallbackInterface.OnMousePosition;
            }
            m_Wrapper.m_ActionPlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public ActionPlayerActions @ActionPlayer => new ActionPlayerActions(this);
    public interface IActionPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnWalk(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
