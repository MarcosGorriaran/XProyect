using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberOfPlayers : MonoBehaviour
{
    public void OnButtonClick(TextMeshProUGUI buttonText)
    {
        if (int.TryParse(buttonText.text, out int number))
        {
            PlayerPrefs.SetInt("SelectedNumber", number);
            PlayerPrefs.Save();
            Debug.Log("N�mero seleccionado: " + number);
            SceneManager.LoadScene("Arnau");
        }
        else
        {
            Debug.LogError("El texto del bot�n no es un n�mero v�lido.");
        }
    }
}
