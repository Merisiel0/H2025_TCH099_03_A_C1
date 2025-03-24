using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool opended = false;
    private KeyCode key = KeyCode.Escape;
    private FadeAnimation fade;

    private void Start()
    {
        fade = GetComponent<FadeAnimation>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(key))
        {
            opended = !opended;
            fade.fadeIn = opended;
            fade.Fade();
        }
    }
}
