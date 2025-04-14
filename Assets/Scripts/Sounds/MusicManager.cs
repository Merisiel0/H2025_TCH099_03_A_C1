using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
  [Range(0, 1)] public float musicVolume = 0.5f;
  [SerializeField] private AudioSource _intro;
  [SerializeField] private AudioSource _loop;

  private MusicManager instance;
  public MusicManager GetInstance()
  {
    return instance;
  }

  void OnValidate()
  {
    ApplyVolume();   
  }

  void Awake()
  {
    instance = this;
    DontDestroyOnLoad(this);

    ApplyVolume();

    _intro.Play();
    StartCoroutine(StartLoop());
  }

  private void ApplyVolume()
  {
    _intro.volume = musicVolume;
    _loop.volume = musicVolume;
  }

  private IEnumerator StartLoop()
  {
    yield return new WaitWhile(() => _intro.isPlaying);
    _loop.Play();
  }
}
