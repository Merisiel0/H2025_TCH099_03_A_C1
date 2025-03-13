using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui permet de gérer la logique des boutons et des menus du menu principal (écran d'acceuil)
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab; // Préfab pour un boutton d'option de niveau
    public GameObject levelButtonHolder; // Conteneur qui contient tous les bouttons d'option de niveau

    /// <summary>
    /// Fonction qui récupère la liste de niveau à partir de la base de données et qui affiche la liste des niveaux disponibles dans
    /// le menu de séléction du niveau
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
                    levelOption.Init(level);
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
