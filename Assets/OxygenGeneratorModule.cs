using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGeneratorModule : ModuleUI
{
    [SerializeField] private float timerLength = 60f;
    [SerializeField] private Image progressBar;

    private float oldValue = 0f;
    private float value = 1f;
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

        if(value < criticalValue && oldValue >= criticalValue)
        {
            MissionEventManager.SendEvent(MissionEvent.CriticalOxygenQuantity);
        } 
        else if(value >= criticalValue && oldValue < criticalValue)
        {
            MissionEventManager.SendEvent(MissionEvent.OxygenQuantityRestored);
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
