using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ElectricPanelModule : ModuleUI, MissionEventListener
{
    private static string apiUrl = "/api/v1/module?module=wires";

    public override MissionEvent startEvent => MissionEvent.ElectricFailure;
    public override MissionEvent endEvent => MissionEvent.ElectricRestart;

    private AudioSource source;

    private WireModuleData data;

    [SerializeField] private GameObject wirePrefab;
    [SerializeField] private Sprite[] wireImages;
    [SerializeField] private Transform[] wireContainers = new Transform[6];
    [SerializeField] private Wire[] wires = new Wire[6];
    private int solution = 0;
        
    public void Start()
    {
        base.Start();

        source = GetComponent<AudioSource>();

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

        data.Init(); // On s'assure que la data soit bien � jours
        int count = 0;
        for(int i = 0; i < wires.Length; i++)
        {
            if (data.couleurs[i] != "")
            {
                count++;
                if(count == data.solution)
                {
                    solution = i;
                }

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

        source.Play();

        if(index != solution)
        {
            MissionManager.SetFailCause("Vous avez coup� le mauvais fil.");
            MissionEventManager.SendEvent(MissionEvent.PlayerEventFailed);
            PlayerInteract.StopInteractions();
        }
        else
        {
            MissionEventManager.SendEvent(endEvent);
            ModuleSucess();
            PlayerInteract.StopInteractions();
        }
    }

    public void OnNotify(MissionEvent e)
    {
        if(e == startEvent)
        {
            ApiController.FetchDataFromAPI<ModuleEventRespone<WireModuleData>>(apiUrl, (data) => {
                data.Init(this);
                InitModule(data.module);
            });

            // UNCOMMENT THIS PART FOR MANUAL MODULE DATA CREATIONS
            //WireModuleData myData = new WireModuleData();
            //myData.nbFils = 6;
            //myData.couleurFil1 = "FF0000";
            //myData.couleurFil2 = "FF0000";
            //myData.couleurFil3 = "FF0000";
            //myData.couleurFil4 = "FF0000";
            //myData.couleurFil5 = "FF0000";
            //myData.couleurFil6 = "FF0000";
            //myData.solution = 1;
            //InitModule(myData);
        }
    }
}
