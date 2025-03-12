using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameTimer : MonoBehaviour
{
    private static GameTimer instance;

    public int totalTime = 360;
    public int remainingTime { get; private set; }
    public bool isPaused { get; private set; }

    public UnityEvent onTimerEnd;

    private Color regularTextColor;
    public Color onTimerEndColor = Color.green;
    public Color onPausedColor = Color.yellow;

    public TMPro.TextMeshProUGUI timerText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        remainingTime = totalTime;
        regularTextColor = timerText.color;

        StartCoroutine(ExecuteTimer());
    }

    public static void Pause(bool paused)
    {
        instance.isPaused = paused;
        if(instance.isPaused)
        {
            instance.timerText.color = instance.onPausedColor;
        } else
        {
            instance.timerText.color = instance.regularTextColor;
        }
    }

    private void UpdateTimerText()
    {
        string minutes = (remainingTime / 60).ToString();
        minutes = minutes.PadLeft(2, '0');

        string seconds = (remainingTime % 60).ToString();
        seconds = seconds.PadLeft(2, '0');

        timerText.SetText(minutes + ":" + seconds); ;
    }

    IEnumerator ExecuteTimer()
    {
        if(remainingTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            if(!isPaused)
            {
                remainingTime -= 1;
                UpdateTimerText();
            }

            StartCoroutine(ExecuteTimer());
        } 
        else
        {
            yield return null;
            onTimerEnd.Invoke();
            timerText.color = onTimerEndColor;
        }
    }
}
