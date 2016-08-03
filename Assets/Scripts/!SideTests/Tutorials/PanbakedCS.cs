using UnityEngine;

//This game object invokes PlaneComputeShader (when attached via drag'n drop in the editor) using the PlaneBufferShader (also attached in the editor)
//to display a grid of points moving back and forth along the z axis.
public class PanbakedCS : MonoBehaviour {

    public Shader shader;
    public ComputeShader computeShader;

    private ComputeBuffer offsetBuffer;
    private ComputeBuffer outputBuffer;
    private ComputeBuffer constantBuffer;
    private int _kernel;
    private Material material;

    public const int VertCount = 16384; //32*32*4*4 (Groups*ThreadsPerGroup)

    //We initialize the buffers and the material used to draw.
    void Start() {
        CreateBuffers();
        CreateMaterial();
        _kernel = computeShader.FindKernel("CSMain");
    }

    //When this GameObject is disabled we must release the buffers or else Unity complains.
    private void OnDisable() {
        ReleaseBuffer();
    }

    //After all rendering is complete we dispatch the compute shader and then set the material before drawing with DrawProcedural
    //this just draws the "mesh" as a set of points
    void OnPostRender() {
        Dispatch();

        material.SetPass(0);
        material.SetBuffer("buf_Points", outputBuffer);
        Graphics.DrawProcedural(MeshTopology.Points, VertCount);
    }

    //To setup a ComputeBuffer we pass in the array length, as well as the size in bytes of a single element.
    //We fill the offset buffer with random numbers between 0 and 2*PI.
    void CreateBuffers() {
        offsetBuffer = new ComputeBuffer(VertCount, 4); //Contains a single float value (OffsetStruct)

        float[] values = new float[VertCount];

        for (int i = 0; i < VertCount; i++) {
            values[i] = Random.value * 2 * Mathf.PI;
        }

        offsetBuffer.SetData(values);

        constantBuffer = new ComputeBuffer(1, 4); //Contains a single element (time) which is a float

        outputBuffer = new ComputeBuffer(VertCount, 12); //Output buffer contains vertices (float3 = Vector3 -> 12 bytes)
    }

    //For some reason I made this method to create a material from the attached shader.
    void CreateMaterial() {
        material = new Material(shader);
    }

    //Remember to release buffers and destroy the material when play has been stopped.
    void ReleaseBuffer() {
        constantBuffer.Release();
        offsetBuffer.Release();
        outputBuffer.Release();

        DestroyImmediate(material);
    }

    //The meat of this script, it sets the constant buffer (current time) and then sets all of the buffers for the compute shader.
    //We then dispatch 32x32x1 groups of threads of our CSMain kernel.
    void Dispatch() {
        constantBuffer.SetData(new[] { Time.time });

        computeShader.SetBuffer(_kernel, "cBuffer", constantBuffer);
        computeShader.SetBuffer(_kernel, "offsets", offsetBuffer);
        computeShader.SetBuffer(_kernel, "output", outputBuffer);

        computeShader.Dispatch(_kernel, 32, 32, 1);
    }
}
