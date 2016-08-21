Shader "Unlit/BrushstrokeDisplay"
{
	Properties
	{
		_BrushTex ("Brush Texture", 2D) = "white" {}
		_Tint("Color", Color) = (1,1,1,1)
		_Size("Size", vector) = (1,1,0,0)
		_PaintThickness("Paint Thickness", Range(0,1)) = 0.25
		_PaintReach("Paint Reach", Range(0,1)) = 0.1   // This specifies how 'far' down the canvasDepth the paint will be applied
			// i.e. paintReach=1 means full coverage of the canvas, paintReach=0.1 means paint will only be applied where the canvasDepth is >0.9 normalized...
			// This will likely change later
		_UseSourceColor("_UseSourceColor", Range(0,1)) = 1.0
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
			#pragma target 5.0
			#include "UnityCG.cginc"

			struct strokeData {
				float3 pos;
			};

			sampler2D _BrushTex;
			float4 _BrushTex_ST;
			float4 _Tint;
			float2 _Size;
			float _PaintThickness;
			float _PaintReach;
			float _UseSourceColor;
			StructuredBuffer<strokeData> strokeDataBuffer;
			StructuredBuffer<float3> quadPointsBuffer;

			// Stores result of Unity's Rendering:
			sampler2D _BrushColorReadTex;
			sampler2D _CanvasColorReadTex;
			sampler2D _CanvasDepthReadTex;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 localUV : TEXCOORD0;  // uv of the brushstroke quad itself, particle texture
				float2 screenUV : TEXCOORD1;  // uv in screenspace of the frag -- for sampling from renderBuffers
				float2 centerUV : TEXCOORD2;  // uv of just the centerPoint of the brushstroke, in screenspace so it can sample from colorBuffer		
			};

			v2f vert(uint id : SV_VertexID, uint inst : SV_InstanceID)
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
				
				return o;
			}

			void frag(v2f i, out half4 outColor : COLOR0, out half4 outDepth : COLOR1)
			{
				half4 buffer0 = tex2D(_BrushColorReadTex, i.centerUV);  //  Color of brushtroke source				
				fixed4 depth = tex2D(_CanvasDepthReadTex, i.screenUV);  // Read Depth:				
				fixed4 col = tex2D(_CanvasColorReadTex, i.screenUV);  // Read Canvas Color:
				fixed4 brush = tex2D(_BrushTex, i.localUV);  // Read Brush Texture
				float threshold = 1.0 - _PaintReach;
				float fade = 0.1;
				float value = smoothstep(threshold - fade, threshold + fade, depth);
				//float paintDepth = 0.25;
				float3 brushHue = lerp(float3(1.0, 1.0, 1.0) * _Tint.rgb, buffer0.rgb * _Tint.rgb, _UseSourceColor);  // use own paint color or use SceneRenderColor?
				col = lerp(col, float4(brushHue, 1.0), value);
				
				//col.rgb = brushHue;
				col.a *= brush.x * _Tint.a;
				depth.a = 1.0;
				depth.rgb += brush.y * _PaintThickness;
				depth.a *= brush.x * _Tint.a;
				outDepth = depth;  // no change to depth for now...
				outColor = col;
			}
		ENDCG
		}
	}
}
