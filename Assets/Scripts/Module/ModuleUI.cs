using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ModuleUI : MonoBehaviour
{
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
                    .setEase(LeanTweenType.easeInCubic)
                    .setOnComplete(() => gameObject.SetActive(false));
        UIEnabled = false;
    }
}