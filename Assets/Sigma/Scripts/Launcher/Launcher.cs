using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{

    public KeyCode Key = KeyCode.Space;
    public Vector3 Shift = Vector3.up;
    public float ForceForward = 1f;
    public float ForceBack = 0.05f;

    public GameObject Door;

    public AudioClip Sound;
    [Range(0, 1)]
    public float Volume = 1f;
    private AudioSource field_audioSource;

    //public float 
    private bool field_launching = false;
    private bool field_fallingBack = false;
    private Vector3 field_initialPosition;
    private Vector3 field_topPosition;
    private bool field_armed = false;



    // Use this for initialization
    void Start()
    {
        field_initialPosition = rigidbody.transform.position;

        field_audioSource = gameObject.AddComponent<AudioSource>();
        field_audioSource.volume = Volume;
    }

    // Update is called once per frame
    void Update()
	{
		if (Time.timeScale < 0.0001f)
			return;

        if (!field_launching && !field_fallingBack && field_armed && Input.GetKeyDown(Key))
        {
            /*Debug.Log(field_launching);
            Debug.Log(field_fallingBack);
            Debug.Log(field_armed);*/
            field_launching = true;
            field_armed = false;

            field_audioSource.PlayOneShot(Sound);
        }
    }

    void Arm()
    {
        if (field_launching || field_fallingBack)
            return;

        field_armed = true;
        Door.SendMessage("Close");
    }

    void FixedUpdate()
    {
        /*Debug.Log(-Shift.normalized * Force);
        Debug.Log(field_launching);
        Debug.Log(field_fallingBack);
        Debug.Log(field_armed);
        Debug.Log((transform.position - field_topPosition).sqrMagnitude);
        Debug.Log(Shift.magnitude);
        Debug.Log(TimeToShake.time);*/
        /*if (!field_armed && !field_launching && Trigger.Triggered)
        {
            Trigger.Triggered = false;
        }*/
        if (field_launching)
        {
            if ((transform.position - field_initialPosition).magnitude > Shift.magnitude)
            {
                rigidbody.transform.position = field_initialPosition + Shift;
                field_topPosition = transform.position;
                field_launching = false;
                field_fallingBack = true;
                rigidbody.velocity = Vector3.zero;
                rigidbody.sleepVelocity = 0f;
                rigidbody.sleepAngularVelocity = 0f;
            }
            else
            {
                rigidbody.AddForce(Shift.normalized * ForceForward, ForceMode.VelocityChange);
            }
        }
        else if (field_fallingBack)
        {
            if ((transform.position - field_topPosition).magnitude > Shift.magnitude)
            {
                transform.position = field_initialPosition;
                field_fallingBack = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.sleepVelocity = 0f;
                rigidbody.sleepAngularVelocity = 0f;
                Door.SendMessage("Open");
            }
            else
            {
                collider.rigidbody.AddForce(Shift.normalized * -ForceBack, ForceMode.VelocityChange);
            }
        }
    }
}
