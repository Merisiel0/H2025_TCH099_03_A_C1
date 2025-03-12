using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionEvent
{
    Blackout,
    ThrustersShutdown
}

public class MissionEventManager : MonoBehaviour
{
    private static MissionEventManager instance;

    private List<MissionEventListener> listeners;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        listeners = new List<MissionEventListener>();
    }

    public static void SendEvent(MissionEvent e)
    {
        foreach(MissionEventListener listener in instance.listeners)
        {
            listener.OnNotify(e);
        }
    }

    public static void AddEventListener(MissionEventListener listener)
    {
        instance.listeners.Add(listener);
    }
}

public class MissionEventListener : MonoBehaviour
{
    public virtual void OnNotify(MissionEvent e) { };
}
