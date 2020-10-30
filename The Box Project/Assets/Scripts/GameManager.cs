using UnityEngine;
using UnityEngine.SceneManagement;
using Yube;

public class GameManager : Singleton<GameManager>
{
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