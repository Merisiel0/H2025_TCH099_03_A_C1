using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class AdminMenu : MonoBehaviour
{
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private int btnGap = 30;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject hintPanel;

    void Start()
    {
        panel.SetActive(false);
        hintPanel.SetActive(true);

        MissionEventStatus[] statuses = MissionEventManager.instance.GetLaunchableEvents();
        float btnHeight = btnPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < statuses.Length; i++)
        {
            GameObject btn = Instantiate(btnPrefab, panel.transform);
            float x = i * btnHeight + (i + 1) * btnGap;
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -x);

            string text = "Toggle ";
            switch (statuses[i].trigger)
            {
                case MissionEvent.LightsOut:
                    text += "lights";
                    break;
                case MissionEvent.ThrustersShutdown:
                    text += "thrusters";
                    break;
                case MissionEvent.ElectricFailure:
                    text += "electrics";
                    break;
                case MissionEvent.PatPlayTrigger:
                    text += "patplay";
                    break;
            }
            text += " event";

            btn.GetComponentInChildren<TMP_Text>().text = text;

            MissionEventStatus status = statuses[i];
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                MissionEventManager.SendEvent(status.isActive ? status.solver : status.trigger);
            });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            panel.SetActive(!panel.activeInHierarchy);
            hintPanel.SetActive(!panel.activeInHierarchy);
        }
    }
}
