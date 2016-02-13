using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EditorGizmoObject : MonoBehaviour {

    public bool meshBuilt = false;
    public Collider collider;
    public Material gizmoMaterial;
    
    public enum GizmoType {
        none,
        axisX,
        axisY,
        axisZ,
        axisAll
    };
    public GizmoType gizmoType = GizmoType.none;

    public enum GizmoMeshShape {
        None,
        Cube,
        Sphere,
        Capsule,
        Arrow,
        OmniArrow
    };

    //public virtual Mesh BuildMesh() {
    //    return new Mesh();
    //}
    public Mesh BuildMesh(GizmoMeshShape shape, GizmoType type) {  // SIMPLE CUBE!
        MeshBuilder meshBuilder = new MeshBuilder();

        if(shape == GizmoMeshShape.Cube) {
            meshBuilder = EditorGizmoMeshShapes.GetCubeMesh(meshBuilder);
            //BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, -0.5f), Vector3.right, Vector3.up); // FRONT
            //BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.back, Vector3.up); // LEFT
            //BuildQuad(meshBuilder, new Vector3(-0.5f, 0.5f, 0.5f), Vector3.back, Vector3.right); // TOP
            //BuildQuad(meshBuilder, new Vector3(0.5f, -0.5f, 0.5f), Vector3.left, Vector3.up); // BACK
            //BuildQuad(meshBuilder, new Vector3(0.5f, -0.5f, -0.5f), Vector3.forward, Vector3.up); // RIGHT
            //BuildQuad(meshBuilder, new Vector3(-0.5f, -0.5f, 0.5f), Vector3.right, Vector3.back); // BOTTOM

            if(type != GizmoType.none) {
                collider = this.gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }
            
            if (gizmoMaterial == null) {
                gizmoMaterial = new Material(Shader.Find("Custom/CritterEditorGizmo"));
                //gizmoMaterial.renderQueue = 4000;
            }
            GetComponent<MeshRenderer>().material = gizmoMaterial;
        }
        else if(shape == GizmoMeshShape.Arrow) {
            EditorGizmoMeshShapes.GetArrowMesh(meshBuilder);

            if (type != GizmoType.none) {
                MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshBuilder.CreateMesh();
                collider = meshCollider;
                //collider.isTrigger = true;
            }

            if (gizmoMaterial == null) {
                gizmoMaterial = new Material(Shader.Find("Custom/CritterEditorGizmo"));
                //gizmoMaterial.renderQueue = 4000;
            }
            GetComponent<MeshRenderer>().material = gizmoMaterial;
        }
        else if (shape == GizmoMeshShape.OmniArrow) {
            EditorGizmoMeshShapes.GetOmniArrowMesh(meshBuilder);

            if (type != GizmoType.none) {
                MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshBuilder.CreateMesh();
                collider = meshCollider;
            }

            if (gizmoMaterial == null) {
                gizmoMaterial = new Material(Shader.Find("Custom/CritterEditorGizmo"));
                //gizmoMaterial.renderQueue = 4000;
            }
            GetComponent<MeshRenderer>().material = gizmoMaterial;
        }
        else {
            Debug.Log("No Gizmo Shape!!!");
        }

        return meshBuilder.CreateMesh();
    }

    public void CreateMesh(GizmoMeshShape shape, GizmoType type) {
        if (meshBuilt == false) {
            Mesh mesh = BuildMesh(shape, type);

            MeshFilter filter = GetComponent<MeshFilter>();

            if (filter != null) {
                filter.sharedMesh = mesh;
            }
            meshBuilt = true;                  
        }
    }

    #region mesh primitive functions:

    /// <summary>
    /// Builds a single quad in the XZ plane, facing up the Y axis.
    /// </summary>
    /// <param name="meshBuilder">The mesh builder currently being added to.</param>
    /// <param name="offset">A position offset for the quad.</param>
    /// <param name="width">The width of the quad.</param>
    /// <param name="length">The length of the quad.</param>
    protected void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, float width, float length) {
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

    /// <summary>
    /// Builds a single quad based on a position offset and width and length vectors.
    /// </summary>
    /// <param name="meshBuilder">The mesh builder currently being added to.</param>
    /// <param name="offset">A position offset for the quad.</param>
    /// <param name="widthDir">The width vector of the quad.</param>
    /// <param name="lengthDir">The length vector of the quad.</param>
    protected void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir) {
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

    /// <summary>
    /// Builds a single triangle.
    /// </summary>
    /// <param name="meshBuilder">The mesh builder currently being added to.</param>
    /// <param name="corner0">The vertex position at index 0 of the triangle.</param>
    /// <param name="corner1">The vertex position at index 1 of the triangle.</param>
    /// <param name="corner2">The vertex position at index 2 of the triangle.</param>
    protected void BuildTriangle(MeshBuilder meshBuilder, Vector3 corner0, Vector3 corner1, Vector3 corner2) {
        Vector3 normal = Vector3.Cross((corner1 - corner0), (corner2 - corner0)).normalized;

        meshBuilder.Vertices.Add(corner0);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(corner1);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(corner2);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        int baseIndex = meshBuilder.Vertices.Count - 3;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
    }

   
    

    #endregion mesh primitive functions

}
