using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	private TrailRenderer field_trailRenderer;
	public float TrailStartLength = 1f;
	public float TrailEndLength = 0.01f;
	public float TrailLengthDecreaseStep = 0.01f;

	private bool field_isElectrified = false;
	public bool IsElectrified { get { return field_isElectrified; } }

	void Start ()
	{
		field_trailRenderer = GetComponent<TrailRenderer>();
	}

	void Update()
	{
		;
	}

	public void Electrify()
	{
		StopCoroutine("ElectrifiedTrailCoroutine");
		StartCoroutine("ElectrifiedTrailCoroutine");
	}
	public IEnumerator ElectrifiedTrailCoroutine()
	{
		field_isElectrified = true;
		field_trailRenderer.enabled = true;
		float initialTimeValue = field_trailRenderer.time;
		while (field_trailRenderer.time < TrailStartLength + initialTimeValue)
		{
			yield return new WaitForEndOfFrame();
			field_trailRenderer.time += Time.deltaTime;
		}
		field_trailRenderer.time = TrailStartLength + initialTimeValue;
		while (field_trailRenderer.time > TrailEndLength)
		{
			yield return new WaitForEndOfFrame();
			field_trailRenderer.time -= TrailLengthDecreaseStep;
		}
		field_trailRenderer.time = 0f;
		field_trailRenderer.enabled = false;
		field_isElectrified = false;
	}
}
