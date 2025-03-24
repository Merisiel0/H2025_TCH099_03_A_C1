using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeAnimation : MonoBehaviour
{
    public bool fadeOnStart = false;
    public bool fadeIn = true;
    public float fadeDuration = 0.1f;
    public float maxAlpha = 1.0f;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (fadeOnStart) Fade();
    }

    public void Fade()
    {
        Fade(null);
    }

    public void Fade(System.Action callback)
    {
        canvasGroup.alpha = fadeIn ? 0.0f : maxAlpha;

        LTDescr lt = LeanTween.alphaCanvas(canvasGroup, fadeIn ? maxAlpha : 0.0f, fadeDuration);
        if(callback != null) lt.setOnComplete(callback);
    }
}
