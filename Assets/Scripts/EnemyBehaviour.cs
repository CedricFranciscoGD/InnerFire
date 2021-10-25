using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    // TODO: Make this script looking into the player list to allow IA follow a random player on the game
    
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_isMounting;
    private Rigidbody m_rb;
    public GameObject m_obstacleMount;
    private MountTo m_mountTo;

    private Vector3 m_jumpForce;

    private Animator m_skeletonAnimator;


    //private ProceduralMapGenerator proceduralMapGenerator;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_skeletonAnimator = GetComponentInChildren<Animator>();
        m_skeletonAnimator.SetBool("isCrawling", true);
        m_skeletonAnimator.SetBool("isWalking", true);
        m_skeletonAnimator.SetBool("isRunning", true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MountArea"))
        {
            Debug.Log("Can Mount");
            m_isMounting = true;
            m_obstacleMount = other.gameObject;
            m_mountTo = m_obstacleMount.GetComponent<MountTo>();
            m_rb.useGravity = false;
            MountObstacle();
        }
        if (other.CompareTag("TargetPoint"))
        {
            m_isMounting = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TargetPoint"))
        {
            m_rb.useGravity = true;
        }
        if (other.CompareTag("MountArea"))
        {
            m_rb.useGravity = true;
            m_isMounting = false;
        }
    }
   
    void Update()
    {
        if (m_isMounting)
        {
            m_rb.velocity = new  Vector3(0,3,0 );
            m_speed = 15;
            m_skeletonAnimator.SetBool("isJumping", true);
        }
        else m_skeletonAnimator.SetBool("isJumping", false);
        Vector3 pos = Vector3.MoveTowards(transform.position, m_target.position, m_speed * Time.deltaTime);
        m_rb.MovePosition(pos);
        transform.LookAt(new Vector3(m_target.position.x, 0, m_target.position.z));
    }

    private void MountObstacle()
    {
        if (!m_isMounting) return;
        Vector3 mount = Vector3.Lerp(transform.position, m_mountTo.m_pointToLerp.position, m_speed * Time.deltaTime);
        m_rb.MovePosition(mount);
    }
}
