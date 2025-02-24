using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScrollableList : MonoBehaviour
{
    
    [SerializeField]
    private float _distanceBetweenElements;
    [SerializeField]
    private APIConectionSO _conectionSO;
    [SerializeField]
    private SelectableProfile _selectableProfilePrefab;
    [SerializeField]
    private SelectableAddProfile _selectableAddProfilePrefab;
    [SerializeField]
    private Button _goUpArrow;
    [SerializeField]
    private Button _goDownArrow;
    private SelectableBox _selectedBox;
    private List<Profile> _profiles;
    private List<SelectableBox> _selectableBoxes;
    private List<SelectableProfile> _boxPool;

    private SelectableBox SelectedBox
    {
        get
        {
            return _selectedBox;
        }
        set
        {
            if(_selectedBox != null)
            {
                _selectedBox.GetComponent<Button>().interactable = false;
            }
            _selectedBox = value;
            value.SelectAction();
            value.GetComponent<Button>().interactable = true;
        }
    }

    private void Start()
    {
        if(AcountManager.Session != null)
        {
            
            
        }
    }
    private IEnumerator WaitProfileData()
    {
        AcountController controller = new AcountController(new HttpClient()
        {
            BaseAddress = new Uri(_conectionSO.URL)
        });
        TaskAwaiter<ResponseDTO<Profile[]>> awaiter = controller.GetAcountProfiles(AcountManager.Session).GetAwaiter();
        yield return new WaitUntil(()=>awaiter.IsCompleted);

        try
        {
            _profiles = awaiter.GetResult().Data.ToList();
            InstantiateNewBox(_selectableAddProfilePrefab);
            foreach(Profile profile in _profiles)
            {
                InstantiateNewBox(_selectableProfilePrefab, profile);
            }
        }
        catch (HttpRequestException)
        {

        }
    }
    private void InstantiateNewBox(SelectableBox box)
    {
        SelectableBox actualBox = Instantiate(box, transform);
        
        _selectableBoxes.Add(actualBox);
        
        Vector2 newBoxLocalPosition;
        if(_selectableBoxes.Count > 0)
        {
            newBoxLocalPosition = _selectableBoxes.Last().transform.localPosition;
            newBoxLocalPosition = new Vector2(newBoxLocalPosition.x,newBoxLocalPosition.y+_distanceBetweenElements);
        }
        else
        {
            newBoxLocalPosition = new Vector2(0,0);
        }
        actualBox.transform.localPosition = newBoxLocalPosition;
        if(SelectedBox == null)
        {
            SelectedBox = actualBox;
        }
    }
    private void InstantiateNewBox(SelectableProfile box,Profile representingProfile)
    {
        SelectableBox actualBox = Instantiate(box, transform);

        _selectableBoxes.Add(actualBox);

        Vector2 newBoxLocalPosition;
        if (_selectableBoxes.Count > 0)
        {
            newBoxLocalPosition = _selectableBoxes.Last().transform.localPosition;
            newBoxLocalPosition = new Vector2(newBoxLocalPosition.x, newBoxLocalPosition.y + _distanceBetweenElements);
        }
        else
        {
            newBoxLocalPosition = new Vector2(0, 0);
        }
        actualBox.transform.localPosition = newBoxLocalPosition;
        if (SelectedBox == null)
        {
            SelectedBox = actualBox;
        }
    }
    private SelectableProfile GrabFromPool()
    {
        SelectableProfile[] availableProfiles = _boxPool.Where(obj => obj.gameObject.activeSelf).ToArray();

        if (availableProfiles.Length > 0)
        {
            //TODO: Reactivate gameObject.
            return availableProfiles.First();
        }
        SelectableProfile createdProfile = Instantiate(_selectableProfilePrefab, transform);
        _boxPool.Add(createdProfile);
        return createdProfile;
    }
}
