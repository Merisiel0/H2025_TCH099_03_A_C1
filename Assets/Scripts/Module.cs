using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Module : MonoBehaviour
{
    public static List<Module> allModules = new List<Module>();

    private SpriteRenderer _sr;

    private Color _okColor = Color.green;
    private Color _errorColor = Color.red;
    private Color _interactColor = Color.yellow;

    public enum ModuleState
    {
        OK,
        ERROR,
        INTERACT
    }

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        allModules.Add(this);
    }

    public void SetState(ModuleState state)
    {
        switch (state)
        {
            case ModuleState.OK:
                _sr.color = _okColor;
                break;
            case ModuleState.ERROR:
                _sr.color = _errorColor;
                break;
            case ModuleState.INTERACT:
                _sr.color = _interactColor;
                break;
        }
    }
}
