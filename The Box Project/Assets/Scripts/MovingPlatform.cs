using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");

        if (!isMoving)
        {
            platformController.enabled = !platformController.enabled;
            Debug.Log("active platform");
            isMoving = true;
        }
        
    }

    [SerializeField]
    private PlatformController platformController;

    private bool isMoving = false;
}
