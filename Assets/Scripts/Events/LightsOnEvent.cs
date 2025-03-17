using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightsOnEvent : MonoBehaviour, MissionEventListener
{
    /// <summary>
    /// Objets comprenants le système de flashlight
    /// </summary>
    [SerializeField] private List<GameObject> _objects;
    [SerializeField] private float _fadeDuration = 0.1f;

    public void Start()
    {
        MissionEventManager.AddEventListener(this);
    }

    public void OnNotify(MissionEvent e)
    {
        if (e == MissionEvent.LightsOn)
        {
            foreach (GameObject obj in _objects)
            {
                obj.SetActive(false);
            }
        }
    }
}
