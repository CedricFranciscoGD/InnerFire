using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    [SerializeField] private float m_distanceChasePlayer;
    [SerializeField] private CrowdAI m_entityAI;

    private GameObject m_destinationGO;
    public GameObject m_playerGO;
    private NavMeshAgent m_navMesh;

    [SerializeField] public GameObject m_followLeft;
    [SerializeField] public GameObject m_followRight;

    private bool m_canChase = false;
    public bool m_isLeader = false;

    private void Start()
    {
        m_entityAI = GameObject.Find("CrowdAI").GetComponent<CrowdAI>();
        m_navMesh = GetComponent<NavMeshAgent>();
        m_playerGO = GameObject.Find("_ChasePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canChase)
        {
            PathFinder();
        }
    }
    
    //______________________________________________________________________________Destination
    private void PathFinder()
    {
        //focus player
        float distToPlayer = Vector3.Distance(m_playerGO.transform.position, gameObject.transform.position);

        if (distToPlayer < m_distanceChasePlayer)
        {
            m_navMesh.SetDestination(m_playerGO.transform.position);
        }
        else
        {
            m_navMesh.SetDestination(m_destinationGO.transform.position);

            if (m_isLeader)
            {
                float distFromLeader =
                    Vector3.Distance(gameObject.transform.position, m_destinationGO.transform.position);
                if (distFromLeader < 3)
                {
                    m_entityAI.GetPassingPoints();
                }
            }
        }
    }
    
    public void SetNewFollowTarget(GameObject p_newDestination)
    {
        m_destinationGO = p_newDestination;
        m_canChase = true;
    }
}
