using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  public int _maxSourcesAmount = 10;
  public int _startSourcesAmount = 4;
  [SerializeField] private AudioClip[] _beeps;
  [SerializeField] private AudioClip[] _carcass;
  [SerializeField] private AudioClip[] _hums;
  [SerializeField] private AudioClip[] _space;
  private List<AudioSource> _sources = new List<AudioSource>();

  public void AddSource()
  {
    _sources.Add(gameObject.AddComponent<AudioSource>());
    _sources.Last().playOnAwake = false;
    _sources.Last().loop = false;
  }

  void Start()
  {
    for (int i = 0; i < _startSourcesAmount; i++)
    {
      AddSource();
    }

    StartAmbientMenuSounds();
  }

  private void StartAmbientMenuSounds()
  {
    PlaySpace(true);
    PlayHum(true);
  }

  private AudioSource GetFreeSource()
  {
    foreach (AudioSource source in _sources)
    {
      if (!source.isPlaying)
      {
        return source;
      }
    }

    if (_sources.Count() < _maxSourcesAmount)
    {
      AddSource();
      return _sources.Last();
    }

    Debug.LogError("Too many sounds playing at the same time, ran out of audio sources.");
    return null;
  }

  private void PlaySound(AudioClip[] clips, bool loop)
  {
    AudioSource source = GetFreeSource();

    if (source == null) return;

    int randomIndex = Random.Range(0, clips.Length);
    source.clip = clips[randomIndex];
    source.loop = loop;
    source.Play();
  }

  public void PlayBeeps(bool loop)
  {
    PlaySound(_beeps, loop);
  }

  public void PlayCarcass(bool loop)
  {
    PlaySound(_carcass, loop);
  }

  public void PlayHum(bool loop)
  {
    PlaySound(_hums, loop);
  }

  public void PlaySpace(bool loop)
  {
    PlaySound(_space, loop);
  }
}
