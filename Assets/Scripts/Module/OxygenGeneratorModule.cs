using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGeneratorModule : ModuleUI
{
    public override MissionEvent startEvent => MissionEvent.CriticalOxygenQuantity;
    public override MissionEvent endEvent => MissionEvent.OxygenQuantityRestored;

    [SerializeField] private float timerLength = 120f;
    [SerializeField] private Image progressBar;

    private float oldValue = 0f;
    [SerializeField] private float value = 1f;
    private float criticalValue;

    private bool buttonPressed = false;

    void Start()
    {
        base.Start();
        criticalValue = value * 0.25f; // 1/4 de la valeur
    }

    public void OnButtonPress()
    {
        buttonPressed = true;
    }

    public void OnButtonRelease()
    {
        buttonPressed = false;
    }

    void Update()
    {
        float modifier;
        if(!buttonPressed) modifier = -Time.deltaTime / timerLength;
        else modifier = Time.deltaTime / timerLength * 15;

        oldValue = value;
        value += modifier;

        value = Mathf.Min(1.0f, value); // Max à la valeur

        if(value < criticalValue && oldValue >= criticalValue)
        {
            MissionEventManager.SendEvent(startEvent);
        } 
        else if(value >= criticalValue && oldValue < criticalValue)
        {
            MissionEventManager.SendEvent(endEvent);
        }

        if(value <= 0)
        {
            progressBar.fillAmount = 0f;
            MissionEventManager.SendEvent(MissionEvent.PlayerEventFailed);
            this.enabled = false; // On évite l'envoie de plusieurs events
        } else
        {
            progressBar.fillAmount = value;
        }
    }
}
