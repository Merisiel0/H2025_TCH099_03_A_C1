using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSoundPlayer : MonoBehaviour
{
    private AudioSource source;
    public static ButtonSoundPlayer instance { get; private set; }
    [SerializeField] private AudioClip uiButtonSound;

    private void Awake()
    {
        instance = this; // On s'assure d'avoir uniquement une seule instance mais pas problématique si il y a un conflit
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public static void PlayUIButtonSound()
    {
        instance.source.Stop();
        instance.source.PlayOneShot(instance.uiButtonSound);
    }
}
