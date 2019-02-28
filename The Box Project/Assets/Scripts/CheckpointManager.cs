﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get { return s_instance; } }

    public Checkpoint CurrentCheckpoint { get { return m_currentCheckpoint; } set { m_currentCheckpoint = value; } }

    #region Private

    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this);
            m_currentCheckpoint = m_beginCheckpoint;
        }
        else
        {
            Debug.Log("Checkpoint manager already exists. Destroying.");
        }
    }

    [SerializeField]
    private Checkpoint m_beginCheckpoint = null;
 
    private Checkpoint m_currentCheckpoint = null;
    private static CheckpointManager s_instance = null;

    #endregion
}