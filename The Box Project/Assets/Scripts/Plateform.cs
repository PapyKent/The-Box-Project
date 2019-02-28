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
        if(m_currentColor != Color.NONE)
        {
            bool sameColor = newColor == m_currentColor;
            m_activeSpriteRenderer.enabled = sameColor;
            m_inactiveSpriteRenderer.enabled = !sameColor;
            m_collider.enabled = sameColor;
        }
    }

    [SerializeField]
    private Color m_currentColor = Color.NONE;
    [SerializeField]
    private SpriteRenderer m_activeSpriteRenderer = null;
    [SerializeField]
    private SpriteRenderer m_inactiveSpriteRenderer = null;
    [SerializeField]
    private Collider2D m_collider = null;


    #endregion
}
