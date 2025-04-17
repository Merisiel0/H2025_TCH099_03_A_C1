using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

/// <summary>
/// Classe qui permet de g�rer la logique des boutons et des menus du menu principal (�cran d'acceuil)
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private Button connectionMenuButton;
    [SerializeField] private GameObject disconnectButton;
    [SerializeField] private GameObject mainMenuSignupButton;

    [Header("Level Menu UI")]
    public GameObject levelButtonPrefab; // Pr�fab pour un boutton d'option de niveau
    public GameObject levelButtonHolder; // Conteneur qui contient tous les bouttons d'option de niveau

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject connexionMenu;
    [SerializeField] private GameObject inscriptionMenu;
    private GameObject currentlyOpenedMenu;

    [Header("Animations")]
    [SerializeField] private FadeAnimation fadeTransition;
    private static float transitionDuration = 0.3f;

    private float openedMenuPosY;
    private float closedMenuPosY;

    private void Awake()
    {
        // Setup l'application pour lorsqu'elle n'est plus focus, le jeu continue
        Application.runInBackground = true;
    }

    private void Start()
    {
        // Inistialisaiton des positons ouverte et fermee des menus
        openedMenuPosY = 0.0f;
        closedMenuPosY = levelMenu.transform.localPosition.y;
        currentlyOpenedMenu = mainMenu;

        if (UserConnectionObject.Exists())
        {
            connectionMenuButton.interactable = false;
            disconnectButton.SetActive(true);
            mainMenuSignupButton.SetActive(UserConnectionObject.Get().admin);
        }
        else
        {
            connectionMenuButton.interactable = true;
            disconnectButton.SetActive(false);
            mainMenuSignupButton.SetActive(false);
        }
    }

    public void OpenMainMenu()
    {
        if(UserConnectionObject.Exists())
        {
            connectionMenuButton.interactable = false;
            disconnectButton.SetActive(true);
            mainMenuSignupButton.SetActive(UserConnectionObject.Get().admin);
        } else
        {
            connectionMenuButton.interactable = true;
            disconnectButton.SetActive(false);
            mainMenuSignupButton.SetActive(false);
        }

        // open menu
        LeanTween.moveLocalY(mainMenu, openedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        // close currently opened menu
        LeanTween.moveLocalY(currentlyOpenedMenu, closedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        // set newly opened menu
        currentlyOpenedMenu = mainMenu;
    }

    public void OpenLevelMenu()
    {
        LeanTween.moveLocalY(levelMenu, openedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocalY(currentlyOpenedMenu, closedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        currentlyOpenedMenu = levelMenu;
    }

    public void OpenConnexionMenu()
    {
        LeanTween.moveLocalY(connexionMenu, openedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocalY(currentlyOpenedMenu, closedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        currentlyOpenedMenu = connexionMenu;
    }

    public void OpenInscriptionMenu()
    {
        LeanTween.moveLocalY(inscriptionMenu, openedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocalY(currentlyOpenedMenu, closedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        currentlyOpenedMenu = inscriptionMenu;
    }

    public void LoadLevel(LevelData data)
    {
        fadeTransition.Fade(() =>
        {
            LevelDataObject dataContainer = new GameObject("LevelDataContainer")
                .AddComponent<LevelDataObject>()
                .Init(data);

            SceneManager.LoadScene("MainScene");
        });
    }

    /// <summary>
    /// Fonction qui r�cup�re la liste de niveau � partir de la base de donn�es et qui affiche la liste des niveaux disponibles dans
    /// le menu de s�l�ction du niveau
    /// </summary>
    public void LoadLevelsData()
    {
        foreach (Transform child in levelButtonHolder.transform)
        {
            Destroy(child.gameObject);
        }

        ApiController.FetchDataFromAPI("api/v1/niveaux", (response) =>
        {
            LevelData[] levels = null;
            try
            {
                levels = JsonConvert.DeserializeObject<LevelData[]>(response);
            }
            catch (Exception e) { Debug.Log(e.Message); }

            if (response != null && levels != null)
            {
                // Trier les niveuax en fonctoin de la durée.
                List<LevelData> sortedLevels = levels.OrderBy(level => level.duree).ToList();
                
                foreach (LevelData level in sortedLevels)
                {
                    LevelOptionButton levelOption = Instantiate(levelButtonPrefab).GetComponent<LevelOptionButton>();
                    levelOption.transform.SetParent(levelButtonHolder.transform);
                    levelOption.Init(this, level);
                }
            }
            else
            {
                LevelOptionButton levelOption = Instantiate(levelButtonPrefab).GetComponent<LevelOptionButton>();
                levelOption.transform.SetParent(levelButtonHolder.transform);
                levelOption.InitAsError();
            }
        });
    }

    /// <summary>
    /// Fonction pour quitter l'application lors de l'appuis du bouton "quitter"
    /// </summary>
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
