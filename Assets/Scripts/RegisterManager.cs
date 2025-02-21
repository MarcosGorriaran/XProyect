using UnityEngine;
using TMPro;
using System.Collections;
using ProyectXAPI.Models;
using ProyectXAPILibrary.Controller;
using System.Net.Http;
using System;
using ProyectXAPI.Models.DTO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using System.Net.Sockets;

public class RegisterManager : AcountManager
{
    const string WrongRepeatPassword = "Deben repetirse los campos 'Contraseña' y 'Repetir contraseña'";
    const string SuccesfulRegister = "El proceso de registro se ha completado y ya tiene una sesion activa";
    const string BadAPIConection = "No ha sido posible conectarse con la API";
    const string UsernameEmpty = "El nombre de usuario no puede estar vacio";
    const string PasswordEmpty = "La contraseña no puede estar vacio";
    const string RepeatPasswordEmpty = "Repetir contraseña no puede estar vacio";
    [SerializeField]
    TMP_InputField _username;
    [SerializeField]
    TMP_Text _usernameErrorField;
    [SerializeField]
    TMP_InputField _password;
    [SerializeField]
    TMP_Text _passwordErrorField;
    [SerializeField]
    TMP_InputField _repeatPassword;
    [SerializeField]
    TMP_Text _repeatPasswordErrorField;
    [SerializeField]
    Button _registerButton;
    

    public override void ManagerAction()
    {
        bool usernameValidation = UsernameValidation();
        bool passwordValidation = PasswordValidation();
        bool repeatPasswordValidation = RepeatPasswordValidation();
        if(usernameValidation && passwordValidation && repeatPasswordValidation)
        {
            StartCoroutine(SendInfo(new Acount()
            {
                Username = _username.text,
                Password = _password.text,
            }));
        }
    }
    private bool UsernameValidation()
    {
        return UsernameValidation(_username.text);
    }
    private bool PasswordValidation()
    {
        return PasswordValidation(_password.text);
    }
    private bool RepeatPasswordValidation()
    {
        return RepeatPasswordValidation(_repeatPassword.text);
    }
    public bool UsernameValidation(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            WriteError(UsernameEmpty,_usernameErrorField);
            return false;
        }
        else
        {
            WriteSuccess(string.Empty, _usernameErrorField);
            return true;
        }
    }
    public bool PasswordValidation(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            WriteError(PasswordEmpty, _passwordErrorField);
            return false;
        }
        else if (password!=_repeatPassword.text)
        {
            WriteError(WrongRepeatPassword,_passwordErrorField);
            return false;
        }
        else
        {
            WriteSuccess(string.Empty, _passwordErrorField);
            return true;
        }
    }
    public bool RepeatPasswordValidation(string repeatPassword)
    {
        if (string.IsNullOrEmpty(repeatPassword))
        {
            WriteError(RepeatPasswordEmpty, _repeatPasswordErrorField);
            return false;
        }
        else if (repeatPassword != _password.text)
        {
            WriteError(WrongRepeatPassword, _repeatPasswordErrorField);
            return false;
        }
        else
        {
            WriteSuccess(string.Empty,_repeatPasswordErrorField);
            return true;
        }
    }
    protected override IEnumerator SendInfo(Acount acountInfo)
    {
        DesactivateInputFields();

        AcountController controller = new AcountController(new HttpClient()
        {
            BaseAddress = new Uri(ApiConectionInfo.URL)
        });
        TaskAwaiter<ResponseDTO<object>> taskInfo = controller.CreateAsync(acountInfo).GetAwaiter();

        yield return new WaitUntil(()=>taskInfo.IsCompleted);
        try
        {
            ResponseDTO<object> responseDTO = taskInfo.GetResult();
            if (!responseDTO.IsSuccess)
            {
                WriteError(TranslateError(responseDTO.Message));
                ActivateInputFields();
            }
            else
            {
                Session = acountInfo;
                WriteSuccess(SuccesfulRegister);
            }
        }
        catch (HttpRequestException)
        {
            WriteError(BadAPIConection);
            ActivateInputFields();
        }
        
    }
    protected override void DesactivateInputFields()
    {
        _username.interactable = false;
        _password.interactable = false;
        _repeatPassword.interactable = false;
        _registerButton.interactable = false;
    }
    protected override void ActivateInputFields()
    {
        _username.interactable = true;
        _password.interactable = true;
        _repeatPassword.interactable = true;
        _registerButton.interactable = true;
    }
}
