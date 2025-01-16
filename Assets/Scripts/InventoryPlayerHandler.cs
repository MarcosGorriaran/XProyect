using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPlayerHandler : MonoBehaviour, InventoryController.IInventoryActions
{
    [SerializeField]
    private Inventory inventory;
    private InventoryController inventoryController;
    void Awake()
    {
        inventoryController = new InventoryController();
    }

    public void OnChangeSlot(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPreviousSlot(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSlot1(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSlot2(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSlot3(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
