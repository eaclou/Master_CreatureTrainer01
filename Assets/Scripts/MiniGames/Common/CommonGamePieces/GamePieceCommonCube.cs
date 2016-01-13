﻿using UnityEngine;
using System.Collections;

public class GamePieceCommonCube : GamePiece {

	public override Mesh BuildMesh() {  // SIMPLE CUBE!
		MeshBuilder meshBuilder = new MeshBuilder();
		
		BuildQuad (meshBuilder, new Vector3(-0.5f, -0.5f, -0.5f), Vector3.right, Vector3.up); // FRONT
		BuildQuad (meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.back, Vector3.up); // LEFT
		BuildQuad (meshBuilder, new Vector3(-0.5f, 0.5f, 0.5f), Vector3.back, Vector3.right); // TOP
		BuildQuad (meshBuilder, new Vector3(0.5f, -0.5f, 0.5f), Vector3.left, Vector3.up); // BACK
		BuildQuad (meshBuilder, new Vector3(0.5f, -0.5f, -0.5f), Vector3.forward, Vector3.up); // RIGHT
		BuildQuad (meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.right, Vector3.back); // BOTTOM
		
		return meshBuilder.CreateMesh ();
	}
}
