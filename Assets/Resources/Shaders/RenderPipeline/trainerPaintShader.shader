Shader "Unlit/trainerPaintShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		//_Color ("Color", Color) = (1,1,1,1)
		//_DepthTex ("DepthTex", 2D) = "black" {}
		_Relief ("Relief", float) = 1.0
	}
	SubShader
	{
		//Tags { "RenderType"="Opaque" }
		//LOD 100
		ZTest Always Cull Off ZWrite Off
		Fog{ Mode off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag			
			
			#include "UnityCG.cginc"
			
			//sampler2D _ColorTex;
			//float4 _ColorTex_ST;
			//float4 _Color;
			//sampler2D _DepthTex;
			float _Relief;

			uniform sampler2D _MainTex;
			uniform sampler2D _CanvasColorReadTex;
			uniform sampler2D _CanvasDepthReadTex;
			
			float4 frag(v2f_img i) : COLOR
			{
				float4 inColor = tex2D(_MainTex, i.uv);
				float4 depth = tex2Dlod(_CanvasDepthReadTex, float4(i.uv, 1.0, 3.0));
				//float height = depth.x;
				float3 normal = normalize(float3(-ddx(depth.x), -ddy(depth.x), 1.0));
				float3 lightDir = normalize(float3(-1.0, 1.0, 0.0));

				float shine = dot(normal, lightDir) * _Relief;
				float4 shineColor = float4(shine, shine, shine, 0.0);
				float4 outColor = inColor + shineColor;
				return outColor;
			}
			ENDCG
		}
	}
}
