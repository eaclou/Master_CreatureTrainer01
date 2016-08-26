using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class TestImpressionismRender02 : MonoBehaviour {

    public Camera mainCam;

    public Shader depthBlitShader;
    private Material depthBlitMaterial;
    public Shader canvasShader;
    public Texture canvasDepthTex;
    private Material canvasMaterial;

    public Texture brushstrokeGessoTexture;
    public Texture brushstrokeMidgroundTexture;
    public Shader brushstrokeWorldShader;
    private Material brushtrokeGessoMaterial;
    private Material brushtrokeMidgroundMaterial;
    public Shader brushstrokeScreenShader;
    private Material brushtrokeScreenMaterial;

    public Color canvasColor = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeGessoTint = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeMidgroundTint = new Color(1f, 1f, 1f, 1f);
    public Vector2 brushSizeGesso = Vector2.one;
    public Vector2 brushSizeMidground = Vector2.one;
    public float paintThicknessGesso = 0.25f;
    public float paintReachGesso = 0.25f;
    public float paintThicknessMidground = 0.25f;
    public float paintReachMidground = 0.25f;

    private ComputeBuffer quadPointsBuffer;
    private ComputeBuffer gessoStrokesBuffer;
    private ComputeBuffer midgroundStrokesBuffer;

    //public Vector3 brushStrokesWorldOffset = new Vector3(0, 0, 0);
    private strokeStruct[] strokeGessoArray;
    private strokeStruct[] strokeMidgroundArray;

    public Vector3 brushStrokesWorldOffset = new Vector3(0, 0, 0);

    // CommandBuffer Stuff:
    private Camera m_Cam;
    // We'll want to add a command buffer on any camera that renders us,
    // so have a dictionary of them.
    private Dictionary<Camera, CommandBuffer> m_Cameras = new Dictionary<Camera, CommandBuffer>();

    private CommandBuffer cmdBuffer;

    private Mesh fullscreenQuadMesh;

    public struct strokeStruct {
        public Vector3 pos;
    }    

    // Use this for initialization
    void Start () {
        Initialize();
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

        cmdBuffer.Clear();  // Clear CommandBuffer

        // Did we already add the command buffer on this camera? Nothing to do then.
        if (m_Cameras.ContainsKey(cam)) {
            //return;
        }
        else {
            cam.AddCommandBuffer(CameraEvent.AfterSkybox, cmdBuffer);
        }

        //if (!m_Material) {
        //    m_Material = new Material(m_BlurShader);
        //    m_Material.hideFlags = HideFlags.HideAndDontSave;  // not sure what this does -- prevents garbage collection??
        //}

        //buf = new CommandBuffer();
        //buf.name = "TestDrawProcedural";
        m_Cameras[cam] = cmdBuffer;  // fill in dictionary entry for this Camera

        // START!!!
        // Canvas First:
        canvasMaterial.SetColor("_Color", canvasColor);  // initialize canvas        
        canvasMaterial.SetTexture("_DepthTex", canvasDepthTex);
        canvasMaterial.SetFloat("_MaxDepth", 1.0f);

        // Create RenderTargets:
        int renderedSceneID = Shader.PropertyToID("_RenderedSceneID");
        int colorReadID = Shader.PropertyToID("_ColorTextureRead");
        int colorWriteID = Shader.PropertyToID("_ColorTextureWrite");
        int depthReadID = Shader.PropertyToID("_DepthTextureRead");
        int depthWriteID = Shader.PropertyToID("_DepthTextureWrite");
        cmdBuffer.GetTemporaryRT(renderedSceneID, -1, -1, 0, FilterMode.Bilinear);  // save contents of Standard Rendering Pipeline
        cmdBuffer.GetTemporaryRT(colorReadID, -1, -1, 0, FilterMode.Bilinear);
        cmdBuffer.GetTemporaryRT(colorWriteID, -1, -1, 0, FilterMode.Bilinear);
        cmdBuffer.GetTemporaryRT(depthReadID, -1, -1, 0, FilterMode.Bilinear);
        cmdBuffer.GetTemporaryRT(depthWriteID, -1, -1, 0, FilterMode.Bilinear);

        cmdBuffer.Blit(BuiltinRenderTextureType.CameraTarget, renderedSceneID);  // save contents of Standard Rendering Pipeline

        RenderTargetIdentifier[] mrt = { colorWriteID, depthWriteID };  // Define multipleRenderTarget so I can render to color AND depth
        cmdBuffer.SetRenderTarget(mrt, BuiltinRenderTextureType.CameraTarget);  // Set render Targets
        cmdBuffer.ClearRenderTarget(true, true, Color.white, 1.0f);  // clear -- needed???
        cmdBuffer.DrawMesh(UpdateFullscreenQuad(mainCam, fullscreenQuadMesh), Matrix4x4.identity, canvasMaterial);   // Write into canvas Color & Depth buffers
        // Copy results into Read buffers for next pass:
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);

        // GessoLayer:
        //brushtrokeWorldMaterial.SetPass(0);
        brushtrokeGessoMaterial.SetColor("_Tint", brushstrokeGessoTint);  // Setup Material Properties:
        brushtrokeGessoMaterial.SetVector("_Size", brushSizeGesso);
        brushtrokeGessoMaterial.SetFloat("_PaintThickness", paintThicknessGesso);
        brushtrokeGessoMaterial.SetFloat("_PaintReach", paintReachGesso);
        brushtrokeGessoMaterial.SetFloat("_UseSourceColor", 0.0f);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
        //cmdBuffer.SetGlobalFloat("_UseSourceColor", 1.0f);
        brushtrokeGessoMaterial.SetTexture("_BrushTex", brushstrokeGessoTexture);
        AddPaintLayer(mrt, renderedSceneID, colorReadID, depthReadID, gessoStrokesBuffer, brushtrokeGessoMaterial);
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);  // Try transferring Height values here

        // MidgroundLayer:
        brushtrokeMidgroundMaterial.SetColor("_Tint", brushstrokeMidgroundTint);  // Setup Material Properties:
        brushtrokeMidgroundMaterial.SetVector("_Size", brushSizeMidground);
        brushtrokeMidgroundMaterial.SetFloat("_PaintThickness", paintThicknessMidground);
        brushtrokeMidgroundMaterial.SetFloat("_PaintReach", paintReachMidground);
        brushtrokeMidgroundMaterial.SetFloat("_UseSourceColor", 1.0f);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
        brushtrokeMidgroundMaterial.SetTexture("_BrushTex", brushstrokeMidgroundTexture);
        AddPaintLayer(mrt, renderedSceneID, colorReadID, depthReadID, midgroundStrokesBuffer, brushtrokeMidgroundMaterial);
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);

        // DISPLAY TO SCREEN:
        cmdBuffer.Blit(colorWriteID, BuiltinRenderTextureType.CameraTarget);   // copy canvas target into main displayTarget so it is what the player sees


        /*
        // Gesso/Primer Pass:
        gessoMaterial.SetPass(0);
        gessoMaterial.SetColor("_Color", new Color(0.19f, 0.192f, 0.194f, 1.0f));
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
        */
    }

    void AddPaintLayer(RenderTargetIdentifier[] mrt, int sceneRenderID, int colorReadID, int depthReadID, ComputeBuffer strokeBuffer, Material strokeMaterial) {
        // MAIN BRUSHSTROKE CONTENTS PASS!!!:    
        strokeMaterial.SetPass(0);
        strokeMaterial.SetBuffer("strokeDataBuffer", strokeBuffer);
        strokeMaterial.SetBuffer("quadPointsBuffer", quadPointsBuffer);
        cmdBuffer.SetGlobalTexture("_CanvasColorReadTex", colorReadID);
        cmdBuffer.SetGlobalTexture("_CanvasDepthReadTex", depthReadID);
        cmdBuffer.SetRenderTarget(mrt, BuiltinRenderTextureType.CameraTarget);  // Set render Targets
        cmdBuffer.SetGlobalTexture("_BrushColorReadTex", sceneRenderID); // Copy the Contents of FrameBuffer into brushstroke material so it knows what color it should be
        cmdBuffer.DrawProcedural(Matrix4x4.identity, strokeMaterial, 0, MeshTopology.Triangles, 6, strokeBuffer.count);   // Apply brushstrokes
    }

    void Initialize() {
        fullscreenQuadMesh = new Mesh();

        depthBlitMaterial = new Material(depthBlitShader);
        canvasMaterial = new Material(canvasShader);
        brushtrokeGessoMaterial = new Material(brushstrokeWorldShader);
        brushtrokeMidgroundMaterial = new Material(brushstrokeWorldShader);

        int numStrokes = 32;
        Vector3 dimensions = new Vector3(40f, 32f, 20f);
        strokeGessoArray = new strokeStruct[numStrokes * numStrokes * numStrokes];
        strokeMidgroundArray = new strokeStruct[numStrokes * numStrokes * numStrokes];
        int index = 0;
        for (int x = 0; x < numStrokes; x++) {
            for (int y = 0; y < numStrokes; y++) {
                for (int z = 0; z < numStrokes; z++) {
                    strokeGessoArray[index].pos = new Vector3(((float)x / (float)numStrokes) * dimensions.x, ((float)y / (float)numStrokes) * dimensions.y, ((float)z / (float)numStrokes) * dimensions.z) + brushStrokesWorldOffset + UnityEngine.Random.insideUnitSphere * (dimensions.magnitude / (float)numStrokes);
                    strokeMidgroundArray[index].pos = new Vector3(((float)x / (float)numStrokes) * dimensions.x, ((float)y / (float)numStrokes) * dimensions.y, ((float)z / (float)numStrokes) * dimensions.z) + brushStrokesWorldOffset + UnityEngine.Random.insideUnitSphere * (dimensions.magnitude / (float)numStrokes);
                    index++;
                }
            }
        }

        gessoStrokesBuffer = new ComputeBuffer(strokeGessoArray.Length, sizeof(float) * 3);
        gessoStrokesBuffer.SetData(strokeGessoArray);
        midgroundStrokesBuffer = new ComputeBuffer(strokeMidgroundArray.Length, sizeof(float) * 3);
        midgroundStrokesBuffer.SetData(strokeMidgroundArray);

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

        cmdBuffer = new CommandBuffer();
        cmdBuffer.name = "cmdBuffer";
    }

    private Mesh UpdateFullscreenQuad(Camera cam, Mesh quadMesh) {
        //Debug.Log("CreateQuad!");
        //Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane + 0.1f));
        vertices[1] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, cam.nearClipPlane + 0.1f));
        vertices[2] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, cam.nearClipPlane + 0.1f));
        vertices[3] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane + 0.1f));

        Vector2[] uvs = new Vector2[4] {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f)
        };

        int[] triangles = new int[6] {
            0, 3, 1, 0, 2, 3
        };

        quadMesh.vertices = vertices;
        quadMesh.uv = uvs;
        quadMesh.triangles = triangles;

        return quadMesh;
    }

    // Update is called once per frame
    void Update () {
        //strokeMaterial.SetColor("_Color", brushStrokeColor);
        //strokeMaterial.SetVector("_Size", size);
    }

    void Cleanup() {
        foreach (var cam in m_Cameras) {
            if (cam.Key) {
                cam.Key.RemoveCommandBuffer(CameraEvent.AfterSkybox, cam.Value);
            }
        }
        m_Cameras.Clear();
        //Object.DestroyImmediate(m_Material);

        gessoStrokesBuffer.Release();
        gessoStrokesBuffer.Dispose();

        midgroundStrokesBuffer.Release();
        midgroundStrokesBuffer.Dispose();

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
