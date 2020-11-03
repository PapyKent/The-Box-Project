using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static bool IsInferior(float value, float limit, bool withEpsilon)
	{
		if (withEpsilon)
		{
			return value - Mathf.Epsilon < limit || value + Mathf.Epsilon < limit;
		}
		else
		{
			return value < limit;
		}
	}

	public static bool IsSuperior(float value, float limit, bool withEpsilon)
	{
		if (withEpsilon)
		{
			return value - Mathf.Epsilon > limit || value + Mathf.Epsilon > limit;
		}
		else
		{
			return value > limit;
		}
	}
}