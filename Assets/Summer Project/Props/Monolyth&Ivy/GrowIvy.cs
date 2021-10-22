using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowIvy : MonoBehaviour
{
    [SerializeField] private Animator m_animIvy;
    [SerializeField] private ParticleSystem m_fireflies;
    [SerializeField] private ParticleSystem m_fireflies1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            m_animIvy.Play("IvyGrow");
            StartCoroutine(ActivateFireflies());
        }
    }

    IEnumerator ActivateFireflies()
    {
        yield return new WaitForSeconds(2);
        m_fireflies.gameObject.SetActive(true);
        m_fireflies1.gameObject.SetActive(true);
    }
}
