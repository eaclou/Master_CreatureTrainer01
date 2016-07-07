using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterGenome {

    public List<CritterNode> CritterNodeList;

    public List<AddonPhysicalAttributes> addonPhysicalAttributesList;
    
    public List<AddonJointAngleSensor> addonJointAngleSensorList;
    public List<AddonContactSensor> addonContactSensorList;
    public List<AddonRaycastSensor> addonRaycastSensorList;
    public List<AddonCompassSensor1D> addonCompassSensor1DList;  // Orientation relative to a target pos
    public List<AddonCompassSensor3D> addonCompassSensor3DList;
    public List<AddonPositionSensor1D> addonPositionSensor1DList;
    public List<AddonPositionSensor3D> addonPositionSensor3DList;
    public List<AddonRotationSensor1D> addonRotationSensor1DList;
    public List<AddonRotationSensor3D> addonRotationSensor3DList;
    public List<AddonVelocitySensor1D> addonVelocitySensor1DList;
    public List<AddonVelocitySensor3D> addonVelocitySensor3DList;
    public List<AddonAltimeter> addonAltimeterList;
    public List<AddonEarBasic> addonEarBasicList;
    public List<AddonGravitySensor> addonGravitySensorList;

    public List<AddonJointMotor> addonJointMotorList;    
    public List<AddonThrusterEffector1D> addonThrusterEffector1DList;
    public List<AddonThrusterEffector3D> addonThrusterEffector3DList;
    public List<AddonTorqueEffector1D> addonTorqueEffector1DList;
    public List<AddonTorqueEffector3D> addonTorqueEffector3DList;
    public List<AddonMouthBasic> addonMouthBasicList;
    public List<AddonNoiseMakerBasic> addonNoiseMakerBasicList;
    public List<AddonSticky> addonStickyList;
    public List<AddonWeaponBasic> addonWeaponBasicList;

    public List<AddonOscillatorInput> addonOscillatorInputList;
    public List<AddonValueInput> addonValueInputList;
    public List<AddonTimerInput> addonTimerInputList;

    public CritterGenome() {
        Debug.Log("CritterGenome Constructor()!");

        CritterNodeList = new List<CritterNode>();
        CritterNode critterRootNode = new CritterNode(0); // create root node
        CritterNodeList.Add(critterRootNode); // set it to first member of the list.

        addonPhysicalAttributesList = new List<AddonPhysicalAttributes>();
        
        addonJointAngleSensorList = new List<AddonJointAngleSensor>();
        addonContactSensorList = new List<AddonContactSensor>();
        addonRaycastSensorList = new List<AddonRaycastSensor>();
        addonCompassSensor1DList = new List<AddonCompassSensor1D>();
        addonCompassSensor3DList = new List<AddonCompassSensor3D>();
        addonPositionSensor1DList = new List<AddonPositionSensor1D>();
        addonPositionSensor3DList = new List<AddonPositionSensor3D>();
        addonRotationSensor1DList = new List<AddonRotationSensor1D>();
        addonRotationSensor3DList = new List<AddonRotationSensor3D>();
        addonVelocitySensor1DList = new List<AddonVelocitySensor1D>();
        addonVelocitySensor3DList = new List<AddonVelocitySensor3D>();
        addonAltimeterList = new List<AddonAltimeter>();
        addonEarBasicList = new List<AddonEarBasic>();
        addonGravitySensorList = new List<AddonGravitySensor>();

        addonJointMotorList = new List<AddonJointMotor>();
        addonThrusterEffector1DList = new List<AddonThrusterEffector1D>();
        addonThrusterEffector3DList = new List<AddonThrusterEffector3D>();
        addonTorqueEffector1DList = new List<AddonTorqueEffector1D>();
        addonTorqueEffector3DList = new List<AddonTorqueEffector3D>();
        addonMouthBasicList = new List<AddonMouthBasic>();
        addonNoiseMakerBasicList = new List<AddonNoiseMakerBasic>();
        addonStickyList = new List<AddonSticky>();
        addonWeaponBasicList = new List<AddonWeaponBasic>();

        addonOscillatorInputList = new List<AddonOscillatorInput>();
        addonValueInputList = new List<AddonValueInput>();
        addonTimerInputList = new List<AddonTimerInput>();
    }

    public void ResetToBlankGenome() {
        if(CritterNodeList == null) {
            CritterNodeList = new List<CritterNode>();
        }
        else {
            CritterNodeList.Clear();
        }
        CritterNode critterRootNode = new CritterNode(0); // create root node
        //critterRootNode.jointLink.parentNode = critterRootNode;
        CritterNodeList.Add(critterRootNode); // set it to first member of the list.
    }

    public void AddNewNode(CritterNode parentNode) {
        //Debug.Log("AddNewNode(CritterNode parentNode)");

    }
    public void AddNewNode(CritterNode parentNode, CritterJointLink jointLink) {
        //Debug.Log("AddNewNode(CritterNode parentNode, CritterJointLink jointLink)");

    }
    public void AddNewNode(CritterNode parentNode, Vector3 attachDir, int id) {        
        CritterNode newCritterNode = new CritterNode(id);
        newCritterNode.jointLink.parentNodeID = parentNode.ID;
        newCritterNode.jointLink.thisNodeID = newCritterNode.ID;
        newCritterNode.jointLink.numberOfRecursions = 0;
        Vector3 newSegmentDimensions = parentNode.dimensions;
        newCritterNode.dimensions = newSegmentDimensions;
        parentNode.attachedChildNodesIdList.Add(newCritterNode.ID);  // check this
        newCritterNode.jointLink.attachDir = attachDir.normalized;        
        CritterNodeList.Add(newCritterNode);
        //Debug.Log("AddNewNode(CritterNode parentNode = " + parentNode.ID.ToString() + ", Vector3 attachDir = " + newCritterNode.jointLink.attachDir.ToString() + ")");
    }
    public void AddNewNode(CritterNode parentNode, Vector3 attachDir, Vector3 restAngleDir, int id) {
        CritterNode newCritterNode = new CritterNode(id);
        newCritterNode.jointLink.parentNodeID = parentNode.ID;
        newCritterNode.jointLink.thisNodeID = newCritterNode.ID;
        newCritterNode.jointLink.numberOfRecursions = 0;
        Vector3 newSegmentDimensions = parentNode.dimensions;
        newCritterNode.dimensions = newSegmentDimensions;
        parentNode.attachedChildNodesIdList.Add(newCritterNode.ID);
        newCritterNode.jointLink.attachDir = attachDir.normalized;
        newCritterNode.jointLink.restAngleDir = restAngleDir.normalized;
        CritterNodeList.Add(newCritterNode);
        Debug.Log("AddNewNode(CritterNodeID: " + newCritterNode.ID.ToString() + ", parentNode = " + parentNode.ID.ToString() + ", attachDir = " + newCritterNode.jointLink.attachDir.ToString() + ")" + " restAngleDir: " + restAngleDir.ToString());
    }

    public void DeleteNode(CritterNode node) {  // Removes the specified node from the genome -- its orphan nodes are attached to the ParentNode of the deleted node.

    }

    public void DeleteBranch(CritterNode node) {  // Removes the specified nodes as well as all of its children / mirrors

    }

    public void RenumberNodes() {  // After deletion of a node, this cleans up the ID's of all critterNodes to ensure consecutive ID#s without any 'holes'

    }

    public void ReconstructGenomeFromLoad() {
        // EZSave had trouble with circular references, so need to repair genome to proper state and re-wire child references:
        // missing: (node)AttachedJointList, (joint)thisNode
        //Debug.Log("ReconstructGenomeFromLoad nodeCount: " + CritterNodeList.Count.ToString());
        for(int i = 0; i < CritterNodeList.Count; i++) {
            //Debug.Log("reconstruct! i: " + i.ToString() + ", CritterNodeList[i].id=" + CritterNodeList[i].ID.ToString());
            if (i == 0) {  // ROOT NODE
                // Fix ChildNode ref:
                //CritterNodeList[i].jointLink.thisNode = CritterNodeList[i];
                // attachedJointList will be populated through child nodes
            }
            else {
                // Fix ChildNode ref:
                //CritterNodeList[i].jointLink.thisNode = CritterNodeList[i];
                // attachedJointList:
                //if(CritterNodeList[i].jointLink.parentNode != null) {
                //    Debug.Log("Add to Child List!  parentID: " + CritterNodeList[i].jointLink.parentNode.ID.ToString() + ", thisID: " + CritterNodeList[i].ID.ToString());
                //    CritterNodeList[i].jointLink.parentNode.attachedJointLinkList.Add(CritterNodeList[i].jointLink);
                //}
                //else {
                //    Debug.Log("No Parent Node! # " + i.ToString());
                //}
            }
        }
    }

    public int[] CalculateNumberOfSegmentsInputsOutputs() {
        int numSegments = 0;
        int numInputs = 0;
        int numOutputs = 0;
        int[] retVal = new int[3];

        bool isPendingChildren = true;
        int currentDepth = 0; // start with RootNode
        int maxDepth = 20;  // safeguard to prevent while loop lock
        int nextSegmentID = 0;
        
        List<BuildSegmentInfo> currentBuildSegmentList = new List<BuildSegmentInfo>();  // keeps track of all current-depth segment build-requests, and holds important metadata
        List<BuildSegmentInfo> nextBuildSegmentList = new List<BuildSegmentInfo>();  // used to keep track of next childSegments that need to be built

        // ***********  Will attempt to traverse the Segments to be created, keeping track of where on each graph (nodes# & segment#) the current build is on.
        BuildSegmentInfo rootSegmentBuildInfo = new BuildSegmentInfo();
        rootSegmentBuildInfo.sourceNode = CritterNodeList[0];
        currentBuildSegmentList.Add(rootSegmentBuildInfo);  // ROOT NODE IS SPECIAL!
        
        // Do a Breadth-first traversal??
        while (isPendingChildren) {
            for (int i = 0; i < currentBuildSegmentList.Count; i++) { 
                numSegments++;
                nextSegmentID++;

                int contactSensorIndex = CheckForAddonContactSensor(currentBuildSegmentList[i].sourceNode.ID);
                if (contactSensorIndex != -1) {
                    numInputs++;
                }
                int raycastSensorIndex = CheckForAddonRaycastSensor(currentBuildSegmentList[i].sourceNode.ID);
                if (raycastSensorIndex != -1) {
                    numInputs++;
                }
                int compassSensor1DIndex = CheckForAddonCompassSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                if (compassSensor1DIndex != -1) {
                    numInputs++;
                }
                int compassSensor3DIndex = CheckForAddonCompassSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                if (compassSensor3DIndex != -1) {
                    numInputs = numInputs + 3;
                }
                int positionSensor1DIndex = CheckForAddonPositionSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                if (positionSensor1DIndex != -1) {
                    numInputs++;
                }
                int positionSensor3DIndex = CheckForAddonPositionSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                if (positionSensor3DIndex != -1) {
                    numInputs = numInputs + 4;
                }
                int rotationSensor1DIndex = CheckForAddonRotationSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                if (rotationSensor1DIndex != -1) {
                    numInputs++;
                }
                int rotationSensor3DIndex = CheckForAddonRotationSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                if (rotationSensor3DIndex != -1) {
                    numInputs = numInputs + 3;
                }
                int velocitySensor1DIndex = CheckForAddonVelocitySensor1D(currentBuildSegmentList[i].sourceNode.ID);
                if (velocitySensor1DIndex != -1) {
                    numInputs++;
                }
                int velocitySensor3DIndex = CheckForAddonVelocitySensor3D(currentBuildSegmentList[i].sourceNode.ID);
                if (velocitySensor3DIndex != -1) {
                    numInputs = numInputs + 0;
                }
                int altimeterIndex = CheckForAddonAltimeter(currentBuildSegmentList[i].sourceNode.ID);
                if (altimeterIndex != -1) {
                    numInputs++;
                }
                int earBasicIndex = CheckForAddonEarBasic(currentBuildSegmentList[i].sourceNode.ID);
                if (earBasicIndex != -1) {
                    numInputs++;
                }
                int gravitySensorIndex = CheckForAddonGravitySensor(currentBuildSegmentList[i].sourceNode.ID);
                if (gravitySensorIndex != -1) {
                    numInputs++;
                }

                int thrusterEffector1DIndex = CheckForAddonThrusterEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                if (thrusterEffector1DIndex != -1) {
                    numOutputs++;
                }
                int thrusterEffector3DIndex = CheckForAddonThrusterEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                if (thrusterEffector3DIndex != -1) {
                    numOutputs = numOutputs + 3;
                }
                int torqueEffector1DIndex = CheckForAddonTorqueEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                if (torqueEffector1DIndex != -1) {
                    numOutputs++;
                }
                int torqueEffector3DIndex = CheckForAddonTorqueEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                if (torqueEffector3DIndex != -1) {
                    numOutputs = numOutputs + 3;
                }
                
                int noiseMakerBasicIndex = CheckForAddonNoiseMakerBasic(currentBuildSegmentList[i].sourceNode.ID);
                if (noiseMakerBasicIndex != -1) {
                    numOutputs++;
                }
                int stickyIndex = CheckForAddonSticky(currentBuildSegmentList[i].sourceNode.ID);
                if (stickyIndex != -1) {
                    numOutputs++;
                }
                int weaponBasicIndex = CheckForAddonWeaponBasic(currentBuildSegmentList[i].sourceNode.ID);
                if (weaponBasicIndex != -1) {
                    numOutputs++;
                }

                int oscillatorInputIndex = CheckForAddonOscillatorInput(currentBuildSegmentList[i].sourceNode.ID);
                if (oscillatorInputIndex != -1) {
                    numInputs++;
                }
                int valueInputIndex = CheckForAddonValueInput(currentBuildSegmentList[i].sourceNode.ID);
                if (valueInputIndex != -1) {
                    numInputs++;
                }
                int timerInputIndex = CheckForAddonTimerInput(currentBuildSegmentList[i].sourceNode.ID);
                if (timerInputIndex != -1) {
                    numInputs++;
                }

                int mouthBasicIndex = CheckForAddonMouthBasic(currentBuildSegmentList[i].sourceNode.ID);
                if (mouthBasicIndex != -1) {
                    //numInputs++;
                    //numOutputs++;
                }

                if (currentBuildSegmentList[i].sourceNode.ID != 0) {  // is NOT ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check                                                                      
                    int jointAngleSensorIndex = CheckForAddonJointAngleSensor(currentBuildSegmentList[i].sourceNode.ID);
                    if(jointAngleSensorIndex != -1) {
                        if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                            numInputs = numInputs + 2;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                            numInputs++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                            numInputs++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                            numInputs = numInputs + 2;
                        }
                    }                    

                    int jointMotorIndex = CheckForAddonJointMotor(currentBuildSegmentList[i].sourceNode.ID);
                    if (jointMotorIndex != -1) { // if there is a jointMotor definition on this sourceNode
                        if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                            numOutputs++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                            numOutputs++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                            numOutputs++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                            numOutputs = numOutputs + 2;
                        }
                    }                    

                    // Check for if the segment currently being built is a Mirror COPY:
                    if (currentBuildSegmentList[i].isMirror) {
                        //Debug.Log("This is a MIRROR COPY segment - Wow!");
                        if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorX) {
                            // Invert the X-axis  (this will propagate down to all this segment's children
                            //newSegment.mirrorX = !newSegment.mirrorX;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorY) {
                            //newSegment.mirrorY = !newSegment.mirrorY;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorZ) {
                            //newSegment.mirrorZ = !newSegment.mirrorZ;
                        }
                    }
                }
                
                // CHECK FOR RECURSION:
                if (currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions > 0) { // if the node being considered has recursions:
                    if(currentBuildSegmentList[i].recursionNumber < currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions) {
                        // if the current buildOrder's recursion number is less than the numRecursions there should be:
                        BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                        newSegmentInfo.sourceNode = currentBuildSegmentList[i].sourceNode;  // request a segment to be built again based on the current sourceNode
                        newSegmentInfo.recursionNumber = currentBuildSegmentList[i].recursionNumber + 1;  // increment num recursions
                        nextBuildSegmentList.Add(newSegmentInfo);
                        //Debug.Log("newSegmentInfo recursion# " + newSegmentInfo.recursionNumber.ToString() + ", currentBuildList[i].recNum: " + currentBuildSegmentList[i].recursionNumber.ToString());
                        // If the node also has Symmetry:
                        if (newSegmentInfo.sourceNode.jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                            // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                            BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                            newSegmentInfoMirror.sourceNode = currentBuildSegmentList[i].sourceNode;  // uses same sourceNode, but tags as Mirror:
                            newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                            newSegmentInfoMirror.recursionNumber = newSegmentInfo.recursionNumber; //  !!!!!!!!!!!!! HIGHLY SUSPECT!!!!
                            
                            nextBuildSegmentList.Add(newSegmentInfoMirror);
                        }
                    }
                    else {  // at the end of a recursion chain, so do not add any other pendingChild builds

                    }
                }
                // Figure out how many unique Child nodes this built node has:
                int numberOfChildNodes = currentBuildSegmentList[i].sourceNode.attachedChildNodesIdList.Count;
                //Debug.Log("numberOfChildNodes: " + numberOfChildNodes.ToString() + "currentBuildSegmentList[i].sourceNode: " + currentBuildSegmentList[i].sourceNode.ID.ToString() + ", i: " + i.ToString());
                for (int c = 0; c < numberOfChildNodes; c++) {
                    // if NO symmetry:
                    // Check if Attaching to a recursion chain && if onlyattachToTail is active:
                    int childID = currentBuildSegmentList[i].sourceNode.attachedChildNodesIdList[c];
                    
                    if (CritterNodeList[childID].jointLink.onlyAttachToTailNode) {
                        if (currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions > 0) {
                            if (currentBuildSegmentList[i].recursionNumber >= currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions) {
                                // Only build segment if it is on the end of a recursion chain:
                                BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                                newSegmentInfo.sourceNode = CritterNodeList[childID];
                                //newSegmentInfo.parentSegment = newSegment;
                                nextBuildSegmentList.Add(newSegmentInfo);

                                if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                    // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                    BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                    newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                                    newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                    //newSegmentInfoMirror.parentSegment = newSegment;  // 
                                    nextBuildSegmentList.Add(newSegmentInfoMirror);
                                }
                            }
                        }
                        else {
                            // It only attaches to End nodes, but is parented to a Non-recursive segment, so proceed normally!!!
                            BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                            newSegmentInfo.sourceNode = CritterNodeList[childID];
                            //newSegmentInfo.parentSegment = newSegment;
                            nextBuildSegmentList.Add(newSegmentInfo);

                            if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                                newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                //newSegmentInfoMirror.parentSegment = newSegment;  // 
                                nextBuildSegmentList.Add(newSegmentInfoMirror);
                            }
                        }
                    }
                    else {  // proceed normally:
                        BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                        newSegmentInfo.sourceNode = CritterNodeList[childID];
                        //newSegmentInfo.parentSegment = newSegment;
                        nextBuildSegmentList.Add(newSegmentInfo);

                        if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                            // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                            BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                            newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                            newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                            //newSegmentInfoMirror.parentSegment = newSegment;  // 
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
        retVal[0] = numSegments;
        retVal[1] = numInputs;
        retVal[2] = numOutputs;
        return retVal;
    }


    public int CheckForAddonPhysicalAttributes(int nodeID) {
        int addonIndex = -1;
        for (int physicalAttributesIndex = 0; physicalAttributesIndex < addonPhysicalAttributesList.Count; physicalAttributesIndex++) {
            if (addonPhysicalAttributesList[physicalAttributesIndex].critterNodeID == nodeID) {
                addonIndex = physicalAttributesIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonJointAngleSensor(int nodeID) {
        int addonIndex = -1;  // -1 means this node does not contain a AngleSensor
        // JointAngleSensor:
        for (int jointAngleSensorIndex = 0; jointAngleSensorIndex < addonJointAngleSensorList.Count; jointAngleSensorIndex++) {
            //Debug.Log("AngleSensor Addon# " + jointAngleSensorIndex.ToString() + ", ");
            // Check if this node contains a AngleSensor Add-on:
            if (addonJointAngleSensorList[jointAngleSensorIndex].critterNodeID == nodeID) {
                addonIndex = jointAngleSensorIndex;
                //Debug.Log("FOUND AngleSensor! " + jointMotorIndex.ToString() + ", nodeID: " + nodeID.ToString());
            }
        }
        return addonIndex;
    }
    public int CheckForAddonContactSensor(int nodeID) {
        int addonIndex = -1;
        for (int contactSensorIndex = 0; contactSensorIndex < addonContactSensorList.Count; contactSensorIndex++) {
            if (addonContactSensorList[contactSensorIndex].critterNodeID == nodeID) {
                addonIndex = contactSensorIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonRaycastSensor(int nodeID) {
        int addonIndex = -1;
        for (int raycastSensorIndex = 0; raycastSensorIndex < addonRaycastSensorList.Count; raycastSensorIndex++) {
            if (addonRaycastSensorList[raycastSensorIndex].critterNodeID == nodeID) {
                addonIndex = raycastSensorIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonCompassSensor1D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonCompassSensor1DList.Count; i++) {
            if (addonCompassSensor1DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonCompassSensor3D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonCompassSensor3DList.Count; i++) {
            if (addonCompassSensor3DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonPositionSensor1D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonPositionSensor1DList.Count; i++) {
            if (addonPositionSensor1DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonPositionSensor3D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonPositionSensor3DList.Count; i++) {
            if (addonPositionSensor3DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonRotationSensor1D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonRotationSensor1DList.Count; i++) {
            if (addonRotationSensor1DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonRotationSensor3D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonRotationSensor3DList.Count; i++) {
            if (addonRotationSensor3DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonVelocitySensor1D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonVelocitySensor1DList.Count; i++) {
            if (addonVelocitySensor1DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonVelocitySensor3D(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonVelocitySensor3DList.Count; i++) {
            if (addonVelocitySensor3DList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonAltimeter(int nodeID) {
        int addonIndex = -1;
        for (int altimeterIndex = 0; altimeterIndex < addonAltimeterList.Count; altimeterIndex++) {
            if (addonAltimeterList[altimeterIndex].critterNodeID == nodeID) {
                addonIndex = altimeterIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonEarBasic(int nodeID) {
        int addonIndex = -1;
        for (int earBasicIndex = 0; earBasicIndex < addonEarBasicList.Count; earBasicIndex++) {
            if (addonEarBasicList[earBasicIndex].critterNodeID == nodeID) {
                addonIndex = earBasicIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonGravitySensor(int nodeID) {
        int addonIndex = -1;
        for (int gravitySensorIndex = 0; gravitySensorIndex < addonGravitySensorList.Count; gravitySensorIndex++) {
            if (addonGravitySensorList[gravitySensorIndex].critterNodeID == nodeID) {
                addonIndex = gravitySensorIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonJointMotor(int nodeID) {
        int addonIndex = -1;  // -1 means this node does not contain a jointMotor
        // Joint Motors:
        for (int jointMotorIndex = 0; jointMotorIndex < addonJointMotorList.Count; jointMotorIndex++) {
            //Debug.Log("jointMotor Addon# " + jointMotorIndex.ToString() + ", ");
            // Check if this node contains a jointMotor Add-on:
            if (addonJointMotorList[jointMotorIndex].critterNodeID == nodeID) {
                addonIndex = jointMotorIndex;
                //Debug.Log("FOUND JOINTMOTOR! " + jointMotorIndex.ToString() + ", nodeID: " + nodeID.ToString());
            }
        }
        return addonIndex;
    }
    public int CheckForAddonThrusterEffector1D(int nodeID) {
        int addonIndex = -1;
        for (int thrusterEffector1DIndex = 0; thrusterEffector1DIndex < addonThrusterEffector1DList.Count; thrusterEffector1DIndex++) {
            if (addonThrusterEffector1DList[thrusterEffector1DIndex].critterNodeID == nodeID) {
                addonIndex = thrusterEffector1DIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonThrusterEffector3D(int nodeID) {
        int addonIndex = -1;
        for (int thrusterEffector3DIndex = 0; thrusterEffector3DIndex < addonThrusterEffector3DList.Count; thrusterEffector3DIndex++) {
            if (addonThrusterEffector3DList[thrusterEffector3DIndex].critterNodeID == nodeID) {
                addonIndex = thrusterEffector3DIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonTorqueEffector1D(int nodeID) {
        int addonIndex = -1;
        for (int torqueEffector1DIndex = 0; torqueEffector1DIndex < addonTorqueEffector1DList.Count; torqueEffector1DIndex++) {
            if (addonTorqueEffector1DList[torqueEffector1DIndex].critterNodeID == nodeID) {
                addonIndex = torqueEffector1DIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonTorqueEffector3D(int nodeID) {
        int addonIndex = -1;
        for (int torqueEffector3DIndex = 0; torqueEffector3DIndex < addonTorqueEffector3DList.Count; torqueEffector3DIndex++) {
            if (addonTorqueEffector3DList[torqueEffector3DIndex].critterNodeID == nodeID) {
                addonIndex = torqueEffector3DIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonMouthBasic(int nodeID) {
        int addonIndex = -1;
        for (int mouthBasicIndex = 0; mouthBasicIndex < addonMouthBasicList.Count; mouthBasicIndex++) {
            if (addonMouthBasicList[mouthBasicIndex].critterNodeID == nodeID) {
                addonIndex = mouthBasicIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonNoiseMakerBasic(int nodeID) {
        int addonIndex = -1;
        for (int noiseMakerBasicIndex = 0; noiseMakerBasicIndex < addonNoiseMakerBasicList.Count; noiseMakerBasicIndex++) {
            if (addonNoiseMakerBasicList[noiseMakerBasicIndex].critterNodeID == nodeID) {
                addonIndex = noiseMakerBasicIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonSticky(int nodeID) {
        int addonIndex = -1;
        for (int stickyIndex = 0; stickyIndex < addonStickyList.Count; stickyIndex++) {
            if (addonStickyList[stickyIndex].critterNodeID == nodeID) {
                addonIndex = stickyIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonWeaponBasic(int nodeID) {
        int addonIndex = -1;
        for (int weaponBasicIndex = 0; weaponBasicIndex < addonWeaponBasicList.Count; weaponBasicIndex++) {
            if (addonWeaponBasicList[weaponBasicIndex].critterNodeID == nodeID) {
                addonIndex = weaponBasicIndex;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonOscillatorInput(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonOscillatorInputList.Count; i++) {
            if (addonOscillatorInputList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonValueInput(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonValueInputList.Count; i++) {
            if (addonValueInputList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
    public int CheckForAddonTimerInput(int nodeID) {
        int addonIndex = -1;
        for (int i = 0; i < addonTimerInputList.Count; i++) {
            if (addonTimerInputList[i].critterNodeID == nodeID) {
                addonIndex = i;
            }
        }
        return addonIndex;
    }
}
