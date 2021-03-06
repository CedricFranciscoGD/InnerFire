using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    //__________________________________________
    // JUSTE POUR LE GYM
    [SerializeField] private bool m_isGym;
    [SerializeField] private bool m_canJump;
    [SerializeField] private bool m_canChasePlayer;
    //__________________________________________
    
    
    [Header("Player & chasing parameters")]
    private GameObject m_playerRef;
    private Vector3 m_playerPos;
    private CharacterController m_charaController;
    private ChangeView m_tummoCam;
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private float m_hearTriggerDistance;
    [SerializeField] private float m_visionTriggerDistance;
    [SerializeField] private float m_attackTriggerDistance;
    [SerializeField] private float m_timeToAttackAgain;
    private bool m_canAttack = true;
    private bool m_isChasing = false;
    private bool m_eyeContact = false;

    //QTE to change
    private bool m_isQTEactive = false;
    private int m_counterQTE;
    private int m_refCounterQTE = 10;
    private float m_timeQTE;
    private float m_reftimeQTE = 3f;

    [Header("Enemy ressources")]
    private NavMeshAgent m_enemyNavMesh;
    [SerializeField] private AI_levelsSO[] m_levelAI;
    [SerializeField] private int m_aiLevel = 0;
    [SerializeField] private bool m_isAlive = false;
    [SerializeField] private float m_baseSpeed;
    [SerializeField] private float m_ChaseSpeed;
    [SerializeField] private float m_stepHeight;
    [SerializeField] private float m_dropHeight;
    [SerializeField] private float m_jumpLength;

    [Header("Raycast")]
    private RaycastHit m_rayHit;
    private Ray m_visionRay;

    [Header("Patrol parameters")]
    [SerializeField] public Transform[] m_patrolPath;
    private int m_progress;
    private int m_progressMax;
    private Vector3 targetMovePointVectorPosition;
    [SerializeField] private float m_distToContinue;


    /// ANIMATIONS
    [SerializeField] private Animator m_skeletonAnimator;
    

    private void Start()
    {
        m_progress = 0;
        m_playerRef = GameObject.Find("p_Tummo");
        m_charaController = m_playerRef.GetComponent<CharacterController>();
        m_tummoCam = m_playerRef.GetComponent<ChangeView>();
        m_enemyNavMesh = GetComponent<NavMeshAgent>();
        m_enemyNavMesh.speed = m_baseSpeed;
        m_progressMax = m_patrolPath.Length - 1;
        
        LoadLevelAI(false);
        
        // GYM PART
        if (m_isGym)
        {
            if (m_aiLevel == 1)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
            }
            if (m_aiLevel == 2)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
            }
            if (m_aiLevel == 3)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
                m_skeletonAnimator.SetBool("isRunning", true);
            }
            if (m_aiLevel == 4)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
                m_skeletonAnimator.SetBool("isRunning", true);
                m_canJump = true;
            }
        }
    }
    
    public void LoadLevelAI(bool p_increaseAI)
    {
        if (p_increaseAI)
        {
            m_aiLevel += 1;
            if (m_aiLevel == 1)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
            }
            if (m_aiLevel == 2)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
            }
            if (m_aiLevel == 3)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
                m_skeletonAnimator.SetBool("isRunning", true);
            }
            if (m_aiLevel == 4)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
                m_skeletonAnimator.SetBool("isRunning", true);
                m_canJump = true;
            }
            
        }
        
        m_isAlive = m_levelAI[m_aiLevel].m_isMoving;
        m_baseSpeed = m_levelAI[m_aiLevel].m_speed;
        m_ChaseSpeed = m_levelAI[m_aiLevel].m_chaseSpeed;
        m_stepHeight = m_levelAI[m_aiLevel].m_stepHeight;
        m_dropHeight = m_levelAI[m_aiLevel].m_dropHeight;
        m_jumpLength = m_levelAI[m_aiLevel].m_jumpLength;

        m_enemyNavMesh.speed = m_baseSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_canJump) return;
        else m_skeletonAnimator.SetBool("isJumping", true);
    }

    private void Update()
    {
        //Debug.Log(m_aiLevel);
        
        if (m_isAlive)
        {
            m_playerPos = m_playerRef.transform.position;

            HearDetect();
            VisionDetect();
            AttackUpdate();

            if (!m_isChasing)
            {
                MoveOnPath();
            }
            else
            {
                m_enemyNavMesh.SetDestination(m_playerPos);
            }
        }

        if (m_isQTEactive && m_canAttack)
        {
            m_charaController.enabled = false;
            m_tummoCam.m_isHanged = true;
            m_enemyNavMesh.speed = 0;
        
            if (m_timeQTE < m_reftimeQTE)
            {
                m_timeQTE += Time.deltaTime;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    m_counterQTE += 1;
                    if (m_counterQTE > m_refCounterQTE)
                    {
                        m_isQTEactive = false;
                        m_charaController.enabled = true;
                        m_tummoCam.m_isHanged = false;
                        StartCoroutine(AttackAgainDelay());
                    }
                }
            }
            else
            {
                m_isQTEactive = false;
                m_timeQTE = 0;
                m_counterQTE = 0;
                m_charaController.enabled = true;
                StartCoroutine(AttackAgainDelay());
            }
        }

        if (m_isGym)
        {
            if (m_aiLevel == 1)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
            }
            if (m_aiLevel == 2)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
            }
            if (m_aiLevel == 3)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
                m_skeletonAnimator.SetBool("isRunning", true);
            }
            if (m_aiLevel == 4)
            {
                m_skeletonAnimator.SetBool("isCrawling", true);
                m_skeletonAnimator.SetBool("isWalking", true);
                m_skeletonAnimator.SetBool("isRunning", true);
                m_skeletonAnimator.SetBool("isJumping", true);
            }
        }
        else return;
        
    }

    private IEnumerator AttackAgainDelay()
    {
        m_canAttack = false;
        yield return new WaitForSeconds(m_timeToAttackAgain);
        m_canAttack = true;
    }

    private void AttackUpdate()
    {
        if (Vector3.Distance(m_playerPos, transform.position) < m_attackTriggerDistance)
        {
            m_isQTEactive = true;
        }
        else
        {
            m_isQTEactive = false;
        }
    }

    private void HearDetect()
    {
        if (m_canChasePlayer && Vector3.Distance(m_playerPos, transform.position) < m_hearTriggerDistance)
        {
            m_isChasing = true;
            SpeedIncrease();
        }
    }

    private void SpeedIncrease()
    {
        m_enemyNavMesh.speed = m_ChaseSpeed;
    }
    
    private void VisionDetect()
    {
        m_visionRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(m_visionRay.origin, transform.forward * m_visionTriggerDistance, Color.red);

        if (Physics.Raycast(m_visionRay, out m_rayHit, m_visionTriggerDistance, m_playerMask, QueryTriggerInteraction.Collide))
        {
            m_isChasing = true;
            SpeedIncrease();
        }
    }

    private void MoveOnPath()
    {
        float distToWaypoint = Vector3.Distance(transform.position, m_patrolPath[m_progress].position);
        
        if (distToWaypoint < m_distToContinue)
        {
            Debug.Log("PROC");
            if (m_progress < m_progressMax)
            {
                m_progress += 1;
                if(m_isGym && m_aiLevel < 5) m_aiLevel += 1;
                else if(m_isGym && m_aiLevel == 5) return;
            }
            else
            {
                m_progress = 0;
            }
        }
        
        targetMovePointVectorPosition = m_patrolPath[m_progress].position;
        m_enemyNavMesh.SetDestination(targetMovePointVectorPosition);
    }
}
