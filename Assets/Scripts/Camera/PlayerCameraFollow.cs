using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui permet à la caméra de suivre le joueur en fonction de sa position et d'une vitesse donné.
/// </summary>
public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public float followSpeed;
    public Vector3 cameraOffset = new Vector3(0, 0, -1);

    private void Update()
    {
        // On fait que la caméra suit doucement le joueur en fonction de sa position, on applique la nouvelle posiiton (lissé) à la caméera en y ajoutant le décallage.
        Vector3 smoothed = Vector3.Lerp(transform.position, player.transform.position, followSpeed);
        transform.position = smoothed + cameraOffset;
    }
}
