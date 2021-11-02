using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSphere : MonoBehaviour
{
    [SerializeField] private bool m_useSerialized;
    [SerializeField] private float m_radius;
    private void OnDrawGizmos()
    {
        if (m_useSerialized)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_radius);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 5f);
        }
    }
}
