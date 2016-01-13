using UnityEngine;
using System.Collections;

public class GamePieceCommonPlane : GamePiece {

	public override Mesh BuildMesh() {  // SIMPLE PLANE!
		MeshBuilder meshBuilder = new MeshBuilder();

		BuildQuad (meshBuilder, new Vector3(-0.5f, 0.0f, 0.5f), Vector3.back, Vector3.right); // TOP
		
		return meshBuilder.CreateMesh ();
	}
}
