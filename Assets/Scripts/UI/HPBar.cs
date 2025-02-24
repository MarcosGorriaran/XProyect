using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Slider))]
public class HPBar : MonoBehaviour
{
    public HPManager trackedHp;
    public Image hpFill;
    public Image damageImage;
    public float duration = 0.2f;

    private float maxHp;    

    private void Start ()
    {
        trackedHp.onHPChange += UpdateBar;
        trackedHp.onRevive += ResetBar; // Escuchar el evento de revivir

        maxHp = trackedHp.GetMaxHp(); // Obtener la vida m�xima al inicio
        hpFill.fillAmount = 1f; // La barra empieza llena
        damageImage.color = new Color(1, 1, 1, 0); // Oculta la imagen de da�o al inicio
    }

    private void UpdateBar(float damage)
    {
        StopAllCoroutines(); // Detiene animaciones previas
        float newFill = trackedHp.GetHp() / maxHp; // Calcula el porcentaje de vida restante
        StartCoroutine(SmoothUpdate(newFill)); // Hace una animaci�n suave para reducir la barra

        if (trackedHp.GetHp() < maxHp / 2) // Si la vida es menor al 50%
        {
            StartCoroutine(FadeEffect()); // Iniciar parpadeo
        }
        else
        {
            StopCoroutine(FadeEffect()); // Detener parpadeo si la vida sube
            damageImage.color = new Color(1, 1, 1, 0); // Ocultar imagen de da�o
        }
    }

    private IEnumerator SmoothUpdate(float targetFill)
    {
        float startFill = hpFill.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            hpFill.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / duration);
            yield return null;
        }

        hpFill.fillAmount = targetFill;

    }

    private IEnumerator FadeEffect()
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
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeSpeed);
                damageImage.color = new Color(1, 1, 1, newAlpha);
                yield return null;
            }

            increasing = !increasing;
        }
    }

    private void ResetBar(GameObject player)
    {
        StopAllCoroutines(); // Detener cualquier animaci�n previa
        hpFill.fillAmount = 1f; // Reiniciar la barra de vida a 100%
        damageImage.color = new Color(1, 1, 1, 0); // Ocultar la imagen de da�o
    }

}
