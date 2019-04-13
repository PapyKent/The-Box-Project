using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get { return s_instance; } }
    public bool PressedJump { get => m_pressedJump; }
    public bool ReleasedJump { get => m_releasedJump; }
    public Vector2 DirectionalInput { get => m_directionalInput; }

    public delegate void OnColorButtonPressed(GridManager.Color color);

    public void RegisterOnColorButtonPressed(OnColorButtonPressed method, bool register)
    {
        if (register)
        {
            m_colorButtonPressedListeners += method;
        }
        else
        {
            m_colorButtonPressedListeners -= method;
        }
    }


    #region Private 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        m_directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        m_pressedJump = Input.GetButtonDown("Jump");

        m_releasedJump = Input.GetButtonUp("Jump");

        if (Input.GetButtonDown("Y Button"))
        {
            m_colorButtonPressedListeners?.Invoke(GridManager.Color.YELLOW);
        }

        if (Input.GetButtonDown("X Button"))
        {
            m_colorButtonPressedListeners?.Invoke(GridManager.Color.BLUE);
        }

        if (Input.GetButtonDown("B Button"))
        {
            m_colorButtonPressedListeners?.Invoke(GridManager.Color.RED);
        }

        if (Input.GetButtonDown("Start"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Awake()
    {
        if (s_instance != null)
        {
            Debug.Log("Duplicate InputManager => Destroyed.");
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
        }
    }

    Vector2 m_directionalInput;
    private bool m_pressedJump = false;
    private bool m_releasedJump = false;

    private static InputManager s_instance = null;
    private OnColorButtonPressed m_colorButtonPressedListeners = null;


    #endregion
}
