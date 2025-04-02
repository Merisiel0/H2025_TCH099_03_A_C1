using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BipolarityModule : ModuleUI, MissionEventListener
{
    private static string apiUrl = "/api/v1/module?module=bipolarity";

    BipolarityModuleData data;

    private Color regularTextColor;
    private Color selectedTextColor;
    private Color goodCaseColor;

    private int currentSelection = 0;
    [SerializeField] private TextMeshProUGUI[] texts = new TextMeshProUGUI[4];

    private int currentCharIndex = 0;
    [SerializeField] private TextMeshProUGUI[] inputTexts = new TextMeshProUGUI[8];

    public void OnConfirm()
    {
        string input = "";
        foreach(TextMeshProUGUI text in inputTexts)
        {
            input += text.text;
        }

        if(input != data.solution)
        {
            MissionEventManager.SendEvent(MissionEvent.PlayerEventFailed);
            PlayerInteract.StopInteractions();
        } else
        {
            MissionEventManager.SendEvent(MissionEvent.ThrustersStart);
            ModuleSucess();
            PlayerInteract.StopInteractions();
        }
    }

    public void OnDelChar()
    {
        if(currentCharIndex > 0)
        {
            currentCharIndex--;
            inputTexts[currentCharIndex].SetText("");
        }
    }

    public void OnAddChar(string ch)
    {
        if(currentCharIndex < inputTexts.Length)
        {
            inputTexts[currentCharIndex].SetText(ch);
            currentCharIndex++;
        }
    }

    public void OnClickCaseButton()
    {
        texts[currentSelection].color = regularTextColor;

        currentSelection++;
        if(currentSelection > texts.Length - 1)
        {
            currentSelection = 0;
        }

        if(data.caseChoisie - 1 == currentSelection)
        {
            texts[currentSelection].color = goodCaseColor;
        } else
        {
            texts[currentSelection].color = selectedTextColor;
        }
    }

    public void OnNotify(MissionEvent e)
    {
        if(e == MissionEvent.ThrustersShutdown)
        {
            ApiController.FetchDataFromAPI<ModuleEventRespone<BipolarityModuleData>>(apiUrl, (data) => {
                data.Init(this);
                InitModule(data.module);
            });

            // UNCOMMENT TO CREATE MODULE DATA MANUALLY
            //BipolarityModuleData data = new BipolarityModuleData();
            //data.id_ = 1;
            //data.lettre1 = "X";
            //data.lettre2 = "J";
            //data.lettre3 = "R";
            //data.lettre4 = "T";
            //data.caseChoisie = 2;
            //data.solution = "01101010";
            //data.couleur = "0000FF";

            //InitModule(data);
        }
    }

    private void Start()
    {
        base.Start();

        data = new BipolarityModuleData();

        foreach (TextMeshProUGUI text in inputTexts)
        {
            text.text = "";
        }

        selectedTextColor = texts[0].color;
        regularTextColor = texts[1].color;

        MissionEventManager.AddEventListener(this);
    }

    public void InitModule(BipolarityModuleData newData)
    {
        currentCharIndex = 0;
        currentSelection = 0;

        data = newData.Init();
        goodCaseColor = data.objCouleur;

        for (int i = 0; i < texts.Length; i++)
        {
            if(i == currentSelection)
            {
                if (data.caseChoisie - 1 == i)
                {
                    texts[i].color = goodCaseColor;
                } else 
                {
                    texts[i].color = selectedTextColor;
                }
            } 
            else
            {
                texts[i].color = regularTextColor;
            }
        }

        foreach (TextMeshProUGUI text in inputTexts)
        {
            text.text = "";
        }

        texts[0].SetText(data.lettre1);
        texts[1].SetText(data.lettre2);
        texts[2].SetText(data.lettre3);
        texts[3].SetText(data.lettre4);
    }
}