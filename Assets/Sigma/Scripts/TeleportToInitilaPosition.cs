using UnityEngine;
using System.Collections;

public class TeleportToInitilaPosition : MonoBehaviour
{

    public KeyCode Key = KeyCode.Tab;
    private Vector3 field_initialPosition = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
        field_initialPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(Key))
	    {
            this.transform.position = field_initialPosition;
	    }
	}
}
