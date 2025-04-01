using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BipolarityModuleData
{
    public int id_;
    public string lettre1;
    public string lettre2;
    public string lettre3;
    public string lettre4;
    public int caseChoisie;

    public string couleur;
    public Color objCouleur;

    public string solution;

    public BipolarityModuleData Init()
    {
        objCouleur = HexToColor.FromHex(couleur);
        return this;
    }
}
