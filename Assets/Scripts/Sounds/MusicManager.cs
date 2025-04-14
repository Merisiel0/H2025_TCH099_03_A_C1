using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Range(0, 1)] public float musicVolume = 0.5f;
    [SerializeField] private AudioSource _intro;
    [SerializeField] private AudioSource _loop;

    private static MusicManager instance;

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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        ApplyVolume();

        _intro.Play();
        _loop.Stop();
        StartCoroutine(StartLoop());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            RestartIntro();
        }
    }

    public void RestartIntro()
    {
        _loop.Stop();
        _intro.Stop();
        _intro.time = 0;
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
