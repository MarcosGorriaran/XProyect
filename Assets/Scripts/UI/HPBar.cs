using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Slider))]
public class HPBar : MonoBehaviour
{
    public HPManager trackedHp;
    private Slider _slider;
    public Image damageImage;
    public float duration = 0.2f;

    private void Start ()
    {
        trackedHp.onHPChange += UpdateBar;
        _slider = GetComponent<Slider> ();
        _slider.maxValue = trackedHp.GetMaxHp ();
        _slider.minValue = 0;
        _slider.value = trackedHp.GetHp ();
        damageImage.color = new Color (1, 1, 1, 0);
    }

    private void UpdateBar (float damage)
    {
        StopAllCoroutines ();
        StartCoroutine (SmoothUpdate (trackedHp.GetHp ()));

        if (trackedHp.GetHp () < trackedHp.GetMaxHp () / 2)
        {
            StartCoroutine (FadeEffect ()); // Inicia el parpadeo
        }
        else
        {
            StopCoroutine (FadeEffect ());
            damageImage.color = new Color (1, 1, 1, 0); // Oculta la imagen
        }
    }

    private IEnumerator FadeEffect ()
    {
        bool increasing = true;
        float minAlpha = 0.2f;
        float maxAlpha = 0.8f;
        float fadeSpeed = 1f;

        while (true)
        {
            float targetAlpha = increasing ? maxAlpha : minAlpha;
            float startAlpha = damageImage.color.a;
            float elapsedTime = 0f;

            while (elapsedTime < fadeSpeed)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp (startAlpha, targetAlpha, elapsedTime / fadeSpeed);
                damageImage.color = new Color (1, 1, 1, newAlpha);
                yield return null;
            }

            increasing = !increasing; // Alternar entre subir y bajar opacidad
        }
    }



    private IEnumerator SmoothUpdate (float targetValue)
    {
        float startValue = _slider.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; 
            _slider.value = Mathf.Lerp (startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        _slider.value = targetValue;
    }
}
