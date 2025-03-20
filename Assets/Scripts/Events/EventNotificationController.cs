using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EventNotificationController : MonoBehaviour, MissionEventListener
{
    private List<EventNotification> eventList;

    [SerializeField] private GameObject notificationPrefab;
    public Color dangerColor = Color.yellow;

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
        // TODO: RETIRER LES TEST
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MissionEventManager.SendEvent(MissionEvent.ThrustersShutdown);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MissionEventManager.SendEvent(MissionEvent.ThrustersStart);
        }
    }

    public void OnNotify(MissionEvent e)
    {
        if (MissionEventManager.IsImportant(e))
        {
            AddEVent(e);
        }
    }

    public void EndEvent(EventNotification e)
    {
        eventList.Remove(e);
        Destroy(e.gameObject);
    }

    private void AddEVent(MissionEvent e)
    {
        EventNotification notification = Instantiate(notificationPrefab).GetComponent<EventNotification>();
        notification.transform.SetParent(transform);
        notification.Init(this, Enumerations.GetDescription(e), 5);
        eventList.Add(notification);
    }
}
