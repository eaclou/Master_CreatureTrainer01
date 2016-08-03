using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestMCMonoComposite : MonoBehaviour {

    public ComputeShader CShaderBuildMC;
    public int resolutionX = 8;
    public int resolutionY = 8;
    public int resolutionZ = 8;
    
    public int _MaxBufferSize = 21660;
    public int _Trilinear = 1;
    public int _MultiSampling = 1;

    public float _Scale = 1f;

    public float cellResolution = 1f;
    public Vector3 GlobalBoundingBoxDimensions = new Vector3(16f, 8f, 8f);

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
        float startTime = Time.realtimeSinceStartup;
        Debug.Log("TestMCMono Start()! " + startTime.ToString());

        //VoxelCalculator.Instance.CreateEmptyVolume(DensityVolume, _SizeZ + 4);
        //VoxelCalculator.Instance.CreateNoiseVolume(DensityVolume, transform.position, _SizeZ + 4);
        //VoxelCalculator.Instance.BuildChunkMesh(DensityVolume, MF.sharedMesh);

        BuildMesh();
    }
    
    public void BuildMesh() {
        float startTime = Time.realtimeSinceStartup;

        // CritterSegmentTransforms!!
        SegmentTransform[] critterSegmentTransforms = new SegmentTransform[2];
        Quaternion rot = Quaternion.Euler(55f, 12f, -230f);
        Quaternion rot2 = Quaternion.Euler(-35f, 112f, -67f);
        SegmentTransform segmentTransform;
        segmentTransform.PX = 10f;
        segmentTransform.PY = 8f;
        segmentTransform.PZ = 13f;
        segmentTransform.RX = rot.x;
        segmentTransform.RY = rot.y;
        segmentTransform.RZ = rot.z;
        segmentTransform.RW = rot.w;
        segmentTransform.SX = 6f;
        segmentTransform.SY = 7f;
        segmentTransform.SZ = 2.5f;
        SegmentTransform segmentTransform2;
        segmentTransform2.PX = 22f;
        segmentTransform2.PY = 11f;
        segmentTransform2.PZ = 15f;
        segmentTransform2.RX = rot2.x;
        segmentTransform2.RY = rot2.y;
        segmentTransform2.RZ = rot2.z;
        segmentTransform2.RW = rot2.w;
        segmentTransform2.SX = 9f;
        segmentTransform2.SY = 3f;
        segmentTransform2.SZ = 7f;
        critterSegmentTransforms[0] = segmentTransform;
        critterSegmentTransforms[1] = segmentTransform2;

        ComputeBuffer cBufferSegmentTransform = new ComputeBuffer(critterSegmentTransforms.Length, sizeof(float) * (3 + 3 + 4));
        cBufferSegmentTransform.SetData(critterSegmentTransforms);
        int kernelID = CShaderBuildMC.FindKernel("CSMain");
        CShaderBuildMC.SetBuffer(kernelID, "segmentTransformBuffer", cBufferSegmentTransform);

        // Figure out how many chunks are needed:
        int numChunksX = Mathf.CeilToInt(GlobalBoundingBoxDimensions.x * cellResolution / 8f);
        int numChunksY = Mathf.CeilToInt(GlobalBoundingBoxDimensions.y * cellResolution / 8f);
        int numChunksZ = Mathf.CeilToInt(GlobalBoundingBoxDimensions.z * cellResolution / 8f);
        Debug.Log("numChunks: (" + numChunksX.ToString() + ", " + numChunksY.ToString() + ", " + numChunksZ.ToString() + ")");

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
                    Vector3 chunkOffset = new Vector3(cellResolution * 8f * x, cellResolution * 8f * y, cellResolution * 8f * z);

                    int[] numPolys = new int[1];
                    ComputeBuffer cBufferNumPoly = new ComputeBuffer(1, sizeof(int));
                    cBufferNumPoly.SetData(numPolys);

                    int id = CShaderBuildMC.FindKernel("CSMain");
                    CShaderBuildMC.SetInt("_CalcNumPolys", 1); // only calculate how many tris so I can correctly size the poly buffer
                    CShaderBuildMC.SetFloat("_GlobalOffsetX", chunkOffset.x);
                    CShaderBuildMC.SetFloat("_GlobalOffsetY", chunkOffset.y);
                    CShaderBuildMC.SetFloat("_GlobalOffsetZ", chunkOffset.z);
                    CShaderBuildMC.SetBuffer(id, "numPolyBuffer", cBufferNumPoly);
                    CShaderBuildMC.Dispatch(id, 1, 1, 1);  // calc num polys      
                    cBufferNumPoly.GetData(numPolys);  // get numPolys
                    Debug.Log("Chunk: " + (z + (numChunksZ * y) + (numChunksZ * numChunksY * x)).ToString() + ", cBufferNumPoly.GetData(numPolys): " + numPolys[0].ToString() + ", chunkIndex: " + chunkIndex.ToString());
                    totalNumPolys += numPolys[0];
                    numPolysArray[chunkIndex] = numPolys[0];

                    //_MaxBufferSize = numPolys[0];
                    if(numPolys[0] > 0) {   // only do this if there was at least 1 triangle in the test pass
                        Poly[] polyArray = new Poly[numPolys[0]];
                        ComputeBuffer cBuffer = new ComputeBuffer(numPolys[0], 72);  // 18 floats x 4 bytes/float = 72
                        cBuffer.SetData(polyArray);

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

        //Construct mesh using received data        
        Mesh newMesh = new Mesh();

        int vindex = 0;
                
        // Why same number of tris as vertices?  == // because all triangles have duplicate verts - no shared vertices?
        Vector3[] vertices = new Vector3[totalNumPolys * 3];
        int[] tris = new int[totalNumPolys * 3];
        Vector2[] uvs = new Vector2[totalNumPolys * 3];
        Vector3[] normals = new Vector3[totalNumPolys * 3];

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

                    vindex++;

                    //B1,B2,B3
                    vPos = new Vector3(PolyArrayArray[i][ix].B1, PolyArrayArray[i][ix].B2, PolyArrayArray[i][ix].B3) + vOffset;
                    vertices[vindex] = vPos * _Scale;
                    normals[vindex] = new Vector3(PolyArrayArray[i][ix].NB1, PolyArrayArray[i][ix].NB2, PolyArrayArray[i][ix].NB3);
                    tris[vindex] = vindex;
                    uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);

                    vindex++;

                    //C1,C2,C3
                    vPos = new Vector3(PolyArrayArray[i][ix].C1, PolyArrayArray[i][ix].C2, PolyArrayArray[i][ix].C3) + vOffset;
                    vertices[vindex] = vPos * _Scale;
                    normals[vindex] = new Vector3(PolyArrayArray[i][ix].NC1, PolyArrayArray[i][ix].NC2, PolyArrayArray[i][ix].NC3);
                    tris[vindex] = vindex;
                    uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);

                    vindex++;
                }
            }            
        }
        
        //We have got all data and are ready to setup a new mesh!

        //newMesh.Clear();

        newMesh.vertices = vertices;
        newMesh.uv = uvs; //Unwrapping.GeneratePerTriangleUV(NewMesh);
        newMesh.triangles = tris;
        newMesh.normals = normals; //NewMesh.RecalculateNormals();
        newMesh.RecalculateNormals();
        newMesh.Optimize();

        //cBuffer.Dispose();
        //cBufferNumPoly.Dispose();
        //cBuffer.Release();
        cBufferSegmentTransform.Release();

        this.GetComponent<MeshFilter>().sharedMesh = newMesh;
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
