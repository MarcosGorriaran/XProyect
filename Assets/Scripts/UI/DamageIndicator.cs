using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField]
    protected HPManager _charHP;
    [SerializeField]
    Image _damageIndicator;
    Coroutine _countdownCoroutine;
    public float feedbackDuration;
    protected virtual void Awake()
    {
        _charHP.onHPChange += OnHarm;
        _damageIndicator.enabled = false;
    }

    protected void OnHarm(float damage)
    {
        _damageIndicator.enabled = true;
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
        _countdownCoroutine = StartCoroutine(DesactivateIndicatorOn(_damageIndicator));
    }
    protected IEnumerator DesactivateIndicatorOn(Image indicator)
    {
        yield return new WaitForSeconds(feedbackDuration);
        indicator.enabled = false;
    }
    protected HPManager GetHPManager()
    {
        return _charHP;
    }
}
