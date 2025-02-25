using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

public class SessionDataList : ScrollableList
{
    const string BadAPIConection = "No se ha podido conectar a la API";
    [SerializeField]
    ProfileScrollableList _profileList;
    [SerializeField]
    SessionDataBox _dataBoxPrefab;
    [SerializeField]
    TMP_Text _errorText;
    [SerializeField]
    APIConectionSO _conectionSO;
    [SerializeField]
    TMP_Text _generalKillText;
    [SerializeField]
    TMP_Text _generalDeathText;
    [SerializeField]
    TMP_Text _generalKDText;
    List<SessionDataBox> _pool=new List<SessionDataBox>();
    Coroutine _request;
    protected virtual void OnEnable()
    {
        SelectableProfile.SelectedProfileChanged += UpdateList;
        _profileList.OnFocusChange += DismantleList;
    }
    protected virtual void OnDisable()
    {
        SelectableProfile.SelectedProfileChanged -= UpdateList;
        _profileList.OnFocusChange -= DismantleList;
    }
    private void UpdateList(Profile profile)
    {
        _errorText.text= string.Empty;
        if(_request != null)
        {
            StopCoroutine(_request);
        }
        _request = StartCoroutine(RequestAndUpdateList(profile));
    }
    private IEnumerator RequestAndUpdateList(Profile profile)
    {
        SessionDataController controller = new SessionDataController(new HttpClient()
        {
            BaseAddress = new Uri(_conectionSO.URL)
        });
        TaskAwaiter < ResponseDTO <SessionData[]>> awaiter = controller.ReadProfileSessionDataAsync(profile.Id.Value,profile.Creator.Username).GetAwaiter();
        yield return new WaitUntil(() => awaiter.IsCompleted);

        try
        {
            ResponseDTO<SessionData[]> response = awaiter.GetResult();
            SessionData[] dataSet = response.Data;
            foreach (SessionData data in dataSet)
            {
                AddData(data);
            }
            FillGeneralData(dataSet);
        }
        catch (HttpRequestException)
        {
            if(_errorText != null)
            {
                _errorText.text = BadAPIConection;
            }
        }
        _request = null;
    }
    private void AddData(SessionData data)
    {
        SessionDataBox dataBox = GrabFromPool();
        dataBox.SetSessionData(data);
        AddBox(dataBox);
    }
    private void DismantleList()
    {
        List<SelectableBox> removeBoxes = new List<SelectableBox>();
        foreach(SelectableBox box in ScrollList)
        {
            box.gameObject.SetActive(false);
            removeBoxes.Add(box);
        }
        foreach(SelectableBox box in removeBoxes)
        {
            ScrollList.Remove(box);
        }
    }
    private SessionDataBox GrabFromPool()
    {
        SessionDataBox[] availableData = _pool.Where(obj => !obj.gameObject.activeSelf).ToArray();

        if (availableData.Length > 0)
        {
            SessionDataBox selectableData = availableData.First();
            selectableData.gameObject.SetActive(true);
            return selectableData;
        }
        SessionDataBox createdBox = Instantiate(_dataBoxPrefab, transform.parent);
        createdBox.transform.parent = transform;
        _pool.Add(createdBox);
        return createdBox;
    }
    private void FillGeneralData(SessionData[] dataSet)
    {
        float kill = dataSet.Sum(obj=>obj.Kills);
        float deaths = dataSet.Sum(obj => obj.Deaths);
        float kd;
        kd = kill/deaths;
        if (kd == float.PositiveInfinity) kd = kill;

        _generalKillText.text = kill.ToString();
        _generalDeathText.text = deaths.ToString();
        _generalKDText.text = kd.ToString();
    }
}
