using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel;

/// <summary>
/// Liste des ï¿½venements possibles dans le jeu durant une mission
/// </summary>
public enum MissionEvent
{
    [Description("Le module a été endommagé")]
    PlayerEventFailed,

    [Description("Panne des générateurs")]
    LightsOut,
    [Description("Redémarrage des générateurs")]
    LightsOn,

    [Description("Panne des propulseurs")]
    ThrustersShutdown,
    [Description("Redémarrage des propulseurs")]
    ThrustersStart,

    [Description("Un fusible vient de lâcher !")]
    ElectricFailure,
    [Description("Le système électrique est redémarré.")]
    ElectricRestart
}

/// <summary>
/// Classe de style sujet observï¿½ qui permet d'envoyer les objets ï¿½ ses abonnï¿½es (des observateurs) lorsqu'un ï¿½vï¿½nement est lancï¿½.
/// </summary>
public class MissionEventManager : MonoBehaviour
{
    /// <summary>
    /// Liste qui contient tous les events principaux, notament les déclencheurs et non les évents interne
    /// </summary>
    private static MissionEvent[] importantEvents = {
        MissionEvent.LightsOn,
    };

    private static MissionEventManager instance; // Instance du singleton statique

    private List<MissionEventListener> listeners; // Liste des observeurs

    public static bool IsImportant(MissionEvent targetEvent)
    {
        if(importantEvents.Any(importantEvent => targetEvent == importantEvent))
        {
            return true;
        }
        return false;
    }

    private void Awake()
    {
        // Crï¿½ation du signleton
        if (instance == null) instance = this;
        else Destroy(this);

        // Crï¿½ation de la liste d'observeurs
        listeners = new List<MissionEventListener>();
    }

    /// <summary>
    /// Fonction qui permet de lancer et de distribuer un ï¿½vï¿½nement ï¿½ tous les observateurs.
    /// </summary>
    /// <param name="e">Objet de type MissionEvent qui correspond ï¿½ un ï¿½vï¿½enement durant une mission.</param>
    public static void SendEvent(MissionEvent e)
    {
        // Envoie ï¿½ chacun des observeur.
        foreach(MissionEventListener listener in instance.listeners)
        {
            listener.OnNotify(e);
        }
    }

    /// <summary>
    /// Fonction qui permet ï¿½ un observeur de s'inscrire comme un abonnï¿½.
    /// </summary>
    /// <param name="listener">L'observeur ï¿½ ajouter ï¿½ la liste d'abonnï¿½</param>
    public static void AddEventListener(MissionEventListener listener)
    {
        instance.listeners.Add(listener);
    }
}

/// <summary>
/// Interface qu'un observeur peut implï¿½menter pour ï¿½couter les ï¿½vï¿½nements en cours dans la mission.
/// </summary>
public interface MissionEventListener
{
    /// <summary>
    /// Fonction appelï¿½ lors du lancement d'un ï¿½vï¿½nement.
    /// </summary>
    /// <param name="e">L'ï¿½vï¿½nement lancï¿½.</param>
    public void OnNotify(MissionEvent e);
}
