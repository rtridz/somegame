using UnityEngine;
using System.Collections;

public class GUIButton : MonoBehaviour
{
	public GameObject GameObject;
	public string CallbackOnClick;
	public KeyCode Key = KeyCode.None;

	public GameObject Arrow;

	void Start ()
	{
		;
	}
	
	void Update ()
	{
		if (Key != KeyCode.None && Input.GetKeyDown(Key))
			OnClick();
	}

	void OnClick()
	{
		GameObject.SendMessage(CallbackOnClick, SendMessageOptions.RequireReceiver);
	}

	void OnHover(bool param_isOver)
	{
		//Arrow.SetActive(param_isOver);
		if (param_isOver)
		{
			Arrow.GetComponent<TweenAlpha>().enabled = true;
			Arrow.GetComponent<TweenAlpha>().PlayForward();
		}
		else
		{
			Arrow.GetComponent<TweenAlpha>().PlayReverse();
		}
	}
}
