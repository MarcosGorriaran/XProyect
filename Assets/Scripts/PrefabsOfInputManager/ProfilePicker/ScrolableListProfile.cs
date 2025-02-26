using System.Collections;
using System.Collections.Generic;
using ProyectXAPILibrary.Controller;
using UnityEngine;
using System.Net.Http;
using System;
using System.Runtime.CompilerServices;
using ProyectXAPI.Models.DTO;
using ProyectXAPI.Models;

public class ScrolableListProfile : ScrollableList
{
    [SerializeField]
    APIConectionSO _conectionSO;
    [SerializeField]
    SelectableProfile _selectablePrefab;
    protected override void Start()
    {
        base.Start();

    }
    private IEnumerator GetProfiles()
    {
        AcountController controller = new AcountController(new HttpClient()
        {
            BaseAddress = new Uri(_conectionSO.URL)
        });
        TaskAwaiter<ResponseDTO<Profile[]>> awaiter = controller.GetAcountProfiles(AcountManager.Session).GetAwaiter();
        yield return new WaitUntil(()=>awaiter.IsCompleted);
        try
        {
            ResponseDTO<Profile[]> responseDTO = awaiter.GetResult();
            foreach (Profile profile in responseDTO.Data)
            {
                SelectableProfile box = Instantiate(_selectablePrefab);
                AddBox(box);

            }
        }
        catch (HttpRequestException)
        {

        }
    }

}
