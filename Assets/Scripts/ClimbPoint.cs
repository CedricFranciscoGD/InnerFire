using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbPoint : MonoBehaviour
{
    [SerializeField] private int m_numberPointToClimb;
    [SerializeField] private Transform[] m_destinationPos;
    private int m_climbProgress = 0;
    private GameObject m_playerRef;
    private Vector3 m_playerPos;
    private CharacterController m_charaController;
    [SerializeField] private float m_climbTriggerDistance;
    private bool m_canClimb;
    private bool m_isClimbing = false;
    private float m_timeElapsed = 0;
    [SerializeField] private float m_lerpLength;
    [SerializeField] private float m_lerpFillingAmount;
    private Vector3 m_stockPlayerPos;
    private bool m_procOnce = false;
    
    /// ANIMATIONS
    [SerializeField] private Animator m_tummoAnimator;

    private void Start()
    {
        m_playerRef = GameObject.Find("p_Tummo");
        m_charaController = m_playerRef.GetComponent<CharacterController>();
    }

    private void Update()
    {
        m_playerPos = m_playerRef.transform.position;

        //LERPS
        if (m_isClimbing)
        {
            ClimbLerping();
        }

        Debug.Log(m_canClimb);
        Checking();
    }

    private void ClimbLerping()
    {
        StockVar();
        if (m_timeElapsed < (m_lerpLength / 100) * m_lerpFillingAmount)
        {
            m_playerRef.transform.position = Vector3.Lerp(m_stockPlayerPos, m_destinationPos[m_climbProgress].transform.position, m_timeElapsed / m_lerpLength);
            m_timeElapsed += Time.deltaTime;//time reference for lerp
        }
        else
        {
            m_timeElapsed = 0;

            if (m_climbProgress < m_destinationPos.Length-1)
            {
                m_climbProgress += 1;
                m_procOnce = false;
            }
            else
            {
                m_climbProgress = 0;
                m_isClimbing = false;
                m_charaController.enabled = true;
                m_procOnce = false;
            }
        }
    }
    
    private void StockVar()
    {
        if (!m_procOnce)
        {
            m_stockPlayerPos = m_playerRef.transform.position;
            m_procOnce = true;
        }
        
    }

    private void Checking()
    {
        if (m_canClimb)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_tummoAnimator.SetBool("isHanging", true);
                m_charaController.enabled = false;
                m_isClimbing = true;
                StartCoroutine(TempFixes());
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (m_canClimb)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.TryGetComponent(out CharacterController p_controller))
        {
            m_canClimb = true;
        }
    }
    
    private void OnTriggerExit(Collider p_other)
    {
        m_canClimb = false;
    }

    private IEnumerator TempFixes()
    {
        yield return new WaitForSeconds(m_numberPointToClimb*1.2f);
        m_tummoAnimator.SetBool("isHanging", false);
        yield return new WaitForSeconds(3f);
        m_canClimb = false;
    }
}
