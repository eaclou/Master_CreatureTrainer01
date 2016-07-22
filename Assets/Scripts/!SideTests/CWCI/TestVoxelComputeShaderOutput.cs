using UnityEngine;
using System.Collections;

public class TestVoxelComputeShaderOutput : MonoBehaviour {

    #region ComputeShader Fields and Properties

    public ComputeShader computeShader;

    int VertCount; // = 10 * 10 * 10 * 10 * 10 * 10; // 1 million
    
    public int Seed;

    public ComputeBuffer outputBuffer;
    public ComputeBuffer mapBuffer;

    public Shader PointShader;
    Material PointMaterial;

    public bool Debugrender = false;

    public int cubeMultiplier = 2;

    public float noiseMag = 0.5f;

    int CSKernel;

    #endregion

    void InitializeBuffers() {
        VertCount = 10 * 10 * 10 * cubeMultiplier * cubeMultiplier * cubeMultiplier;

        outputBuffer = new ComputeBuffer(VertCount, (sizeof(float) * 3) + (sizeof(int) * 6));
        mapBuffer = new ComputeBuffer(VertCount, sizeof(int));

        int width = 10 * cubeMultiplier;
        int height = 10 * cubeMultiplier;
        int depth = 10 * cubeMultiplier;

        TestNoise noise = new TestNoise(1f, 1f, 3);
        float[][] tempNoiseHeight = new float[10 * cubeMultiplier][];
        for(int i = 0; i < tempNoiseHeight.Length; i++) {
            tempNoiseHeight[i] = new float[10 * cubeMultiplier];

            for(int j = 0; j < tempNoiseHeight[i].Length; j++) {
                tempNoiseHeight[i][j] = noise.GetNoise((double)(i) * 0.05, (double)(j) * 0.14, 0.0) * 0.5f + 0.5f; //UnityEngine.Random.Range(0f, 1f);
            }
        }

        int[] map = new int[VertCount];

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                for(int z = 0; z < depth; z++) {
                    int idx = x + (y * 10 * cubeMultiplier) + (z * 10 * cubeMultiplier * 10 * cubeMultiplier);

                    if (tempNoiseHeight[x][z] >= y / (float)height)
                        map[idx] = 1;
                    else
                        map[idx] = 0;
                }
            }
        }

        mapBuffer.SetData(map);

        computeShader.SetBuffer(CSKernel, "outputBuffer", outputBuffer);
        computeShader.SetBuffer(CSKernel, "mapBuffer", mapBuffer);

        computeShader.SetVector("group_size", new Vector3(cubeMultiplier, cubeMultiplier, cubeMultiplier));

        if (Debugrender)
            PointMaterial.SetBuffer("buf_Points", outputBuffer);

        transform.position -= (Vector3.one * 10 * cubeMultiplier) * 0.5f;
    }

    void SetBuffers() {
        //outputBuffer = new ComputeBuffer(VertCount, (sizeof(float) * 3) + (sizeof(int) * 6));
        //mapBuffer = new ComputeBuffer(VertCount, sizeof(int));

        int width = 10 * cubeMultiplier;
        int height = 10 * cubeMultiplier;
        int depth = 10 * cubeMultiplier;

        TestNoise noise = new TestNoise(1f, 1f, 3);
        float[][] tempNoiseHeight = new float[10 * cubeMultiplier][];
        for (int i = 0; i < tempNoiseHeight.Length; i++) {
            tempNoiseHeight[i] = new float[10 * cubeMultiplier];

            for (int j = 0; j < tempNoiseHeight[i].Length; j++) {
                tempNoiseHeight[i][j] = noise.GetNoise(((double)(i) + (double)(Time.time * 0.16)) * 0.05, (double)(j) * 0.14 + (double)(Time.time * 0.005), (double)(Time.time * 0.01)) * noiseMag + 0.5f; //UnityEngine.Random.Range(0f, 1f);
            }
        }

        int[] map = new int[VertCount];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {
                    int idx = x + (y * 10 * cubeMultiplier) + (z * 10 * cubeMultiplier * 10 * cubeMultiplier);

                    if (tempNoiseHeight[x][z] >= y / (float)height)
                        map[idx] = 1;
                    else
                        map[idx] = 0;
                }
            }
        }

        mapBuffer.SetData(map);

        //computeShader.SetBuffer(CSKernel, "outputBuffer", outputBuffer);
        computeShader.SetBuffer(CSKernel, "mapBuffer", mapBuffer);
    }

    public void Dispatch() {
        if (!SystemInfo.supportsComputeShaders) {
            Debug.LogWarning("Compute shaders not supported (not using dx11?)");
            return;
        }

        computeShader.Dispatch(CSKernel, cubeMultiplier, cubeMultiplier, cubeMultiplier);
    }

    void ReleaseBuffers() {
        outputBuffer.Release();
        mapBuffer.Release();
    }

    // Use this for initialization
    void Start() {
        CSKernel = computeShader.FindKernel("CSMain");

        if (Debugrender) {
            PointMaterial = new Material(PointShader);
            PointMaterial.SetVector("_worldPos", transform.position);
        }

        InitializeBuffers();
    }

    void Update() {
        SetBuffers();
    }

    void OnRenderObject() {
        if (Debugrender) {
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

