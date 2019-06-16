using UnityEngine;

public class CharacterCrown : MonoBehaviour
{
	#region Private

	private void Start()
	{
		InputManager.Instance.RegisterOnColorButtonPressed(OnColorButtonPressed, true);
	}

	private void Update()
	{
		if (CharacterController.Instance != null)
		{
			m_parent.eulerAngles = new Vector3(0.0f, 0.0f,
				CharacterController.Instance.FaceRight ? m_zRotation : -1 * m_zRotation);
		}
	}

	private void OnDestroy()
	{
		InputManager.Instance?.RegisterOnColorButtonPressed(OnColorButtonPressed, false);
	}

	private void OnColorButtonPressed(GridManager.Color color)
	{
		ParticleSystem.MainModule mainFire = m_fireFx.main;
		ParticleSystem.MainModule mainBurst = m_burstFx.main;

		switch (color)
		{
			case GridManager.Color.BLUE:
				mainFire.startColor = m_blueParticle;
				mainBurst.startColor = m_blueParticle;
				m_fxMaterial.SetColor("_Color", m_blueMat);
				break;

			case GridManager.Color.RED:
				mainFire.startColor = m_redParticle;
				mainBurst.startColor = m_redParticle;
				m_fxMaterial.SetColor("_Color", m_redMat);
				break;

			case GridManager.Color.YELLOW:
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
	private Material m_fxMaterial;
	[SerializeField]
	private ParticleSystem m_fireFx;
	[SerializeField]
	private ParticleSystem m_burstFx;

	[Header("Particles Color")]
	[SerializeField]
	private Color m_blueParticle;
	[SerializeField]
	private Color m_redParticle;
	[SerializeField]
	private Color m_yellowParticle;

	[Header("Material Colors")]
	[SerializeField]
	[ColorUsageAttribute(true, true)]
	private Color m_blueMat;
	[SerializeField]
	[ColorUsageAttribute(true, true)]
	private Color m_redMat;
	[SerializeField]
	[ColorUsageAttribute(true, true)]
	private Color m_yellowMat;

	[Header("Other Settings")]
	[SerializeField]
	private Transform m_parent = null;
	[SerializeField]
	private float m_zRotation = 0.0f;

	private const string COLOR_FIELD_MATERIAL = "_Color";

	#endregion Private
}