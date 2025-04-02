using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WireModuleData
{
    public int nbFils;
    [System.NonSerialized] public string[] couleurs;
    public string filsCouleur1;
    public string filsCouleur2;
    public string filsCouleur3;
    public string filsCouleur4;
    public string filsCouleur5;
    public string filsCouleur6;

    public int solution;

    public void Init()
    {
        string[] wireColors = { filsCouleur1, filsCouleur2, filsCouleur3, filsCouleur4, filsCouleur5, filsCouleur6 };
        couleurs = wireColors;
    }
}
