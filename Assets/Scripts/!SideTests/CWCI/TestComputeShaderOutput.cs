using UnityEngine;
using System.Collections;
using System.Linq;

[AddComponentMenu("Scripts/CWCI/TestComputeshaderOutput")]

public class TestComputeShaderOutput : MonoBehaviour {

    #region ComputeShader Fields and Properties

    public ComputeShader computeShader;

    public const int VertCount = 10 * 10 * 10 * 10 * 10 * 10; // 1 million

    public ComputeBuffer outputBuffer;

    public Shader PointShader;
    Material PointMaterial;

    public bool Debugrender = false;

    int CSKernel;

    #endregion

    void InitializeBuffers() {
        outputBuffer = new ComputeBuffer(VertCount, (sizeof(float) * 3) + (sizeof(int) * 6));

        computeShader.SetBuffer(CSKernel, "outputBuffer", outputBuffer);

        if (Debugrender)
            PointMaterial.SetBuffer("buf_Points", outputBuffer);
    }

    public void Dispatch() {
        if(!SystemInfo.supportsComputeShaders) {
            Debug.LogWarning("Compute shaders not supported (not using dx11?)");
            return;
        }

        computeShader.Dispatch(CSKernel, 10, 10, 10);
    }

    void ReleaseBuffers() {
        outputBuffer.Release();
    }

    // Use this for initialization
    void Start () {
        CSKernel = computeShader.FindKernel("CSMainGrid");

        if(Debugrender) {
            PointMaterial = new Material(PointShader);
            PointMaterial.SetVector("_worldPos", transform.position);
        }

        InitializeBuffers();	
	}

    void OnRenderObject() {
        if(Debugrender) {
            Dispatch();
            PointMaterial.SetPass(0);
            PointMaterial.SetVector("_worldPos", transform.position);

            Graphics.DrawProcedural(MeshTopology.Points, VertCount);
        }
    }

    private void OnDisable() {
        ReleaseBuffers();
    }	
}
