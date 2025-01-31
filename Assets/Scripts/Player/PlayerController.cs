using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;

    [SerializeField]
    private int playerIndex = 0;


    [SerializeField]
    private float jumpForce = 1.5f;

    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 inputVector = Vector2.zero;
    private bool isGrounded;
    private Transform orientation;
    private Inventory inventory;
    private IWeapon currentWeapon;
    private int currentWeaponIndex = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        orientation = transform.Find("Camera");
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        if (inventory != null && inventory.GetInventorySize() > 0)
        {
            ChangeWeapon(0);
        }
    }


    private void Update()
    {
        MovePlayer();
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * 0.1f;
        float rayLength = 1.1f;

        // Dibuja el rayo en la escena
        Debug.DrawRay(origin, Vector3.down * rayLength, isGrounded ? Color.green : Color.red);
        isGrounded = Physics.Raycast(origin, Vector3.down, rayLength, groundLayer);
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

        currentWeapon = inventory.ChangeWeapon((uint)currentWeaponIndex);

        Image delayAttackImage = GetComponentInChildren<Canvas>().GetComponentInChildren<Image>();
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
