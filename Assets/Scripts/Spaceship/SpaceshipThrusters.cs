using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui gère l'activation et la fermeture des propulseurs du vaisseau. Gère les particules et de l'animation
/// des propulseurs en fonction de leur état.
/// </summary>
public class SpaceshipThrusters : MonoBehaviour, MissionEventListener
{
    private float startPos;
    public float endPos;
    public float transitionSpeed;
    SpriteRenderer[] sprites;

    public GameObject speedParticlesContainer;

    private void Awake()
    {
        startPos = transform.position.x;

        // Écouter les événements de la mission
        MissionEventManager.AddEventListener(this);
    }

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// On écoute l'allumage et la fermeture des propusleurs et on les ferme ou les ouvre en conséquence
    /// </summary>
    /// <param name="e">L'événement de mission</param>
    public void OnNotify(MissionEvent e)
    {
        if(e == MissionEvent.ThrustersShutdown)
        {
            Disable();
        } 
        else if(e == MissionEvent.ThrustersStart)
        {
            Enable();
        }
    }

    /// <summary>
    /// Fonction qui gère l'animation pour chacun des propulseur lors de l'allumage et de la fermeture
    /// </summary>
    /// <param name="newXPosition">La position où on doit amener le propusleur</param>
    /// <param name="desiredOpacity">La nouvelle opacité du propusleur</param>
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
                    Color color = sprite.color;
                    color.a = val;
                    sprite.color = color;
                });
        }
    }

    /// <summary>
    /// Fonction qui permet d'allumer ou éteindre tous les systèmes de particules que contient un conteneur
    /// </summary>
    /// <param name="container">Le parent des sytèmes de particules</param>
    /// <param name="status">Vrai si on veut commencer l'émission, faux pour l'éteindre.</param>
    private void SetParticuleEmitterStatus(GameObject container, bool status)
    {
        ParticleSystem[] ps = container.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem system in ps)
        {
            if (!status) system.Stop();
            else system.Play();
        }
    }

    /// <summary>
    /// Fonction pour allumer les propulseur, relancer le minuteur et commencer les particules.
    /// </summary>
    public void Enable()
    {
        GameTimer.Pause(false);
        MoveThrusters(startPos, 1.0f);
        SetParticuleEmitterStatus(speedParticlesContainer, true);
    }

    /// <summary>
    /// Fonciton pour éteindre les propulseurs, mettre le minuteur sur pause, éteindre les particules.
    /// </summary>
    public void Disable()
    {
        GameTimer.Pause(true);
        MoveThrusters(endPos, 0.0f);
        SetParticuleEmitterStatus(speedParticlesContainer, false);
    }
}
