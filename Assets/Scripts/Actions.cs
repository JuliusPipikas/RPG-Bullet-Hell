// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Actions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Actions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Actions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Actions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""5805f864-0c26-4bcb-bb0e-7946845bcbe4"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2531d50f-1bb8-423f-b4be-a425f11d67e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ae481f30-355a-45d7-8c95-f0bd9a041153"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a5092631-f1dc-42a6-a54a-6409699f426c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapWeapons1"",
                    ""type"": ""Button"",
                    ""id"": ""8856dd2c-f304-4907-887f-6ff88c571e0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapWeapons2"",
                    ""type"": ""Button"",
                    ""id"": ""6239c8ec-08f3-45fb-86b4-00d7b0e9cb13"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapWeapons3"",
                    ""type"": ""Button"",
                    ""id"": ""ebb8e5e2-d335-4317-b339-bb37a1c2245c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""111f81dd-fbf2-4504-9652-15255a37289f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Previous"",
                    ""type"": ""Button"",
                    ""id"": ""b87ca0b5-da99-4053-8acd-919aefda7e3f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f27acb9d-fb09-45f2-b74d-75d38a8181eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ac5236ae-45fb-4b5c-99f8-9af8ac9a25c9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67f010d6-8e23-4c96-81c0-49e1e94513a7"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""481adca2-736f-48cc-9adc-5edee789ce29"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ca0157c4-0cf5-456c-b4bb-34996913a236"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a762e642-ca57-4fb8-a5d8-c2cb74be6eb6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7dd56e29-e116-4c00-b786-29bb4d6f5ad8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e34ba90d-6ea1-440a-86af-b4a10e998364"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f4c06e38-911d-44b9-8605-24b534e2426c"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapWeapons1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91cb2753-99e7-495f-9697-53df9d570372"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapWeapons2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25797b8a-d36f-4857-b367-3477b536fe8b"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapWeapons3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d435859-ecf6-4a19-a654-fd73a68ced27"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e68a0dd-1aa2-4472-8daf-5ffd9ed208cb"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d84c269b-5dad-451c-a2f3-0165dc66c9e1"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_MousePosition = m_Player.FindAction("MousePosition", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_SwapWeapons1 = m_Player.FindAction("SwapWeapons1", throwIfNotFound: true);
        m_Player_SwapWeapons2 = m_Player.FindAction("SwapWeapons2", throwIfNotFound: true);
        m_Player_SwapWeapons3 = m_Player.FindAction("SwapWeapons3", throwIfNotFound: true);
        m_Player_Next = m_Player.FindAction("Next", throwIfNotFound: true);
        m_Player_Previous = m_Player.FindAction("Previous", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_MousePosition;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_SwapWeapons1;
    private readonly InputAction m_Player_SwapWeapons2;
    private readonly InputAction m_Player_SwapWeapons3;
    private readonly InputAction m_Player_Next;
    private readonly InputAction m_Player_Previous;
    private readonly InputAction m_Player_Pause;
    public struct PlayerActions
    {
        private @Actions m_Wrapper;
        public PlayerActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @MousePosition => m_Wrapper.m_Player_MousePosition;
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @SwapWeapons1 => m_Wrapper.m_Player_SwapWeapons1;
        public InputAction @SwapWeapons2 => m_Wrapper.m_Player_SwapWeapons2;
        public InputAction @SwapWeapons3 => m_Wrapper.m_Player_SwapWeapons3;
        public InputAction @Next => m_Wrapper.m_Player_Next;
        public InputAction @Previous => m_Wrapper.m_Player_Previous;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @MousePosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @SwapWeapons1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons1;
                @SwapWeapons1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons1;
                @SwapWeapons1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons1;
                @SwapWeapons2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons2;
                @SwapWeapons2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons2;
                @SwapWeapons2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons2;
                @SwapWeapons3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons3;
                @SwapWeapons3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons3;
                @SwapWeapons3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapWeapons3;
                @Next.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNext;
                @Next.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNext;
                @Next.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNext;
                @Previous.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrevious;
                @Previous.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrevious;
                @Previous.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPrevious;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @SwapWeapons1.started += instance.OnSwapWeapons1;
                @SwapWeapons1.performed += instance.OnSwapWeapons1;
                @SwapWeapons1.canceled += instance.OnSwapWeapons1;
                @SwapWeapons2.started += instance.OnSwapWeapons2;
                @SwapWeapons2.performed += instance.OnSwapWeapons2;
                @SwapWeapons2.canceled += instance.OnSwapWeapons2;
                @SwapWeapons3.started += instance.OnSwapWeapons3;
                @SwapWeapons3.performed += instance.OnSwapWeapons3;
                @SwapWeapons3.canceled += instance.OnSwapWeapons3;
                @Next.started += instance.OnNext;
                @Next.performed += instance.OnNext;
                @Next.canceled += instance.OnNext;
                @Previous.started += instance.OnPrevious;
                @Previous.performed += instance.OnPrevious;
                @Previous.canceled += instance.OnPrevious;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnSwapWeapons1(InputAction.CallbackContext context);
        void OnSwapWeapons2(InputAction.CallbackContext context);
        void OnSwapWeapons3(InputAction.CallbackContext context);
        void OnNext(InputAction.CallbackContext context);
        void OnPrevious(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
