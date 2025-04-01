using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventResponse
{
    public int id_;
    public string nom;
    public string description;
    public int duree;
    public string couleur;
    public string typeModule;
}

[System.Serializable]
public class ModuleEventResponse<T>
{
    public T module;
    public EventResponse eventData;

    public void Init()
    {
        EventNotificationController.PushNotification(eventData.description, eventData.duree);
    }
}
