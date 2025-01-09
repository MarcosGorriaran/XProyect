using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour, Camera.IControlsActions
{
    Camera cameraController;
    void Awake()
    {
        cameraController = new Camera();
        cameraController.Controls.SetCallbacks(this);
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
        Mathf.Clamp(context.ReadValue<Vector2>().x, -90, 90);
    }
}
