using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get { return s_instance; } }

    public delegate void OnColorButtonPressed(Plateform.Color color);

    public void RegisterOnColorButtonPressed(OnColorButtonPressed method, bool register)
    {
        if(register)
        {
            m_colorButtonPressedListeners += method;
        }
        else
        {
            m_colorButtonPressedListeners -= method;
        }
    }

    private void Awake()
    {
        if(s_instance != null)
        {
            Debug.Log("Duplicate PlayerInput => Destroyed.");
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
        }
    }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            player.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
        }

        if(Input.GetButtonDown("Y Button"))
        {
            m_colorButtonPressedListeners?.Invoke(Plateform.Color.YELLOW);
        }

        if (Input.GetButtonDown("X Button"))
        {
            m_colorButtonPressedListeners?.Invoke(Plateform.Color.BLUE);
        }

        if (Input.GetButtonDown("B Button"))
        {
            m_colorButtonPressedListeners?.Invoke(Plateform.Color.RED);
        }
    }

    private Player player;
    private static PlayerInput s_instance = null;
    private OnColorButtonPressed m_colorButtonPressedListeners = null;
}
