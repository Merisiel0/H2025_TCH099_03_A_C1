using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Module : MonoBehaviour
{
    public static List<Module> allModules = new List<Module>();
  
    // Position pour l'ouverture et la fermeture de l'écran du module
    private static float openedPosition = 0.0f;
    private float closedPosition;

    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private GameObject moduleUI;

    void Start()
    {
        closedPosition = moduleUI.transform.localPosition.y;
        allModules.Add(this);
        moduleUI.SetActive(false);
    }

    public void EnableUI()
    {
        LeanTween.cancel(moduleUI);
        moduleUI.gameObject.SetActive(true);
        LeanTween.moveLocalY(moduleUI, openedPosition, animationDuration).setEase(LeanTweenType.easeOutCubic);
    }

    public void DisableUI()
    {
        LeanTween.cancel(moduleUI);
        LeanTween.moveLocalY(moduleUI, closedPosition, animationDuration)
                    .setEase(LeanTweenType.easeInCubic)
                    .setOnComplete(() => moduleUI.gameObject.SetActive(false));
    }
}
