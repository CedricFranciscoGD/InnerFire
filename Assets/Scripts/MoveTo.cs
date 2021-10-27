using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private int m_index = 0;
    [SerializeField] private float m_moveDist = 0;
    [SerializeField] private Vector3 m_originalPos;

    private void Start()
    {
        m_originalPos = transform.position;
        StartCoroutine(ChangeMap());
    }

    IEnumerator ChangeMap()
    {
        m_index = Random.Range(1, 4);
        m_moveDist = (Random.Range(0,0.2f));
        if (m_index == 1)
        {
            new Vector3(transform.position.x* m_moveDist, transform.position.y* m_moveDist, transform.position.z * m_moveDist);
        }

        yield return new WaitForSeconds(1);
    }
}
