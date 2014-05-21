using System;
using UnityEngine;
using System.Collections;

public class CameraPostEffects : MonoBehaviour
{
	public Shader Shader;

	public bool ColorCorrectionOn = false;
	public Color ColorCorrectionRed = Color.red;
	public Color ColorCorrectionGreen = Color.green;
	public Color ColorCorrectionBlue = Color.blue;
	[Range(0, 1f)]
	public float ColorCorrectionAmount = 0;

	[Range(0, 1f)]
	public float ColorBurn = 0;
	[Range(0, 10f)]
	public float Contrast = 0;
	public bool BlurOn = false;
	[Range(0, 1f)]
	public float DepthOfFieldBlur = 0;
	public float DepthOfFieldBlurNearPlane = 0;
	public float DepthOfFieldBlurMiddlePlane = 0.5f;
	public float DepthOfFieldBlurFarPlane = 1;

	private Material field_material;

	void Start ()
	{
		field_material = new Material(Shader);
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
	}
	
	void Update ()
	{
	}
	
	void OnRenderImage(RenderTexture param_renderTextureSource, RenderTexture param_renderTextureDestination)
	{
		Color red = ColorCorrectionOn ? ColorCorrectionRed : Color.red;
		Color green = ColorCorrectionOn ? ColorCorrectionGreen : Color.green;
		Color blue = ColorCorrectionOn ? ColorCorrectionBlue : Color.blue;
		field_material.SetColor("_CorrectionRedColor", red);
		field_material.SetColor("_CorrectionGreenColor", green);
		field_material.SetColor("_CorrectionBlueColor", blue);
		field_material.SetFloat("_CorrectionRGBAmount", ColorCorrectionAmount);
		field_material.SetFloat("_CorrectionColorBurn", ColorBurn);
		field_material.SetFloat("_CorrectionContrast", Contrast);
		field_material.SetFloat("_CorrectionDofBlur", BlurOn ? DepthOfFieldBlur : 0);
		field_material.SetFloat("_CorrectionDofBlurNearPlane", DepthOfFieldBlurNearPlane);
		field_material.SetFloat("_CorrectionDofBlurMiddlePlane", DepthOfFieldBlurMiddlePlane);
		field_material.SetFloat("_CorrectionDofBlurFarPlane", DepthOfFieldBlurFarPlane);
		Graphics.Blit(param_renderTextureSource, param_renderTextureDestination, field_material);
	}

	/*void OnGUI()
	{

		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, Color.black);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(new Rect(10, 10, 256, 28), "Controls: A,D,Q,E,LeftShift,Spacebar,Tab");
	}*/
}
