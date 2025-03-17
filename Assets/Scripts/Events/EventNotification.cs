using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI durationText;

    public void Init(string name, int duration)
    {
        nameText.SetText(name);
        nameText.SetText(duration + "s");
    }
}
