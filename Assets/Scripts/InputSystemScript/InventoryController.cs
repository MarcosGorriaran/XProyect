//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Inventory.inputactions
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

public partial class @InventoryController: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InventoryController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inventory"",
    ""maps"": [
        {
            ""name"": ""Inventory"",
            ""id"": ""0dd27d43-3f1e-49a9-b28d-bb3f143e7d81"",
            ""actions"": [
                {
                    ""name"": ""Slot1"",
                    ""type"": ""Button"",
                    ""id"": ""a897c8c9-8855-4760-a279-614234804043"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slot2"",
                    ""type"": ""Button"",
                    ""id"": ""d23c03a2-4410-4023-bffc-7b489b906c02"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slot3"",
                    ""type"": ""Button"",
                    ""id"": ""6c07e359-a6ae-4d38-bc25-ca17869396eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PreviousSlot"",
                    ""type"": ""Button"",
                    ""id"": ""52ee7ce9-e7a7-42f5-86a3-4d5c683e41a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeSlot"",
                    ""type"": ""Value"",
                    ""id"": ""230d93c5-0e80-44ae-8095-250400389f70"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c1bf49a5-a776-4259-ac8d-37679e7f3907"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Slot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""daa89609-4ac3-4a68-9749-48ff1067ce35"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Slot2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d617467d-df3f-4f68-b16a-e5ca9fed02b7"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Slot3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""886eb369-7e23-49ea-b357-b58797819ed9"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""PreviousSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e573fabc-c875-4da1-956c-7daad43ba005"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ChangeSlot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_Slot1 = m_Inventory.FindAction("Slot1", throwIfNotFound: true);
        m_Inventory_Slot2 = m_Inventory.FindAction("Slot2", throwIfNotFound: true);
        m_Inventory_Slot3 = m_Inventory.FindAction("Slot3", throwIfNotFound: true);
        m_Inventory_PreviousSlot = m_Inventory.FindAction("PreviousSlot", throwIfNotFound: true);
        m_Inventory_ChangeSlot = m_Inventory.FindAction("ChangeSlot", throwIfNotFound: true);
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

    // Inventory
    private readonly InputActionMap m_Inventory;
    private List<IInventoryActions> m_InventoryActionsCallbackInterfaces = new List<IInventoryActions>();
    private readonly InputAction m_Inventory_Slot1;
    private readonly InputAction m_Inventory_Slot2;
    private readonly InputAction m_Inventory_Slot3;
    private readonly InputAction m_Inventory_PreviousSlot;
    private readonly InputAction m_Inventory_ChangeSlot;
    public struct InventoryActions
    {
        private @InventoryController m_Wrapper;
        public InventoryActions(@InventoryController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Slot1 => m_Wrapper.m_Inventory_Slot1;
        public InputAction @Slot2 => m_Wrapper.m_Inventory_Slot2;
        public InputAction @Slot3 => m_Wrapper.m_Inventory_Slot3;
        public InputAction @PreviousSlot => m_Wrapper.m_Inventory_PreviousSlot;
        public InputAction @ChangeSlot => m_Wrapper.m_Inventory_ChangeSlot;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void AddCallbacks(IInventoryActions instance)
        {
            if (instance == null || m_Wrapper.m_InventoryActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InventoryActionsCallbackInterfaces.Add(instance);
            @Slot1.started += instance.OnSlot1;
            @Slot1.performed += instance.OnSlot1;
            @Slot1.canceled += instance.OnSlot1;
            @Slot2.started += instance.OnSlot2;
            @Slot2.performed += instance.OnSlot2;
            @Slot2.canceled += instance.OnSlot2;
            @Slot3.started += instance.OnSlot3;
            @Slot3.performed += instance.OnSlot3;
            @Slot3.canceled += instance.OnSlot3;
            @PreviousSlot.started += instance.OnPreviousSlot;
            @PreviousSlot.performed += instance.OnPreviousSlot;
            @PreviousSlot.canceled += instance.OnPreviousSlot;
            @ChangeSlot.started += instance.OnChangeSlot;
            @ChangeSlot.performed += instance.OnChangeSlot;
            @ChangeSlot.canceled += instance.OnChangeSlot;
        }

        private void UnregisterCallbacks(IInventoryActions instance)
        {
            @Slot1.started -= instance.OnSlot1;
            @Slot1.performed -= instance.OnSlot1;
            @Slot1.canceled -= instance.OnSlot1;
            @Slot2.started -= instance.OnSlot2;
            @Slot2.performed -= instance.OnSlot2;
            @Slot2.canceled -= instance.OnSlot2;
            @Slot3.started -= instance.OnSlot3;
            @Slot3.performed -= instance.OnSlot3;
            @Slot3.canceled -= instance.OnSlot3;
            @PreviousSlot.started -= instance.OnPreviousSlot;
            @PreviousSlot.performed -= instance.OnPreviousSlot;
            @PreviousSlot.canceled -= instance.OnPreviousSlot;
            @ChangeSlot.started -= instance.OnChangeSlot;
            @ChangeSlot.performed -= instance.OnChangeSlot;
            @ChangeSlot.canceled -= instance.OnChangeSlot;
        }

        public void RemoveCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInventoryActions instance)
        {
            foreach (var item in m_Wrapper.m_InventoryActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InventoryActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IInventoryActions
    {
        void OnSlot1(InputAction.CallbackContext context);
        void OnSlot2(InputAction.CallbackContext context);
        void OnSlot3(InputAction.CallbackContext context);
        void OnPreviousSlot(InputAction.CallbackContext context);
        void OnChangeSlot(InputAction.CallbackContext context);
    }
}
