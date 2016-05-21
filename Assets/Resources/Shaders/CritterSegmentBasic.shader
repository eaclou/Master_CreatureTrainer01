Shader "Custom/CritterSegmentBasic" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_NeuronPosTex("Neuron Positions", 2D) = "black" {}
		//_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TargetPosX ("TargetPosX", Range(-1,1)) = 0.0
		_TargetPosY ("TargetPosY", Range(-1,1)) = 0.0
		_TargetPosZ ("TargetPosZ", Range(-1,1)) = 0.0
		_SizeX("SizeX", Float) = 1.0
		_SizeY("SizeY", Float) = 1.0
		_SizeZ("SizeZ", Float) = 1.0
		_Selected ("Selected", Range(0.0,1.0)) = 0.0
		//_MouseOver("MouseOver", Range(0.0,1.0)) = 0.0
		_DisplayTarget ("DisplayTarget", Range(0.0,1.0)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque"  "RenderQueue" = "Opaque" }
		//LOD 200
		
		pass {
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma target 4.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0

			
			// uniforms
			uniform fixed4 _Color;	
			//sampler2D _MainTex;
			uniform sampler2D _NeuronPosTex;
			uniform float4 _NeuronPosTex_TexelSize;
			uniform fixed _TargetPosX;
			uniform fixed _TargetPosY;	
			uniform fixed _TargetPosZ;	
			uniform fixed _SizeX;
			uniform fixed _SizeY;
			uniform fixed _SizeZ;
			uniform fixed _Selected;
			uniform fixed _DisplayTarget;
			uniform fixed4 _LightColor0;

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
				float3 normalDirection = normalize(mul(float4(i.normal, 1.0), _World2Object).xyz);
				//float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 lightDirection = float3(0.2, 1.0, 0.3);
				float3 diffuse = _LightColor0.xyz * max(0.0, dot(normalDirection, lightDirection)) * _Color;
				float4 posWorld = mul(_Object2World, i.vertex);
				//view direction
				float3 camToWorldVector = _WorldSpaceCameraPos.xyz - posWorld.xyz;
				//diffuse = lightDirection;
				//o.wpos = mul( _Object2World, i.vertex ).xyz;
				//o.pos = posWorld;
				o.wpos = posWorld;
				//o.pos = i.vertex;
				//o.color = _Color;
				
				// DEBUG options - sets color to other variables
				
				o.uv = i.texcoord;
				//o.color = i.texcoord1;
				//o.color = i.vertex;
				//o.color = i.vertex + float4(0.5, 0.5, 0.5, 0.0 );  // add .5 to offset if the model verts go from -0.5 to 0.5
				//o.color = i.tangent;
				//o.color = float4( i.normal * 0.5 + 0.5, 1.0 ); // scale and bias the normal to get it in range 0-1
				//o.color = i.color // vertex colors
				//o.color = float4( i.normal, 1.0);
							

				float dist = sqrt(dot(camToWorldVector, camToWorldVector));
				float maxFogDist = 5.0;
				float fogAmount = 0.1 + clamp(0.0, 1.0, dist / maxFogDist);
				float4 fogColor = float4(0.3, 0.5, 0.65, 1.0);
				//o.color = fogColor;
				o.color = float4(diffuse, 1.0);
				o.color = lerp(o.color, fogColor, fogAmount);
				//float4 light = float4(brightness, brightness, brightness, 0.0);
				
				//o.color = float4(o.color.x + brightness, o.color.y + brightness, o.color.z + brightness, 1.0);
				return o;
			}
			
			half4 frag( fragmentInput i ) : COLOR
			{
				//i.color = lerp(float4(0.8, 0.8, 0.8, 1.0), i.color, 0.1);	
				float4 diffuse = i.color;
				float gradient = i.wpos.x + i.wpos.y + i.wpos.z;
				gradient = (gradient + 3) / 6;
				//float4 newColor = float4(gradient, gradient, gradient, 1.0);
				float4 baseColor = float4(0.32, 0.45, 0.62, 1.0);
				//baseColor = float4(0.8, 0.8, 0.8, 1.0);
				if (_Selected) {
					baseColor = float4(0.4, 0.6, 0.7, 1.0);
				}
				i.color = diffuse * float4(0.25, 0.75, 0.55, 1.0);;
				float4 edgeColor = float4(0.7, 0.7, 0.7, 1.0);
				float edgeWidth = 0.025;
				//i.color += float4(i.wpos * 0.02, 0.0);
				//i.color += i.wpos.z * 0.1;
				i.wpos.x *= _SizeX;
				i.wpos.y *= _SizeY;
				i.wpos.z *= _SizeZ;
				_TargetPosX *= _SizeX;
				_TargetPosY *= _SizeY;
				_TargetPosZ *= _SizeZ;
				float xy = sqrt(pow(0.5 * _SizeX - abs(i.wpos.x), 2.0) + pow(0.5 * _SizeY - abs(i.wpos.y), 2.0));
				float xz = sqrt(pow(0.5 * _SizeX - abs(i.wpos.x), 2.0) + pow(0.5 * _SizeZ - abs(i.wpos.z), 2.0));
				float yz = sqrt(pow(0.5 * _SizeY - abs(i.wpos.y), 2.0) + pow(0.5 * _SizeZ - abs(i.wpos.z), 2.0));
				if (xy < edgeWidth) {
					//i.color = edgeColor;
				}
				if (xz < edgeWidth) {
					//i.color = edgeColor;
				}
				if (yz < edgeWidth) {
					//i.color = edgeColor;
				}
				// Target:
				if (_DisplayTarget >= 0.5) {
					float targetRadius = 0.4;
					float targetDeltaX = _TargetPosX - i.wpos.x;
					float targetDeltaY = _TargetPosY - i.wpos.y;
					float targetDeltaZ = _TargetPosZ - i.wpos.z;
					float targetdist = sqrt(targetDeltaX * targetDeltaX + targetDeltaY * targetDeltaY + targetDeltaZ * targetDeltaZ);

					float targetEdgeWidth = 0.01;
					float targetEdgeFadeWidth = 0.005;
					float4 targetEdgeColor = float4(0.75, 1.0, 0.8, 1.0) * 0.75;
					//float txy = sqrt(pow(0.5 - abs(i.wpos.x), 2.0) + pow(0.5 - abs(i.wpos.y), 2.0));
					//float txz = sqrt(pow(0.5 - abs(i.wpos.x), 2.0) + pow(0.5 - abs(i.wpos.z), 2.0));
					//float tyz = sqrt(pow(0.5 - abs(i.wpos.y), 2.0) + pow(0.5 - abs(i.wpos.z), 2.0));
					float onFaceThreshold = 0.01;
					float tx = abs(0.5 * _SizeX - abs(_TargetPosX));
					float ty = abs(0.5 * _SizeY - abs(_TargetPosY));
					float tz = abs(0.5 * _SizeZ - abs(_TargetPosZ));
					float x = abs(i.wpos.x - _TargetPosX);
					float y = abs(i.wpos.y - _TargetPosY);
					float z = abs(i.wpos.z - _TargetPosZ);
					bool onFaceX = false;
					bool onFaceY = false;
					bool onFaceZ = false;
					if (tx < onFaceThreshold) { // On X Face
						onFaceX = true;
					}
					if (ty < onFaceThreshold) { // On Y Face
						onFaceY = true;
					}
						
					if (tz < onFaceThreshold) { // On Z Face
						onFaceZ = true;
					}

					if (!onFaceX) {
						if (x < targetEdgeWidth) {
							//i.color = targetEdgeColor;
						}
					}
					if (!onFaceY) {
						if (y < targetEdgeWidth) {
							//i.color = targetEdgeColor;
						}
					}
					if (!onFaceZ) {
						if (z < targetEdgeWidth) {
							//i.color = targetEdgeColor;
						}
					}						
					//i.color += float4(0.16, 0.16, 0.16, 0.0);  // brighten segment being hovered over
					
					if (targetdist < targetRadius)
					{
						//i.color = lerp(float4(2.0, 3.0, 2.25, 1.0), i.color, pow((targetdist / targetRadius), 0.05));
						
						
					}
				}

				float accumLight = 0.0;
				float lightCoreRange = 0.3;
				float lightMaxRange = 1.0;
				float numNeurons = _NeuronPosTex_TexelSize.z; //contains width
				float incrementSize = 1.0 / numNeurons;
				float distanceToNeuron = 0.0;
				for (float k = 0.0; k < 40.0; k++) {
					if (k < numNeurons) {
						float4 neuronInfo = tex2D(_NeuronPosTex, half2(k * incrementSize + incrementSize * 0.5, 0.0)); // x y z = pos, w = brightness
						//neuronInfo.x = (neuronInfo.x - 0.5) * 4.0;
						//neuronInfo.y = (neuronInfo.y - 0.5) * 4.0;
						//neuronInfo.z = (neuronInfo.z - 0.5) * 4.0;
						//neuronInfo.w = (neuronInfo.w - 0.5) * 2.0;
						float3 vectorToNeuron = neuronInfo.xyz - (i.wpos);
						distanceToNeuron = sqrt(vectorToNeuron.x * vectorToNeuron.x + vectorToNeuron.y * vectorToNeuron.y + vectorToNeuron.z * vectorToNeuron.z);
						accumLight += max(0.0, min(1.0, (lightMaxRange - distanceToNeuron) / lightMaxRange)) * neuronInfo.w * 0.35;
						accumLight += max(0.0, min(1.0, (lightCoreRange - distanceToNeuron) / lightCoreRange)) * neuronInfo.w * 0.4;
					}
				}
				float brightness = min(accumLight * 0.4, 0.4);
				
				//return float4(xy, yz, _TargetPosZ, 1.0);
				//i.color += float4(brightness, brightness, brightness, 0.0);

				//i.color = lerp(diffuse, i.color, 0.5);
				i.color += float4(brightness, brightness, brightness, 0.0);
				return i.color;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
