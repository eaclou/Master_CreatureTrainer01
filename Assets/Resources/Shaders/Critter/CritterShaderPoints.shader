Shader "Custom/Critter/CritterShaderPoints" {
	Properties{
		_Sprite("Sprite", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Size("Size", vector) = (1,1,0,0)
		
	}
	SubShader{
		Tags{ "Queue" = "Overlay+100" "RenderType" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
		ZWrite off

		pass {
			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "Assets/Resources/Shaders/Inc/SimplexShared.cginc"

			sampler2D _Sprite;
			float4 _Color = float4(1, 0.5f, 0.0f, 1);
			float2 _Size = float2(1, 1);
			float _NoiseScale = 36.0;
			float3 _worldPos;
			
			int _StaticCylinderSpherical = 2;  // 0=static, 1=cylinder, 2=spherical

			struct data {
				float3 pos;
				float3 normal;
			};

			struct segmentTransforms {
				float3 pos;
				float4 rot;
				float4x4 xform;
			};

			// buffer containing points we want to draw:
			StructuredBuffer<data> buf_decorations;

			StructuredBuffer<segmentTransforms> buf_xforms;
			
			struct fragInput {
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float2 uv : TEXCOORD0;
				float4 color : COLOR0;
				float3 viewDir : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};

			float3 RotatePoint(float3 position, float4 rotation) {
				float3 t = 2 * cross(rotation.xyz, position);
				return (position + rotation.w * t + cross(rotation.xyz, t));
			}

			float DistancePointToBox(float3 pos, float3 boxPos, float3 boxScale, float4 boxRotation, float extrudeAmount) {

				float3 boxCenterToPoint = pos - boxPos + float3(0.00001, 0.00001, 0.00001);  // prevent divide by 0
				float3 right = RotatePoint(float3(1.0, 0, 0), boxRotation);
				float3 up = RotatePoint(float3(0, 1.0, 0), boxRotation);
				float3 forward = RotatePoint(float3(0, 0, 1.0), boxRotation);

				float dotRight = dot(right, boxCenterToPoint);
				float dotUp = dot(up, boxCenterToPoint);
				float dotForward = dot(forward, boxCenterToPoint);

				// Cache distance amounts per box-local-dimensions:
				float dotRightAbs = abs(dotRight);
				float dotRightSign = dotRight / dotRightAbs;
				float dotUpAbs = abs(dotUp);
				float dotUpSign = dotUp / dotUpAbs;
				float dotForwardAbs = abs(dotForward);
				float dotForwardSign = dotForward / dotForwardAbs;

				float minDistance = 0.001;  // prevent divide by 0 and negative distances
											//float roundness = 1; // multiplier on boxScale
											//float extrudeAmount = 0.25;

											// find point on box 'edge' to measure against samplepoint:
				float distRight = min(dotRightAbs, max(boxScale.x - extrudeAmount, 0.0)) * dotRightSign; // amount to move in box's local X direction
				float distUp = min(dotUpAbs, max(boxScale.y - extrudeAmount, 0.0)) * dotUpSign;
				float distForward = min(dotForwardAbs, max(boxScale.z - extrudeAmount, 0.0)) * dotForwardSign;

				float3 offsetVector = pos - (right * distRight + up * distUp + forward * distForward + boxPos);

				float distance = max(minDistance, length(offsetVector));

				return distance;
			}

			float DistancePointToPoint(float3 pos, float3 boxPos) {
				float3 boxCenterToPoint = pos - boxPos + float3(0.0001, 0.0001, 0.0001);  // prevent divide by 0

				float distance = length(boxCenterToPoint);

				return distance;
			}

			float4 CalculateSkinning(float4 pos) {
				int2 indices = int2(0, 0);  // just in case, zero out values
				float2 weights = float2(0.0, 0.0);

				// BoneWeights!!!:
				uint elements;
				uint stride;
				buf_xforms.GetDimensions(elements, stride);  // cache the length of xForm buffer?

				float2 boneInfluence = float2(0.0, 0.0);
				for (int i = 0; i < 16; i++) {
					if (i < elements) {
						float dist = 1.0 * DistancePointToPoint(pos.xyz, buf_xforms[i].pos);
						//dist += 0.1 * DistancePointToBox(pos, segmentTransformBuffer[i].P, segmentTransformBuffer[i].S, segmentTransformBuffer[i].R, 0.5);
						float rawInfluence = 1.0 / (dist * dist);

						if (rawInfluence > boneInfluence.x) {  // new top dawg:
							boneInfluence.y = boneInfluence.x;
							boneInfluence.x = rawInfluence;
							indices.y = indices.x;
							indices.x = i;
						}
						else {
							if (rawInfluence > boneInfluence.y) {  // new beta wolf:
								boneInfluence.y = rawInfluence;
								indices.y = i;
							}
						}
					}
				}
				float totalInfluence = boneInfluence.x + boneInfluence.y;
				weights = float2(boneInfluence.x / totalInfluence, boneInfluence.y / totalInfluence);

				// Transform points!
				float3 newPos = (pos.xyz - buf_xforms[indices.x].pos, 1) + buf_xforms[indices.x].pos;
				//float4 newPos = pos + 1.0;
				return float4(newPos, 1);
			}

			fragInput vert(uint id : SV_VertexID) {
				fragInput o;
				o.pos = float4(buf_decorations[id].pos + _worldPos, 1.0f);
				//  + simplex3d(buf_decorations[id].pos)
				o.norm = buf_decorations[id].normal;
				float3 lightDirection = float3(0.2, 1.0, 0.3);
				float3 diffuse = dot(o.norm, lightDirection) * 0.5 + 0.5;
				o.color = float4(diffuse, 1.0);
				float3 camToWorldVector = _WorldSpaceCameraPos.xyz - _worldPos.xyz;
				o.viewDir = normalize(camToWorldVector);
				return o;
			}

			float4 RotPoint(float4 p, float3 offset, float3 sideVector, float3 upVector) {
				float3 finalPos = p.xyz;
				finalPos += offset.x * sideVector;
				finalPos += offset.y * upVector;

				return float4(finalPos, 1);
			}

			[maxvertexcount(4)]
			void geom(point fragInput p[1], inout TriangleStream<fragInput> triStream) {
				float2 halfS = _Size + simplex3d(p[0].pos.xyz * 16) * _Size;

				float4 v[4];

				//if (_StaticCylinderSpherical == 0) {
				//	v[0] = p[0].pos.xyzw + float4(-halfS.x, -halfS.y, 0, 0);
				//	v[1] = p[0].pos.xyzw + float4(-halfS.x, halfS.y, 0, 0);
				//	v[2] = p[0].pos.xyzw + float4(halfS.x, -halfS.y, 0, 0);
				//	v[3] = p[0].pos.xyzw + float4(halfS.x, halfS.y, 0, 0);
				//}
				//else {
				float3 up = p[0].norm;  // normalize(float3(0, 1, 0));
				float3 look = float3(simplex3d(p[0].pos.xyz * 10), simplex3d(p[0].pos.xyz * 20), simplex3d(p[0].pos.xyz * 30));   //p[0].norm;  // _WorldSpaceCameraPos - p[0].pos.xyz;
				
				look = normalize(look);
				float3 right = normalize(cross(look, up));
				up = normalize(cross(right, look));

				v[0] = RotPoint(p[0].pos, float3(-halfS.x, -halfS.y, 0), right, up);
				v[1] = RotPoint(p[0].pos, float3(-halfS.x, halfS.y, 0), right, up);
				v[2] = RotPoint(p[0].pos, float3(halfS.x, -halfS.y, 0), right, up);
				v[3] = RotPoint(p[0].pos, float3(halfS.x, halfS.y, 0), right, up);
				//}
				
				fragInput pIn;
				pIn.norm = p[0].norm;
				pIn.color = p[0].color;
				pIn.viewDir = p[0].viewDir;
				
				pIn.pos = CalculateSkinning(v[0]);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 0);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = CalculateSkinning(v[1]);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 1);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = CalculateSkinning(v[2]);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 0);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = CalculateSkinning(v[3]);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 1);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);
			}

			float4 frag(fragInput i) : COLOR{
				float viewAngle = dot(i.norm, i.viewDir);
				float outlineStrength = 1.0 - pow(viewAngle, 1);
				
				
				fixed4 col = tex2D(_Sprite, i.uv) * _Color * i.color;
				col = 0.5 * (col + outlineStrength * 0.75) * (1.0 - viewAngle) + 0.5 * col;
				col.a = 0.2;

				UNITY_APPLY_FOG(i.fogCoord, col); // apply fog

				return col;
			}

			ENDCG
		}
	}
	FallBack Off
}
