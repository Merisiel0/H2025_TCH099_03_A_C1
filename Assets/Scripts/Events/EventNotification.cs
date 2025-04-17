using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class EventNotification : MonoBehaviour, MissionEventListener
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI durationText;
    private EventNotificationController controller;
    private MissionEvent endEvent;

    private int duration;

    private CanvasGroup canvasGroup;

    /// <summary>
    /// Récup des composant et fade-in
    /// </summary>
    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        LeanTween.alphaCanvas(canvasGroup, 1.0f, 0.2f);
    }

    /// <summary>
    /// Retrait de l'écoute d'events et suppresion de l'objet
    /// </summary>
    public void EndEvent()
    {
        LeanTween.alphaCanvas(canvasGroup, 0.0f, 0.3f).setOnComplete(()=>
        {
            MissionEventManager.RemoveEventListener(this);
            controller.EndEvent(this);
        });
    }

    /// <summary>
    /// Creation de la notification et MAJ de ses paramètres
    /// </summary>
    /// <param name="controller">Le controlleur des notifiactions</param>
    /// <param name="name">Le titre de la notification</param>
    /// <param name="duration">Le temps en seconde de la notification</param>
    /// <param name="endEvent">L'evenement qui peut causer la suppresion de la notification</param>
    public void Init(EventNotificationController controller, string name, int duration, MissionEvent endEvent)
    {
        MissionEventManager.AddEventListener(this);

        this.controller = controller;
        this.duration = duration;
        this.endEvent = endEvent;
        UpdateDurationText();

        StartCoroutine(ExecuteTimer());

        nameText.SetText(name);
    }

    /// <summary>
    /// Mise à jour du texte qui affiche la durée restante
    /// </summary>
    private void UpdateDurationText()
    {
        if(duration <= 5)
        {
            durationText.color = controller.dangerColor;
        }

        durationText.SetText(duration + "s");
    }

    /// <summary>
    /// Boucle asynchrone qui calcule le temps restant et qui compte les secondes
    /// </summary>
    /// <returns></returns>
    IEnumerator ExecuteTimer()
    {
        if (duration > 0)
        {
            yield return new WaitForSeconds(1.0f);
            duration -= 1;
            UpdateDurationText();
            StartCoroutine(ExecuteTimer());
        }
        else
        {
            yield return null;
            EndEvent();
        }
    }

    /// <summary>
    /// Fonction qui écoute les événements lancé dans le jeu et écoute pour l'évent qui met fin à la notification
    /// </summary>
    /// <param name="e">Evenement lancé</param>
    public void OnNotify(MissionEvent e)
    {
        Debug.Log("notified " + e + "-> " + endEvent);
        if(e == endEvent)
        {
            EndEvent();
        }
    }
}
