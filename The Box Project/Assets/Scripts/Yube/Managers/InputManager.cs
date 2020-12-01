using System;
using System.Collections.Generic;
using UnityEngine;
using Yube.Relays;

namespace Yube
{
	public abstract class InputManager<AXIS_INPUT, KEY_INPUT> : Singleton<InputManager<AXIS_INPUT, KEY_INPUT>>
		where AXIS_INPUT : struct, IConvertible
		where KEY_INPUT : struct, IConvertible
	{
		public enum EKeyInputEvent
		{
			DOWN,
			PRESSED,
			UP
		}

		public void RegisterAxisListener(AXIS_INPUT axisInput, Action<AXIS_INPUT, float> listener, bool rawAxis, bool register)
		{
			(rawAxis ? m_axisRawInputsRelays[axisInput] : m_axisInputsRelays[axisInput]).RegisterListener(listener, register);
		}

		public void RegisterKeyListener(KEY_INPUT keyInput, Action<KEY_INPUT, EKeyInputEvent> listener, bool register)
		{
			m_keyInputsRelays[keyInput].RegisterListener(listener, register);
		}

		public void Enable(bool enable)
		{
			enabled = enable;
		}

		#region Private

		protected override void Awake()
		{
			base.Awake();
			m_axisInputsRelays = new Dictionary<AXIS_INPUT, Relay<AXIS_INPUT, float>>();
			m_axisRawInputsRelays = new Dictionary<AXIS_INPUT, Relay<AXIS_INPUT, float>>();
			m_keyInputsRelays = new Dictionary<KEY_INPUT, Relay<KEY_INPUT, EKeyInputEvent>>();

			m_axisInputEnumValues = Enum.GetValues(typeof(AXIS_INPUT));
			m_keyInputEnumValues = Enum.GetValues(typeof(KEY_INPUT));

			foreach (AXIS_INPUT input in m_axisInputEnumValues)
			{
				m_axisInputsRelays.Add(input, new Relay<AXIS_INPUT, float>());
			}

			foreach (AXIS_INPUT input in m_axisInputEnumValues)
			{
				m_axisRawInputsRelays.Add(input, new Relay<AXIS_INPUT, float>());
			}

			foreach (KEY_INPUT input in m_keyInputEnumValues)
			{
				m_keyInputsRelays.Add(input, new Relay<KEY_INPUT, EKeyInputEvent>());
			}
		}

		protected virtual void Update()
		{
			foreach (AXIS_INPUT axisInput in m_axisInputEnumValues)
			{
				float inputValue = Input.GetAxis(axisInput.ToString());
				float rawInputValue = Input.GetAxisRaw(axisInput.ToString());
				if (inputValue != 0.0f)
				{
					m_axisInputsRelays[axisInput].Dispatch(axisInput, inputValue);
				}
				if (rawInputValue != 0.0f)
				{
					m_axisRawInputsRelays[axisInput].Dispatch(axisInput, inputValue);
				}
			}

			foreach (KeyValuePair<KEY_INPUT, Relay<KEY_INPUT, EKeyInputEvent>> keyInput in m_keyInputsRelays)
			{
				if (Input.GetButtonDown(keyInput.Key.ToString()))
				{
					keyInput.Value.Dispatch(keyInput.Key, EKeyInputEvent.DOWN);
				}
				if (Input.GetButton(keyInput.Key.ToString()))
				{
					keyInput.Value.Dispatch(keyInput.Key, EKeyInputEvent.PRESSED);
				}
				if (Input.GetButtonUp(keyInput.Key.ToString()))
				{
					keyInput.Value.Dispatch(keyInput.Key, EKeyInputEvent.UP);
				}
			}
		}

		[NonSerialized]
		private Dictionary<AXIS_INPUT, Relay<AXIS_INPUT, float>> m_axisInputsRelays = null;
		[NonSerialized]
		private Dictionary<AXIS_INPUT, Relay<AXIS_INPUT, float>> m_axisRawInputsRelays = null;
		[NonSerialized]
		private Dictionary<KEY_INPUT, Relay<KEY_INPUT, EKeyInputEvent>> m_keyInputsRelays = null;

		[NonSerialized]
		private Array m_axisInputEnumValues = null;
		[NonSerialized]
		private Array m_keyInputEnumValues = null;

		#endregion Private
	}
}