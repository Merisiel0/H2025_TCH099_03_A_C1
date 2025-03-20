using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Module : Interactable
{
    [SerializeField] private ModuleUI moduleUI;
    private bool opened = false;

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        if (opened)
        {
            moduleUI.DisableUI();
            opened = false;
        }
        else
        {
            moduleUI.gameObject.SetActive(true); // Protection en cas de problème
            moduleUI.EnableUI();
            opened = true;
        }
    }
}
