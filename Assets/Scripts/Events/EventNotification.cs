using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class EventNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI durationText;
    private EventNotificationController controller;

    private int duration;

    private CanvasGroup canvasGroup;

    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        LeanTween.alphaCanvas(canvasGroup, 1.0f, 0.2f);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            EndEvent();
        }
    }

    public void EndEvent()
    {
        LeanTween.alphaCanvas(canvasGroup, 0.0f, 0.3f).setOnComplete(()=>
        {
            controller.EndEvent(this);
        });
    }

    public void Init(EventNotificationController controller, string name, int duration)
    {
        this.controller = controller;

        this.duration = duration;
        UpdateDurationText();

        StartCoroutine(ExecuteTimer());

        nameText.SetText(name);
    }

    private void UpdateDurationText()
    {
        if(duration <= 5)
        {
            durationText.color = controller.dangerColor;
        }

        durationText.SetText(duration + "s");
    }

    IEnumerator ExecuteTimer()
    {
        if (duration > 0)
        {
            yield return new WaitForSeconds(1.0f);
            duration -= 1;
            UpdateDurationText();
            StartCoroutine(ExecuteTimer());
        }
        else
        {
            yield return null;
            EndEvent();
        }
    }
}
