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
        if (instance != null) Debug.LogError("Il y a deux ButtonSoundPlayer dans la sc�ne!");
        instance = this; // On s'assure d'avoir uniquement une seule instance mais pas probl�matique si il y a un conflit
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
