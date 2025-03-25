using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    private bool opended = false;
    private KeyCode key = KeyCode.Escape;
    private FadeAnimation fade;

    [SerializeField] private UnityEvent backToMenuEvent;
    [SerializeField] private CanvasGroup button;

    private void Start()
    {
        fade = GetComponent<FadeAnimation>();

        button.interactable = false;
        button.blocksRaycasts = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(key))
        {
            opended = !opended;
            fade.fadeIn = opended;

            button.interactable = opended;
            button.blocksRaycasts = opended;

            fade.Fade();
        }
    }
}
