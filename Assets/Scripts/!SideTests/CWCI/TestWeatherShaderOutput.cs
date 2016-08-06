using UnityEngine;
using System.Collections;

public class TestWeatherShaderOutput : MonoBehaviour {

    public ComputeShader computeShader;

    public const int VertCount = 32 * 32 * 16 * 16 * 1;

    ComputeBuffer startPointBuffer;
    public ComputeBuffer outputBuffer;
    ComputeBuffer constantBuffer;
    ComputeBuffer modBuffer;

    public Shader PointShader;
    Material PointMaterial;

    [Range(0.1f, 200f)]
    public float speed = 1f;

    public bool wobble = false;

    public Vector3 wind = Vector3.zero;

    public float spacing = 5;

    int CSKernel;

    void InitializeBuffers() {
        startPointBuffer = new ComputeBuffer(VertCount, 4);  // float = 4 bytes
        constantBuffer = new ComputeBuffer(1, 4);
        modBuffer = new ComputeBuffer(VertCount, 8);
        outputBuffer = new ComputeBuffer(VertCount, 12);

        float[] values = new float[VertCount];
        Vector2[] mods = new Vector2[VertCount];

        for(int i = 0; i < VertCount; i++) {
            values[i] = Random.value * 2 * Mathf.PI;
            mods[i] = new Vector2(0.1f + Random.value, 0.1f + Random.value);
        }

        modBuffer.SetData(mods);
        startPointBuffer.SetData(values);
        computeShader.SetBuffer(CSKernel, "startPointBuffer", startPointBuffer);
    }

    public void Dispatch() {
        constantBuffer.SetData(new[] { Time.time * 0.001f });

        computeShader.SetBuffer(CSKernel, "modBuffer", modBuffer);
        computeShader.SetBuffer(CSKernel, "constBuffer", constantBuffer);
        computeShader.SetBuffer(CSKernel, "outputBuffer", outputBuffer);
        computeShader.SetFloat("speed", speed);
        computeShader.SetInt("wobble", wobble ? 1 : 0);
        computeShader.SetVector("wind", wind);
        computeShader.SetFloat("spacing", spacing);

        computeShader.Dispatch(CSKernel, 32, 32, 1);
    }

    void ReleaseBuffers() {
        modBuffer.Release();
        constantBuffer.Release();
        startPointBuffer.Release();
        outputBuffer.Release();
    }
    
	// Use this for initialization
	void Start () {
        CSKernel = computeShader.FindKernel("CSMain");

        PointMaterial = new Material(PointShader);

        InitializeBuffers();
	}

    void OnPostrender() {
        if(!SystemInfo.supportsComputeShaders) {
            Debug.LogWarning("Compute Shaders not supported!");
        }

        Dispatch();

        PointMaterial.SetPass(0);
        PointMaterial.SetBuffer("buf_Points", outputBuffer);

        //Graphics.DrawProcedural(MeshTopology.Points, VertCount);
    }

    private void OnDisable() {
        ReleaseBuffers();
    }
	
}
