using UnityEngine;
using System.Collections;

public class PrefabGenerator : MonoBehaviour {

    public KeyCode Key = KeyCode.Tab;
    public GameObject Prefab;
    public Vector3 Position = Vector3.zero;
    public Quaternion Rotation = Quaternion.identity;
    public float Timeout = 0f;

    private float field_lastTime;

	// Use this for initialization
	void Start () {
        field_lastTime = -Timeout;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(Key) && Time.time > field_lastTime + Timeout)
        {
            field_lastTime = Time.time;
            GameObject prefab = Instantiate(Prefab, Position, Rotation) as GameObject;
            prefab.transform.parent = transform;
        }
	}
}
