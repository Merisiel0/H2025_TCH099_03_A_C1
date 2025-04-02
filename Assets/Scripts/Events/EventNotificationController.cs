using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EventNotificationController : MonoBehaviour
{
    public static EventNotificationController instance { get; private set; }

    private List<EventNotification> eventList;

    [SerializeField] private GameObject notificationPrefab;
    public Color dangerColor = Color.yellow;

    public void Awake()
    {
        instance = this;

        eventList = new List<EventNotification>();
    }

    public static void PushNotification(string name, int duration)
    {
        instance.AddEVent(name, duration);
    }

    public void EndEvent(EventNotification e)
    {
        eventList.Remove(e);
        Destroy(e.gameObject);
    }

    private void AddEVent(string name, int duration)
    {
        EventNotification notification = Instantiate(notificationPrefab).GetComponent<EventNotification>();
        notification.transform.SetParent(transform);
        notification.Init(this, name, duration);
        eventList.Add(notification);
    }
}
