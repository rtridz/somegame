using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	public AudioClip Sound;
	private AudioSource field_audioSource;

	public ParticleSystem ParticleSystem;

    private bool field_isActivated = false;

    private Light field_light;

	// Use this for initialization
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Light")
            {
                field_light = child.gameObject.GetComponent<Light>();
                break;
            }
		}
		field_audioSource = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (field_isActivated)
        {
            //Debug.Log("!");
            field_light.enabled = true;
        }
        else
        {
            field_light.enabled = false;
        }
    }

    public void Deactivate()
	{
		if (!field_isActivated)
			return;

        field_isActivated = false;
		//field_audioSource.Stop();
		//ParticleSystem.Stop();
    }

    public void Activate(Collider param_collider)
    {
		if (field_isActivated)
			return;

		field_isActivated = true;
		field_audioSource.PlayOneShot(Sound);
		ParticleSystem.Play();
		param_collider.gameObject.GetComponent<Ball>().Electrify();
    }

    public bool IsActive()
    {
        return field_isActivated;
    }

}
