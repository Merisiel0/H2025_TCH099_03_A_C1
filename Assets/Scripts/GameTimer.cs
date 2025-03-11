using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameTimer : MonoBehaviour
{
    public int totalTime = 360;
    public int remainingTime { get; private set; }

    public UnityEvent onTimerEnd;
    public Color onTimerEndColor = Color.green;

    public TMPro.TextMeshProUGUI timerText;

    private void Start()
    {
        remainingTime = totalTime;
        StartCoroutine(ExecuteTimer());
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
            remainingTime -= 1;
            UpdateTimerText();
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
