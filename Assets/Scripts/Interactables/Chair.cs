using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Chair : Interactable
{
    private Rigidbody2D _rb;

    public float maxTurnVelocity = 1100f;
    public float minTurnVelocity = 900f;
    private float turnVelocity;

    protected override void Start()
    {   
        base.Start();
        _rb = GetComponent<Rigidbody2D>();

        turnVelocity = Random.Range(minTurnVelocity, maxTurnVelocity);
    }

    public override void Interact()
    {
        _rb.angularVelocity = turnVelocity;
    }
}
