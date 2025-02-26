using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcountSessionManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] _logedInElement;
    [SerializeField]
    GameObject[] _logedOutElement;
    [SerializeField]
    Button[] _disableInteractableOnLogOut;
    [SerializeField]
    TMP_Text _usernameShow;
    // Start is called before the first frame update
    void Start()
    {
        if (AcountManager.Session != null)
        {
            LogIn();
            _usernameShow.text = AcountManager.Session.Username;
        }
        else
        {
            LogOut();
        }
    }

    public void LogOut()
    {
        AcountManager.CloseSession();
        foreach (GameObject go in _logedInElement)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in _logedOutElement)
        {
            go.SetActive(true);
        }
        foreach (Button button in _disableInteractableOnLogOut)
        {
            button.interactable = false;
        }
    }
    private void LogIn()
    {
        foreach (GameObject go in _logedInElement)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in _logedOutElement)
        {
            go.SetActive(false) ;
        }
        foreach(Button button in _disableInteractableOnLogOut)
        {
            button.interactable = true;
        }
    }
}
