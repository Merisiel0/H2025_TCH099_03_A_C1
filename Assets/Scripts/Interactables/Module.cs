using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Module : Interactable
{
    // Position pour l'ouverture et la fermeture de l'�cran du module
    private static float openedPosition = 0.0f;
    private float closedPosition;
    private bool opened = false;

    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private GameObject moduleUI;

    protected override void Start()
    {
        base.Start();
        closedPosition = moduleUI.transform.localPosition.y;
        moduleUI.SetActive(false);
    }

    private void EnableUI()
    {
        LeanTween.cancel(moduleUI);
        moduleUI.gameObject.SetActive(true);
        LeanTween.moveLocalY(moduleUI, openedPosition, animationDuration).setEase(LeanTweenType.easeOutCubic);
    }

    private void DisableUI()
    {
        LeanTween.cancel(moduleUI);
        LeanTween.moveLocalY(moduleUI, closedPosition, animationDuration)
                    .setEase(LeanTweenType.easeInCubic)
                    .setOnComplete(() => moduleUI.gameObject.SetActive(false));
    }

    public override void Interact()
    {
        if (opened)
        {
            DisableUI();
            opened = false;
        }
        else
        {
            EnableUI();
            opened = true;
        }
    }
}
