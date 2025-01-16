using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour, Camera.IControlsActions
{
    Camera cameraController;
    [SerializeField]
    public bool lockCursor;
    public float lookSensitivity = 2f;
    public float smoothTime = 0.1f;
    private Vector2 mouseDelta;
    private Vector2 currentMouseDelta;
    private float cameraRotationX = 0f;
    private float cameraRotationY = 0f;

    [Range (0f, 90f)]
    public float xCameraLimitRotation = 90f;

    public event Action<float> OnRotationYEvent;
    void Awake()
    {
        cameraController = new Camera();
        cameraController.Controls.SetCallbacks(this);
    }
    void Update()
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
    void OnEnable()
    {
        cameraController.Enable();
    }
    void OnDisable()
    {
        cameraController.Disable();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        Vector2 targetMouseDelta = mouseDelta * lookSensitivity;

        currentMouseDelta.x = Mathf.Lerp(currentMouseDelta.x, targetMouseDelta.x, smoothTime);
        currentMouseDelta.y = Mathf.Lerp(currentMouseDelta.y, targetMouseDelta.y, smoothTime);

        OnRotationYEvent?.Invoke(currentMouseDelta.x);

        cameraRotationX -= currentMouseDelta.y;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -xCameraLimitRotation, xCameraLimitRotation); // Limitar la inclinación vertical
        transform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }
}
