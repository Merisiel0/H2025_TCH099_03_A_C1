using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class OxygenGeneratorModule : ModuleUI
{
    public override MissionEvent startEvent => MissionEvent.CriticalOxygenQuantity;
    public override MissionEvent endEvent => MissionEvent.OxygenQuantityRestored;

    private AudioSource source;
    [SerializeField] private AudioClip generateSound;
    private float maxVolume;
    private float fadeSoundDuration = 0.3f;

    [SerializeField] private float timerLength = 5f;
    [SerializeField] private Image progressBar;

    private float oldValue = 0f;
    [SerializeField] private float value = 1f;
    private float criticalValue;

    private bool buttonPressed = false;

    void Start()
    {
        base.Start();

        source = GetComponent<AudioSource>();
        maxVolume = source.volume;
        source.loop = true;

        criticalValue = value * 0.25f; // 1/4 de la valeur
    }

    public void OnButtonPress()
    {

        buttonPressed = true;

        source.volume = 0;
        source.clip = generateSound;
        StopAllCoroutines();
        StartCoroutine(AudioFade.FadeIn(source, fadeSoundDuration, maxVolume));
    }

    public void OnButtonRelease()
    {
        buttonPressed = false;
        StopAllCoroutines();
        StartCoroutine(AudioFade.FadeOut(source, fadeSoundDuration));
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
            MissionManager.SetFailCause("Vous avez manqué d'oxygène."); 
            MissionEventManager.SendEvent(MissionEvent.PlayerEventFailed);
            this.enabled = false; // On évite l'envoie de plusieurs events
        } else
        {
            progressBar.fillAmount = value;
        }
    }
}
