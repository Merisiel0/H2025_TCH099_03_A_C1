using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenGeneratorModule : ModuleUI
{
    [SerializeField] private float timerLength = 30f;
    [SerializeField] private Image progressBar;
    private float value = 1f;
    private bool buttonPressed = false;

    void Start()
    {
        base.Start();
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

        value += modifier;

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
