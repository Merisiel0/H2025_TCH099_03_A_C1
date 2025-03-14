using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Chair : Interactable
{
    private Rigidbody2D _rb;

    public float turnVelocity = 1000f;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void Interact()
    {
        _rb.angularVelocity = turnVelocity;
    }
}
