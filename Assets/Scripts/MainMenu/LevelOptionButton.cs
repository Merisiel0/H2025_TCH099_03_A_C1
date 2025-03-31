using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

/// <summary>
/// Classe qui correspond � une option de niveau dans le menu principal de selection.
/// Permet de remplir les champs de texte en fonction d'un objet de type LevelData qui contient tous les champs n�cessaires.
/// Permet aussi d'afficher un texte en cas d'erreur.
/// </summary>
public class LevelOptionButton : MonoBehaviour, IPointerClickHandler
{
    private LevelData levelData;
    private MainMenu mainMenu;

    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI durationText;

    public GameObject regularLayout;
    public GameObject errorLayout;

    /// <summary>
    /// Initialiser l'objet comme un erreur. On affiche uniquement le layout d'erreur.
    /// </summary>
    public void InitAsError()
    {
        regularLayout.SetActive(false);
        errorLayout.SetActive(true);
    }

    /// <summary>
    /// On initialise les champs de texte avec un object LevelData qui contient les informations pour un niveau.
    /// </summary>
    /// <param name="data">Les informations du niveau.</param>
    public void Init(MainMenu mainMenu, LevelData data)
    {
        this.mainMenu = mainMenu;
        this.levelData = data;

        headerText.SetText(data.nom + " - " + data.duree);
        descriptionText.SetText(data.description);

        string minutes = (data.duree / 60).ToString();
        minutes = minutes.PadLeft(1, '0');
        string seconds = (data.duree % 60).ToString();
        seconds = seconds.PadLeft(2, '0');
        durationText.SetText(minutes + ":" + seconds);

        // On set la couleurs
        Color color = HexToColor.FromHex(data.couleur);
        headerText.color = color;
        descriptionText.color = color;
        durationText.color = color;
    }

    /// <summary>
    /// Fonction appelé lorsque l'on clique sur le boutton d'option de niveau et qui s'occupe de charger le niveau et de lancer la partie
    /// </summary>
    /// 
    public void OnPointerClick(PointerEventData eventData)
    {
        mainMenu.LoadLevel(levelData);
    }
}
