using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class EventNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI durationText;

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
            // TODO: NOTIFIER LE NOTIFICATION CONTROLLER
            Destroy(gameObject);
        });
    }

    public void Init(string name, int duration)
    {
        nameText.SetText(name);
        durationText.SetText(duration + "s");
    }
}
