using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
  [SerializeField] private AudioSource _intro;
  [SerializeField] private AudioSource _loop;

  private MusicManager instance;
  public MusicManager GetInstance(){
    return instance;
  }

  void Awake()
  {
    instance = this;
    Invoke("StartLoop", _intro.clip.length);
  }

  private void StartLoop()
  {
    _loop.Play();
  }
}
