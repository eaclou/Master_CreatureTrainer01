Shader "Custom/SimpleBrainNetworkShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags{ "Queue" = "Transparent" }
		//Tags{ "RenderType" = "Opaque" }
		//LOD 200
		
		pass {
			//ZWrite Off
			ZTest Always

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma vertex vert
			#pragma fragment frag

			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0

			
			// uniforms
			uniform fixed4 _Color;	

			struct vertexInput {
				float4 vertex : POSITION; // position in object coordinates
				fixed4 color : COLOR;  // vertex color
				//float4 tangent : TANGENT; // vector orthogonal to the surface normal
				float3 normal : NORMAL; // surface normal vector (in object coordinates; usually normalized)
				//float4 texcoord : TEXCOORD0; // 0th set of texture coordinates ( UV)
				//float4 texcoord1 : TEXCOORD1; // 1st set of ...
				
			};
			
			struct fragmentInput
			{
				float4 pos : SV_Position;
				//float4 uv : TEXCOORD0;
				float4 color : COLOR0;
				float3 normalDir : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};

			fragmentInput vert( vertexInput i) 
			{
				fragmentInput o;				
				o.color = _Color;
				
				//normal dir
				o.normalDir = normalize ( mul ( float4( i.normal, 0.0 ), _World2Object).xyz );				
				//world position
				float4 posWorld = mul(_Object2World, i.vertex);				
				//view direction
				float3 camToWorldVector = _WorldSpaceCameraPos.xyz - posWorld.xyz;
				//o.viewDir = normalize( camToWorldVector);
				// pos
				//float val = abs(((o.color.r + o.color.g + o.color.b) / 3.0) - 0.5) + 1;
				//i.vertex = (i.vertex - 0.5) * val * 1.0;
				o.pos = mul( UNITY_MATRIX_MVP, i.vertex );
				
				
				// DEBUG options - sets color to other variables
				//o.color = i.texcoord;
				//o.uv = i.texcoord;
				//o.color = i.texcoord1;
				//o.color = i.vertex;
				//o.color = i.vertex + float4(0.5, 0.5, 0.5, 0.0 );  // add .5 to offset if the model verts go from -0.5 to 0.5
				//o.color = i.tangent;
				//o.color = float4( i.normal * 0.5 + 0.5, 1.0 ); // scale and bias the normal to get it in range 0-1
				//o.color = i.color // vertex colors
				//o.color = float4( i.normal, 1.0);
				

				float dist = sqrt(dot(camToWorldVector, camToWorldVector));
				float maxFogDist = 20.0;
				float fogAmount = clamp(0.0, 1.0, dist/maxFogDist);
				float4 fogColor = float4(0.4, 0.6, 0.75, 1.0);
				//o.color = fogColor;
				o.color = lerp(i.color * _Color, fogColor, fogAmount);
				//o.color.a = 0.0;
				
				return o;
			}
			
			half4 frag( fragmentInput i ) : COLOR
			{
				//float outlineStrength = dot(i.normalDir, i.viewDir) * 12.2;
				//outlineStrength = pow(outlineStrength, 4) * 0.00045 + 0.25;							
				//return i.color * outlineStrength;
				i.color.a = 0.5;
				return i.color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
