Shader "Unlit/gessoShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,0,1)
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _ColorReadTex;
			sampler2D _DepthReadTex;			
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			void frag(v2f i, out half4 outColor : COLOR0, out half4 outDepth : COLOR1)
			{
				// Read Depth:
				fixed4 depth = tex2D(_DepthReadTex, i.uv);  // just pass along depth info unedited for now....				
				fixed4 col = tex2D(_ColorReadTex, i.uv);
				float threshold = 0.25;
				float fade = 0.05;
				float value = smoothstep(threshold - fade, threshold + fade, depth);
				col = lerp(col, _Color, value);
				float primerDepth = 0.25;

				outDepth = max(depth, primerDepth);
				outColor = col;
				

				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//return col;
			}
			ENDCG
		}
	}
}
