using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yube
{
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		public static T Instance { get { return s_instance; } }

		#region Private

		protected virtual void Awake()
		{
			if (s_instance == null)
			{
				s_instance = (T)this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Debug.LogError($"Second instance of type {typeof(T).ToString()} found. Destroying its gameobject.", this);
				Destroy(gameObject);
			}
		}

		private static T s_instance = null;

		#endregion Private
	}
}