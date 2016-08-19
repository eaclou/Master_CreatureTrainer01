Shader "Unlit/BrushstrokeDisplay"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		//Tags { "RenderType"="Opaque" }
		//LOD 100
		Tags{ "RenderType" = "Transparant" }
		ZTest Off
		ZWrite Off
		Cull Off
		//Blend SrcAlpha One
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#pragma target 5.0
			#include "UnityCG.cginc"

			struct strokeData {
				float3 pos;
			};

			//sampler2D _MainTex;
			//float4 _MainTex_ST;
			float4 _Color;
			StructuredBuffer<strokeData> strokeDataBuffer;
			StructuredBuffer<float3> quadPointsBuffer;

			//struct appdata  // reads from structuredBuffers instead
			//{
			//	float4 vertex : POSITION;
			//	float2 uv : TEXCOORD0;
			//};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)				
			};
			
			v2f vert (uint id : SV_VertexID, uint inst : SV_InstanceID)
			{
				v2f o;

				//Only transform world pos by view matrix
				//To Create a billboarding effect
				float3 worldPosition = strokeDataBuffer[inst].pos;
				float3 quadPoint = quadPointsBuffer[id];
				o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, float4(worldPosition, 1.0f)) + float4(quadPoint, 0.0f));
				//Shift coordinates for uvs
				o.uv = quadPointsBuffer[id] + 0.5f;

				// DEFAULT:
				//o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target  // COLOR
			{
				// sample the texture
				fixed4 col = _Color;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
