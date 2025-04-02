using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Module : Interactable, MissionEventListener
{
    [SerializeField] private ModuleUI moduleUI;
    [SerializeField] private GameObject alarmIndicator;
    [SerializeField] private MissionEvent startEvent;
    [SerializeField] private MissionEvent endEvent;

    [SerializeField] private bool allowOpenningAnytime = false;
    private bool isActive = false;

    private bool opened = false;

    protected override void Start()
    {
        base.Start();

        MissionEventManager.AddEventListener(this);

        if (alarmIndicator)
            ShowIndicator(false);
    }

    private void ShowIndicator(bool show)
    {
        alarmIndicator.SetActive(show);
    }

    public override bool Interact()
    {
        if (opened)
        {
            moduleUI.DisableUI();
            opened = false;
            return true;
        }
        else if(isActive || allowOpenningAnytime)
        {
            moduleUI.gameObject.SetActive(true); // Protection en cas de problème
            moduleUI.EnableUI();
            opened = true;
            return true;
        }
        return false;
    }

    public void OnNotify(MissionEvent e)
    {
        if(e == startEvent)
        {
            ShowIndicator(true);
            isActive = true;
        } 
        else if(e == endEvent)
        {
            ShowIndicator(false);
            isActive = false;
        }
    }
}
