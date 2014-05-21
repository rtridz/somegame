using System;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public class Bumper : MonoBehaviour
{

    public float Force = 1f;
    public float Radius = 0.1f;
    public AudioClip SoundOfKickOne;
    public AudioClip SoundOfKickTwo;
    public AudioClip SoundOfNoKick;
	public TriggerWithLayerMask TriggerUsedAsBallsContainer;

    private Random field_random;
    private bool field_isActivated = true;
    private Animator field_animator;
    private AudioSource field_audioSource;

	// Use this for initialization
	void Start ()
	{
        field_random = new Random();
        field_animator = GetComponent<Animator>();
	    field_audioSource = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void Deactivate()
    {
        field_isActivated = false;
    }

    void Activate()
    {
        field_isActivated = true;
    }

    public bool IsActive()
    {
        return field_isActivated && !field_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    void OnTriggerEnter(Collider param_collider)
    {
        // If not active
        if (!field_isActivated)
            return;

        // If not ready
        if (!field_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))//.GetBool("Recharging"))
            return;

        // If not a ball
        if (param_collider.gameObject.layer != LayerMask.NameToLayer("Balls"))
            return;

        field_audioSource.PlayOneShot(field_random.Next(0, 100) > 50 ? SoundOfKickOne : SoundOfKickTwo);
        field_animator.SetTrigger("Activate");

        int layer = LayerMask.NameToLayer("Balls");
		GameObject[] balls = TriggerUsedAsBallsContainer.GameObjects;
		foreach (GameObject gameObject in balls)
        {
            // Check the layer
            if (gameObject.layer != layer)
                continue;

            if ((gameObject.transform.position - transform.position).magnitude > Radius)
                continue;

            var direction = gameObject.collider.transform.position - transform.position;
            direction.Normalize();
            direction *= Force;
            gameObject.collider.rigidbody.AddForce(direction, ForceMode.VelocityChange);
        }

    }
}
