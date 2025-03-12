using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui permet � la cam�ra de suivre le joueur en fonction de sa position et d'une vitesse donn�.
/// </summary>
public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public float followSpeed;
    public Vector3 cameraOffset = new Vector3(0, 0, -1);

    private void Update()
    {
        // On fait que la cam�ra suit doucement le joueur en fonction de sa position, on applique la nouvelle posiiton (liss�) � la cam�era en y ajoutant le d�callage.
        Vector3 smoothed = Vector3.Lerp(transform.position, player.transform.position, followSpeed);
        transform.position = smoothed + cameraOffset;
    }
}
