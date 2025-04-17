using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Mathematics;

public class LevitatingLightsModule : ModuleUI, MissionEventListener
{
    private static string apiUrl = "/api/v1/module?module=lights";

    public override MissionEvent startEvent => MissionEvent.LightsOut;
    public override MissionEvent endEvent => MissionEvent.LightsOn;

    public Color lightOn;
    public Color lightOff;
    public Color leverColor;
    [SerializeField] private GameObject lightsContainer;
    [SerializeField] private GameObject leversContainer;
    [SerializeField] private Image[] lights;
    [SerializeField] private Image[] levers;
    private bool[] solution;
    private bool activated = false;

    public void OnNotify(MissionEvent e)
    {
        if (e == startEvent)
        {
            ApiController.FetchDataFromAPI<ModuleEventRespone<LevitatingLightsModuleData>>(apiUrl, (data) => {
                data.Init(this);
                InitModule(data.module);
            });
        }
    }

    private void Start()
    {
        base.Start();

        foreach (Image light in lights)
        {
            light.color = lightOff;
        }

        foreach (Image lever in levers)
        {
            lever.color = leverColor;
        }

        MissionEventManager.AddEventListener(this);
    }

    public void InitModule(LevitatingLightsModuleData data)
    {
        data = data.Init();

        solution = new bool[data.solution.Length];
        for (int i = 0; i < solution.Length; i++)
        {
            solution[i] = data.solution[i] == '1';
        }

        for (int i = 0; i < lights.Length; i++)
        {
            if (data.lumiere[i] == 'O')
            {
                lights[i].color = lightOn;
            }
        }

        activated = true;
    }

    public void OnLeverSwitched(int index)
    {
        // toggle lever
        ToggleLever(index);

        if (activated)
        {
            // check if all levers are good
            for (int i = 0; i < levers.Length; i++)
            {
                if (solution[i] != IsLeverToggled(i))
                {
                    return;
                }
            }

            // if all levers are good, quit module
            PlayerInteract.StopInteractions();
            MissionEventManager.SendEvent(endEvent);
            activated = false;
            ModuleSucess();
        }
    }


    private bool IsLeverToggled(int i)
    {
        return levers[i].rectTransform.localScale.y < 0;
    }

    private void ToggleLever(int i)
    {
        Vector3 scale = levers[i].rectTransform.localScale;
        scale.y *= -1;
        levers[i].rectTransform.localScale = scale;

        int n = IsLeverToggled(i) ? -1 : 1;
        levers[i].rectTransform.anchoredPosition += n * new Vector2(1, 29);
    }
}