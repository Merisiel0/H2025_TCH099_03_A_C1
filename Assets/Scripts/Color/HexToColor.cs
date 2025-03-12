using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui permet de convertir un code hexadecimal en objet Color de unity
/// </summary>
public class HexToColor : MonoBehaviour
{
    /// <summary>
    /// Fonction qui permet de convertir un code hexadecimal en objet Color de unity
    /// </summary>
    /// <param name="hex">Le code couleur hexadécimal sous forme (#ffffff ou ffffff) à convertir</param>
    /// <returns>Retourne un objet Color qui correspond au bon code couleur.</returns>
    public static Color FromHex(string hex)
    {
        hex = hex.Replace("#", "");

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
