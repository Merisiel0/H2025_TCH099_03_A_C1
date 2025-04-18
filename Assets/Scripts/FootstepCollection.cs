using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Audio/Sound Collection")]
[System.Serializable]
public class SoundCollection : ScriptableObject
{
    [SerializeField] private AudioClip[] collection;

    public AudioClip GetRandom()
    {
        int randIndex = Random.Range(0, collection.Length);
        return collection[randIndex];
    }
}
