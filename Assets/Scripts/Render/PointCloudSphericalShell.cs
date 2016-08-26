using UnityEngine;
using System.Collections;

public class PointCloudSphericalShell {

	public static Vector3[] GetPointsSphericalShell(float radius, int resolution, float randomness) {
        //int xSize, ySize, zSize;
        //int roundness;
        
        int cornerVertices = 8;
        int edgeVertices = (resolution + resolution + resolution - 3) * 4;
        int faceVertices = ((resolution - 1) * (resolution - 1) +
                            (resolution - 1) * (resolution - 1) +
                            (resolution - 1) * (resolution - 1)) * 2;
        Vector3[] vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        Vector3[] normals = new Vector3[vertices.Length];

        float maxRandomDrift = (radius / (float)resolution) * randomness; 

        int v = 0;
        for (int y = 0; y <= resolution; y++) {
            for (int x = 0; x <= resolution; x++) {
                SetVertex(vertices, normals, v++, x, y, 0, resolution, radius, maxRandomDrift);
            }
            for (int z = 1; z <= resolution; z++) {
                SetVertex(vertices, normals, v++, resolution, y, z, resolution, radius, maxRandomDrift);
            }
            for (int x = resolution - 1; x >= 0; x--) {
                SetVertex(vertices, normals, v++, x, y, resolution, resolution, radius, maxRandomDrift);
            }
            for (int z = resolution - 1; z > 0; z--) {
                SetVertex(vertices, normals, v++, 0, y, z, resolution, radius, maxRandomDrift);
            }
        }
        for (int z = 1; z < resolution; z++) {
            for (int x = 1; x < resolution; x++) {
                SetVertex(vertices, normals, v++, x, resolution, z, resolution, radius, maxRandomDrift);
            }
        }
        for (int z = 1; z < resolution; z++) {
            for (int x = 1; x < resolution; x++) {
                SetVertex(vertices, normals, v++, x, 0, z, resolution, radius, maxRandomDrift);
            }
        }


        return vertices;
    }

    public static void SetVertex(Vector3[] vertices, Vector3[] normals, int i, int x, int y, int z, int resolution, float radius, float maxRandomDrift) {

        Vector3 v = new Vector3(x, y, z) * 2f / resolution - Vector3.one;
        float x2 = v.x * v.x;
        float y2 = v.y * v.y;
        float z2 = v.z * v.z;
        Vector3 s;
        s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
        s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
        s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);
        normals[i] = s;
        vertices[i] = normals[i] * radius + UnityEngine.Random.insideUnitSphere * maxRandomDrift;

        /*Vector3 inner = vertices[i] = new Vector3(x, y, z);
        int roundness = resolution / 2;

        if (x < roundness) {
            inner.x = roundness;
        }
        else if (x > resolution - roundness) {
            inner.x = resolution - roundness;
        }
        if (y < roundness) {
            inner.y = roundness;
        }
        else if (y > resolution - roundness) {
            inner.y = resolution - roundness;
        }
        if (z < roundness) {
            inner.z = roundness;
        }
        else if (z > resolution - roundness) {
            inner.z = resolution - roundness;
        }

        normals[i] = (vertices[i] - inner).normalized;
        vertices[i] = inner + normals[i] * roundness;
        */
    }
}
