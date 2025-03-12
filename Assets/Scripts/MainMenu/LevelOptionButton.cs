using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe qui correspond à une option de niveau dans le menu principal de selection.
/// Permet de remplir les champs de texte en fonction d'un objet de type LevelData qui contient tous les champs nécessaires.
/// Permet aussi d'afficher un texte en cas d'erreur.
/// </summary>
public class LevelOptionButton : MonoBehaviour
{
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
    public void Init(LevelData data)
    {
        headerText.SetText(data.nom + " - " + data.difficulty);
        descriptionText.SetText(data.description);

        string minutes = (data.duration / 60).ToString();
        minutes = minutes.PadLeft(1, '0');
        string seconds = (data.duration % 60).ToString();
        seconds = seconds.PadLeft(2, '0');
        durationText.SetText(minutes + ":" + seconds);
    }
}
