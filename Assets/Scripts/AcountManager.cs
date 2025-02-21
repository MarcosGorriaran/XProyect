using System.Collections;
using System.Collections.Generic;
using ProyectXAPI.Models;
using TMPro;
using UnityEngine;

public abstract class AcountManager : MonoBehaviour
{
    protected const string DefaultError = "Ha habido un problema en el proceso";
    const string WrongLogin = "Incorrect login, check user and password";
    const string WrongPassword = "The new password can't be the same as the old password";
    const string WrongLoginTranslation = "Login incorrecto, revisa usuario y contraseña";
    const string WrongPasswordTranslation = "La nueva contraseña no puede ser la misma que la antigua";
    const string RepeatedUsername = "There is an acount with the same username";
    const string RepeatedUsernameTranslation = "Ya hay una cuenta con el mismo nombre";
    private static Dictionary<string, string> _errorTranslator = new Dictionary<string, string>() 
    {
        {WrongLogin,WrongLoginTranslation },
        {WrongPassword,WrongPasswordTranslation},
        {RepeatedUsername,RepeatedUsernameTranslation},
    };
    private static Acount _session;
    [SerializeField]
    APIConectionSO _apiConectionInfo;
    [SerializeField]
    Color _errorColor;
    [SerializeField]
    Color _successColor;
    [SerializeField]
    TMP_Text _generalErrorMessage;


    public static Acount Session
    {
        get { return _session; }
        protected set { _session = value; }
    }
    protected APIConectionSO ApiConectionInfo
    {
        get { return _apiConectionInfo; }
    }
    protected Color ErrorColor
    {
        get { return _errorColor; }
    }
    protected Color SuccessColor
    {
        get { return _successColor; }
    }
    protected void WriteError(string message, TMP_Text textBox)
    {
        textBox.text = message;
        textBox.color = _errorColor;
    }
    protected void WriteSuccess(string message, TMP_Text textBox)
    {
        textBox.text = message;
        textBox.color = _successColor;
    }
    protected string TranslateError(string error)
    {
        string translation;
        try
        {
            translation = _errorTranslator[error];
        }
        catch (KeyNotFoundException)
        {
            translation= DefaultError;
        }
        return translation;
    }
    protected void WriteError(string message)
    {
        _generalErrorMessage.text = message;
        _generalErrorMessage.color = _errorColor;
    }
    protected void WriteSuccess(string message)
    {
        _generalErrorMessage.text = message;
        _generalErrorMessage.color = _successColor;
    }
    protected abstract IEnumerator SendInfo(Acount acount);
    protected abstract void DesactivateInputFields();
    protected abstract void ActivateInputFields();
    public abstract void ManagerAction();
}
