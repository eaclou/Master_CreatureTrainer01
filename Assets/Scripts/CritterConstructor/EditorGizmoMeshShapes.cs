using UnityEngine;
using System.Collections;

public class EditorGizmoMeshShapes {

    public EditorGizmoMeshShapes() {
        // Constructor
    }

    public static Mesh GetCubeMesh() {  // SIMPLE CUBE!
        MeshBuilder meshBuilder = new MeshBuilder();

        BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, -0.5f), Vector3.right, Vector3.up); // FRONT
        BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.back, Vector3.up); // LEFT
        BuildQuad(meshBuilder, new Vector3(-0.5f, 0.5f, 0.5f), Vector3.back, Vector3.right); // TOP
        BuildQuad(meshBuilder, new Vector3(0.5f, -0.5f, 0.5f), Vector3.left, Vector3.up); // BACK
        BuildQuad(meshBuilder, new Vector3(0.5f, -0.5f, -0.5f), Vector3.forward, Vector3.up); // RIGHT
        BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.right, Vector3.back); // BOTTOM

        return meshBuilder.CreateMesh();
    }

    /// <summary>
    /// Builds a single quad based on a position offset and width and length vectors.
    /// </summary>
    /// <param name="meshBuilder">The mesh builder currently being added to.</param>
    /// <param name="offset">A position offset for the quad.</param>
    /// <param name="widthDir">The width vector of the quad.</param>
    /// <param name="lengthDir">The length vector of the quad.</param>
    private static void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir) {
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        meshBuilder.Vertices.Add(offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(normal);

        //we don't know how many verts the meshBuilder is up to, but we only care about the four we just added:
        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }
}
