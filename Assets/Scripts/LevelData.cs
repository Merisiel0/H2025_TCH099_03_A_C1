using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataObject : MonoBehaviour
{
    private static LevelDataObject instance = null;

    [SerializeField] private LevelData data;

    private void Awake()
    {
        // On remplace l'ancienne instance si elle existe encore. Permet une gestion simplifié du singleton.
        // => Dernier arrivé = le seul qui existe.
        if (instance != null) Destroy(instance.gameObject);
        instance = this;

        data = new LevelData();

        DontDestroyOnLoad(gameObject);
    }

    public LevelDataObject Init(LevelData data)
    {
        this.data.id = data.id;
        this.data.nom = data.nom;
        this.data.description = data.description;
        this.data.difficulty = data.difficulty;
        this.data.duration = data.duration;
        this.data.color = data.color;

        return this;
    }

    public static LevelData Get() { return instance.data; }
    public static bool Exists() { return instance != null; }
}

/// <summary>
/// Classe qui contient toutes les information pour une option de niveau
/// </summary>
[System.Serializable]
public class LevelData
{
    public int id = 0;
    public string nom = "Nom";
    public string description = "Description";
    public string difficulty = "Difficulty";
    public int duration = 9999; // temps en seconde (durée du niveau aprox)
    public string color = "FF00FF";
}

/// <summary>
/// Classe qui contient toutes les informations pour une liste de niveau
/// </summary>
[System.Serializable]
public class LevelDataWrapper
{
    public LevelData[] levels;
}