using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMovement : MonoBehaviour
{
    public CharacterController m_controller;
    public Transform m_cameraRef;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_smoothMove = 0.1f;
    private float m_smoothVelocity;
    private bool m_isSprinting;
    [SerializeField] private float m_sprintSpeed;
    
    private Vector3 m_VectorVelocity;
    [SerializeField] private float m_gravity = -6f;

    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private float m_groundCheckDistance = 0.2f;
    public LayerMask CanStandOnMask;
    private bool m_isOnGround;
    
    [SerializeField] private float m_jumpHeight = 3f;
    
    
    
    // ANIMATIONS
    [SerializeField] private Animator m_animator;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //GROUND CHECK
        m_isOnGround = Physics.CheckSphere(m_groundCheck.position, m_groundCheckDistance, CanStandOnMask);

        if (m_isOnGround && m_VectorVelocity.y < 0)
        {
            m_VectorVelocity.y = -2f;
        }

        
            
            float Horizontal = Input.GetAxisRaw("Horizontal");
            float Vertical = Input.GetAxisRaw("Vertical");
            Vector3 Direction = new Vector3(Horizontal, 0f, Vertical).normalized;
       
        
        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_isSprinting = true;
            m_animator.SetBool("isRunning", true);
        }
        else
        {
            m_isSprinting = false;
            m_animator.SetBool("isRunning", false);
        }

        if (Direction.magnitude >= 0.1f)
        {
            m_animator.SetBool("isWalking", true);
            float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + m_cameraRef.eulerAngles.y;
            float angle =
                Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_smoothVelocity, m_smoothMove);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (m_isSprinting)
            {
                m_controller.Move(moveDirection.normalized * m_sprintSpeed * Time.deltaTime);
            }
            else
            {
                m_controller.Move(moveDirection.normalized * m_speed * Time.deltaTime);
            }
        }
        else m_animator.SetBool("isWalking", false);
        
        //JUMP
        if (Input.GetButtonDown("Jump") && m_isOnGround)
        {
            Debug.Log("Is Jumping");
            m_VectorVelocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
            StartCoroutine(Jump());
        }

        //GRAVITY
        m_VectorVelocity.y += m_gravity * Time.deltaTime;
        if (m_controller != null)
        {
            m_controller.Move(m_VectorVelocity * Time.deltaTime);
        }
    }

    IEnumerator Jump()
    {
        m_animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(.7f);
        m_animator.SetBool("isJumping", false);
    }
}
