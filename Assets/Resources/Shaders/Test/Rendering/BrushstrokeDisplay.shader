Shader "Unlit/BrushstrokeDisplay"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Size("Size", vector) = (1,1,0,0)
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
			float2 _Size;
			StructuredBuffer<strokeData> strokeDataBuffer;
			StructuredBuffer<float3> quadPointsBuffer;

			// Unity GBuffers special names:
			//sampler2D _CameraGBufferTexture0;
			//sampler2D _CameraGBufferTexture1;
			//sampler2D _CameraGBufferTexture2;
			//sampler2D _CameraGBufferTexture3;

			// Stores result of Unity's Rendering:
			sampler2D _FrameBufferTexture;
			sampler2D _ColorReadTex;
			sampler2D _DepthReadTex;

			//struct appdata  // reads from structuredBuffers instead
			//{
			//	float4 vertex : POSITION;
			//	float2 uv : TEXCOORD0;
			//};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 localUV : TEXCOORD0;  // uv of the brushstroke quad itself, particle texture
				float2 screenUV : TEXCOORD1;  // uv in screenspace of the frag -- for sampling from renderBuffers
				float2 centerUV : TEXCOORD2;  // uv of just the centerPoint of the brushstroke, in screenspace so it can sample from colorBuffer
				//UNITY_FOG_COORDS(1)				
			};
			
			v2f vert (uint id : SV_VertexID, uint inst : SV_InstanceID)
			{
				v2f o;

				
				//Only transform world pos by view matrix
				//To Create a billboarding effect
				float3 worldPosition = strokeDataBuffer[inst].pos;
				float3 quadPoint = quadPointsBuffer[id];
				
				o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, float4(worldPosition, 1.0f)) + float4(quadPoint * float3(_Size, 1.0), 0.0f));
				float4 screenUV = ComputeScreenPos(o.pos);
				o.screenUV = screenUV.xy / screenUV.w;				
				
				// Magic to get proper UV's for sampling from GBuffers:
				float4 pos = mul(UNITY_MATRIX_VP, float4(worldPosition, 1));  
				float4 centerUV = ComputeScreenPos(pos);
				o.centerUV = centerUV.xy / centerUV.w;
				
				//Shift coordinates for uvs
				o.localUV = quadPointsBuffer[id] + 0.5f;


				// DEFAULT:
				//o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target  // COLOR
			{
				half4 buffer0 = tex2D(_FrameBufferTexture, i.centerUV);  //  Color of rendered scene	
				// Read Depth:
				fixed4 depth = tex2D(_DepthReadTex, i.screenUV);  // just pass along depth info unedited for now....	
				// Read Canvas Color:
				fixed4 col = tex2D(_ColorReadTex, i.screenUV);
				float threshold = 0.35;
				float fade = 0.1;
				float value = smoothstep(threshold - fade, threshold + fade, depth);				
				float paintDepth = 0.25;
				col = lerp(col, float4(buffer0.xyz, 1.0) * _Color, value);

				//outDepth = depth;
				//outColor = col;

				return col;
			}
			ENDCG
		}
	}
}
