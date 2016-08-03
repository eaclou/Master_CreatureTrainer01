using UnityEngine;
using System.Collections;

public class TestSegmentMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MeshBuilder meshBuilder = new MeshBuilder();

        //BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, -0.5f), Vector3.right, Vector3.up); // FRONT
        //BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.back, Vector3.up); // LEFT
        //BuildQuad(meshBuilder, new Vector3(-0.5f, 0.5f, 0.5f), Vector3.back, Vector3.right); // TOP
        //BuildQuad(meshBuilder, new Vector3(0.5f, -0.5f, 0.5f), Vector3.left, Vector3.up); // BACK
        //BuildQuad(meshBuilder, new Vector3(0.5f, -0.5f, -0.5f), Vector3.forward, Vector3.up); // RIGHT
        //BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.right, Vector3.back); // BOTTOM

        this.GetComponent<MeshFilter>().sharedMesh = meshBuilder.CreateMesh();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void BuildTesselatedCube(MeshBuilder meshBuilder, float sizeX, float sizeY, float sizeZ, int xSegments, int ySegments, int zSegments) {
        
    }

    /*public static void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, float width, float length) {
        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, length) + offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(width, 0.0f, length) + offset);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);

        meshBuilder.Vertices.Add(new Vector3(width, 0.0f, 0.0f) + offset);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);

        //we don't know how many verts the meshBuilder is up to, but we only care about the four we just added:
        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }

    public static void BuildQuadForGrid(MeshBuilder meshBuilder, Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow, Vector3 normal) {
        meshBuilder.Vertices.Add(position);
        meshBuilder.UVs.Add(uv);
        meshBuilder.Normals.Add(normal);

        if (buildTriangles) {
            int baseIndex = meshBuilder.Vertices.Count - 1;

            int index0 = baseIndex;
            int index1 = baseIndex - 1;
            int index2 = baseIndex - vertsPerRow;
            int index3 = baseIndex - vertsPerRow - 1;

            meshBuilder.AddTriangle(index0, index2, index1);
            meshBuilder.AddTriangle(index2, index3, index1);
        }
    }
    */
}
