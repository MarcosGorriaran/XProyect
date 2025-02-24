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
    const string RepeatPasswordEmpty = "Repetir contraseña no puede estar vacio";
    
    [SerializeField]
    TMP_InputField _repeatPassword;
    [SerializeField]
    TMP_Text _repeatPasswordErrorField;
    

    public override void ManagerAction()
    {
        bool usernameValidation = UsernameValidation();
        bool passwordValidation = PasswordValidation();
        bool repeatPasswordValidation = RepeatPasswordValidation();
        if(usernameValidation && passwordValidation && repeatPasswordValidation)
        {
            StartCoroutine(SendInfo(new Acount()
            {
                Username = Username.text,
                Password = Password.text,
            }));
        }
    }
    private bool RepeatPasswordValidation()
    {
        return RepeatPasswordValidation(_repeatPassword.text);
    }
    public override bool PasswordValidation(string password)
    {
        bool baseReturn = base.PasswordValidation(password);
        if (!baseReturn) return baseReturn;
        if (password != _repeatPassword.text)
        {
            WriteError(WrongRepeatPassword, PasswordErrorField);
            return false;
        }
        else
        {
            WriteSuccess(string.Empty, PasswordErrorField);
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
        else if (repeatPassword != Password.text)
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
                AttemptExit();
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
        base.DesactivateInputFields();
        _repeatPassword.interactable = false;
    }
    protected override void ActivateInputFields()
    {
        base.ActivateInputFields();
        _repeatPassword.interactable = true;
    }
}
