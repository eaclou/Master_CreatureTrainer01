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

        RebuildCritterFromGenome(false);
    }

    public void UpdateCritterFromGenome() {  // only use if the genome node graph hasn't changed -- only attributes & settings!!!
        for (int i = 0; i < masterCritterGenome.CritterNodeList.Count; i++) {  // cycle through ChildNodes of currentNode
            critterSegmentList[i].transform.localScale = masterCritterGenome.CritterNodeList[i].dimensions;
            //Debug.Log("dimensions: " + masterCritterGenome.CritterNodeList[i].dimensions.ToString());
            // if Root node:
            if (i == 0) {   // RE-VISIT!!!!
                critterSegmentList[i].transform.rotation = Quaternion.identity;                
            }
            else { // not root node!!
                SetSegmentTransform(critterSegmentList[i]); // update segment's position & orientation based on its Parent segment & both segments' dimensions
            }
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_SizeX", critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.dimensions.x);
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_SizeY", critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.dimensions.y);
            critterSegmentList[i].GetComponent<MeshRenderer>().material.SetFloat("_SizeZ", critterSegmentList[i].GetComponent<CritterSegment>().sourceNode.dimensions.z);
        }
    }

    private void SetSegmentTransform(GameObject newSegmentGO) {
        Vector3 parentPos = new Vector3(0f, 0f, 0f);
        Vector3 newSegmentPos = new Vector3(0f, 0f, 0f);
        CritterSegment newSegment = newSegmentGO.GetComponent<CritterSegment>();
        //Debug.Log("newSegment.parentSegment: " + newSegment.parentSegment.ToString());
        parentPos = newSegment.parentSegment.transform.position;
        float x = newSegment.sourceNode.parentJointLink.attachDir.x;
        float y = newSegment.sourceNode.parentJointLink.attachDir.y;
        float z = newSegment.sourceNode.parentJointLink.attachDir.z;
        Vector3 normalDirection = new Vector3(0f, 0f, 0f);
        int attachSide = 0;  // 0 = X, 1 = Y, 2 = Z
        if (Mathf.Abs(x) > Mathf.Abs(y)) {
            if (Mathf.Abs(x) > Mathf.Abs(z)) {  // x is largest
                if (x != 0)
                    x = x / Mathf.Abs(x); // make either -1 or 1 
                                          //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = x * newSegment.parentSegment.transform.right;
                attachSide = 0;
            }
            else {  // z is largest
                if (z != 0)
                    z = z / Mathf.Abs(z); // make either -1 or 1 
                                          //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = z * newSegment.parentSegment.transform.forward;
                attachSide = 2;
            }
        }
        else {  // y>x
            if (Mathf.Abs(x) > Mathf.Abs(z)) {  // y is largest
                if (y != 0)
                    y = y / Mathf.Abs(y); // make either -1 or 1 
                                          //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                normalDirection = y * newSegment.parentSegment.transform.up;
                attachSide = 1;
            }
            else {  // z>x
                    // x is smallest
                if (Mathf.Abs(y) > Mathf.Abs(z)) {  // y is largest
                    if (y != 0)
                        y = y / Mathf.Abs(y); // make either -1 or 1 
                                              //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                    normalDirection = y * newSegment.parentSegment.transform.up;
                    attachSide = 1;
                }
                else { // z is largest
                    if (z != 0)
                        z = z / Mathf.Abs(z); // make either -1 or 1 
                                              //Debug.Log("x " + x.ToString() + ", y " + y.ToString() + ", z " + z.ToString() + "... " + selectedObject.GetComponent<CritterSegment>().transform.forward.ToString());
                    normalDirection = z * newSegment.parentSegment.transform.forward;
                    attachSide = 2;
                }
            }
        }
        Vector3 right = newSegment.parentSegment.transform.right;
        Vector3 up = newSegment.parentSegment.transform.up;
        Vector3 forward = newSegment.parentSegment.transform.forward;
        right *= newSegment.sourceNode.parentJointLink.attachDir.x * newSegment.sourceNode.parentJointLink.parentNode.dimensions.x;
        up *= newSegment.sourceNode.parentJointLink.attachDir.y * newSegment.sourceNode.parentJointLink.parentNode.dimensions.y;
        forward *= newSegment.sourceNode.parentJointLink.attachDir.z * newSegment.sourceNode.parentJointLink.parentNode.dimensions.z;

        Vector3 attachDirWorld = new Vector3(0f, 0f, 0f);
        attachDirWorld = right + up + forward;
        float a = Vector3.Dot(normalDirection, attachDirWorld); // proportion of normalDirection that attachDirWorld reaches. This will determine how far to extend the ray to hit the cube face
        float ratio = 1f / a;
        attachDirWorld *= ratio;

        Vector3 attachPosWorld = new Vector3(0f, 0f, 0f);
        float parentDepth = 0f;
        if (attachSide == 0) {  // attached to the X side, so use parent's X scale
            parentDepth = newSegment.parentSegment.sourceNode.dimensions.x * 0.5f;
        }
        else if (attachSide == 1) {
            parentDepth = newSegment.parentSegment.sourceNode.dimensions.y * 0.5f;
        }
        else {   //  attachSide == 2
            parentDepth = newSegment.parentSegment.sourceNode.dimensions.z * 0.5f;
        }
        attachPosWorld = newSegment.parentSegment.transform.position + attachDirWorld * parentDepth;
        //Debug.Log("attachPosWorld = " + attachPosWorld.ToString());
        newSegmentPos = attachPosWorld + normalDirection * newSegment.sourceNode.dimensions.z * 0.5f;  // REVISIT -- will only work for cubes! 
        newSegment.transform.rotation = Quaternion.LookRotation(normalDirection);
        newSegmentGO.transform.position = newSegmentPos;
        //Debug.Log("attachPosWorld = " + attachPosWorld.ToString() + ", attachPosition = " + ") newSegmentPos = " + newSegmentPos.ToString());
        //Debug.Log("a= " + a.ToString() + ", normalDirection = " + normalDirection.ToString() + ") attachDirWorld= " + attachDirWorld.ToString() + ", parentPos " + newSegment.parentSegment.gameObject.transform.position.ToString());
    }

    public void RebuildCritterFromGenome(bool physicsOn) {  // resets and constructs the entire critter based on its current genome -- recreating all of its segments
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
                newSegment.parentSegment = critterSegmentList[newSegment.sourceNode.parentJointLink.parentNode.ID].GetComponent<CritterSegment>();
                Debug.Log("RebuildCritter() newSegment.parentSegment: " + newSegment.parentSegment.ToString() + ", " + masterCritterGenome.CritterNodeList[i].parentJointLink.parentNode.ID.ToString());
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
    }
    public void RebuildCritterFromGenomeRecursive(bool physicsOn) {
        // Delete existing Segment GameObjects
        DeleteSegments();
        InitializeSegmentMaterial();

        //Debug.Log("RebuildCritter()");
        critterSegmentList = new List<GameObject>();
        // interpret Genome and construct critter in its bind pose
        bool isChildren = true;
        int currentDepth = 0; // start with RootNode
        int nextSegmentID = 0;
        //int currentParentNode = 0;
        List<CritterNode> buildNodesQueue = new List<CritterNode>();
        List<CritterNode> pendingChildNodesQueue = new List<CritterNode>();
        //CritterNode currentProcessNode;  // the node currently being considered for Building
        buildNodesQueue.Add(masterCritterGenome.CritterNodeList[0]);  // Add the root node to the Build Queue
        

        // Do a Breadth-first traversal??
        while(isChildren) {
            //int numberOfChildNodes = masterCritterGenome.CritterNodeList[currentNode].attachedJointLinkList.Count;
            Debug.Log("currentDepth: " + currentDepth.ToString() + "buildNodesQueue.Count: " + buildNodesQueue.Count.ToString());
            for (int i = 0; i < buildNodesQueue.Count; i++) {
                // Iterate through Child nodes
                // Build current Child node --> Segment, and add that node to a Queue for processing
                GameObject newNode = new GameObject("Node" + nextSegmentID.ToString());
                CritterSegment newSegment = newNode.AddComponent<CritterSegment>();
                newNode.layer = LayerMask.NameToLayer("editorSegment"); ; // set segmentGO layer to editorSegment, to distinguish it from Gizmos
                newNode.GetComponent<MeshRenderer>().material = critterSegmentMaterial;
                newNode.GetComponent<MeshRenderer>().material.SetFloat("_DisplayTarget", 0f);
                newNode.GetComponent<MeshRenderer>().material.SetFloat("_Selected", 0f);
                newNode.transform.SetParent(this.gameObject.transform);
                newNode.AddComponent<BoxCollider>().isTrigger = false;
                critterSegmentList.Add(newNode);  // Add to master Linear list of Segments
                newSegment.InitGamePiece();  // create the mesh and some other initialization stuff
                newSegment.sourceNode = buildNodesQueue[i];
                newSegment.id = nextSegmentID;  // !!!!!!!!!!!!!!!!!@$%@$#^@$^@$%^$%^#!!!!!!!!!!!!!!!!!!! REVISIT THIS!!!! should id's be different btw segments and nodes?
                newNode.transform.localScale = masterCritterGenome.CritterNodeList[i].dimensions;
                nextSegmentID++;
                if (buildNodesQueue[i].parentJointLink.parentNode == null) {  // is ROOT segment
                    newNode.transform.rotation = Quaternion.identity;
                }
                else {
                    // How best to keep track of SEGMENT structure/parents/children???
                    //  FIX PARENT SEGMENT!!!!! figure it out
                    for(int p = 0; p < critterSegmentList.Count; p++) {
                        if(critterSegmentList[p].GetComponent<CritterSegment>().sourceNode.ID == newSegment.sourceNode.parentJointLink.parentNode.ID) {
                            // traverse all currently built segments -- if a segment's genome node id matches that of the new one, it's a parent
                            // however, with recursion, this will return potentially multiple matches!!
                            newSegment.parentSegment = critterSegmentList[p].GetComponent<CritterSegment>();
                        }
                    }     
                    // +++++++ Also set CritterSegment. CHILD_Segments!!!!! *****************               
                    //newSegment.parentSegment = critterSegmentList[newSegment.sourceNode.parentJointLink.parentNode.ID].GetComponent<CritterSegment>(); // OLD
                    //Debug.Log("RebuildCritter() newSegment.parentSegment: " + newSegment.parentSegment.ToString() + ", " + masterCritterGenome.CritterNodeList[i].parentJointLink.parentNode.ID.ToString());
                    SetSegmentTransform(newNode);
                }
                Debug.Log("BUILD SEGMENT: " + buildNodesQueue[i].ID.ToString());
                // Figure out how many Child nodes this built node has:
                
                // CHECK FOR RECURSION:
                if(buildNodesQueue[i].parentJointLink.numberOfRecursions > 0 && buildNodesQueue[i].parentJointLink.recursionInstances < buildNodesQueue[i].parentJointLink.numberOfRecursions) {
                    // if this JointLink has recursion, AND the number of recursion instances created so far is below the max # of recursions
                    // Then add this node itself into the childNodeQueue
                    buildNodesQueue[i].parentJointLink.recursionInstances++;  // incrememt # times recursion has been used for this jointLink
                    pendingChildNodesQueue.Add(buildNodesQueue[i]);
                }
                int numberOfChildNodes = buildNodesQueue[i].attachedJointLinkList.Count;
                Debug.Log("currentNode: " + buildNodesQueue[i].ID.ToString() + "numberOfChildNodes: " + numberOfChildNodes.ToString());
                for (int c = 0; c < numberOfChildNodes; c++) {
                    pendingChildNodesQueue.Add(buildNodesQueue[i].attachedJointLinkList[c].childNode);  // Add each child node to the nodeQueue
                }
            }
            // After all buildNodes have been built, and their subsequent childNodes Enqueued, copy pendingChildQueue into buildNodesQueue
            buildNodesQueue.Clear();
            if(pendingChildNodesQueue.Count > 0) {
                for (int j = 0; j < pendingChildNodesQueue.Count; j++) {
                    buildNodesQueue.Add(pendingChildNodesQueue[j]);
                }
                pendingChildNodesQueue.Clear();  // empty the childNodesQueue
                Debug.Log("buildNodesQueue: " + buildNodesQueue.Count.ToString());
            }
            else {
                isChildren = false;
            }
            currentDepth++;
        }        
    }

    void ConfigureJointSettings(CritterNode node, ref ConfigurableJoint joint) {

        if (node.parentJointLink.jointType == CritterJointLink.JointType.Fixed) { // Fixed Joint
                                                                           // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
        }
        else if (node.parentJointLink.jointType == CritterJointLink.JointType.HingeX) { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(1f, 0f, 0f);
            joint.secondaryAxis = new Vector3(0f, 1f, 0f);
            JointDrive jointDrive = joint.angularXDrive;
            jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularXDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            // Joint Limits:
            SoftJointLimit limitXMin = joint.lowAngularXLimit;
            limitXMin.limit = -node.parentJointLink.jointLimitMaxTemp;
            joint.lowAngularXLimit = limitXMin;
            SoftJointLimit limitXMax = joint.highAngularXLimit;
            limitXMax.limit = node.parentJointLink.jointLimitMaxTemp;
            joint.highAngularXLimit = limitXMax;
        }
        else if (node.parentJointLink.jointType == CritterJointLink.JointType.HingeY) { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(1f, 0f, 0f);
            joint.secondaryAxis = new Vector3(0f, 1f, 0f);
            JointDrive jointDrive = joint.angularYZDrive;
            jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularYZDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            // Joint Limits:
            SoftJointLimit limitY = joint.angularYLimit;
            limitY.limit = node.parentJointLink.jointLimitMaxTemp;
            joint.angularYLimit = limitY;
        }
        else if (node.parentJointLink.jointType == CritterJointLink.JointType.HingeZ) { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(0f, 1f, 0f);
            joint.secondaryAxis = new Vector3(1f, 0f, 0f);
            JointDrive jointDrive = joint.angularYZDrive;
            jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularYZDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Limited;
            // Joint Limits:
            SoftJointLimit limitZ = joint.angularZLimit;
            limitZ.limit = node.parentJointLink.jointLimitMaxTemp;
            joint.angularZLimit = limitZ;
        }
    }

    private Vector3 GetJointAnchor(CritterNode node) {
        Vector3 retVec = new Vector3(0f, 0f, 0f);
        retVec.x = 0f;
        retVec.y = 0f;  // These might change when/if I implement childAttachPos, or initial Rotation
        retVec.z = -0.5f;
        return retVec;
    }

    private Vector3 GetJointConnectedAnchor(CritterNode node) {
        //Vector3 retVec = new Vector3(0f, 0f, 0f);
        //retVec.x = 0f;
        //retVec.y = 0f;  // These might change when/if I implement childAttachPos, or initial Rotation
        //retVec.z = -0.5f;

        return ConvertAttachDirToLocalCoords(node.parentJointLink.attachDir);
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

        Debug.Log("ConvertAttachDirToLocalCoords: " + attachDirLocal.ToString());
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

    private void DeleteSegments() {
        // Delete existing Segment GameObjects
        var children = new List<GameObject>();
        foreach (Transform child in this.gameObject.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public void RebuildBranchFromGenome(CritterNode node) {  // resets and re-constructs only a branch of the Critter, starting with specified node

    }

    public void DeleteNode(CritterNode node) {
        int id = node.ID;
        Debug.Log("DeleteNode before: " + masterCritterGenome.CritterNodeList.Count.ToString());
        masterCritterGenome.CritterNodeList.RemoveAt(id);
        Debug.Log("DeleteNode after: " + masterCritterGenome.CritterNodeList.Count.ToString());
    }

    public void RenumberNodes() {
        if(masterCritterGenome != null) {
            // traverse the main node List, if there is a gap in id's, change the id of next lowest node
            int checkingID = 0;
            //int nextFreeID = -1;
            int lastConsecutiveID = 0;
            for(int i = 0; i < masterCritterGenome.CritterNodeList.Count; i++) {
                Debug.Log("RenumberNodesBEFORE checkingID: " + checkingID.ToString() + ", nodeID: " + masterCritterGenome.CritterNodeList[i].ID.ToString() + ", i: " + i.ToString());
                if (masterCritterGenome.CritterNodeList[i].ID == checkingID) { // if node exists for each checkingID
                    lastConsecutiveID = checkingID;
                    // no need to renumber, node exists for this number
                }
                else {  //  nodeID didn't match checkingID - there is a gap!
                    // Search for instances of references to this node -- i.e. children of this node
                    for(int j = 1; j < masterCritterGenome.CritterNodeList.Count; j++) {  // make this less of a brute force algo later!!!
                        // SKIP ROOT ^^^^
                        Debug.Log("objectReference2: j: " + j.ToString() + ", " + masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.ToString() + ", " + masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.ID.ToString() + ", lci: " + lastConsecutiveID.ToString());
                        //if (masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode == null) { // was a child of the missing segment
                        if (masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.ID == i) { // 
                            // attach to current node?
                            //Debug.Log("objectReference: " + masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.ToString() + ", " + masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.ID.ToString() + ", lci: " + lastConsecutiveID.ToString());
                            
                            masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode = masterCritterGenome.CritterNodeList[lastConsecutiveID];
                            masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.RenumberNodeID(lastConsecutiveID);
                            

                        }
                           //if (masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.ID == masterCritterGenome.CritterNodeList[i].ID) {
                            // update the referenced ID value to what the node is being renumbered to:
                            //masterCritterGenome.CritterNodeList[j].parentJointLink.parentNode.ID = checkingID;
                        //}
                    }
                    masterCritterGenome.CritterNodeList[i].RenumberNodeID(checkingID); // set id of node to newID
                    // Set all other references:
                    Debug.Log("RenumberNodesMIDDle checkingID: " + checkingID.ToString() + ", nodeID: " + masterCritterGenome.CritterNodeList[i].ID.ToString() + ", i: " + i.ToString());
                }
                if(i != 0) {
                    Debug.Log("RenumberNodes checkingID: " + checkingID.ToString() + ", nodeID: " + masterCritterGenome.CritterNodeList[i].ID.ToString() + ", i: " + i.ToString() + ", " + masterCritterGenome.CritterNodeList[i].parentJointLink.parentNode.ID.ToString());
                }
                

                checkingID++;
            }
        }
    }

}
