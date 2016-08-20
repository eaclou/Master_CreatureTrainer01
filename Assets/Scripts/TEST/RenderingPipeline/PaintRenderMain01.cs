using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class PaintRenderMain01 : MonoBehaviour {

    //public RenderTexture canvasRT;
    public Camera mainCam;

    public Shader canvasShader;
    public Texture canvasDepthTex;
    private Material canvasMaterial;

    public Shader gessoShader;
    private Material gessoMaterial;

    public Shader strokeShader;
    private Material strokeMaterial;
    
    public Vector2 size = Vector2.one;
    public Color brushStrokeColor = new Color(1f, 0.6f, 0.3f, 1f);

    public ComputeShader strokeCompute;
    public ComputeBuffer strokeBuffer;

    private ComputeBuffer quadPointsBuffer;

    public struct strokeStruct {
        public Vector3 pos;
    }

    public Vector3 brushStrokesWorldOffset = new Vector3(0, 0, 0);
    strokeStruct[] strokeArray;

    // CommandBuffer Stuff:
    private Camera m_Cam;
    // We'll want to add a command buffer on any camera that renders us,
    // so have a dictionary of them.
    private Dictionary<Camera, CommandBuffer> m_Cameras = new Dictionary<Camera, CommandBuffer>();

    // Use this for initialization
    void Start () {
        //canvasRT = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 0, RenderTextureFormat.Default);
        InitializeBuffers();        
    }

    void InitializeBuffers() {
        strokeMaterial = new Material(strokeShader);
        int numStrokes = 16;
        Vector3 dimensions = new Vector3(10f, 10f, 10f);
        strokeArray = new strokeStruct[numStrokes * numStrokes * numStrokes];
        int index = 0;
        for(int x = 0; x < numStrokes; x++) {
            for (int y = 0; y < numStrokes; y++) {
                for (int z = 0; z < numStrokes; z++) {
                    strokeArray[index].pos = new Vector3(((float)x / (float)numStrokes) * dimensions.x, ((float)y / (float)numStrokes) * dimensions.y, ((float)z / (float)numStrokes) * dimensions.z) + brushStrokesWorldOffset + UnityEngine.Random.insideUnitSphere * 0.25f;
                    index++;
                }
            }
        }
        //{
        //    new strokeStruct() { pos = Vector3.left + Vector3.up },
        //    new strokeStruct() { pos = Vector3.right + Vector3.up },
        //    new strokeStruct() { pos = Vector3.zero },
        //    new strokeStruct() { pos = Vector3.left + Vector3.down },
        //    new strokeStruct() { pos = Vector3.right + Vector3.down }
        //};

        strokeBuffer = new ComputeBuffer(strokeArray.Length, sizeof(float) * 3);
        strokeBuffer.SetData(strokeArray);

        //Create quad buffer
        quadPointsBuffer = new ComputeBuffer(6, sizeof(float) * 3);

        quadPointsBuffer.SetData(new[] {
            new Vector3(-0.5f, 0.5f),
            new Vector3(0.5f, 0.5f),
            new Vector3(0.5f, -0.5f),
            new Vector3(0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f)
        });


        gessoMaterial = new Material(gessoShader);
        canvasMaterial = new Material(canvasShader);
    }

    private Mesh CreateFullscreenQuad(Camera cam) {
        Debug.Log("CreateQuad!");
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane + 0.01f));
        vertices[1] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, cam.nearClipPlane + 0.01f));
        vertices[2] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, cam.nearClipPlane + 0.01f));
        vertices[3] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane + 0.01f));

        Vector2[] uvs = new Vector2[4] {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f)
        };

        int[] triangles = new int[6] {
            0, 3, 1, 0, 2, 3
        };

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        
        return mesh;
    }

    // Whenever any camera will render us, add a command buffer to do the work on it
    public void OnWillRenderObject() {   // REQUIRES THIS OBJ TO HAVE MESHRENDERER COMPONENT!!!!
        var act = gameObject.activeInHierarchy && enabled;
        if (!act) {
            Cleanup();
            return;
        }

        var cam = Camera.current;
        if (!cam)
            return;

        CommandBuffer buf = null;  // clear CommandBuffer... why does this have to be done every frame?
                                   // Did we already add the command buffer on this camera? Nothing to do then.
        if (m_Cameras.ContainsKey(cam))
            return;

        //if (!m_Material) {
        //    m_Material = new Material(m_BlurShader);
        //    m_Material.hideFlags = HideFlags.HideAndDontSave;  // not sure what this does -- prevents garbage collection??
        //}

        buf = new CommandBuffer();
        buf.name = "TestDrawProcedural";
        m_Cameras[cam] = buf;  // fill in dictionary entry for this Camera

        // START!!!
        // Canvas First:
        canvasMaterial.SetColor("_Color", Color.white);  // initialize canvas        
        canvasMaterial.SetTexture("_DepthTex", canvasDepthTex);
        canvasMaterial.SetFloat("_MaxDepth", 1.0f);

        // Create RenderTargets:
        int colorReadID = Shader.PropertyToID("_ColorTextureRead");
        int colorWriteID = Shader.PropertyToID("_ColorTextureWrite");
        int depthReadID = Shader.PropertyToID("_DepthTextureRead");
        int depthWriteID = Shader.PropertyToID("_DepthTextureWrite");
        buf.GetTemporaryRT(colorReadID, -1, -1, 0, FilterMode.Bilinear);
        buf.GetTemporaryRT(colorWriteID, -1, -1, 0, FilterMode.Bilinear);
        buf.GetTemporaryRT(depthReadID, -1, -1, 0, FilterMode.Bilinear);
        buf.GetTemporaryRT(depthWriteID, -1, -1, 0, FilterMode.Bilinear);

        RenderTargetIdentifier[] mrt = { colorWriteID, depthWriteID };  // Define multipleRenderTarget so I can render to color AND depth
        buf.SetRenderTarget(mrt, BuiltinRenderTextureType.CameraTarget);  // Set render Targets
        buf.ClearRenderTarget(true, true, Color.white, 1.0f);  // clear -- needed???
        buf.DrawMesh(CreateFullscreenQuad(mainCam), Matrix4x4.identity, canvasMaterial);   // Write into canvas Color & Depth buffers
        
        // Copy results into Read buffers for next pass:
        buf.Blit(colorWriteID, colorReadID);
        buf.Blit(depthWriteID, depthReadID);

        // Gesso/Primer Pass:
        gessoMaterial.SetPass(0);
        gessoMaterial.SetColor("_Color", Color.cyan);
        buf.SetGlobalTexture("_ColorReadTex", colorReadID);
        buf.SetGlobalTexture("_DepthReadTex", depthReadID);
        buf.SetRenderTarget(mrt, BuiltinRenderTextureType.CameraTarget);  // Set render Targets
        buf.DrawMesh(CreateFullscreenQuad(mainCam), Matrix4x4.identity, gessoMaterial);
        // Copy results into Read buffers for next pass:
        buf.Blit(colorWriteID, colorReadID);
        buf.Blit(depthWriteID, depthReadID);

        // MAIN BRUSHSTROKE CONTENTS PASS!!!:
        strokeMaterial.SetPass(0);
        strokeMaterial.SetColor("_Color", brushStrokeColor);
        strokeMaterial.SetVector("_Size", size);
        strokeMaterial.SetBuffer("strokeDataBuffer", strokeBuffer);
        strokeMaterial.SetBuffer("quadPointsBuffer", quadPointsBuffer);
        buf.SetGlobalTexture("_ColorReadTex", colorReadID);
        buf.SetGlobalTexture("_DepthReadTex", depthReadID);
        buf.SetRenderTarget(colorWriteID);
        buf.SetGlobalTexture("_FrameBufferTexture", BuiltinRenderTextureType.CameraTarget); // Copy the Contents of FrameBuffer into brushstroke material so it knows what color it should be
        buf.DrawProcedural(Matrix4x4.identity, strokeMaterial, 0, MeshTopology.Triangles, 6, strokeBuffer.count);   // Apply brushstrokes

        // DISPLAY TO SCREEN:
        buf.Blit(colorWriteID, BuiltinRenderTextureType.CameraTarget);   // copy canvas target into main displayTarget so it is what the player sees

        // apply the commandBuffer
        cam.AddCommandBuffer(CameraEvent.AfterFinalPass, buf);  
        

        
        //buf.SetRenderTarget(canvasRT);
        //buf.ClearRenderTarget(true, true, Color.black, 1.0f);
        //int canvasID = Shader.PropertyToID("_CanvasTexture");
        //int tempID = Shader.PropertyToID("_TempTexture");
        //buf.GetTemporaryRT(canvasID, -1, -1, 0, FilterMode.Bilinear);  // Create a Temporary RenderTarget for the "canvas"
        //buf.GetTemporaryRT(tempID, -1, -1, 0, FilterMode.Bilinear);  // Create a Temporary RenderTarget for the "canvas"
        //buf.SetRenderTarget(canvasID);  // Set commandBuffer target to this "canvas" so the DrawProcedural will apply to this
        //buf.ClearRenderTarget(true, true, Color.white, 1.0f);  // Clear the target each frame and rebuild
        //buf.Blit(canvasID, tempID);  // copy into temporary buffer
        //buf.Blit(tempID, canvasID, gessoBlitMaterial);  // copy back into renderTarget, apply Gesso Primer
        //buf.SetGlobalTexture("_FrameBufferTexture", BuiltinRenderTextureType.CameraTarget);  // Copy the Contents of FrameBuffer into brushstroke material so it knows what color it should be
        //buf.DrawProcedural(Matrix4x4.identity, strokeMaterial, 0, MeshTopology.Triangles, 6, strokeBuffer.count);   // Apply brushstrokes

        // MRT example:
        //RenderTargetIdentifier[] mrt = { BuiltinRenderTextureType.GBuffer0, BuiltinRenderTextureType.GBuffer2 };
        //buf.SetRenderTarget(mrt, BuiltinRenderTextureType.CameraTarget);

        //buf.Blit(canvasID, BuiltinRenderTextureType.CameraTarget);   // copy canvas target into main displayTarget so it is what the player sees

        //Material testMat = new Material(Shader.Find("Unlit/Color"));
        //testMat.color = Color.yellow;
        //buf.DrawMesh(CreateFullscreenQuad(mainCam), Matrix4x4.identity, testMat);

        //buf.ReleaseTemporaryRT(colorReadID);
        //buf.ReleaseTemporaryRT(colorWriteID);
        //buf.ReleaseTemporaryRT(depthReadID);
        //buf.ReleaseTemporaryRT(depthWriteID);
    }

    // Update is called once per frame
    void Update () {
        strokeMaterial.SetColor("_Color", brushStrokeColor);
        strokeMaterial.SetVector("_Size", size);
    }

    void Cleanup() {
        foreach (var cam in m_Cameras) {
            if (cam.Key) {
                cam.Key.RemoveCommandBuffer(CameraEvent.AfterFinalPass, cam.Value);
            }
        }
        m_Cameras.Clear();
        //Object.DestroyImmediate(m_Material);

        strokeBuffer.Release();
        strokeBuffer.Dispose();

        quadPointsBuffer.Release();
        quadPointsBuffer.Dispose();
    }

    void OnEnable() {
        //Cleanup();
    }

    void OnDisable() {
        Cleanup();
    }

    void OnDestroy() {
        Cleanup();
    }
}
