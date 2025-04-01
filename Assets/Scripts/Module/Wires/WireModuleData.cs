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
        ValidateData();

        string[] wireColors = { filsCouleur1, filsCouleur2, filsCouleur3, filsCouleur4, filsCouleur5, filsCouleur6 };
        couleurs = wireColors;
    }

    private void ValidateData()
    {
        // Remplace les valeurs nulles par une couleur par défaut
        filsCouleur1 = string.IsNullOrEmpty(filsCouleur1) ? "000000" : filsCouleur1;
        filsCouleur2 = string.IsNullOrEmpty(filsCouleur2) ? "000000" : filsCouleur2;
        filsCouleur3 = string.IsNullOrEmpty(filsCouleur3) ? "000000" : filsCouleur3;
        filsCouleur4 = string.IsNullOrEmpty(filsCouleur4) ? "000000" : filsCouleur4;
        filsCouleur5 = string.IsNullOrEmpty(filsCouleur5) ? "000000" : filsCouleur5;
        filsCouleur6 = string.IsNullOrEmpty(filsCouleur6) ? "000000" : filsCouleur6;
    }
}
