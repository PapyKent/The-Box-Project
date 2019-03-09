using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrackeysPlayerMovement : MonoBehaviour
{

    [SerializeField] Brackeys2DController m_controller;
    [SerializeField] float m_runSpeed = 40f;
    private float m_horizontalMove = 0f;
    private bool m_isJumping = false;
    private bool m_keepJumping = false;

    [SerializeField] float m_jumpTimer = 0.35f;
    private float m_jumpTimePressed = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {
        m_horizontalMove = Input.GetAxisRaw("Horizontal") * m_runSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            m_isJumping = true;
        }

        if (Input.GetButton("Jump"))
        {
            m_keepJumping = true;
        }

    }


    private void FixedUpdate()
    {
        //move the character
        m_controller.Move(m_horizontalMove * Time.fixedDeltaTime, m_isJumping, m_keepJumping);
        m_keepJumping = false;
        m_isJumping = false;
    }
}
