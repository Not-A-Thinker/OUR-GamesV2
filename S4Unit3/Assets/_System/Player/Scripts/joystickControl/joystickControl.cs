//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/_System/Player/Scripts/joystickControl/joystickControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @JoystickControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @JoystickControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""joystickControl"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""078d26a4-c160-41f5-b52e-0fe211842e87"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""09569ab5-d2cc-411a-9cb0-b9b2c2cc650e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RotateMouse"",
                    ""type"": ""PassThrough"",
                    ""id"": ""17837a79-21c8-4e51-a205-3ff74b5707b0"",
                    ""expectedControlType"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""75cfe72e-1fd2-4896-9c10-1dec78da6cca"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Succ"",
                    ""type"": ""Button"",
                    ""id"": ""5f03c65b-64a4-44a8-8964-77e02b741b91"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SuccFriendlyObject"",
                    ""type"": ""Button"",
                    ""id"": ""3e5edc09-041b-4b26-9862-f9cb2788ec7a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""dfcaff73-5a2c-4444-96a5-de3fb2825550"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LockBoss"",
                    ""type"": ""Button"",
                    ""id"": ""496582f0-f095-4d4a-ae20-43e8151abe1b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Respawn"",
                    ""type"": ""Button"",
                    ""id"": ""231acc1b-0308-4b81-b551-f77557c42a9d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c05d4a63-f76a-4920-9de7-c7d7b3cc18fc"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.3,max=1)"",
                    ""groups"": ""JoyStick"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""e2ef9b71-6ab4-48c8-9b7d-75318f289f87"",
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
                    ""id"": ""9bbc0806-045d-43b1-a2b0-bd7276dde470"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5e59f505-c348-48cb-9ea2-aa492f990e29"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0dfc9185-a9a0-4798-868d-76f083da9f53"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4914562b-a7a2-4a90-a674-c4114d7c7178"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""161c9923-8f05-4e77-9d92-dd0f8b498bf4"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.3,max=1)"",
                    ""groups"": ""JoyStick"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c76c62e9-b587-4f58-85d0-92769dce0b49"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""JoyStick"",
                    ""action"": ""Succ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f69ff5c-ae15-4b8a-9435-b320078be653"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""JoyStick"",
                    ""action"": ""Succ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d73b0402-601c-4586-b5af-f56140cfb7d9"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Succ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""743d5509-2dd6-46d3-96ab-1937d25bb2e6"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""SuccFriendlyObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab6578df-260a-4699-88c9-d7cfd4b9e351"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""JoyStick"",
                    ""action"": ""SuccFriendlyObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2eac9f95-4eb9-4156-8d00-05805c3805b1"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""JoyStick"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a22990f-a706-4780-92c9-ac6c996320e3"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a6dd4d6-de14-430d-9ce3-8d040d262e23"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""JoyStick"",
                    ""action"": ""LockBoss"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1ff0bf5-bafd-4932-be04-0071c8937fa1"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""RotateMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afaffb37-830f-47e5-818b-9dea1ab9a1c3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""JoyStick"",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ed8f9fa-f7dd-43ba-b247-8d92b2b0914f"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse And Keyboard"",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse And Keyboard"",
            ""bindingGroup"": ""Mouse And Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""JoyStick"",
            ""bindingGroup"": ""JoyStick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Move = m_GamePlay.FindAction("Move", throwIfNotFound: true);
        m_GamePlay_RotateMouse = m_GamePlay.FindAction("RotateMouse", throwIfNotFound: true);
        m_GamePlay_Rotate = m_GamePlay.FindAction("Rotate", throwIfNotFound: true);
        m_GamePlay_Succ = m_GamePlay.FindAction("Succ", throwIfNotFound: true);
        m_GamePlay_SuccFriendlyObject = m_GamePlay.FindAction("SuccFriendlyObject", throwIfNotFound: true);
        m_GamePlay_Dash = m_GamePlay.FindAction("Dash", throwIfNotFound: true);
        m_GamePlay_LockBoss = m_GamePlay.FindAction("LockBoss", throwIfNotFound: true);
        m_GamePlay_Respawn = m_GamePlay.FindAction("Respawn", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private IGamePlayActions m_GamePlayActionsCallbackInterface;
    private readonly InputAction m_GamePlay_Move;
    private readonly InputAction m_GamePlay_RotateMouse;
    private readonly InputAction m_GamePlay_Rotate;
    private readonly InputAction m_GamePlay_Succ;
    private readonly InputAction m_GamePlay_SuccFriendlyObject;
    private readonly InputAction m_GamePlay_Dash;
    private readonly InputAction m_GamePlay_LockBoss;
    private readonly InputAction m_GamePlay_Respawn;
    public struct GamePlayActions
    {
        private @JoystickControl m_Wrapper;
        public GamePlayActions(@JoystickControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_GamePlay_Move;
        public InputAction @RotateMouse => m_Wrapper.m_GamePlay_RotateMouse;
        public InputAction @Rotate => m_Wrapper.m_GamePlay_Rotate;
        public InputAction @Succ => m_Wrapper.m_GamePlay_Succ;
        public InputAction @SuccFriendlyObject => m_Wrapper.m_GamePlay_SuccFriendlyObject;
        public InputAction @Dash => m_Wrapper.m_GamePlay_Dash;
        public InputAction @LockBoss => m_Wrapper.m_GamePlay_LockBoss;
        public InputAction @Respawn => m_Wrapper.m_GamePlay_Respawn;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void SetCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @RotateMouse.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRotateMouse;
                @RotateMouse.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRotateMouse;
                @RotateMouse.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRotateMouse;
                @Rotate.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRotate;
                @Succ.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSucc;
                @Succ.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSucc;
                @Succ.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSucc;
                @SuccFriendlyObject.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSuccFriendlyObject;
                @SuccFriendlyObject.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSuccFriendlyObject;
                @SuccFriendlyObject.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSuccFriendlyObject;
                @Dash.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnDash;
                @LockBoss.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLockBoss;
                @LockBoss.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLockBoss;
                @LockBoss.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnLockBoss;
                @Respawn.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRespawn;
                @Respawn.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRespawn;
                @Respawn.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnRespawn;
            }
            m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @RotateMouse.started += instance.OnRotateMouse;
                @RotateMouse.performed += instance.OnRotateMouse;
                @RotateMouse.canceled += instance.OnRotateMouse;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Succ.started += instance.OnSucc;
                @Succ.performed += instance.OnSucc;
                @Succ.canceled += instance.OnSucc;
                @SuccFriendlyObject.started += instance.OnSuccFriendlyObject;
                @SuccFriendlyObject.performed += instance.OnSuccFriendlyObject;
                @SuccFriendlyObject.canceled += instance.OnSuccFriendlyObject;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @LockBoss.started += instance.OnLockBoss;
                @LockBoss.performed += instance.OnLockBoss;
                @LockBoss.canceled += instance.OnLockBoss;
                @Respawn.started += instance.OnRespawn;
                @Respawn.performed += instance.OnRespawn;
                @Respawn.canceled += instance.OnRespawn;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    private int m_MouseAndKeyboardSchemeIndex = -1;
    public InputControlScheme MouseAndKeyboardScheme
    {
        get
        {
            if (m_MouseAndKeyboardSchemeIndex == -1) m_MouseAndKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse And Keyboard");
            return asset.controlSchemes[m_MouseAndKeyboardSchemeIndex];
        }
    }
    private int m_JoyStickSchemeIndex = -1;
    public InputControlScheme JoyStickScheme
    {
        get
        {
            if (m_JoyStickSchemeIndex == -1) m_JoyStickSchemeIndex = asset.FindControlSchemeIndex("JoyStick");
            return asset.controlSchemes[m_JoyStickSchemeIndex];
        }
    }
    public interface IGamePlayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRotateMouse(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnSucc(InputAction.CallbackContext context);
        void OnSuccFriendlyObject(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnLockBoss(InputAction.CallbackContext context);
        void OnRespawn(InputAction.CallbackContext context);
    }
}
