using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, MissionEventListener
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip moduleFailedSound;

    [SerializeField] private FadeAnimation fade;
    [SerializeField] private FadeAnimation fadeOverlay;

    [SerializeField] private string successText = "Réussite de la mission";
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private string failText = "Échec de la mission";
    [SerializeField] private Color failColor = Color.red;

    [SerializeField] private CanvasGroup menuButton;
    [SerializeField] private TextMeshProUGUI missionStatusText;
    [SerializeField] private TextMeshProUGUI survivedTimeText;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();

        MissionEventManager.AddEventListener(this);
    }

    public void OnNotify(MissionEvent e)
    {
        if(e == MissionEvent.PlayerEventFailed)
        {
            EndMission(false);
        }
    }

    public void EndMission(bool success)
    {
        missionStatusText.text = success ? successText : failText;
        missionStatusText.color = success ? successColor : failColor;

        menuButton.interactable = true;
        menuButton.blocksRaycasts = true;

        int survivedTime = GameTimer.instance.totalTime - GameTimer.instance.remainingTime;
        string minutes = (survivedTime / 60).ToString();
        minutes = minutes.PadLeft(2, '0');
        string seconds = (survivedTime % 60).ToString();
        seconds = seconds.PadLeft(2, '0');
        survivedTimeText.SetText(minutes + ":" + seconds); ;

        StartCoroutine(EndMissionDelay());
    }

    private IEnumerator EndMissionDelay()
    {
        yield return new WaitForSeconds(3.0f);
        fade.fadeIn = true;
        fade.Fade();
    }

    public void BackToMainMenu()
    {
        fadeOverlay.fadeIn = true;
        fadeOverlay.Fade(() => {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
