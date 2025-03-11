using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipThrusters : MonoBehaviour
{
    private float startPos;
    public float endPos;
    public float transitionSpeed;
    SpriteRenderer[] sprites;

    private void Awake()
    {
        startPos = transform.position.x;
    }

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            Enable();
        }
        else if (Input.GetKeyDown("2"))
        {
            Disable();
        }
    }

    private void MoveThrusters(float newXPosition, float desiredOpacity)
    {
        float distance = Mathf.Abs(transform.position.x - newXPosition);
        float duration = distance / transitionSpeed;
        LeanTween.cancel(gameObject);
        LeanTween.moveX(gameObject, newXPosition, duration / 2).setEase(LeanTweenType.easeInOutQuart);

        foreach (SpriteRenderer sprite in sprites)
        {
            float startOpacity = sprite.color.a;
            LeanTween.cancel(sprite.gameObject);
            LeanTween.value(sprite.gameObject, startOpacity, desiredOpacity, duration * 2)
                .setEase(LeanTweenType.easeOutBounce)
                .setOnUpdate((float val) =>
                {
                    Debug.Log(val);
                    Color color = sprite.color;
                    color.a = val;
                    sprite.color = color;
                });
        }
    }

    public void Enable()
    {
        GameTimer.Pause(false);
        MoveThrusters(startPos, 1.0f);
    }

    public void Disable()
    {
        GameTimer.Pause(true);
        MoveThrusters(endPos, 0.0f);
    }
}
