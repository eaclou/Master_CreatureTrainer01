Shader "Custom/CritterEditorGizmo" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		//_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_AxisX ("AxisX", Range(0,1)) = 0.0
		_AxisY ("AxisY", Range(0,1)) = 0.0
		_AxisZZ ("AxisZ", Range(0,1)) = 0.0
		_Selected ("Selected", Range(0.0,1.0)) = 0.0
		//_MouseOver("MouseOver", Range(0.0,1.0)) = 0.0
		_DisplayTarget ("DisplayTarget", Range(0.0,1.0)) = 0.0
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		
		
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
			//sampler2D _MainTex;
			uniform fixed _TargetPosX;
			uniform fixed _TargetPosY;	
			uniform fixed _TargetPosZ;	
			uniform fixed _Selected;
			uniform fixed _DisplayTarget;

			struct vertexInput {
				float4 vertex : POSITION; // position in object coordinates
				float4 tangent : TANGENT; // vector orthogonal to the surface normal
				float3 normal : NORMAL; // surface normal vector (in object coordinates; usually normalized)
				float4 texcoord : TEXCOORD0; // 0th set of texture coordinates ( UV)
				float4 texcoord1 : TEXCOORD1; // 1st set of ...
				fixed4 color : COLOR;  // vertex color
			};
			
			struct fragmentInput
			{
				float4 pos : POSITION;
				float4 uv : TEXCOORD0;
				float3 wpos : TEXCOORD1;
				float4 color : COLOR0;
			};

			fragmentInput vert( vertexInput i) 
			{
				fragmentInput o;
				o.pos = mul( UNITY_MATRIX_MVP, i.vertex );
				//o.wpos = mul( _Object2World, i.vertex ).xyz;
				o.wpos = i.vertex.xyz;
				//o.pos = i.vertex;
				//o.color = _Color;
				
				// DEBUG options - sets color to other variables
				o.color = i.color * _Color;
				o.uv = i.texcoord;
				//o.color = i.texcoord1;
				//o.color = i.vertex;
				//o.color = i.vertex + float4(0.5, 0.5, 0.5, 0.0 );  // add .5 to offset if the model verts go from -0.5 to 0.5
				//o.color = i.tangent;
				//o.color = float4( i.normal * 0.5 + 0.5, 1.0 ); // scale and bias the normal to get it in range 0-1
				//o.color = i.color // vertex colors
				//o.color = float4( i.normal, 1.0);
				
				return o;
			}
			
			half4 frag( fragmentInput i ) : COLOR
			{
				//i.color = lerp(float4(0.8, 0.8, 0.8, 1.0), i.color, 0.1);	
				
				float gradient = i.wpos.x + i.wpos.y + i.wpos.z;
				gradient = (gradient + 3) / 6;
				//float4 newColor = float4(gradient, gradient, gradient, 1.0);
				float4 baseColor = float4(0.35, 0.35, 0.35, 1.0);
				//baseColor = float4(0.8, 0.8, 0.8, 1.0);
				if (_Selected) {
					baseColor = float4(0.55, 0.65, 0.58, 1.0);
				}
				//i.color = baseColor;
				float4 edgeColor = float4(0.7, 0.7, 0.7, 1.0);
				float edgeWidth = 0.025;
				float xy = sqrt(pow(0.5 - abs(i.wpos.x), 2.0) + pow(0.5 - abs(i.wpos.y), 2.0));
				float xz = sqrt(pow(0.5 - abs(i.wpos.x), 2.0) + pow(0.5 - abs(i.wpos.z), 2.0));
				float yz = sqrt(pow(0.5 - abs(i.wpos.y), 2.0) + pow(0.5 - abs(i.wpos.z), 2.0));
				if (xy < edgeWidth) {
					i.color = edgeColor;
				}
				if (xz < edgeWidth) {
					i.color = edgeColor;
				}
				if (yz < edgeWidth) {
					i.color = edgeColor;
				}
									
				
				//return float4(1, 0, 0, 1.0);
				return i.color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
