using UnityEngine;

public class MovingPlatformTrigger : MonoBehaviour
{
	#region Private

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Trigger enter");
		if (collision.tag == "Player")
		{
			m_movingPlatform.SetCurrentPlayer(collision.GetComponentInParent<CharacterController>());
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Debug.Log("Trigger exit");
		if (collision.tag == "Player")
		{
			m_movingPlatform.SetCurrentPlayer(false);
		}
	}

	[SerializeField]
	private MovingPlatform m_movingPlatform = null;

	#endregion Private
}