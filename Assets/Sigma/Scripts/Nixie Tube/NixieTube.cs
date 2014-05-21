using UnityEngine;
using System.Collections;

public class NixieTube : MonoBehaviour {

    public Texture Zero;
    public Texture One;
    public Texture Two;
    public Texture Three;
    public Texture Four;
    public Texture Five;
    public Texture Six;
    public Texture Seven;
    public Texture Eight;
    public Texture Nine;

    private Renderer field_indicatorRenderer;

	// Use this for initialization
	void Start () {
        
        foreach (Transform child in transform)
        {
            if (child.name == "Box001")
            {
                field_indicatorRenderer = child.renderer;
                break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        //SetValue(Random.Range(1, 10));
	}

    void SetValue(int param_value)
    {
        Texture texture = null;
        switch (param_value)
        {
            case 0:
                texture = Zero;
                break;
            case 1:
                texture = One;
                break;
            case 2:
                texture = Two;
                break;
            case 3:
                texture = Three;
                break;
            case 4:
                texture = Four;
                break;
            case 5:
                texture = Five;
                break;
            case 6:
                texture = Six;
                break;
            case 7:
                texture = Seven;
                break;
            case 8:
                texture = Eight;
                break;
            case 9:
                texture = Nine;
                break;
            default:
                texture = Zero;
                break;
        }
        field_indicatorRenderer.renderer.material.mainTexture = texture;
    }
}
