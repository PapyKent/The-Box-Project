﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCharacterCollider : MonoBehaviour
{
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public Vector2 aboveCollider, belowCollider;
        public Vector2 leftCollider, rightCollider;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            aboveCollider = belowCollider = leftCollider = rightCollider = Vector2.zero;
        }
    }

    public Bounds ColliderBounds { get { return m_groundCollider.bounds; } }

    public void CheckVerticalCollisions(ref CollisionInfo collisions)
    {
        Vector2 origin = m_groundCollider.bounds.min;
        for (int i = 0; i < m_verticalRaycastCount + 1; i++)
        {
            Debug.DrawRay(origin, (Vector3.up * -1) * m_shootRayDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.up * -1, m_shootRayDistance, m_collisionMask);
            
            if(hit.collider != null )
            {
                if(hit.collider.tag == "Ground")
                {
                    collisions.below = true;
                    collisions.belowCollider = hit.point;
                }
            }
            origin = new Vector2(origin.x + (m_groundCollider.bounds.size.x / m_verticalRaycastCount), origin.y);
            if(origin.x > m_groundCollider.bounds.max.x)
            {
                origin = m_groundCollider.bounds.max;
            }
        }
    }

    #region Private

    private void Start()
    {
        m_horizontalRaycastCount = Mathf.RoundToInt(m_groundCollider.bounds.size.y / m_distanceBetweenRay);
        m_verticalRaycastCount = Mathf.RoundToInt(m_groundCollider.bounds.size.x / m_distanceBetweenRay);
    }

    [SerializeField]
    private float m_distanceBetweenRay = 0.1f;
    [SerializeField]
    private float m_shootRayDistance = 0.1f;
    [SerializeField]
    private BoxCollider2D m_groundCollider = null;
    [SerializeField]
    private LayerMask m_collisionMask;

    private float m_verticalRaycastCount = 0;
    private float m_horizontalRaycastCount = 0;
    #endregion
}