using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberOfPlayers : MonoBehaviour
{
    public void OnButtonClick(TextMeshProUGUI buttonText)
    {
        Debug.Log("Botón presionado: " + buttonText.text);
        if (int.TryParse(buttonText.text, out int number))
        {
            PlayerPrefs.SetInt("SelectedNumber", number);
            PlayerPrefs.Save();
            Debug.Log("Número seleccionado: " + number);
            SceneManager.LoadScene("SelectSkin");
        }
        else
        {
            Debug.LogError("El texto del botón no es un número válido.");
        }
    }
}
