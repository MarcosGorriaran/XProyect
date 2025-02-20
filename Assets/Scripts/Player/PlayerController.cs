using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float walkSpeed = 3f;
    [SerializeField] public float runSpeed = 6f;
    [SerializeField] private float jumpForce = 1.5f;
    [SerializeField] private LayerMask groundLayer;
    private int playerIndex;

    private Rigidbody rb;
    private Vector2 inputVector = Vector2.zero;
    private bool isGrounded;
    private Transform orientation;
    private Inventory inventory;
    private IWeapon currentWeapon;
    private int currentWeaponIndex = 0;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        orientation = transform.Find("Camera");
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MovePlayer();
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        Debug.Log("Ejecutando CheckGrounded()"); // Asegurarnos de que se ejecuta

        float rayLength = 1f;
        float sphereCastHeight = 1.0f;

        Vector3 spherePosition = transform.position + Vector3.up * sphereCastHeight;

        isGrounded = Physics.SphereCast(spherePosition, 0.3f, Vector3.down, out _, rayLength, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        Debug.DrawRay(spherePosition, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);
    }



    private void MovePlayer()
    {
        float moveX = inputVector.x;
        float moveY = inputVector.y;

        if (moveX == 0 && moveY == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
            return;
        }

        float joystickIntensity = inputVector.magnitude;
        bool isRunning = joystickIntensity > 0.6f;
        float moveSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 moveDirection = orientation.forward * moveY + orientation.right * moveX;
        moveDirection.y = 0;

        // Detectar pendiente
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, groundLayer))
        {
            Vector3 normal = hit.normal;
            float slopeAngle = Vector3.Angle(normal, Vector3.up);

            if (slopeAngle < 45f) // Subir si la pendiente es menor a 45°
            {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, normal).normalized;
            }
        }

        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Pasar valores al Animator
        animator.SetFloat("MoveX", moveX * (isRunning ? 1.2f : 1f)); // Escalamos para diferenciar correr de caminar
        animator.SetFloat("MoveY", moveY * (isRunning ? 1.5f : 1f));
    }

    public void SetInputVector(Vector2 direction)
    {
        Debug.Log($"SetInputVector recibido: {direction}");
        inputVector = direction;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    public void Attack()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Attack();
        }
    }


    public void ChangeWeapon(float direction)
    {
        if (inventory == null) return; 

        int inventorySize = inventory.GetInventorySize();
        if (inventorySize == 0) return;

        // Cambiar índice del arma según la dirección del input (LB = -1, RB = +1)
        currentWeaponIndex += direction > 0 ? 1 : -1;

       
        if (currentWeaponIndex >= inventorySize)
            currentWeaponIndex = 0;
        else if (currentWeaponIndex < 0)
            currentWeaponIndex = inventorySize - 1;

        inventory.ChangeWeapon((uint)currentWeaponIndex);
        currentWeapon = inventory.GetActiveWeapon();

        Image delayAttackImage = transform.parent.GetComponentInChildren<Canvas>().GetComponentInChildren<Image>();
        if (delayAttackImage != null)
        {
            if (currentWeapon is Crossbow crossbow)
            {
                crossbow.SetDelayAttackImage(delayAttackImage);
            }
            else if (currentWeapon is Shotgun shotgun)
            {
                shotgun.SetDelayAttackImage(delayAttackImage);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró la imagen de la barra de recarga.");
        }

    }

    public void Recharge()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Recharge();
        }
    }


    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }


    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}
