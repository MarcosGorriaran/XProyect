using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SelectableBox : MonoBehaviour
{
    private TMP_Text _boxName;
    
    protected TMP_Text BoxName
    {
        get { return _boxName; }
    }
    public abstract void SelectAction();
    public abstract void UseAction();
    protected virtual void Start()
    {
        _boxName = GetComponentInChildren<TMP_Text>();
    }
}
