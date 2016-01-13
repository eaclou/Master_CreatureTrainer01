using UnityEngine;
using System.Collections;

public class GamePieceSpaceshipBody : GamePieceRigidBody {

	float scaleX = 1f;
	float scaleY = 1f;
	float scaleZ = 1f;

	public void SetScale(float x, float y, float z) {
		scaleX = x;
		scaleY = y;
		scaleZ = z;
	}

	public override Mesh BuildMesh() {  // SIMPLE CUBE!
		MeshBuilder meshBuilder = new MeshBuilder();
		
		BuildQuad (meshBuilder, new Vector3(-scaleX * 0.5f, -scaleY * 0.5f, -scaleZ * 0.5f), Vector3.right * scaleX, Vector3.up * scaleY); // FRONT
		BuildQuad (meshBuilder, new Vector3(-scaleX * 0.5f, -scaleY * 0.5f, scaleZ * 0.5f), Vector3.back * scaleZ, Vector3.up * scaleY); // LEFT
		BuildQuad (meshBuilder, new Vector3(-scaleX * 0.5f, scaleY * 0.5f, scaleZ * 0.5f), Vector3.back * scaleZ, Vector3.right * scaleX); // TOP
		BuildQuad (meshBuilder, new Vector3(scaleX * 0.5f, -scaleY * 0.5f, scaleZ * 0.5f), Vector3.left * scaleX, Vector3.up * scaleY); // BACK
		BuildQuad (meshBuilder, new Vector3(scaleX * 0.5f, -scaleY * 0.5f, -scaleZ * 0.5f), Vector3.forward * scaleZ, Vector3.up * scaleY); // RIGHT
		BuildQuad (meshBuilder, new Vector3(-scaleX * 0.5f, -scaleY * 0.5f, scaleZ * 0.5f), Vector3.right * scaleX, Vector3.back * scaleZ); // BOTTOM
		
		return meshBuilder.CreateMesh ();
	}
}
