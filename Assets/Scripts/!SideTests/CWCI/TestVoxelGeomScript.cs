using UnityEngine;
using System.Collections;

public class TestVoxelGeomScript : MonoBehaviour {

    public Shader geomShader;
    Material material;

    public Texture2D sprite;
    public float size = 0.1f;
    public Color color = new Color(1f, 0.6f, 0.3f, 0.03f);

    TestVoxelComputeShaderOutput cso;

    bool getdata = true;
    
    // Use this for initialization
	void Start () {
        material = new Material(geomShader);

        cso = GetComponent<TestVoxelComputeShaderOutput>();
	}

    void OnRenderObject() {
        if(getdata) {
            //getdata = false;
            cso.Dispatch();
        }

        material.SetPass(0);
        material.SetColor("_Color", color);
        material.SetBuffer("buf_Points", cso.outputBuffer);
        material.SetTexture("_Sprite", sprite);

        material.SetFloat("_Size", size);
        material.SetMatrix("world", transform.localToWorldMatrix);

        Graphics.DrawProcedural(MeshTopology.Points, cso.outputBuffer.count);
    }
	
}
