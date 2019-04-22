using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region Static Instance

	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
				if (instance == null)
				{
					Instance = new GameObject("Spawned GameManager", typeof(GameManager)).GetComponent<GameManager>();
				}
			}
			return instance;
		}
		private set
		{
			instance = value;
			DontDestroyOnLoad(value);
		}
	}

	#endregion Static Instance

	private void Start()
	{
		AudioManager.Instance.PlayMusic();
	}

	public void LoadScene(Scene scene)
	{
		SceneManager.LoadScene(scene.buildIndex);
	}

	public void LoadScene(int buildIndex)
	{
		SceneManager.LoadScene(buildIndex);
	}
}