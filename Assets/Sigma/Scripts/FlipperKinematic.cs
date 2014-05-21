using System.Collections.Generic;
using UnityEngine;

public class FlipperKinematic : MonoBehaviour
{
    public KeyCode Key = KeyCode.LeftArrow;

    public Vector3 Axis = Vector3.up;
    public float Angle = 90;
    public float Force = 10;

    private Quaternion field_startRotation;
    private Quaternion field_endRotation;
    private bool field_isRotated = false;

    private GameObject field_model;

    private List<GameObject> field_balls = new List<GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Flipper Model")
            {
                field_model = child.gameObject;
                break;
            }
        }

        field_startRotation = field_model.transform.rotation;
        field_model.transform.Rotate(Axis, Angle);
        field_endRotation = field_model.transform.rotation;
        field_model.transform.rotation = field_startRotation;
    }

    void FixedUpdate()
	{
		if (Time.timeScale < 0.0001f)
			return;

        foreach (GameObject ball in field_balls)
        {
            Debug.DrawLine(ball.transform.position, ball.transform.position + Axis * 0.1f, Color.gray);
        }
        Debug.DrawLine(transform.position, transform.position + new Vector3(0.1f, 0, 0), Color.red);
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0.1f, 0), Color.green);
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0, 0.1f), Color.blue);

        bool keyDown = Input.GetKey(Key);
        if (keyDown && !field_isRotated)
        {
            field_isRotated = true;
            field_model.transform.rotation = field_endRotation;

            foreach (GameObject ball in field_balls)
            {
                ball.GetComponent<DontGoThroughThings>().SendMessage("PassNextUpdate");

                Vector3 ballPositionPrevious = ball.rigidbody.transform.position;
                ball.rigidbody.transform.RotateAround(transform.position, Axis, Angle); // Hacking here, actual rotation is done further
                Vector3 ballPositionNext = ball.rigidbody.transform.position;
                RaycastHit hitInfo;
                // Bring the ball to contact the flipper TODO: THIS BLOCK NEVER EXECUTES o_O
                if (Physics.Raycast(ballPositionPrevious, ballPositionNext - ballPositionPrevious, out hitInfo, 1f, LayerMask.NameToLayer("Flippers")))
                {
                    ball.rigidbody.position = hitInfo.point - (ballPositionNext - ballPositionPrevious / 1f);
                    ball.rigidbody.transform.RotateAround(transform.position, Axis, Angle); // Rotate, this time for real
                    Debug.Log("WOW!");
                }
                float actualForce = Force * (ball.rigidbody.transform.position - transform.position).magnitude;
                ball.rigidbody.AddForce(new Vector3(0, 0, 1) * actualForce, ForceMode.VelocityChange);
            }
        }
        else if (!keyDown && field_isRotated)
        {
            field_isRotated = false;
            field_model.transform.rotation = field_startRotation;

            foreach (GameObject ball in field_balls)
            {
                ball.GetComponent<DontGoThroughThings>().SendMessage("PassNextUpdate");

                Vector3 ballPositionPrevious = ball.rigidbody.transform.position;
                ball.rigidbody.transform.RotateAround(transform.position, Axis, -Angle); // Hacking here, actual rotation is done further
                Vector3 ballPositionNext = ball.rigidbody.transform.position;
                RaycastHit hitInfo;
                // Bring the ball to contact the flipper TODO: THIS BLOCK NEVER EXECUTES o_O
                if (Physics.Raycast(ballPositionPrevious, ballPositionNext - ballPositionPrevious, out hitInfo, 1f, LayerMask.NameToLayer("Flippers")))
                {
                    ball.rigidbody.position = hitInfo.point - (ballPositionNext - ballPositionPrevious / 1f);
                    ball.rigidbody.transform.RotateAround(transform.position, Axis, -Angle); // Rotate, this time for real
                    Debug.Log("WOW!");
                }
                //float actualForce = Force * (ball.transform.position - transform.position).magnitude / 10;
                //ball.rigidbody.AddForce(new Vector3(0, 0, -1) * actualForce, ForceMode.VelocityChange);
            }
        }
    }

    public void AddBall(Collider param_collider)
    {
        field_balls.Add(param_collider.gameObject);
        //Debug.Log("Added ball " + param_collider.gameObject.GetInstanceID());
    }
    public void RemoveBall(Collider param_collider)
    {
        for (int i = field_balls.Count - 1; i >= 0; i--)
        {
            if (field_balls[i].GetInstanceID() == param_collider.gameObject.GetInstanceID())
            {
                field_balls.RemoveAt(i);
                //Debug.Log("Removed ball " + param_collider.gameObject.GetInstanceID());
            }
        }
    }
}
