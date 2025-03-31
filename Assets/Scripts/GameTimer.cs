using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/// <summary>
/// Classe singleton qui s'occupe de g�rer le minuteur de la partie au cours d'une mission
/// </summary>
public class GameTimer : MonoBehaviour
{
    public static GameTimer instance { get; private set; }

    public float startTime { get; private set; }

    public int totalTime = 360;
    public int remainingTime { get; private set; }
    public bool isPaused { get; private set; }

    public UnityEvent onTimerEnd; // Callback lorsque le minuteur � atteint le temps 0;

    private Color regularTextColor; // Couleur normale du texte
    public Color onTimerEndColor = Color.green; // Couleur lorsque le temps est �coul�
    public Color onPausedColor = Color.yellow; // Couleur lorsque le minuteur est sur pause

    public TMPro.TextMeshProUGUI timerText;

    private void Awake()
    {
        // Init du singleton
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (LevelDataObject.Exists())
        {
            totalTime = LevelDataObject.Get().duration;
        }

        // Initialisation du minuteur
        remainingTime = totalTime;
        regularTextColor = timerText.color;
        UpdateTimerText();

        // On commence le d�compte
        startTime = Time.time;
        StartCoroutine(ExecuteTimer());
    }


    /// <summary>
    /// Fonction qui g�re la mise en pause ou non du minuteur. On vient notament changer la couleurs du texte.
    /// </summary>
    /// <param name="paused">Vrai si on met le minuteur sur pause, Faux si on r�sume.</param>
    public static void Pause(bool paused)
    {
        instance.isPaused = paused;
        if(instance.isPaused)
        {
            instance.timerText.color = instance.onPausedColor;
        } else
        {
            instance.timerText.color = instance.regularTextColor;
        }
    }

    /// <summary>
    /// Fonction qui met � jours le texte qui affiche le temps r�stant au minuteurs en convertissant le nombre de
    /// secondes au format MM:SS (MM = minutes sur deux chiffres et SS = secondes sur deux chiffres)
    /// </summary>
    private void UpdateTimerText()
    {
        string minutes = (remainingTime / 60).ToString();
        minutes = minutes.PadLeft(2, '0');

        string seconds = (remainingTime % 60).ToString();
        seconds = seconds.PadLeft(2, '0');

        timerText.SetText(minutes + ":" + seconds); ;
    }

    /// <summary>
    /// Fonction Asynchrone qui fait le d�compte du temps. Se r�p�te chaque seconde et s'arr�te � la fin du d�compte en plus d'appeler le callback
    /// pr�vu � cet effet. Le d�compte ne s'effectue par lorsque le minuteur est sur pause.
    /// </summary>
    /// <returns></returns>
    IEnumerator ExecuteTimer()
    {
        if(remainingTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            if(!isPaused)
            {
                remainingTime -= 1;
                UpdateTimerText();
            }

            StartCoroutine(ExecuteTimer());
        } 
        else
        {
            yield return null;
            onTimerEnd.Invoke();
            timerText.color = onTimerEndColor;
        }
    }
}
