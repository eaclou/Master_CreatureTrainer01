using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public GameObject cameraGO;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        pos = cameraGO.transform.position;
        this.transform.position = pos;
    }
}
