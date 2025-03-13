using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Module : MonoBehaviour
{
    public static List<Module> allModules = new List<Module>();

    private SpriteRenderer _sr;

    [SerializeField] private GameObject moduleUI;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        allModules.Add(this);
    }

    public void EnableUI()
    {
        moduleUI.SetActive(true);
    }

    public void DisableUI()
    {
        moduleUI.SetActive(false);
    }
}
