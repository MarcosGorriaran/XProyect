using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;

    [SerializeField]
    private int playerIndex = 0;


    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 inputVector = Vector2.zero;
    private bool isGrounded;
    private Transform orientation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        orientation = transform.Find("Camera");
    }

    private void Update()
    {
        MovePlayer();
        CheckGrounded();
    }

    private void CheckGrounded()
    { 
        Vector3 origin = transform.position + Vector3.down * 0.1f;
        isGrounded = Physics.Raycast(origin, Vector3.down, 1.1f, groundLayer);
    }

    private void MovePlayer()
    {
        if (inputVector == Vector2.zero) { rb.velocity = new Vector3(0, rb.velocity.y, 0); return; }

        Vector3 moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;
        moveDirection.y = 0; // Evitar movimiento vertical
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}
