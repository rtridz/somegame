using UnityEngine;
using System.Collections;

public class star_script : MonoBehaviour {

	public float freq = 0.1f; // Частотота мигания
	public Color FirstColor = Color.red;
	public Color SecondColor = Color.blue;
	public KeyCode Key = KeyCode.Y;

	private bool cur = false;

	// Use this for initialization
	void Start () {
		renderer.material.color = FirstColor;
	}
	
	void Update()
	{

	}

	void OnTriggerEnter(Collider obj)
	{
		if (obj.gameObject.layer == LayerMask.NameToLayer("Balls")) {
			renderer.material.color = SecondColor;
		}

	}

	void OnTriggerExit()
	{
		try
		{
			StopCoroutine("coroutine");
		}
		catch{}
		
		StartCoroutine("coroutine");

	}

	IEnumerator coroutine()
	{
		var i = 0;
		while(renderer.material.color != FirstColor)
		{
			renderer.material.color = Color.Lerp(renderer.material.color, FirstColor, freq * Time.deltaTime);
			
			yield return null;
		}
	}

}
