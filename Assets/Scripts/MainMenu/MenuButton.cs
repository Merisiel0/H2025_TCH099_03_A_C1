using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 scaleModifier = new Vector3(1.1f, 1.1f, 1);
    private Vector3 newScale;
    private Vector3 oldScale;
    private static float animationDuration = 0.1f;

    public void Start()
    {
        oldScale = transform.localScale;
        newScale = oldScale;
        newScale.x = newScale.x * scaleModifier.x;
        newScale.y = newScale.y * scaleModifier.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, newScale, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, oldScale, animationDuration);
    }
}
