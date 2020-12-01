using UnityEngine;
using Yube;
using Yube.Relays;

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
	public IRelayLink<Platform.Color> OnSwitchRelay { get { return m_onSwitchRelay ?? (m_onSwitchRelay = new Relay<Platform.Color>()); } }

	protected override void Awake()
	{
		base.Awake();
		m_directionalInput = Vector2.zero;
		if (GameManager.Instance.SwitchMode == GameManager.ESwitchMode.SIMPLE_SWITCH)
		{
			RegisterKeyListener(EKey.SWITCH, OnSwitchInput, true);
		}
	}

	private void OnDestroy()
	{
		if (GameManager.Instance.SwitchMode == GameManager.ESwitchMode.SIMPLE_SWITCH)
		{
			RegisterKeyListener(EKey.SWITCH, OnSwitchInput, false);
		}
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

	private void OnSwitchInput(EKey keyInput, EKeyInputEvent inputEvent)
	{
		if (inputEvent != EKeyInputEvent.DOWN)
		{
			return;
		}
		m_currentColor = m_currentColor == Platform.Color.RED ? Platform.Color.BLUE : Platform.Color.RED;
		m_onSwitchRelay?.Dispatch(m_currentColor);
	}

	private Vector2 m_directionalInput;
	private Platform.Color m_currentColor = Platform.Color.RED;
	private Relay<Platform.Color> m_onSwitchRelay = null;
}