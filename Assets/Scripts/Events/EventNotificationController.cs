using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNotificationController : MonoBehaviour, MissionEventListener
{
    private List<EventNotification> eventList;

    [SerializeField] private GameObject notificationPrefab;

    public void Awake()
    {
        eventList = new List<EventNotification>();
    }

    public void Start()
    {
        MissionEventManager.AddEventListener(this);
    }

    public void Update()
    {
        // TODO: RETIRER LE TEST
        if(Input.GetKeyDown("1"))
        {
            MissionEventManager.SendEvent(MissionEvent.Blackout);
        }
    }

    public void OnNotify(MissionEvent e)
    {
        if(MissionEventManager.IsImportant(e))
        {
            AddEVent(e);
        }
    }

    private void AddEVent(MissionEvent e)
    {
        EventNotification notification = Instantiate(notificationPrefab).GetComponent<EventNotification>();
        notification.transform.SetParent(transform);
        // TODO: Récuperer la durée avec l'api
        notification.Init(e.ToString(), 30);
        eventList.Add(notification);
    }
}
