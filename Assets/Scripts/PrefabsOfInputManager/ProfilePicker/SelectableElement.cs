using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SelectableElement : MonoBehaviour
{
    protected PlayerInput playerInput;
    protected int playerIndex;
    protected SkinPickerManager skinPickerManager;

    protected virtual void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        skinPickerManager = FindObjectOfType<SkinPickerManager>();
    }

    
}
