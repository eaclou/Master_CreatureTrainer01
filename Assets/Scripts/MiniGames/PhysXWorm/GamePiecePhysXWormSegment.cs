﻿using UnityEngine;
using System.Collections;

public class GamePiecePhysXWormSegment : GamePieceRigidBody {

	public override Mesh BuildMesh() {  // SIMPLE CUBE!
		MeshBuilder meshBuilder = new MeshBuilder();

		// OG:
		/*BuildQuad (meshBuilder, new Vector3(-0.5f, -0.5f, -0.5f), Vector3.right, Vector3.up); // FRONT
		BuildQuad (meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.back, Vector3.up); // LEFT
		BuildQuad (meshBuilder, new Vector3(-0.5f, 0.5f, 0.5f), Vector3.back, Vector3.right); // TOP
		BuildQuad (meshBuilder, new Vector3(0.5f, -0.5f, 0.5f), Vector3.left, Vector3.up); // BACK
		BuildQuad (meshBuilder, new Vector3(0.5f, -0.5f, -0.5f), Vector3.forward, Vector3.up); // RIGHT
		BuildQuad (meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.right, Vector3.back); // BOTTOM
		*/

		int m_HeightSegmentCount = 8;
		int m_RadialSegmentCount = 8;
		float m_Radius = 0.75f;
		float m_VerticalScale = 1f;
		Quaternion rotation = Quaternion.identity;
		Vector3 offset = new Vector3(0f, -0.75f, 0f);
		//the angle increment per height segment:
		float angleInc = Mathf.PI / m_HeightSegmentCount;
		
		//the vertical (scaled) radius of the sphere:
		float verticalRadius = m_Radius * m_VerticalScale;
		
		//build the rings:
		for (int i = 0; i <= m_HeightSegmentCount; i++)
		{
			Vector3 centrePos = Vector3.zero;
			
			//calculate a height offset and radius based on a vertical circle calculation:
			centrePos.y = -Mathf.Cos(angleInc * i);
			float radius = Mathf.Sin(angleInc * i);
			
			//calculate the slope of the shpere at this ring based on the height and radius:
			Vector2 slope = new Vector3(-centrePos.y / m_VerticalScale, radius);
			slope.Normalize();
			
			//multiply the unit height by the vertical radius, and then add the radius to the height to make this sphere originate from its base rather than its centre:
			centrePos.y = centrePos.y * verticalRadius + verticalRadius;
			
			//scale the radius by the one stored in the partData:
			radius *= m_Radius;
			
			//calculate the final position of the ring centre:
			Vector3 finalRingCentre = rotation * centrePos + offset;
			
			//V coordinate:
			float v = (float)i / m_HeightSegmentCount;
			
			//build the ring:
			BuildRing(meshBuilder, m_RadialSegmentCount, finalRingCentre, radius, v, i > 0, rotation, slope);
		}

		return meshBuilder.CreateMesh ();
	}
}
