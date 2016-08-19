using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class PaintRenderMain01 : MonoBehaviour {

    public Shader strokeShader;
    private Material strokeMaterial;
    public Vector2 size = Vector2.one;
    public Color color = new Color(1f, 0.6f, 0.3f, 1f);

    public ComputeShader strokeCompute;
    public ComputeBuffer strokeBuffer;

    private ComputeBuffer quadPointsBuffer;

    public struct strokeStruct {
        public Vector3 pos;
    }

    strokeStruct[] strokeArray;

    // CommandBuffer Stuff:
    private Camera m_Cam;
    // We'll want to add a command buffer on any camera that renders us,
    // so have a dictionary of them.
    private Dictionary<Camera, CommandBuffer> m_Cameras = new Dictionary<Camera, CommandBuffer>();

    // Use this for initialization
    void Start () {

        InitializeBuffers();
    }

    void InitializeBuffers() {
        strokeMaterial = new Material(strokeShader);
        strokeArray = new strokeStruct[] {
            new strokeStruct() { pos = Vector3.left + Vector3.up },
            new strokeStruct() { pos = Vector3.right + Vector3.up },
            new strokeStruct() { pos = Vector3.zero },
            new strokeStruct() { pos = Vector3.left + Vector3.down },
            new strokeStruct() { pos = Vector3.right + Vector3.down }
        };

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

        strokeMaterial.SetPass(0);
        strokeMaterial.SetColor("_Color", color);
        strokeMaterial.SetBuffer("strokeDataBuffer", strokeBuffer);
        strokeMaterial.SetBuffer("quadPointsBuffer", quadPointsBuffer);
        
        buf.DrawProcedural(Matrix4x4.identity, strokeMaterial, 0, MeshTopology.Triangles, 6, strokeBuffer.count);
        cam.AddCommandBuffer(CameraEvent.AfterImageEffects, buf);

        // copy screen into temporary RT
        //int screenCopyID = Shader.PropertyToID("_ScreenCopyTexture");
        //buf.GetTemporaryRT(screenCopyID, -1, -1, 0, FilterMode.Bilinear);  // -1 = "camera pixel width"??
        //buf.Blit(BuiltinRenderTextureType.CurrentActive, screenCopyID);  // dest (screenCopyID) becomes currently active renderTarget after Blit call
        // This copies currently active renderTarget (result of camera render pass up to Skydome) and gets it ready for post-processing

        // get two smaller RTs
        //int blurredID = Shader.PropertyToID("_Temp1");
        //int blurredID2 = Shader.PropertyToID("_Temp2");
        //buf.GetTemporaryRT(blurredID, -2, -2, 0, FilterMode.Bilinear);  // -1 = currentCam renderTexture dimensions, -2 = half, -4 = quarter, etc.
        //buf.GetTemporaryRT(blurredID2, -2, -2, 0, FilterMode.Bilinear);

        // downsample screen copy into smaller RT, release screen RT
        //buf.Blit(screenCopyID, blurredID);  // half-res due to -2 above
        //buf.ReleaseTemporaryRT(screenCopyID);

        // Actual Blurs computed here: alternate horizontal/vertical and pass between the two temporary Render Textures
        // horizontal blur
        //buf.SetGlobalVector("offsets", new Vector4(2.0f / Screen.width, 0, 0, 0));
        //buf.Blit(blurredID, blurredID2, m_Material);  // passes textures through "_MainTex" property
        //buf.Blit(blurredID2, blurredID);
        // vertical blur
        //buf.SetGlobalVector("offsets", new Vector4(0, 2.0f / Screen.height, 0, 0));
        //buf.Blit(blurredID2, blurredID, m_Material);
        // horizontal blur 2
        //buf.SetGlobalVector("offsets", new Vector4(4.0f / Screen.width, 0, 0, 0));
        //buf.Blit(blurredID, blurredID2, m_Material);
        // vertical blur 2
        //buf.SetGlobalVector("offsets", new Vector4(0, 4.0f / Screen.height, 0, 0));
        //buf.Blit(blurredID2, blurredID, m_Material);
        // horizontal blur 3
        //buf.SetGlobalVector("offsets", new Vector4(8.0f / Screen.width, 0, 0, 0));
        //buf.Blit(blurredID, blurredID2, m_Material);
        // vertical blur 3
        //buf.SetGlobalVector("offsets", new Vector4(0, 8.0f / Screen.height, 0, 0));
        //buf.Blit(blurredID2, blurredID, m_Material);

        //buf.SetGlobalTexture("_GrabBlurTexture", blurredID);

        //cam.AddCommandBuffer(CameraEvent.AfterSkybox, buf);
    }

    /*void OnRenderObject() {

        strokeMaterial.SetPass(0);
        //strokeMaterial.SetColor("_Color", color);
        strokeMaterial.SetBuffer("strokeDataBuffer", strokeBuffer);
        strokeMaterial.SetBuffer("quadPointsBuffer", quadPointsBuffer);
        //strokeMaterial.SetVector("_Size", size);

        Graphics.DrawProcedural(MeshTopology.Triangles, 6, strokeBuffer.count);  // 6 = number of verts in Quad billboard mesh, strokebuffer is the #of Instances of that quad mesh to be rendered
        //Debug.Log("DrawProcedural! " + strokeMaterial.GetVector("_Size").ToString());
    }*/

    // Update is called once per frame
    void Update () {
        strokeMaterial.SetColor("_Color", color);
    }

    void Cleanup() {
        foreach (var cam in m_Cameras) {
            if (cam.Key) {
                cam.Key.RemoveCommandBuffer(CameraEvent.AfterImageEffects, cam.Value);
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
