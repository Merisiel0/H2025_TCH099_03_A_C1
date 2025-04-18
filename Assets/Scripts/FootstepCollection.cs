using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Footsteps", menuName = "Audio/Footstep Collection")]
[System.Serializable]
public class FootstepCollection : ScriptableObject
{
    [SerializeField] private AudioClip[] collection;

    public AudioClip GetRandom()
    {
        int randIndex = Random.Range(0, collection.Length);
        return collection[randIndex];
    }
}
