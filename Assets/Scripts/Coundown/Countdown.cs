using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeRemaining = 2 * 65;
    public bool isRunning = false;
    private bool isFlashing = false;
    public GameManager gameManager;
    public AudioSource audioSource;
    public AudioClip countdownSound;
    private bool hasStartedSound = false; // Para evitar reiniciar el sonido varias veces

    void Update()
    {
        if (isRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();

                // Iniciar parpadeo y sonido si quedan 16 segundos
                if (timeRemaining <= 16 && !isFlashing)
                {
                    isFlashing = true;
                    StartCoroutine(FlashText());
                }

                if (timeRemaining <= 16 && !hasStartedSound)
                {
                    hasStartedSound = true;
                    audioSource.clip = countdownSound;
                    audioSource.Play();
                }
            }
            else
            {
                timeRemaining = 0;
                isRunning = false;
                timerText.text = "00:00";

                // Detener el sonido cuando se acabe el tiempo
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        gameManager.CheckWinner();

        if (timeRemaining <= 0)
        {
            gameManager.PlayerWithMoreKills();
        }
    }

    IEnumerator FlashText()
    {
        while (timeRemaining > 0)
        {
            timerText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            timerText.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
