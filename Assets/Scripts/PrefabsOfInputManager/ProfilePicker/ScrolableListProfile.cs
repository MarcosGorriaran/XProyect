using System.Collections;
using System.Collections.Generic;
using ProyectXAPILibrary.Controller;
using UnityEngine;
using System.Net.Http;
using System;
using System.Runtime.CompilerServices;
using ProyectXAPI.Models.DTO;
using ProyectXAPI.Models;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ScrolableListProfile : ScrollableList
{
    public event Action<Profile,int,ScrolableListProfile> selectedProfile;
    [SerializeField]
    APIConectionSO _conectionSO;
    [SerializeField]
    SelectableProfile _selectablePrefab;
    [SerializeField]
    Button _selectNextArrow;
    [SerializeField]
    Button _selectPreviousArrow;
    private int _playerIndex;

    public int PlayerIndex
    {
        get { return _playerIndex; }
        set { _playerIndex = value; }
    }
    protected override void Start()
    {
        base.Start();
        if(AcountManager.Session != null)
        {
            StartCoroutine(GetProfiles());
        }
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
            SelectableProfile box = Instantiate(_selectablePrefab, FindObjectOfType<Canvas>().transform);
            AddBox(box);
            box.ProfileSelected += SelectedProfile;
            foreach (Profile profile in responseDTO.Data)
            {
                box = Instantiate(_selectablePrefab,FindObjectOfType<Canvas>().transform);
                AddBox(box);
                box.RepresentingProfile = profile;
                box.ProfileSelected += SelectedProfile;
            }
        }
        catch (HttpRequestException)
        {

        }
    }
    private void SelectedProfile(Profile profile)
    {
        selectedProfile?.Invoke(profile,PlayerIndex,this);
    }
    public void ToggleNavButtons()
    {
        _selectNextArrow.interactable = !_selectNextArrow.interactable;
        _selectPreviousArrow.interactable = !_selectPreviousArrow.interactable;
    }

}
