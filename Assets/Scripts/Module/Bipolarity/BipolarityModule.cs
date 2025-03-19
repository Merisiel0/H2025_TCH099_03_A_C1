using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BipolarityModule : MonoBehaviour, MissionEventListener
{
    BipolarityModuleData data;

    private Color regularTextColor;
    private Color selectedTextColor;

    private int currentSelection = 0;
    [SerializeField] private TextMeshProUGUI[] texts = new TextMeshProUGUI[4];

    public void OnClickCaseButton()
    {
        Debug.Log("pressed");

        texts[currentSelection].color = regularTextColor;

        currentSelection++;
        if(currentSelection > texts.Length - 1)
        {
            currentSelection = 0;
        }

        texts[currentSelection].color = selectedTextColor;
    }

    public void OnNotify(MissionEvent e)
    {
        // TODO CHANGER POUR LE BON EVENT
        if(e == MissionEvent.LightsOut)
        {
            BipolarityModuleData data = new BipolarityModuleData();
            data.id_ = 1;
            data.lettre1 = "X";
            data.lettre2 = "J";
            data.lettre3 = "R";
            data.lettre4 = "T";
            data.caseChoisie = 2;
            data.couleur = "0000FF";

            InitModule(data);
        }
    }

    private void Start()
    {
        data = new BipolarityModuleData();

        selectedTextColor = texts[0].color;
        regularTextColor = texts[1].color;

        MissionEventManager.AddEventListener(this);
    }

    public void InitModule(BipolarityModuleData newData)
    {
        data = newData.Init();

        texts[0].SetText(data.lettre1);
        texts[1].SetText(data.lettre2);
        texts[2].SetText(data.lettre3);
        texts[3].SetText(data.lettre4);
    }
}