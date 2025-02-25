using ProyectXAPI.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileNameEditor : MonoBehaviour
{
    [SerializeField]
    TMP_InputField _textEditor;
    [SerializeField]
    ProfileScrollableList _scrollableList;
    SelectableProfile _selectedProfile;
    public void EditProfile(SelectableProfile boxProfile)
    {
        _textEditor.text = boxProfile.RepresentingProfile.ProfileName;
        _selectedProfile = boxProfile;
    }
    public void ExecuteEdition()
    {
        _selectedProfile.RepresentingProfile.ProfileName = _textEditor.text;
        _selectedProfile.UpdateProfile(_selectedProfile.RepresentingProfile);
    }
    public void CreateProfile()
    {
        _textEditor.text = string.Empty;
    }
    public void ExecuteCreation()
    {
        _scrollableList.CreateProfile(new Profile()
        {
            Creator = AcountManager.Session,
            ProfileName = _textEditor.text,
            Id = 0
        });
    }
    public void CancelAction()
    {
        _selectedProfile = null;
        gameObject.SetActive(false);
    }
}
