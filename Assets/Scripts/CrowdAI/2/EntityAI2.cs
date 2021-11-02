using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityAI2 : MonoBehaviour
{
    [Header("References")]
    private GameObject m_playerGO;
    private PathFinder2 m_pathFinder;
    [SerializeField] private GameObject[] m_passingPoint;
    [SerializeField] private List<GameObject> m_skeletonList;
    [SerializeField] private List<GameObject> m_leaderList;
    private GameObject m_lastLeader;

    [Header("AI packs")]
    [SerializeField] private float m_delayPathing;
    [SerializeField] private float m_distToRepath;
    private bool[] m_isRepathed;
    private bool[] m_isTakeable;
    private GameObject m_closestPointGO;
    private float m_closestPointValue = 0;
    private bool m_pointVarReseted = true;

    private void Start()
    {
        m_playerGO = GameObject.Find("_ChasePoint");
        m_pathFinder = GetComponent<PathFinder2>();
        
        //getting start leaders
        for (int i = 0; i < m_skeletonList.Count; i++)
        {
            AgentAI2 brain = m_skeletonList[i].GetComponent<AgentAI2>();

            if (brain.m_isLeader)
            {
                m_leaderList.Add(m_skeletonList[i]);
                m_lastLeader = m_skeletonList[i];
            }
            else
            {
                brain.SetNewTarget(m_lastLeader);
            }
        }

        for (int j = 0; j < m_leaderList.Count; j++)
        {
            AgentAI2 leaderBrain = m_leaderList[j].GetComponent<AgentAI2>();
            leaderBrain.SetNewTarget(m_playerGO);
        }

        StartCoroutine(CheckingPacks());
    }

    private IEnumerator CheckingPacks()
    {
        m_isRepathed = new bool[m_leaderList.Count];
        m_isTakeable = new bool[m_passingPoint.Length];
        
        for (int i = 0; i < m_leaderList.Count; i++)
        {
            for (int j = i+1; j < m_leaderList.Count; j++)
            {
                float dist = Vector3.Distance(m_leaderList[i].transform.position, m_leaderList[j].transform.position);

                if (!m_isRepathed[i] && dist < m_distToRepath)
                {
                    m_isRepathed[i] = true;
                    m_isRepathed[j] = true;
                    Repath(i);
                }
            }
        }
        yield return new WaitForSeconds(m_delayPathing);
        StartCoroutine(CheckingPacks());
    }

    private void Repath(int p_repath)
    {
        AgentAI2 brain = m_leaderList[p_repath].GetComponent<AgentAI2>();

        for (int i = 0; i < m_passingPoint.Length; i++)
        {
            if (m_pointVarReseted)
            {
                m_pointVarReseted = false;
                m_closestPointValue = Vector3.Distance(brain.gameObject.transform.position, m_passingPoint[i].transform.position);
                m_closestPointGO = m_passingPoint[i];
            }
            else
            {
                float dist = Vector3.Distance(brain.gameObject.transform.position, m_passingPoint[i].transform.position);

                if (dist < m_closestPointValue)
                {
                    m_closestPointValue = dist;
                    m_closestPointGO = m_passingPoint[i];
                }
            }
        }

        PickAPath(brain);
    }

    private void PickAPath(AgentAI2 p_brain1)
    {
        int random = Random.Range(0, m_passingPoint.Length);
        if (m_passingPoint[random].gameObject == m_closestPointGO)
        {
            Debug.Log("same");
        }
        else
        {
            Debug.Log("different");
        }
        RepathOrder(p_brain1, m_passingPoint[random]);
    }
    
    private void RepathOrder(AgentAI2 p_brain2, GameObject p_targetFollow)
    {
        m_pointVarReseted = true;
        p_brain2.SetNewTarget(p_targetFollow);
    }
}
