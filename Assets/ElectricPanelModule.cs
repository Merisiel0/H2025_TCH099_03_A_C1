using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricPanelModule : MonoBehaviour, MissionEventListener
{
    private WireModuleData data;

    [SerializeField] private GameObject wirePrefab;
    [SerializeField] private Sprite[] wireImages;
    [SerializeField] private Transform[] wireContainers;

    public void Start()
    {
        data = new WireModuleData();
        MissionEventManager.AddEventListener(this);
    }

    public void InitModule(WireModuleData data)
    {
        this.data = data;

        data.Init(); // On s'assure que la data soit bien à jours
        for(int i = 0; i < data.nbFils; i++)
        {
            if (data.couleurs[i] != "NULL")
            {
                Debug.Log(data.couleurs[i]);
                Color color = HexToColor.FromHex(data.couleurs[i]);
                GameObject wire = Instantiate(wirePrefab);
                wire.transform.parent = wireContainers[i];
                Image image = wire.GetComponent<Image>();
                image.sprite = wireImages[i];
                image.color = color;
                image.rectTransform.localPosition = Vector3.zero;
                image.rectTransform.localScale = Vector3.one;
            }
        }
    }

    public void OnNotify(MissionEvent e)
    {
        WireModuleData myData = new WireModuleData();
        myData.nbFils = 6;
        myData.couleurFil1 = "FF0000";
        myData.couleurFil2 = "FF0000";
        myData.couleurFil3 = "FF0000";
        myData.couleurFil4 = "FF0000";
        myData.couleurFil5 = "FF0000";
        myData.couleurFil6 = "FF0000";
        myData.solution = 1;
        InitModule(myData);
    }
}
