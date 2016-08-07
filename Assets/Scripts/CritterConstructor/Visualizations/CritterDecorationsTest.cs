using UnityEngine;
using System.Collections;

public class CritterDecorationsTest : MonoBehaviour {

    public Critter critter;
    public ComputeShader skinningComputeShader;
    public ComputeShader debugComputeShader;
    public Shader geomShader;
    public Material procMaterial;

    public Texture2D sprite;
    public Vector2 size = Vector2.one;
    public Color color = new Color(1f, 0.6f, 0.3f, 0.03f);

    ComputeBuffer outputBuffer;  // original positions of each decoration anchor
    ComputeBuffer segmentBuffer;

    ComputeBuffer bindPoseBuffer;
    bindPoseStruct[] bindPosesArray;
    ComputeBuffer skinningBuffer;

    ComputeBuffer indexBuffer;
    ComputeBuffer debugBuffer;

    public bool isOn = false;

    [Range(0, 2)]
    [Tooltip("Billboard type 0 = static, 1 = cylindrical, 2 = spherical")]
    public int billboardType = 2;

    public SegmentXform[] critterSegmentXforms;
    public decorationStruct[] decorations;
    
    public struct decorationStruct {
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

    public struct bindPoseStruct {
        public Matrix4x4 bindPoseMatrix;
    }
    
    public struct skinningStruct {  // Temp for debug purposes:
        public int index0, index1;
        public float weight0, weight1;
    }

    public struct intStruct {
        public int id;
    }

    // Use this for initialization
    void Start() {
        if (procMaterial == null) {
            procMaterial = new Material(geomShader);
        }
    }

    void ReleaseBuffers() {
        outputBuffer.Release();
        segmentBuffer.Release();
        bindPoseBuffer.Release();
        skinningBuffer.Release();
        indexBuffer.Release();
        debugBuffer.Release();

        outputBuffer.Dispose();
        segmentBuffer.Dispose();
        bindPoseBuffer.Dispose();
        skinningBuffer.Dispose();
        indexBuffer.Dispose();
        debugBuffer.Dispose();
    }

    public void TurnOn(decorationStruct[] decorationsArray) {

        InitializeBuffers(decorationsArray);

        isOn = true;
    }

    public void TurnOff() {
        isOn = false;
        ReleaseBuffers();
    }

    public void InitializeBuffers(decorationStruct[] decorationsArray) {
        
        decorations = decorationsArray; // ???        
        
        outputBuffer = new ComputeBuffer(decorations.Length, 24);
        outputBuffer.SetData(decorations);
        procMaterial.SetBuffer("buf_decorations", outputBuffer);
        int kernelID = skinningComputeShader.FindKernel("CSMain");
        skinningComputeShader.SetBuffer(kernelID, "buf_DecorationData", outputBuffer);

        indexBuffer = new ComputeBuffer(outputBuffer.count, sizeof(int));
        debugBuffer = new ComputeBuffer(outputBuffer.count, sizeof(int));

        skinningBuffer = new ComputeBuffer(outputBuffer.count, sizeof(float) * 2 + sizeof(int) * 2);

        bindPosesArray = new bindPoseStruct[critter.critterSegmentList.Count];
        bindPoseBuffer = new ComputeBuffer(critter.critterSegmentList.Count, sizeof(float) * 16);

        segmentBuffer = new ComputeBuffer(critter.critterSegmentList.Count, sizeof(float) * 26);
        critterSegmentXforms = new SegmentXform[critter.critterSegmentList.Count];  // grab numSegments from Critter

        InitBindPose();
    }

    public void InitBindPose() {
        UpdateXforms();

        for (int i = 0; i < critter.critterSegmentList.Count; i++) {            
            bindPosesArray[i].bindPoseMatrix = Matrix4x4.Inverse(Matrix4x4.TRS(critter.critterSegmentList[i].transform.position, critter.critterSegmentList[i].transform.rotation, critter.critterSegmentList[i].transform.localScale));
        }        
        bindPoseBuffer.SetData(bindPosesArray);  // set ComputeBuffer to inverse Mat4x4 data:        

        int kernelID = skinningComputeShader.FindKernel("CSMain");  // Get Kernel        
        skinningComputeShader.SetBuffer(kernelID, "buf_SkinningData", skinningBuffer);  // Don't need to set skinning buffer on CPU, but want to debugLog values:
        skinningComputeShader.SetBuffer(kernelID, "buf_SegmentData", segmentBuffer);        
        skinningComputeShader.SetBuffer(kernelID, "buf_IndexData", indexBuffer);

        procMaterial.SetBuffer("buf_skinningData", skinningBuffer);  // Link same buffer to display Shader!
        procMaterial.SetBuffer("buf_bindPoses", bindPoseBuffer);  // set buffer within display Shader to same ComputeBuffer  (then just pray it works)

        skinningComputeShader.Dispatch(kernelID, 256, 1, 1);
    }

    public void UpdateXforms() {
        if(critter != null) {            
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
        if(outputBuffer != null && isOn) {
            UpdateXforms();

            procMaterial.SetPass(0);
            procMaterial.SetColor("_Color", color);
            //procMaterial.SetBuffer("buf_decorations", outputBuffer);
            //procMaterial.SetBuffer("buf_xforms", segmentBuffer);   // THIS IS SET WITHIN THE UPDATE XFORMS METHIOD!!
            //segmentBuffer.GetData(critterSegmentXforms);
            //for (int i = 0; i < segmentBuffer.count; i++) {
                //Debug.Log("Index: " + i.ToString() + ": " + critterSegmentXforms[i].PY.ToString());
                //Debug.Log("Index: " + indexArray[i].id.ToString() + " SkinningData! " + debugSkinningArray[i].index0.ToString() + ", " + debugSkinningArray[i].index1.ToString() + ", " + debugSkinningArray[i].weight0.ToString() + ", " + debugSkinningArray[i].weight1.ToString());
            //}
            procMaterial.SetTexture("_Sprite", sprite);
            procMaterial.SetVector("_Size", size);
            procMaterial.SetInt("_StaticCylinderSpherical", billboardType);
            procMaterial.SetVector("_worldPos", transform.position);

            Graphics.DrawProcedural(MeshTopology.Points, outputBuffer.count);
            //Debug.Log("DrawProcedural! " + material.GetVector("_Size").ToString());
        }
    }

    void OnDestroy() {
        ReleaseBuffers();
    }
}
