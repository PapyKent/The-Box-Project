using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLevel : MonoBehaviour
{
    [SerializeField] int nextLevelID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            GameManager.Instance.loadLevel(nextLevelID);
    }
}
