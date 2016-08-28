using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class TrainerRenderManager : MonoBehaviour {

    public TrainerCritterMarchingCubes trainerCritterMarchingCubes;
    public TrainerCritterBrushstrokeManager trainerCritterBrushstrokeManager;

    public Camera mainCamera;

    public Shader canvasShader;
    private Material canvasMaterial;

    public Shader brushWorldSpaceShader;
    public Shader brushScreenSpaceShader;
    public Shader brushCritterShader;

    public Texture canvasDepthTexture;
    public Texture brushstrokeGessoTexture;
    public Texture brushstrokeBackgroundTexture;
    public Texture brushstrokeCritterTexture;
    public Texture brushstrokeDecorationsTexture;

    private Material brushstrokeGessoMaterial;
    private Material brushstrokeBackgroundMaterial;
    public Material brushstrokeCritterMaterial;
    private Material brushstrokeDecorationsMaterial;

    public Color canvasColor = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeGessoTint = new Color(0.8f, 0.7f, 0.6f, 1f);
    public Color brushstrokeBackgroundTint = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeCritterTint = new Color(1f, 1f, 1f, 1f);

    public Vector2 brushSizeGesso = Vector2.one;
    public Vector2 brushSizeBackground = Vector2.one;
    public Vector2 brushSizeCritter = Vector2.one;
    public Vector2 brushSizeDecorations = Vector2.one;

    public float paintThicknessGesso = 0.25f;
    public float paintReachGesso = 1f;
    public float paintThicknessBackground = 0.25f;
    public float paintReachBackground = 1f;
    public float paintThicknessCritter = 0.25f;
    public float paintReachCritter = 1f;
    public float paintThicknessDecorations = 0.25f;
    public float paintReachDecorations = 1f;

    private ComputeBuffer quadPointsBuffer;
    private ComputeBuffer gessoStrokesBuffer;
    private ComputeBuffer backgroundStrokesBuffer;
    //private ComputeBuffer critterStrokesBuffer;  // The ComputeBuffers for this will be stored in the TrainerCritterBrushstrokeManager subclass
    // That class will store the buffers and have functions to update them, but will be affecting the 'brushstrokeCritterMaterial' that lives here!
    private ComputeBuffer decorationsStrokesBuffer;

    private strokeStruct[] strokeGessoArray;
    private strokeStruct[] strokeBackgroundArray;
    private strokeStruct[] strokeCritterArray;
    private strokeStruct[] strokeDecorationsArray;

    private Camera m_Cam;
    private Dictionary<Camera, CommandBuffer> m_Cameras = new Dictionary<Camera, CommandBuffer>();

    private CommandBuffer cmdBuffer;

    private Mesh fullscreenQuadMesh;  // is this still needed?

    public struct strokeStruct {
        public Vector3 pos;
        public Vector3 col;
        public Vector3 normal;
    }

    private bool isActiveAgent = false;
    
    // Use this for initialization
    void Start() {
        InitializeOnStartup();
    }
    
    // Update is called once per frame
    void Update() {
               
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
        m_Cameras[cam] = cmdBuffer;  // fill in dictionary entry for this Camera

        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // Use brushstrokeManager Here:
        // Update Critter xForms  -- or do this through Trainer so it's sync'ed with FixedUpdate?
        // 
        trainerCritterBrushstrokeManager.UpdateBuffersAndMaterial(ref brushstrokeCritterMaterial);
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!

        // START!!!
        // Canvas First:
        canvasMaterial.SetColor("_Color", canvasColor);  // initialize canvas        
        //canvasMaterial.SetTexture("_DepthTex", canvasDepthTex);
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
        cmdBuffer.DrawMesh(UpdateFullscreenQuad(mainCamera, fullscreenQuadMesh), Matrix4x4.identity, canvasMaterial);   // Write into canvas Color & Depth buffers
        // Copy results into Read buffers for next pass:
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);
                
        // GessoLayer:  // Re-Implement This when I figure out how to create the brushstrokes buffer properly!!!!
        //brushtrokeWorldMaterial.SetPass(0);
        brushstrokeGessoMaterial.SetColor("_Tint", brushstrokeGessoTint);  // Setup Material Properties:
        brushstrokeGessoMaterial.SetVector("_Size", brushSizeGesso);
        brushstrokeGessoMaterial.SetFloat("_PaintThickness", paintThicknessGesso);
        brushstrokeGessoMaterial.SetFloat("_PaintReach", paintReachGesso);
        brushstrokeGessoMaterial.SetFloat("_UseSourceColor", 0.0f);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
        //cmdBuffer.SetGlobalFloat("_UseSourceColor", 1.0f);
        brushstrokeGessoMaterial.SetTexture("_BrushTex", brushstrokeGessoTexture);
        AddPaintLayer(mrt, renderedSceneID, colorReadID, depthReadID, gessoStrokesBuffer, brushstrokeGessoMaterial);
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);  // Try transferring Height values here

        // BackgroundLayer:
        brushstrokeBackgroundMaterial.SetColor("_Tint", brushstrokeBackgroundTint); // brushstrokeBackgroundTint);  // Setup Material Properties:
        brushstrokeBackgroundMaterial.SetVector("_Size", brushSizeBackground);
        brushstrokeBackgroundMaterial.SetFloat("_PaintThickness", paintThicknessBackground);
        brushstrokeBackgroundMaterial.SetFloat("_PaintReach", paintReachBackground);
        brushstrokeBackgroundMaterial.SetFloat("_UseSourceColor", 1.0f);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
        brushstrokeBackgroundMaterial.SetTexture("_BrushTex", brushstrokeBackgroundTexture);
        AddPaintLayer(mrt, renderedSceneID, colorReadID, depthReadID, backgroundStrokesBuffer, brushstrokeBackgroundMaterial);
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);

        // Critter Layer:
        if(isActiveAgent) {
            brushstrokeCritterMaterial.SetColor("_Tint", brushstrokeCritterTint);
            brushstrokeCritterMaterial.SetVector("_Size", brushSizeCritter);
            brushstrokeCritterMaterial.SetFloat("_PaintThickness", paintThicknessCritter);
            brushstrokeCritterMaterial.SetFloat("_PaintReach", paintReachCritter);
            brushstrokeCritterMaterial.SetFloat("_UseSourceColor", 0.0f);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
            brushstrokeCritterMaterial.SetTexture("_BrushTex", brushstrokeCritterTexture);
            
            brushstrokeCritterMaterial.SetPass(0);
            //strokeMaterial.SetBuffer("strokeDataBuffer", strokeBuffer);
            brushstrokeCritterMaterial.SetBuffer("quadPointsBuffer", quadPointsBuffer);
            cmdBuffer.SetGlobalTexture("_CanvasColorReadTex", colorReadID);
            cmdBuffer.SetGlobalTexture("_CanvasDepthReadTex", depthReadID);
            cmdBuffer.SetRenderTarget(mrt, BuiltinRenderTextureType.CameraTarget);  // Set render Targets
            cmdBuffer.SetGlobalTexture("_BrushColorReadTex", renderedSceneID); // Copy the Contents of FrameBuffer into brushstroke material so it knows what color it should be
            cmdBuffer.DrawProcedural(Matrix4x4.identity, brushstrokeCritterMaterial, 0, MeshTopology.Triangles, 6, trainerCritterBrushstrokeManager.critterBrushstrokesArray.Length);   // Apply brushstrokes
            cmdBuffer.Blit(colorWriteID, colorReadID);
            cmdBuffer.Blit(depthWriteID, depthReadID);
            // Won't be able to use the function until I switch from using Geometry Shader to instancing!!!
            //AddPaintLayer(mrt, renderedSceneID, colorReadID, depthReadID, critterStrokesBuffer, brushstrokeCritterMaterial);            
        }

        // DISPLAY TO SCREEN:
        cmdBuffer.Blit(colorWriteID, BuiltinRenderTextureType.CameraTarget);   // copy canvas target into main displayTarget so it is what the player sees
        
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

    public void InitializeNewAgent(Critter critter) {
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // Use brushstrokeManager Here:
        // Populate Critter Brushstroke Points Initial Positions Buffer
        // Compute Skinning Data (Weights & Indices)  so that it's ready for the renderShader to transform each brushstroke properly
        //trainerCritterBrushstrokeManager.critter = critter; // set critter so it can update xForms??? -- might be unnecessary if I do this from Trainer class
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!
        // &&&&&&&&&&#$$$$$$$$$$%&#$%&#$%&#$%&#$%^&#%^&#^&#^&#&^#&%^&##################%^&#%^&#%^&#%^&#^&#%^&#$%!#$%!#%$@&%^*($^()$*@$#%!%!%!$#%!

        trainerCritterMarchingCubes.SetCritterTransformArray(critter);
        trainerCritterBrushstrokeManager.critter = critter;
        trainerCritterMarchingCubes.BuildMesh(ref trainerCritterBrushstrokeManager);  // This call also initializes trainerCritterBrushstrokeManager's buffers
        trainerCritterBrushstrokeManager.InitializeMaterialBuffers(ref brushstrokeCritterMaterial);  // Then those buffers are set to the actual Material here (needed reference to this class)
        //critterStrokesBuffer = trainerCritterMarchingCubes.critterPointsBuffer;
                
        isActiveAgent = true;


        

        /*
        // Need to Setup Arrays and ComputeBuffers HERE!!!
        // Make sure this is called when a new agent is created, since the vertCounts could be different & break shit!
        
        strokeCritterArray = new strokeStruct[numStrokes];
        strokeDecorationsArray = new strokeStruct[numStrokes];

        // DO I NEED TO RELEASE these commandBuffers before re-creating them?
        critterStrokesBuffer = new ComputeBuffer(strokeCritterArray.Length, sizeof(float) * 3);
        critterStrokesBuffer.SetData(strokeCritterArray);
        decorationsStrokesBuffer = new ComputeBuffer(strokeDecorationsArray.Length, sizeof(float) * 3);
        decorationsStrokesBuffer.SetData(strokeDecorationsArray);
        */
    }

    public void ClearAgent() {
        trainerCritterMarchingCubes.ClearCritterMesh();
        isActiveAgent = false;

        //if (critterStrokesBuffer != null) {
        //    critterStrokesBuffer.Release();
        //    critterStrokesBuffer.Dispose();
        //}
    }

    void InitializeOnStartup() {
        fullscreenQuadMesh = new Mesh();
        
        // Create Materials        
        canvasMaterial = new Material(canvasShader);
        brushstrokeGessoMaterial = new Material(brushWorldSpaceShader);
        brushstrokeBackgroundMaterial = new Material(brushWorldSpaceShader);
        brushstrokeCritterMaterial = new Material(brushCritterShader);
        brushstrokeDecorationsMaterial = new Material(brushWorldSpaceShader);

        // GESSO BUFFER:
        int gessoRes = 12;
        Vector3[] gesso1 = PointCloudSphericalShell.GetPointsSphericalShell(50f, gessoRes, 1f);
        Vector3[] gesso2 = PointCloudSphericalShell.GetPointsSphericalShell(50f, gessoRes, 1f);
        Vector3[] gesso3 = PointCloudSphericalShell.GetPointsSphericalShell(50f, gessoRes, 1f);        
        strokeGessoArray = new strokeStruct[gesso1.Length + gesso2.Length + gesso3.Length];
        for(int i = 0; i < gesso1.Length; i++) {
            strokeGessoArray[i].pos = gesso1[i];
            strokeGessoArray[i].col = new Vector3(1f, 1f, 1f);
            strokeGessoArray[i].normal = -gesso1[i].normalized;
            strokeGessoArray[i + gesso1.Length].pos = gesso2[i];
            strokeGessoArray[i + gesso1.Length].col = new Vector3(1f, 1f, 1f);
            strokeGessoArray[i + gesso1.Length].normal = -gesso2[i].normalized;
            strokeGessoArray[i + gesso1.Length + gesso2.Length].pos = gesso3[i];
            strokeGessoArray[i + gesso1.Length + gesso2.Length].col = new Vector3(1f, 1f, 1f);
            strokeGessoArray[i + gesso1.Length + gesso2.Length].normal = -gesso3[i].normalized;
        }
        gessoStrokesBuffer = new ComputeBuffer(strokeGessoArray.Length, sizeof(float) * (3 + 3 + 3));
        gessoStrokesBuffer.SetData(strokeGessoArray);

        // BACKGROUND BUFFER:
        int backgroundRes = 24;
        Vector3[] background1 = PointCloudSphericalShell.GetPointsSphericalShell(50f, backgroundRes, 1f);
        Vector3[] background2 = PointCloudSphericalShell.GetPointsSphericalShell(50f, backgroundRes, 1f);
        Vector3[] background3 = PointCloudSphericalShell.GetPointsSphericalShell(50f, backgroundRes, 1f);
        strokeBackgroundArray = new strokeStruct[background1.Length + background2.Length + background3.Length];
        for (int i = 0; i < background1.Length; i++) {
            strokeBackgroundArray[i].pos = background1[i];
            strokeBackgroundArray[i].col = new Vector3(1f, 1f, 1f);
            strokeBackgroundArray[i].normal = -background1[i].normalized;
            strokeBackgroundArray[i + background1.Length].pos = background2[i];
            strokeBackgroundArray[i + background1.Length].col = new Vector3(1f, 1f, 1f);
            strokeBackgroundArray[i + background1.Length].normal = -background2[i].normalized;
            strokeBackgroundArray[i + background1.Length + background2.Length].pos = background3[i];
            strokeBackgroundArray[i + background1.Length + background2.Length].col = new Vector3(1f, 1f, 1f);
            strokeBackgroundArray[i + background1.Length + background2.Length].normal = -background3[i].normalized;
        }
        backgroundStrokesBuffer = new ComputeBuffer(strokeBackgroundArray.Length, sizeof(float) * (3 + 3 + 3));
        backgroundStrokesBuffer.SetData(strokeBackgroundArray);

        // Experimental!
        //critterStrokesBuffer = new ComputeBuffer(0, sizeof(float) * 3);

        //Create quad buffer for brushtroke billboard
        quadPointsBuffer = new ComputeBuffer(6, sizeof(float) * 3);
        quadPointsBuffer.SetData(new[] {
            new Vector3(-0.5f, 0.5f),
            new Vector3(0.5f, 0.5f),
            new Vector3(0.5f, -0.5f),
            new Vector3(0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f)
        });

        // Create master commandBuffer that makes the magic happen
        cmdBuffer = new CommandBuffer();
        cmdBuffer.name = "cmdBuffer";
    }

    void Cleanup() {
        foreach (var cam in m_Cameras) {
            if (cam.Key) {
                cam.Key.RemoveCommandBuffer(CameraEvent.AfterSkybox, cam.Value);
            }
        }
        m_Cameras.Clear();
        //Object.DestroyImmediate(m_Material);

        if(gessoStrokesBuffer != null) {
            gessoStrokesBuffer.Release();
            gessoStrokesBuffer.Dispose();
        }        
        if(backgroundStrokesBuffer != null) {
            backgroundStrokesBuffer.Release();
            backgroundStrokesBuffer.Dispose();
        }
        //if (critterStrokesBuffer != null) {
        //    critterStrokesBuffer.Release();
        //    critterStrokesBuffer.Dispose();
        //}
        if (decorationsStrokesBuffer != null) {
            decorationsStrokesBuffer.Release();
            decorationsStrokesBuffer.Dispose();
        }

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

    private Mesh UpdateFullscreenQuad(Camera cam, Mesh quadMesh) {
        
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
}
