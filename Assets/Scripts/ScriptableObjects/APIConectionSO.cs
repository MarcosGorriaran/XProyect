using UnityEngine;

[CreateAssetMenu(fileName = "APIConection", menuName = "ScriptableObjects/APIConection", order = 1)]
public class APIConectionSO : ScriptableObject
{
    [SerializeField]
    string _uRL;

    public string URL
    {
        get { return _uRL; }

    }
}
