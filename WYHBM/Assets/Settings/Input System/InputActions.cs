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
                    ""expectedControlType"": ""Vector2"",
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
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1fa19f8f-6aed-4dd1-930f-c79783b67118"",
                    ""path"": ""<HID::Microntek              USB Joystick          >/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
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
    public struct ActionPlayerActions
    {
        private @InputActions m_Wrapper;
        public ActionPlayerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_ActionPlayer_Move;
        public InputAction @Jump => m_Wrapper.m_ActionPlayer_Jump;
        public InputAction @Interaction => m_Wrapper.m_ActionPlayer_Interaction;
        public InputAction @Pause => m_Wrapper.m_ActionPlayer_Pause;
        public InputAction @Click => m_Wrapper.m_ActionPlayer_Click;
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
    }
}
