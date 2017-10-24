using UnityEngine;
using System.Collections;

public class DebugRenderOptions : MonoBehaviour {
  /*
	public GameObject m_EnemyFovRenderer = null;
	private bool m_EnemyFovVisible = false;
	private MeshRenderer[] m_NoiseSources = null;
	private bool m_NoisesVisible = false;
	private MeshRenderer[] m_Waypoints = null;
	private bool m_WaypointsVisible = false;

	public GameObject m_FlashLight = null;
	private bool m_FlashLightDefault = false;
	private Color m_FlashLightColor;
	private LightRenderMode m_FlashLightRenderMode;

	// Use this for initialization
	void Start () {
		GameObject[] noises = GameObject.FindGameObjectsWithTag(StringManager.Tags.noise);
		m_NoiseSources = new MeshRenderer[noises.Length];
		for (int i = 0; i < noises.Length; i++)
		{
			m_NoiseSources[i] = noises[i].GetComponent<MeshRenderer>();
		}

		GameObject[] waypoints = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
		m_Waypoints = new MeshRenderer[waypoints.Length];
		for (int i = 0; i < waypoints.Length; i++)
		{
			m_Waypoints[i] = waypoints[i].GetComponent<MeshRenderer>();
		}

		m_FlashLightColor = m_FlashLight.GetComponent<Light>().color;
		m_FlashLightRenderMode = m_FlashLight.GetComponent<Light>().renderMode;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F9))
		{
			m_NoisesVisible = !m_NoisesVisible;
			RendererDisplayChange(m_NoiseSources, m_NoisesVisible);
        }

		if (Input.GetKeyDown(KeyCode.F10))
		{
			m_WaypointsVisible = !m_WaypointsVisible;
			RendererDisplayChange(m_Waypoints, m_WaypointsVisible);
		}

		if (Input.GetKeyDown(KeyCode.F11))
		{
			m_EnemyFovVisible = !m_EnemyFovVisible;
			EnemyFovRendererChange(m_EnemyFovVisible);
        }

		if (Input.GetKeyDown(KeyCode.F12))
		{
			m_FlashLightDefault = !m_FlashLightDefault;
			ChangeFlashLight(m_FlashLightDefault);
		}
	}

	void ChangeFlashLight(bool change)
	{
		Light light = m_FlashLight.GetComponent<Light>();
		if (change)
		{
			// Sebastian
			light.range = 40.0f;
			light.spotAngle = 80.0f;
			light.color = new Color(0.8f, 0.8f, 0.4f, 1.0f);
            light.intensity = 2.0f;
			light.shadows = LightShadows.None;
			light.renderMode = m_FlashLightRenderMode;
        }
		else
		{
			// Alexander
			light.range = 4.5f;
			light.spotAngle = 87.0f;
			light.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			light.intensity = 4.91f;

			light.shadows = LightShadows.Hard;
			light.shadowStrength = 1.0f;
			light.shadowBias = 0.0f;
			light.shadowNormalBias = 0.4f;
			light.shadowNearPlane = 0.2f;

			light.renderMode = LightRenderMode.Auto;
		}
	}

	void EnemyFovRendererChange(bool change)
	{
		m_EnemyFovRenderer.GetComponent<MeshRenderer>().enabled = change;
    }

	void RendererDisplayChange(MeshRenderer[] renderers, bool change)
	{
		int length = renderers.Length;
		for (int i = 0; i < length; i++)
		{
			renderers[i].enabled = change;
		}
	}
  */
}
