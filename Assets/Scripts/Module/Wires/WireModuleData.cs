using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireModuleData
{
    public int id_;
    public int nbFils;

    public string[] couleurs;
    public string couleurFil1;
    public string couleurFil2;
    public string couleurFil3;
    public string couleurFil4;
    public string couleurFil5;
    public string couleurFil6;

    public int solution;

    public void Init()
    {
        string[] wireColors =
        {
            couleurFil1,
            couleurFil2,
            couleurFil3,
            couleurFil4,
            couleurFil5,
            couleurFil6
        };
        couleurs = wireColors;
    }
}
