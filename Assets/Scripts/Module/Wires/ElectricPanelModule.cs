using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricPanelModule : ModuleUI, MissionEventListener
{
    private WireModuleData data;

    [SerializeField] private GameObject wirePrefab;
    [SerializeField] private Sprite[] wireImages;
    [SerializeField] private Transform[] wireContainers = new Transform[6];
    [SerializeField] private Wire[] wires = new Wire[6];

    public void Start()
    {
        base.Start();

        data = new WireModuleData();
        MissionEventManager.AddEventListener(this);
    }

    public void InitModule(WireModuleData data)
    {
        foreach(Wire wire in wires)
        {
            if(wire)
            {
                Destroy(wire.gameObject);
            }
        }

        this.data = data;

        data.Init(); // On s'assure que la data soit bien à jours
        for(int i = 0; i < data.nbFils; i++)
        {
            if (data.couleurs[i] != "NULL")
            {
                Color color = HexToColor.FromHex(data.couleurs[i]);
                GameObject wireObj = Instantiate(wirePrefab);
                wireObj.transform.SetParent(wireContainers[i]);

                Sprite sprite = wireImages[Random.Range(0, wireImages.Length - 1)];
                Wire wire = wireObj.GetComponent<Wire>();
                wire.Init(this, sprite, color, i);
                wires[i] = wire;
            }
        }
    }

    public void CutWire(int index)
    {
        Destroy(wires[index].gameObject);

        if(index != data.solution)
        {
            MissionEventManager.SendEvent(MissionEvent.PlayerEventFailed);
            PlayerInteract.StopInteractions();
        } 
        else
        {
            MissionEventManager.SendEvent(MissionEvent.ElectricRestart);
            PlayerInteract.StopInteractions();
        }
    }

    public void OnNotify(MissionEvent e)
    {
        if(e == MissionEvent.ElectricFailure)
        {
            // TODO MODIFIER POUR ACCEPTERS LES VRAI EVENTS
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
}
