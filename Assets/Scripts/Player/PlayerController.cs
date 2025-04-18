using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rb;

    [Header("Movements")]
    [SerializeField] private float m_walkingSpeed = 2;
    [SerializeField] private float m_runningSpeed = 3.5f;
    private float m_currentSpeed;
    private Vector2 m_dir;

    [Header("Sounds")]
    [SerializeField] private FootstepCollection m_footsteps;
    [SerializeField] private float m_regularFootstepDelay = 0.4f;
    [SerializeField] private float m_runningFootstepDelay = 0.23f;
    private float m_volume;
    private float m_footstepDelay = 0.4f;
    private float m_footstepTimer = 0f;
    private AudioSource m_audioSource;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();

        m_audioSource = GetComponent<AudioSource>();
        m_volume = m_audioSource.volume;
        m_audioSource.loop = false;
    }

    private void Update()
    {
        HandleRotation();

        if (m_dir.magnitude > 0.1f)
        {
            m_footstepTimer += Time.deltaTime;

            if (m_footstepTimer >= m_footstepDelay)
            {
                PlayFootstep();
                m_footstepTimer = 0f;
            }
        }
        else
        {
            m_footstepTimer = m_footstepDelay;
        }
    }

    private void FixedUpdate()
    {
        HandlePosition();
        m_rb.velocity = m_dir * m_currentSpeed;
    }

    private void PlayFootstep()
    {
        StartCoroutine(AudioFade.FadeOut(m_audioSource, 0.04f));
        StartCoroutine(AudioFade.FadeIn(m_audioSource, 0.04f, m_volume));
        m_audioSource.PlayOneShot(m_footsteps.GetRandom());
    }

    public void StopMoving()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = 0.0f;
    }

    private void HandlePosition()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        m_footstepDelay = isRunning ? m_runningFootstepDelay : m_regularFootstepDelay; // Change footstep duration
        m_currentSpeed = isRunning ? m_runningSpeed : m_walkingSpeed;

        m_dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        m_dir.Normalize();
    }

    private void HandleRotation()
    {
        float camDis = Camera.main.transform.position.y - m_rb.position.y;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));

        float AngleRad = Mathf.Atan2(mouse.y - m_rb.position.y, mouse.x - m_rb.position.x);
        float angle = 180 / Mathf.PI * AngleRad;

        m_rb.rotation = angle - 90;
    }
}
