using UnityEngine;

public class SwitchLevel : MonoBehaviour
{
	[SerializeField]
	private int m_sceneIndex = 0;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			GameManager.Instance.LoadScene(m_sceneIndex);
	}
}