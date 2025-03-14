using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public static List<Interactable> allInteractables = new List<Interactable>();

    protected virtual void Start()
    {
        allInteractables.Add(this);
    }

    public abstract void Interact();
}
