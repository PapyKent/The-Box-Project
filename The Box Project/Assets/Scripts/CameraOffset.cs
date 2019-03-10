using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CinemachineTransposer transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
            transposer.m_FollowOffset.x += 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region Private
    [SerializeField]
    CinemachineVirtualCamera vcam = null;
    #endregion
}
