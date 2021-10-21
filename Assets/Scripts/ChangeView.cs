using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChangeView : MonoBehaviour
{
    [SerializeField] private float m_duration = 1f;
    [SerializeField] private Transform m_transDest;
    [SerializeField] private GameObject m_cam;
    [SerializeField] private GameObject m_camSide;
    [SerializeField] private bool m_isHanged = false;

    [SerializeField] private int m_keyPressTime = 0;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) ;
        {
            Debug.Log("Dans la zone");
            m_camSide.SetActive(true);
            m_isHanged = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"));
        {
            Debug.Log("Hors de la zone");
            m_camSide.SetActive(false);
        }
    }

    private void Update()
    {
        if (!m_isHanged) return;
        if (Input.GetKey(KeyCode.X))
        {
            m_keyPressTime += 1;
            if (m_keyPressTime >= 3)
            {
                Debug.Log("Vous vous échappez avec succès");
                m_camSide.SetActive(false);
                m_keyPressTime = 0;
                m_isHanged = false;
            }
        }
    }
    
}

