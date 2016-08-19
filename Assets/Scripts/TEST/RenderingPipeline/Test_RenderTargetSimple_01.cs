using UnityEngine;
using System.Collections;

public class Test_RenderTargetSimple_01 : MonoBehaviour {

    private RenderTexture renderTexture;

    public Camera sourceCamera;
    public GameObject displayScreen;

	// Use this for initialization
	void Start () {
        renderTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);

        this.GetComponent<Camera>().targetTexture = renderTexture;
        //sourceCamera.targetDisplay = 
        displayScreen.GetComponent<MeshRenderer>().material.mainTexture = renderTexture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPreRender() {

    }
}
