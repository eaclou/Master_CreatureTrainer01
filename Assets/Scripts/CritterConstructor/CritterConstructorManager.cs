﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CritterConstructorManager : MonoBehaviour {

    public CritterEditorInputManager critterEditorInputManager;
    
    private GameObject masterCritterGO;
    public Critter masterCritter;
    //public List<GameObject> critterSegmentList; //moved to critter

    // Update is called once per frame
    void Update() {
        //if (Debug.isDebugBuild) Debug.Log(" CritterConstructorManager Update!");
        if(critterEditorInputManager != null) {
            critterEditorInputManager.CheckInputs();
        }
        else {
            //if (Debug.isDebugBuild) Debug.Log(" CritterConstructorManager Update INPUT NULL!");
        }
    }

    public void PressButtonTempReset() {        
        ResetToBlankCritter();
    }

    public void ResetToBlankCritter() {  // Check how to make sure no memory is leaked!
        //Debug.Log("ResetToBlankCritter()");
        CritterEditorState.nextNodeInnov = 0;
        CritterEditorState.nextAddonInnov = 0;
        
        if (masterCritter == null) {  // first time!!!
            masterCritterGO = new GameObject("masterCritterGO");
            masterCritter = masterCritterGO.AddComponent<Critter>();
            masterCritter.InitializeBlankCritter();
        }
        else {
            masterCritter.InitializeBlankCritter();
        }
    }

    public void UpdateSegmentSelectionVis() {
        // Change the material colors/attrs on critterSegments to show which are selected
        /*if(masterCritter != null) {
            for (int i = 0; i < masterCritter.critterSegmentList.Count; i++) {
                masterCritter.critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);
            }
            if (critterEditorInputManager.critterEditorState.isSegmentSelected) {
                critterEditorInputManager.critterEditorState.selectedSegment.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);
            }
        }   */     
    }

    public void UpdateSegmentShaderStates() {
        if (masterCritter != null) {
            for (int i = 0; i < masterCritter.critterSegmentList.Count; i++) {
                masterCritter.critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
            }
        }
    }

    /*public Button tempButtonReset;

    private CritterGenome masterCritterGenome;
    private GameObject critterGroup;
    public Material critterSegmentMaterial;
    //public Material critterSelectedSegmentMaterial;
    public List<GameObject> critterSegmentList;
    

    public GameObject selectedObject;
    private GameObject gizmoScaleGO;

    // Use this for initialization
    void Start () {
        //selectedObject = new GameObject();
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
            critterSegmentMaterial = new Material(Shader.Find("Custom/CritterSegmentBasic"));
        }
        //if (critterSelectedSegmentMaterial == null) {
            //critterSelectedSegmentMaterial = new Material(Shader.Find("Custom/CritterSegmentBasic"));
            //critterSelectedSegmentMaterial.color = new Color(0.7f, 1f, 0.75f);
        //}


        RebuildCritter();
    }

    private void RebuildCritter() {
        Debug.Log("RebuildCritter()");
        critterSegmentList = new List<GameObject>();

        // interpret Genome and construct critter in its bind pose
        int currentNode = 0; // start with RootNode

        GameObject rootNode = new GameObject("rootNode" + currentNode.ToString());
        critterSegmentList.Add(rootNode);
        //GOwormSegments[i].gameObject.layer = LayerMask.NameToLayer("worm");
        rootNode.AddComponent<CritterSegment>().InitGamePiece();
        rootNode.GetComponent<CritterSegment>().sourceNode = masterCritterGenome.CritterNodeList[0];
        rootNode.GetComponent<CritterSegment>().id = masterCritterGenome.CritterNodeList[0].ID;
        rootNode.GetComponent<MeshRenderer>().material = critterSegmentMaterial;
        //rootNode.GetComponent<MeshRenderer>().material.SetFloat("_TargetPosX", 0.5f);
        rootNode.transform.SetParent(critterGroup.gameObject.transform);        
        rootNode.AddComponent<BoxCollider>();        
    }

    public void UpdateSegmentSelectionVis() {
        // Change the material colors on critterSegments to show which are selected
        for(int i = 0; i < critterSegmentList.Count; i++) {
            //critterSegmentList[i].GetComponent<MeshRenderer>().material = critterSegmentMaterial;
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);
        }
        if (selectedObject != null) {
            //selectedObject.GetComponent<MeshRenderer>().material = critterSelectedSegmentMaterial;
            selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 1f);
        }
    }

    public void UpdateSegmentShaderStates() {
        for (int i = 0; i < critterSegmentList.Count; i++) {
            //critterSegmentList[i].GetComponent<MeshRenderer>().material = critterSegmentMaterial;
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
        }


    }

    public void AddNewCritterNode(Vector3 attachPosition) {
        Debug.Log("AddNewCritterNode() " + selectedObject.ToString() + ", " + attachPosition.ToString());

        CritterNode parentNode;
        CritterNode newCritterNode = new CritterNode();
        if (selectedObject != null) {
            if(selectedObject.GetComponent<CritterSegment>() != null) {
                parentNode = selectedObject.GetComponent<CritterSegment>().sourceNode;
                //newCritterNode.jointLink.parentNode = parentNode;
                if (parentNode != null) {
                    Debug.Log("parentNode: " + selectedObject.GetComponent<CritterSegment>().ToString() + ", LIST: " + masterCritterGenome.CritterNodeList[0].ToString());
                }
            }
        }        

        GameObject newSegment = new GameObject("newSegment" + newCritterNode.ID.ToString());
        critterSegmentList.Add(newSegment);        
        newSegment.AddComponent<CritterSegment>().InitGamePiece();
        newSegment.GetComponent<CritterSegment>().sourceNode = newCritterNode;
        newSegment.GetComponent<MeshRenderer>().material = critterSegmentMaterial;
        newSegment.transform.SetParent(critterGroup.gameObject.transform);

        Vector3 parentPos = selectedObject.GetComponent<CritterSegment>().transform.position;
        Vector3 pointParentToAttachPos = attachPosition - parentPos;
        float x = Vector3.Dot(pointParentToAttachPos, selectedObject.GetComponent<CritterSegment>().transform.right);
        float y = Vector3.Dot(pointParentToAttachPos, selectedObject.GetComponent<CritterSegment>().transform.up);
        float z = Vector3.Dot(pointParentToAttachPos, selectedObject.GetComponent<CritterSegment>().transform.forward);

        //newCritterNode.jointLink.attachCoordinates = new Vector3(x, y, z);

        Vector3 normalDirection = new Vector3(0f, 0f, 0f);
        if(Mathf.Abs(x) > Mathf.Abs(y)) {
            if(Mathf.Abs(x) > Mathf.Abs(z)) {  // x is largest
                if (x != 0)
                    x = x / Mathf.Abs(x); // make either -1 or 1 
                //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = x * selectedObject.GetComponent<CritterSegment>().transform.right;
            }
            else {  // z is largest
                if (z != 0)
                    z = z / Mathf.Abs(z); // make either -1 or 1 
                //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = z * selectedObject.GetComponent<CritterSegment>().transform.forward;
            }
        }
        else {  // y>x
            if(Mathf.Abs(x) > Mathf.Abs(z)) {  // y is largest
                if (y != 0)
                    y = y / Mathf.Abs(y); // make either -1 or 1 
                //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = y * selectedObject.GetComponent<CritterSegment>().transform.up;
            }  
            else {  // z>x
                // x is smallest
                if (Mathf.Abs(y) > Mathf.Abs(z)) {  // y is largest
                    if (y != 0)
                        y = y / Mathf.Abs(y); // make either -1 or 1 
                    //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                    normalDirection = y * selectedObject.GetComponent<CritterSegment>().transform.up;
                }
                else { // z is largest
                    if (z != 0)
                        z = z / Mathf.Abs(z); // make either -1 or 1 
                    //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                    normalDirection = z * selectedObject.GetComponent<CritterSegment>().transform.forward;
                }                
            }
        }
        
        Vector3 newSegmentPos = attachPosition + normalDirection * newCritterNode.dimensions.x / 2f; // REVISIT
        //Debug.Log("normalDirection() " + normalDirection.ToString() + ", newSegmentPos: " + newSegmentPos.ToString() + ", attachPosition: " + attachPosition.ToString() + ", pointParentToAttachPos: " + pointParentToAttachPos.ToString());
        newSegment.transform.position = newSegmentPos; // + selectedObject.GetComponent<CritterSegment>().transform.position;
        //newSegment.transform.LookAt(newSegmentPos + normalDirection);
        newSegment.transform.rotation = Quaternion.LookRotation(normalDirection);
        newSegment.AddComponent<BoxCollider>();
        selectedObject = newSegment;
        
        UpdateSegmentSelectionVis();
        //masterCritterGenome.CritterNodeList[0]
    }

    public void ScaleCritterNode() {
        gizmoScaleGO = new GameObject("gizmoScaleGO");
        gizmoScaleGO.AddComponent<MeshFilter>().sharedMesh = EditorGizmoMeshShapes.GetCubeMesh();
        gizmoScaleGO.AddComponent<MeshRenderer>();
        //gizmoScaleGO.transform.position = selectedObject.GetComponent<CritterSegment>().sourceNode.  // GET ATTACH POSITION
    }
    */

}
