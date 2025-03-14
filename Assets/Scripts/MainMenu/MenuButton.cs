using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static Vector3 newScale = new Vector3(1.1f, 1.1f, 1);
    private Vector3 oldScale;
    private static float animationDuration = 0.2f;

    public void Start()
    {
        oldScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, newScale, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, oldScale, animationDuration);
    }
}
