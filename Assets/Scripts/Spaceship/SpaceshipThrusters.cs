using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui g�re l'activation et la fermeture des propulseurs du vaisseau. G�re les particules et de l'animation
/// des propulseurs en fonction de leur �tat.
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

        // �couter les �v�nements de la mission
        MissionEventManager.AddEventListener(this);
    }

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// On �coute l'allumage et la fermeture des propusleurs et on les ferme ou les ouvre en cons�quence
    /// </summary>
    /// <param name="e">L'�v�nement de mission</param>
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
    /// Fonction qui g�re l'animation pour chacun des propulseur lors de l'allumage et de la fermeture
    /// </summary>
    /// <param name="newXPosition">La position o� on doit amener le propusleur</param>
    /// <param name="desiredOpacity">La nouvelle opacit� du propusleur</param>
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
    /// Fonction qui permet d'allumer ou �teindre tous les syst�mes de particules que contient un conteneur
    /// </summary>
    /// <param name="container">Le parent des syt�mes de particules</param>
    /// <param name="status">Vrai si on veut commencer l'�mission, faux pour l'�teindre.</param>
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
    /// Fonciton pour �teindre les propulseurs, mettre le minuteur sur pause, �teindre les particules.
    /// </summary>
    public void Disable()
    {
        GameTimer.Pause(true);
        MoveThrusters(endPos, 0.0f);
        SetParticuleEmitterStatus(speedParticlesContainer, false);
    }
}
