using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Critter : MonoBehaviour {

    public CritterGenome masterCritterGenome;
    public Material critterSegmentMaterial;
    public PhysicMaterial segmentPhysicMaterial;
    // List of generated segments, based on critter's full Genome
    public List<GameObject> critterSegmentList;

    public List<BrainInputChannel> inputChannelsList;
    public List<BrainOutputChannel> outputChannelsList;

    // Should these 'live' on each segment themselves, or stay as separate global lists??  v v v
    public List<SegaddonPhysicalAttributes> segaddonPhysicalAttributesList;

    public List<SegaddonJointAngleSensor> segaddonJointAngleSensorList;
    public List<SegaddonContactSensor> segaddonContactSensorList;
    public List<SegaddonRaycastSensor> segaddonRaycastSensorList;    
    public List<SegaddonCompassSensor1D> segaddonCompassSensor1DList;
    public List<SegaddonCompassSensor3D> segaddonCompassSensor3DList;
    public List<SegaddonPositionSensor1D> segaddonPositionSensor1DList;
    public List<SegaddonPositionSensor3D> segaddonPositionSensor3DList;
    public List<SegaddonRotationSensor1D> segaddonRotationSensor1DList;
    public List<SegaddonRotationSensor3D> segaddonRotationSensor3DList;
    public List<SegaddonVelocitySensor1D> segaddonVelocitySensor1DList;
    public List<SegaddonVelocitySensor3D> segaddonVelocitySensor3DList;
    public List<SegaddonAltimeter> segaddonAltimeterList;
    public List<SegaddonEarBasic> segaddonEarBasicList;
    public List<SegaddonGravitySensor> segaddonGravitySensorList;
    public List<SegaddonOscillatorInput> segaddonOscillatorInputList;
    public List<SegaddonValueInput> segaddonValueInputList;
    public List<SegaddonTimerInput> segaddonTimerInputList;

    public List<SegaddonJointMotor> segaddonJointMotorList;
    public List<SegaddonThrusterEffector1D> segaddonThrusterEffector1DList;
    public List<SegaddonThrusterEffector3D> segaddonThrusterEffector3DList;
    public List<SegaddonTorqueEffector1D> segaddonTorqueEffector1DList;
    public List<SegaddonTorqueEffector3D> segaddonTorqueEffector3DList;
    public List<SegaddonMouthBasic> segaddonMouthBasicList;
    public List<SegaddonNoiseMakerBasic> segaddonNoiseMakerBasicList;
    public List<SegaddonSticky> segaddonStickyList;
    public List<SegaddonWeaponBasic> segaddonWeaponBasicList;
    

    public List<PhysicMaterial> segmentPhysicMaterialList;

    public void InitializeBlankCritter() {
        if(critterSegmentList == null) {
            critterSegmentList = new List<GameObject>(); // the 'official' record of this critters Segments
        }

        Debug.Log("InitializeCritterFromGenome(CritterGenome genome)  created SEGADDON LISTS!!!");
        InitializeAddonLists();
        if(segmentPhysicMaterialList == null) {
            segmentPhysicMaterialList = new List<PhysicMaterial>();
        }
        CreateBlankCritterGenome();        
    }

    public void InitializeCritterFromGenome(CritterGenome genome) {
        masterCritterGenome = genome;
        if (critterSegmentList == null) {
            critterSegmentList = new List<GameObject>(); // the 'official' record of this critters Segments
        }
        InitializeAddonLists();
        Debug.Log("InitializeCritterFromGenome(CritterGenome genome)  created SEGADDON LISTS!!!");
        
    }

    public void InitializeAddonLists() {
        if (segaddonPhysicalAttributesList == null) {
            segaddonPhysicalAttributesList = new List<SegaddonPhysicalAttributes>();
        }

        if (segaddonJointAngleSensorList == null) {
            segaddonJointAngleSensorList = new List<SegaddonJointAngleSensor>(); 
        }
        if (segaddonContactSensorList == null) {
            segaddonContactSensorList = new List<SegaddonContactSensor>(); 
        }
        if (segaddonRaycastSensorList == null) {
            segaddonRaycastSensorList = new List<SegaddonRaycastSensor>(); 
        }
        if (segaddonCompassSensor1DList == null) {
            segaddonCompassSensor1DList = new List<SegaddonCompassSensor1D>(); 
        }
        if (segaddonCompassSensor3DList == null) {
            segaddonCompassSensor3DList = new List<SegaddonCompassSensor3D>(); 
        }
        if (segaddonPositionSensor1DList == null) {
            segaddonPositionSensor1DList = new List<SegaddonPositionSensor1D>();
        }
        if (segaddonPositionSensor3DList == null) {
            segaddonPositionSensor3DList = new List<SegaddonPositionSensor3D>();
        }
        if (segaddonRotationSensor1DList == null) {
            segaddonRotationSensor1DList = new List<SegaddonRotationSensor1D>();
        }
        if (segaddonRotationSensor3DList == null) {
            segaddonRotationSensor3DList = new List<SegaddonRotationSensor3D>();
        }
        if (segaddonVelocitySensor1DList == null) {
            segaddonVelocitySensor1DList = new List<SegaddonVelocitySensor1D>();
        }
        if (segaddonVelocitySensor3DList == null) {
            segaddonVelocitySensor3DList = new List<SegaddonVelocitySensor3D>();
        }
        if (segaddonAltimeterList == null) {
            segaddonAltimeterList = new List<SegaddonAltimeter>();
        }
        if (segaddonEarBasicList == null) {
            segaddonEarBasicList = new List<SegaddonEarBasic>();
        }
        if (segaddonGravitySensorList == null) {
            segaddonGravitySensorList = new List<SegaddonGravitySensor>();
        }
        if (segaddonOscillatorInputList == null) {
            segaddonOscillatorInputList = new List<SegaddonOscillatorInput>();
        }
        if (segaddonValueInputList == null) {
            segaddonValueInputList = new List<SegaddonValueInput>();
        }
        if (segaddonTimerInputList == null) {
            segaddonTimerInputList = new List<SegaddonTimerInput>();
        }

        if (segaddonJointMotorList == null) {
            segaddonJointMotorList = new List<SegaddonJointMotor>(); 
        }
        if (segaddonThrusterEffector1DList == null) {
            segaddonThrusterEffector1DList = new List<SegaddonThrusterEffector1D>();
        }
        if (segaddonThrusterEffector3DList == null) {
            segaddonThrusterEffector3DList = new List<SegaddonThrusterEffector3D>();
        }
        if (segaddonTorqueEffector1DList == null) {
            segaddonTorqueEffector1DList = new List<SegaddonTorqueEffector1D>();
        }
        if (segaddonTorqueEffector3DList == null) {
            segaddonTorqueEffector3DList = new List<SegaddonTorqueEffector3D>();
        }
        if (segaddonMouthBasicList == null) {
            segaddonMouthBasicList = new List<SegaddonMouthBasic>();
        }
        if (segaddonNoiseMakerBasicList == null) {
            segaddonNoiseMakerBasicList = new List<SegaddonNoiseMakerBasic>();
        }
        if (segaddonStickyList == null) {
            segaddonStickyList = new List<SegaddonSticky>();
        }
        if (segaddonWeaponBasicList == null) {
            segaddonWeaponBasicList = new List<SegaddonWeaponBasic>();
        }        
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
        newSegment.surfaceArea = new Vector3(newSegmentDimensions.y * newSegmentDimensions.z * 2f, newSegmentDimensions.x * newSegmentDimensions.z * 2f, newSegmentDimensions.x * newSegmentDimensions.y * 2f);       
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
            // subtract intersecting surface areas:  // DOESN"T HANDLE OFFSET POSITIONS YET!!!
            // uses y & z of parent size, but x & y of own size, since it always has its 'back' to the parent
            float overlap = (Mathf.Min(newSegmentDimensions.y, newSegmentParentDimensions.y) * Mathf.Min(newSegmentDimensions.x, newSegmentParentDimensions.z));
            // uses math.max to prevent going below 0
            newSegment.surfaceArea.z = Mathf.Max(0f, newSegment.surfaceArea.z - overlap);  // subtract the shared region from this segment that should not be in contact with the water
            newSegment.parentSegment.surfaceArea.x = Mathf.Max(0f, newSegment.parentSegment.surfaceArea.x - overlap);  // subtract the shared region from the parent segment that should not be in contact with the water
        }
        else if (attachSide == 1) {
            parentDepth = newSegmentParentDimensions.y * 0.5f;

            float overlap = (Mathf.Min(newSegmentDimensions.x, newSegmentParentDimensions.x) * Mathf.Min(newSegmentDimensions.y, newSegmentParentDimensions.z));
            newSegment.surfaceArea.z = Mathf.Max(0f, newSegment.surfaceArea.z - overlap); 
            newSegment.parentSegment.surfaceArea.y = Mathf.Max(0f, newSegment.parentSegment.surfaceArea.y - overlap); 
        }
        else {   //  attachSide == 2
            parentDepth = newSegmentParentDimensions.z * 0.5f;

            float overlap = (Mathf.Min(newSegmentDimensions.x, newSegmentParentDimensions.x) * Mathf.Min(newSegmentDimensions.y, newSegmentParentDimensions.y));
            newSegment.surfaceArea.z = Mathf.Max(0f, newSegment.surfaceArea.z - overlap);
            newSegment.parentSegment.surfaceArea.z = Mathf.Max(0f, newSegment.parentSegment.surfaceArea.z - overlap);
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

    /*
    public int CalculateNumSegments() {
        int numSegments = 0;
        RebuildCritterFromGenomeRecursive(false);
        numSegments = critterSegmentList.Count;
        Debug.Log("CalculateNumSegments: " + numSegments.ToString());
        DeleteSegments();
        return numSegments;
    }
    */  // deprecated, use similar function within CritterGenome class
    
    public void RebuildCritterFromGenomeRecursive(bool physicsOn) {
        //Debug.Log("RebuildCritterFromGenomeRecursive: " + masterCritterGenome.CritterNodeList.Count.ToString());
        masterCritterGenome.PreBuildCritter(0.8f);
        // Delete existing Segment GameObjects
        DeleteSegments();
        // Is this the best way to clear the lists? from a memory standpoint...
        segaddonPhysicalAttributesList.Clear();
        
        segaddonJointAngleSensorList.Clear();
        segaddonContactSensorList.Clear();
        segaddonRaycastSensorList.Clear();
        segaddonCompassSensor1DList.Clear();
        segaddonCompassSensor3DList.Clear();
        segaddonPositionSensor1DList.Clear();
        segaddonPositionSensor3DList.Clear();
        segaddonRotationSensor1DList.Clear();
        segaddonRotationSensor3DList.Clear();
        segaddonVelocitySensor1DList.Clear();
        segaddonVelocitySensor3DList.Clear();
        segaddonAltimeterList.Clear();
        segaddonEarBasicList.Clear();
        segaddonGravitySensorList.Clear();
        segaddonOscillatorInputList.Clear();
        segaddonValueInputList.Clear();
        segaddonTimerInputList.Clear();

        segaddonJointMotorList.Clear();
        segaddonThrusterEffector1DList.Clear();
        segaddonThrusterEffector3DList.Clear();
        segaddonTorqueEffector1DList.Clear();
        segaddonTorqueEffector3DList.Clear();
        segaddonMouthBasicList.Clear();
        segaddonNoiseMakerBasicList.Clear();
        segaddonStickyList.Clear();
        segaddonWeaponBasicList.Clear();        

        InitializeSegmentMaterial();

        if (critterSegmentList != null) {
            critterSegmentList.Clear();
        }
        if(inputChannelsList != null) {
            inputChannelsList.Clear();
        }
        else {
            inputChannelsList = new List<BrainInputChannel>();
        }
        if (outputChannelsList != null) {
            outputChannelsList.Clear();
        }
        else {
            outputChannelsList = new List<BrainOutputChannel>();
        }
        // interpret Genome and construct critter in its bind pose
        bool isPendingChildren = true;
        int currentDepth = 0; // start with RootNode
        int maxDepth = 20;  // safeguard to prevent while loop lock
        int nextSegmentID = 0;
        float critterTotalVolume = 0f;
        List<CritterSegment> builtSegmentsList = new List<CritterSegment>();  // keep track of segments that have been built - linear in-order array 0-n segments
        List<BuildSegmentInfo> currentBuildSegmentList = new List<BuildSegmentInfo>();  // keeps track of all current-depth segment build-requests, and holds important metadata
        List<BuildSegmentInfo> nextBuildSegmentList = new List<BuildSegmentInfo>();  // used to keep track of next childSegments that need to be built

        // ***********  Will attempt to traverse the Segments to be created, keeping track of where on each graph (nodes# & segment#) the current build is on.
        BuildSegmentInfo rootSegmentBuildInfo = new BuildSegmentInfo();
        rootSegmentBuildInfo.sourceNode = masterCritterGenome.CritterNodeList[0];
        currentBuildSegmentList.Add(rootSegmentBuildInfo);  // ROOT NODE IS SPECIAL!        

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
                newSegment.id = nextSegmentID;
                nextSegmentID++;
                
                if (currentBuildSegmentList[i].sourceNode.ID == 0) {  // is ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check
                    newGO.transform.position = masterCritterGenome.centerOfMassOffset;
                    newGO.transform.rotation = Quaternion.identity;
                    newSegment.scalingFactor = newSegment.sourceNode.jointLink.recursionScalingFactor;
                    newGO.transform.localScale = currentBuildSegmentList[i].sourceNode.dimensions * newSegment.scalingFactor;
                    critterTotalVolume += newGO.transform.localScale.x * newGO.transform.localScale.y * newGO.transform.localScale.z;
                    newSegment.surfaceArea = new Vector3(newGO.transform.localScale.y * newGO.transform.localScale.z * 2f, newGO.transform.localScale.x * newGO.transform.localScale.z * 2f, newGO.transform.localScale.x * newGO.transform.localScale.y * 2f);
                    
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
                if (physicsOn) {
                    newGO.AddComponent<Rigidbody>().isKinematic = false;

                    //newGO.GetComponent<Rigidbody>().drag = 20f;
                    //newGO.GetComponent<Rigidbody>().angularDrag = 20f;
                    // Bouncy Root:
                    //GameObject anchorGO = new GameObject("Anchor");
                    //anchorGO.transform.SetParent(this.gameObject.transform);
                    //anchorGO.AddComponent<Rigidbody>().isKinematic = true;
                    /*ConfigurableJoint configJoint = newGO.AddComponent<ConfigurableJoint>();
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
                    */
                }
                // PositionSEGMENT GameObject!!!
                if (newSegment.id == 0) {  // if ROOT NODE:

                }
                else {
                    SetSegmentTransform(newGO);  // Properly position the SegmentGO where it should be  && scale!
                    critterTotalVolume += newGO.transform.localScale.x * newGO.transform.localScale.y * newGO.transform.localScale.z;
                    if (physicsOn) {
                        //newGO.GetComponent<Rigidbody>().isKinematic = false;
                        newGO.GetComponent<Rigidbody>().ResetInertiaTensor();
                        ConfigurableJoint configJoint = newGO.AddComponent<ConfigurableJoint>();
                        configJoint.autoConfigureConnectedAnchor = false;
                        configJoint.connectedBody = newSegment.parentSegment.gameObject.GetComponent<Rigidbody>();
                        configJoint.anchor = GetJointAnchor(newSegment);
                        configJoint.connectedAnchor = GetJointConnectedAnchor(newSegment); // <-- Might be Unnecessary
                        ConfigureJointSettings(newSegment, ref configJoint);  // UPDATE THIS TO USE segaddonJointMotorSettings!?!?
                    }
                }
                
                // SEGMENT ADDONS:
                if (physicsOn) {
                    // Check for Physical Attributes:
                    List<AddonPhysicalAttributes> physicalAttributesList = masterCritterGenome.CheckForAddonPhysicalAttributes(currentBuildSegmentList[i].sourceNode.ID);
                    for(int a = 0; a < physicalAttributesList.Count; a++) {
                        SegaddonPhysicalAttributes newPhysicalAttributes = new SegaddonPhysicalAttributes(physicalAttributesList[a]);
                        newPhysicalAttributes.segmentID = newSegment.id;
                        segaddonPhysicalAttributesList.Add(newPhysicalAttributes);
                        
                        segmentPhysicMaterial.dynamicFriction = newPhysicalAttributes.dynamicFriction[0];
                        segmentPhysicMaterial.staticFriction = newPhysicalAttributes.staticFriction[0];
                        segmentPhysicMaterial.bounciness = newPhysicalAttributes.bounciness[0];
                        RigidbodyConstraints rbConstraints = newGO.GetComponent<Rigidbody>().constraints;
                        if (newPhysicalAttributes.freezePositionX[0]) {
                            rbConstraints = rbConstraints | RigidbodyConstraints.FreezePositionX;
                        }
                        if (newPhysicalAttributes.freezePositionY[0]) {
                            rbConstraints = rbConstraints | RigidbodyConstraints.FreezePositionY;
                        }
                        if (newPhysicalAttributes.freezePositionZ[0]) {
                            rbConstraints = rbConstraints | RigidbodyConstraints.FreezePositionZ;
                        }
                        if (newPhysicalAttributes.freezeRotationX[0]) {
                            rbConstraints = rbConstraints | RigidbodyConstraints.FreezeRotationX;
                        }
                        if (newPhysicalAttributes.freezeRotationY[0]) {
                            rbConstraints = rbConstraints | RigidbodyConstraints.FreezeRotationY;
                        }
                        if (newPhysicalAttributes.freezeRotationZ[0]) {
                            rbConstraints = rbConstraints | RigidbodyConstraints.FreezeRotationZ;
                        }
                        newGO.GetComponent<Rigidbody>().constraints = rbConstraints;
                        newGO.GetComponent<BoxCollider>().material = segmentPhysicMaterial;
                    }

                    #region INPUTS
                    List<AddonJointAngleSensor> jointAngleSensorList = masterCritterGenome.CheckForAddonJointAngleSensor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < jointAngleSensorList.Count; j++) {
                        SegaddonJointAngleSensor newJointAngleSensor = new SegaddonJointAngleSensor(jointAngleSensorList[j]);
                        newJointAngleSensor.segmentID = newSegment.id;
                        segaddonJointAngleSensorList.Add(newJointAngleSensor);
                                                
                        if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                            string inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleX";
                            BrainInputChannel BIC_SegmentAngle = new BrainInputChannel(ref newJointAngleSensor.angleX, true, inputChannelName);
                            inputChannelsList.Add(BIC_SegmentAngle);

                            if (newJointAngleSensor.measureVel) {
                                inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleVelX";
                                BrainInputChannel BIC_SegmentAngleVel = new BrainInputChannel(ref newJointAngleSensor.angleVelX, true, inputChannelName);
                                inputChannelsList.Add(BIC_SegmentAngleVel);
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                            string inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleY";
                            BrainInputChannel BIC_SegmentAngle = new BrainInputChannel(ref newJointAngleSensor.angleY, true, inputChannelName);
                            inputChannelsList.Add(BIC_SegmentAngle);

                            if (newJointAngleSensor.measureVel) {
                                inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleVelY";
                                BrainInputChannel BIC_SegmentAngleVel = new BrainInputChannel(ref newJointAngleSensor.angleVelY, true, inputChannelName);
                                inputChannelsList.Add(BIC_SegmentAngleVel);
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                            string inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleZ";
                            BrainInputChannel BIC_SegmentAngle = new BrainInputChannel(ref newJointAngleSensor.angleZ, true, inputChannelName);
                            inputChannelsList.Add(BIC_SegmentAngle);

                            if (newJointAngleSensor.measureVel) {
                                inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleVelZ";
                                BrainInputChannel BIC_SegmentAngleVel = new BrainInputChannel(ref newJointAngleSensor.angleVelZ, true, inputChannelName);
                                inputChannelsList.Add(BIC_SegmentAngleVel);
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                            string inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleX";
                            BrainInputChannel BIC_SegmentAngleX = new BrainInputChannel(ref newJointAngleSensor.angleX, true, inputChannelName);
                            inputChannelsList.Add(BIC_SegmentAngleX);

                            inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleY";
                            BrainInputChannel BIC_SegmentAngleY = new BrainInputChannel(ref newJointAngleSensor.angleY, true, inputChannelName);
                            inputChannelsList.Add(BIC_SegmentAngleY);

                            if (newJointAngleSensor.measureVel) {
                                inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleVelX";
                                BrainInputChannel BIC_SegmentAngleVelX = new BrainInputChannel(ref newJointAngleSensor.angleVelX, true, inputChannelName);
                                inputChannelsList.Add(BIC_SegmentAngleVelX);

                                inputChannelName = "Segment " + newJointAngleSensor.segmentID.ToString() + " AngleVelY";
                                BrainInputChannel BIC_SegmentAngleVelY = new BrainInputChannel(ref newJointAngleSensor.angleVelY, true, inputChannelName);
                                inputChannelsList.Add(BIC_SegmentAngleVelY);
                            }
                        }
                    }
                    List<AddonContactSensor> contactSensorList = masterCritterGenome.CheckForAddonContactSensor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < contactSensorList.Count; j++) {
                        SegaddonContactSensor newContactSensor = new SegaddonContactSensor(contactSensorList[j]);
                        newContactSensor.segmentID = newSegment.id;
                        segaddonContactSensorList.Add(newContactSensor);
                        SegaddonCollisionDetector collisionDetector = newGO.AddComponent<SegaddonCollisionDetector>();
                        collisionDetector.referencedContactSensor = newContactSensor;

                        string inputChannelName = "Segment " + newContactSensor.segmentID.ToString() + " Contact Sensor";
                        BrainInputChannel BIC_SegmentContactInput = new BrainInputChannel(ref newContactSensor.contactStatus, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentContactInput);
                    }
                    List<AddonRaycastSensor> raycastSensorList = masterCritterGenome.CheckForAddonRaycastSensor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < raycastSensorList.Count; j++) {
                        SegaddonRaycastSensor newRaycastSensor = new SegaddonRaycastSensor(raycastSensorList[j]);
                        newRaycastSensor.segmentID = newSegment.id;
                        segaddonRaycastSensorList.Add(newRaycastSensor);

                        string inputChannelName = "Segment " + newRaycastSensor.segmentID.ToString() + " Raycast Sensor(dist)";
                        BrainInputChannel BIC_SegmentRaycastInput = new BrainInputChannel(ref newRaycastSensor.distance, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentRaycastInput);
                    }
                    List<AddonCompassSensor1D> compassSensor1DList = masterCritterGenome.CheckForAddonCompassSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < compassSensor1DList.Count; j++) {
                        SegaddonCompassSensor1D newCompassSensor1D = new SegaddonCompassSensor1D(compassSensor1DList[j]);
                        newCompassSensor1D.segmentID = newSegment.id;
                        segaddonCompassSensor1DList.Add(newCompassSensor1D);

                        string inputChannelName = "Segment " + newCompassSensor1D.segmentID.ToString() + " Compass1D";
                        BrainInputChannel BIC_SegmentCompass1DInput = new BrainInputChannel(ref newCompassSensor1D.dotProduct, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentCompass1DInput);
                    }
                    List<AddonCompassSensor3D> compassSensor3DList = masterCritterGenome.CheckForAddonCompassSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < compassSensor3DList.Count; j++) {
                        SegaddonCompassSensor3D newCompassSensor3D = new SegaddonCompassSensor3D(compassSensor3DList[j]);
                        newCompassSensor3D.segmentID = newSegment.id;
                        segaddonCompassSensor3DList.Add(newCompassSensor3D);

                        string inputChannelName = "Segment " + newCompassSensor3D.segmentID.ToString() + " Compass3D Right";
                        BrainInputChannel BIC_SegmentCompass3DInputRight = new BrainInputChannel(ref newCompassSensor3D.dotProductRight, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentCompass3DInputRight);

                        inputChannelName = "Segment " + newCompassSensor3D.segmentID.ToString() + " Compass3D Up";
                        BrainInputChannel BIC_SegmentCompass3DInputUp = new BrainInputChannel(ref newCompassSensor3D.dotProductUp, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentCompass3DInputUp);

                        inputChannelName = "Segment " + newCompassSensor3D.segmentID.ToString() + " Compass3D Forward";
                        BrainInputChannel BIC_SegmentCompass3DInputForward = new BrainInputChannel(ref newCompassSensor3D.dotProductForward, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentCompass3DInputForward);
                    }
                    List<AddonPositionSensor1D> positionSensor1DList = masterCritterGenome.CheckForAddonPositionSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < positionSensor1DList.Count; j++) {
                        SegaddonPositionSensor1D newPositionSensor1D = new SegaddonPositionSensor1D(positionSensor1DList[j]);
                        newPositionSensor1D.segmentID = newSegment.id;
                        segaddonPositionSensor1DList.Add(newPositionSensor1D);

                        string inputChannelName = "Segment " + newPositionSensor1D.segmentID.ToString() + " Position1D Forward";
                        BrainInputChannel BIC_SegmentPosition1DInput = new BrainInputChannel(ref newPositionSensor1D.linearDistance, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentPosition1DInput);
                    }
                    List<AddonPositionSensor3D> positionSensor3DList = masterCritterGenome.CheckForAddonPositionSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < positionSensor3DList.Count; j++) {
                        SegaddonPositionSensor3D newPositionSensor3D = new SegaddonPositionSensor3D(positionSensor3DList[j]);
                        newPositionSensor3D.segmentID = newSegment.id;
                        segaddonPositionSensor3DList.Add(newPositionSensor3D);

                        string inputChannelName = "Segment " + newPositionSensor3D.segmentID.ToString() + " Position3D Right";
                        BrainInputChannel BIC_SegmentPosition3DInputRight = new BrainInputChannel(ref newPositionSensor3D.distanceRight, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentPosition3DInputRight);

                        inputChannelName = "Segment " + newPositionSensor3D.segmentID.ToString() + " Position3D Up";
                        BrainInputChannel BIC_SegmentPosition3DInputUp = new BrainInputChannel(ref newPositionSensor3D.distanceUp, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentPosition3DInputUp);

                        inputChannelName = "Segment " + newPositionSensor3D.segmentID.ToString() + " Position3D Forward";
                        BrainInputChannel BIC_SegmentPosition3DInputForward = new BrainInputChannel(ref newPositionSensor3D.distanceForward, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentPosition3DInputForward);
                    }
                    List<AddonRotationSensor1D> rotationSensor1DList = masterCritterGenome.CheckForAddonRotationSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < rotationSensor1DList.Count; j++) {
                        SegaddonRotationSensor1D newRotationSensor1D = new SegaddonRotationSensor1D(rotationSensor1DList[j]);
                        newRotationSensor1D.segmentID = newSegment.id;
                        segaddonRotationSensor1DList.Add(newRotationSensor1D);

                        string inputChannelName = "Segment " + newRotationSensor1D.segmentID.ToString() + " Rotation1D";
                        BrainInputChannel BIC_SegmentRotation1DInput = new BrainInputChannel(ref newRotationSensor1D.rotationRate, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentRotation1DInput);
                    }
                    List<AddonRotationSensor3D> rotationSensor3DList = masterCritterGenome.CheckForAddonRotationSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < rotationSensor3DList.Count; j++) {
                        SegaddonRotationSensor3D newRotationSensor3D = new SegaddonRotationSensor3D(rotationSensor3DList[j]);
                        newRotationSensor3D.segmentID = newSegment.id;
                        segaddonRotationSensor3DList.Add(newRotationSensor3D);

                        string inputChannelName = "Segment " + newRotationSensor3D.segmentID.ToString() + " Rotation3D X";
                        BrainInputChannel BIC_SegmentRotation3DInputX = new BrainInputChannel(ref newRotationSensor3D.rotationRateX, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentRotation3DInputX);

                        inputChannelName = "Segment " + newRotationSensor3D.segmentID.ToString() + " Rotation3D Y";
                        BrainInputChannel BIC_SegmentRotation3DInputY = new BrainInputChannel(ref newRotationSensor3D.rotationRateY, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentRotation3DInputY);

                        inputChannelName = "Segment " + newRotationSensor3D.segmentID.ToString() + " Rotation3D Z";
                        BrainInputChannel BIC_SegmentRotation3DInputZ = new BrainInputChannel(ref newRotationSensor3D.rotationRateZ, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentRotation3DInputZ);
                    }
                    List<AddonVelocitySensor1D> velocitySensor1DList = masterCritterGenome.CheckForAddonVelocitySensor1D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < velocitySensor1DList.Count; j++) {
                        SegaddonVelocitySensor1D newVelocitySensor1D = new SegaddonVelocitySensor1D(velocitySensor1DList[j]);
                        newVelocitySensor1D.segmentID = newSegment.id;
                        segaddonVelocitySensor1DList.Add(newVelocitySensor1D);

                        string inputChannelName = "Segment " + newVelocitySensor1D.segmentID.ToString() + " Velocity1D";
                        BrainInputChannel BIC_SegmentVelocity1DInput = new BrainInputChannel(ref newVelocitySensor1D.componentVelocity, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentVelocity1DInput);
                    }
                    List<AddonVelocitySensor3D> velocitySensor3DList = masterCritterGenome.CheckForAddonVelocitySensor3D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < velocitySensor3DList.Count; j++) {
                        SegaddonVelocitySensor3D newVelocitySensor3D = new SegaddonVelocitySensor3D(velocitySensor3DList[j]);
                        newVelocitySensor3D.segmentID = newSegment.id;
                        segaddonVelocitySensor3DList.Add(newVelocitySensor3D);

                        string inputChannelName = "Segment " + newVelocitySensor3D.segmentID.ToString() + " Velocity3D Right";
                        BrainInputChannel BIC_SegmentVelocity3DInputRight = new BrainInputChannel(ref newVelocitySensor3D.velocityRight, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentVelocity3DInputRight);

                        inputChannelName = "Segment " + newVelocitySensor3D.segmentID.ToString() + " Velocity3D Up";
                        BrainInputChannel BIC_SegmentVelocity3DInputUp = new BrainInputChannel(ref newVelocitySensor3D.velocityUp, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentVelocity3DInputUp);

                        inputChannelName = "Segment " + newVelocitySensor3D.segmentID.ToString() + " Velocity3D Forward";
                        BrainInputChannel BIC_SegmentVelocity3DInputForward = new BrainInputChannel(ref newVelocitySensor3D.velocityForward, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentVelocity3DInputForward);
                    }
                    List<AddonAltimeter> altimeterList = masterCritterGenome.CheckForAddonAltimeter(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < altimeterList.Count; j++) {
                        SegaddonAltimeter newAltimeter = new SegaddonAltimeter(altimeterList[j]);
                        newAltimeter.segmentID = newSegment.id;
                        segaddonAltimeterList.Add(newAltimeter);

                        string inputChannelName = "Segment " + newAltimeter.segmentID.ToString() + " Altimeter";
                        BrainInputChannel BIC_SegmentAltimeterInput = new BrainInputChannel(ref newAltimeter.altitude, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentAltimeterInput);
                    }
                    List<AddonEarBasic> earBasicList = masterCritterGenome.CheckForAddonEarBasic(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < earBasicList.Count; j++) {
                        SegaddonEarBasic newEarBasic = new SegaddonEarBasic(earBasicList[j]);
                        newEarBasic.segmentID = newSegment.id;
                        segaddonEarBasicList.Add(newEarBasic);

                        string inputChannelName = "Segment " + newEarBasic.segmentID.ToString() + " Ear Basic";
                        BrainInputChannel BIC_SegmentEarBasicInput = new BrainInputChannel(ref newEarBasic.activation, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentEarBasicInput);
                    }
                    List<AddonGravitySensor> gravitySensorList = masterCritterGenome.CheckForAddonGravitySensor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < gravitySensorList.Count; j++) {
                        SegaddonGravitySensor newGravitySensor = new SegaddonGravitySensor(gravitySensorList[j]);
                        newGravitySensor.segmentID = newSegment.id;
                        segaddonGravitySensorList.Add(newGravitySensor);

                        string inputChannelName = "Segment " + newGravitySensor.segmentID.ToString() + " Gravity Sensor";
                        BrainInputChannel BIC_SegmentGravityInput = new BrainInputChannel(ref newGravitySensor.gravityDot, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentGravityInput);
                    }
                    List<AddonOscillatorInput> oscillatorInputList = masterCritterGenome.CheckForAddonOscillatorInput(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < oscillatorInputList.Count; j++) {
                        SegaddonOscillatorInput newOscillatorInput = new SegaddonOscillatorInput(oscillatorInputList[j]);
                        newOscillatorInput.segmentID = newSegment.id;
                        segaddonOscillatorInputList.Add(newOscillatorInput);

                        //SegaddonOscillatorInput oscillatorInput = critterBeingTested.segaddonOscillatorInputList[oscillatorInputIndex];
                        string inputChannelName = "Segment " + newOscillatorInput.segmentID.ToString() + " Oscillator Input";
                        BrainInputChannel BIC_SegmentOscillatorInput = new BrainInputChannel(ref newOscillatorInput.value, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentOscillatorInput);
                    }
                    List<AddonValueInput> valueInputList = masterCritterGenome.CheckForAddonValueInput(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < valueInputList.Count; j++) {
                        SegaddonValueInput newValueInput = new SegaddonValueInput(valueInputList[j]);
                        newValueInput.segmentID = newSegment.id;
                        segaddonValueInputList.Add(newValueInput);

                        string inputChannelName = "Segment " + newValueInput.segmentID.ToString() + " Value Input";
                        BrainInputChannel BIC_SegmentValueInput = new BrainInputChannel(ref newValueInput.value, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentValueInput);
                    }
                    List<AddonTimerInput> timerInputList = masterCritterGenome.CheckForAddonTimerInput(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < timerInputList.Count; j++) {
                        SegaddonTimerInput newTimerInput = new SegaddonTimerInput(timerInputList[j]);
                        newTimerInput.segmentID = newSegment.id;
                        segaddonTimerInputList.Add(newTimerInput);

                        string inputChannelName = "Segment " + newTimerInput.segmentID.ToString() + " Timer Input";
                        BrainInputChannel BIC_SegmentTimerInput = new BrainInputChannel(ref newTimerInput.value, true, inputChannelName);
                        inputChannelsList.Add(BIC_SegmentTimerInput);
                    }
                    #endregion

                    #region OUTPUTS:
                    List<AddonJointMotor> jointMotorList = masterCritterGenome.CheckForAddonJointMotor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < jointMotorList.Count; j++) {
                        SegaddonJointMotor newJointMotor = new SegaddonJointMotor(jointMotorList[j]);
                        newJointMotor.segmentID = newSegment.id;
                        segaddonJointMotorList.Add(newJointMotor);

                        //SegaddonJointMotor motor = critterBeingTested.segaddonJointMotorList[motorIndex];
                        if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                            string outputChannelName = "Segment " + newJointMotor.segmentID.ToString() + " Motor Target X";
                            BrainOutputChannel BOC_SegmentAngleVel = new BrainOutputChannel(ref newJointMotor.targetAngularX, true, outputChannelName);
                            outputChannelsList.Add(BOC_SegmentAngleVel);
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                            string outputChannelName = "Segment " + newJointMotor.segmentID.ToString() + " Motor Target Y";
                            BrainOutputChannel BOC_SegmentAngleVel = new BrainOutputChannel(ref newJointMotor.targetAngularY, true, outputChannelName);
                            outputChannelsList.Add(BOC_SegmentAngleVel);
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                            string outputChannelName = "Segment " + newJointMotor.segmentID.ToString() + " Motor Target Z";
                            BrainOutputChannel BOC_SegmentAngleVel = new BrainOutputChannel(ref newJointMotor.targetAngularZ, true, outputChannelName);
                            outputChannelsList.Add(BOC_SegmentAngleVel);
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                            string outputChannelName = "Segment " + newJointMotor.segmentID.ToString() + " Motor Target X";
                            BrainOutputChannel BOC_SegmentAngleVelX = new BrainOutputChannel(ref newJointMotor.targetAngularX, true, outputChannelName);
                            outputChannelsList.Add(BOC_SegmentAngleVelX);

                            outputChannelName = "Segment " + newJointMotor.segmentID.ToString() + " Motor Target Y";
                            BrainOutputChannel BOC_SegmentAngleVelY = new BrainOutputChannel(ref newJointMotor.targetAngularY, true, outputChannelName);
                            outputChannelsList.Add(BOC_SegmentAngleVelY);
                        }
                    }
                    List<AddonThrusterEffector1D> thrusterEffector1DList = masterCritterGenome.CheckForAddonThrusterEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < thrusterEffector1DList.Count; j++) {
                        SegaddonThrusterEffector1D newThrusterEffector1D = new SegaddonThrusterEffector1D(thrusterEffector1DList[j]);
                        newThrusterEffector1D.segmentID = newSegment.id;
                        segaddonThrusterEffector1DList.Add(newThrusterEffector1D);

                        string outputChannelName = "Segment " + newThrusterEffector1D.segmentID.ToString() + " Thruster1D";
                        BrainOutputChannel BOC_SegmentThruster1D = new BrainOutputChannel(ref newThrusterEffector1D.throttle, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentThruster1D);
                    }
                    List<AddonThrusterEffector3D> thrusterEffector3DList = masterCritterGenome.CheckForAddonThrusterEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < thrusterEffector3DList.Count; j++) {
                        SegaddonThrusterEffector3D newThrusterEffector3D = new SegaddonThrusterEffector3D(thrusterEffector3DList[j]);
                        newThrusterEffector3D.segmentID = newSegment.id;
                        segaddonThrusterEffector3DList.Add(newThrusterEffector3D);

                        string outputChannelName = "Segment " + newThrusterEffector3D.segmentID.ToString() + " Thruster3D X";
                        BrainOutputChannel BOC_SegmentThruster3DX = new BrainOutputChannel(ref newThrusterEffector3D.throttleX, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentThruster3DX);

                        outputChannelName = "Segment " + newThrusterEffector3D.segmentID.ToString() + " Thruster3D Y";
                        BrainOutputChannel BOC_SegmentThruster3DY = new BrainOutputChannel(ref newThrusterEffector3D.throttleY, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentThruster3DY);

                        outputChannelName = "Segment " + newThrusterEffector3D.segmentID.ToString() + " Thruster3D Z";
                        BrainOutputChannel BOC_SegmentThruster3DZ = new BrainOutputChannel(ref newThrusterEffector3D.throttleZ, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentThruster3DZ);
                    }
                    List<AddonTorqueEffector1D> torqueEffector1DList = masterCritterGenome.CheckForAddonTorqueEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < torqueEffector1DList.Count; j++) {
                        SegaddonTorqueEffector1D newTorqueEffector1D = new SegaddonTorqueEffector1D(torqueEffector1DList[j]);
                        newTorqueEffector1D.segmentID = newSegment.id;
                        segaddonTorqueEffector1DList.Add(newTorqueEffector1D);

                        string outputChannelName = "Segment " + newTorqueEffector1D.segmentID.ToString() + " Torque1D";
                        BrainOutputChannel BOC_SegmentTorque1D = new BrainOutputChannel(ref newTorqueEffector1D.throttle, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentTorque1D);
                    }
                    List<AddonTorqueEffector3D> torqueEffector3DList = masterCritterGenome.CheckForAddonTorqueEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < torqueEffector3DList.Count; j++) {
                        SegaddonTorqueEffector3D newTorqueEffector3D = new SegaddonTorqueEffector3D(torqueEffector3DList[j]);
                        newTorqueEffector3D.segmentID = newSegment.id;
                        segaddonTorqueEffector3DList.Add(newTorqueEffector3D);

                        string outputChannelName = "Segment " + newTorqueEffector3D.segmentID.ToString() + " Torque3D X";
                        BrainOutputChannel BOC_SegmentTorque3DX = new BrainOutputChannel(ref newTorqueEffector3D.throttleX, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentTorque3DX);

                        outputChannelName = "Segment " + newTorqueEffector3D.segmentID.ToString() + " Torque3D Y";
                        BrainOutputChannel BOC_SegmentTorque3DY = new BrainOutputChannel(ref newTorqueEffector3D.throttleY, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentTorque3DY);

                        outputChannelName = "Segment " + newTorqueEffector3D.segmentID.ToString() + " Torque3D Z";
                        BrainOutputChannel BOC_SegmentTorque3DZ = new BrainOutputChannel(ref newTorqueEffector3D.throttleZ, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentTorque3DZ);
                    }
                    List<AddonMouthBasic> mouthBasicList = masterCritterGenome.CheckForAddonMouthBasic(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < mouthBasicList.Count; j++) {
                        SegaddonMouthBasic newMouthBasic = new SegaddonMouthBasic(mouthBasicList[j]);
                        newMouthBasic.segmentID = newSegment.id;
                        segaddonMouthBasicList.Add(newMouthBasic);

                        SegaddonTriggerDetector triggerDetector = newGO.AddComponent<SegaddonTriggerDetector>();
                        triggerDetector.referencedMouth = newMouthBasic;
                    }
                    List<AddonNoiseMakerBasic> noiseMakerBasicList = masterCritterGenome.CheckForAddonNoiseMakerBasic(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < noiseMakerBasicList.Count; j++) {
                        SegaddonNoiseMakerBasic newNoiseMakerBasic = new SegaddonNoiseMakerBasic(noiseMakerBasicList[j]);
                        newNoiseMakerBasic.segmentID = newSegment.id;
                        segaddonNoiseMakerBasicList.Add(newNoiseMakerBasic);

                        string outputChannelName = "Segment " + newNoiseMakerBasic.segmentID.ToString() + " NoiseMaker";
                        BrainOutputChannel BOC_SegmentNoiseMaker = new BrainOutputChannel(ref newNoiseMakerBasic.volume, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentNoiseMaker);
                    }
                    List<AddonSticky> stickyList = masterCritterGenome.CheckForAddonSticky(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < stickyList.Count; j++) {
                        SegaddonSticky newSticky = new SegaddonSticky(stickyList[j]);
                        newSticky.segmentID = newSegment.id;
                        segaddonStickyList.Add(newSticky);

                        string outputChannelName = "Segment " + newSticky.segmentID.ToString() + " Sticky";
                        BrainOutputChannel BOC_SegmentSticky = new BrainOutputChannel(ref newSticky.stickiness, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentSticky);
                    }
                    List<AddonWeaponBasic> weaponBasicList = masterCritterGenome.CheckForAddonWeaponBasic(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < weaponBasicList.Count; j++) {
                        SegaddonWeaponBasic newWeaponBasic = new SegaddonWeaponBasic(weaponBasicList[j]);
                        newWeaponBasic.segmentID = newSegment.id;
                        segaddonWeaponBasicList.Add(newWeaponBasic);

                        string outputChannelName = "Segment " + newWeaponBasic.segmentID.ToString() + " Weapon";
                        BrainOutputChannel BOC_SegmentWeapon = new BrainOutputChannel(ref newWeaponBasic.strength, true, outputChannelName);
                        outputChannelsList.Add(BOC_SegmentWeapon);
                    }                    
                    #endregion
                    
                    
                }
                
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

        //Debug.Log("RebuildCritterFromGenomeRecursive " + inputChannelsList.Count.ToString() + ", " + outputChannelsList.Count.ToString());
        //Debug.Log("RebuildCritterFromGenomeRecursive: COM_Offset: (" + masterCritterGenome.centerOfMassOffset.x.ToString() + ", " + masterCritterGenome.centerOfMassOffset.y.ToString() + ", " + masterCritterGenome.centerOfMassOffset.z.ToString() + "), totalVolume: " + critterTotalVolume.ToString());
    }

    void ConfigureJointSettings(CritterSegment segment, ref ConfigurableJoint joint) {
        CritterNode node = segment.sourceNode;
        joint.breakForce = 1000000f;
        joint.breakTorque = 1000000f;
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
            limitXMin.bounciness = 0f;
            //limitXMin.contactDistance = 1f;
            limitXMin.limit = -node.jointLink.jointLimitPrimary;
            joint.lowAngularXLimit = limitXMin;
            SoftJointLimit limitXMax = joint.highAngularXLimit;
            limitXMax.bounciness = 0f;
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
            limitY.bounciness = 0f;
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
            limitZ.bounciness = 0f;
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
            limitXMin.bounciness = 0f;
            limitXMin.limit = -node.jointLink.jointLimitPrimary;
            joint.lowAngularXLimit = limitXMin;
            SoftJointLimit limitXMax = joint.highAngularXLimit;
            limitXMax.bounciness = 0f;
            limitXMax.limit = node.jointLink.jointLimitPrimary;
            joint.highAngularXLimit = limitXMax;
            SoftJointLimit limitYMax = joint.angularYLimit;
            limitYMax.bounciness = 0f;
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
    
    private void InitializeSegmentMaterial() {
        if (critterSegmentMaterial == null) {
            critterSegmentMaterial = new Material(Shader.Find("Custom/CritterSegmentBasic"));
            critterSegmentMaterial.renderQueue = 2000;
        }
        if(segmentPhysicMaterial == null) {
            segmentPhysicMaterial = new PhysicMaterial();
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
        //segmentPhysicMaterialList.Clear();
    }

    public void RebuildBranchFromGenome(CritterNode node) {  // resets and re-constructs only a branch of the Critter, starting with specified node

    }

    public void DeleteNode(CritterNode node) {
        int deletedID = node.ID; // id of the node being deleted 
        masterCritterGenome.DeleteNode(deletedID);  // handled within Genome
    }    

    public void RenumberNodes() {
        if(masterCritterGenome != null) {
            masterCritterGenome.RenumberNodes();            
        }
    }

    public void LoadCritterGenome(CritterGenome newGenome) {
        Debug.Log("LoadCritterGenome!!");
        masterCritterGenome = newGenome;
        masterCritterGenome.ReconstructGenomeFromLoad();
        RebuildCritterFromGenomeRecursive(false);
    }
}
