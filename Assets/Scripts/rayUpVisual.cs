using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayUpVisual : MonoBehaviour
{
    private Ray m_ray;
    [SerializeField] private float m_length;

    private void Update()
    {
        RayVisual();
    }

    private void RayVisual()
    {
        m_ray = new Ray(transform.position, Vector3.up);
        Debug.DrawRay(m_ray.origin, transform.up * m_length, Color.green);
    }
}
