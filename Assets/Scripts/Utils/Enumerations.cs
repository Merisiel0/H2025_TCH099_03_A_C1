using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Enumerations : MonoBehaviour
{
    public static string GetDescription(Enum val)
    {
        FieldInfo info = val.GetType().GetField(val.ToString());
        DescriptionAttribute[] descriptions = info.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if(descriptions != null && descriptions.Length > 0)
        {
            return descriptions.First().Description;
        }
        return val.ToString();
    }
}
