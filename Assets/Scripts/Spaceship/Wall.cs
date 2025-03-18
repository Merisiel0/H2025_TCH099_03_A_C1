using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Wall : MonoBehaviour
{
    private BoxCollider2D _collider;
    [HideInInspector] public Vector2 center;
    [HideInInspector] public float left, top, right, bottom;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();

        center.x = transform.position.x;
        center.y = transform.position.y;

        left = transform.position.x - _collider.bounds.extents.x;
        top = transform.position.y + _collider.bounds.extents.y;
        right = transform.position.x + _collider.bounds.extents.x;
        bottom = transform.position.y - _collider.bounds.extents.y;
    }
}
