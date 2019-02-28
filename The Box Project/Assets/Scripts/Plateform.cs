using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform : MonoBehaviour
{
    public enum Color
    {
        NONE,
        BLUE,
        RED, 
        YELLOW
    }

    public Color CurrentColor { get { return m_currentColor; } }


    #region Private

    private void Start()
    {
        PlayerInput.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, true);
    }

    private void OnDestroy()
    {
        PlayerInput.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, false);
    }

    private void Update()
    {
        
    }

    private void OnColorButtonPressed(Color newColor)
    {
        gameObject.SetActive(newColor == m_currentColor);
    }

    [SerializeField]
    private Color m_currentColor = Color.NONE;


    #endregion
}
