using UnityEngine;
using System.Collections;

public class TestComputeShader01 : MonoBehaviour {

    public ComputeShader shader;

    struct VecMatPair {
        public Vector3 point;
        public Matrix4x4 matrix;
    }

    // Use this for initialization
    void Start () {
        RunShader();
        RunMultiplyShader();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RunShader() {
        int kernelHandle = shader.FindKernel("CSMain");

        RenderTexture tex = new RenderTexture(256, 256, 24);
        tex.enableRandomWrite = true;
        tex.Create();

        shader.SetTexture(kernelHandle, "Result", tex);
        shader.Dispatch(kernelHandle, 256 / 8, 256 / 8, 1);

        GetComponent<MeshRenderer>().material.mainTexture = tex;
    }

    public void RunMultiplyShader() {
        VecMatPair[] data = new VecMatPair[5];
        VecMatPair[] output = new VecMatPair[5];
        // Init Data here!!!
        for (int i = 0; i < data.Length; i++) {
            data[i].point = UnityEngine.Random.onUnitSphere;
            data[i].matrix = Matrix4x4.TRS(UnityEngine.Random.onUnitSphere, UnityEngine.Random.rotation, UnityEngine.Random.onUnitSphere);
            Debug.Log("PreShader! Pos: " + data[i].point.ToString() + ", Matrix: " + data[i].matrix.ToString());
        }

        ComputeBuffer buffer = new ComputeBuffer(data.Length, 76);
        int kernelHandle = shader.FindKernel("Multiply");
        buffer.SetData(data);
        shader.SetBuffer(kernelHandle, "dataBuffer", buffer);
        shader.Dispatch(kernelHandle, data.Length, 1, 1);
        buffer.GetData(output);

        for (int i = 0; i < output.Length; i++) {            
            Debug.Log("PostShader! Pos: " + output[i].point.ToString() + ", Matrix: " + output[i].matrix.ToString());
        }

        buffer.Dispose();

        /*public ComputeShader compute;
        public ComputeBuffer buffer;
        public int[] cols;
   
        void Start () {
        var mesh = GetComponent<MeshFilter>().mesh;
        int n = mesh.vertexCount;
            ///
        buffer = new ComputeBuffer (n, 16);
        ///
        cols = new int[n];
            ///
        for (int i = 0; i < n; ++i)
             cols[i] = 0;      
        buffer.SetData (cols); 
            ///
        compute.SetBuffer(compute.FindKernel ("CSMain"),"bufColors", buffer);
            ///
            compute.Dispatch(0,4,4,1);
        ///
        buffer.GetData(cols);
        Debug.Log (cols[0]); 
        */
    }
}
