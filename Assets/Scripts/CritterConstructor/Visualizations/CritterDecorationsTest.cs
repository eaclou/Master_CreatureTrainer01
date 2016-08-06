using UnityEngine;
using System.Collections;

public class CritterDecorationsTest : MonoBehaviour {

    public Critter critter;
    public Shader geomShader;
    public Material procMaterial;

    public Texture2D sprite;
    public Vector2 size = Vector2.one;
    public Color color = new Color(1f, 0.6f, 0.3f, 0.03f);

    ComputeBuffer outputBuffer;
    ComputeBuffer segmentBuffer;
    
    [Range(0, 2)]
    [Tooltip("Billboard type 0 = static, 1 = cylindrical, 2 = spherical")]
    public int billboardType = 2;

    public SegmentXform[] critterSegmentXforms;
    public decoration[] points;
    
    public struct decoration {
        public Vector3 pos;
        public Vector3 normal;
    }

    public struct SegmentXform {
        // Position:
        public float PX, PY, PZ;
        // Rotation:
        public float RX, RY, RZ, RW;
        // Scale:
        public float SX, SY, SZ;

        public Matrix4x4 xform;
    }

    // Use this for initialization
    void Start() {
        if (procMaterial == null) {
            procMaterial = new Material(geomShader);
        }
    }

    public void InitializeBuffers(decoration[] decorationsArray) {
        
        points = decorationsArray;

        outputBuffer = new ComputeBuffer(points.Length, 24);
        outputBuffer.SetData(points);
                
    }

    public void UpdateXforms() {
        if(critter != null) {
            segmentBuffer = new ComputeBuffer(critter.critterSegmentList.Count, sizeof(float) * 26);
            critterSegmentXforms = new SegmentXform[critter.critterSegmentList.Count];  // grab numSegments from Critter

            for (int i = 0; i < critter.critterSegmentList.Count; i++) {
                SegmentXform segmentTransform;
                segmentTransform.PX = critter.critterSegmentList[i].transform.position.x;
                segmentTransform.PY = critter.critterSegmentList[i].transform.position.y;
                segmentTransform.PZ = critter.critterSegmentList[i].transform.position.z;
                segmentTransform.RX = critter.critterSegmentList[i].transform.rotation.x;
                segmentTransform.RY = critter.critterSegmentList[i].transform.rotation.y;
                segmentTransform.RZ = critter.critterSegmentList[i].transform.rotation.z;
                segmentTransform.RW = critter.critterSegmentList[i].transform.rotation.w;
                segmentTransform.SX = critter.critterSegmentList[i].transform.localScale.x / 2f;
                segmentTransform.SY = critter.critterSegmentList[i].transform.localScale.y / 2f;
                segmentTransform.SZ = critter.critterSegmentList[i].transform.localScale.z / 2f;
                segmentTransform.xform = Matrix4x4.TRS(critter.critterSegmentList[i].transform.position, critter.critterSegmentList[i].transform.rotation, critter.critterSegmentList[i].transform.localScale);
                critterSegmentXforms[i] = segmentTransform;
            }

            segmentBuffer.SetData(critterSegmentXforms);
            procMaterial.SetBuffer("buf_xforms", segmentBuffer);
        }        
    }

    void OnRenderObject() {
        if(outputBuffer != null) {
            UpdateXforms();

            procMaterial.SetPass(0);
            procMaterial.SetColor("_Color", color);
            procMaterial.SetBuffer("buf_decorations", outputBuffer);
            procMaterial.SetTexture("_Sprite", sprite);
            procMaterial.SetVector("_Size", size);
            procMaterial.SetInt("_StaticCylinderSpherical", billboardType);
            procMaterial.SetVector("_worldPos", transform.position);

            Graphics.DrawProcedural(MeshTopology.Points, outputBuffer.count);
            //Debug.Log("DrawProcedural! " + material.GetVector("_Size").ToString());
        }
    }

    void OnDestroy() {
        outputBuffer.Release();
    }
}
