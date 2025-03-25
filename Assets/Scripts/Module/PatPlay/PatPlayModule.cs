using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;

public class PatPlayModule : ModuleUI, MissionEventListener
{
    public float startTimout = 1;
    public float roundTimeout = 0.5f;
    public float flashDuration = 1;
    public Color flashColor = Color.black;
    private int[] solution;
    private bool isPlaying = false;
    private bool isInit = false;

    private PatPlayModuleData data;

    enum Symbols
    {
        Triangle, Circle, Square, X, Count
    }

    enum Positions
    {
        UpLeft, UpRight, DownLeft, DownRight, Count
    }

    private Dictionary<string, int> shapeMap = new Dictionary<string, int>
    {
        { "Triangle", (int)Positions.UpLeft },
        { "Cercle", (int)Positions.UpRight },
        { "Carre", (int)Positions.DownLeft },
        { "X", (int)Positions.DownRight }
    };

    [SerializeField] private Button[] _buttons;
    private Image[] _sprites = new Image[(int)Symbols.Count];
    private Image[] _spriteBackgrounds = new Image[(int)Symbols.Count];
    private Vector3[] _positions = new Vector3[(int)Positions.Count];

    public void OnNotify(MissionEvent e)
    {
        if (e == MissionEvent.LightsOut)
        {
            PatPlayModuleData data = new PatPlayModuleData();
            data.couleurTriangle = "0000ff";
            data.couleurCercle = "ffff00";
            data.couleurCarre = "00ff00";
            data.couleurX = "ff0000";
            data.formeHG = "Carre";
            data.formeHD = "X";
            data.formeBG = "Cercle";
            data.formeBD = "Triangle";
            data.solution = "32201";

            InitModule(data);
        }
    }

    private void Start()
    {
        base.Start();

        data = new PatPlayModuleData();

        for (int i = 0; i < (int)Symbols.Count; i++)
        {
            _sprites[i] = _buttons[i].GetComponent<Image>();
            _positions[i] = _buttons[i].transform.position;

            _spriteBackgrounds[i] = _buttons[i].transform.GetChild(0).GetComponent<Image>();
            _spriteBackgrounds[i].color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        }

        MissionEventManager.AddEventListener(this);
    }

    void Update()
    {
        if (isInit)
        {
            if (UIEnabled && !isPlaying)
            {
                StartCoroutine(PatPlay());
                isPlaying = true;
            }
            else if (!UIEnabled && isPlaying)
            {
                StopCoroutine(PatPlay());
                isPlaying = false;
            }
        }
    }

    public void InitModule(PatPlayModuleData newData)
    {
        data = newData.Init();

        _sprites[(int)Symbols.Triangle].color = data.objCouleurTriangle;
        _sprites[(int)Symbols.Circle].color = data.objCouleurCercle;
        _sprites[(int)Symbols.Square].color = data.objCouleurCarre;
        _sprites[(int)Symbols.X].color = data.objCouleurX;

        string[] positions = new[] { data.formeHG, data.formeHD, data.formeBG, data.formeBD };
        for (int i = 0; i < positions.Length; i++)
        {
            switch (positions[i])
            {
                case "Triangle":
                    _buttons[(int)Symbols.Triangle].transform.position = _positions[i];
                    break;
                case "Cercle":
                    _buttons[(int)Symbols.Circle].transform.position = _positions[i];
                    break;
                case "Carre":
                    _buttons[(int)Symbols.Square].transform.position = _positions[i];
                    break;
                case "X":
                    _buttons[(int)Symbols.X].transform.position = _positions[i];
                    break;
            }
        }

        solution = data.solution.Select(c => c - '0').ToArray();

        isInit = true;
    }

    private IEnumerator PatPlay()
    {
        // wait for timeout
        yield return new WaitForSeconds(startTimout);

        // setup button callbacks
        BoolRef trianglePressed = new BoolRef(false);
        _buttons[(int)Symbols.Triangle].onClick.AddListener(() =>
        {
            trianglePressed.value = true;
        });

        BoolRef circlePressed = new BoolRef(false); ;
        _buttons[(int)Symbols.Circle].onClick.AddListener(() =>
        {
            circlePressed.value = true;
        });

        BoolRef squarePressed = new BoolRef(false); ;
        _buttons[(int)Symbols.Square].onClick.AddListener(() =>
        {
            squarePressed.value = true;
        });

        BoolRef xPressed = new BoolRef(false); ;
        _buttons[(int)Symbols.X].onClick.AddListener(() =>
        {
            xPressed.value = true;
        });

        BoolRef[] buttons = new BoolRef[] { trianglePressed, circlePressed, squarePressed, xPressed };

        // play the game
        for (int solutionProgress = 0; solutionProgress < solution.Length; solutionProgress++)
        {
            // wait a bit
            yield return new WaitForSeconds(startTimout);

            // flash the solution
            for (int i = 0; i <= solutionProgress; i++)
            {
                yield return Flash(_spriteBackgrounds[solution[i]], flashColor, flashDuration);
            }

            // one round = pressing all the right buttons
            for (int roundProgress = 0; roundProgress <= solutionProgress; roundProgress++)
            {
                // wait for all buttons to be pressed in right order
                while (!buttons[solution[roundProgress]].value)
                {
                    yield return null;
                }
                buttons[solution[roundProgress]].value = false;
            }
        }

        // cleanup listeners
        foreach (Button btn in _buttons)
        {
            btn.onClick.RemoveAllListeners();
        }
        PlayerInteract.StopInteractions();
        Debug.Log("YOU WIN");
    }

    private IEnumerator Flash(Image image, Color flashColor, float duration)
    {
        Color originalColor = image.color;

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // flash in
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            image.color = Color.Lerp(originalColor, flashColor, elapsed / halfDuration);
            yield return null;
        }

        elapsed = 0f;

        // flash out
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            image.color = Color.Lerp(flashColor, originalColor, elapsed / halfDuration);
            yield return null;
        }
        image.color = originalColor;
    }
}