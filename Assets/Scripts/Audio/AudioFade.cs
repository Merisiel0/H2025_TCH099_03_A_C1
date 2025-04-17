using UnityEngine;
using System.Collections;

public static class AudioFade
{
    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime, float maxVolume)
    {
        audioSource.Play();

        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += maxVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }
}