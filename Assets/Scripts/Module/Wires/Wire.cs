using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Wire : MonoBehaviour, IPointerClickHandler
{
    private int index;
    private Image image;
    private ElectricPanelModule controller;

    public void Init(ElectricPanelModule controller, Sprite sprite, Color color, int index)
    {
        image = GetComponent<Image>();

        this.controller = controller;
        this.index = index;

        image.sprite = sprite;
        image.color = color;
        image.rectTransform.localPosition = Vector3.zero;
        image.rectTransform.localScale = Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.CutWire(index);
    }
}
