﻿using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance { get { return s_instance; } }
	public Vector2 DirectionalInput { get => m_directionalInput; }

	public delegate void OnColorButtonPressed(Platform.Color color);

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
			m_colorButtonPressedListeners?.Invoke(Platform.Color.YELLOW);
		}

		if (Input.GetButtonDown("X Button"))
		{
			m_colorButtonPressedListeners?.Invoke(Platform.Color.BLUE);
		}

		if (Input.GetButtonDown("B Button"))
		{
			m_colorButtonPressedListeners?.Invoke(Platform.Color.RED);
		}

		if (Input.GetKeyUp(KeyCode.KeypadMinus))
		{
			if (Time.timeScale > 0.2)
			{
				Time.timeScale -= 0.2f;
			}
		}

		if (Input.GetKeyUp(KeyCode.KeypadPlus))
		{
			if (Time.timeScale < 4)
			{
				Time.timeScale += 0.2f;
			}
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