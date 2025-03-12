using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelOptionButton : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI durationText;

    public GameObject regularLayout;
    public GameObject errorLayout;

    public void InitAsError()
    {
        regularLayout.SetActive(false);
        errorLayout.SetActive(true);
    }

    public void Init(LevelData data)
    {
        headerText.SetText(data.nom + " - " + data.difficulty);
        descriptionText.SetText(data.description);

        string minutes = (data.duration / 60).ToString();
        minutes = minutes.PadLeft(1, '0');
        string seconds = (data.duration % 60).ToString();
        seconds = seconds.PadLeft(2, '0');
        durationText.SetText(minutes + ":" + seconds);
    }
}
