using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatPlayModuleData
{
    public int id_;

    public string couleurTriangle;
    public Color objCouleurTriangle;

    public string couleurCercle;
    public Color objCouleurCercle;

    public string couleurCarre;
    public Color objCouleurCarre;

    public string couleurX;
    public Color objCouleurX;

    public string formeHG;
    public string formeHD;
    public string formeBG;
    public string formeBD;

    public string solution;

    public PatPlayModuleData Init()
    {
        objCouleurTriangle = HexToColor.FromHex(couleurTriangle);
        objCouleurCercle = HexToColor.FromHex(couleurCercle);
        objCouleurCarre = HexToColor.FromHex(couleurCarre);
        objCouleurX = HexToColor.FromHex(couleurX);

        return this;
    }
}
