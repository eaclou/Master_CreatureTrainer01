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
            newNode.GetComponent<MeshRenderer>().material = critterSegmentMaterial;
            newNode.transform.SetParent(this.gameObject.transform);
            newNode.AddComponent<BoxCollider>();
            critterSegmentList.Add(newNode);            
            newSegment.InitGamePiece();
            newSegment.sourceNode = masterCritterGenome.CritterNodeList[i];
            newSegment.id = i;
            
            Vector3 parentPos = new Vector3(0f, 0f, 0f);
            Vector3 newSegmentPos = new Vector3(0f, 0f, 0f);
            // if Root node:
            if (i == 0) {   // RE-VISIT!!!!
                newSegment.transform.rotation = Quaternion.identity;
            }
            else { // not root node!!                
                newSegment.parentSegment = critterSegmentList[masterCritterGenome.CritterNodeList[i].parentJointLink.parentNode.ID].GetComponent<CritterSegment>();  // !!! RE-VISIT!!!!
                parentPos = newSegment.parentSegment.transform.position;
                Vector3 attachPosition = new Vector3(0f, 0f, 0f);
                //Vector3 pointParentToAttachPos = attachPosition - parentPos;
                float x = newSegment.sourceNode.parentJointLink.attachCoords.x;
                float y = newSegment.sourceNode.parentJointLink.attachCoords.y;
                float z = newSegment.sourceNode.parentJointLink.attachCoords.z;
                attachPosition = x * newSegment.parentSegment.gameObject.transform.right +
                    y * newSegment.parentSegment.gameObject.transform.up +
                    z * newSegment.parentSegment.gameObject.transform.forward +
                    x * newSegment.parentSegment.gameObject.transform.position;
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

                newSegmentPos = attachPosition + normalDirection * newSegment.sourceNode.dimensions.x / 2f; // REVISIT -- will only work for cubes!  
                newSegment.transform.rotation = Quaternion.LookRotation(normalDirection);

                Debug.Log("CreateNewSegment(sourceNode = " + newSegment.sourceNode.ToString() + ", attachPosition = " + attachPosition.ToString() + ") newSegmentPos = " + newSegmentPos.ToString());
            }            
                                                                                                          
            newSegment.transform.position = newSegmentPos; // + selectedObject.GetComponent<CritterSegment>().transform.position;
            
        }        
    }

    private void EnqueueNodeForTranscription(CritterNode node) {

    }

    private void InitializeSegmentMaterial() {
        if (critterSegmentMaterial == null) {
            critterSegmentMaterial = new Material(Shader.Find("Custom/CritterSegmentBasic"));
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
