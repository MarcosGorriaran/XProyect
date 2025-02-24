using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AcountSessionManager : MonoBehaviour
{
    [SerializeField]
    GameObject _logedInElement;
    [SerializeField]
    GameObject _logedOutElement;
    [SerializeField]
    TMP_Text _usernameShow;
    // Start is called before the first frame update
    void Start()
    {
        if (AcountManager.Session != null)
        {
            _logedInElement.SetActive(true);
            _logedOutElement.SetActive(false);
            _usernameShow.text = AcountManager.Session.Username;
        }
        else
        {
            _logedInElement.SetActive(false);
            _logedOutElement.SetActive(true);
        }
    }

    public void LogOut()
    {
        AcountManager.CloseSession();
        _logedInElement.SetActive(false);
        _logedOutElement.SetActive(true);
    }
}
