using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Start"))
        {
            m_player.transform.position = transform.position;
        }
    }

    [SerializeField]
    private Player m_player = null;
}
