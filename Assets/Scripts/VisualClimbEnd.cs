using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualClimbEnd : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
