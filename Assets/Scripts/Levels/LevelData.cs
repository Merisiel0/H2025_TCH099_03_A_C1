using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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
        this.data = data;
        return this;
    }

    public static LevelData Get() { 
        if(Exists())
        {
            return instance.data;
        } 
        else return new LevelData();
    }

    public static bool Exists() { return instance != null; }
}

/// <summary>
/// Classe qui contient toutes les information pour une option de niveau
/// </summary>
[System.Serializable]
public class LevelData
{
    public int id_ = 0;
    public string nom = "Nom";
    public string description = "Description";
    public string difficulte = "Difficulty";
    public int duree = 9999; // temps en seconde (durée du niveau aprox)
    public string couleur = "FF00FF";
    [JsonProperty("minTemps")]
    public int minTemps;
    [JsonProperty("maxTemps")]
    public int maxTemps;
}

/// <summary>
/// Classe qui contient toutes les informations pour une liste de niveau
/// </summary>
[System.Serializable]
public class LevelDataWrapper
{
    public LevelData[] levels;
}