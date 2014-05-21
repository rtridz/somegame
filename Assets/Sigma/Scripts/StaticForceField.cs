using UnityEngine;
using System.Collections;

public class StaticForceField : MonoBehaviour {

    public Vector3 Direction;
    public float Force;
    public LayerMask LayerMask;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider param_collider)
    {
        // If not on the layer we need - return
        if (((1 << param_collider.gameObject.layer) & LayerMask) == 0)
            return;

        param_collider.rigidbody.AddForce(Direction.normalized * Force, ForceMode.Force);
    }
}
