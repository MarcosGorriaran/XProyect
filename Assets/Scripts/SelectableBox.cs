using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SelectableBox : MonoBehaviour
{
    public event Action FinishedSmoothMove;
    [SerializeField]
    private TMP_Text _boxName;
    private Coroutine _smoothMooveRoutine;
    protected TMP_Text BoxName
    {
        get { return _boxName; }
    }
    public abstract void SelectAction();
    public abstract void UseAction();
    public void SmoothlyMoveTowards(Vector2 destination,float duration)
    {
        if(_smoothMooveRoutine != null)
        {
            StopCoroutine(_smoothMooveRoutine);
        }
        _smoothMooveRoutine = StartCoroutine(SmoothMove(destination,duration));
    }
    private IEnumerator SmoothMove(Vector2 destination,float duration)
    {
        Vector2 start = GetComponent<RectTransform>().localPosition;
        RectTransform actualPos = GetComponent<RectTransform>();
        float actualTime = 0;
        while(actualTime <= duration)
        {
            actualTime += Time.deltaTime;
            float normalizedTimeValue = actualTime / duration;
            actualPos.localPosition = Vector2.Lerp(start,destination,normalizedTimeValue);
            float expectedDistance = Vector2.Distance(start, destination);
            float actualDistance = Vector2.Distance(start, actualPos.localPosition);
            if (actualDistance > expectedDistance)
            {
                actualPos.localPosition = destination;
            }
            yield return null;
        }
        FinishedSmoothMove?.Invoke();
        _smoothMooveRoutine = null;
    }
}
