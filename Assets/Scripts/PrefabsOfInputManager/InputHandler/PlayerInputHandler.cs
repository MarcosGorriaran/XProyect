using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;



public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;
    private CameraRotation cameraRotation;

    [System.Obsolete]
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        int deviceID = playerInput.devices[0].deviceId; //busca el device id del playerInput

        //busca tots els playerController a la escena
        var players = FindObjectsOfType<PlayerController>();
        bool assigned = false;
        while(!assigned)
        {
            playerController = players.FirstOrDefault(x => x.GetInputIndex() == deviceID); //busca el playerController que tenga de indice el device id del playerInput
            if(playerController != null)
            {
                Debug.Log("PlayerController founded!");
                cameraRotation = playerController.GetComponentInChildren<CameraRotation>();
                if(cameraRotation != null)
                {
                    Debug.Log("Camera founded!");
                }
                assigned = true;
            }   
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move input: " + moveInput);
        if (playerController != null)
        {
            playerController.SetInputVector(moveInput);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (playerController != null)
        {
            playerController.Jump();
        }

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (cameraRotation != null)
        {
            cameraRotation.SetInputDelta(context.ReadValue<Vector2>());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (playerController != null)
        {
            playerController.Attack();
        }
    }

    public void OnRecharge(InputAction.CallbackContext context)
    {
        if (playerController != null)
        {
            playerController.Recharge();
        }
    }

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (playerController != null && context.performed)
        {
            float direction = context.ReadValue<float>();

            playerController.ChangeWeapon(direction);
        }
    }

    public void OnChangeWeaponInventory(InputAction.CallbackContext context)
    {
        if (playerController != null && context.performed)
        {
            playerController.ChangeWeaponInventory();
        }
    }

    public void OnChangeSensibility(InputAction.CallbackContext context)
    {
        if (playerController != null && cameraRotation != null && context.performed)
        {
            float inputValue = context.ReadValue<float>();
            playerController.ChangeSensibility(inputValue);
        }
    }

}
