
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ButtonNavigator : MonoBehaviour
{
    public Button[] buttons;
    private int currentSelection = 0; // Índice del botón seleccionado

    private void Start()
    {
        string buttonGroupTag = "ButtonGroup";  
        buttons = GameObject.FindGameObjectsWithTag(buttonGroupTag)
            .Select(obj => obj.GetComponent<Button>())
            .Where(button => button != null)
            .ToArray();

        // Asegúrate de que los botones estén bien asignados
        if (buttons.Length == 0)
        {
            Debug.LogError("No hay botones asignados.");
            return;
        }

        // Resaltar el primer botón al inicio
        UpdateButtonHighlight(currentSelection);
    }

    // Este método se llama cuando se navega usando el mando (arrows o joystick)
    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 input = context.ReadValue<Vector2>();

        // Solo permitir la navegación si hay más de un botón
        if (buttons.Length == 0)
            return;

        // Navegación horizontal (izquierda/derecha)
        if (input.x > 0)
        {
            // Moverse a la derecha
            currentSelection = (currentSelection + 1) % buttons.Length;
        }
        else if (input.x < 0)
        {
            // Moverse a la izquierda
            currentSelection = (currentSelection - 1 + buttons.Length) % buttons.Length;
        }

        UpdateButtonHighlight(currentSelection); // Actualizar el resaltado del botón
    }

    // Este método se llama cuando el jugador selecciona un botón (presionando "A" o "Enter")
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (buttons.Length > 0)
        {
            // Ejecutar la acción del botón seleccionado
            buttons[currentSelection].onClick.Invoke();
            Debug.Log($"Botón {currentSelection} seleccionado.");
        }
    }

    // Método para resaltar el botón seleccionado
    private void UpdateButtonHighlight(int newSelection)
    {
        // Asegurarse de que hay botones disponibles
        if (buttons.Length == 0)
            return;

        // Eliminar resaltado de todos los botones
        foreach (var button in buttons)
        {
            button.GetComponentInChildren<TMP_Text>().color = Color.white; // Color por defecto
        }

        // Resaltar eltexto del boton seleccionado en hexadecimal
        buttons[newSelection].GetComponentInChildren<TMP_Text>().color = new Color(1f, 0.753631f, 0f); 
    }
}
