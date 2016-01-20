using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CritterConstructorManager : MonoBehaviour {

    public Button tempButtonReset;

    private CritterGenome masterCritterGenome;
    private GameObject critterGroup;
    private Material critterSegmentMaterial;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetCreature() {  // Check how to make sure no memory is leaked!
        Debug.Log("ResetCreature()");

        masterCritterGenome = new CritterGenome();
        if(critterGroup == null) {  // create gameObject transform to hold critter's segments
            critterGroup = new GameObject("critterGroup");
            critterGroup.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            var children = new List<GameObject>();
            foreach (Transform child in critterGroup.gameObject.transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }
        if(critterSegmentMaterial == null) {
            critterSegmentMaterial = new Material(Shader.Find("Diffuse"));
        }
            

        RebuildCritter();
    }

    private void RebuildCritter() {
        Debug.Log("RebuildCritter()");

        // interpret Genome and construct critter in its bind pose
        int currentNode = 0; // start with RootNode

        GameObject rootNode = new GameObject("rootNode" + currentNode.ToString());
        //GOwormSegments[i].gameObject.layer = LayerMask.NameToLayer("worm");
        rootNode.AddComponent<CritterSegment>().InitGamePiece();
        rootNode.GetComponent<MeshRenderer>().material = critterSegmentMaterial;
        rootNode.transform.SetParent(critterGroup.gameObject.transform);        
        rootNode.AddComponent<BoxCollider>();
    }
}
