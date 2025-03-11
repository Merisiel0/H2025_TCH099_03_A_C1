using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public float followSpeed;
    public Vector3 cameraOffset = new Vector3(0, 0, -1);

    private void Update()
    {
        Vector3 smoothed = Vector3.Lerp(transform.position, player.transform.position, followSpeed);
        transform.position = smoothed + cameraOffset;
    }
}
