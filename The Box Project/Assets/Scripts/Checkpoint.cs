using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 Position { get { return m_position; } } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckpointManager.Instance.CurrentCheckpoint = this;
    }

    private void Start()
    {
        m_position = transform.position;
    }

    private Vector3 m_position;
}
