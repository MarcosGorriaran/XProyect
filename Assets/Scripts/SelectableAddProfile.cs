using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableAddProfile : SelectableBox
{
    public static event Action SelectedAddProfile;
    public static event Action UseAddProfile;

    public override void SelectAction()
    {
        SelectedAddProfile.Invoke();
    }

    public override void UseAction()
    {
        UseAddProfile.Invoke();
    }
}
