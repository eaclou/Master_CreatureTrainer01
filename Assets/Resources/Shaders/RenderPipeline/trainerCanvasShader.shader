Shader "Unlit/trainerCanvasShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_DepthTex ("DepthTex", 2D) = "black" {}
		_MaxDepth ("MaxDepth", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			//sampler2D _ColorTex;
			//float4 _ColorTex_ST;
			float4 _Color;
			sampler2D _DepthTex;
			float _MaxDepth;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			void frag (v2f i, out half4 outColor : COLOR0, out half4 outDepth : COLOR1)
			{
				// sample the texture
				//fixed4 col = tex2D(_ColorTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//return col;

				fixed4 col = _Color;
				outColor = col;
				fixed4 depth = tex2D(_DepthTex, i.uv) * _MaxDepth;
				outDepth = depth;

			}
			ENDCG
		}
	}
}
