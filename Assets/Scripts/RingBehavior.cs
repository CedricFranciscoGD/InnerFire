using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehavior : MonoBehaviour
{ 
    private int m_startColor = 0;
    [SerializeField] private Material[] m_colorsRing;
    private Renderer m_renderer;
    

    private void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_renderer.material = m_colorsRing[m_startColor];
    }

    public void ChangeColorRing(int p_colorID)
    {
        m_renderer.material = m_colorsRing[p_colorID];
    }
}
