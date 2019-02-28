using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Private

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        if(m_player != null)
        {
            transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, transform.position.z);
        }
    }

    [SerializeField]
    private Player m_player = null;

    #endregion
}
