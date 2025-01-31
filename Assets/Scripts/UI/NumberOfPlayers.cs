using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberOfPlayers : MonoBehaviour
{
    public void OnButtonClick(TextMeshProUGUI buttonText)
    {
        Debug.Log("Bot�n presionado: " + buttonText.text);
        if (int.TryParse(buttonText.text, out int number))
        {
            PlayerPrefs.SetInt("SelectedNumber", number);
            PlayerPrefs.Save();
            Debug.Log("N�mero seleccionado: " + number);
            SceneManager.LoadScene("SelectSkin");
        }
        else
        {
            Debug.LogError("El texto del bot�n no es un n�mero v�lido.");
        }
    }
}
