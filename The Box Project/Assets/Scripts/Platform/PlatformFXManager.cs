using UnityEngine;

public class PlatformFXManager : MonoBehaviour
{
	public static PlatformFXManager Instance { get; private set; } = null;

	public PlatformFXConfig GetPlatformFXConfig(GridManager.Color color)
	{
		switch (color)
		{
			case GridManager.Color.RED:
				return m_redPlatformConfig;

			case GridManager.Color.YELLOW:
				return m_yellowPlatformConfig;

			case GridManager.Color.BLUE:
				return m_bluePlatformConfig;
		}
		return null;
	}

	#region Private

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning("PlatformFXManager already instanced. Destroying new one.");
		}
		else
		{
			Instance = this;
		}
	}

	[SerializeField]
	private PlatformFXConfig m_redPlatformConfig = null;
	[SerializeField]
	private PlatformFXConfig m_bluePlatformConfig = null;
	[SerializeField]
	private PlatformFXConfig m_yellowPlatformConfig = null;

	#endregion Private
}