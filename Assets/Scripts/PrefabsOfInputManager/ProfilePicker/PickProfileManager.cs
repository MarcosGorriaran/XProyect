using ProyectXAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class PickProfileManager : PickManager
{
    [SerializeField]
    private GameObject _scrolableListGameObject;
    private Canvas _canvas;
    [SerializeField]
    private Transform[] _playersPos;
    private static Dictionary<int,Profile> _selectedProfiles = new Dictionary<int, Profile>();
    public static Dictionary<int,Profile> SelectedProfiles
    {
        get { return _selectedProfiles; }
    }

    protected override void Start()
    {
        base.Start();
        _canvas = GetComponent<Canvas>();
        Array.Resize(ref _playersPos, maxPlayers);
        SetUpPlayerSelect();
    }
    private void SetUpPlayerSelect()
    {
        for (int i = 0; i<maxPlayers; i++)
        {
            GameObject listParent = Instantiate(_scrolableListGameObject, _canvas.transform);
            ScrolableListProfile list = listParent.GetComponentInChildren<ScrolableListProfile>();
            listParent.transform.localPosition = _playersPos[i].localPosition;
            list.selectedProfile += ProfileSelected;
            list.PlayerIndex = i;
        }
    }
    private void ProfileSelected(Profile profile,int player,ScrolableListProfile eventThrower)
    {
        if (SelectedProfiles.ContainsKey(player))
        {
            SelectedProfiles.Remove(player);
            eventThrower.ToggleNavButtons();
            RestPlayerReady(player);
        }
        else if (!SelectedProfiles.ContainsValue(profile))
        {
            SelectedProfiles.Add(player, profile);
            eventThrower.ToggleNavButtons();
            SumePlayerReady(player);
        }
        
    }
    protected override void NextScene()
    {
        SceneManager.LoadScene("Scenary");
    }
}
