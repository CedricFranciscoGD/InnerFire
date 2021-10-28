using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_isMounting;
    [SerializeField] private bool m_canAttack;
    private Rigidbody m_rb;


    private Animator m_skeletonAnimator;
    [SerializeField] private ChangeView m_changeView;

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
            m_isMounting = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MountArea"))
        {
            m_isMounting = false;
        }
    }
   
    void Update()
    {
        if (m_isMounting)
        {
            StartCoroutine(Jump());
        }
        transform.LookAt(new Vector3(m_target.position.x, transform.position.y, m_target.position.z));
        if (Vector3.Distance(m_target.position, transform.position) < 1.3f)
        {
            if(m_canAttack)
            {
                StartCoroutine(Attack());
                Debug.Log("Tummo s'est fait atrrapée");
            }
        }
        else
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, m_target.position, m_speed * Time.deltaTime);
            m_rb.MovePosition(pos);
            m_skeletonAnimator.SetBool("isAttacking", false);
        }
    }

    IEnumerator Jump()
    {
        m_rb.velocity = new Vector3(0,3f,0 );
        m_speed = 10;
        m_skeletonAnimator.SetBool("isJumping", true);
        yield return new WaitForSeconds(1);
        m_speed = 4;
        m_skeletonAnimator.SetBool("isJumping", false);;
    }
    IEnumerator Attack()
    {
        m_canAttack = false;
        m_changeView.m_isHanged = true;
        m_skeletonAnimator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(5);
        m_canAttack = true;
    }
    
}
