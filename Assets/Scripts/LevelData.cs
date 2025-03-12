using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int id = 0;
    public string nom = "Nom";
    public string description = "Description";
    public string difficulty = "Difficulty";
    public int duration = 9999; // Duration in seconds
    public string color = "FF00FF";
}

[System.Serializable]
public class LevelDataWrapper
{
    public LevelData[] levels;
}