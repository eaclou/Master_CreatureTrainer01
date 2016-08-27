using UnityEngine;
using System.Collections;

public class TrainerCritterBrushstrokeManager : MonoBehaviour {

    public Critter critter;
    public ComputeShader skinningComputeShader;
    //public Shader geomShader;
    //public Material procMaterial;

    public Texture2D sprite;
    public Vector3 size = Vector3.one;
    public Vector3 sizeRandom = Vector3.zero;
    public Color color = new Color(1f, 1f, 1f, 1f);
    public float bodyColorAmount = 1f;
    //public float orientAttitude = 1f;  // 1 = straight 'up' along vertex normal, 0 = 'flat' along shell tangent
    public float orientForwardTangent = 0f;   // 0 = random facing direction, 1 = aligned with shell forward tangent
    public float orientRandom = 0f;
    public int type = 0;   // 0 = quad, 1 = scales, 2 = hair
    public int numRibbonSegments = 1;
    public float diffuse;
    public float diffuseWrap;
    public float rimGlow;
    public float rimPow;

    ComputeBuffer initPositionsBuffer;  // original positions of each decoration anchor
    ComputeBuffer segmentBuffer;  // critter segment XForms
    ComputeBuffer bindPoseBuffer;    // critter segment inverse xForm bindpose matrices 
    ComputeBuffer skinningBuffer;    // skinning data for each point

    public SegmentXform[] critterSegmentXforms;
    bindPoseStruct[] bindPosesArray;
    //public decorationStruct[] decorations;
    public TrainerRenderManager.strokeStruct[] critterBrushstrokesArray;


    public bool isOn = false;

    [Range(0, 2)]
    [Tooltip("Billboard type 0 = static, 1 = cylindrical, 2 = spherical")]
    public int billboardType = 2;

    

    //public struct decorationStruct {
    //    public Vector3 pos;
    //    public Vector3 normal;
    //    public Vector3 tangent;
    //    public Vector3 color;
    //}    

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
    
    // Use this for initialization
    void Start() {
        
    }

    void ReleaseBuffers() {
        initPositionsBuffer.Release();
        segmentBuffer.Release();
        bindPoseBuffer.Release();
        skinningBuffer.Release();

        initPositionsBuffer.Dispose();
        segmentBuffer.Dispose();
        bindPoseBuffer.Dispose();
        skinningBuffer.Dispose();
    }

    public void TurnOn(TrainerRenderManager.strokeStruct[] brushstrokeArray) {

        InitializeBuffers(brushstrokeArray);

        isOn = true;
    }

    public void TurnOff() {
        isOn = false;
        ReleaseBuffers();
    }

    public void InitializeBuffers(TrainerRenderManager.strokeStruct[] brushstrokeArray) {

        critterBrushstrokesArray = brushstrokeArray; // ???   This Array actually 'lives' inside TrainerCritterMarchingCubes instance...     

        initPositionsBuffer = new ComputeBuffer(critterBrushstrokesArray.Length, 12);
        initPositionsBuffer.SetData(critterBrushstrokesArray);
        //procMaterial.SetBuffer("buf_decorations", outputBuffer);
        //procMaterial.SetColor("_Color", color);
        //procMaterial.SetTexture("_Sprite", sprite);
        //procMaterial.SetVector("_Size", size);
        //procMaterial.SetVector("_SizeRandom", sizeRandom);
        //procMaterial.SetFloat("_BodyColorAmount", bodyColorAmount);
        //procMaterial.SetFloat("_OrientForwardTangent", orientForwardTangent);
        //procMaterial.SetFloat("_OrientRandom", orientRandom);
        //procMaterial.SetInt("_Type", type);
        //procMaterial.SetInt("_NumRibbonSegments", numRibbonSegments);
        //procMaterial.SetInt("_StaticCylinderSpherical", billboardType);
        
        int kernelID = skinningComputeShader.FindKernel("CSMain");
        skinningComputeShader.SetBuffer(kernelID, "buf_CritterBrushstrokeData", initPositionsBuffer);
        
        skinningBuffer = new ComputeBuffer(initPositionsBuffer.count, sizeof(float) * 2 + sizeof(int) * 2);

        bindPosesArray = new bindPoseStruct[critter.critterSegmentList.Count];
        bindPoseBuffer = new ComputeBuffer(critter.critterSegmentList.Count, sizeof(float) * 16);

        segmentBuffer = new ComputeBuffer(critter.critterSegmentList.Count, sizeof(float) * 26);
        critterSegmentXforms = new SegmentXform[critter.critterSegmentList.Count];  // grab numSegments from Critter

        InitBindPose();
    }

    // Need to set material buffers that only need to be set once at the beginning (all except segmentX-Forms)
    public void InitializeMaterialBuffers(ref Material brushstrokeCritterMaterialRef) {
        brushstrokeCritterMaterialRef.SetBuffer("strokeDataBuffer", initPositionsBuffer);
        //procMaterial.SetColor("_Color", color);
        //procMaterial.SetTexture("_Sprite", sprite);
        //procMaterial.SetVector("_Size", size);
        //procMaterial.SetVector("_SizeRandom", sizeRandom);
        //procMaterial.SetFloat("_BodyColorAmount", bodyColorAmount);
        //procMaterial.SetFloat("_OrientForwardTangent", orientForwardTangent);
        //procMaterial.SetFloat("_OrientRandom", orientRandom);
        //procMaterial.SetInt("_Type", type);
        //procMaterial.SetInt("_NumRibbonSegments", numRibbonSegments);
        //procMaterial.SetInt("_StaticCylinderSpherical", billboardType);
        brushstrokeCritterMaterialRef.SetBuffer("buf_skinningData", skinningBuffer);  // Link same buffer to display Shader!
        brushstrokeCritterMaterialRef.SetBuffer("buf_bindPoses", bindPoseBuffer);  // set buffer within display Shader to same ComputeBuffer  (then just pray it works)
        brushstrokeCritterMaterialRef.SetBuffer("buf_xforms", segmentBuffer);
    }

    public void UpdateBuffersAndMaterial(ref Material brushstrokeCritterMaterialRef) {
        if (initPositionsBuffer != null && isOn) {
            UpdateXforms();

            brushstrokeCritterMaterialRef.SetPass(0);
            brushstrokeCritterMaterialRef.SetBuffer("buf_xforms", segmentBuffer);
            //brushstrokeCritterMaterialRef.SetColor("_Color", color);
            //brushstrokeCritterMaterialRef.SetTexture("_Sprite", sprite);
            //brushstrokeCritterMaterialRef.SetVector("_Size", size);
            //brushstrokeCritterMaterialRef.SetInt("_StaticCylinderSpherical", billboardType);
            //brushstrokeCritterMaterialRef.SetVector("_SizeRandom", sizeRandom);
            //brushstrokeCritterMaterialRef.SetFloat("_BodyColorAmount", bodyColorAmount);
            //brushstrokeCritterMaterialRef.SetFloat("_OrientForwardTangent", orientForwardTangent);
            //brushstrokeCritterMaterialRef.SetFloat("_OrientRandom", orientRandom);
            //brushstrokeCritterMaterialRef.SetFloat("_Diffuse", diffuse);
            //brushstrokeCritterMaterialRef.SetFloat("_DiffuseWrap", diffuseWrap);
            //brushstrokeCritterMaterialRef.SetFloat("_RimGlow", rimGlow);
            //brushstrokeCritterMaterialRef.SetFloat("_RimPow", rimPow);

            //Graphics.DrawProcedural(MeshTopology.Points, outputBuffer.count);            
            //Debug.Log("DrawProcedural! " + material.GetVector("_Size").ToString());
        }
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

        //procMaterial.SetBuffer("buf_skinningData", skinningBuffer);  // Link same buffer to display Shader!
        //procMaterial.SetBuffer("buf_bindPoses", bindPoseBuffer);  // set buffer within display Shader to same ComputeBuffer  (then just pray it works)

        skinningComputeShader.Dispatch(kernelID, 256, 1, 1);
    }

    public void UpdateXforms() {
        if (critter != null) {
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
            //procMaterial.SetBuffer("buf_xforms", segmentBuffer);
        }
    }

    void OnRenderObject() {
        /*if (outputBuffer != null && isOn) {
            UpdateXforms();

            procMaterial.SetPass(0);
            procMaterial.SetColor("_Color", color);
            procMaterial.SetTexture("_Sprite", sprite);
            procMaterial.SetVector("_Size", size);
            procMaterial.SetInt("_StaticCylinderSpherical", billboardType);
            procMaterial.SetVector("_SizeRandom", sizeRandom);
            procMaterial.SetFloat("_BodyColorAmount", bodyColorAmount);
            procMaterial.SetFloat("_OrientForwardTangent", orientForwardTangent);
            procMaterial.SetFloat("_OrientRandom", orientRandom);
            procMaterial.SetFloat("_Diffuse", diffuse);
            procMaterial.SetFloat("_DiffuseWrap", diffuseWrap);
            procMaterial.SetFloat("_RimGlow", rimGlow);
            procMaterial.SetFloat("_RimPow", rimPow);

            Graphics.DrawProcedural(MeshTopology.Points, outputBuffer.count);
            if (procMaterial.GetInt("_InitRibbonPoints") == 1) {
                procMaterial.SetInt("_InitRibbonPoints", 0);
            }
            //Debug.Log("DrawProcedural! " + material.GetVector("_Size").ToString());
        }*/
    }

    void OnDestroy() {
        ReleaseBuffers();
    }
}