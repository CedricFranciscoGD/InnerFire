using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPath : MonoBehaviour
{
    [SerializeField] private bool m_needNavMeshObstacle;
    [SerializeField] private GameObject m_navMeshObstacleGO;
    [SerializeField] private GameObject m_finalPosRefGO;
    private bool m_isLerping = false;
    private float m_timeElapsed = 0;
    [SerializeField] private float m_lerpLength;
    [SerializeField] private float m_lerpFillingAmount;

    private void Update()
    {
        if (m_isLerping)
        {
            
            if (m_timeElapsed < (m_lerpLength / 100) * m_lerpFillingAmount)
            {
                transform.position = Vector3.Lerp(transform.position, m_finalPosRefGO.transform.position, m_timeElapsed / m_lerpLength);
                m_timeElapsed += Time.deltaTime;//time reference for lerp
            }
            else
            {
                m_isLerping = false;
                m_timeElapsed = 0;
                m_finalPosRefGO.SetActive(false);
                Destroy(m_navMeshObstacleGO);
            }
            
        }
    }

    public void OpenThePath()
    {
        m_isLerping = true;
    }
}
