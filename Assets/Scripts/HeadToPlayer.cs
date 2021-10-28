using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadToPlayer : MonoBehaviour
{
    private GameObject m_player;
    private Transform m_playerTarget;

    private void Start()
    {
        m_player = GameObject.Find("p_Tummo");
        m_playerTarget = m_player.transform;
    }

    private void Update()
    {
        transform.LookAt(m_playerTarget);
    }
}
