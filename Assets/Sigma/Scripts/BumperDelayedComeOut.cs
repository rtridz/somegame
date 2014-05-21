using UnityEngine;
using System.Collections;

public class BumperDelayedComeOut : MonoBehaviour
{

    public float Delay = 1f;
    public float Shift = 0.1f;
    public float Step = 0.001f;
    private float field_initialPositionY;

	// Use this for initialization
	void Start ()
	{
        field_initialPositionY = this.transform.position.y;
        // Shift down
        this.transform.Translate(0, -Shift, 0);
        // Deactivate the bumper
        GetComponent<Bumper>().SendMessage("Deactivate");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if (Time.time > Delay)
        {
            // Shift a bit up
            this.transform.Translate(0, Step, 0);
            // Check if we are done
            if (this.transform.position.y >= field_initialPositionY)
            {
                // Shift precisly to initial position
                this.transform.transform.position.Set(this.transform.position.x, field_initialPositionY, this.transform.position.z);
                // Activate the bumper
                GetComponent<Bumper>().SendMessage("Activate");
                // Remove this script as it is not needed anymore
                Destroy(this);
	        }
        }
	}
}
