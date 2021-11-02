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
    private NavMeshAgent m_navMeshAgent;
    private EntityAI2 m_entityAI;

    [Header("Target & destination")]
    [SerializeField] private GameObject m_targetGO;
    private bool m_hasATarget = false;
    private float m_distToChase = 10;
    private float m_distToMoveOn = 4;

    private void Start()
    {
        m_entityAI = GameObject.Find("_ChasePoint").GetComponent<EntityAI2>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_playerGO = GameObject.Find("_ChasePoint");
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
        float distToPoint = Vector3.Distance(gameObject.transform.position, m_targetGO.transform.position);
        float distToPlayer = Vector3.Distance(gameObject.transform.position, m_playerGO.transform.position);

        if (distToPlayer < m_distToChase || distToPoint < m_distToMoveOn)
        {
            m_navMeshAgent.SetDestination(m_targetGO.transform.position);
        }
        else
        {
            m_navMeshAgent.SetDestination(m_targetGO.transform.position);
        }
    }

    public void SetNewTarget(GameObject p_newTarget)
    {
        float distToPlayer = Vector3.Distance(gameObject.transform.position, m_playerGO.transform.position);
        float distToTarget = Vector3.Distance(gameObject.transform.position, m_playerGO.transform.position);

        if (distToPlayer < distToTarget)
        {
            Debug.Log("rejected");
        }
        else
        {
            m_targetGO = p_newTarget;
            m_hasATarget = true;
        }
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
