using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance { get { return s_instance; } }
	public Vector2 DirectionalInput { get => m_directionalInput; }

	public delegate void OnColorButtonPressed(GridManager.Color color);

	public delegate void OnJumpInputPressed(bool buttonUp);

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

	public void RegisterOnJumpInput(OnJumpInputPressed method, bool register)
	{
		if (register)
		{
			m_jumpInputListeners += method;
		}
		else
		{
			m_jumpInputListeners -= method;
		}
	}

	#region Private

	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		m_directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetButtonDown("Jump"))
		{
			m_jumpInputListeners?.Invoke(false);
		}

		if (Input.GetButtonUp("Jump"))
		{
			m_jumpInputListeners?.Invoke(true);
		}

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

		//if (Input.GetButtonDown("Start"))
		//{
		//	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		//}
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

	private Vector2 m_directionalInput;
	private static InputManager s_instance = null;
	private OnColorButtonPressed m_colorButtonPressedListeners = null;
	private OnJumpInputPressed m_jumpInputListeners = null;

	#endregion Private
}