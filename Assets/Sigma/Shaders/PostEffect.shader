Shader "Custom/PostEffect" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CorrectionRedColor ("Color Correction Red", Color) = (1,0,0,1)
		_CorrectionGreenColor ("Color Correction Green", Color) = (0,1,0,1)
		_CorrectionBlueColor ("Color Correction Blue", Color) = (0,0,1,1)
		_CorrectionRGBAmount ("Color Correction Amount", Range(0, 1)) = 0
		_CorrectionColorBurn ("Color Burn", Range(0, 1)) = 0
		_CorrectionContrast ("Contrast", Range(0, 10)) = 0
		_CorrectionDofBlur ("DoF Blur", Range(0, 1)) = 0
		_CorrectionDofBlurNearPlane ("DoF Blur", Range(0, 1)) = 0
		_CorrectionDofBlurMiddlePlane ("DoF Blur", Range(0, 1)) = 0.5
		_CorrectionDofBlurFarPlane ("DoF Blur", Range(0, 1)) = 1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert //_img // Built-in vert_img
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _CameraDepthNormalsTexture;
			fixed4 _CorrectionRedColor;
			fixed4 _CorrectionGreenColor;
			fixed4 _CorrectionBlueColor;
			float _CorrectionRGBAmount;
			float _CorrectionColorBurn;
			float _CorrectionContrast;
			float _CorrectionDofBlur;
			float _CorrectionDofBlurNearPlane;
			float _CorrectionDofBlurMiddlePlane;
			float _CorrectionDofBlurFarPlane;
			
			struct VS_INPUT
			{
				float4 vertex : POSITION;
				float4 texcoord0 : TEXCOORD0;
			};

			struct VS_OUTPUT
			{
				float4 position : SV_POSITION;
				float4 texcoord0 : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};

			VS_OUTPUT vert(VS_INPUT input)
			{         
				VS_OUTPUT output;
				output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				output.texcoord0 = input.texcoord0;
				output.screenPos = ComputeScreenPos(output.position);
				return output;
			}

			fixed4  color_around(float2 uv)
			{
				fixed4 color = fixed4(0,0,0,0); 
				fixed2 shift =  fixed2(1/_ScreenParams.x, 1/_ScreenParams.y) * 1;
				color += tex2D(_MainTex, float2(uv.x - shift.x, uv.y - shift.y)) * 0.125;
				color += tex2D(_MainTex, float2(uv.x - shift.x, uv.y + shift.y)) * 0.125;
				color += tex2D(_MainTex, float2(uv.x + shift.x, uv.y + shift.y)) * 0.125;
				color += tex2D(_MainTex, float2(uv.x + shift.x, uv.y - shift.y)) * 0.125;
				color += tex2D(_MainTex, float2(uv.x, uv.y - shift.y)) * 0.125;
				color += tex2D(_MainTex, float2(uv.x, uv.y + shift.y)) * 0.125;
				color += tex2D(_MainTex, float2(uv.x - shift.x, uv.y)) * 0.125;
				color += tex2D(_MainTex, float2(uv.x + shift.x, uv.y)) * 0.125;
				return color;
			}

			fixed4 frag(VS_OUTPUT i) : COLOR
			{
				float2 uv = float2(i.texcoord0.x, i.texcoord0.y);
				fixed4 color = tex2D(_MainTex, uv);
				fixed4 colorAround = color_around(uv);

				fixed depthFromBuffer = Linear01Depth(tex2Dproj(_CameraDepthNormalsTexture, UNITY_PROJ_COORD(i.screenPos)).w);
				//return fixed4(depthFromBuffer, depthFromBuffer, depthFromBuffer, 1);

				color.rgb = color.rgb * (1 - _CorrectionRGBAmount) + (color.r * _CorrectionRedColor.rgb + color.g * _CorrectionGreenColor.rgb + color.b * _CorrectionBlueColor.rgb) * _CorrectionRGBAmount;
				
				fixed lumence = dot(color.rgb, float3(0.299, 0.587, 0.144));
				fixed4 colorGrey = fixed4(lumence, lumence, lumence, 1);

				float blur = _CorrectionDofBlur;
				if (depthFromBuffer > _CorrectionDofBlurNearPlane && depthFromBuffer < _CorrectionDofBlurMiddlePlane)
					blur *= 1 - (depthFromBuffer - _CorrectionDofBlurNearPlane) / (_CorrectionDofBlurMiddlePlane - _CorrectionDofBlurNearPlane);
				else if (depthFromBuffer > _CorrectionDofBlurMiddlePlane && depthFromBuffer < _CorrectionDofBlurFarPlane)
					blur *= (depthFromBuffer - _CorrectionDofBlurMiddlePlane) / (_CorrectionDofBlurFarPlane - _CorrectionDofBlurMiddlePlane);
					
				color.rgb = color.rgb + 
					+ (color.rgb - colorGrey.rgb / 2) * _CorrectionColorBurn + 
					+ (color.rgb - colorAround.rgb) * _CorrectionContrast;
				color.rgb = color.rgb * (1 - blur) + (color.rgb * 0.2 + colorAround.rgb * 0.8) * blur;

				return color;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
