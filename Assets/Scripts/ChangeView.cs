using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ChangeView : MonoBehaviour
{
    [SerializeField] private GameObject m_camSide;
    public bool m_isHanged = false;

    //public int m_keyPressTime = 0;
    
    [SerializeField] private TPSMovement m_tpsMovement;

    private void Update()
    {
        if (!m_isHanged) return;
        if (m_isHanged)
        {
            m_camSide.SetActive(true);
            m_tpsMovement.enabled = false;
        }
        /*if (Input.GetKeyDown(KeyCode.X))
        {
            m_keyPressTime++;
            if (m_keyPressTime >= 2 && m_isHanged)
            {
                m_tpsMovement.enabled = true;
                Debug.Log("Vous vous échappez avec succès");
                
                
                m_keyPressTime = 0;
            }
        }*/
    }

    public void Escaping()
    {
        m_tpsMovement.enabled = true;
        m_isHanged = false;
        m_camSide.SetActive(false);
    }
}

