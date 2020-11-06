using UnityEngine;

public class CharacterCrown : MonoBehaviour
{
	#region Private

	private void Start()
	{
		InputManager.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, true);
		m_fxMaterial = m_fxRenderer.sharedMaterial;
	}

	private void Update()
	{
		if (m_player != null)
		{
			m_parent.eulerAngles = new Vector3(0.0f, 0.0f,
				m_player.FaceRight ? m_zRotation : -1 * m_zRotation);
		}
	}

	private void OnDestroy()
	{
		InputManager.Instance?.RegisterOnColorButtonPressed(OnColorButtonPressed, false);
	}

	private void OnColorButtonPressed(Platform.Color color)
	{
		ParticleSystem.MainModule mainFire = m_fireFx.main;
		ParticleSystem.MainModule mainBurst = m_burstFx.main;

		switch (color)
		{
			case Platform.Color.BLUE:
				mainFire.startColor = m_blueParticle;
				mainBurst.startColor = m_blueParticle;
				m_fxMaterial.SetColor("_Color", m_blueMat);
				break;

			case Platform.Color.RED:
				mainFire.startColor = m_redParticle;
				mainBurst.startColor = m_redParticle;
				m_fxMaterial.SetColor("_Color", m_redMat);
				break;

			case Platform.Color.YELLOW:
				mainFire.startColor = m_yellowParticle;
				mainBurst.startColor = m_yellowParticle;
				m_fxMaterial.SetColor("_Color", m_yellowMat);
				break;

			default:
				break;
		}
		m_burstFx.Play();
	}

	[Header("Particles Systems")]
	[SerializeField]
	private Renderer m_fxRenderer = null;
	[SerializeField]
	private ParticleSystem m_fireFx = null;
	[SerializeField]
	private ParticleSystem m_burstFx = null;

	[Header("Particles Color")]
	[SerializeField]
	private Color m_blueParticle = Color.blue;
	[SerializeField]
	private Color m_redParticle = Color.red;
	[SerializeField]
	private Color m_yellowParticle = Color.yellow;

	[Header("Material Colors")]
	[SerializeField]
	[ColorUsageAttribute(true, true)]
	private Color m_blueMat = Color.blue;
	[SerializeField]
	[ColorUsageAttribute(true, true)]
	private Color m_redMat = Color.red;
	[SerializeField]
	[ColorUsageAttribute(true, true)]
	private Color m_yellowMat = Color.yellow;

	[Header("Other Settings")]
	[SerializeField]
	private Transform m_parent = null;
	[SerializeField]
	private float m_zRotation = 0.0f;
	[SerializeField]
	private CharacterController m_player = null;

	private Material m_fxMaterial = null;
	private const string COLOR_FIELD_MATERIAL = "_Color";

	#endregion Private
}