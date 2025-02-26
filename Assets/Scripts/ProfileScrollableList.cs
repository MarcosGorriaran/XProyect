using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScrollableList : MonoBehaviour
{
    public event Action OnFocusChange;
    [SerializeField]
    private float _distanceBetweenElements;
    [SerializeField]
    private float _transitionTime;
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
    [SerializeField]
    private ProfileNameEditor _profileNameEditor;
    private SelectableBox _selectedBox;
    private List<Profile> _profiles = new List<Profile>();
    private List<SelectableBox> _selectableBoxes = new List<SelectableBox>();
    private List<SelectableProfile> _boxPool = new List<SelectableProfile>();

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
            OnFocusChange?.Invoke();
        }
    }

    private void Start()
    {
        if(AcountManager.Session != null)
        {
            StartCoroutine(WaitProfileData());
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
            ResponseDTO<Profile[]> responseDTO = awaiter.GetResult();
            _profiles = responseDTO.Data.ToList();
            SelectableBox box = InstantiateNewBox(_selectableAddProfilePrefab);
            if(box.TryGetComponent(out Button buton))
            {
                buton.onClick.AddListener(CreateProcess);
            }
            foreach(Profile profile in _profiles)
            {
                InstantiateNewBox(profile).ConectionSO = _conectionSO;
            }
        }
        catch (HttpRequestException)
        {

        }
    }
    private SelectableBox InstantiateNewBox(SelectableBox box)
    {
        SelectableBox actualBox = Instantiate(box, transform.parent);
        actualBox.transform.parent = transform;
        actualBox.FinishedSmoothMove += EnableNavButtons;
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
        actualBox.GetComponent<RectTransform>().localPosition = newBoxLocalPosition;
        if (SelectedBox == null)
        {
            SelectedBox = actualBox;
        }
        
        _selectableBoxes.Add(actualBox);
        return actualBox;
    }
    private SelectableProfile InstantiateNewBox(Profile representingProfile)
    {
        SelectableProfile actualBox = GrabFromPool(transform.parent);
        actualBox.transform.parent = transform;
        actualBox.RepresentingProfile = representingProfile;
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
        actualBox.GetComponent<RectTransform>().localPosition = newBoxLocalPosition;
        if (SelectedBox == null)
        {
            SelectedBox = actualBox;
        }
        _selectableBoxes.Add(actualBox);
        return actualBox;
    }
    private SelectableProfile GrabFromPool(Transform parent)
    {
        SelectableProfile[] availableProfiles = _boxPool.Where(obj => !obj.gameObject.activeSelf).ToArray();

        if (availableProfiles.Length > 0)
        {
            SelectableProfile selectableProfile = availableProfiles.First();
            selectableProfile.transform.parent = parent;
            return selectableProfile;
        }
        SelectableProfile createdProfile = Instantiate(_selectableProfilePrefab, parent);
        _boxPool.Add(createdProfile);
        return createdProfile;
    }
    private void DisableNavButtons()
    {
        _goUpArrow.interactable = false;
        _goDownArrow.interactable = false;
    }
    private void EnableNavButtons()
    {
        _goUpArrow.interactable = true;
        _goDownArrow.interactable = true;
    }
    public void SelectUpElement()
    {
        DisableNavButtons();
        SelectableBox actualBox;
        float distance;
        try
        {
            actualBox = _selectableBoxes[_selectableBoxes.IndexOf(SelectedBox)+1];
            distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0)) * -1;
        }
        catch (Exception)
        {
            actualBox = _selectableBoxes.First();
            distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0));
        }
        foreach(SelectableBox box in _selectableBoxes)
        {
            box.SmoothlyMoveTowards(new Vector2(box.transform.localPosition.x,box.transform.localPosition.y + distance),_transitionTime);
        }
        SelectedBox = actualBox;
    }
    public void SelectDownElement()
    {
        DisableNavButtons();
        SelectableBox actualBox;
        float distance;
        try
        {
            actualBox = _selectableBoxes[_selectableBoxes.IndexOf(SelectedBox) - 1];
            distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0));
        }
        catch (Exception)
        {
            actualBox = _selectableBoxes.Last();
            distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0)) * -1;
        }
        foreach (SelectableBox box in _selectableBoxes)
        {
            box.SmoothlyMoveTowards(new Vector2(box.transform.localPosition.x , box.transform.localPosition.y + distance), _transitionTime);
        }
        SelectedBox = actualBox;
    }
    private void UpdateList()
    {
        FindObjectOfType<MainMenu>().OnButtonClickPerfilesMenu();
    }
    public void CreateProfile(Profile profile)
    {
        StartCoroutine(CreateRequest(profile));
    }
    private IEnumerator CreateRequest(Profile profile)
    {
        ProfileController controller = new ProfileController(new HttpClient()
        {
            BaseAddress = new Uri(_conectionSO.URL)
        });
        TaskAwaiter<ResponseDTO<object>> awaiter = controller.CreateAsync(profile).GetAwaiter();
        yield return new WaitUntil(()=>awaiter.IsCompleted);
        try
        {
            if (awaiter.GetResult().IsSuccess)
            {
                UpdateList();
            }
        }
        catch (HttpRequestException)
        {

        }
    }
    public void DeleteProfile()
    {
        StartCoroutine(DeleteRequest());
    }
    private IEnumerator DeleteRequest()
    {
        bool exception = false;
        ProfileController controller = new ProfileController(new HttpClient()
        {
            BaseAddress = new Uri(_conectionSO.URL)
        });
        TaskAwaiter<ResponseDTO<object>> awaiter;
        try
        {
            ((SelectableProfile)SelectedBox).RepresentingProfile.Creator = AcountManager.Session;
            awaiter = controller.DeleteAsync(((SelectableProfile)SelectedBox).RepresentingProfile).GetAwaiter();
        }
        catch (Exception)
        {
            exception = true;
        }
        if(!exception)
        {
            yield return new WaitUntil(() => awaiter.IsCompleted);
            try
            {
                if (awaiter.GetResult().IsSuccess)
                {
                    UpdateList();
                }
            }
            catch (HttpRequestException)
            {

            }
        }
        yield return null;
    }
    public void UpdateProcess()
    {
        try
        {
            _profileNameEditor.gameObject.SetActive(true);
            _profileNameEditor.EditProfile((SelectableProfile)SelectedBox);
        }
        catch (Exception)
        {
            _profileNameEditor.CancelAction();
        }
       
    }
    public void CreateProcess()
    {
        _profileNameEditor.gameObject.SetActive(true);
        _profileNameEditor.CreateProfile();
    }
}
