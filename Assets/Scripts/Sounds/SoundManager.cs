using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public int maxSourcesAmount = 10;
    public int startSourcesAmount = 4;

    [Range(.2f, 2)] public float fadeDuration = 1.0f;

    /// <summary>
    /// Range of beeps playing randomly, can play anywhere from x seconds to y seconds.
    /// </summary>
    public Vector2 beepsRange = new Vector2(10, 45);
    [Range(0, 1)] public float beepsVolume = 0.5f;
    public BoolRef playRandomBeeps = new BoolRef(true);

    public Vector2 carcassRange = new Vector2(10, 45);
    [Range(0, 1)] public float carcassVolume = 0.5f;
    public BoolRef playRandomCarcass = new BoolRef(true);

    public Vector2 humRange = new Vector2(10, 45);
    [Range(0, 1)] public float humVolume = 0.5f;
    public BoolRef playRandomHum = new BoolRef(true);

    public Vector2 spaceRange = new Vector2(10, 45);
    [Range(0, 1)] public float spaceVolume = 0.5f;
    public BoolRef playRandomSpace = new BoolRef(true);

    [SerializeField] private AudioClip[] _beeps;
    [SerializeField] private AudioClip[] _carcass;
    [SerializeField] private AudioClip[] _hums;
    [SerializeField] private AudioClip[] _space;
    private List<AudioSource> _sources = new List<AudioSource>();

    /// <summary>
    /// source index is true if it's waiting to play.
    /// </summary>
    private List<bool> _waitingSources = new List<bool>();

    public void AddSource()
    {
        _sources.Add(gameObject.AddComponent<AudioSource>());
        _sources.Last().playOnAwake = false;
        _sources.Last().loop = false;

        _waitingSources.Add(false);
    }

    void Start()
    {
        for (int i = 0; i < startSourcesAmount; i++)
        {
            AddSource();
        }

        if (playRandomBeeps.value)
        {
            PlaySound(_beeps, beepsVolume, beepsRange, true, playRandomBeeps);
        }
        if (playRandomCarcass.value)
        {
            PlaySound(_carcass, carcassVolume, carcassRange, true, playRandomCarcass);
        }
        if (playRandomHum.value)
        {
            PlaySound(_hums, humVolume, humRange, true, playRandomHum);
        }
        if (playRandomSpace.value)
        {
            PlaySound(_space, spaceVolume, spaceRange, true, playRandomSpace);
        }
    }

    private AudioSource GetFreeSource()
    {
        for (int i = 0; i < _sources.Count; i++)
        {
            if (!_sources[i].enabled && _waitingSources[i] == false)
            {
                _sources[i].enabled = true;
                return _sources[i];
            }
        }

        if (_sources.Count() < maxSourcesAmount)
        {
            AddSource();
            return _sources.Last();
        }

        Debug.LogError("Too many sounds playing at the same time, ran out of audio sources.");
        return null;
    }

    /// <summary>
    /// Plays a random sound from clips. <br/>
    /// If the range is Vector2.zero, the sound will play immediately. <br/>
    /// Otherwise, sound will play anywhere from range.x seconds to range.y seconds. <br/>
    /// If loop is true, the sound will loop.
    /// </summary>
    /// <param name="clips">set from which a random sound will be picked</param>
    /// <param name="range">range in which to play the sound</param>
    /// <param name="loop">loops if set to true</param>
    /// <param name="keepPlaying">stops looping when set to false</param>
    private void PlaySound(AudioClip[] clips, float volume, Vector2 range, bool loop = false, BoolRef keepPlaying = null)
    {
        if (clips == null || clips.Length == 0) return;

        AudioSource source = GetFreeSource();
        if (source == null) return;

        if (range == Vector2.zero)
        {
            int randomIndex = Random.Range(0, clips.Length);

            source.clip = clips[randomIndex];
            source.loop = loop;
            source.Play();
        }
        else
        {
            StartCoroutine(DelayedPlay(source, clips, volume, range, keepPlaying));
        }
    }

    private IEnumerator DelayedPlay(AudioSource source, AudioClip[] clips, float volume, Vector2 delayRange, BoolRef keepPlaying)
    {
    Repeat:
        // init clip
        int randomIndex = Random.Range(0, clips.Length);
        source.clip = clips[randomIndex];

        int sourceIndex = _sources.IndexOf(source);
        if (sourceIndex != -1)
            _waitingSources[sourceIndex] = true;

        // delay
        float delay = Random.Range(delayRange.x, delayRange.y);
        yield return new WaitForSeconds(delay);

        // fade in
        source.volume = 0;
        source.Play();
        yield return StartCoroutine(FadeVolume(source, 0f, volume));

        // wait clip duration - fadeDuration * 2
        yield return new WaitWhile(() => source.isPlaying && source.time < source.clip.length - fadeDuration);

        // fade out
        yield return StartCoroutine(FadeVolume(source, volume, 0f));

        // loop
        if (keepPlaying.value)
        {
            goto Repeat;
        }

        // de-init clip
        if (sourceIndex != -1)
            _waitingSources[sourceIndex] = false;

        source.enabled = false;
    }

    private IEnumerator FadeVolume(AudioSource source, float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        source.volume = to;
    }

    public void PlayBeeps()
    {
        PlaySound(_beeps, beepsVolume, Vector2.zero);
    }

    public void PlayCarcass()
    {
        PlaySound(_carcass, carcassVolume, Vector2.zero);
    }

    public void PlayHum()
    {
        PlaySound(_hums, humVolume, Vector2.zero);
    }

    public void PlaySpace()
    {
        PlaySound(_space, spaceVolume, Vector2.zero);
    }

    void OnValidate()
    {
        foreach (AudioSource source in _sources)
        {
            if (source.enabled)
            {
                if (_beeps.Contains(source.clip))
                {
                    source.volume = beepsVolume;
                }
                else if (_carcass.Contains(source.clip))
                {
                    source.volume = carcassVolume;
                }
                else if (_hums.Contains(source.clip))
                {
                    source.volume = humVolume;
                }
                else if (_space.Contains(source.clip))
                {
                    source.volume = spaceVolume;
                }
            }
        }
    }
}
