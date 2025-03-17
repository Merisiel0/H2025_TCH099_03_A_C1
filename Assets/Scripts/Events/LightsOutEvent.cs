using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackoutEvent : MonoBehaviour, MissionEventListener
{
    /// <summary>
    /// Objets comprenants le système de flashlight
    /// </summary>
    [SerializeField] private List<GameObject> _objects;

    public void Start()
    {
        MissionEventManager.AddEventListener(this);
    }

    public void OnNotify(MissionEvent e)
    {
        if (e == MissionEvent.LightsOut)
        {
            foreach (GameObject obj in _objects)
            {
                obj.SetActive(true);
            }
        }
    }
}
