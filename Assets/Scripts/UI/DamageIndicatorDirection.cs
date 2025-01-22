using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorDirection : DamageIndicator
{
    [SerializeField]
    Image _directionIndicator;
    [SerializeField]
    Transform _charOrigin;
    float _offsetFromCenter;
    Coroutine _countdownCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _charHP.onHPChangeBy += OnHarm;
        _directionIndicator.enabled = false;
        _offsetFromCenter = Vector2.Distance(_directionIndicator.transform.position, Vector2.zero);
    }

    private void OnHarm(float damage,GameObject source)
    {
        OnHarm(damage);
        if (this._countdownCoroutine != null)
        {
            StopCoroutine(this._countdownCoroutine);
            this._countdownCoroutine = null;
        }
        _directionIndicator.enabled = true;

        Vector2 direction = (source.transform.position - _charOrigin.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x);

        _directionIndicator.rectTransform.localRotation = Quaternion.Euler(0, 0, angle * 180f / Mathf.PI - 90f);
        _directionIndicator.rectTransform.localPosition = direction * _offsetFromCenter;

        _countdownCoroutine = StartCoroutine(DesactivateIndicatorOn(_directionIndicator));
    }
}
