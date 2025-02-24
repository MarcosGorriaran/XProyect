using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller;
using System;
using System.Collections;
using System.Net.Http;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoginManager : AcountManager
{
    const string SuccesfulRegister = "Ha iniciado sesion como: ";
    public override void ManagerAction()
    {
        bool usernameValidation = UsernameValidation();
        bool passwordValidation = PasswordValidation();
        if (usernameValidation && passwordValidation)
        {
            StartCoroutine(SendInfo(new Acount()
            {
                Username = Username.text,
                Password = Password.text
            }));
        }
    }
    protected override IEnumerator SendInfo(Acount acount)
    {
        DesactivateInputFields();

        AcountController controller = new AcountController(new HttpClient()
        {
            BaseAddress = new Uri(ApiConectionInfo.URL)
        });
        TaskAwaiter<ResponseDTO<Acount>> taskInfo = controller.CheckLogin(acount).GetAwaiter();
        yield return new WaitUntil(()=>taskInfo.IsCompleted);
        try
        {
            ResponseDTO<Acount> responseDTO = taskInfo.GetResult();
            if (!responseDTO.IsSuccess)
            {
                WriteError(TranslateError(responseDTO.Message));
                ActivateInputFields();
            }
            else
            {
                Session = acount;
                WriteSuccess(SuccesfulRegister+acount.Username);
                AttemptExit();
            }
        }
        catch (HttpRequestException)
        {
            WriteError(BadAPIConection);
            ActivateInputFields();
        }
    }
    
}
