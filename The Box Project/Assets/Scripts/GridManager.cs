using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public enum Color
    {
        NONE,
        BLUE,
        RED,
        YELLOW
    }


    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, true);

    }

    private void OnDestroy()
    {
        InputManager.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Private

    private void OnColorButtonPressed(Color newColor)
    {
        if (newColor == Color.BLUE)
        {
            m_bluePlatforms.SetActive(true);
            m_redPlatforms.SetActive(false);
            m_yellowPlatforms.SetActive(false);
        }
        else if (newColor == Color.RED)
        {
            m_bluePlatforms.SetActive(false);
            m_redPlatforms.SetActive(true);
            m_yellowPlatforms.SetActive(false);
        }
        else if (newColor == Color.YELLOW)
        {
            m_bluePlatforms.SetActive(false);
            m_redPlatforms.SetActive(false);
            m_yellowPlatforms.SetActive(true);
        }
    }

    [SerializeField]
    GameObject m_bluePlatforms = null;

    [SerializeField]
    GameObject m_redPlatforms = null;

    [SerializeField]
    GameObject m_yellowPlatforms = null;



    #endregion
}
