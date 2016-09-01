using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class TrainerRenderManager : MonoBehaviour {

    public TrainerCritterMarchingCubes trainerCritterMarchingCubes;
    public TrainerCritterBrushstrokeManager trainerCritterBrushstrokeManager;
    public TrainerTerrainManager trainerTerrainManager;

    public ComputeShader terrainGeneratorCompute;
    
    public Camera mainCamera;
    //public Cubemap skyboxTexture;

    public Shader canvasShader;
    private Material canvasMaterial;

    public Shader brushWorldSpaceShader;
    public Shader brushTerrainShader;
    public Shader brushGessoShader;
    public Shader brushCritterShader;

    public Texture canvasDepthTexture;
    public Texture brushstrokeGessoTexture;
    public Texture brushstrokeBackgroundTexture;
    public Texture brushstrokeTerrainTexture;
    public Texture brushstrokeCritterTexture;
    public Texture brushstrokeDecorationsTexture;

    private Material brushstrokeGessoMaterial;
    private Material brushstrokeBackgroundMaterial;
    private Material brushstrokeTerrainMaterial;
    public Material brushstrokeCritterMaterial;
    private Material brushstrokeDecorationsMaterial;

    public Color canvasColor = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeGessoTint = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeBackgroundTint = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeTerrainTint = new Color(1f, 1f, 1f, 1f);
    public Color brushstrokeCritterTint = new Color(1f, 1f, 1f, 1f);

    public Vector2 brushSizeGesso = Vector2.one;
    public Vector2 brushSizeBackground = Vector2.one;
    public Vector2 brushSizeTerrain = Vector2.one;
    public Vector2 brushSizeCritter = Vector2.one;
    public Vector2 brushSizeDecorations = Vector2.one;

    public float paintThicknessGesso = 0.25f;
    public float paintReachGesso = 1f;
    public float paintThicknessBackground = 0.25f;
    public float paintReachBackground = 1f;    
    public float backgroundFramebufferColor = 1f;
    public float paintThicknessTerrain = 0.25f;
    public float paintReachTerrain = 1f;
    public float terrainFramebufferColor = 1f;
    public float paintThicknessCritter = 0.25f;
    public float paintReachCritter = 1f;
    public float paintThicknessDecorations = 0.25f;
    public float paintReachDecorations = 1f;

    private ComputeBuffer quadPointsBuffer;
    private ComputeBuffer gessoStrokesBuffer;
    private ComputeBuffer backgroundStrokesBuffer;
    private ComputeBuffer terrainStrokesBuffer;
    private ComputeBuffer terrainMeshBuffer;
    //private ComputeBuffer critterStrokesBuffer;  // The ComputeBuffers for this will be stored in the TrainerCritterBrushstrokeManager subclass
    // That class will store the buffers and have functions to update them, but will be affecting the 'brushstrokeCritterMaterial' that lives here!
    private ComputeBuffer decorationsStrokesBuffer;

    private strokeStruct[] strokeGessoArray;
    private strokeStruct[] strokeBackgroundArray;
    private strokeStruct[] strokeTerrainArray;
    private strokeStruct[] strokeCritterArray;
    private strokeStruct[] strokeDecorationsArray;

    private Camera m_Cam;
    private Dictionary<Camera, CommandBuffer> m_Cameras = new Dictionary<Camera, CommandBuffer>();
    
    private CommandBuffer cmdBuffer;

    private Mesh fullscreenQuadMesh;  // is this still needed?

    public struct strokeStruct {
        public Vector3 pos;  // pivot point of stroke
        public Vector3 col;   // base color of stroke
        public Vector3 normal;   // 3d facing direction of stroke
        public Vector3 tangent;  // direction of stroke in 3-dimensional space
        public Vector3 prevPos;   // previousPosition, to help with velocity
        public Vector2 dimensions;  // brush Width/Height
        public int brushType;  // type of brushStroke -- 0 = default, 1 = flat-press, 2 = dot, 3 = splatter? -- maps to TextureUV (different rows are different strokeTypes, columns are variations)
    }

    public struct meshData {
        public Vector3 pos;
        public Vector3 normal;
        public Vector2 uv;
        public Color color;
    };

    private bool isActiveAgent = false;

    public float skyNoiseFrequency = 0.02f;
    private Vector3 skyNoiseOffset = new Vector3(0f, 0f, 0f);
    public Vector3 skyNoiseScroll = new Vector3(0f, 0f, 0f);
    public float terrainNoiseFrequency = 0.1f;
    public float terrainNoiseAmplitude = 2f;
    public float terrainSize = 32f;

    public Gradient skyColorGradient;
    public Gradient terrainColorGradient;

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
        
        cmdBuffer.Clear();

        // Did we already add the command buffer on this camera? Nothing to do then.
        if (m_Cameras.ContainsKey(cam)) {
            //return;
        }
        else {
            if(cam == mainCamera) {
                cam.AddCommandBuffer(CameraEvent.AfterSkybox, cmdBuffer);
                m_Cameras[cam] = cmdBuffer;  // fill in dictionary entry for this Camera
            }            
        }

        // Use brushstrokeManager Here:
        // Update Critter xForms  -- or do this through Trainer so it's sync'ed with FixedUpdate?
        UpdateSkyTangents();
        trainerCritterBrushstrokeManager.UpdateBuffersAndMaterial(ref brushstrokeCritterMaterial);
        

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
        brushstrokeBackgroundMaterial.SetFloat("_UseSourceColor", backgroundFramebufferColor);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
        brushstrokeBackgroundMaterial.SetTexture("_BrushTex", brushstrokeBackgroundTexture);
        AddPaintLayer(mrt, renderedSceneID, colorReadID, depthReadID, backgroundStrokesBuffer, brushstrokeBackgroundMaterial);
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);

        // TerrainLayer:
        brushstrokeTerrainMaterial.SetColor("_Tint", brushstrokeTerrainTint);
        brushstrokeTerrainMaterial.SetVector("_Size", brushSizeTerrain);
        brushstrokeTerrainMaterial.SetFloat("_PaintThickness", paintThicknessTerrain);
        brushstrokeTerrainMaterial.SetFloat("_PaintReach", paintReachTerrain);
        brushstrokeTerrainMaterial.SetFloat("_UseSourceColor", terrainFramebufferColor);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
        brushstrokeTerrainMaterial.SetTexture("_BrushTex", brushstrokeTerrainTexture);
        AddPaintLayer(mrt, renderedSceneID, colorReadID, depthReadID, terrainStrokesBuffer, brushstrokeTerrainMaterial);
        cmdBuffer.Blit(colorWriteID, colorReadID);
        cmdBuffer.Blit(depthWriteID, depthReadID);

        // Critter Layer:
        if (isActiveAgent) {
            brushstrokeCritterMaterial.SetColor("_Tint", brushstrokeCritterTint);
            brushstrokeCritterMaterial.SetVector("_Size", brushSizeCritter);
            brushstrokeCritterMaterial.SetFloat("_PaintThickness", paintThicknessCritter);
            brushstrokeCritterMaterial.SetFloat("_PaintReach", paintReachCritter);
            brushstrokeCritterMaterial.SetFloat("_UseSourceColor", 0.0f);  // 1.0 will use Unity's scene render as color, 0.0 will just use brushTintColor
            brushstrokeCritterMaterial.SetTexture("_BrushTex", brushstrokeCritterTexture);
            // Some render settings from brushstrokeManager:
            brushstrokeCritterMaterial.SetFloat("_Diffuse", trainerCritterBrushstrokeManager.diffuse);
            brushstrokeCritterMaterial.SetFloat("_DiffuseWrap", trainerCritterBrushstrokeManager.diffuseWrap);
            brushstrokeCritterMaterial.SetFloat("_RimGlow", trainerCritterBrushstrokeManager.rimGlow);
            brushstrokeCritterMaterial.SetFloat("_RimPow", trainerCritterBrushstrokeManager.rimPow);

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
        
        // Use brushstrokeManager Here:
        // Populate Critter Brushstroke Points Initial Positions Buffer        

        trainerCritterMarchingCubes.SetCritterTransformArray(critter);
        trainerCritterBrushstrokeManager.critter = critter;
        trainerCritterMarchingCubes.BuildMesh(ref trainerCritterBrushstrokeManager);  // This call also initializes trainerCritterBrushstrokeManager's buffers
        trainerCritterBrushstrokeManager.InitializeMaterialBuffers(ref brushstrokeCritterMaterial);  // Then those buffers are set to the actual Material here (needed reference to this class)
        //critterStrokesBuffer = trainerCritterMarchingCubes.critterPointsBuffer;
                
        isActiveAgent = true;        
    }

    public void ClearAgent() {
        trainerCritterMarchingCubes.ClearCritterMesh();
        isActiveAgent = false;
        
    }

    void InitializeOnStartup() {
        fullscreenQuadMesh = new Mesh();
        
        // Create Materials        
        canvasMaterial = new Material(canvasShader);
        brushstrokeGessoMaterial = new Material(brushGessoShader);
        brushstrokeBackgroundMaterial = new Material(brushWorldSpaceShader);
        brushstrokeTerrainMaterial = new Material(brushTerrainShader);
        brushstrokeCritterMaterial = new Material(brushCritterShader);
        brushstrokeDecorationsMaterial = new Material(brushWorldSpaceShader);

        // GESSO BUFFER:
        int gessoRes = 12;
        Vector3[] gesso1 = PointCloudSphericalShell.GetPointsSphericalShell(50f, gessoRes, 1f);
        Vector3[] gesso2 = PointCloudSphericalShell.GetPointsSphericalShell(55f, gessoRes, 1f);
        Vector3[] gesso3 = PointCloudSphericalShell.GetPointsSphericalShell(60f, gessoRes, 1f);        
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
        gessoStrokesBuffer = new ComputeBuffer(strokeGessoArray.Length, sizeof(float) * (3 + 3 + 3 + 3 + 3 + 2) + sizeof(int) * 1);  // col=3f, pos=3f, nml=3f, tan=3f, prevP=3f, dim=2f, type=1i
        gessoStrokesBuffer.SetData(strokeGessoArray);

        // BACKGROUND BUFFER:
        int backgroundRes = 20;
        Vector3[] background1 = PointCloudSphericalShell.GetPointsSphericalShell(50f, backgroundRes, 1f);
        Vector3[] background2 = PointCloudSphericalShell.GetPointsSphericalShell(55f, backgroundRes, 1f);
        Vector3[] background3 = PointCloudSphericalShell.GetPointsSphericalShell(60f, backgroundRes, 1f);
        strokeBackgroundArray = new strokeStruct[background1.Length + background2.Length + background3.Length];
        
        //float groundPos = -10f;
        for (int i = 0; i < background1.Length; i++) {
            background1[i].y = Mathf.Abs(background1[i].y);
            NoiseSample noiseSample = NoisePrime.Simplex3D(background1[i], skyNoiseFrequency);            
            strokeBackgroundArray[i].pos = background1[i];            
            strokeBackgroundArray[i].col = new Vector3(noiseSample.value, noiseSample.value, noiseSample.value);
            strokeBackgroundArray[i].normal = -background1[i].normalized;       
            //Vector3 cross = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);
            strokeBackgroundArray[i].tangent = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);
            strokeBackgroundArray[i].prevPos = background1[i];
            strokeBackgroundArray[i].dimensions = new Vector2(1f, noiseSample.derivative.magnitude);

            background2[i].y = Mathf.Abs(background2[i].y);
            noiseSample = NoisePrime.Simplex3D(background2[i], skyNoiseFrequency);
            strokeBackgroundArray[i + background1.Length].pos = background2[i];
            strokeBackgroundArray[i + background1.Length].col = new Vector3(noiseSample.value, noiseSample.value, noiseSample.value);           
            strokeBackgroundArray[i + background1.Length].normal = -background2[i].normalized;
            //strokeBackgroundArray[i + background1.Length].normal = -background2[i].normalized;
            //cross = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);
            strokeBackgroundArray[i + background1.Length].tangent = Vector3.Cross(strokeBackgroundArray[i + background1.Length].normal, noiseSample.derivative);
            strokeBackgroundArray[i + background1.Length].prevPos = background2[i];
            strokeBackgroundArray[i + background1.Length].dimensions = new Vector2(1f, noiseSample.derivative.magnitude);

            background3[i].y = Mathf.Abs(background3[i].y);
            noiseSample = NoisePrime.Simplex3D(background3[i], skyNoiseFrequency);            
            strokeBackgroundArray[i + background1.Length + background2.Length].pos = background3[i];
            strokeBackgroundArray[i + background1.Length + background2.Length].col = new Vector3(noiseSample.value, noiseSample.value, noiseSample.value);            
            strokeBackgroundArray[i + background1.Length + background2.Length].normal = -background1[i].normalized;            
            //cross = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);
            strokeBackgroundArray[i + background1.Length + background2.Length].tangent = Vector3.Cross(strokeBackgroundArray[i + background1.Length + background2.Length].normal, noiseSample.derivative);
            strokeBackgroundArray[i + background1.Length + background2.Length].prevPos = background3[i];
            strokeBackgroundArray[i + background1.Length + background2.Length].dimensions = new Vector2(1f, noiseSample.derivative.magnitude);
        }
        backgroundStrokesBuffer = new ComputeBuffer(strokeBackgroundArray.Length, sizeof(float) * (3 + 3 + 3 + 3 + 3 + 2) + sizeof(int) * 1);
        backgroundStrokesBuffer.SetData(strokeBackgroundArray);

        /*// TERRAIN BUFFER:
        int terrainCount = 20000;
        Vector3[] terrain1 = new Vector3[terrainCount]; //PointCloudSphericalShell.GetPointsSphericalShell(2f, backgroundRes, 1f);
        strokeTerrainArray = new strokeStruct[terrain1.Length];

        float groundPos = -5f;
        for (int i = 0; i < strokeTerrainArray.Length; i++) {
            terrain1[i] = UnityEngine.Random.insideUnitSphere * groundSpreadExponent;
            //terrain1[i] *= terrain1[i].sqrMagnitude;
            terrain1[i].y = groundPos;  // start at groundHeight
            NoiseSample noiseSample = NoisePrime.Simplex3D(new Vector3(terrain1[i].x, 0f, terrain1[i].z), groundNoiseFrequency);
            float heightOffset = noiseSample.value * groundNoiseAmplitude;
            terrain1[i].y += heightOffset;
            strokeTerrainArray[i].pos = terrain1[i];
            Color color = terrainColorGradient.Evaluate(noiseSample.value * 0.5f + 0.5f);
            strokeTerrainArray[i].col = new Vector3(color.r, color.g, color.b);
            Vector3 preTangent = noiseSample.derivative;
            preTangent.y *= groundNoiseAmplitude;
            strokeTerrainArray[i].tangent = preTangent.normalized;
            strokeTerrainArray[i].normal = new Vector3(-noiseSample.derivative.x, 1f, -noiseSample.derivative.z).normalized;
            //Vector3 cross = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);
            strokeTerrainArray[i].prevPos = strokeTerrainArray[i].prevPos;
            strokeTerrainArray[i].dimensions = new Vector2(1f, 1f);
            
        }
        terrainStrokesBuffer = new ComputeBuffer(strokeTerrainArray.Length, sizeof(float) * (3 + 3 + 3 + 3 + 3 + 2) + sizeof(int) * 1);
        terrainStrokesBuffer.SetData(strokeTerrainArray);
        */

        InitTerrain();

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

    private void InitTerrain() {
        int meshGridSize = 16;
        int numTerrainMeshVertices = meshGridSize * meshGridSize;
        terrainMeshBuffer = new ComputeBuffer(numTerrainMeshVertices, sizeof(float) * (3 + 3 + 2 + 4));
        int numStrokesPerVertX = 8;
        int numStrokesPerVertZ = 8;
        int numTerrainStrokes = meshGridSize * meshGridSize * numStrokesPerVertX * numStrokesPerVertZ;
        terrainStrokesBuffer = new ComputeBuffer(numTerrainStrokes, sizeof(float) * (3 + 3 + 3 + 3 + 3 + 2) + sizeof(int) * 1);

        //terrainGeneratorCompute = new ComputeShader();
        int kernel_id = terrainGeneratorCompute.FindKernel("CSMain");
        terrainGeneratorCompute.SetFloat("_GridSideLength", terrainSize);
        terrainGeneratorCompute.SetFloat("_NoiseFrequency", terrainNoiseFrequency);
        terrainGeneratorCompute.SetFloat("_NoiseAmplitude", terrainNoiseAmplitude);
        terrainGeneratorCompute.SetInt("_NumGroupsX", numStrokesPerVertX);
        terrainGeneratorCompute.SetInt("_NumGroupsZ", numStrokesPerVertZ);
        terrainGeneratorCompute.SetBuffer(kernel_id, "buf_StrokeData", terrainStrokesBuffer);
        terrainGeneratorCompute.SetBuffer(kernel_id, "buf_MeshData", terrainMeshBuffer);

        meshData[] meshDataArray = new meshData[numTerrainMeshVertices];  // memory to receive data from computeshader
        terrainGeneratorCompute.Dispatch(kernel_id, numStrokesPerVertX, 1, numStrokesPerVertZ);  // fill buffers

        terrainMeshBuffer.GetData(meshDataArray);  // download mesh Data

        // generate Mesh from data:
        //Construct mesh using received data         
        // Why same number of tris as vertices?  == // because all triangles have duplicate verts - no shared vertices?
        Vector3[] vertices = new Vector3[numTerrainMeshVertices];
        Color[] colors = new Color[numTerrainMeshVertices];
        int[] tris = new int[2 * (meshGridSize - 1) * (meshGridSize - 1) * 3];
        Vector2[] uvs = new Vector2[numTerrainMeshVertices];
        Vector3[] normals = new Vector3[numTerrainMeshVertices];

        for(int i = 0; i < numTerrainMeshVertices; i++) {
            vertices[i] = meshDataArray[i].pos;
            normals[i] = meshDataArray[i].normal;
            uvs[i] = meshDataArray[i].uv;
            colors[i] = meshDataArray[i].color;            
        }
        // Figure out triangles:
        int index = 0;
        int numSquares = meshGridSize - 1;
        for (int y = 0; y < numSquares; y++) {
            for(int x = 0; x < numSquares; x++) {
                // trying clockwise first:
                tris[index] = ((y + 1) * meshGridSize) + x;
                tris[index + 1] = (y * meshGridSize) + x + 1;
                tris[index + 2] = (y * meshGridSize) + x;

                tris[index + 3] = ((y + 1) * meshGridSize) + x;
                tris[index + 4] = ((y + 1) * meshGridSize) + x + 1;
                tris[index + 5] = (y * meshGridSize) + x + 1;

                index = index + 6;
            }
        }

        Mesh terrainMesh = new Mesh();
        terrainMesh.vertices = vertices;
        terrainMesh.uv = uvs; //Unwrapping.GeneratePerTriangleUV(NewMesh);
        terrainMesh.triangles = tris;
        terrainMesh.normals = normals; //NewMesh.RecalculateNormals();        
        terrainMesh.colors = colors;
        terrainMesh.RecalculateNormals();
        terrainMesh.RecalculateBounds();

        trainerTerrainManager.GetComponent<MeshFilter>().sharedMesh = terrainMesh;

        terrainMeshBuffer.Release();
        terrainMeshBuffer.Dispose();
    }

    private void UpdateSkyTangents() {
        for (int i = 0; i < strokeBackgroundArray.Length; i++) {
            //background1[i].y = Mathf.Abs(background1[i].y);
            NoiseSample noiseSample = NoisePrime.Simplex3D(strokeBackgroundArray[i].pos + skyNoiseOffset, skyNoiseFrequency);
            //float size = Vector3.Dot(noiseSample.derivative.normalized, strokeBackgroundArray[i].normal);
            noiseSample.value = noiseSample.value * 0.5f + 0.5f;
            //strokeBackgroundArray[i].pos = background1[i];
            Color color = skyColorGradient.Evaluate(Vector3.Dot(strokeBackgroundArray[i].pos.normalized, Vector3.up));
            strokeBackgroundArray[i].col = new Vector3(color.r, color.g, color.b);
            //strokeBackgroundArray[i].col = new Vector3(noiseSample.value, noiseSample.value, noiseSample.value);
            //strokeBackgroundArray[i].normal = -background1[i].normalized;
            //Vector3 cross = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);
            strokeBackgroundArray[i].tangent = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);
            //strokeBackgroundArray[i].prevPos = background1[i];
            
            //size = (1.0f - Mathf.Abs(size));  //0 = 1, 
            strokeBackgroundArray[i].dimensions = new Vector2(1f, 1f);
        }
        skyNoiseOffset += skyNoiseScroll * Time.deltaTime;
        backgroundStrokesBuffer.SetData(strokeBackgroundArray);


        /*float groundPos = -5f;
        for (int i = 0; i < strokeTerrainArray.Length; i++) {
            //strokeTerrainArray[i].pos = UnityEngine.Random.insideUnitSphere * groundSpreadExponent;
            //strokeTerrainArray[i].pos *= strokeTerrainArray[i].pos.sqrMagnitude;
            strokeTerrainArray[i].pos.y = groundPos;  // start at groundHeight
            NoiseSample noiseSample = NoisePrime.Simplex3D(new Vector3(strokeTerrainArray[i].pos.x, 0f, strokeTerrainArray[i].pos.z), groundNoiseFrequency);
            float heightOffset = noiseSample.value * groundNoiseAmplitude;
            strokeTerrainArray[i].pos.y = groundPos + heightOffset;
            //strokeTerrainArray[i].pos = terrain1[i];
            Color color = terrainColorGradient.Evaluate(noiseSample.value * 0.5f + 0.5f);
            strokeTerrainArray[i].col = new Vector3(color.r, color.g, color.b);
            //Vector3 preTangent = noiseSample.derivative;
            //preTangent.y *= groundNoiseAmplitude;

            //strokeTerrainArray[i].normal = new Vector3(-noiseSample.derivative.x, 1f, -noiseSample.derivative.z);
            //Vector3 cross = Vector3.Cross(strokeBackgroundArray[i].normal, noiseSample.derivative);  
            strokeTerrainArray[i].tangent = Vector3.Cross(strokeTerrainArray[i].normal, noiseSample.derivative);
            strokeTerrainArray[i].dimensions = new Vector2(1f, 1f);

        }        
        terrainStrokesBuffer.SetData(strokeTerrainArray);
        */
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
        if (terrainStrokesBuffer != null) {
            terrainStrokesBuffer.Release();
            terrainStrokesBuffer.Dispose();
        }
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
        vertices[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane + 1f));
        vertices[1] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, cam.nearClipPlane + 1f));
        vertices[2] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, cam.nearClipPlane + 1f));
        vertices[3] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane + 1f));

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
