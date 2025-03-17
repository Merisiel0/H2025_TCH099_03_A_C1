using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Liste des �venements possibles dans le jeu durant une mission
/// </summary>
public enum MissionEvent
{
    LightsOut,
    LightsOn,
    ThrustersShutdown,
    ThrustersStart,
}

/// <summary>
/// Classe de style sujet observ� qui permet d'envoyer les objets � ses abonn�es (des observateurs) lorsqu'un �v�nement est lanc�.
/// </summary>
public class MissionEventManager : MonoBehaviour
{
    private static MissionEventManager instance; // Instance du singleton statique

    private List<MissionEventListener> listeners; // Liste des observeurs

    private void Awake()
    {
        // Cr�ation du signleton
        if (instance == null) instance = this;
        else Destroy(this);

        // Cr�ation de la liste d'observeurs
        listeners = new List<MissionEventListener>();
    }

    /// <summary>
    /// Fonction qui permet de lancer et de distribuer un �v�nement � tous les observateurs.
    /// </summary>
    /// <param name="e">Objet de type MissionEvent qui correspond � un �v�enement durant une mission.</param>
    public static void SendEvent(MissionEvent e)
    {
        // Envoie � chacun des observeur.
        foreach(MissionEventListener listener in instance.listeners)
        {
            listener.OnNotify(e);
        }
    }

    /// <summary>
    /// Fonction qui permet � un observeur de s'inscrire comme un abonn�.
    /// </summary>
    /// <param name="listener">L'observeur � ajouter � la liste d'abonn�</param>
    public static void AddEventListener(MissionEventListener listener)
    {
        instance.listeners.Add(listener);
    }
}

/// <summary>
/// Interface qu'un observeur peut impl�menter pour �couter les �v�nements en cours dans la mission.
/// </summary>
public interface MissionEventListener
{
    /// <summary>
    /// Fonction appel� lors du lancement d'un �v�nement.
    /// </summary>
    /// <param name="e">L'�v�nement lanc�.</param>
    public void OnNotify(MissionEvent e);
}
