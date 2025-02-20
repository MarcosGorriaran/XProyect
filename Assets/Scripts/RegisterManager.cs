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
    [SerializeField]
    TMP_InputField _username;
    [SerializeField]
    TMP_InputField _password;
    [SerializeField]
    TMP_InputField _repeatPassword;
    [SerializeField]
    Button _registerButton;
    

    public override void ManagerAction()
    {
        if (_password.text != _repeatPassword.text)
        {
            WriteError(WrongRepeatPassword);
        }
        else
        {
            StartCoroutine(SendInfo(new Acount()
            {
                Username = _username.text,
                Password = _password.text,
            }));
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
