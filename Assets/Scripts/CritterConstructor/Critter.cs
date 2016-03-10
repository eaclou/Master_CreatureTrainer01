using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Critter : MonoBehaviour {

    public CritterGenome masterCritterGenome;
    public Material critterSegmentMaterial;
    // List of generated segments, based on critter's full Genome
    public List<GameObject> critterSegmentList;
    
    public void InitializeBlankCritter() {
        if(critterSegmentList == null) {
            critterSegmentList = new List<GameObject>(); // the 'official' record of this critters Segments
        }        
        CreateBlankCritterGenome();        
    }

    private void CreateBlankCritterGenome() {
        if(masterCritterGenome == null) {
            masterCritterGenome = new CritterGenome();
        }
        else {
            masterCritterGenome.ResetToBlankGenome();
        }

        RebuildCritterFromGenomeRecursive(false);
    }

    public void UpdateCritterFromGenome() {  // only use if the genome node graph hasn't changed -- only attributes & settings!!!
        for (int i = 0; i < critterSegmentList.Count; i++) {  // cycle through ChildNodes of currentNode
            
            //Debug.Log("dimensions: " + masterCritterGenome.CritterNodeList[i].dimensions.ToString());
            // if Root node:
            if (i == 0) {   // RE-VISIT!!!!
                critterSegmentList[i].transform.rotation = Quaternion.identity;
                critterSegmentList[i].transform.localScale = masterCritterGenome.CritterNodeList[i].dimensions * critterSegmentList[i].GetComponent<CritterSegment>().scalingFactor;
            }
            else { // not root node!!
                SetSegmentTransform(critterSegmentList[i]); // update segment's position & orientation based on its Parent segment & both segments' dimensions
            }
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_SizeX", critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.dimensions.x);
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_SizeY", critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.dimensions.y);
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_SizeZ", critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.dimensions.z);
        }
    }

    private void SetSegmentTransform(GameObject newSegmentGO) {  // ALSO SCALES!!!!
        Vector3 parentPos = new Vector3(0f, 0f, 0f);
        Vector3 newSegmentPos = new Vector3(0f, 0f, 0f);
        CritterSegment newSegment = newSegmentGO.GetComponent<CritterSegment>();

        Vector3 newSegmentParentDimensions = newSegment.parentSegment.sourceNode.dimensions * newSegment.parentSegment.scalingFactor;
        Vector3 newSegmentDimensions = newSegment.sourceNode.dimensions * newSegment.scalingFactor;        
        parentPos = newSegment.parentSegment.transform.position;        
        Vector3 attachDir = newSegment.sourceNode.jointLink.attachDir;
        Vector3 restAngleDir = newSegment.sourceNode.jointLink.restAngleDir;

        // MODIFY AttachDir based on recursion Forward:
        if (newSegment.recursionNumber > 1) {  // if segment is not the root of a recursion chain
            attachDir = Vector3.Lerp(attachDir, new Vector3(0f, 0f, 1f), newSegment.sourceNode.jointLink.recursionForward);
        }
        // MODIFY AttachDir based on Mirroring!!!!!
        if (newSegment.mirrorX) {
            attachDir.x *= -1f;
            restAngleDir.x *= -1f;
            restAngleDir.z *= -1f;
        }
        if (newSegment.mirrorY) {
            attachDir.y *= -1f;
            restAngleDir.y *= -1f;
            restAngleDir.z *= -1f;
        }
        if (newSegment.mirrorZ) { // deprecated??
            attachDir.z *= -1f;            
        }       

        // These are different from attachDir because they're used to find the SIDE of the cube it's attached to, in order to calc attachNormalDir
        float x = attachDir.x;
        float y = attachDir.y;
        float z = attachDir.z;        
        
        Vector3 normalDirection = new Vector3(0f, 0f, 0f);
        Vector3 localNormalDirection = new Vector3(0f, 0f, 0f);
        int attachSide = 0;  // 0 = X, 1 = Y, 2 = Z
        if (Mathf.Abs(x) > Mathf.Abs(y)) {
            if (Mathf.Abs(x) > Mathf.Abs(z)) {  // x is largest
                if (x != 0)
                    x = x / Mathf.Abs(x); // make either -1 or 1 
                                          //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = x * newSegment.parentSegment.transform.right;
                localNormalDirection.x = x;
                attachSide = 0;
            }
            else {  // z is largest
                if (z != 0)
                    z = z / Mathf.Abs(z); // make either -1 or 1 
                                          //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = z * newSegment.parentSegment.transform.forward;
                localNormalDirection.z = z;
                attachSide = 2;
            }
        }
        else {  // y>x
            if (Mathf.Abs(x) > Mathf.Abs(z)) {  // y is largest
                if (y != 0)
                    y = y / Mathf.Abs(y); // make either -1 or 1 
                                          //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = y * newSegment.parentSegment.transform.up;
                localNormalDirection.y = y;
                attachSide = 1;
            }
            else {  // z>x
                    // x is smallest
                if (Mathf.Abs(y) > Mathf.Abs(z)) {  // y is largest
                    if (y != 0)
                        y = y / Mathf.Abs(y); // make either -1 or 1 
                                              //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                    normalDirection = y * newSegment.parentSegment.transform.up;
                    localNormalDirection.y = y;
                    attachSide = 1;
                }
                else { // z is largest
                    if (z != 0)
                        z = z / Mathf.Abs(z); // make either -1 or 1 
                                              //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                    normalDirection = z * newSegment.parentSegment.transform.forward;
                    localNormalDirection.z = z;
                    attachSide = 2;
                }
            }
        }
        Vector3 right = newSegment.parentSegment.transform.right;
        Vector3 up = newSegment.parentSegment.transform.up;
        Vector3 forward = newSegment.parentSegment.transform.forward;
        right *= attachDir.x * newSegmentParentDimensions.x;
        up *= attachDir.y * newSegmentParentDimensions.y;
        forward *= attachDir.z * newSegmentParentDimensions.z;

        Vector3 attachDirWorld = new Vector3(0f, 0f, 0f);
        attachDirWorld = right + up + forward;
        float a = Vector3.Dot(normalDirection, attachDirWorld); // proportion of normalDirection that attachDirWorld reaches. This will determine how far to extend the ray to hit the cube face
        float ratio = 1f / a;
        attachDirWorld *= ratio;

        Vector3 attachPosWorld = new Vector3(0f, 0f, 0f);
        float parentDepth = 0f;
        if (attachSide == 0) {  // attached to the X side, so use parent's X scale
            parentDepth = newSegmentParentDimensions.x * 0.5f;
        }
        else if (attachSide == 1) {
            parentDepth = newSegmentParentDimensions.y * 0.5f;
        }
        else {   //  attachSide == 2
            parentDepth = newSegmentParentDimensions.z * 0.5f;
        }
        attachPosWorld = newSegment.parentSegment.transform.position + attachDirWorld * parentDepth;
        
        //Debug.Log("attachPosWorld = " + attachPosWorld.ToString() + ", parentDepth: " + parentDepth.ToString() + ", attachSide: " + attachSide.ToString());
        Quaternion RestAngleOffset = Quaternion.FromToRotation(new Vector3(0f, 0f, 1f), new Vector3(restAngleDir.x, restAngleDir.y, 1f));
        Quaternion RestAngleOffsetTwist = Quaternion.AngleAxis(restAngleDir.z * 45f, Vector3.forward); // 45f can be replaced with a max twist angle var
        //Quaternion newSegmentNormal = Quaternion.LookRotation(normalDirection) * RestAngleOffset;
        //normalDirection = newSegmentNormal * Vector3.forward;
        // Calculate LOCAL-SPACE normalDirection of parent face at attach point,
        // Then add (saved Parent Rotation (forward vector)) + (local-space attachDir Rotation) + (rest-angle Offset rotation)?
        Quaternion localAttachRotation = Quaternion.LookRotation(localNormalDirection);
        Quaternion newSegmentRotation = newSegment.parentSegment.transform.rotation * localAttachRotation * RestAngleOffset * RestAngleOffsetTwist;
        Vector3 worldNormalDir = newSegmentRotation * Vector3.forward; 

        newSegmentPos = attachPosWorld + worldNormalDir * newSegmentDimensions.z * 0.5f;  // REVISIT -- will only work for cubes! 
        newSegment.transform.rotation = newSegmentRotation;
        newSegmentGO.transform.position = newSegmentPos;
        newSegmentGO.transform.localScale = newSegmentDimensions;
        //Debug.Log("attachPosWorld = " + attachPosWorld.ToString() + ", attachPosition = " + ") newSegmentPos = " + newSegmentPos.ToString());
        //Debug.Log("a= " + a.ToString() + ", normalDirection = " + normalDirection.ToString() + ") attachDirWorld= " + attachDirWorld.ToString() + ", parentPos " + newSegment.parentSegment.gameObject.transform.position.ToString());
    }

    /*public void RebuildCritterFromGenome(bool physicsOn) {  // resets and constructs the entire critter based on its current genome -- recreating all of its segments
        // Delete existing Segment GameObjects
        DeleteSegments();
        InitializeSegmentMaterial();

        //Debug.Log("RebuildCritter()");
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
            newNode.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
            newNode.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);
            newNode.transform.SetParent(this.gameObject.transform);
            newNode.AddComponent<BoxCollider>().isTrigger = false;
            //newNode.AddComponent<Rigidbody>();
            critterSegmentList.Add(newNode);            
            newSegment.InitGamePiece();
            newSegment.sourceNode = masterCritterGenome.CritterNodeList[i];
            newSegment.id = i;
            Debug.Log("RebuildCritter() newSegment.parentSegment: " + newSegment.ToString());
            newNode.transform.localScale = masterCritterGenome.CritterNodeList[i].dimensions;
            // if Root node:
            if (i == 0) {   // RE-VISIT!!!!
                newNode.transform.rotation = Quaternion.identity;
                if (physicsOn) {
                    newNode.AddComponent<Rigidbody>().isKinematic = true;
                }
            }
            else { // not root node!!  
                
                //newSegment.parentSegment = critterSegmentList[0].GetComponent<CritterSegment>();
                newSegment.parentSegment = critterSegmentList[newSegment.sourceNode.jointLink.parentNode.ID].GetComponent<CritterSegment>();
                Debug.Log("RebuildCritter() newSegment.parentSegment: " + newSegment.parentSegment.ToString() + ", " + masterCritterGenome.CritterNodeList[i].jointLink.parentNode.ID.ToString());
                SetSegmentTransform(newNode);

                if(physicsOn) {
                    newNode.AddComponent<Rigidbody>().isKinematic = false;
                    ConfigurableJoint configJoint = newNode.AddComponent<ConfigurableJoint>();
                    configJoint.autoConfigureConnectedAnchor = false;
                    configJoint.connectedBody = newSegment.parentSegment.gameObject.GetComponent<Rigidbody>();
                    configJoint.anchor = GetJointAnchor(newSegment.sourceNode);
                    configJoint.connectedAnchor = GetJointConnectedAnchor(newSegment.sourceNode); // <-- Might be Unnecessary
                    ConfigureJointSettings(newSegment.sourceNode, ref configJoint);
                }                
            }
        }        
    }*/

    public void RebuildCritterFromGenomeRecursive(bool physicsOn) {
        //Debug.Log("RebuildCritterFromGenomeRecursive: " + masterCritterGenome.CritterNodeList.Count.ToString());
        // Delete existing Segment GameObjects
        DeleteSegments();
        InitializeSegmentMaterial();

        if (critterSegmentList != null) {
            critterSegmentList.Clear();
        }
        // interpret Genome and construct critter in its bind pose
        bool isPendingChildren = true;
        int currentDepth = 0; // start with RootNode
        int maxDepth = 20;  // safeguard to prevent while loop lock
        int nextSegmentID = 0;
        List<CritterSegment> builtSegmentsList = new List<CritterSegment>();  // keep track of segments that have been built - linear in-order array 0-n segments
        //List<CritterNode> currentBuildNodeList = new List<CritterNode>();  // the childNodes that are next in line to be translated into Segments
        //List<CritterNode> pendingChildNodeList = new List<CritterNode>();  // the childNodes that are next in line to be translated into Segments
        //List<CritterSegment> pendingBuildNodeParentSegmentList = new List<CritterSegment>();  // a List that mirrors the pending Node list, but keeps track of each of their corresponding ParentSegments
        //List<CritterSegment> currentBuildNodeParentSegmentList = new List<CritterSegment>();

        List<BuildSegmentInfo> currentBuildSegmentList = new List<BuildSegmentInfo>();  // keeps track of all current-depth segment build-requests, and holds important metadata
        List<BuildSegmentInfo> nextBuildSegmentList = new List<BuildSegmentInfo>();  // used to keep track of next childSegments that need to be built

        // ***********  Will attempt to traverse the Segments to be created, keeping track of where on each graph (nodes# & segment#) the current build is on.
        BuildSegmentInfo rootSegmentBuildInfo = new BuildSegmentInfo();
        rootSegmentBuildInfo.sourceNode = masterCritterGenome.CritterNodeList[0];
        //if(rootSegmentBuildInfo.sourceNode.attachedJointLinkList.Count > 0) 
        //    Debug.Log("Root Children: " + rootSegmentBuildInfo.sourceNode.attachedJointLinkList.Count + ", child: " + rootSegmentBuildInfo.sourceNode.attachedJointLinkList[0]);
        currentBuildSegmentList.Add(rootSegmentBuildInfo);  // ROOT NODE IS SPECIAL!
        //currentBuildNodeList.Add(masterCritterGenome.CritterNodeList[0]);  // Root Node
        

        // Do a Breadth-first traversal??
        while (isPendingChildren) {
            //int numberOfChildNodes = masterCritterGenome.CritterNodeList[currentNode].attachedJointLinkList.Count;
            
            for (int i = 0; i < currentBuildSegmentList.Count; i++) {
                //Debug.Log("currentDepth: " + currentDepth.ToString() + "builtNodesQueue.Count: " + builtSegmentsList.Count.ToString() + ", pendingNodes: " + currentBuildSegmentList.Count.ToString() + ", i: " + i.ToString());
                // Iterate through pending nodes
                // Build current node --> Segment
                GameObject newGO = new GameObject("Node" + nextSegmentID.ToString());
                CritterSegment newSegment = newGO.AddComponent<CritterSegment>();
                builtSegmentsList.Add(newSegment);
                newGO.layer = LayerMask.NameToLayer("editorSegment"); ; // set segmentGO layer to editorSegment, to distinguish it from Gizmos
                newGO.GetComponent<MeshRenderer>().material = critterSegmentMaterial;
                newGO.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
                newGO.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);
                newGO.transform.SetParent(this.gameObject.transform);
                newGO.AddComponent<BoxCollider>().isTrigger = false;
                critterSegmentList.Add(newGO);  // Add to master Linear list of Segments
                newSegment.InitGamePiece();  // create the mesh and some other initialization stuff
                newSegment.sourceNode = currentBuildSegmentList[i].sourceNode;
                newSegment.id = nextSegmentID;  // !!!!!!!!!!!!!!!!!@$%@$#^@$^@$%^$%^#!!!!!!!!!!!!!!!!!!! REVISIT THIS!!!! should id's be different btw segments and nodes?
                nextSegmentID++;
                
                if (currentBuildSegmentList[i].sourceNode.ID == 0) {  // is ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check
                    newGO.transform.rotation = Quaternion.identity;
                    newSegment.scalingFactor = newSegment.sourceNode.jointLink.recursionScalingFactor;
                    newGO.transform.localScale = currentBuildSegmentList[i].sourceNode.dimensions * newSegment.scalingFactor;
                    if (physicsOn) {
                        newGO.AddComponent<Rigidbody>().isKinematic = true;
                        newGO.GetComponent<Rigidbody>().drag = 20f;
                        newGO.GetComponent<Rigidbody>().angularDrag = 20f;
                        // Bouncy Root:
                        GameObject anchorGO = new GameObject("Anchor");
                        anchorGO.transform.SetParent(this.gameObject.transform);
                        anchorGO.AddComponent<Rigidbody>().isKinematic = true;
                        
                        ConfigurableJoint configJoint = newGO.AddComponent<ConfigurableJoint>();
                        configJoint.autoConfigureConnectedAnchor = false;
                        configJoint.connectedBody = anchorGO.GetComponent<Rigidbody>();
                        configJoint.anchor = new Vector3(0f, 0f, 0f);
                        configJoint.connectedAnchor = new Vector3(0f, 0f, 0f);
                        configJoint.xMotion = ConfigurableJointMotion.Locked;
                        configJoint.yMotion = ConfigurableJointMotion.Locked;
                        configJoint.zMotion = ConfigurableJointMotion.Locked;
                        configJoint.angularXMotion = ConfigurableJointMotion.Locked;
                        configJoint.angularYMotion = ConfigurableJointMotion.Locked;
                        configJoint.angularZMotion = ConfigurableJointMotion.Locked;
                        SoftJointLimitSpring limitSpring = configJoint.linearLimitSpring;
                        limitSpring.spring = 80f;
                        limitSpring.damper = 800f;
                        configJoint.linearLimitSpring = limitSpring;
                        SoftJointLimit jointLimit = configJoint.linearLimit;
                        jointLimit.limit = 0.01f;
                        jointLimit.bounciness = 0.01f;
                        configJoint.linearLimit = jointLimit;
                        configJoint.angularXLimitSpring = limitSpring;
                        configJoint.angularYZLimitSpring = limitSpring;
                    }
                }
                else {  // if NOT root segment, can consider parent-related stuff:
                    
                    newSegment.parentSegment = currentBuildSegmentList[i].parentSegment;
                    // Inherit Axis-Inversions from parent segment:
                    newSegment.mirrorX = newSegment.parentSegment.mirrorX;
                    newSegment.mirrorY = newSegment.parentSegment.mirrorY;
                    newSegment.mirrorZ = newSegment.parentSegment.mirrorZ;
                    // inherit scaling factor from parent -- this is later adjusted again if it is part of a recursion chain
                    newSegment.scalingFactor = newSegment.parentSegment.scalingFactor;
                    newSegment.scalingFactor *= currentBuildSegmentList[i].sourceNode.jointLink.recursionScalingFactor; // propagate scaling factor
                    // Check for if the segment currently being built is a Mirror COPY:
                    if (currentBuildSegmentList[i].isMirror) {
                        //Debug.Log("This is a MIRROR COPY segment - Wow!");
                        if(currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorX) {
                            // Invert the X-axis  (this will propagate down to all this segment's children
                            newSegment.mirrorX = !newSegment.mirrorX;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorY) {
                            newSegment.mirrorY = !newSegment.mirrorY;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorZ) {
                            newSegment.mirrorZ = !newSegment.mirrorZ;
                        }
                    }
                }

                //Debug.Log("BUILD SEGMENT: " + (nextSegmentID - 1).ToString() + ", segID: " + newSegment.id.ToString());

                // CHECK FOR RECURSION:
                if (currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions > 0) { // if the node being considered has recursions:
                    //Debug.Log("currentNode: " + currentBuildSegmentList[i].sourceNode.ID.ToString() + "newSegmentRecursion#: " + newSegment.recursionNumber.ToString() + ", parentRecursion#: " + currentBuildSegmentList[i].parentSegment.recursionNumber.ToString());
                    if (newSegment.sourceNode == currentBuildSegmentList[i].parentSegment.sourceNode) {  // if this segment's sourceNode is the same is its parent Segment's sourceNode, then it is not the root of the recursion chain!
                        //Debug.Log("newSegment.sourceNode == currentBuildNodeParentSegmentList[i].sourceNode!");

                        // Are we at the end of a recursion chain?
                        if (currentBuildSegmentList[i].parentSegment.recursionNumber >= currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions) {
                            //Debug.Log("recursion number greater than numRecursions! ( " + currentBuildSegmentList[i].parentSegment.recursionNumber.ToString() + " vs " + currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions.ToString());
                            newSegment.recursionNumber = currentBuildSegmentList[i].parentSegment.recursionNumber + 1; //
                            //newSegment.scalingFactor *= currentBuildSegmentList[i].sourceNode.jointLink.recursionScalingFactor;
                        }
                        else {  // create new recursion instance!!
                            BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                            newSegmentInfo.sourceNode = currentBuildSegmentList[i].sourceNode;  // request a segment to be built again based on the current sourceNode
                            newSegment.recursionNumber = currentBuildSegmentList[i].parentSegment.recursionNumber + 1;
                            //newSegment.scalingFactor *= currentBuildSegmentList[i].sourceNode.jointLink.recursionScalingFactor; // propagate scaling factor
                            newSegmentInfo.parentSegment = newSegment; // parent of itself (the just-built Segment)
                            nextBuildSegmentList.Add(newSegmentInfo);
                            // If the node also has Symmetry:
                            if (newSegmentInfo.sourceNode.jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                newSegmentInfoMirror.sourceNode = currentBuildSegmentList[i].sourceNode;  // uses same sourceNode, but tags as Mirror:
                                newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                newSegmentInfoMirror.parentSegment = newSegment;  // 
                                nextBuildSegmentList.Add(newSegmentInfoMirror);
                            }
                        }
                    }
                    else { // this is the root --- its sourceNode has recursion, and this segment is unique from its parentNode:
                        BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                        newSegmentInfo.sourceNode = currentBuildSegmentList[i].sourceNode;
                        newSegment.recursionNumber = 1;
                        newSegmentInfo.parentSegment = newSegment;
                        nextBuildSegmentList.Add(newSegmentInfo);
                        // If the node also has Symmetry:
                        if (newSegmentInfo.sourceNode.jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                            // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                            BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                            newSegmentInfoMirror.sourceNode = currentBuildSegmentList[i].sourceNode;  // uses same sourceNode, but tags as Mirror:
                            newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                            newSegmentInfoMirror.parentSegment = newSegment;  // 
                            nextBuildSegmentList.Add(newSegmentInfoMirror);
                        }
                    }
                }
                // Figure out how many unique Child nodes this built node has:
                //int numberOfChildNodes = currentBuildSegmentList[i].sourceNode.attachedJointLinkList.Count; // old
                int numberOfChildNodes = currentBuildSegmentList[i].sourceNode.attachedChildNodesIdList.Count;
                //Debug.Log("numberOfChildNodes: " + numberOfChildNodes.ToString() + "currentBuildSegmentList[i].sourceNode: " + currentBuildSegmentList[i].sourceNode.ID.ToString() + ", i: " + i.ToString());
                for (int c = 0; c < numberOfChildNodes; c++) {

                    // if NO symmetry:
                    // Check if Attaching to a recursion chain && if onlyattachToTail is active:
                    int childID = currentBuildSegmentList[i].sourceNode.attachedChildNodesIdList[c];
                    //Debug.Log("%%%%% c=" + c.ToString() + ", childID: " + childID.ToString() + ", critterNodeListCount: " + masterCritterGenome.CritterNodeList.Count.ToString());
                    // if(currentBuildSegmentList[i].sourceNode.attachedJointLinkList[c].childNode.parentJointLink.onlyAttachToTailNode)  // OLD
                    if (masterCritterGenome.CritterNodeList[childID].jointLink.onlyAttachToTailNode) { 
                        if (currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions > 0) {
                            if (newSegment.recursionNumber > newSegment.sourceNode.jointLink.numberOfRecursions) {
                                // Only build segment if it is on the end of a recursion chain:
                                BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                                newSegmentInfo.sourceNode = masterCritterGenome.CritterNodeList[childID];
                                newSegmentInfo.parentSegment = newSegment;
                                nextBuildSegmentList.Add(newSegmentInfo);

                                if (masterCritterGenome.CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                    // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                    BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                    newSegmentInfoMirror.sourceNode = masterCritterGenome.CritterNodeList[childID];
                                    newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                    newSegmentInfoMirror.parentSegment = newSegment;  // 
                                    nextBuildSegmentList.Add(newSegmentInfoMirror);
                                }
                            }
                        }
                        else {
                            // It only attaches to End nodes, but is parented to a Non-recursive segment, so proceed normally!!!
                            BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                            newSegmentInfo.sourceNode = masterCritterGenome.CritterNodeList[childID];
                            newSegmentInfo.parentSegment = newSegment;
                            nextBuildSegmentList.Add(newSegmentInfo);

                            if (masterCritterGenome.CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                newSegmentInfoMirror.sourceNode = masterCritterGenome.CritterNodeList[childID];
                                newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                newSegmentInfoMirror.parentSegment = newSegment;  // 
                                nextBuildSegmentList.Add(newSegmentInfoMirror);
                            }
                        }                       
                    }
                    else {  // proceed normally:
                        BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                        newSegmentInfo.sourceNode = masterCritterGenome.CritterNodeList[childID];
                        newSegmentInfo.parentSegment = newSegment;
                        nextBuildSegmentList.Add(newSegmentInfo);

                        if (masterCritterGenome.CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                            // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                            BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                            newSegmentInfoMirror.sourceNode = masterCritterGenome.CritterNodeList[childID];
                            newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                            newSegmentInfoMirror.parentSegment = newSegment;  // 
                            nextBuildSegmentList.Add(newSegmentInfoMirror);
                        }
                    }             
                }

                // PositionSEGMENT GameObject!!!
                if(newSegment.id == 0) {  // if ROOT NODE:

                }
                else {
                    SetSegmentTransform(newGO);  // Properly position the SegmentGO where it should be  && scale!
                    if (physicsOn) {
                        newGO.AddComponent<Rigidbody>().isKinematic = false;
                        ConfigurableJoint configJoint = newGO.AddComponent<ConfigurableJoint>();
                        configJoint.autoConfigureConnectedAnchor = false;
                        configJoint.connectedBody = newSegment.parentSegment.gameObject.GetComponent<Rigidbody>();
                        configJoint.anchor = GetJointAnchor(newSegment);
                        configJoint.connectedAnchor = GetJointConnectedAnchor(newSegment); // <-- Might be Unnecessary
                        ConfigureJointSettings(newSegment, ref configJoint);
                    }
                }                
            }
            // After all buildNodes have been built, and their subsequent childNodes Enqueued, copy pendingChildQueue into buildNodesQueue
            currentBuildSegmentList.Clear();
            // SWAP LISTS:
            if (nextBuildSegmentList.Count > 0) {
                for (int j = 0; j < nextBuildSegmentList.Count; j++) {
                    currentBuildSegmentList.Add(nextBuildSegmentList[j]);
                }
                nextBuildSegmentList.Clear();  // empty this list for next depth-round
            }
            else {
                isPendingChildren = false;
            }
            if (currentDepth >= maxDepth) { // SAFEGUARD!! prevents infinite loop in case I mess something up
                isPendingChildren = false;
            }
            currentDepth++;
        }   
    }

    void ConfigureJointSettings(CritterSegment segment, ref ConfigurableJoint joint) {
        CritterNode node = segment.sourceNode;

        if (node.jointLink.jointType == CritterJointLink.JointType.Fixed) { // Fixed Joint
                                                                                  // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
        }
        else if (node.jointLink.jointType == CritterJointLink.JointType.HingeX) { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(1f, 0f, 0f);
            joint.secondaryAxis = new Vector3(0f, 1f, 0f);
            JointDrive jointDrive = joint.angularXDrive;
            //jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularXDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            if (node.jointLink.jointLimitPrimary == 180)
                joint.angularXMotion = ConfigurableJointMotion.Free;
            else
                joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            // Joint Limits:
            SoftJointLimit limitXMin = joint.lowAngularXLimit;
            limitXMin.limit = -node.jointLink.jointLimitPrimary;
            joint.lowAngularXLimit = limitXMin;
            SoftJointLimit limitXMax = joint.highAngularXLimit;
            limitXMax.limit = node.jointLink.jointLimitPrimary;
            joint.highAngularXLimit = limitXMax;
        }
        else if (node.jointLink.jointType == CritterJointLink.JointType.HingeY) { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(1f, 0f, 0f);
            joint.secondaryAxis = new Vector3(0f, 1f, 0f);
            JointDrive jointDrive = joint.angularYZDrive;
            //jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularYZDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            if (node.jointLink.jointLimitPrimary == 180)
                joint.angularYMotion = ConfigurableJointMotion.Free;
            else
                joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            // Joint Limits:
            SoftJointLimit limitY = joint.angularYLimit;
            limitY.limit = node.jointLink.jointLimitPrimary;
            joint.angularYLimit = limitY;
        }
        else if (node.jointLink.jointType == CritterJointLink.JointType.HingeZ) { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(0f, 1f, 0f);
            joint.secondaryAxis = new Vector3(1f, 0f, 0f);
            JointDrive jointDrive = joint.angularYZDrive;
            //jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularYZDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            if (node.jointLink.jointLimitPrimary == 180)
                joint.angularZMotion = ConfigurableJointMotion.Free;
            else
                joint.angularZMotion = ConfigurableJointMotion.Limited;
            // Joint Limits:
            SoftJointLimit limitZ = joint.angularZLimit;
            limitZ.limit = node.jointLink.jointLimitPrimary;
            joint.angularZLimit = limitZ;
        }
        else if (node.jointLink.jointType == CritterJointLink.JointType.DualXY) { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(1f, 0f, 0f);
            joint.secondaryAxis = new Vector3(0f, 1f, 0f);
            JointDrive jointDriveX = joint.angularXDrive;
            JointDrive jointDriveY = joint.angularYZDrive;
            //jointDrive.mode = JointDriveMode.Velocity;
            jointDriveX.positionDamper = 1f;
            jointDriveX.positionSpring = 1f;
            joint.angularXDrive = jointDriveX;
            jointDriveY.positionDamper = 1f;
            jointDriveY.positionSpring = 1f;
            joint.angularYZDrive = jointDriveY;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            if (node.jointLink.jointLimitPrimary == 180) {
                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Free;
            }                
            else {
                joint.angularXMotion = ConfigurableJointMotion.Limited;
                joint.angularYMotion = ConfigurableJointMotion.Limited;
            }
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            // Joint Limits:
            SoftJointLimit limitXMin = joint.lowAngularXLimit;
            limitXMin.limit = -node.jointLink.jointLimitPrimary;
            joint.lowAngularXLimit = limitXMin;
            SoftJointLimit limitXMax = joint.highAngularXLimit;
            limitXMax.limit = node.jointLink.jointLimitPrimary;
            joint.highAngularXLimit = limitXMax;
            SoftJointLimit limitYMax = joint.angularYLimit;
            limitYMax.limit = node.jointLink.jointLimitSecondary;
            joint.angularYLimit = limitYMax;
        }
    }

    private Vector3 GetJointAnchor(CritterSegment segment) {
        CritterNode node = segment.sourceNode;
        Vector3 retVec = new Vector3(0f, 0f, 0f);
        retVec.x = 0f;
        retVec.y = 0f;  // These might change when/if I implement childAttachPos, or initial Rotation
        retVec.z = -0.5f;
        return retVec;
    }

    private Vector3 GetJointConnectedAnchor(CritterSegment segment) {
        //Vector3 retVec = new Vector3(0f, 0f, 0f);
        //retVec.x = 0f;
        //retVec.y = 0f;  // These might change when/if I implement childAttachPos, or initial Rotation
        //retVec.z = -0.5f;
        CritterNode node = segment.sourceNode;
        Vector3 attachDir = node.jointLink.attachDir;
        // MODIFY AttachDir based on recursion Forward:
        // $*$*$*$*$*$*$*$ This code is duplicated in SetTransform --  Look into how to combine!!!
        if (segment.recursionNumber > 1) {  // if segment is not the root of a recursion chain
            attachDir = Vector3.Lerp(attachDir, new Vector3(0f, 0f, 1f), segment.sourceNode.jointLink.recursionForward);
        }
        if (segment.mirrorX) {
            attachDir.x *= -1f;
        }
        if (segment.mirrorY) {
            attachDir.y *= -1f;
        }
        if (segment.mirrorZ) {
            attachDir.z *= -1f;
        }
        return ConvertAttachDirToLocalCoords(attachDir);
    }

    private Vector3 ConvertAttachDirToLocalCoords(Vector3 attachDir) {
        //Vector3 LocalCoords = new Vector3(0f, 0f, 0f);

        //parentPos = parentSegment.transform.position;
        float x = attachDir.x;
        float y = attachDir.y;
        float z = attachDir.z;
        Vector3 normalDirection = new Vector3(0f, 0f, 0f);
        //int attachSide = 0;  // 0 = X, 1 = Y, 2 = Z
        // FIND DOMINANT SIDE OF CUBE:::
        if (Mathf.Abs(x) > Mathf.Abs(y)) {
            if (Mathf.Abs(x) > Mathf.Abs(z)) {  // x is largest
                if (x != 0)
                    x = x / Mathf.Abs(x); // make either -1 or 1 
                normalDirection.x = x;
                //attachSide = 0;
            }
            else {  // z is largest
                if (z != 0)
                    z = z / Mathf.Abs(z); // make either -1 or 1 
                normalDirection.z = z;
                //attachSide = 2;
            }
        }
        else {  // y>x
            if (Mathf.Abs(x) > Mathf.Abs(z)) {  // y is largest
                if (y != 0)
                    y = y / Mathf.Abs(y); // make either -1 or 1
                normalDirection.y = y;
                //attachSide = 1;
            }
            else {  // z>x
                    // x is smallest
                if (Mathf.Abs(y) > Mathf.Abs(z)) {  // y is largest
                    if (y != 0)
                        y = y / Mathf.Abs(y); // make either -1 or 1
                    normalDirection.y = y;
                    //attachSide = 1;
                }
                else { // z is largest
                    if (z != 0)
                        z = z / Mathf.Abs(z); // make either -1 or 1
                    normalDirection.z = z;
                    //attachSide = 2;
                }
            }
        }
        Vector3 right = new Vector3(1f, 0f, 0f);
        Vector3 up = new Vector3(0f, 1f, 0f);
        Vector3 forward = new Vector3(0f, 0f, 1f);
        right *= attachDir.x;
        up *= attachDir.y;
        forward *= attachDir.z;

        Vector3 attachDirLocal = new Vector3(0f, 0f, 0f);
        attachDirLocal = right + up + forward;
        float a = Vector3.Dot(normalDirection, attachDirLocal); // proportion of normalDirection that attachDirWorld reaches. This will determine how far to extend the ray to hit the cube face
        float ratio = 1f / a;
        attachDirLocal *= ratio;
        attachDirLocal *= 0.5f;

        //Vector3 attachPos = new Vector3(0f, 0f, 0f);
        /*float parentDepth = 0f;
        if (attachSide == 0) {  // attached to the X side, so use parent's X scale
            parentDepth = newSegment.parentSegment.sourceNode.dimensions.x * 0.5f;
        }
        else if (attachSide == 1) {
            parentDepth = newSegment.parentSegment.sourceNode.dimensions.y * 0.5f;
        }
        else {   //  attachSide == 2
            parentDepth = newSegment.parentSegment.sourceNode.dimensions.z * 0.5f;
        }*/
        //attachPosWorld = newSegment.parentSegment.transform.position + attachDirLocal * parentDepth;

        //Debug.Log("ConvertAttachDirToLocalCoords: " + attachDirLocal.ToString());
        return attachDirLocal;
    }

    private void EnqueueNodeForTranscription(CritterNode node) {

    }

    private void InitializeSegmentMaterial() {
        if (critterSegmentMaterial == null) {
            critterSegmentMaterial = new Material(Shader.Find("Custom/CritterSegmentBasic"));
            critterSegmentMaterial.renderQueue = 2000;
        }
    }

    public void DeleteSegments() {
        // Delete existing Segment GameObjects
        
        var children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        //foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
        //children.ForEach(child => Debug.Log("child!" + child.ToString()));

        //Debug.Log("DeleteSegments: " + critterSegmentList.Count.ToString());
        //for(int i = 0; i < critterSegmentList.Count; i++) {
        //    Destroy(critterSegmentList[i]);
        //}
        //Debug.Log("DeleteSegments: " + critterSegmentList.Count.ToString());
        critterSegmentList.Clear();
        //Debug.Log("DeleteSegments: " + critterSegmentList.Count.ToString());
    }

    public void RebuildBranchFromGenome(CritterNode node) {  // resets and re-constructs only a branch of the Critter, starting with specified node

    }

    public void DeleteNode(CritterNode node) {
        int deletedID = node.ID; // id of the node being deleted 
        CritterNode parentNode = masterCritterGenome.CritterNodeList[node.jointLink.parentNodeID];  // get parent of node being deleted
        //Debug.Log("DeleteNode before: " + masterCritterGenome.CritterNodeList.Count.ToString() + " deletedID: " + deletedID.ToString() + " parentChildCount: " + parentNode.attachedChildNodesIdList.Count.ToString());
        int childIndex = -1;
        // evalute parent of deleted node, go through its childList to find deleted Node ....        
        for(int i = 0; i < parentNode.attachedChildNodesIdList.Count; i++) {
            //Debug.Log("*******B$B$B$ i: " + i.ToString() + " deletedID: " + deletedID.ToString() + ", parentNodeID: " + parentNode.ID.ToString() + " attachedChildNodesIdList[i]: " + parentNode.attachedChildNodesIdList[i].ToString());
            // go through parent node's children list and remove this node from it before deleting it from master list:
            if (parentNode.attachedChildNodesIdList[i] == deletedID) { // if the child in parent's List is the child being deleted:
                //Debug.Log("******* i: " + i.ToString() + " deletedID: " + deletedID.ToString() + ", parentNodeID: " + parentNode.ID.ToString());
                childIndex = i;   // save index of node being deleted so it can be removed after loop             
            }
        }
        // and remove it from childList
        parentNode.attachedChildNodesIdList.RemoveAt(childIndex); // remove here to avoid shortening length of list while traversing it

        // Attach children of deleted node to parentNode:
        for(int j = 0; j < node.attachedChildNodesIdList.Count; j++) { // go through deleted node's children
            //Debug.Log("**** j: " + j.ToString() + " jchildID: " + node.attachedChildNodesIdList[j].ToString());
            //masterCritterGenome.CritterNodeList[  masterCritterGenome.CritterNodeList[  node.attachedChildNodesIdList[j]  ].jointLink.parentNodeID  ] = parentNode; // OLD; see if this needs to be done with ints rather than saving a ref to parentNode
            // Set child of deleted-node's parentID  to the original parent of the deleted node:
            masterCritterGenome.CritterNodeList[node.attachedChildNodesIdList[j]].jointLink.parentNodeID = parentNode.ID;
            // add the orphaned child ID's to the original parent of the deleted node:
            parentNode.attachedChildNodesIdList.Add(node.attachedChildNodesIdList[j]);
        }
        // Remove node from master List
        masterCritterGenome.CritterNodeList.RemoveAt(deletedID);
        //Debug.Log("DeleteNode after: " + masterCritterGenome.CritterNodeList.Count.ToString() + ", parentFullChildList: " + parentNode.attachedChildNodesIdList.Count.ToString());
    }

    public void RenumberNodes() {
        if(masterCritterGenome != null) {
            // traverse the main node List, if there is a gap in id's, iterate through all nodes/joints and adjust all ID's down by 1 until no gaps exist
            int checkingID = 0;
            int lastConsecutiveID = 0;
            for(int i = 0; i < masterCritterGenome.CritterNodeList.Count; i++) {
                //Debug.Log("RenumberNodesBEFORE checkingID: " + checkingID.ToString() + ", nodeID: " + masterCritterGenome.CritterNodeList[i].ID.ToString() + ", i: " + i.ToString());
                if (masterCritterGenome.CritterNodeList[i].ID == checkingID) { // if node exists for each checkingID
                    lastConsecutiveID = checkingID;
                    // no need to renumber, node exists for this number
                }
                else {  //  nodeID didn't match checkingID - there is a gap!
                    //masterCritterGenome.CritterNodeList[i].RenumberNodeID(masterCritterGenome.CritterNodeList[i].ID - 1);
                    
                    // Iterate through all nodes:
                    for (int j = 0; j < masterCritterGenome.CritterNodeList.Count; j++) {  // make this less of a brute force algo later!!!
                        // check parentNodeID to see if it is above the current Gap ID:
                        if (masterCritterGenome.CritterNodeList[j].jointLink.parentNodeID > checkingID) { 
                            // if so, subtract 1 to fill in gap
                            masterCritterGenome.CritterNodeList[j].jointLink.parentNodeID--;
                            // OLD:
                            //masterCritterGenome.CritterNodeList[  masterCritterGenome.CritterNodeList[j].jointLink.parentNodeID  ].jointLink.parentNodeID = masterCritterGenome.CritterNodeList[lastConsecutiveID].ID; // reset parentNodeID
                            //masterCritterGenome.CritterNodeList[  masterCritterGenome.CritterNodeList[j].jointLink.parentNodeID  ].RenumberNodeID(lastConsecutiveID);        // reset thisNode ID                    
                        }
                        if (masterCritterGenome.CritterNodeList[j].jointLink.thisNodeID > checkingID) {    // check if this node's joint selfID > checkingID:                       
                            masterCritterGenome.CritterNodeList[j].jointLink.thisNodeID--; // if so, subtract 1 to fill in gap
                        }
                        // check all child indices of this node:
                        for (int k = 0; k < masterCritterGenome.CritterNodeList[j].attachedChildNodesIdList.Count; k++) {
                            // if childID is greater than GapID, subtract 1 to bring it in line:
                            if(masterCritterGenome.CritterNodeList[j].attachedChildNodesIdList[k] > checkingID) {
                                masterCritterGenome.CritterNodeList[j].attachedChildNodesIdList[k]--;
                                
                            }
                            //Debug.Log("RenumberNodesCHILDREN j: " + j.ToString() + ", k: " + k.ToString() + ", childID: " + masterCritterGenome.CritterNodeList[j].attachedChildNodesIdList[k].ToString());
                        }
                        // if current Node's id is greater than gapID, subtract 1:
                        if(masterCritterGenome.CritterNodeList[j].ID > checkingID) {
                            masterCritterGenome.CritterNodeList[j].RenumberNodeID(masterCritterGenome.CritterNodeList[j].ID - 1);
                        }
                    }
                    //OLDOLD: masterCritterGenome.CritterNodeList[i].RenumberNodeID(checkingID); // set id of node to newID
                    // Set all other references:
                    //Debug.Log("RenumberNodesMIDDle checkingID: " + checkingID.ToString() + ", nodeID: " + masterCritterGenome.CritterNodeList[i].ID.ToString() + ", i: " + i.ToString());
                }
                if(i != 0) {
                    //Debug.Log("RenumberNodes checkingID: " + checkingID.ToString() + ", nodeID: " + masterCritterGenome.CritterNodeList[i].ID.ToString() + ", i: " + i.ToString() + ", " + masterCritterGenome.CritterNodeList[i].jointLink.parentNodeID.ToString());
                }
                

                checkingID++;
            }
            //Debug.Log("genome# nodes: " + masterCritterGenome.CritterNodeList.Count.ToString());
        }
    }

    public void LoadCritterGenome(CritterGenome newGenome) {
        Debug.Log("LoadCritterGenome!!");
        masterCritterGenome = newGenome;
        masterCritterGenome.ReconstructGenomeFromLoad();
        RebuildCritterFromGenomeRecursive(false);
    }
}
