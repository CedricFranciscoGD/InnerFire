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
    public bool m_isHanged = false;

    [SerializeField] private int m_keyPressTime = 0;

    private void Update()
    {
        if (!m_isHanged) return;
        if (m_isHanged)
        {
            m_camSide.SetActive(true);
        }
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

