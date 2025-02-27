using System;
using System.Collections;
using System.Net.Http;
using System.Runtime.CompilerServices;
using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller;
using UnityEngine;

public class SelectableProfile : SelectableBox
{
    public static event Action<Profile> SelectedProfileChanged;
    public static event Action<Profile> UseProfileChanged;
    public event Action<Profile> ProfileSelected;
    private Profile _representingProfile;
    [SerializeField]
    private APIConectionSO _conectionSO;
    public Profile RepresentingProfile
    {
        get
        {
            return _representingProfile;
        }
        set
        {
            _representingProfile = value;
            BoxName.text = value.ProfileName;
        }
    }
    public APIConectionSO ConectionSO
    {
        set
        {
            _conectionSO = value;
        }
    }
    public override void SelectAction()
    {
        SelectedProfileChanged?.Invoke(_representingProfile);
    }

    public override void UseAction()
    {
        UseProfileChanged?.Invoke(_representingProfile);
        ProfileSelected?.Invoke(_representingProfile);
    }
    public void UpdateProfile(Profile profile)
    {
        StartCoroutine(SendChangeRequest(profile));
    }
    private IEnumerator SendChangeRequest(Profile profile)
    {
        ProfileController controller = new ProfileController(new HttpClient()
        {
            BaseAddress = new Uri(_conectionSO.URL)
        });
        profile.Creator = AcountManager.Session;
        TaskAwaiter<ResponseDTO<object>> awaiter = controller.UpdateAsync(profile).GetAwaiter();
        yield return new WaitUntil(()=>awaiter.IsCompleted);
        try
        {
            ResponseDTO<object> response = awaiter.GetResult();
            if (response.IsSuccess)
            {
                RepresentingProfile = profile;
            }
        }
        catch (HttpRequestException)
        {

        }
    }
}
