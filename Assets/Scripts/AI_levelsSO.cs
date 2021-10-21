using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI_levelSO", menuName = "ScriptableObject/AI_level")]
public class AI_levelsSO : ScriptableObject
{
    public bool m_isMoving;
    public float m_speed;
    public float m_chaseSpeed;
    public float m_stepHeight;
    public float m_dropHeight;
    public float m_jumpLength;
}
