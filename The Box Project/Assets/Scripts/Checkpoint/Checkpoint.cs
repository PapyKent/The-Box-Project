﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			CheckpointManager.Instance.CurrentCheckpoint = this;
	}

	private void OnDrawGizmos()
	{
		UnityEditor.Handles.Label(transform.position, "CP");
	}
}