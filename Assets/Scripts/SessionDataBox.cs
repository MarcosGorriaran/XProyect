using ProyectXAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SessionDataBox : SelectableBox
{
    const string DateFormat = "yyyy-MM-dd";
    [SerializeField]
    TMP_Text _dateGame;
    [SerializeField]
    TMP_Text _kDRation;
    [SerializeField]
    TMP_Text _kills;
    [SerializeField]
    TMP_Text _death;
    SessionData _representingData;
    public override void SelectAction()
    {
        
    }
    public override void UseAction()
    {
        
    }
    public void SetSessionData(SessionData representingData)
    {
        _dateGame.text =  representingData.Session.DateGame.ToString();
        _kills.text = representingData.Kills.ToString();
        _death.text = representingData.Deaths.ToString();
        try
        {
            _kDRation.text = (representingData.Kills / representingData.Deaths).ToString();
        }
        catch (DivideByZeroException)
        {
            _kDRation.text = representingData.Kills.ToString();
        }
        
        _representingData = representingData;
    }
}
