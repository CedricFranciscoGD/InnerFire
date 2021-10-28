using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ConduitBehavior : MonoBehaviour
{
    [Header("Player & conduit parameters")]
    private GameObject m_playerRef;
    private Vector3 m_playerPos;
    [SerializeField] private int m_aiToLoad;
    [SerializeField] private bool m_unlockClimb;
    [SerializeField] private bool m_unlockJump;
    [SerializeField] private GameObject m_climbObstacles;
    [SerializeField] private GameObject m_jumpObstacles;

    [SerializeField] private float m_triggerDistance;
    private bool m_canTrigger = false;
    private bool m_isActive = false;

    [Header("Enemies")]
    [SerializeField] private GameObject[] m_enemies;
    [SerializeField] private float m_delayToUpgradeFoes;
    [SerializeField] private float m_delayToOpenPath;

    private Animator m_animator;
    
    [SerializeField] private GameObject m_reanimZone;
    [SerializeField] private float m_maxZone;
    private Vector3 m_growingZone;
    private float m_sizeZone = 0;
    private bool m_zoneIsGrowing;
    private float m_timeElapsed = 0;
    [SerializeField] private float m_lerpLength;
    [SerializeField] private float m_lerpFillingAmount;
    
    [SerializeField] private OpenPath[] m_bridgeToOpen;
    
    /// ANIMATIONS
    [SerializeField] private Animator m_tummoAnimator;

    private void Start()
    {
        m_playerRef = GameObject.Find("p_Tummo");
        m_animator = GetComponent<Animator>();
        m_reanimZone.SetActive(false);
        m_growingZone = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        TriggerUpdate();

        if (m_canTrigger)
        {
            if (!m_isActive && Input.GetKeyUp(KeyCode.R))
            {
                m_tummoAnimator.SetBool("isUsing", true);
                StartCoroutine(UpgradeEnemies());
                m_isActive = true;
                m_animator.Play("IvyGrow");
                m_reanimZone.SetActive(true);
                m_zoneIsGrowing = true;
                StartCoroutine(OpenPathCoroutine());
            }
        }
        
        if (m_zoneIsGrowing)
        {
            if (m_timeElapsed < (m_lerpLength / 100) * m_lerpFillingAmount)
            {
                m_sizeZone = Mathf.Lerp(0, m_maxZone, m_timeElapsed / m_lerpLength);
                m_reanimZone.transform.localScale = new Vector3(m_sizeZone, 0.3f, m_sizeZone);
                m_timeElapsed += Time.deltaTime;//time reference for lerp
            }
            else
            {
                m_zoneIsGrowing = false;
                m_timeElapsed = 0;
            }
        }
    }

    private IEnumerator OpenPathCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        m_tummoAnimator.SetBool("isUsing", false);
        yield return new WaitForSeconds(m_delayToOpenPath);
        for (int i = 0; i < m_bridgeToOpen.Length; i++)
        {
            m_bridgeToOpen[i].OpenThePath();
        }
    }

    private void TriggerUpdate()
    {
        m_playerPos = m_playerRef.transform.position;
        float distPlayerToConduit = Vector3.Distance(transform.position, m_playerPos);

        if (distPlayerToConduit < m_triggerDistance)
        {
            m_canTrigger = true;
        }
        else
        {
            m_canTrigger = false;
        }
    }

    IEnumerator UpgradeEnemies()
    {
        yield return new WaitForSeconds(m_delayToUpgradeFoes);
        for (int i = 0; i < m_enemies.Length; i++)
        {
            m_enemies[i].GetComponent<EnemyBehavior>().LoadLevelAI(true, m_aiToLoad);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }

        if (m_unlockClimb)
        {
            m_climbObstacles.SetActive(false);
        }

        if (m_jumpObstacles)
        {
            m_jumpObstacles.SetActive(false);
        }
    }
}
