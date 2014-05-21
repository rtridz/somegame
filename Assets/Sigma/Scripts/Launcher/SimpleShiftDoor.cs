using UnityEngine;
using System.Collections;

public class SimpleShiftDoor : MonoBehaviour
{

    public Vector3 Shift = Vector3.up;
    public float Time = 1f;

    private Vector3 field_initialPosition;
    //private float field_initialTime;
    private bool field_opening = false;
    private bool field_closing = false;

    // Use this for initialization
    void Start()
    {
        field_initialPosition = transform.position;
    }

    void Close()
    {
        if (field_closing)
            return;

        field_opening = false;
        field_closing = true;
        //field_initialTime = UnityEngine.TimeToShake.time;
        //transform.position = field_initialPosition + Shift;
    }

    void Open()
    {
        if (field_opening)
            return;

        field_opening = true;
        field_closing = false;
        //field_initialTime = UnityEngine.TimeToShake.time;
        //transform.position = field_initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!field_opening && !field_closing)
            return;

        transform.position += (field_opening ? -Shift : Shift) * UnityEngine.Time.deltaTime / Time;
        
        if (field_opening && Vector3.Dot(transform.position - field_initialPosition, Shift) < 0)
        {
            field_opening = false;
            transform.position = field_initialPosition;
        }
        if (field_closing && Vector3.Dot(transform.position - (field_initialPosition + Shift), Shift) > 0)
        {
            field_closing = false;
            transform.position = field_initialPosition + Shift;
        }
    }
}
