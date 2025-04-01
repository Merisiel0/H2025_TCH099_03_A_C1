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

    public void Init(ModuleUI module)
    {
        module.InitModule(nom, duree);
        EventNotificationController.PushNotification(description, duree);
    }
}

// API Responses templates for each modules
[System.Serializable]
public class WiresEventResponse
{
    public T module;
    public EventResponse eventData;
}

[System.Serializable]
public class BipolarityEventResponse
{
    public BipolarityModuleData module;
    public EventResponse eventData;
}

[System.Serializable]
public class LightsEventResponse
{
    public LevitatingLightsModuleData module;
    public EventResponse eventData;
}

[System.Serializable]
public class PatPlayEventResponse
{
    public PatPlayModuleData module;
    public EventResponse eventData;
}

