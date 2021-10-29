using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CrowdAI : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private GameObject m_playerGO;

    [Header("Pack Set")] 
    [SerializeField] private PackAI[] m_packsArray;
    private int TempFixAdressArray = 0;

    [Header("Skeleton Pack #1")] 
    [SerializeField] private int m_numberOfPacks;
    [SerializeField] private GameObject[] m_skeletonPack1;
    [SerializeField] private GameObject[] m_skeletonPack2;

    [SerializeField] private GameObject[] m_passingPoints;
    [SerializeField] private float m_wayPointAvailableDist;

    private void Start()
    {
        m_playerGO = GameObject.Find("_ChasePoint");
        SetPack();

        m_packsArray = new PackAI[m_numberOfPacks];

        GetPassingPoints();
    }
    
    //create pack script and add skeleton to it
    private void SetPack()
    {
        m_packsArray[0].GetPack(m_skeletonPack1);
        m_packsArray[1].GetPack(m_skeletonPack2);
    }

    private void GetPassingPoints()
    {
        for (int i = 0; i < m_passingPoints.Length; i++)
        {
            float dist = Vector3.Distance(m_playerGO.transform.position, m_passingPoints[i].transform.position);
            if (dist < m_wayPointAvailableDist)
            {
                int random = Random.Range(0, m_packsArray.Length);
                Debug.Log(random);
                m_packsArray[random].NewPath(m_passingPoints[i]);
            }
        }
    }
}
