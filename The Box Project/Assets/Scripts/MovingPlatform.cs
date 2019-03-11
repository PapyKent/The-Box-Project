﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    public void ResetMovingPlatform()
    {
        m_isMoving = false;
        percentBetweenWaypoints = 0.0f;
        transform.position = globalWaypoints[0];
        if (m_alwaysMove)
            m_isMoving = true;
    }

    public void SetCurrentPlayer(CharacterController player)
    {
        m_playerOnPlatform = player;
        if (m_playerOnPlatform != null)
            m_isMoving = true;
    }

    #region Private

    private void Start()
    {
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < globalWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
        if(m_alwaysMove)
        {
            m_isMoving = true;
        }
    }

    private void Update()
    {
        if(m_isMoving)
        {
            Vector3 velocity = CalculatePlatformMovement();
            CalculatePassengerMovement(velocity);
            transform.Translate(velocity);
        }
    }

    private Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * m_speed / distanceBetweenWaypoints;

        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0f;
            fromWaypointIndex++;

            if (!m_cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }

            nextMoveTime = Time.time + m_waitTIme;
        }

        return newPos - transform.position;
    }

    private void CalculatePassengerMovement(Vector3 velocity)
    {
        if(m_playerOnPlatform != null)
        {
            m_playerOnPlatform.SetExternalForce(velocity);
        }
    }

    private float Ease(float x)
    {
        float a = easeAmount + 1f;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }



    [SerializeField]
    private bool m_alwaysMove = false;
    [SerializeField]
    private Vector3[] localWaypoints;

    [SerializeField]
    private float m_speed;
    private bool m_cyclic;
    private float m_waitTIme;
    [Range(0, 2)]
    [SerializeField]
    public float easeAmount;

    private int fromWaypointIndex;
    private float percentBetweenWaypoints;
    private float nextMoveTime;


    private bool m_isMoving = false;
    private Vector3[] globalWaypoints;
    private CharacterController m_playerOnPlatform = null;
    #endregion

}
