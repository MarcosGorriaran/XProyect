using System;
using ProyectXAPI.Models;
using UnityEngine;

public class SelectableProfile : SelectableBox
{
    public static event Action<Profile> SelectedProfileChanged;
    public static event Action<Profile> UseProfileChanged;
    private Profile _representingProfile;

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
    public override void SelectAction()
    {
        SelectedProfileChanged.Invoke(_representingProfile);
    }

    public override void UseAction()
    {
        UseProfileChanged.Invoke(_representingProfile);
    }
}
