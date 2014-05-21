using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class ShakeTheTable : MonoBehaviour
{
    public LayerMask LayerMask;
    public KeyCode KeyKickFromLeft;
    public KeyCode KeyKickFromRight;
    public Vector3 Distance = Vector3.left;
    public float Force = 1f;
    public float TimeToShake = 0.1f;
    public float Timeout = 0.1f;

    public AudioClip Sound1;
    public AudioClip Sound2;
    public AudioClip Sound3;
    [Range (0, 1)]
    public float Volume = 1f;
    private AudioSource field_audioSource;

    private Vector3 field_initialPosition;
    private bool field_shaking;
    private bool field_timingout;
    private float field_timeOfShakingStarted;
    private bool field_invertDirection;

	// Use this for initialization
	void Start () {
        field_audioSource = gameObject.AddComponent<AudioSource>();
        field_audioSource.volume = Volume;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeScale < 0.0001f)
			return;

        // Shaking here
	    if (field_shaking)
	    {
            // Do precalculations
            Vector3 actualShift = (field_invertDirection ? -Distance : Distance) * UnityEngine.Time.deltaTime;
	        Camera mainCamera = FindObjectOfType<Camera>();

            // Shake it!
            if (Time.time < field_timeOfShakingStarted + TimeToShake / 2)
                mainCamera.transform.position += actualShift;
	        else
                mainCamera.transform.position -= actualShift;

	        // End shaking and switch to timeout if time is up
            if (Time.time > field_timeOfShakingStarted + TimeToShake)
	        {
                mainCamera.transform.position = field_initialPosition;
	            field_shaking = false;
	            field_timingout = true;
	        }
	    }
        // Timing out after shaking here
        else if (field_timingout)
        {
            if (Time.time > field_timeOfShakingStarted + TimeToShake + Timeout)
            {
                field_timingout = false;
            }
        }
        // Ready to shake, whatch user input
	    else
	    {
	        bool left = Input.GetKey(KeyKickFromLeft);
	        bool right = Input.GetKey(KeyKickFromRight);
	        if (left || right)
            {
                float random = Random.Range(0f, 3f);
                field_audioSource.PlayOneShot(random < 1 ? Sound1 : (random < 2 ? Sound2 : Sound3));

                Camera mainCamera = FindObjectOfType<Camera>();
                field_initialPosition = mainCamera.transform.position;
	            field_shaking = true;
                field_timeOfShakingStarted = UnityEngine.Time.time;
	            field_invertDirection = right;
	        }
	    }
	}

    void FixedUpdate()
    {
        if (field_shaking)
        {
            // Do precalculations
            Vector3 force = (field_invertDirection ? -Distance : Distance).normalized * Force;
            if (UnityEngine.Time.time > field_timeOfShakingStarted + TimeToShake / 2)
                force = -force;

            // Cycle through all objects in the game
            foreach (GameObject gameObject in FindObjectsOfType<GameObject>())
            {
                // Check the layer
                if (((1 << gameObject.layer) & LayerMask) != 0)
                {
                    // That should be a ball, use force, Luke!
                    gameObject.rigidbody.AddForce(force);
                }
            }

        }
    }
}
