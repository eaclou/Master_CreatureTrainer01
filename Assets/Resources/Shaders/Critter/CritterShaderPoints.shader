Shader "Custom/Critter/CritterShaderPoints" {
	Properties{
		_Sprite("Sprite", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Size("Size", vector) = (1,1,1,0)
		_SizeRandom("SizeRandom", vector) = (0,0,0,0)
		_BodyColorAmount("BodyColorAmount", Range(0, 1.0)) = 0.0
		//_OrientAttitude("OrientAttitude", Range(0, 1.0)) = 1.0  // 1 = straight 'up' along vertex normal, 0 = 'flat' along shell tangent
		_OrientForwardTangent("OrientForwardTangent", Range(-1.0, 1.0)) = 1.0   // 0 = random facing direction, 1 = aligned with shell forward tangent
		_OrientRandom("OrientRandom", Range(0, 1.0)) = 0.0
		_Type("Type", Int) = 0   // 0 = quad, 1 = scales, 2 = hair
		_Segments("Segments", Int) = 1
		_Diffuse("Diffuse", Range(0.0, 1.0)) = 0
		_DiffuseWrap("DiffuseWrap", Range(0.0, 1.0)) = 0
		_RimGlow("RimGlow", Range(0.0, 1.0)) = 0
		_RimPow("RimPow", Range(0.1, 10.0)) = 1
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
			float3 _Size = float3(1, 1, 1);
			float3 _SizeRandom = float3(0,0,0);
			float _BodyColorAmount = 1.0;
			//float _OrientAttitude = 1.0;  // 1 = straight 'up' along vertex normal, 0 = 'flat' along shell tangent
			float _OrientForwardTangent = 0.0;   // 0 = random facing direction, 1 = aligned with shell forward tangent
			float _OrientRandom = 0.0;
			int _Type = 0;   // 0 = quad, 1 = scales, 2 = hair
			int _Segments = 1;			
			int _StaticCylinderSpherical = 2;  // 0=static, 1=cylinder, 2=spherical
			uniform float _Diffuse;
			uniform float _DiffuseWrap;
			uniform float _RimGlow;
			uniform float _RimPow;

			struct data {
				float3 pos;
				float3 normal;
				float3 tangent;
				float3 color;
			};

			struct segmentTransforms {
				float3 pos;
				float4 rot;
				float3 scale;
				float4x4 xform;
			};

			struct bindPoseStruct {
				float4x4 inverse;
			};

			struct skinningStruct {
				int2 indices;
				float2 weights;
			};

			// buffer containing points we want to draw:
			StructuredBuffer<data> buf_decorations;
			StructuredBuffer<segmentTransforms> buf_xforms;
			StructuredBuffer<bindPoseStruct> buf_bindPoses;
			StructuredBuffer<skinningStruct> buf_skinningData;
			
			struct fragInput {
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float2 uv : TEXCOORD0;
				float4 color : COLOR0;
				float3 viewDir : TEXCOORD2;
				int index : TEXCOORD3;
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

			void CalculateSkinning(inout float4 pos, inout float3 normal, int id) {
				
				//return float4(pos.xyz + buf_xforms[buf_skinningData[id].indices.x].pos * 1, 1);
				//buf_skinningData[id]
				float4 norm4 = float4(normal, 0);  // 0 for 'direction' rather than position
				
				//Get position in local segment space for each Bone by using bindPose inverse Mat4x4:
				float4 localBonePos0 = mul(buf_bindPoses[buf_skinningData[id].indices.x].inverse, pos);
				float4 localBonePos1 = mul(buf_bindPoses[buf_skinningData[id].indices.y].inverse, pos);
				float4 localBoneNorm0 = mul(buf_bindPoses[buf_skinningData[id].indices.x].inverse, norm4);
				float4 localBoneNorm1 = mul(buf_bindPoses[buf_skinningData[id].indices.y].inverse, norm4);
				// Transform this bindPose position by the CURRENT segment position:
				float4 skinnedBonePos0 = mul(buf_xforms[buf_skinningData[id].indices.x].xform, localBonePos0);
				float4 skinnedBonePos1 = mul(buf_xforms[buf_skinningData[id].indices.y].xform, localBonePos1);
				float4 skinnedBoneNorm0 = mul(buf_xforms[buf_skinningData[id].indices.x].xform, localBoneNorm0);
				float4 skinnedBoneNorm1 = mul(buf_xforms[buf_skinningData[id].indices.y].xform, localBoneNorm1);
				// Combine positions:
				float4 skinnedPos = skinnedBonePos0 * buf_skinningData[id].weights.x + skinnedBonePos1 * buf_skinningData[id].weights.y;
				float4 skinnedNorm = skinnedBoneNorm0 * buf_skinningData[id].weights.x + skinnedBoneNorm1 * buf_skinningData[id].weights.y;
				pos = skinnedPos;
				normal = skinnedNorm.xyz;				
			}

			fragInput vert(uint id : SV_VertexID) {
				fragInput o;
				o.index = id;
				o.pos = float4(buf_decorations[id].pos, 1.0f);
				o.norm = buf_decorations[id].normal;
				float3 lightDirection = float3(0.2, 1.0, 0.3);
				float3 diffuse = dot(o.norm, lightDirection);
				diffuse = diffuse * (1.0 - _DiffuseWrap * 0.5) + _DiffuseWrap * 0.5;
				o.color = lerp(_Color, float4(buf_decorations[id].color, _Color.a), _BodyColorAmount);
				o.color = lerp(o.color, o.color * float4(diffuse, o.color.a), _Diffuse);

				float3 camToWorldVector = _WorldSpaceCameraPos.xyz - o.pos.xyz;
				o.viewDir = normalize(camToWorldVector);
				return o;
			}

			float4 RotPoint(float4 p, float3 offset, float3 sideVector, float3 upVector) {
				float3 finalPos = p.xyz;
				finalPos += offset.x * sideVector;
				finalPos += offset.y * upVector;
				finalPos += offset.z * cross(sideVector, upVector);

				return float4(finalPos, 1);
			}

			[maxvertexcount(8)]
			void geom(point fragInput p[1], inout TriangleStream<fragInput> triStream) {
				
				//float3 _Size = float3(1, 1, 1);
				//float3 _SizeRandom = float3(0, 0, 0);
				//float _BodyColorAmount = 1.0;
				//float _OrientAttitude = 1.0;  // 1 = straight 'up' along vertex normal, 0 = 'flat' along shell tangent
				//float _OrientForwardTangent = 0.0;   // 0 = random facing direction, 1 = aligned with shell forward tangent
				//float _OrientRandom = 0.0;
				//int type = 0;   // 0 = quad, 1 = scales, 2 = hair
				//int segments = 1;
				//int _StaticCylinderSpherical = 2;  // 0=static, 1=cylinder, 2=spherical
				
				float randomValue = simplex3d(p[0].pos.xyz * 128);	
				float3 randomDir = normalize(float3(simplex3d(p[0].pos.xyz * 100), simplex3d(p[0].pos.xyz * 200), simplex3d(p[0].pos.xyz * 300)));
				float3 halfS = _Size + (randomValue * _Size * _SizeRandom);
				
				float4 v[8];

				//if (_StaticCylinderSpherical == 0) {
				//	v[0] = p[0].pos.xyzw + float4(-halfS.x, -halfS.y, 0, 0);
				//	v[1] = p[0].pos.xyzw + float4(-halfS.x, halfS.y, 0, 0);
				//	v[2] = p[0].pos.xyzw + float4(halfS.x, -halfS.y, 0, 0);
				//	v[3] = p[0].pos.xyzw + float4(halfS.x, halfS.y, 0, 0);
				//}	

				//float tangentSign = _OrientForwardTangent / abs(_OrientForwardTangent + 0.001); // could still divide by ZERO if = -0.001
				float3 tangent = buf_decorations[p[0].index].tangent;
				if (_OrientForwardTangent < 0) {
					tangent = -tangent;
				}
				float3 up = lerp(p[0].norm, tangent, abs(_OrientForwardTangent));
				up = lerp(up, randomDir, _OrientRandom);  // normalize(float3(0, 1, 0));
				
				float3 look = -randomDir;  // float3(simplex3d(p[0].pos.xyz * 100), simplex3d(p[0].pos.xyz * 200), simplex3d(p[0].pos.xyz * 300));   //p[0].norm;  // _WorldSpaceCameraPos - p[0].pos.xyz;
				
				//look = normalize(look);
				float3 right = normalize(cross(look, up));
				//up = normalize(cross(right, look));

				v[0] = RotPoint(p[0].pos, float3(-halfS.x, 0, 0), right, up);
				v[1] = RotPoint(p[0].pos, float3(-halfS.x, halfS.y * 2.0, 0), right, up);
				v[2] = RotPoint(p[0].pos, float3(halfS.x, 0.0, 0), right, up);
				v[3] = RotPoint(p[0].pos, float3(halfS.x, halfS.y * 2.0, 0), right, up);

				v[4] = RotPoint(p[0].pos, float3(-halfS.x, halfS.y * 2.0, 0), right, up);
				v[5] = RotPoint(p[0].pos, float3(-halfS.x, halfS.y * 4.0, halfS.z), right, up);
				v[6] = RotPoint(p[0].pos, float3(halfS.x, halfS.y * 2.0, 0), right, up);
				v[7] = RotPoint(p[0].pos, float3(halfS.x, halfS.y * 4.0, halfS.z), right, up);
								
				fragInput pIn;
				pIn.index = p[0].index;
				pIn.norm = p[0].norm;
				pIn.color = p[0].color;
				pIn.viewDir = p[0].viewDir;
				
				//pIn.pos = v[0];				
				pIn.pos = v[0];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 0);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[1];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 1);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[2];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 0);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[3];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 1);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				triStream.RestartStrip();

				pIn.pos = v[4];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 0);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[5];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 1);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[6];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 0);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[7];
				CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 1);
				UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);
			}

			float4 frag(fragInput i) : COLOR{
				float viewAngle = saturate(dot(i.norm, i.viewDir));
				float outlineStrength = (1.0 - pow(viewAngle, _RimPow));
				
				
				fixed4 col = tex2D(_Sprite, i.uv) * i.color;
				//col.xyz += outlineStrength;
				col.xyz = _RimGlow * (col.xyz + outlineStrength) + (1.0 - _RimGlow) * col.xyz;
				//col.a = col.r * 0.3;

				UNITY_APPLY_FOG(i.fogCoord, col); // apply fog

				return col;
			}

			ENDCG
		}
	}
	FallBack Off
}
