using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe qui permet de g�rer la logique des boutons et des menus du menu principal (�cran d'acceuil)
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab; // Pr�fab pour un boutton d'option de niveau
    public GameObject levelButtonHolder; // Conteneur qui contient tous les bouttons d'option de niveau

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private FadeAnimation fadeTransition;
    private static float transitionDuration = 0.3f;

    private float openedMenuPosY;
    private float closedMenuPosY;

    private void Start()
    {
        // Inistialisaiton des posiitons ouverte et fermee des menus
        openedMenuPosY = 0.0f;
        closedMenuPosY = levelMenu.transform.localPosition.y;
    }

    public void OpenLevelMenu()
    {
        LeanTween.moveLocalY(mainMenu, closedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocalY(levelMenu, openedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
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

    public void OpenMainMenu()
    {
        LeanTween.moveLocalY(mainMenu, openedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocalY(levelMenu, closedMenuPosY, transitionDuration).setEase(LeanTweenType.easeOutCubic);
    }

    /// <summary>
    /// Fonction qui r�cup�re la liste de niveau � partir de la base de donn�es et qui affiche la liste des niveaux disponibles dans
    /// le menu de s�l�ction du niveau
    /// </summary>
    public void LoadLevelsData()
    {
        foreach(Transform child in levelButtonHolder.transform)
        {
            Destroy(child.gameObject);
        }

        ApiController.FetchDataFromAPI("api/v1/niveaux", (response) =>
        {
            LevelDataWrapper data = null;
            try
            {
                data = JsonUtility.FromJson<LevelDataWrapper>("{\"levels\":" + response + "}");
            }
            catch (Exception e) { Debug.Log(e.Message); }

            if (response != null && data != null)
            {
                foreach (LevelData level in data.levels)
                {
                    LevelOptionButton levelOption = Instantiate(levelButtonPrefab).GetComponent<LevelOptionButton>();
                    levelOption.transform.SetParent(levelButtonHolder.transform);
                    levelOption.Init(this, level);
                }
            } else
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
    }
}
