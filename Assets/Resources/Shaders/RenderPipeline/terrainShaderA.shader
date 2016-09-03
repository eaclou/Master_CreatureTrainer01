// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/TerrainShaderA" {
	Properties {
		_MainTex("MainTex", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vIN
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float3 normal : NORMAL;
			};
			struct vOUT
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR0;
				float3 wNorm : TEXCOORD0;
			};
			vOUT vert(vIN v)
			{
				vOUT o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.wNorm = mul((float3x3)unity_ObjectToWorld, v.normal);
				o.color = v.color;
				return o;
			}

			float4 frag(vOUT i) : COLOR
			{
				float3 norm = (i.wNorm * 0.5) + 0.5;
				return i.color;
			}
			ENDCG

		}		
	} 
	FallBack "Diffuse"
}
