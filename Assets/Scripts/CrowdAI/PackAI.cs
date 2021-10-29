using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class PackAI : MonoBehaviour
{
    private GameObject m_playerGO;
    private GameObject[] m_packGO;
    [SerializeField] private SkeletonAI[] m_skeletonAI;

    private Transform m_nextDestination;
    private float m_distanceToTravelTo = 10;

    private int m_random = 0;

    private bool m_isChasingDirectly;

    private void Start()
    {
        m_playerGO = GameObject.Find("_ChasePoint");
    }

    public void GetPack(GameObject[] p_packGO)
    {
        m_packGO = new GameObject[p_packGO.Length];
        m_skeletonAI = new SkeletonAI[p_packGO.Length];
        
        for (int i = 0; i < m_packGO.Length; i++)
        {
            m_packGO[i] = p_packGO[i];
            m_skeletonAI[i] = p_packGO[i].GetComponent<SkeletonAI>();
        }
        GetAIs();
    }
    
    private void GetAIs()
    {
        for (int i = 0; i < m_packGO.Length; i++)
        {
            m_skeletonAI[i] = m_packGO[i].GetComponent<SkeletonAI>();
            
            if (i < 1)
            {
                m_skeletonAI[i].SetNewFollowTarget(m_skeletonAI[0].m_playerGO);
                m_skeletonAI[i].m_isLeader = true;
            }
            else if (i == 1)
            {
                m_skeletonAI[i].SetNewFollowTarget(m_skeletonAI[0].m_followLeft);
            }
            else 
            {
                m_skeletonAI[i].SetNewFollowTarget(m_skeletonAI[0].m_followRight);
            }
        }
    }

    public void NewPath(GameObject p_newPath)
    {
        m_skeletonAI[0].SetNewFollowTarget(p_newPath);
    }
}
