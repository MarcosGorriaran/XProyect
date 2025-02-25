using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Countdown : MonoBehaviour
{
    //texto TMP que mostrará el tiempo restante
    public TextMeshProUGUI timerText;
    private float timeRemaining = 10 * 60;
    public bool isRunning = false;
    private bool isFlashing = false;
    public GameManager gameManager;

    void Update()
    {
        if (isRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();

                if (timeRemaining <= 16 && !isFlashing)
                {
                    isFlashing = true;
                    StartCoroutine(FlashText());
                }
            }
            else
            {
                timeRemaining = 0;
                isRunning = false;
                timerText.text = "00:00";
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
