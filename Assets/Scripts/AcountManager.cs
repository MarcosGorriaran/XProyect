using System.Collections;
using System.Collections.Generic;
using ProyectXAPI.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AcountManager : MonoBehaviour
{
    protected const string DefaultError = "Ha habido un problema en el proceso";
    const string WrongLogin = "Incorrect login, check user and password";
    const string WrongPassword = "The new password can't be the same as the old password";
    const string WrongLoginTranslation = "Login incorrecto, revisa usuario y contraseña";
    const string WrongPasswordTranslation = "La nueva contraseña no puede ser la misma que la antigua";
    const string RepeatedUsername = "There is an acount with the same username";
    const string RepeatedUsernameTranslation = "Ya hay una cuenta con el mismo nombre";
    const string UsernameEmpty = "El nombre de usuario no puede estar vacio";
    const string PasswordEmpty = "La contraseña no puede estar vacio";
    protected const string BadAPIConection = "No ha sido posible conectarse con la API";
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
    [SerializeField]
    TMP_InputField _username;
    [SerializeField]
    TMP_Text _usernameErrorField;
    [SerializeField]
    TMP_InputField _password;
    [SerializeField]
    TMP_Text _passwordErrorField;
    [SerializeField]
    Button _actionButton;


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
    protected TMP_InputField Username
    {
        get { return _username; }
    }
    protected TMP_Text UsernameErrorField
    {
        get { return _usernameErrorField; }
    }
    protected TMP_InputField Password
    {
        get { return _password; }
    }
    protected TMP_Text PasswordErrorField
    {
        get { return _passwordErrorField; }
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
    protected bool UsernameValidation()
    {
        return UsernameValidation(_username.text);
    }
    protected bool PasswordValidation()
    {
        return PasswordValidation(_password.text);
    }
    public virtual bool UsernameValidation(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            WriteError(UsernameEmpty, _usernameErrorField);
            return false;
        }
        else
        {
            WriteSuccess(string.Empty, _usernameErrorField);
            return true;
        }
    }
    public virtual bool PasswordValidation(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            WriteError(PasswordEmpty, _passwordErrorField);
            return false;
        }
        else
        {
            WriteSuccess(string.Empty, _passwordErrorField);
            return true;
        }
    }
    protected abstract IEnumerator SendInfo(Acount acount);
    protected virtual void DesactivateInputFields()
    {
        Username.interactable = false;
        Password.interactable = false;
        _actionButton.interactable = false;
    }
    protected virtual void ActivateInputFields()
    {
        Username.interactable = true;
        Password.interactable = true;
        _actionButton.interactable = true;
    }
    public abstract void ManagerAction();
    public static void CloseSession()
    {
        Session = null;
    }
    protected void AttemptExit()
    {
        MainMenu exitManager = FindAnyObjectByType<MainMenu>();
        if (exitManager != null)
        {
            FindAnyObjectByType<MainMenu>().OnButtonClickRegisterRegister();
        }
        
    }
}
