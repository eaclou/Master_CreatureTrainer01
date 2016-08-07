using UnityEngine;
using System.Collections;

public class TestComputeSharedBufferA : MonoBehaviour {
    
    public ComputeShader computeShaderA;
    public int numPoints = 64;
    public ComputeBuffer computeBuffer;
    public ComputeBuffer timeBuffer;
    public Shader pointShaderA;   
    private Material material;
    int kernelID;

    float[] timeArray;

    void InitializeBuffers() {
        computeBuffer = new ComputeBuffer(numPoints, 12);
        computeShaderA.SetBuffer(kernelID, "outputBuffer", computeBuffer);
        timeBuffer = new ComputeBuffer(1, 4);
        timeArray = new float[1];
        material.SetBuffer("buf_Points", computeBuffer);        
    }

    // Use this for initialization
    void Start () {        
        kernelID = computeShaderA.FindKernel("CSMainGrid");
        material = new Material(pointShaderA);
        InitializeBuffers();        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnRenderObject() {
        timeArray[0] = Time.fixedTime;
        timeBuffer.SetData(timeArray);
        computeShaderA.Dispatch(kernelID, 1, 1, 1);
        material.SetPass(0);
        material.SetBuffer("buf_Time", timeBuffer);
        Graphics.DrawProcedural(MeshTopology.Points, computeBuffer.count);
        //Debug.Log("DrawProcedural! " + material.GetVector("_Size").ToString());
    }

    void OnDestroy() {
        computeBuffer.Release();
    }
}
