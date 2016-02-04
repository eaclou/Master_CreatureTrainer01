using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Critter : MonoBehaviour {

    public CritterGenome masterCritterGenome;
    public Material critterSegmentMaterial;
    // List of generated segments, based on critter's full Genome
    public List<GameObject> critterSegmentList;
    
    public void InitializeBlankCritter() {
        CreateBlankCritterGenome();
    }

    private void CreateBlankCritterGenome() {
        if(masterCritterGenome == null) {
            masterCritterGenome = new CritterGenome();
        }
        else {
            masterCritterGenome.ResetToBlankGenome();
        }

        RebuildCritterFromGenome();
    }

    public void UpdateCritterFromGenome() {  // only use if the genome node graph hasn't changed -- only attributes & settings!!!
        for (int i = 0; i < masterCritterGenome.CritterNodeList.Count; i++) {  // cycle through ChildNodes of currentNode
            GameObject newNode = critterSegmentList[i];
            CritterSegment newSegment = newNode.GetComponent<CritterSegment>();

            Vector3 parentPos = new Vector3(0f, 0f, 0f);
            Vector3 newSegmentPos = new Vector3(0f, 0f, 0f);
            newNode.transform.localScale = masterCritterGenome.CritterNodeList[i].dimensions;
            Debug.Log("dimensions: " + masterCritterGenome.CritterNodeList[i].dimensions.ToString());
            // if Root node:
            if (i == 0) {   // RE-VISIT!!!!
                newSegment.transform.rotation = Quaternion.identity;                
            }
            else { // not root node!!                
                
                parentPos = newSegment.parentSegment.transform.position;
                float x = newSegment.sourceNode.parentJointLink.attachDir.x;
                float y = newSegment.sourceNode.parentJointLink.attachDir.y;
                float z = newSegment.sourceNode.parentJointLink.attachDir.z;               
                Vector3 normalDirection = new Vector3(0f, 0f, 0f);
                if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    if (Mathf.Abs(x) > Mathf.Abs(z)) {  // x is largest
                        if (x != 0)
                            x = x / Mathf.Abs(x); // make either -1 or 1 
                                                  //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                        normalDirection = x * newSegment.parentSegment.transform.right;
                    }
                    else {  // z is largest
                        if (z != 0)
                            z = z / Mathf.Abs(z); // make either -1 or 1 
                                                  //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                        normalDirection = z * newSegment.parentSegment.transform.forward;
                    }
                }
                else {  // y>x
                    if (Mathf.Abs(x) > Mathf.Abs(z)) {  // y is largest
                        if (y != 0)
                            y = y / Mathf.Abs(y); // make either -1 or 1 
                                                  //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                        normalDirection = y * newSegment.parentSegment.transform.up;
                    }
                    else {  // z>x
                            // x is smallest
                        if (Mathf.Abs(y) > Mathf.Abs(z)) {  // y is largest
                            if (y != 0)
                                y = y / Mathf.Abs(y); // make either -1 or 1 
                                                      //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                            normalDirection = y * newSegment.parentSegment.transform.up;
                        }
                        else { // z is largest
                            if (z != 0)
                                z = z / Mathf.Abs(z); // make either -1 or 1 
                                                      //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                            normalDirection = z * newSegment.parentSegment.transform.forward;
                        }
                    }
                }
                Vector3 right = newSegment.parentSegment.transform.right;
                Vector3 up = newSegment.parentSegment.transform.up;
                Vector3 forward = newSegment.parentSegment.transform.forward;
                right *= newSegment.sourceNode.parentJointLink.attachDir.x;
                up *= newSegment.sourceNode.parentJointLink.attachDir.y;
                forward *= newSegment.sourceNode.parentJointLink.attachDir.z;

                Vector3 attachDirWorld = new Vector3(0f, 0f, 0f);
                attachDirWorld = right + up + forward;
                float a = Vector3.Dot(normalDirection, attachDirWorld); // proportion of normalDirection that attachDirWorld reaches. This will determine how far to extend the ray to hit the cube face
                float ratio = 1f / a;
                attachDirWorld *= ratio;

                Vector3 attachPosWorld = new Vector3(0f, 0f, 0f);
                attachPosWorld = newSegment.parentSegment.transform.position + attachDirWorld * newSegment.parentSegment.sourceNode.dimensions.x * 0.5f;
                newSegmentPos = attachPosWorld + normalDirection * newSegment.sourceNode.dimensions.z * 0.5f;  // REVISIT -- will only work for cubes! 
                newSegment.transform.rotation = Quaternion.LookRotation(normalDirection);

                //Debug.Log("attachPosWorld = " + attachPosWorld.ToString() + ", attachPosition = " + ") newSegmentPos = " + newSegmentPos.ToString());
                //Debug.Log("a= " + a.ToString() + ", normalDirection = " + normalDirection.ToString() + ") attachDirWorld= " + attachDirWorld.ToString() + ", parentPos " + newSegment.parentSegment.gameObject.transform.position.ToString());
            }

            newSegment.transform.position = newSegmentPos; // + selectedObject.GetComponent<CritterSegment>().transform.position;

        }
    }

    public void RebuildCritterFromGenome() {  // resets and constructs the entire critter based on its current genome -- recreating all of its segments
        // Delete existing Segment GameObjects
        DeleteSegments();
        InitializeSegmentMaterial();

        Debug.Log("RebuildCritter()");
        critterSegmentList = new List<GameObject>();
        // interpret Genome and construct critter in its bind pose



        //int currentNode = 0; // start with RootNode
        //int currentParentNode = 0;
        for(int i = 0; i < masterCritterGenome.CritterNodeList.Count; i++) {  // cycle through ChildNodes of currentNode
            GameObject newNode = new GameObject("Node" + i.ToString());
            CritterSegment newSegment = newNode.AddComponent<CritterSegment>();
            //int layer = 10;  // editorSegment
            //int layermask = 1 << layer;
            newNode.layer = LayerMask.NameToLayer("editorSegment"); ; // set segmentGO layer to editorSegment, to distinguish it from Gizmos
            newNode.GetComponent<MeshRenderer>().material = critterSegmentMaterial;
            newNode.transform.SetParent(this.gameObject.transform);
            newNode.AddComponent<BoxCollider>().isTrigger = true;
            critterSegmentList.Add(newNode);            
            newSegment.InitGamePiece();
            newSegment.sourceNode = masterCritterGenome.CritterNodeList[i];
            newSegment.id = i;

            Vector3 parentPos = new Vector3(0f, 0f, 0f);
            Vector3 newSegmentPos = new Vector3(0f, 0f, 0f);
            newNode.transform.localScale = masterCritterGenome.CritterNodeList[i].dimensions;
            // if Root node:
            if (i == 0) {   // RE-VISIT!!!!
                newSegment.transform.rotation = Quaternion.identity;
            }
            else { // not root node!!                
                newSegment.parentSegment = critterSegmentList[masterCritterGenome.CritterNodeList[i].parentJointLink.parentNode.ID].GetComponent<CritterSegment>();  // !!! RE-VISIT!!!!
                parentPos = newSegment.parentSegment.transform.position;                
                float x = newSegment.sourceNode.parentJointLink.attachDir.x;
                float y = newSegment.sourceNode.parentJointLink.attachDir.y;
                float z = newSegment.sourceNode.parentJointLink.attachDir.z;
                //Debug.Log("$$$$$$$$ parentPos: " + parentPos.ToString() + "attachDir: ( " + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + " )");                
                Vector3 normalDirection = new Vector3(0f, 0f, 0f);
                if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    if (Mathf.Abs(x) > Mathf.Abs(z)) {  // x is largest
                        if (x != 0)
                            x = x / Mathf.Abs(x); // make either -1 or 1 
                                                  //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                        normalDirection = x * newSegment.parentSegment.transform.right;
                    }
                    else {  // z is largest
                        if (z != 0)
                            z = z / Mathf.Abs(z); // make either -1 or 1 
                                                  //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                        normalDirection = z * newSegment.parentSegment.transform.forward;
                    }
                }
                else {  // y>x
                    if (Mathf.Abs(x) > Mathf.Abs(z)) {  // y is largest
                        if (y != 0)
                            y = y / Mathf.Abs(y); // make either -1 or 1 
                                                  //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                        normalDirection = y * newSegment.parentSegment.transform.up;
                    }
                    else {  // z>x
                            // x is smallest
                        if (Mathf.Abs(y) > Mathf.Abs(z)) {  // y is largest
                            if (y != 0)
                                y = y / Mathf.Abs(y); // make either -1 or 1 
                                                      //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                            normalDirection = y * newSegment.parentSegment.transform.up;
                        }
                        else { // z is largest
                            if (z != 0)
                                z = z / Mathf.Abs(z); // make either -1 or 1 
                                                      //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                            normalDirection = z * newSegment.parentSegment.transform.forward;
                        }
                    }
                }
                Vector3 right = newSegment.parentSegment.transform.right;
                Vector3 up = newSegment.parentSegment.transform.up;
                Vector3 forward = newSegment.parentSegment.transform.forward;                              
                right *= newSegment.sourceNode.parentJointLink.attachDir.x;
                up *= newSegment.sourceNode.parentJointLink.attachDir.y;
                forward *= newSegment.sourceNode.parentJointLink.attachDir.z;

                Vector3 attachDirWorld = new Vector3(0f, 0f, 0f);
                attachDirWorld = right + up + forward;
                float a = Vector3.Dot(normalDirection, attachDirWorld); // proportion of normalDirection that attachDirWorld reaches. This will determine how far to extend the ray to hit the cube face
                float ratio = 1f / a;
                attachDirWorld *= ratio;

                Vector3 attachPosWorld = new Vector3(0f, 0f, 0f);
                attachPosWorld = newSegment.parentSegment.transform.position + attachDirWorld * newSegment.parentSegment.sourceNode.dimensions.x * 0.5f;
                newSegmentPos = attachPosWorld + normalDirection * newSegment.sourceNode.dimensions.z * 0.5f;  // REVISIT -- will only work for cubes! 
                newSegment.transform.rotation = Quaternion.LookRotation(normalDirection);

                Debug.Log("attachPosWorld = " + attachPosWorld.ToString() + ", attachPosition = " + ") newSegmentPos = " + newSegmentPos.ToString());
                Debug.Log("a= " + a.ToString() + ", normalDirection = " + normalDirection.ToString() + ") attachDirWorld= " + attachDirWorld.ToString() + ", parentPos " + newSegment.parentSegment.gameObject.transform.position.ToString());
            }            
                                                                                                          
            newSegment.transform.position = newSegmentPos; // + selectedObject.GetComponent<CritterSegment>().transform.position;
            
        }        
    }

    private void EnqueueNodeForTranscription(CritterNode node) {

    }

    private void InitializeSegmentMaterial() {
        if (critterSegmentMaterial == null) {
            critterSegmentMaterial = new Material(Shader.Find("Custom/CritterSegmentBasic"));
            critterSegmentMaterial.renderQueue = 2000;
        }
    }

    private void DeleteSegments() {
        // Delete existing Segment GameObjects
        var children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public void RebuildBranchFromGenome(CritterNode node) {  // resets and re-constructs only a branch of the Critter, starting with specified node

    }

}
