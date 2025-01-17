using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using System.Collections;
using System.Collections.Generic;
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

       
        //busca tots els playerController a la escena
        var players = FindObjectsOfType<PlayerController>();
        //obte l'index del playerInput actual
        var index = playerInput.playerIndex;
        //troba el playerController corresponent al index
        playerController = players.FirstOrDefault(m => m.GetPlayerIndex() == index);
        if(playerController != null )
        {
            Debug.Log("PlayerController founded!");
            cameraRotation = playerController.GetComponentInChildren<CameraRotation>();
            if(cameraRotation != null )
            {
                Debug.Log("Camera founded!");
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerController != null)
        {
            Debug.Log("OnMove");
            playerController.SetInputVector(context.ReadValue<Vector2>());
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
        if(cameraRotation != null)
        {
            cameraRotation.SetInputDelta(context.ReadValue<Vector2>());
        }
    }

}
