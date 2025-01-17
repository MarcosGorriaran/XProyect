using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    public bool lockCursor = true;
    public float lookSensitivity = 2f;
    public float smoothTime = 0.1f;

    private Vector2 currentInputDelta = Vector2.zero;
    private float cameraRotationX = 0f;
    private float cameraRotationY = 0f;

    [Range (0f, 90f)]
    public float xCameraLimitRotation = 90f;

    public event Action<float> OnRotationYEvent;

    private void Start()
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    public void SetInputDelta(Vector2 inputDelta)
    {
        currentInputDelta = inputDelta;
    }

    public void Update()
    {
        Look(currentInputDelta);
    }

    private void Look(Vector2 inputDelta)
    {
        Vector2 targetInputDelta = inputDelta * lookSensitivity;

        currentInputDelta.x = Mathf.Lerp(currentInputDelta.x, targetInputDelta.x, smoothTime);
        currentInputDelta.y = Mathf.Lerp(currentInputDelta.y, targetInputDelta.y, smoothTime);

        OnRotationYEvent?.Invoke(currentInputDelta.x);

        cameraRotationX -= currentInputDelta.y;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -xCameraLimitRotation, xCameraLimitRotation);
        transform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);

        cameraRotationY += currentInputDelta.x;
        cameraRotationY = Mathf.Repeat(cameraRotationY, 360f);
        transform.parent.rotation = Quaternion.Euler(0f, cameraRotationY, 0f);
    }
}
