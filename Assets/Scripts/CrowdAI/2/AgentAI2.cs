using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentAI2 : MonoBehaviour
{
    [Header("Leading parameters")]
    [SerializeField] public bool m_isLeader;
    [SerializeField] private bool m_isUnderOrder;

    [Header("References")] 
    private GameObject m_playerGO;
    private EntityAI2 m_entityAI;

    [Header("Target & destination")]
    [SerializeField] private GameObject m_targetGO;
    private bool m_hasATarget = false;
    private bool m_iChasing = false;
    private float m_distToChase = 10;
    private float m_distToMoveOn = 4;

    [Header("AI/SO")]
    private NavMeshAgent m_navMeshAgent;
    [SerializeField] public AI_levelsSO[] m_aiLevelSO;
    [SerializeField] private int m_levelAI;
    private float m_speed;

    private void Start()
    {
        m_playerGO = GameObject.Find("_ChasePoint");
        m_entityAI = GameObject.Find("_ChasePoint").GetComponent<EntityAI2>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    public void LoadLevelAI(bool p_increaseAI, int p_levelAI)
    {
        if (p_increaseAI)
        {
            m_levelAI = p_levelAI;
            
            m_speed = m_aiLevelSO[p_levelAI].m_speed;

            m_navMeshAgent.speed = m_speed;
        }
    }

    private void Update()
    {
        if (m_hasATarget)
        {
            Navigation();
        }
    }

    private void Navigation()
    {
        m_navMeshAgent.SetDestination(m_targetGO.transform.position);
    }

    public void SetNewTarget(GameObject p_newTarget)
    {
        m_targetGO = p_newTarget;
        m_hasATarget = true;
    }

    private void OnDrawGizmos()
    {
        if (m_isLeader)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 7f);
        }
    }
}
