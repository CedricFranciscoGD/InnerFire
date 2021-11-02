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
    [SerializeField] private AI_levelsSO[] m_aiEntitySO;

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
            brain.m_aiLevelSO = m_aiEntitySO;

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
    }
}
