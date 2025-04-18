using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MissionManager : MonoBehaviour, MissionEventListener
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip moduleFailedSound;
    [SerializeField] private AudioClip missionSuccessSound;

    [SerializeField] private FadeAnimation fade;
    [SerializeField] private FadeAnimation fadeOverlay;

    [SerializeField] private string successText = "Réussite de la mission";
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private string failText = "Échec de la mission";
    [SerializeField] private Color failColor = Color.red;

    [SerializeField] private CanvasGroup menuButton;
    [SerializeField] private TextMeshProUGUI missionStatusText;
    [SerializeField] private TextMeshProUGUI survivedTimeText;

    [SerializeField] private GameObject[] disableOnGameEnd;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInteract playerInteract;

    private bool gameHasEnded = false;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();

        MissionEventManager.AddEventListener(this);
    }

    public void OnNotify(MissionEvent e)
    {
        if (gameHasEnded) return;

        if(e == MissionEvent.PlayerEventFailed)
        {
            EndMission(false);
        }
    }

    public void EndMission(bool success)
    {
        gameHasEnded = true;

        playerInteract.enabled = false;
        playerController.enabled = false;

        missionStatusText.text = success ? successText : failText;
        missionStatusText.color = success ? successColor : failColor;

        menuButton.interactable = true;
        menuButton.blocksRaycasts = true;

        int survivedTime = (int) (Time.time - GameTimer.instance.startTime);
        string minutes = (survivedTime / 60).ToString();
        minutes = minutes.PadLeft(2, '0');
        string seconds = (survivedTime % 60).ToString();
        seconds = seconds.PadLeft(2, '0');
        survivedTimeText.SetText(minutes + ":" + seconds); ;

        StartCoroutine(EndMissionDelay(success));
    }

    private IEnumerator EndMissionDelay(bool success)
    {
        yield return new WaitForSeconds(3.0f);
        fade.fadeIn = true;
        fade.Fade();

        AudioClip clip = success ? missionSuccessSound : moduleFailedSound;
        audioSource.PlayOneShot(clip);

        playerController.enabled = false;
    }

    public void BackToMainMenu()
    {
        fadeOverlay.fadeIn = true;
        fadeOverlay.Fade(() => {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
