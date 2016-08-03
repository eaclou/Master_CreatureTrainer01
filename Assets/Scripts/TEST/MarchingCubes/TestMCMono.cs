using UnityEngine;
using System.Collections;

public class TestMCMono : MonoBehaviour {

    public ComputeShader CShaderBuildMC;
    public int resolutionX = 8;
    public int resolutionY = 8;
    public int resolutionZ = 8;
    
    public int _MaxBufferSize = 21660;
    public int _Trilinear = 1;
    public int _MultiSampling = 1;

    public float _Scale = 1f;

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
        
        int[] numPolys = new int[1];        
        ComputeBuffer cBufferNumPoly = new ComputeBuffer(1, sizeof(int));        
        cBufferNumPoly.SetData(numPolys);

        int id = CShaderBuildMC.FindKernel("CSMain");        
        CShaderBuildMC.SetInt("_CalcNumPolys", 1); // only calculate how many tris so I can correctly size the poly buffer
        CShaderBuildMC.SetBuffer(id, "numPolyBuffer", cBufferNumPoly);        
        CShaderBuildMC.Dispatch(id, 1, 1, 1);  // calc num polys      
        cBufferNumPoly.GetData(numPolys);  // get numPolys
        Debug.Log("cBufferNumPoly.GetData(numPolys): " + numPolys[0].ToString());

        _MaxBufferSize = numPolys[0];
        Poly[] polyArray = new Poly[_MaxBufferSize];
        ComputeBuffer cBuffer = new ComputeBuffer(_MaxBufferSize, 72);  // 18 floats x 4 bytes/float = 72
        cBuffer.SetData(polyArray);

        CShaderBuildMC.SetBuffer(id, "buffer", cBuffer);
        CShaderBuildMC.SetInt("_CalcNumPolys", 0);  // Actually calc tris        
        CShaderBuildMC.Dispatch(id, 1, 1, 1);
        cBuffer.GetData(polyArray);  // return data from GPU
        
        //Construct mesh using received data
        Mesh newMesh = new Mesh();

        int vindex = 0;
        //int count = 0;

        //Count real data length   --- Looks like there might be wasted data??? -- investigate how to Optimize
        /*for (count = 0; count < _MaxBufferSize; count++) {
            if (polyArray[count].A1 == 0.0f && polyArray[count].B1 == 0.0f && polyArray[count].C1 == 0.0 &&
                polyArray[count].A2 == 0.0f && polyArray[count].B2 == 0.0f && polyArray[count].C2 == 0.0 &&
                polyArray[count].A3 == 0.0f && polyArray[count].B3 == 0.0f && polyArray[count].C3 == 0.0) {

                break;
            }
        }*/
        //Debug.Log(count+" triangles got");
        // Why same number of tris as vertices?  == // because all triangles have duplicate verts - no shared vertices?
        Vector3[] vertices = new Vector3[_MaxBufferSize * 3];
        int[] tris = new int[_MaxBufferSize * 3];
        Vector2[] uvs = new Vector2[_MaxBufferSize * 3];
        Vector3[] normals = new Vector3[_MaxBufferSize * 3];

        //Parse triangles
        for (int ix = 0; ix < _MaxBufferSize; ix++) {

            Vector3 vPos;
            Vector3 vOffset = new Vector3(0, 0, 0);   //???  offsets all vertices by this amount, but why 30?? 
                                                            //A1,A2,A3
            vPos = new Vector3(polyArray[ix].A1, polyArray[ix].A2, polyArray[ix].A3) + vOffset;
            vertices[vindex] = vPos * _Scale;
            normals[vindex] = new Vector3(polyArray[ix].NA1, polyArray[ix].NA2, polyArray[ix].NA3);
            tris[vindex] = vindex;
            uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);

            vindex++;

            //B1,B2,B3
            vPos = new Vector3(polyArray[ix].B1, polyArray[ix].B2, polyArray[ix].B3) + vOffset;
            vertices[vindex] = vPos * _Scale;
            normals[vindex] = new Vector3(polyArray[ix].NB1, polyArray[ix].NB2, polyArray[ix].NB3);
            tris[vindex] = vindex;
            uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);

            vindex++;

            //C1,C2,C3
            vPos = new Vector3(polyArray[ix].C1, polyArray[ix].C2, polyArray[ix].C3) + vOffset;
            vertices[vindex] = vPos * _Scale;
            normals[vindex] = new Vector3(polyArray[ix].NC1, polyArray[ix].NC2, polyArray[ix].NC3);
            tris[vindex] = vindex;
            uvs[vindex] = new Vector2(vertices[vindex].z, vertices[vindex].x);

            vindex++;
        }

        //We have got all data and are ready to setup a new mesh!

        //newMesh.Clear();

        newMesh.vertices = vertices;
        newMesh.uv = uvs; //Unwrapping.GeneratePerTriangleUV(NewMesh);
        newMesh.triangles = tris;
        newMesh.normals = normals; //NewMesh.RecalculateNormals();
        newMesh.RecalculateNormals();
        newMesh.Optimize();

        cBuffer.Dispose();
        cBufferNumPoly.Dispose();
        //cBuffer.Release();

        this.GetComponent<MeshFilter>().sharedMesh = newMesh;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy() {
        
        //DensityVolume.Release();        

    }
}
