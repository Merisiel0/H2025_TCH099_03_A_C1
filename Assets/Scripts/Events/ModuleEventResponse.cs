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
    public string matricule;
}

// API Responses templates for each modules
[System.Serializable]
public class ModuleEventRespone<T>
{
    public T module;
    public EventResponse eventData;

    public void Init(ModuleUI module)
    {
        module.InitModule(eventData.matricule, eventData.duree);
        EventNotificationController.PushNotification(eventData.description, eventData.duree);
    }
}