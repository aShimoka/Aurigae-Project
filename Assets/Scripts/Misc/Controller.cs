// GENERATED AUTOMATICALLY FROM 'Assets/Misc/Controller.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controller : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controller"",
    ""maps"": [
        {
            ""name"": ""Whitebox"",
            ""id"": ""dccbfe1d-1067-4d06-b8ec-9ece9af51a3a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""1a15feec-7db6-421e-8853-49b7d9b0fcad"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""d061b657-dec0-4706-906b-5fe7814e5976"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Break"",
                    ""type"": ""Button"",
                    ""id"": ""64be514b-fab8-47e2-829a-fa05a9c9c871"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Swap"",
                    ""type"": ""Button"",
                    ""id"": ""378c2a38-f5ed-4f31-8df9-4b9abcd9a3a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""97ef5513-a2e4-4f00-a659-e26f3ea2507a"",
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
                    ""id"": ""b3b9f6fe-e4cf-468b-bf47-e81a55102252"",
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
                    ""id"": ""8e821c43-382d-4d63-8871-4a3b569f8500"",
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
                    ""id"": ""cbb9c35c-e106-49b0-bddc-0de96aa7d909"",
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
                    ""id"": ""99aaac38-1e9e-43aa-a0ee-77a7a877e678"",
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
                    ""id"": ""c36cf046-ec7b-4dc8-aaff-6502fcce0f8f"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ace938b-9fa8-4da4-8d5e-b565ce5e09bc"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aafcfde6-7971-437d-a52f-ea6b84dcba9e"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26d72533-012f-4a34-ab75-33994ef6713e"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Break"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4bff8b91-a3a1-4910-9de4-162b922db377"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2eeb4db7-80c2-4334-98d8-5a6c152ee7b7"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Whitebox
        m_Whitebox = asset.FindActionMap("Whitebox", throwIfNotFound: true);
        m_Whitebox_Move = m_Whitebox.FindAction("Move", throwIfNotFound: true);
        m_Whitebox_Drop = m_Whitebox.FindAction("Drop", throwIfNotFound: true);
        m_Whitebox_Break = m_Whitebox.FindAction("Break", throwIfNotFound: true);
        m_Whitebox_Swap = m_Whitebox.FindAction("Swap", throwIfNotFound: true);
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

    // Whitebox
    private readonly InputActionMap m_Whitebox;
    private IWhiteboxActions m_WhiteboxActionsCallbackInterface;
    private readonly InputAction m_Whitebox_Move;
    private readonly InputAction m_Whitebox_Drop;
    private readonly InputAction m_Whitebox_Break;
    private readonly InputAction m_Whitebox_Swap;
    public struct WhiteboxActions
    {
        private @Controller m_Wrapper;
        public WhiteboxActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Whitebox_Move;
        public InputAction @Drop => m_Wrapper.m_Whitebox_Drop;
        public InputAction @Break => m_Wrapper.m_Whitebox_Break;
        public InputAction @Swap => m_Wrapper.m_Whitebox_Swap;
        public InputActionMap Get() { return m_Wrapper.m_Whitebox; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WhiteboxActions set) { return set.Get(); }
        public void SetCallbacks(IWhiteboxActions instance)
        {
            if (m_Wrapper.m_WhiteboxActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnMove;
                @Drop.started -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnDrop;
                @Break.started -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnBreak;
                @Break.performed -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnBreak;
                @Break.canceled -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnBreak;
                @Swap.started -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnSwap;
                @Swap.performed -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnSwap;
                @Swap.canceled -= m_Wrapper.m_WhiteboxActionsCallbackInterface.OnSwap;
            }
            m_Wrapper.m_WhiteboxActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @Break.started += instance.OnBreak;
                @Break.performed += instance.OnBreak;
                @Break.canceled += instance.OnBreak;
                @Swap.started += instance.OnSwap;
                @Swap.performed += instance.OnSwap;
                @Swap.canceled += instance.OnSwap;
            }
        }
    }
    public WhiteboxActions @Whitebox => new WhiteboxActions(this);
    public interface IWhiteboxActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnBreak(InputAction.CallbackContext context);
        void OnSwap(InputAction.CallbackContext context);
    }
}
