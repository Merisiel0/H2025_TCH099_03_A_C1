using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Vent : Interactable
{
    private AudioSource source;
    [SerializeField] private SoundCollection sounds;

    protected override void Start()
    {
        base.Start();
        source = GetComponent<AudioSource>();
    }

    public override bool Interact()
    {
        source.Stop();
        source.pitch = Random.Range(0.7f, 1f);
        source.PlayOneShot(sounds.GetRandom());
        return true;
    }
}