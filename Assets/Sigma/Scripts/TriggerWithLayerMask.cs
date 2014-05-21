using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class TriggerWithLayerMask : MonoBehaviour
{
    public LayerMask LayerMask;
    public GameObject GameObject;
    public string CallbackOnEnter;
    public string CallbackOnExit;
	private Dictionary<int, GameObject> field_gameObjectsByInstanceId = new Dictionary<int, GameObject>();
	private bool field_gameObjectsChanged = false;
	private GameObject[] field_gameObjects = new GameObject[0];
	public GameObject[] GameObjects
	{
		get
		{
			if (field_gameObjectsChanged)
			{
				field_gameObjects = field_gameObjectsByInstanceId.Values.ToArray();
				field_gameObjectsChanged = false;
			}
			return field_gameObjects;
		}
	}

    void OnTriggerEnter(Collider param_collider)
    {
		int id = param_collider.gameObject.GetInstanceID();
		if ((LayerMask == 0 || ((1 << param_collider.gameObject.layer) & LayerMask) != 0) && !field_gameObjectsByInstanceId.ContainsKey(id))
		{
			if (CallbackOnEnter != "")
			{
				GameObject.SendMessage(CallbackOnEnter, param_collider, SendMessageOptions.RequireReceiver);
			}
			field_gameObjectsByInstanceId.Add(id, param_collider.gameObject);
			field_gameObjectsChanged = true;
		}
    }
    void OnTriggerExit(Collider param_collider)
	{
		int id = param_collider.gameObject.GetInstanceID();
		if ((LayerMask == 0 || ((1 << param_collider.gameObject.layer) & LayerMask) != 0) && field_gameObjectsByInstanceId.ContainsKey(id))
		{
			if (CallbackOnExit != "")
			{
				GameObject.SendMessage(CallbackOnExit, param_collider, SendMessageOptions.RequireReceiver);
			}
			field_gameObjectsByInstanceId.Remove(id);
			field_gameObjectsChanged = true;
		}
    }
}
