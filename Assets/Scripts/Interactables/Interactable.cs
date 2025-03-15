using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour 
{
    public static List<Interactable> allInteractables = new List<Interactable>();

    public bool isTrigger { get; private set; }
    [SerializeField] private GameObject outline;

    protected virtual void Start()
    {
        allInteractables.Add(this);
    }

    public virtual void OnPlayerTriggerStart() 
    {
        isTrigger = true;
        if(outline)
        {
            outline.SetActive(true);
        }
    }

    public virtual void OnPlayerTriggerEnd() 
    {
        Debug.Log(name);
        isTrigger = false;
        if (outline)
        {
            outline.SetActive(false);
        }
    }

    public abstract void Interact();
}
