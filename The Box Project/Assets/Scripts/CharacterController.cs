using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Private

    private void Start()
    {
        
    }

    private void Update()
    {
        m_inputs = new Vector2(Input.GetAxis("Horizontal") * m_groundSpeed, 0.0f);
        HandleSpriteDirection();
        m_collisions.Reset();
        m_groundCollider.CheckVerticalCollisions(ref m_collisions);
        HandleJump();
        ApplyGravity();
        m_inputs.y = m_velocity.y * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = transform.position + (Vector3)m_inputs;
        if(m_isJumping)
        {
            Collider2D collider = Physics2D.OverlapBox(newPos, m_groundCollider.ColliderBounds.size, 0.0f, m_obstacleLayer);
            if (collider != null)
            {
                newPos = new Vector3(newPos.x, collider.bounds.max.y + m_groundCollider.ColliderBounds.extents.y, newPos.z);
            }
        }
        transform.position = newPos;
        
            //transform.Translate(m_inputs);
    }

    private void HandleSpriteDirection()
    {
        if(m_inputs.x != 0.0f)
        {
            bool flipX = m_inputs.x > 0.0f;
            m_sprite.flipX = flipX;
        }
    }

    private void HandleJump()
    {
        m_pressedJump = Input.GetButtonDown("Jump");
        m_releasedJump = Input.GetButtonUp("Jump");

        if(!m_isJumping && m_pressedJump)
        {
            m_isJumping = true;
            m_hasReleasedJump = false;
            m_velocity.y = m_jumpSpeed;
            m_yJumpStart = transform.position.y;
        }
        else if (m_isJumping && m_releasedJump && !m_hasReleasedJump)
        {
            m_hasReleasedJump = true;
            m_velocity.y = 0.0f;
        }
        else if(transform.position.y - m_yJumpStart >= m_maxJumpHeight )
        {
            m_velocity.y = 0.0f;
            Debug.Log("On passe par la");
            m_hasReleasedJump = true;
        }
        else if(m_isJumping && m_collisions.below)
        {
            m_isJumping = false;
            Debug.Log("au sol" );
        }
    }

    private void ApplyGravity()
    {
        if(m_isJumping && m_hasReleasedJump)
        {
            if(m_velocity.y > m_maxFallSpeed)
            {
                float newVelocity = m_velocity.y + (m_gravity * Time.fixedDeltaTime);
                if (newVelocity < m_maxFallSpeed)
                {
                    newVelocity = m_maxFallSpeed;
                }
                m_velocity.y = newVelocity;
            }
        }
        else if(!m_isJumping)
        {
            m_velocity.y = 0.0f;
        }
    }

    [SerializeField]
    private float m_groundSpeed = 5.0f;
    [SerializeField]
    private float m_jumpSpeed = 20.0f;
    [SerializeField]
    private float m_maxJumpHeight = 5.0f;
    [SerializeField]
    private float m_gravity = -9.81f;
    [SerializeField]
    private float m_maxFallSpeed = -25.0f;
    [SerializeField]
    private SpriteRenderer m_sprite = null;
    [SerializeField]
    private GroundCharacterCollider m_groundCollider = null;

    [SerializeField]
    private LayerMask m_obstacleLayer;

    private Vector2 m_inputs = Vector2.zero;
    private Vector2 m_velocity = Vector2.zero;
    private GroundCharacterCollider.CollisionInfo m_collisions;
    private bool m_isJumping = false;
    private bool m_pressedJump = false;
    private bool m_releasedJump = false;
    private float m_yJumpStart = 0.0f;

    private bool m_hasReleasedJump = false;
    #endregion
}
