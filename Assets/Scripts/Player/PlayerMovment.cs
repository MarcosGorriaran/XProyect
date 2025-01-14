using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour, XProyect.IPlayerActions
{
    [Header("Movment Settings")]

    public float moveSpeed = 5f;

    [Header("Jump Settings")]

    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public float groundDistance = 0.2f;

    [Header("Cam Settings")]

    public float lookSensitivity = 2f;
    public Transform playerCamera;
    public float smoothTime = 0.1f;
    private Vector2 mouseDelta;
    private float cameraRotationX = 0f;
   
   
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;

    private Transform orientation;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    private Rigidbody rb;
    private XProyect playerInputActions;

    private bool isGrounded;

    private void Awake()
    {
        playerInputActions = new XProyect();
        playerInputActions.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        orientation = playerCamera;
    }

    private void Update()
    {
        MovePlayer();
        CheckGrounded();
        RotatePlayerAndCamera();
    }

    public void RotatePlayerAndCamera()
    {
        Vector2 targetMouseDelta = mouseDelta * lookSensitivity;

        currentMouseDelta.x = Mathf.Lerp(currentMouseDelta.x, targetMouseDelta.x, smoothTime);
        currentMouseDelta.y = Mathf.Lerp(currentMouseDelta.y, targetMouseDelta.y, smoothTime);

        transform.Rotate(Vector3.up * currentMouseDelta.x);

        cameraRotationX -= currentMouseDelta.y;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f); // Limitar la inclinación vertical
        playerCamera.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }

    private void CheckGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.red);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);

    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
       if(context.performed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = 0;
        rb.velocity = moveDirection.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);
    }



    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }


}
