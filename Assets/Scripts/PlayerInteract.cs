using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInteract : MonoBehaviour
{
    private PlayerController m_controller;
    private Module m_currentlyOpenedModule = null;

    [Header("Interactions")]
    [SerializeField] private float m_moduleDetectionRange = 0.5f;

    private void Start()
    {
        m_controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(m_currentlyOpenedModule == null)
            {
                foreach (Module module in Module.allModules)
                {
                    if (Vector3.Distance(module.transform.position, transform.position) <= m_moduleDetectionRange)
                    {
                        module.EnableUI();
                        m_currentlyOpenedModule = module;
                        m_controller.enabled = false;
                        break;
                    }
                }
            } else
            {
                CloseCurrentModule();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCurrentModule();
        }
    }

    private void CloseCurrentModule()
    {
        if (m_currentlyOpenedModule != null)
        {
            m_currentlyOpenedModule.DisableUI();
            m_currentlyOpenedModule = null;
            m_controller.enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_moduleDetectionRange);
    }
}
