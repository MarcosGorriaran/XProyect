using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;
    private CameraRotation cameraRotation;

    [System.Obsolete]
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        //busca tots els playerController a la escena
        var players = FindObjectsOfType<PlayerController>();

        playerController = players.FirstOrDefault(m => m.GetPlayerIndex() == playerInput.playerIndex);
        if (playerController != null)
        {
            Debug.Log("PlayerController founded!");
            cameraRotation = playerController.GetComponentInChildren<CameraRotation>();
            if (cameraRotation != null)
            {
                Debug.Log("Camera founded!");
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

}
