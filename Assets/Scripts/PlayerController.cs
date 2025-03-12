using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rb;

    [Header("Movements")]
    [SerializeField] private float m_walkingSpeed = 2;
    [SerializeField] private float m_runningSpeed = 3.5f;
    private float m_currentSpeed;
    private Vector2 m_dir;

    [Header("Interactions")]
    [SerializeField] private float m_moduleDetectionRange = 0.5f;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleRotation();
        HandleInteract();
    }

    private void FixedUpdate()
    {
        HandlePosition();
        m_rb.velocity = m_dir * m_currentSpeed;
    }

    private void HandlePosition()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
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

    private void HandleInteract()
    {
        if (Input.GetKey(KeyCode.E))
        {
            foreach(Module module in Module.allModules){
                if(Vector3.Distance(module.transform.position, transform.position) <= m_moduleDetectionRange){
                    module.SetState(Module.ModuleState.INTERACT);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_moduleDetectionRange);
    }
}
