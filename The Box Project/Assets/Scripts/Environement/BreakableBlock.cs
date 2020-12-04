using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yube;

public class BreakableBlock : MonoBehaviour
{
	public void Break()
	{
		ResourceManager.Instance.ReleaseInstance(this);
	}
}