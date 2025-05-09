using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerInteract : MonoBehaviour
{
    private static PlayerInteract instance;

    private AudioSource source;
    [SerializeField] private AudioClip interactSound;

    private PlayerController m_controller;
    private Module m_currentlyOpenedModule = null;

    [Header("Interactions")]
    [SerializeField] private float m_interactableDetectionRange = 0.5f;

    public static void StopInteractions()
    {
        instance.CloseCurrentModule();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        m_controller = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactableObject = collision.GetComponent<Interactable>();
        if (interactableObject != null)
        {
            interactableObject.OnPlayerTriggerStart();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactableObject = collision.GetComponent<Interactable>();
        if (interactableObject != null && interactableObject.isTrigger)
        {
            interactableObject.OnPlayerTriggerEnd();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_interactableDetectionRange);
            if (m_currentlyOpenedModule == null)
            {
                foreach (Collider2D hit in hits)
                {
                    Interactable interactableObject = hit.GetComponent<Interactable>();
                    if (interactableObject != null)
                    {
                        if(interactableObject.Interact())
                        {
                            if (interactableObject is Module module)
                            {
                                m_currentlyOpenedModule = module;
                                m_controller.StopMoving();
                                m_controller.enabled = false;
                            } 
                            else
                            {
                                source.Stop();
                                source.PlayOneShot(interactSound);
                            }
                        }
                    }
                }
            }
            else
            {
                CloseCurrentModule();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCurrentModule();
        }
    }

    private void CloseCurrentModule()
    {
        if (m_currentlyOpenedModule != null)
        {
            m_currentlyOpenedModule.Interact();
            m_currentlyOpenedModule = null;
            m_controller.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_interactableDetectionRange);
    }
}
