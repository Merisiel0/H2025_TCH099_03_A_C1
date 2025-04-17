using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Chair : Interactable
{
    private Rigidbody2D _rb;
    private AudioSource source;
    [SerializeField] private AudioClip squeakSound;

    public float maxTurnVelocity = 1100f;
    public float minTurnVelocity = 900f;
    private float turnVelocity;

    protected override void Start()
    {   
        base.Start();
        source = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();

        turnVelocity = Random.Range(minTurnVelocity, maxTurnVelocity);
    }

    public override bool Interact()
    {
        source.Stop();
        source.pitch = Random.Range(0.7f, 1f);
        source.PlayOneShot(squeakSound);
        _rb.angularVelocity = turnVelocity;
        return true;
    }
}
