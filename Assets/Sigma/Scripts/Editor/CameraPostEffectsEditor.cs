using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(CameraPostEffects))]
public class CameraPostEffectsEditor : Editor
{
	private SerializedProperty field_propertyShader;
	private SerializedProperty field_propertyColorCorrectionOn;
	private SerializedProperty field_propertyColorCorrectionRed;
	private SerializedProperty field_propertyColorCorrectionGreen;
	private SerializedProperty field_propertyColorCorrectionBlue;
	private SerializedProperty field_propertyColorCorrectionAmount;
	private SerializedProperty field_propertyBlurOn;
	private SerializedProperty field_propertyDepthOfFieldBlur;
	private SerializedProperty field_propertyDepthOfFieldBlurNearPlane;
	private SerializedProperty field_propertyDepthOfFieldBlurMiddlePlane;
	private SerializedProperty field_propertyDepthOfFieldBlurFarPlane;
	private SerializedProperty field_propertyColorBurn;
	private SerializedProperty field_propertyContrast;

	void Start()
	{
		;
	}

	void Update()
	{
		;
	}

	void OnEnable()
	{
		field_propertyShader = serializedObject.FindProperty("Shader");
		field_propertyColorCorrectionOn = serializedObject.FindProperty("ColorCorrectionOn");
		field_propertyColorCorrectionRed = serializedObject.FindProperty("ColorCorrectionRed");
		field_propertyColorCorrectionGreen = serializedObject.FindProperty("ColorCorrectionGreen");
		field_propertyColorCorrectionBlue = serializedObject.FindProperty("ColorCorrectionBlue");
		field_propertyColorCorrectionAmount = serializedObject.FindProperty("ColorCorrectionAmount");
		field_propertyBlurOn = serializedObject.FindProperty("BlurOn");
		field_propertyDepthOfFieldBlur = serializedObject.FindProperty("DepthOfFieldBlur");
		field_propertyDepthOfFieldBlurNearPlane = serializedObject.FindProperty("DepthOfFieldBlurNearPlane");
		field_propertyDepthOfFieldBlurMiddlePlane = serializedObject.FindProperty("DepthOfFieldBlurMiddlePlane");
		field_propertyDepthOfFieldBlurFarPlane = serializedObject.FindProperty("DepthOfFieldBlurFarPlane");
		field_propertyColorBurn = serializedObject.FindProperty("ColorBurn");
		field_propertyContrast = serializedObject.FindProperty("Contrast");
	}
	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();

		//int count = propertyCount.intValue;
		//EditorGUI.Vector3Field(new Rect(0,0,100,10), "vector3", new Vector3(1,2,3));
		EditorGUILayout.PropertyField(field_propertyShader);
		EditorGUILayout.PropertyField(field_propertyColorCorrectionOn, new GUIContent() { text = "Color correction" });
		if (field_propertyColorCorrectionOn.boolValue)
		{
			EditorGUI.indentLevel += 1;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.ColorField("Red", field_propertyColorCorrectionRed.colorValue);
			EditorGUILayout.ColorField("Green", field_propertyColorCorrectionGreen.colorValue);
			EditorGUILayout.ColorField("Blue", field_propertyColorCorrectionBlue.colorValue);
			EditorGUILayout.EndVertical();
			if (GUILayout.Button("Normalize colors", GUILayout.Height(48)))
			{
				field_propertyColorCorrectionRed.colorValue = Normalize(field_propertyColorCorrectionRed.colorValue);
				field_propertyColorCorrectionGreen.colorValue = Normalize(field_propertyColorCorrectionGreen.colorValue);
				field_propertyColorCorrectionBlue.colorValue = Normalize(field_propertyColorCorrectionBlue.colorValue);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.PropertyField(field_propertyColorCorrectionAmount, new GUIContent() { text = "Amount" });
			EditorGUILayout.PropertyField(field_propertyColorBurn);
			EditorGUILayout.PropertyField(field_propertyContrast);
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel -= 1;
		}
		EditorGUILayout.PropertyField(field_propertyBlurOn, new GUIContent() { text = "Blur" });
		if (field_propertyBlurOn.boolValue)
		{
			EditorGUI.indentLevel += 1;
			EditorGUILayout.PropertyField(field_propertyDepthOfFieldBlur, new GUIContent() { text = "Amount" });

			GUILayoutOption[] miniButton = { GUILayout.Width(20f) };
			GUILayoutOption[] miniButtonHigh = { GUILayout.Width(20f), GUILayout.Height(48) };

			EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginVertical();
					EditorGUILayout.PropertyField(field_propertyDepthOfFieldBlurNearPlane, new GUIContent() { text = "Near plane" });
					EditorGUILayout.PropertyField(field_propertyDepthOfFieldBlurMiddlePlane, new GUIContent() { text = "Middle plane" });
					EditorGUILayout.PropertyField(field_propertyDepthOfFieldBlurFarPlane, new GUIContent() { text = "Far plane" });
				EditorGUILayout.EndVertical();
				if (GUILayout.Button(new GUIContent("-", "closer"), EditorStyles.miniButtonLeft, miniButtonHigh))
				{
					field_propertyDepthOfFieldBlurNearPlane.floatValue /= 2;
					field_propertyDepthOfFieldBlurMiddlePlane.floatValue /= 2;
					field_propertyDepthOfFieldBlurFarPlane.floatValue /= 2;
				}
				EditorGUILayout.BeginVertical(GUILayout.Width(40f));
					EditorGUILayout.BeginHorizontal();
						if (GUILayout.Button(new GUIContent("-", "closer"), EditorStyles.miniButtonLeft, miniButton))
						{
							field_propertyDepthOfFieldBlurNearPlane.floatValue = field_propertyDepthOfFieldBlurNearPlane.floatValue - (field_propertyDepthOfFieldBlurMiddlePlane.floatValue - field_propertyDepthOfFieldBlurNearPlane.floatValue);
						}
						if (GUILayout.Button(new GUIContent("+", "further"), EditorStyles.miniButtonRight, miniButton))
						{
							field_propertyDepthOfFieldBlurNearPlane.floatValue = field_propertyDepthOfFieldBlurNearPlane.floatValue + (field_propertyDepthOfFieldBlurMiddlePlane.floatValue - field_propertyDepthOfFieldBlurNearPlane.floatValue) / 2;
						}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
						if (GUILayout.Button(new GUIContent("-", "closer"), EditorStyles.miniButtonLeft, miniButton))
						{
							field_propertyDepthOfFieldBlurMiddlePlane.floatValue = field_propertyDepthOfFieldBlurMiddlePlane.floatValue - (field_propertyDepthOfFieldBlurMiddlePlane.floatValue - field_propertyDepthOfFieldBlurNearPlane.floatValue) / 2;
						}
						if (GUILayout.Button(new GUIContent("+", "further"), EditorStyles.miniButtonRight, miniButton))
						{
							field_propertyDepthOfFieldBlurMiddlePlane.floatValue = field_propertyDepthOfFieldBlurMiddlePlane.floatValue + (field_propertyDepthOfFieldBlurFarPlane.floatValue - field_propertyDepthOfFieldBlurMiddlePlane.floatValue) / 2;
						}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
						if (GUILayout.Button(new GUIContent("-", "closer"), EditorStyles.miniButtonLeft, miniButton))
						{
							field_propertyDepthOfFieldBlurFarPlane.floatValue = field_propertyDepthOfFieldBlurFarPlane.floatValue - (field_propertyDepthOfFieldBlurFarPlane.floatValue - field_propertyDepthOfFieldBlurMiddlePlane.floatValue) / 2;
						}
						if (GUILayout.Button(new GUIContent("+", "further"), EditorStyles.miniButtonRight, miniButton))
						{
							field_propertyDepthOfFieldBlurFarPlane.floatValue = field_propertyDepthOfFieldBlurFarPlane.floatValue + (field_propertyDepthOfFieldBlurFarPlane.floatValue - field_propertyDepthOfFieldBlurMiddlePlane.floatValue);
						}
					EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				if (GUILayout.Button(new GUIContent("+", "further"), EditorStyles.miniButtonRight, miniButtonHigh))
				{
					field_propertyDepthOfFieldBlurNearPlane.floatValue *= 2;
					field_propertyDepthOfFieldBlurMiddlePlane.floatValue *= 2;
					field_propertyDepthOfFieldBlurFarPlane.floatValue *= 2;
				}
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel -= 1;
		}
		//Debug.Log(DateTime.Now.Ticks);
		//Debug.Log((target as GUI01).Count);
		//Debug.Log(count);

		serializedObject.ApplyModifiedProperties();
	}

	Color Normalize(Color param_color)
	{
		Vector3 vector = new Vector3(param_color.r, param_color.g, param_color.b);
		vector.Normalize();
		return new Color(vector.x, vector.y, vector.z, 1);
	}
	
}