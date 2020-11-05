using UnityEngine;
using UnityEngine.SceneManagement;
using Yube;

public class GameManager : Singleton<GameManager>
{
	public GameConstants GameConstants { get { return m_gameConstants; } }

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

	#region Private

	[SerializeField]
	private GameConstants m_gameConstants = null;

	#endregion Private
}