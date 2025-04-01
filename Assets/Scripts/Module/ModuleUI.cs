using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;

public abstract class ModuleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI serialText;

    public void OnTimerEnd()
    {
        MissionEventManager.SendEvent(MissionEvent.PlayerEventFailed);
    }

    // Position pour l'ouverture et la fermeture de l'�cran du module
    private static float openedPosition = 0.0f;
    private float closedPosition;
    protected bool UIEnabled = false;

    [SerializeField] private float animationDuration = 0.15f;

    public void Start()
    {
        closedPosition = transform.localPosition.y;
    }

    public void EnableUI()
    {
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalY(gameObject, openedPosition, animationDuration).setEase(LeanTweenType.easeOutCubic);
        UIEnabled = true;
    }

    public void DisableUI()
    {
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalY(gameObject, closedPosition, animationDuration)
                    .setEase(LeanTweenType.easeInCubic);
        UIEnabled = false;
    }

    public void InitModule(string serial, int duration)
    {
        serialText.text = serial;
        StartCoroutine(WaitForEndOfModuleTime(duration));
    }

    protected void ModuleSucess()
    {
        StopAllCoroutines(); // Arret du timer
    }

    private IEnumerator WaitForEndOfModuleTime(int time)
    {
        yield return new WaitForSeconds(time);
        OnTimerEnd();
    }
}