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
    private int inputIndex;

    private Rigidbody rb;
    private Vector2 inputVector = Vector2.zero;
    private bool isGrounded;
    private bool isRecharged = true;
    private Transform orientation;
    private Inventory inventory;
    private IWeapon currentWeapon;
    private int currentWeaponIndex = 0;
    private Animator animator;
    private Animator currentFPAnimator;
    public Animator FPCrossbow;
    private bool hasGorrocoptero = false;


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
        UpdateFirstPersonAnimations();
    }

    private void CheckGrounded()
    {
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
            //ejecutar animacion de idle (default ya que es la entry)
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
       
        inputVector = direction;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            hasGorrocoptero = inventory.HasGorrocoptero();

            if(hasGorrocoptero)
            {
                rb.AddForce(Vector3.up * jumpForce * 3, ForceMode.Impulse);
                animator.SetTrigger("Jump");
                return;
            }
          
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    public void Attack()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Attack();
            animator.SetTrigger("Shoot");
            if(currentFPAnimator != null)
            {
                currentFPAnimator.SetTrigger("Shoot");
                currentFPAnimator.SetBool("isShooting", true);
            }
            if(FPCrossbow != null)
            {
                FPCrossbow.SetTrigger("Shoot");
                FPCrossbow.SetBool("isShooting", true);
                FPCrossbow.SetBool("Recharged", false);   
            }
        }
    }

    public void UnrechargedState()
    {
        //mientras recharged de FPCrossbow sea falso, se ejecuta la animacion del arma sin recargar
        if(FPCrossbow != null)
        {
            isRecharged = FPCrossbow.GetBool("Recharged");
            if(isRecharged == false)
            {
                FPCrossbow.Play("Unrecharged");
            }
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

        Image delayAttackImage = transform.parent.Find("Canvas/Container").Find("RechargeTime").GetComponent<Image>(); 
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
            if(currentFPAnimator != null)
            {
                currentFPAnimator.SetTrigger("Recharge");
                currentFPAnimator.SetBool("isRecharging", true);
            }
            if(FPCrossbow != null)
            {
                FPCrossbow.SetTrigger("Recharge");
                FPCrossbow.SetBool("Recharged", true);
            }
        }
    }


    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    public void SetInputIndex(int index)
    {
        inputIndex = index;
    }


    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public int GetInputIndex()
    {
        return inputIndex;
    }

    private void UpdateFirstPersonAnimations()
    {
        if(inventory == null) return;   

        GameObject activeWeaponModel = inventory.GetActiveFirstPersonWeapon();
        if(activeWeaponModel == null) return;

        Animator newAnimator = activeWeaponModel.GetComponent<Animator>();
        if(newAnimator != currentFPAnimator)
        {
            currentFPAnimator = newAnimator;
        }
        if (currentFPAnimator == null) return;          

        bool isMoving = inputVector.magnitude > 0.1f;

        currentFPAnimator.SetBool("isMoving", isMoving);
    }

    public void ChangeWeaponInventory()
    {
        Debug.Log("Cambiando arma de inventario");
        //Si tienes solo una arma en WeaponSO[] slots de Inventory.cs, se añade a la lista de armas
        //Si tienes 2 armas en WeaponSO[] slots de Inventory.cs, se cambia por la que tienes equipada y se equipa la otra
        //tirar un raycast para ver si hay una arma en el suelo y si la hay, miramos su WEAPONSO y la añadimos a la lista de armas

        if(inventory == null) return;
        Camera playerCamera = GetComponentInChildren<Camera>();
        if(playerCamera == null)
        {
            Debug.LogWarning("No se encontró la cámara del jugador.");
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 2f);

        int layerMask = ~LayerMask.GetMask("FP" + playerIndex); // Ignorar la capa de la cámara del jugador
        RaycastHit[] hits = Physics.RaycastAll(ray, 3f, layerMask); // Raycast a todos los objetos en el rango

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Obstacle")) continue; // Ignorar el escudo

            WeaponSO weaponSO = hit.collider.GetComponent<WeaponItem>()?.weaponSO;
            if (weaponSO != null)
            {
                int inventorySize = inventory.GetInventorySize();
                Debug.Log("Tamaño del inventario: " + inventorySize);
                if (inventorySize == 1)
                {
                    Debug.Log("Añadiendo arma al inventario");
                    inventory.AddWeapon(weaponSO);
                }
                else if (inventorySize == 2)
                {
                    Debug.Log("Cambiando arma de inventario por otra");
                    inventory.ChangeWeaponInventory(weaponSO);
                }
                break; // Solo coger la primera arma válida
            }
        }

    }
}
