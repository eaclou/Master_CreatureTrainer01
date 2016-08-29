Shader "Unlit/trainerBrushstrokesCritter" {
	Properties{
		//_Sprite("Sprite", 2D) = "white" {}
		//_Color("Color", Color) = (1,1,1,1)
		//_Size("Size", vector) = (1,1,1,0)
		//_SizeRandom("SizeRandom", vector) = (0,0,0,0)
		//_BodyColorAmount("BodyColorAmount", Range(0, 1.0)) = 0.0
		//_OrientAttitude("OrientAttitude", Range(0, 1.0)) = 1.0  // 1 = straight 'up' along vertex normal, 0 = 'flat' along shell tangent
		//_OrientForwardTangent("OrientForwardTangent", Range(-1.0, 1.0)) = 1.0   // 0 = random facing direction, 1 = aligned with shell forward tangent
		//_OrientRandom("OrientRandom", Range(0, 1.0)) = 0.0
		//_Type("Type", Int) = 0   // 0 = quad, 1 = scales, 2 = hair
		//_Segments("Segments", Int) = 1		
		//_InitRibbonPoints("InitRibbonPoints", Int) = 1

		_BrushTex("Brush Texture", 2D) = "white" {}
		_Tint("Color", Color) = (1,1,1,1)
		_Size("Size", vector) = (1,1,0,0)
		_PaintThickness("Paint Thickness", Range(0,1)) = 0.25
		_PaintReach("Paint Reach", Range(0,1)) = 0.1   // This specifies how 'far' down the canvasDepth the paint will be applied
													   // i.e. paintReach=1 means full coverage of the canvas, paintReach=0.1 means paint will only be applied where the canvasDepth is >0.9 normalized...
													   // This will likely change later
		_UseSourceColor("_UseSourceColor", Range(0,1)) = 0.0

		_Diffuse("Diffuse", Range(0.0, 1.0)) = 1
		_DiffuseWrap("DiffuseWrap", Range(0.0, 1.0)) = 0
		_RimGlow("RimGlow", Range(0.0, 1.0)) = 0.4
		_RimPow("RimPow", Range(0.1, 10.0)) = 0.5
	}
	SubShader{
		//Tags{ "Queue" = "Overlay+100" "RenderType" = "Transparent" }
		//Tags{ "RenderType" = "Opaque" }
		//LOD 100
		//Blend SrcAlpha OneMinusSrcAlpha
		//Cull off
		//ZWrite off
			// ^^^^ OLD
		Tags{ "RenderType" = "Transparent" }
		ZTest Off
		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		pass {
			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vert
			//#pragma geometry geom
			#pragma fragment frag

			//#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "Assets/Resources/Shaders/Inc/SimplexShared.cginc"

			//sampler2D _Sprite;
			//float4 _Color = float4(1, 0.5f, 0.0f, 1);
			//float3 _Size = float3(1, 1, 1);
			//float3 _SizeRandom = float3(0,0,0);
			//float _BodyColorAmount = 1.0;
			//float _OrientAttitude = 1.0;  // 1 = straight 'up' along vertex normal, 0 = 'flat' along shell tangent
			//float _OrientForwardTangent = 0.0;   // 0 = random facing direction, 1 = aligned with shell forward tangent
			//float _OrientRandom = 0.0;
			//int _Type = 0;   // 0 = quad, 1 = scales, 2 = hair
			//int _NumRibbonSegments = 1;			
			//int _StaticCylinderSpherical = 2;  // 0=static, 1=cylinder, 2=spherical			
			//int _InitRibbonPoints = 1;

			struct strokeData {
				float3 pos;
				float3 color;
				float3 normal;
				float3 tangent;
				float3 prevPos;
				float2 dimensions;
				int strokeType;
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

			/*struct fragInput {
			float4 pos : SV_POSITION;
			float3 norm : NORMAL;
			float2 uv : TEXCOORD0;
			float4 color : COLOR0;
			float3 viewDir : TEXCOORD2;
			int index : TEXCOORD3;
			//UNITY_FOG_COORDS(1)
			};*/
			struct v2f
			{
				float4 pos : SV_POSITION;				
				float3 normal : NORMAL;
				float2 localUV : TEXCOORD0;  // uv of the brushstroke quad itself, particle texture
				float2 screenUV : TEXCOORD1;  // uv in screenspace of the frag -- for sampling from renderBuffers
				float2 centerUV : TEXCOORD2;  // uv of just the centerPoint of the brushstroke, in screenspace so it can sample from colorBuffer
				float3 viewDir : TEXCOORD5;
				float4 uncolor : TEXCOORD3;
				int index : TEXCOORD4;
				//float4 blah : TEXCOORD5;
			};

			sampler2D _BrushTex;
			float4 _BrushTex_ST;
			float4 _Tint;
			float2 _Size;
			float _PaintThickness;
			float _PaintReach;
			float _UseSourceColor;
			uniform float _Diffuse;
			uniform float _DiffuseWrap;
			uniform float _RimGlow;
			uniform float _RimPow;
			
			StructuredBuffer<strokeData> strokeDataBuffer;
			StructuredBuffer<float3> quadPointsBuffer;
			StructuredBuffer<segmentTransforms> buf_xforms;
			StructuredBuffer<bindPoseStruct> buf_bindPoses;
			StructuredBuffer<skinningStruct> buf_skinningData;

			// Stores result of Unity's Rendering:
			sampler2D _BrushColorReadTex;
			sampler2D _CanvasColorReadTex;
			sampler2D _CanvasDepthReadTex;
			

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

			void CalculateSkinning(inout float4 pos, inout float3 normal, inout float3 tangent, int id) {
				
				//return float4(pos.xyz + buf_xforms[buf_skinningData[id].indices.x].pos * 1, 1);
				//buf_skinningData[id]
				float4 norm4 = float4(normal, 0);  // 0 for 'direction' rather than position
				float4 tan4 = float4(tangent, 0);  // 0 for 'direction' rather than position
				
				//Get position in local segment space for each Bone by using bindPose inverse Mat4x4:
				float4 localBonePos0 = mul(buf_bindPoses[buf_skinningData[id].indices.x].inverse, pos);
				float4 localBonePos1 = mul(buf_bindPoses[buf_skinningData[id].indices.y].inverse, pos);
				float4 localBoneNorm0 = mul(buf_bindPoses[buf_skinningData[id].indices.x].inverse, norm4);
				float4 localBoneNorm1 = mul(buf_bindPoses[buf_skinningData[id].indices.y].inverse, norm4);
				float4 localBoneTan0 = mul(buf_bindPoses[buf_skinningData[id].indices.x].inverse, tan4);
				float4 localBoneTan1 = mul(buf_bindPoses[buf_skinningData[id].indices.y].inverse, tan4);
				// Transform this bindPose position by the CURRENT segment position:
				float4 skinnedBonePos0 = mul(buf_xforms[buf_skinningData[id].indices.x].xform, localBonePos0);
				float4 skinnedBonePos1 = mul(buf_xforms[buf_skinningData[id].indices.y].xform, localBonePos1);
				float4 skinnedBoneNorm0 = mul(buf_xforms[buf_skinningData[id].indices.x].xform, localBoneNorm0);
				float4 skinnedBoneNorm1 = mul(buf_xforms[buf_skinningData[id].indices.y].xform, localBoneNorm1);
				float4 skinnedBoneTan0 = mul(buf_xforms[buf_skinningData[id].indices.x].xform, localBoneTan0);
				float4 skinnedBoneTan1 = mul(buf_xforms[buf_skinningData[id].indices.y].xform, localBoneTan1);
				// Combine positions:
				float4 skinnedPos = skinnedBonePos0 * buf_skinningData[id].weights.x + skinnedBonePos1 * buf_skinningData[id].weights.y;
				float4 skinnedNorm = skinnedBoneNorm0 * buf_skinningData[id].weights.x + skinnedBoneNorm1 * buf_skinningData[id].weights.y;
				float4 skinnedTan = skinnedBoneTan0 * buf_skinningData[id].weights.x + skinnedBoneTan1 * buf_skinningData[id].weights.y;
				pos = skinnedPos;
				normal = skinnedNorm.xyz;	
				tangent = skinnedTan.xyz;
			}

			/*fragInput vert(uint id : SV_VertexID) {
				fragInput o;
				o.index = id;
				o.pos = float4(buf_InitCritterBrushstrokeData[id].pos, 1.0f);
				o.norm = buf_InitCritterBrushstrokeData[id].normal;
				float3 lightDirection = float3(0.2, 1.0, 0.3);
				float3 diffuse = dot(o.norm, lightDirection);
				diffuse = diffuse * (1.0 - _DiffuseWrap * 0.5) + _DiffuseWrap * 0.5;
				o.color = lerp(_Color, float4(buf_InitCritterBrushstrokeData[id].color, _Color.a), _BodyColorAmount);
				o.color = lerp(o.color, o.color * float4(diffuse, o.color.a), _Diffuse);

				float3 camToWorldVector = _WorldSpaceCameraPos.xyz - o.pos.xyz;
				o.viewDir = normalize(camToWorldVector);
				return o;
			}*/

			v2f vert(uint id : SV_VertexID, uint inst : SV_InstanceID)
			{
				v2f o;
				
				o.index = inst;  // needed to sample brushstrokeData buffer in fragment shader
				
				float4 worldPosition = float4(strokeDataBuffer[inst].pos, 1.0);				
				float3 tempNormal = strokeDataBuffer[inst].normal;	
				float3 tempTangent = strokeDataBuffer[inst].tangent;
				
				CalculateSkinning(worldPosition, tempNormal, tempTangent, inst);
				
				o.normal = tempNormal;
				//o.tangent = tempTangent;
				float3 camToWorldVector = _WorldSpaceCameraPos.xyz - worldPosition.xyz;
				o.viewDir = normalize(camToWorldVector);
				float3 side = normalize(cross(o.viewDir, tempTangent));
				float3 forward = normalize(cross(o.viewDir, side));
				float3 quadPoint = quadPointsBuffer[id];				

				// Purely Camera-Facing:
				//o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, worldPosition) + float4(quadPoint * float3(_Size, 1.0), 0.0));
				//float3 vertexOffset = (right * quadPoint.x * _Size.x) + (up * quadPoint.y * _Size.y);
				//o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, worldPosition + float4(vertexOffset, 0.0)));
				
				//float3 vertexOffset = float3(quadPoint.x * _Size.x, quadPoint.y * _Size.y, 0.0);
				float3 vertexOffset = (forward * quadPoint.x * _Size.x * strokeDataBuffer[inst].dimensions.y) + (side * quadPoint.y * _Size.y * strokeDataBuffer[inst].dimensions.x);
				
				if (dot(o.viewDir, o.normal) > 0.0) {  // if normal is facing away from camera, make triangles degenerate so they won't render backfaces
					worldPosition.xyz += vertexOffset;
				}
				o.pos = mul(UNITY_MATRIX_VP, worldPosition);
				
				float4 screenUV = ComputeScreenPos(o.pos);
				o.screenUV = screenUV.xy / screenUV.w;

				// Magic to get proper UV's for sampling from GBuffers:
				float4 pos = mul(UNITY_MATRIX_VP, worldPosition);
				float4 centerUV = ComputeScreenPos(pos);
				o.centerUV = centerUV.xy / centerUV.w;

				//Shift coordinates for uvs
				o.localUV = quadPointsBuffer[id] + 0.5f;

				// COLOR:
				float3 lightDirection = float3(0.2, 1.0, 0.3);
				float3 diffuse = dot(o.normal, lightDirection);
				diffuse = diffuse * (1.0 - _DiffuseWrap * 0.5) + _DiffuseWrap * 0.5;
				o.uncolor = float4(strokeDataBuffer[inst].color.rgb, _Tint.a);
				o.uncolor = lerp(o.uncolor, o.uncolor * float4(diffuse, o.uncolor.a), _Diffuse);
				//o.blah = strokeDataBuffer[inst].color;
				//o.color = float4(strokeDataBuffer[inst].color.x, diffuse.yz, 1.0);
				//float4 broke = float4(strokeDataBuffer[inst].tint, 1);
				//o.viewDir = broke.xyz;
				//o.uncolor = broke;
				return o;
			}

			float4 RotPoint(float3 p, float3 offset, float3 sideVector, float3 upVector) {
				float3 finalPos = p;
				finalPos += offset.x * sideVector;
				finalPos += offset.y * upVector;
				finalPos += offset.z * cross(sideVector, upVector);

				return float4(finalPos, 1);
			}
			
			/*[maxvertexcount(4)]
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

				//uint elementsDeco;
				//uint strideDeco;
				//buf_decorations.GetDimensions(elementsDeco, strideDeco);
				
				float randomValue = simplex3d(p[0].pos.xyz * 128);	
				float3 randomDir = normalize(float3(simplex3d(p[0].pos.xyz * 100), simplex3d(p[0].pos.xyz * 200), simplex3d(p[0].pos.xyz * 300)));
				float3 halfS = _Size + (randomValue * _Size * _SizeRandom);
				
				float4 v[4];

				float4 rootPos = p[0].pos;
				float3 rootNorm = p[0].norm;
				float3 rootTan = buf_InitCritterBrushstrokeData[p[0].index].tangent;
				CalculateSkinning(rootPos, rootNorm, rootTan, p[0].index); // Get current position and orientation of root Vertex

				// Working in WORLD SPACE NOW!!!!!
				float3 tangent = rootTan;  // buf_decorations[p[0].index].tangent;
				if (_OrientForwardTangent < 0) {
					tangent = -tangent;
				}
				float3 up = lerp(rootNorm, tangent, abs(_OrientForwardTangent));
				up = lerp(up, randomDir, _OrientRandom);  // normalize(float3(0, 1, 0));				
				float3 look = -randomDir;  // float3(simplex3d(p[0].pos.xyz * 100), simplex3d(p[0].pos.xyz * 200), simplex3d(p[0].pos.xyz * 300));   //p[0].norm;  // _WorldSpaceCameraPos - p[0].pos.xyz;
				float3 right = normalize(cross(look, up));
				look = normalize(cross(right, up));
				
				fragInput pIn;
				pIn.index = p[0].index;
				pIn.norm = rootNorm; // p[0].norm;
				pIn.color = p[0].color;
				pIn.viewDir = p[0].viewDir;
				
				v[0] = rootPos + float4(-halfS.x * right + 0 * up, 0);
				v[1] = rootPos + float4(-halfS.x * right + halfS.y * 2.0 * up, 0);
				v[2] = rootPos + float4(halfS.x * right + 0 * up, 0);
				v[3] = rootPos + float4(halfS.x * right + halfS.y * 2.0 * up, 0);
								
				pIn.pos = v[0];
				//CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 0);
				//UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[1];
				//CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(0, 1);
				//UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[2];
				//CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 0);
				//UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);

				pIn.pos = v[3];
				//CalculateSkinning(pIn.pos, pIn.norm, p[0].index);
				pIn.pos = mul(UNITY_MATRIX_VP, pIn.pos);
				pIn.uv = float2(1, 1);
				//UNITY_TRANSFER_FOG(pIn, pIn.pos);
				triStream.Append(pIn);
				
				//triStream.RestartStrip();
			}

			float4 frag(fragInput i) : COLOR{
				float viewAngle = saturate(dot(i.norm, i.viewDir));
				float outlineStrength = (1.0 - pow(viewAngle, _RimPow));				
				
				fixed4 col = tex2D(_Sprite, i.uv) * i.color;
				//col.xyz += outlineStrength;
				col.xyz = _RimGlow * (col.xyz + outlineStrength) + (1.0 - _RimGlow) * col.xyz;
				//col.a = col.r * 0.3;

				//UNITY_APPLY_FOG(i.fogCoord, col); // apply fog

				return col;
			}*/

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
				//float3 lightDirection = float3(0.2, 1.0, 0.3);
				//float3 diffuse = dot(i.normal, lightDirection);
				//diffuse = diffuse * (1.0 - _DiffuseWrap * 0.5) + _DiffuseWrap * 0.5;
				//float4 temp = float4(strokeDataBuffer[i.index].tint.rgb, _Tint.a);
				//temp = lerp(temp, temp * float4(diffuse, temp.a), _Diffuse);
				float3 brushHue = lerp(i.uncolor * _Tint.rgb, buffer0.rgb * _Tint.rgb, _UseSourceColor);  // use own paint color or use SceneRenderColor?
				col = lerp(col, float4(brushHue, 1.0), value);

				//col.rgb = brushHue;
				col.a *= brush.x * _Tint.a;
				depth.a = 1.0;
				depth.rgb += brush.y * _PaintThickness;
				depth.a *= brush.x * _Tint.a;
				outDepth = depth;  // no change to depth for now...
				//col = i.uncolor;

				float viewAngle = saturate(dot(i.viewDir, i.normal));
				float outlineStrength = (1.0 - pow(viewAngle, _RimPow));
				//float4 col = 0.5 * (i.color + outlineStrength * 0.75) * (1.0 - viewAngle) + 0.5 * i.color;
				//float4 col = i.color * _Tint;
				col.xyz = _RimGlow * (col.xyz + outlineStrength) + (1.0 - _RimGlow) * col.xyz;

				outColor = col;
			}

			ENDCG
		}
	}
	FallBack Off
}
