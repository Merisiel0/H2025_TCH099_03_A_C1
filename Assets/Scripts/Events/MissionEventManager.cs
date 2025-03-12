using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Liste des évenements possibles dans le jeu durant une mission
/// </summary>
public enum MissionEvent
{
    Blackout,
    ThrustersShutdown,
    ThrustersStart,
}

/// <summary>
/// Classe de style sujet observé qui permet d'envoyer les objets à ses abonnées (des observateurs) lorsqu'un événement est lancé.
/// </summary>
public class MissionEventManager : MonoBehaviour
{
    private static MissionEventManager instance; // Instance du singleton statique

    private List<MissionEventListener> listeners; // Liste des observeurs

    private void Awake()
    {
        // Création du signleton
        if (instance == null) instance = this;
        else Destroy(this);

        // Création de la liste d'observeurs
        listeners = new List<MissionEventListener>();
    }

    /// <summary>
    /// Fonction qui permet de lancer et de distribuer un événement à tous les observateurs.
    /// </summary>
    /// <param name="e">Objet de type MissionEvent qui correspond à un évéenement durant une mission.</param>
    public static void SendEvent(MissionEvent e)
    {
        // Envoie à chacun des observeur.
        foreach(MissionEventListener listener in instance.listeners)
        {
            listener.OnNotify(e);
        }
    }

    /// <summary>
    /// Fonction qui permet à un observeur de s'inscrire comme un abonné.
    /// </summary>
    /// <param name="listener">L'observeur à ajouter à la liste d'abonné</param>
    public static void AddEventListener(MissionEventListener listener)
    {
        instance.listeners.Add(listener);
    }
}

/// <summary>
/// Interface qu'un observeur peut implémenter pour écouter les événements en cours dans la mission.
/// </summary>
public interface MissionEventListener
{
    /// <summary>
    /// Fonction appelé lors du lancement d'un événement.
    /// </summary>
    /// <param name="e">L'événement lancé.</param>
    public void OnNotify(MissionEvent e);
}
