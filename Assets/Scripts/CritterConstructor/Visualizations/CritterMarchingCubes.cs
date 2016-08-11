using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterMarchingCubes : MonoBehaviour {

    public ComputeShader CShaderBuildMC;
    public ComputeShader CShaderSimplex;
    public CritterDecorationsTest critterDecorationsTest;

    private int resolutionX = 8;
    private int resolutionY = 8;
    private int resolutionZ = 8;
    
    private int _MaxBufferSize = 21660;
    private int _Trilinear = 1;
    private int _MultiSampling = 1;
    private float _Scale = 1f;
    private float cellResolution = 1f;
    private Vector3 GlobalBoundingBoxDimensions = new Vector3(16f, 8f, 8f);
    private Vector3 GlobalBoundingBoxOffset = new Vector3(0f, 0f, 0f);

    public Color colorPrimary = new Color(1, 1, 1, 1);
    public Color colorSecondary = new Color(0, 0, 0, 1);
    public float colorNoiseScale = 1f;
    public float colorSmlAmplitude = 1f;
    public float colorMedAmplitude = 1f;
    public float colorLrgAmplitude = 1f;
    public float colorContrast = 0f;
    public float colorThreshold = 0.5f;
    public Vector3 skinNoiseScale = Vector3.one;
    public float skinNoiseAmplitude = 1f;
    public Vector3 skinLocalTaper = Vector3.zero;
    public Vector3 skinLocalSinFreq = Vector3.one;
    public Vector3 skinLocalSinAmp = Vector3.one;
    // Local Segment-space modifications, sin, taper, etc.

    SegmentTransform[] critterSegmentTransforms;

    private Critter critter;
    
    public struct Poly {
        //Vertex A:
        public float A1, A2, A3;
        //Vertex B:
        public float B1, B2, B3;
        //Vertex C:
        public float C1, C2, C3;

        //Normals:
        public float NA1, NA2, NA3;
        public float NB1, NB2, NB3;
        public float NC1, NC2, NC3;
        // COLORS!! The colors!!
        public float CAR, CAG, CAB;
        public float CBR, CBG, CBB;
        public float CCR, CCG, CCB;
        // Boner weights:
        public int BoneIndexA0, BoneIndexA1;
        public float BoneWeightA0, BoneWeightA1;
        public int BoneIndexB0, BoneIndexB1;
        public float BoneWeightB0, BoneWeightB1;
        public int BoneIndexC0, BoneIndexC1;
        public float BoneWeightC0, BoneWeightC1;
    }

    public struct DecorationData {
        public Vector3 pos;
    }

    public struct SegmentTransform {
        // Position:
        public float PX, PY, PZ;
        // Rotation:
        public float RX, RY, RZ, RW;
        // Scale:
        public float SX, SY, SZ;
    }

    // Use this for initialization
    void Start () {
        

    }

    public void SetCritterTransformArray(Critter sourceCritter) {
        critter = sourceCritter;
        critterDecorationsTest.critter = sourceCritter;

        critterSegmentTransforms = new SegmentTransform[sourceCritter.critterSegmentList.Count];  // grab numSegments from Critter

        for(int i = 0; i < sourceCritter.critterSegmentList.Count; i++) {
            SegmentTransform segmentTransform;
            segmentTransform.PX = sourceCritter.critterSegmentList[i].transform.position.x;
            segmentTransform.PY = sourceCritter.critterSegmentList[i].transform.position.y;
            segmentTransform.PZ = sourceCritter.critterSegmentList[i].transform.position.z;
            segmentTransform.RX = sourceCritter.critterSegmentList[i].transform.rotation.x;
            segmentTransform.RY = sourceCritter.critterSegmentList[i].transform.rotation.y;
            segmentTransform.RZ = sourceCritter.critterSegmentList[i].transform.rotation.z;
            segmentTransform.RW = sourceCritter.critterSegmentList[i].transform.rotation.w;
            segmentTransform.SX = sourceCritter.critterSegmentList[i].transform.localScale.x / 2f;
            segmentTransform.SY = sourceCritter.critterSegmentList[i].transform.localScale.y / 2f;
            segmentTransform.SZ = sourceCritter.critterSegmentList[i].transform.localScale.z / 2f;
            critterSegmentTransforms[i] = segmentTransform;
        }
        //Debug.Log("SetCritterTransformArray numSegments: " + critterSegmentTransforms.Length + ", BoundingBox: " + sourceCritter.BoundingBoxMinCorner.ToString() + " -> " + sourceCritter.BoundingBoxMaxCorner.ToString());

        // Largest boundingBox dimension determines cellResolution?
        GlobalBoundingBoxDimensions = (sourceCritter.BoundingBoxMaxCorner - sourceCritter.BoundingBoxMinCorner) * 1.15f;  // buffer amount
        GlobalBoundingBoxOffset = (sourceCritter.BoundingBoxMaxCorner + sourceCritter.BoundingBoxMinCorner) / 2f;        
        int approxChunksPerDimension = 5;
        float avgRadius = (GlobalBoundingBoxDimensions.x + GlobalBoundingBoxDimensions.y + GlobalBoundingBoxDimensions.z) / 3f;
        float chunkSize = avgRadius / (float)approxChunksPerDimension;
        float cellSize = chunkSize / 8f;
        cellResolution = cellSize;        
    }

    public void ClearCritterMesh() {

        this.GetComponent<SkinnedMeshRenderer>().enabled = false;
        critterDecorationsTest.TurnOff();
    }
    
    public void BuildMesh() {
        float startTime = Time.realtimeSinceStartup;
        
        // NOISE VOLUME!
        RenderTexture DensityVolume = new RenderTexture(16, 16, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.sRGB);
        DensityVolume.volumeDepth = 16;
        DensityVolume.isVolume = true;
        DensityVolume.enableRandomWrite = true;
        DensityVolume.filterMode = FilterMode.Bilinear;
        DensityVolume.wrapMode = TextureWrapMode.Repeat;
        DensityVolume.Create();
        int mgen_id = CShaderSimplex.FindKernel("FillEmpty");
        // uses renderTexture rather than StructuredBuffer?
        CShaderSimplex.SetTexture(mgen_id, "Result", DensityVolume);  // Links RenderTexture to the "Result" RWTexture in the compute shader?	
        CShaderSimplex.Dispatch(mgen_id, 1, 1, 16);  // run computeShader "FillEmpty" with 1 x 1 x 31 threadGroups?      
        mgen_id = CShaderSimplex.FindKernel("Simplex3d");
        CShaderSimplex.SetTexture(mgen_id, "Result", DensityVolume);
        //CShaderSimplex.SetVector("_StartPos", new Vector4(0f, 0f, 0f, 0.0f));  // position of the Chunk GameObject
        //CShaderSimplex.SetFloat("_Str", 4f);
        //CShaderSimplex.SetFloat("_NoiseA", 0.000718f);
        //CShaderSimplex.SetFloat("_NoiseB", 0.000632f);
        //CShaderSimplex.SetFloat("_NoiseC", 0.000695f);
        CShaderSimplex.Dispatch(mgen_id, 1, 1, 16);  // Fill shared RenderTexture with GPU simplex Noise
        

        ComputeBuffer cBufferSegmentTransform = new ComputeBuffer(critterSegmentTransforms.Length, sizeof(float) * (3 + 3 + 4));
        cBufferSegmentTransform.SetData(critterSegmentTransforms);
        int kernelID = CShaderBuildMC.FindKernel("CSMain");
        CShaderBuildMC.SetBuffer(kernelID, "segmentTransformBuffer", cBufferSegmentTransform);
        CShaderBuildMC.SetTexture(kernelID, "noise_volume", DensityVolume);  // Noise 3D texture
        //Debug.Log(DensityVolume.colorBuffer.ToString());

        // Figure out how many chunks are needed:
        int numChunksX = Mathf.CeilToInt(GlobalBoundingBoxDimensions.x / (cellResolution * 8f));
        int numChunksY = Mathf.CeilToInt(GlobalBoundingBoxDimensions.y / (cellResolution * 8f));
        int numChunksZ = Mathf.CeilToInt(GlobalBoundingBoxDimensions.z / (cellResolution * 8f));
        //Debug.Log("numChunks: (" + numChunksX.ToString() + ", " + numChunksY.ToString() + ", " + numChunksZ.ToString() + ")");

        int totalNumChunks = numChunksX * numChunksY * numChunksZ;
        Poly[][] PolyArrayArray = new Poly[totalNumChunks][];  // This will hold the mesh data from the chunks calculated on the GPU
        int[] numPolysArray = new int[totalNumChunks];
        int totalNumPolys = 0;

        // Get each chunk!
        int chunkIndex = 0;
        for(int x = 0; x < numChunksX; x++) {
            for(int y = 0; y < numChunksY; y++) {
                for(int z = 0; z < numChunksZ; z++) {
                    // Figure out chunk offset amount:
                    Vector3 chunkOffset = new Vector3(cellResolution * 8f * x, cellResolution * 8f * y, cellResolution * 8f * z) + GlobalBoundingBoxOffset - (GlobalBoundingBoxDimensions / 2f);

                    int[] numPolys = new int[1];
                    ComputeBuffer cBufferNumPoly = new ComputeBuffer(1, sizeof(int));
                    cBufferNumPoly.SetData(numPolys);

                    int id = CShaderBuildMC.FindKernel("CSMain");
                    CShaderBuildMC.SetInt("_CalcNumPolys", 1); // only calculate how many tris so I can correctly size the poly buffer
                    CShaderBuildMC.SetFloat("_GlobalOffsetX", chunkOffset.x);
                    CShaderBuildMC.SetFloat("_GlobalOffsetY", chunkOffset.y);
                    CShaderBuildMC.SetFloat("_GlobalOffsetZ", chunkOffset.z);
                    CShaderBuildMC.SetFloat("_CellSize", cellResolution);
                    CShaderBuildMC.SetVector("_ColorPrimary", colorPrimary);
                    CShaderBuildMC.SetVector("_ColorSecondary", colorSecondary);
                    CShaderBuildMC.SetFloat("_ColorNoiseScale", colorNoiseScale);
                    CShaderBuildMC.SetFloat("_ColorSmlAmplitude", colorSmlAmplitude);
                    CShaderBuildMC.SetFloat("_ColorMedAmplitude", colorMedAmplitude);
                    CShaderBuildMC.SetFloat("_ColorLrgAmplitude", colorLrgAmplitude);
                    CShaderBuildMC.SetFloat("_ColorContrast", colorContrast);
                    CShaderBuildMC.SetFloat("_ColorThreshold", colorThreshold);
                    CShaderBuildMC.SetVector("_SkinNoiseScale", skinNoiseScale);
                    CShaderBuildMC.SetFloat("_SkinNoiseAmplitude", skinNoiseAmplitude);
                    CShaderBuildMC.SetVector("_SkinLocalTaper", skinLocalTaper);
                    CShaderBuildMC.SetVector("_SkinLocalSinFreq", skinLocalSinFreq);
                    CShaderBuildMC.SetVector("_SkinLocalSinAmp", skinLocalSinAmp);
                    // Local Segment-space modifications, sin, taper, etc.

                    CShaderBuildMC.SetBuffer(id, "numPolyBuffer", cBufferNumPoly);
                    CShaderBuildMC.Dispatch(id, 1, 1, 1);  // calc num polys      
                    cBufferNumPoly.GetData(numPolys);  // get numPolys
                    //Debug.Log("Chunk: " + (z + (numChunksZ * y) + (numChunksZ * numChunksY * x)).ToString() + ", cBufferNumPoly.GetData(numPolys): " + numPolys[0].ToString() + ", chunkOffset: " + chunkOffset.ToString());
                    totalNumPolys += numPolys[0];
                    numPolysArray[chunkIndex] = numPolys[0];

                    //_MaxBufferSize = numPolys[0];
                    if(numPolys[0] > 0) {   // only do this if there was at least 1 triangle in the test pass
                        Poly[] polyArray = new Poly[numPolys[0]];
                        int cBufferStride = sizeof(float) * (18 + 9 + 6) + sizeof(int) * (6);
                        ComputeBuffer cBuffer = new ComputeBuffer(numPolys[0], cBufferStride);  // 18 floats x 4 bytes/float = 72   + COLORS! 9 x 4 = 36  = 108   + BONES! 6x4 = 24 + 6 xint...
                        cBuffer.SetData(polyArray);

                        //procMaterial.SetBuffer("buf_Polys", cBuffer);  // TESTING!!!!
                        //procBufferLength = numPolys[0];

                        CShaderBuildMC.SetBuffer(id, "buffer", cBuffer);
                        CShaderBuildMC.SetInt("_CalcNumPolys", 0);  // Actually calc tris        
                        CShaderBuildMC.Dispatch(id, 1, 1, 1);
                        cBuffer.GetData(polyArray);  // return data from GPU

                        PolyArrayArray[chunkIndex] = polyArray;
                        cBuffer.Dispose();
                    }

                    cBufferNumPoly.Dispose();

                    chunkIndex++;
                }
            }
        }

        
        CritterDecorationsTest.decorationStruct[] points = new CritterDecorationsTest.decorationStruct[totalNumPolys];
        
        //Construct mesh using received data 
        int vindex = 0;
        int decindex = 0;
                
        // Why same number of tris as vertices?  == // because all triangles have duplicate verts - no shared vertices?
        Vector3[] vertices = new Vector3[totalNumPolys * 3];
        Color[] colors = new Color[totalNumPolys * 3];
        int[] tris = new int[totalNumPolys * 3];
        Vector2[] uvs = new Vector2[totalNumPolys * 3];
        Vector3[] normals = new Vector3[totalNumPolys * 3];
        BoneWeight[] weights = new BoneWeight[totalNumPolys * 3];

        //Parse triangles
        for(int i = 0; i < PolyArrayArray.Length; i++) {
            if(numPolysArray[i] > 0) {  // only do this if there was at least 1 triangle in the test pass
                for (int ix = 0; ix < numPolysArray[i]; ix++) {

                    Vector3 vPos;
                    Vector3 vOffset = new Vector3(0, 0, 0);   //???  offsets all vertices by this amount, but why 30?? 
                                                              //A1,A2,A3
                    vPos = new Vector3(PolyArrayArray[i][ix].A1, PolyArrayArray[i][ix].A2, PolyArrayArray[i][ix].A3) + vOffset;
                    vertices[vindex] = vPos * _Scale;
                    normals[vindex] = new Vector3(PolyArrayArray[i][ix].NA1, PolyArrayArray[i][ix].NA2, PolyArrayArray[i][ix].NA3);
                    tris[vindex] = vindex;
                    uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);
                    colors[vindex] = new Color(PolyArrayArray[i][ix].CAR, PolyArrayArray[i][ix].CAG, PolyArrayArray[i][ix].CAB, 1.0f);
                    weights[vindex].boneIndex0 = PolyArrayArray[i][ix].BoneIndexA0;
                    weights[vindex].boneIndex1 = PolyArrayArray[i][ix].BoneIndexA1;
                    weights[vindex].weight0 = PolyArrayArray[i][ix].BoneWeightA0;
                    weights[vindex].weight1 = PolyArrayArray[i][ix].BoneWeightA1;

                    points[decindex].pos = vPos;
                    points[decindex].normal = normals[vindex];
                    points[decindex].color = new Vector3(colors[vindex].r, colors[vindex].g, colors[vindex].b);
                    //decorationsDataArray[vindex].pos = vPos;
                    decindex++;
                    vindex++;

                    //B1,B2,B3
                    vPos = new Vector3(PolyArrayArray[i][ix].B1, PolyArrayArray[i][ix].B2, PolyArrayArray[i][ix].B3) + vOffset;
                    vertices[vindex] = vPos * _Scale;
                    normals[vindex] = new Vector3(PolyArrayArray[i][ix].NB1, PolyArrayArray[i][ix].NB2, PolyArrayArray[i][ix].NB3);
                    tris[vindex] = vindex;
                    uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);
                    colors[vindex] = new Color(PolyArrayArray[i][ix].CBR, PolyArrayArray[i][ix].CBG, PolyArrayArray[i][ix].CBB, 1.0f);
                    weights[vindex].boneIndex0 = PolyArrayArray[i][ix].BoneIndexB0;
                    weights[vindex].boneIndex1 = PolyArrayArray[i][ix].BoneIndexB1;
                    weights[vindex].weight0 = PolyArrayArray[i][ix].BoneWeightB0;
                    weights[vindex].weight1 = PolyArrayArray[i][ix].BoneWeightB1;

                    //decorationsDataArray[vindex].pos = vPos;
                    vindex++;

                    //C1,C2,C3
                    vPos = new Vector3(PolyArrayArray[i][ix].C1, PolyArrayArray[i][ix].C2, PolyArrayArray[i][ix].C3) + vOffset;
                    vertices[vindex] = vPos * _Scale;
                    normals[vindex] = new Vector3(PolyArrayArray[i][ix].NC1, PolyArrayArray[i][ix].NC2, PolyArrayArray[i][ix].NC3);
                    tris[vindex] = vindex;
                    uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);
                    colors[vindex] = new Color(PolyArrayArray[i][ix].CCR, PolyArrayArray[i][ix].CCG, PolyArrayArray[i][ix].CCB, 1.0f);
                    weights[vindex].boneIndex0 = PolyArrayArray[i][ix].BoneIndexC0;
                    weights[vindex].boneIndex1 = PolyArrayArray[i][ix].BoneIndexC1;
                    weights[vindex].weight0 = PolyArrayArray[i][ix].BoneWeightC0;
                    weights[vindex].weight1 = PolyArrayArray[i][ix].BoneWeightC1;

                    //decorationsDataArray[vindex].pos = vPos;
                    vindex++;
                }
            }            
        }
        
        //We have got all data and are ready to setup a new mesh!
        //newMesh.Clear();
        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.uv = uvs; //Unwrapping.GeneratePerTriangleUV(NewMesh);
        newMesh.triangles = tris;
        newMesh.normals = normals; //NewMesh.RecalculateNormals();
        newMesh.colors = colors;
        //newMesh.RecalculateNormals();
        newMesh.Optimize();        

        // Set up SKINNING!!!:
        Transform[] bones = new Transform[critter.critterSegmentList.Count];
        Matrix4x4[] bindPoses = new Matrix4x4[critter.critterSegmentList.Count];
        // Try just using existing critter's GameObjects/Transforms:
        for(int seg = 0; seg < critter.critterSegmentList.Count; seg++) {
            bones[seg] = critter.critterSegmentList[seg].transform;
            bindPoses[seg] = bones[seg].worldToLocalMatrix * transform.localToWorldMatrix;  // ?????????????????  
            // the bind pose is the inverse of inverse transformation matrix of the bone, when the bone is in the bind pose .... unhelpful ....
        }
        newMesh.boneWeights = weights;
        newMesh.bindposes = bindPoses;
        SkinnedMeshRenderer skinnedMeshRenderer = this.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.bones = bones;
        skinnedMeshRenderer.sharedMesh = newMesh;
        skinnedMeshRenderer.enabled = true;

        //cBuffer.Dispose();
        //cBufferNumPoly.Dispose();
        //cBuffer.Release();
        cBufferSegmentTransform.Release();

        critterDecorationsTest.TurnOn(points);

        float calcTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("MeshCreated! " + calcTime.ToString());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy() {

        //DensityVolume.Release(); 
    }
}
