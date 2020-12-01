using UnityEngine;
using Yube;

public class LocalInputManager : InputManager<LocalInputManager.EAxis, LocalInputManager.EKey>
{
	public enum EAxis
	{
		HORIZONTAL,
		VERTICAL
	}

	public enum EKey
	{
		JUMP,
		SWITCH_RED,
		SWITCH_BLUE,
		SWITCH_YELLOW,
		SWITCH
	}

	public static new LocalInputManager Instance { get { return InputManager<EAxis, EKey>.Instance as LocalInputManager; } }

	public Vector2 DirectionalInput { get => m_directionalInput; }

	protected override void Awake()
	{
		base.Awake();
		m_directionalInput = Vector2.zero;
	}

	protected override void Update()
	{
		m_directionalInput = new Vector2(Input.GetAxisRaw(EAxis.HORIZONTAL.ToString()), Input.GetAxisRaw(EAxis.VERTICAL.ToString()));
		base.Update();

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
	}

	private Vector2 m_directionalInput;
}