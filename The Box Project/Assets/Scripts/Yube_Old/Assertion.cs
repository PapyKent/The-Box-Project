using System.Diagnostics;
using UnityEngine;

namespace Yube
{
	public class Assertion
	{
		[Conditional("UNITY_EDITOR")]
		public static void Check(bool condition, string errorMessage = null, Object context = null)
		{
			if (!condition)
			{
				UnityEngine.Debug.LogError($"ASSERTION!: {errorMessage}", context);
			}
		}
	}
}