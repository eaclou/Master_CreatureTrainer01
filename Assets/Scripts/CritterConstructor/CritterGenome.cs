using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterGenome {

    public List<CritterNode> CritterNodeList;
    public int savedNextNodeInno;
    public int savedNextAddonInno;

    public Vector3 centerOfMassOffset = new Vector3();

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

    public List<AddonOscillatorInput> addonOscillatorInputList;
    public List<AddonValueInput> addonValueInputList;
    public List<AddonTimerInput> addonTimerInputList;

    public List<AddonJointMotor> addonJointMotorList;    
    public List<AddonThrusterEffector1D> addonThrusterEffector1DList;
    public List<AddonThrusterEffector3D> addonThrusterEffector3DList;
    public List<AddonTorqueEffector1D> addonTorqueEffector1DList;
    public List<AddonTorqueEffector3D> addonTorqueEffector3DList;
    public List<AddonMouthBasic> addonMouthBasicList;
    public List<AddonNoiseMakerBasic> addonNoiseMakerBasicList;
    public List<AddonSticky> addonStickyList;
    public List<AddonWeaponBasic> addonWeaponBasicList;

    public bool degenerate = false;   

    public CritterGenome() {
        //Debug.Log("CritterGenome Constructor()!");

        CritterNodeList = new List<CritterNode>();
        CritterNode critterRootNode = new CritterNode(0, 0); // create root node  // Does Root node ALWAYS have innov=0?
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

        addonOscillatorInputList = new List<AddonOscillatorInput>();
        addonValueInputList = new List<AddonValueInput>();
        addonTimerInputList = new List<AddonTimerInput>();

        addonJointMotorList = new List<AddonJointMotor>();
        addonThrusterEffector1DList = new List<AddonThrusterEffector1D>();
        addonThrusterEffector3DList = new List<AddonThrusterEffector3D>();
        addonTorqueEffector1DList = new List<AddonTorqueEffector1D>();
        addonTorqueEffector3DList = new List<AddonTorqueEffector3D>();
        addonMouthBasicList = new List<AddonMouthBasic>();
        addonNoiseMakerBasicList = new List<AddonNoiseMakerBasic>();
        addonStickyList = new List<AddonSticky>();
        addonWeaponBasicList = new List<AddonWeaponBasic>();        
    }

    public void ResetToBlankGenome() {
        if(CritterNodeList == null) {
            CritterNodeList = new List<CritterNode>();
        }
        else {
            ClearAllAddons();
            CritterNodeList.Clear();
        }
        CritterNode critterRootNode = new CritterNode(0, 0); // create root node
        //critterRootNode.jointLink.parentNode = critterRootNode;
        CritterNodeList.Add(critterRootNode); // set it to first member of the list.
    }

    public void ClearAllAddons() {
        addonPhysicalAttributesList.Clear();

        addonJointAngleSensorList.Clear();
        addonContactSensorList.Clear();
        addonRaycastSensorList.Clear();
        addonCompassSensor1DList.Clear();
        addonCompassSensor3DList.Clear();
        addonPositionSensor1DList.Clear();
        addonPositionSensor3DList.Clear();
        addonRotationSensor1DList.Clear();
        addonRotationSensor3DList.Clear();
        addonVelocitySensor1DList.Clear();
        addonVelocitySensor3DList.Clear();
        addonAltimeterList.Clear();
        addonEarBasicList.Clear();
        addonGravitySensorList.Clear();

        addonOscillatorInputList.Clear();
        addonValueInputList.Clear();
        addonTimerInputList.Clear();

        addonJointMotorList.Clear();
        addonThrusterEffector1DList.Clear();
        addonThrusterEffector3DList.Clear();
        addonTorqueEffector1DList.Clear();
        addonTorqueEffector3DList.Clear();
        addonMouthBasicList.Clear();
        addonNoiseMakerBasicList.Clear();
        addonStickyList.Clear();
        addonWeaponBasicList.Clear();        
    }

    public void AddNewNode(CritterNode parentNode) {
        //Debug.Log("AddNewNode(CritterNode parentNode)");

    }
    public void AddNewNode(CritterNode parentNode, CritterJointLink jointLink) {
        //Debug.Log("AddNewNode(CritterNode parentNode, CritterJointLink jointLink)");

    }
    /*public void AddNewNode(CritterNode parentNode, Vector3 attachDir, int id) {        
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
    }*/
    public void AddNewNode(CritterNode parentNode, Vector3 attachDir, Vector3 restAngleDir, int id, int inno) {
        CritterNode newCritterNode = new CritterNode(id, inno);
        newCritterNode.jointLink.parentNodeID = parentNode.ID;
        //newCritterNode.jointLink.jointType = CritterJointLink.JointType. // Need to change this so the jointType can vary -- right now it's only HingeX!
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

    public void DeleteNode(int nodeID) {  // Removes the specified node from the genome -- its orphan nodes are attached to the ParentNode of the deleted node.
        CritterNode node = CritterNodeList[nodeID];  // readability
        CritterNode parentNode = CritterNodeList[node.jointLink.parentNodeID];  // get parent of node being deleted
        int childIndex = -1;
        // evalute parent of deleted node, go through its childList to find deleted Node ....        
        for (int i = 0; i < parentNode.attachedChildNodesIdList.Count; i++) {
            // go through parent node's children list and remove this node from it before deleting it from master list:
            if (parentNode.attachedChildNodesIdList[i] == nodeID) { // if the child in parent's List is the child being deleted:
                childIndex = i;   // save index of node being deleted so it can be removed after loop             
            }
        }
        // and remove it from childList
        parentNode.attachedChildNodesIdList.RemoveAt(childIndex); // remove here to avoid shortening length of list while traversing it

        // Attach children of deleted node to parentNode:
        for (int j = 0; j < node.attachedChildNodesIdList.Count; j++) { // go through deleted node's children
            // Set child of deleted-node's parentID  to the original parent of the deleted node:
            CritterNodeList[node.attachedChildNodesIdList[j]].jointLink.parentNodeID = parentNode.ID;
            // add the orphaned child ID's to the original parent of the deleted node:
            parentNode.attachedChildNodesIdList.Add(node.attachedChildNodesIdList[j]);
        }

        // ############# Delete all Addons of the deleted segment Here, before deleting segmentNode:
        DeleteAllAddonsOfNode(nodeID);
        // Remove node from master List
        CritterNodeList.RemoveAt(nodeID);
    }

    public void DeleteAllAddonsOfNode(int nodeID) {
        for(int i = 0; i < addonPhysicalAttributesList.Count; i++) {
            if(addonPhysicalAttributesList[i].critterNodeID == nodeID) {
                addonPhysicalAttributesList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonJointAngleSensorList.Count; i++) {
            if (addonJointAngleSensorList[i].critterNodeID == nodeID) {
                addonJointAngleSensorList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonContactSensorList.Count; i++) {
            if (addonContactSensorList[i].critterNodeID == nodeID) {
                addonContactSensorList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonRaycastSensorList.Count; i++) {
            if (addonRaycastSensorList[i].critterNodeID == nodeID) {
                addonRaycastSensorList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonCompassSensor1DList.Count; i++) {
            if (addonCompassSensor1DList[i].critterNodeID == nodeID) {
                addonCompassSensor1DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonCompassSensor3DList.Count; i++) {
            if (addonCompassSensor3DList[i].critterNodeID == nodeID) {
                addonCompassSensor3DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonPositionSensor1DList.Count; i++) {
            if (addonPositionSensor1DList[i].critterNodeID == nodeID) {
                addonPositionSensor1DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonPositionSensor3DList.Count; i++) {
            if (addonPositionSensor3DList[i].critterNodeID == nodeID) {
                addonPositionSensor3DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonRotationSensor1DList.Count; i++) {
            if (addonRotationSensor1DList[i].critterNodeID == nodeID) {
                addonRotationSensor1DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonRotationSensor3DList.Count; i++) {
            if (addonRotationSensor3DList[i].critterNodeID == nodeID) {
                addonRotationSensor3DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonVelocitySensor1DList.Count; i++) {
            if (addonVelocitySensor1DList[i].critterNodeID == nodeID) {
                addonVelocitySensor1DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonVelocitySensor3DList.Count; i++) {
            if (addonVelocitySensor3DList[i].critterNodeID == nodeID) {
                addonVelocitySensor3DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonAltimeterList.Count; i++) {
            if (addonAltimeterList[i].critterNodeID == nodeID) {
                addonAltimeterList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonEarBasicList.Count; i++) {
            if (addonEarBasicList[i].critterNodeID == nodeID) {
                addonEarBasicList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonGravitySensorList.Count; i++) {
            if (addonGravitySensorList[i].critterNodeID == nodeID) {
                addonGravitySensorList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonOscillatorInputList.Count; i++) {
            if (addonOscillatorInputList[i].critterNodeID == nodeID) {
                addonOscillatorInputList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonValueInputList.Count; i++) {
            if (addonValueInputList[i].critterNodeID == nodeID) {
                addonValueInputList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonTimerInputList.Count; i++) {
            if (addonTimerInputList[i].critterNodeID == nodeID) {
                addonTimerInputList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonJointMotorList.Count; i++) {
            if (addonJointMotorList[i].critterNodeID == nodeID) {
                addonJointMotorList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonThrusterEffector1DList.Count; i++) {
            if (addonThrusterEffector1DList[i].critterNodeID == nodeID) {
                addonThrusterEffector1DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonThrusterEffector3DList.Count; i++) {
            if (addonThrusterEffector3DList[i].critterNodeID == nodeID) {
                addonThrusterEffector3DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonTorqueEffector1DList.Count; i++) {
            if (addonTorqueEffector1DList[i].critterNodeID == nodeID) {
                addonTorqueEffector1DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonTorqueEffector3DList.Count; i++) {
            if (addonTorqueEffector3DList[i].critterNodeID == nodeID) {
                addonTorqueEffector3DList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonMouthBasicList.Count; i++) {
            if (addonMouthBasicList[i].critterNodeID == nodeID) {
                addonMouthBasicList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonNoiseMakerBasicList.Count; i++) {
            if (addonNoiseMakerBasicList[i].critterNodeID == nodeID) {
                addonNoiseMakerBasicList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonStickyList.Count; i++) {
            if (addonStickyList[i].critterNodeID == nodeID) {
                addonStickyList.RemoveAt(i);
            }
        }
        for (int i = 0; i < addonWeaponBasicList.Count; i++) {
            if (addonWeaponBasicList[i].critterNodeID == nodeID) {
                addonWeaponBasicList.RemoveAt(i);
            }
        }        
    }

    public void DeleteBranch(CritterNode node) {  // Removes the specified nodes as well as all of its children / mirrors

    }

    public void RenumberNodes() {  // After deletion of a node, this cleans up the ID's of all critterNodes to ensure consecutive ID#s without any 'holes'
                                   // traverse the main node List, if there is a gap in id's, iterate through all nodes/joints and adjust all ID's down by 1 until no gaps exist
        int checkingID = 0;
        int lastConsecutiveID = 0;
        for (int i = 0; i < CritterNodeList.Count; i++) {
            if (CritterNodeList[i].ID == checkingID) { // if node exists for each checkingID
                lastConsecutiveID = checkingID;
                // no need to renumber, node exists for this number
            }
            else {  //  nodeID didn't match checkingID - there is a gap!                 
                    // Iterate through all nodes:
                for (int j = 0; j < CritterNodeList.Count; j++) {  // make this less of a brute force algo later!!!                    

                    // check parentNodeID to see if it is above the current Gap ID:
                    if (CritterNodeList[j].jointLink.parentNodeID > checkingID) {
                        // if so, subtract 1 to fill in gap
                        CritterNodeList[j].jointLink.parentNodeID--;
                    }
                    if (CritterNodeList[j].jointLink.thisNodeID > checkingID) {    // check if this node's joint selfID > checkingID:                       
                        CritterNodeList[j].jointLink.thisNodeID--; // if so, subtract 1 to fill in gap
                    }
                    // check all child indices of this node:
                    for (int k = 0; k < CritterNodeList[j].attachedChildNodesIdList.Count; k++) {
                        // if childID is greater than GapID, subtract 1 to bring it in line:
                        if (CritterNodeList[j].attachedChildNodesIdList[k] > checkingID) {
                            CritterNodeList[j].attachedChildNodesIdList[k]--;
                        }
                    }
                    // if current Node's id is greater than gapID, subtract 1:
                    if (CritterNodeList[j].ID > checkingID) {
                        RenumberAllAddonsOfNode(CritterNodeList[j].ID);  // All add-ons with matching ID have their sourceNodeID decremented by 1 before the actual nodeId is decremented
                        CritterNodeList[j].RenumberNodeID(CritterNodeList[j].ID - 1);                        
                    }
                }
            }
            checkingID++;
        }
        //Debug.Log("genome# nodes: " + masterCritterGenome.CritterNodeList.Count.ToString());
    }

    public void RenumberAllAddonsOfNode(int nodeID) {
        for (int i = 0; i < addonPhysicalAttributesList.Count; i++) {
            if (addonPhysicalAttributesList[i].critterNodeID == nodeID) {
                addonPhysicalAttributesList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonJointAngleSensorList.Count; i++) {
            if (addonJointAngleSensorList[i].critterNodeID == nodeID) {
                addonJointAngleSensorList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonContactSensorList.Count; i++) {
            if (addonContactSensorList[i].critterNodeID == nodeID) {
                addonContactSensorList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonRaycastSensorList.Count; i++) {
            if (addonRaycastSensorList[i].critterNodeID == nodeID) {
                addonRaycastSensorList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonCompassSensor1DList.Count; i++) {
            if (addonCompassSensor1DList[i].critterNodeID == nodeID) {
                addonCompassSensor1DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonCompassSensor3DList.Count; i++) {
            if (addonCompassSensor3DList[i].critterNodeID == nodeID) {
                addonCompassSensor3DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonPositionSensor1DList.Count; i++) {
            if (addonPositionSensor1DList[i].critterNodeID == nodeID) {
                addonPositionSensor1DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonPositionSensor3DList.Count; i++) {
            if (addonPositionSensor3DList[i].critterNodeID == nodeID) {
                addonPositionSensor3DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonRotationSensor1DList.Count; i++) {
            if (addonRotationSensor1DList[i].critterNodeID == nodeID) {
                addonRotationSensor1DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonRotationSensor3DList.Count; i++) {
            if (addonRotationSensor3DList[i].critterNodeID == nodeID) {
                addonRotationSensor3DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonVelocitySensor1DList.Count; i++) {
            if (addonVelocitySensor1DList[i].critterNodeID == nodeID) {
                addonVelocitySensor1DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonVelocitySensor3DList.Count; i++) {
            if (addonVelocitySensor3DList[i].critterNodeID == nodeID) {
                addonVelocitySensor3DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonAltimeterList.Count; i++) {
            if (addonAltimeterList[i].critterNodeID == nodeID) {
                addonAltimeterList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonEarBasicList.Count; i++) {
            if (addonEarBasicList[i].critterNodeID == nodeID) {
                addonEarBasicList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonGravitySensorList.Count; i++) {
            if (addonGravitySensorList[i].critterNodeID == nodeID) {
                addonGravitySensorList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonOscillatorInputList.Count; i++) {
            if (addonOscillatorInputList[i].critterNodeID == nodeID) {
                addonOscillatorInputList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonValueInputList.Count; i++) {
            if (addonValueInputList[i].critterNodeID == nodeID) {
                addonValueInputList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonTimerInputList.Count; i++) {
            if (addonTimerInputList[i].critterNodeID == nodeID) {
                addonTimerInputList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonJointMotorList.Count; i++) {
            if (addonJointMotorList[i].critterNodeID == nodeID) {
                addonJointMotorList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonThrusterEffector1DList.Count; i++) {
            if (addonThrusterEffector1DList[i].critterNodeID == nodeID) {
                addonThrusterEffector1DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonThrusterEffector3DList.Count; i++) {
            if (addonThrusterEffector3DList[i].critterNodeID == nodeID) {
                addonThrusterEffector3DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonTorqueEffector1DList.Count; i++) {
            if (addonTorqueEffector1DList[i].critterNodeID == nodeID) {
                addonTorqueEffector1DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonTorqueEffector3DList.Count; i++) {
            if (addonTorqueEffector3DList[i].critterNodeID == nodeID) {
                addonTorqueEffector3DList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonMouthBasicList.Count; i++) {
            if (addonMouthBasicList[i].critterNodeID == nodeID) {
                addonMouthBasicList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonNoiseMakerBasicList.Count; i++) {
            if (addonNoiseMakerBasicList[i].critterNodeID == nodeID) {
                addonNoiseMakerBasicList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonStickyList.Count; i++) {
            if (addonStickyList[i].critterNodeID == nodeID) {
                addonStickyList[i].critterNodeID--;
            }
        }
        for (int i = 0; i < addonWeaponBasicList.Count; i++) {
            if (addonWeaponBasicList[i].critterNodeID == nodeID) {
                addonWeaponBasicList[i].critterNodeID--;
            }
        }        
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

    public List<GeneNodeNEAT> GetBlankBrainNodesFromBody() {
        List<GeneNodeNEAT> newNodeList = new List<GeneNodeNEAT>();

        // Split into Input/Output list and put together afterward?
        List<GeneNodeNEAT> inputNodeList = new List<GeneNodeNEAT>();
        List<GeneNodeNEAT> outputNodeList = new List<GeneNodeNEAT>();

        int numSegments = 0;
        int numInputs = 0;
        int numOutputs = 0;

        bool isPendingChildren = true;
        int currentDepth = 0; // start with RootNode
        int maxDepth = 20;  // safeguard to prevent while loop lock
        int nextSegmentID = 0;

        int currentBrainNodeID = 0;

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
                
                // PhysicalAttributes no inputs/outputs
                #region INPUTS
                // Joint Angle Sensors:
                if (currentBuildSegmentList[i].sourceNode.ID != 0) {  // is NOT ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check                                                                      
                    List<AddonJointAngleSensor> jointAngleSensorList = CheckForAddonJointAngleSensor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < jointAngleSensorList.Count; j++) {
                        // HINGE X X X X X
                        if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                            if (jointAngleSensorList[j].measureVel[0]) {  // ALSO MEASURES ANGULAR VELOCITY:
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT1);
                                currentBrainNodeID++;

                                numInputs++;
                                GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                                inputNodeList.Add(newNodeNEAT2);
                                currentBrainNodeID++;
                            }
                            else {  // Only Angle, no velocity:
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT);
                                currentBrainNodeID++;
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                            if (jointAngleSensorList[j].measureVel[0]) {
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT1);
                                currentBrainNodeID++;

                                numInputs++;
                                GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                                inputNodeList.Add(newNodeNEAT2);
                                currentBrainNodeID++;
                            }
                            else {
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT);
                                currentBrainNodeID++;
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                            if (jointAngleSensorList[j].measureVel[0]) {
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT1);
                                currentBrainNodeID++;

                                numInputs++;
                                GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                                inputNodeList.Add(newNodeNEAT2);
                                currentBrainNodeID++;
                            }
                            else {
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT);
                                currentBrainNodeID++;
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                            if (jointAngleSensorList[j].measureVel[0]) {
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT1);
                                currentBrainNodeID++;

                                numInputs++;
                                GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                                inputNodeList.Add(newNodeNEAT2);
                                currentBrainNodeID++;

                                numInputs++;
                                GeneNodeNEAT newNodeNEAT3 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 2);
                                inputNodeList.Add(newNodeNEAT3);
                                currentBrainNodeID++;

                                numInputs++;
                                GeneNodeNEAT newNodeNEAT4 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 3);
                                inputNodeList.Add(newNodeNEAT4);
                                currentBrainNodeID++;
                            }
                            else {
                                numInputs++;
                                GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                                inputNodeList.Add(newNodeNEAT1);
                                currentBrainNodeID++;

                                numInputs++;
                                GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, jointAngleSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                                inputNodeList.Add(newNodeNEAT2);
                                currentBrainNodeID++;
                            }
                        }
                    }
                }
                List<AddonContactSensor> contactSensorList = CheckForAddonContactSensor(currentBuildSegmentList[i].sourceNode.ID);
                for(int j = 0; j < contactSensorList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, contactSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }                
                List<AddonRaycastSensor> raycastSensorList = CheckForAddonRaycastSensor(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < raycastSensorList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, raycastSensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonCompassSensor1D> compassSensor1DList = CheckForAddonCompassSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < compassSensor1DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, compassSensor1DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonCompassSensor3D> compassSensor3DList = CheckForAddonCompassSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < compassSensor3DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, compassSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT1);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, compassSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                    inputNodeList.Add(newNodeNEAT2);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT3 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, compassSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 2);
                    inputNodeList.Add(newNodeNEAT3);
                    currentBrainNodeID++;
                }
                List<AddonPositionSensor1D> positionSensor1DList = CheckForAddonPositionSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < positionSensor1DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, positionSensor1DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonPositionSensor3D> positionSensor3DList = CheckForAddonPositionSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < positionSensor3DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, positionSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT1);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, positionSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                    inputNodeList.Add(newNodeNEAT2);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT3 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, positionSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 2);
                    inputNodeList.Add(newNodeNEAT3);
                    currentBrainNodeID++;
                }
                List<AddonRotationSensor1D> rotationSensor1DList = CheckForAddonRotationSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < rotationSensor1DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, rotationSensor1DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonRotationSensor3D> rotationSensor3DList = CheckForAddonRotationSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < rotationSensor3DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, rotationSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT1);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, rotationSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                    inputNodeList.Add(newNodeNEAT2);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT3 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, rotationSensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 2);
                    inputNodeList.Add(newNodeNEAT3);
                    currentBrainNodeID++;
                }
                List<AddonVelocitySensor1D> velocitySensor1DList = CheckForAddonVelocitySensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < velocitySensor1DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, velocitySensor1DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonVelocitySensor3D> velocitySensor3DList = CheckForAddonVelocitySensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < velocitySensor3DList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, velocitySensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT1);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, velocitySensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                    inputNodeList.Add(newNodeNEAT2);
                    currentBrainNodeID++;

                    numInputs++;
                    GeneNodeNEAT newNodeNEAT3 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, velocitySensor3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 2);
                    inputNodeList.Add(newNodeNEAT3);
                    currentBrainNodeID++;
                }
                List<AddonAltimeter> altimeterList = CheckForAddonAltimeter(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < altimeterList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, altimeterList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonEarBasic> earBasicList = CheckForAddonEarBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < earBasicList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, earBasicList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonGravitySensor> gravitySensorList = CheckForAddonGravitySensor(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < gravitySensorList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, gravitySensorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonOscillatorInput> oscillatorInputList = CheckForAddonOscillatorInput(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < oscillatorInputList.Count; j++) {
                    numInputs++;
                    //Debug.Log("Oscillator: nodeIndex: " + currentBrainNodeID.ToString() + ", " + new Int3(oscillatorInputList[j].innov, currentBuildSegmentList[i].recursionNumber, 0).ToString());
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, oscillatorInputList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;                    
                }
                List<AddonValueInput> valueInputList = CheckForAddonValueInput(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < valueInputList.Count; j++) {
                    numInputs++;
                    //Debug.Log("Value: nodeIndex: " + currentBrainNodeID.ToString() + ", " + new Int3(valueInputList[j].innov, currentBuildSegmentList[i].recursionNumber, 0).ToString());
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, valueInputList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;                    
                }
                //Debug.Log(currentBuildSegmentList[i].sourceNode.ID.ToString() + " oscillatorInputList Count: " + oscillatorInputList.Count.ToString() + ", valueInputList Count: " + valueInputList.Count.ToString());
                List<AddonTimerInput> timerInputList = CheckForAddonTimerInput(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < timerInputList.Count; j++) {
                    numInputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.In, TransferFunctions.TransferFunction.Linear, timerInputList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    inputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }                
                #endregion

                #region OUTPUTS:
                if (currentBuildSegmentList[i].sourceNode.ID != 0) {  // is NOT ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check
                    List<AddonJointMotor> jointMotorList = CheckForAddonJointMotor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < jointMotorList.Count; j++) {
                        if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                            numOutputs++;
                            GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, jointMotorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                            outputNodeList.Add(newNodeNEAT);
                            currentBrainNodeID++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                            numOutputs++;
                            GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, jointMotorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                            outputNodeList.Add(newNodeNEAT);
                            currentBrainNodeID++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                            numOutputs++;
                            GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, jointMotorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                            outputNodeList.Add(newNodeNEAT);
                            currentBrainNodeID++;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                            numOutputs++;
                            GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, jointMotorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                            outputNodeList.Add(newNodeNEAT1);
                            currentBrainNodeID++;

                            numOutputs++;
                            GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, jointMotorList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                            outputNodeList.Add(newNodeNEAT2);
                            currentBrainNodeID++;
                        }
                    }
                    // %%%%%% DO I STILL NEED THIS SECTION???
                    // Check for if the segment currently being built is a Mirror COPY:
                    /*if (currentBuildSegmentList[i].isMirror) {
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
                    }*/
                }

                List<AddonThrusterEffector1D> thrusterEffector1DList = CheckForAddonThrusterEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < thrusterEffector1DList.Count; j++) {
                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, thrusterEffector1DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    outputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonThrusterEffector3D> thrusterEffector3DList = CheckForAddonThrusterEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < thrusterEffector3DList.Count; j++) {
                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, thrusterEffector3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    outputNodeList.Add(newNodeNEAT1);
                    currentBrainNodeID++;

                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, thrusterEffector3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                    outputNodeList.Add(newNodeNEAT2);
                    currentBrainNodeID++;

                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT3 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, thrusterEffector3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 2);
                    outputNodeList.Add(newNodeNEAT3);
                    currentBrainNodeID++;
                }
                List<AddonTorqueEffector1D> torqueEffector1DList = CheckForAddonTorqueEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < torqueEffector1DList.Count; j++) {
                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, torqueEffector1DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    outputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonTorqueEffector3D> torqueEffector3DList = CheckForAddonTorqueEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < torqueEffector3DList.Count; j++) {
                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT1 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, torqueEffector3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    outputNodeList.Add(newNodeNEAT1);
                    currentBrainNodeID++;

                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT2 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, torqueEffector3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 1);
                    outputNodeList.Add(newNodeNEAT2);
                    currentBrainNodeID++;

                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT3 = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, torqueEffector3DList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 2);
                    outputNodeList.Add(newNodeNEAT3);
                    currentBrainNodeID++;
                }
                List<AddonMouthBasic> mouthBasicList = CheckForAddonMouthBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < mouthBasicList.Count; j++) {
                    // BLANK FOR NOW!!!
                }
                List<AddonNoiseMakerBasic> noiseMakerBasicList = CheckForAddonNoiseMakerBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < noiseMakerBasicList.Count; j++) {
                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, noiseMakerBasicList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    outputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonSticky> stickyList = CheckForAddonSticky(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < stickyList.Count; j++) {
                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, stickyList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    outputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                List<AddonWeaponBasic> weaponBasicList = CheckForAddonWeaponBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < weaponBasicList.Count; j++) {
                    numOutputs++;
                    GeneNodeNEAT newNodeNEAT = new GeneNodeNEAT(currentBrainNodeID, GeneNodeNEAT.GeneNodeType.Out, TransferFunctions.TransferFunction.RationalSigmoid, weaponBasicList[j].innov, currentBuildSegmentList[i].recursionNumber, currentBuildSegmentList[i].isMirror, 0);
                    outputNodeList.Add(newNodeNEAT);
                    currentBrainNodeID++;
                }
                #endregion
                
                // CHECK FOR RECURSION:
                if (currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions > 0) { // if the node being considered has recursions:
                    if (currentBuildSegmentList[i].recursionNumber < currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions) {
                        // if the current buildOrder's recursion number is less than the numRecursions there should be:
                        BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                        newSegmentInfo.sourceNode = currentBuildSegmentList[i].sourceNode;  // request a segment to be built again based on the current sourceNode
                        newSegmentInfo.recursionNumber = currentBuildSegmentList[i].recursionNumber + 1;  // increment num recursions
                        newSegmentInfo.isMirror = currentBuildSegmentList[i].isMirror;
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
                                newSegmentInfo.isMirror = currentBuildSegmentList[i].isMirror;
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
                            newSegmentInfo.isMirror = currentBuildSegmentList[i].isMirror;
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
                        newSegmentInfo.isMirror = currentBuildSegmentList[i].isMirror;
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

        for(int i = 0; i < inputNodeList.Count; i++) {
            newNodeList.Add(inputNodeList[i]);
        }
        for (int i = 0; i < outputNodeList.Count; i++) {
            newNodeList.Add(outputNodeList[i]);
        }
        for (int i = 0; i < newNodeList.Count; i++) {
            newNodeList[i].id = i;  // Re-number nodes so that their list Index is always the same as their ID
        }

        return newNodeList;
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

                #region INPUTS
                // PhysAttrs would be here, but it has no inputs/outputs
                if (currentBuildSegmentList[i].sourceNode.ID != 0) {  // is NOT ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check                                                                      
                    List<AddonJointAngleSensor> jointAngleSensorList = CheckForAddonJointAngleSensor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < jointAngleSensorList.Count; j++) {
                        // HINGE X X X X X
                        if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                            if (jointAngleSensorList[j].measureVel[0]) {  // ALSO MEASURES ANGULAR VELOCITY:
                                numInputs = numInputs + 2;
                            }
                            else {  // Only Angle, no velocity:
                                numInputs++;
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                            if (jointAngleSensorList[j].measureVel[0]) {
                                numInputs = numInputs + 2;
                            }
                            else {
                                numInputs++;
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                            if (jointAngleSensorList[j].measureVel[0]) {
                                numInputs = numInputs + 2;
                            }
                            else {
                                numInputs++;
                            }
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                            if (jointAngleSensorList[j].measureVel[0]) {
                                numInputs = numInputs + 4;
                            }
                            else {
                                numInputs = numInputs + 2;
                            }
                        }
                    }                    
                }
                List<AddonContactSensor> contactSensorList = CheckForAddonContactSensor(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < contactSensorList.Count; j++) {
                    numInputs++;
                }
                List<AddonRaycastSensor> raycastSensorList = CheckForAddonRaycastSensor(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < raycastSensorList.Count; j++) {
                    numInputs++;
                }
                List<AddonCompassSensor1D> compassSensor1DList = CheckForAddonCompassSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < compassSensor1DList.Count; j++) {
                    numInputs++;
                }
                List<AddonCompassSensor3D> compassSensor3DList = CheckForAddonCompassSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < compassSensor3DList.Count; j++) {
                    numInputs = numInputs + 3;
                }
                List<AddonPositionSensor1D> positionSensor1DList = CheckForAddonPositionSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < positionSensor1DList.Count; j++) {
                    numInputs++;
                }
                List<AddonPositionSensor3D> positionSensor3DList = CheckForAddonPositionSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < positionSensor3DList.Count; j++) {
                    numInputs = numInputs + 3;
                }
                List<AddonRotationSensor1D> rotationSensor1DList = CheckForAddonRotationSensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < rotationSensor1DList.Count; j++) {
                    numInputs++;
                }
                List<AddonRotationSensor3D> rotationSensor3DList = CheckForAddonRotationSensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < rotationSensor3DList.Count; j++) {
                    numInputs = numInputs + 3;
                }
                List<AddonVelocitySensor1D> velocitySensor1DList = CheckForAddonVelocitySensor1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < velocitySensor1DList.Count; j++) {
                    numInputs++;
                }
                List<AddonVelocitySensor3D> velocitySensor3DList = CheckForAddonVelocitySensor3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < velocitySensor3DList.Count; j++) {
                    numInputs = numInputs + 3;
                }
                List<AddonAltimeter> altimeterList = CheckForAddonAltimeter(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < altimeterList.Count; j++) {
                    numInputs++;
                }
                List<AddonEarBasic> earBasicList = CheckForAddonEarBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < earBasicList.Count; j++) {
                    numInputs++;
                }
                List<AddonGravitySensor> gravitySensorList = CheckForAddonGravitySensor(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < gravitySensorList.Count; j++) {
                    numInputs++;
                }
                List<AddonOscillatorInput> oscillatorInputList = CheckForAddonOscillatorInput(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < oscillatorInputList.Count; j++) {
                    numInputs++;
                }
                List<AddonValueInput> valueInputList = CheckForAddonValueInput(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < valueInputList.Count; j++) {
                    numInputs++;
                }
                List<AddonTimerInput> timerInputList = CheckForAddonTimerInput(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < timerInputList.Count; j++) {
                    numInputs++;
                }
                #endregion

                #region OUTPUTS:
                if (currentBuildSegmentList[i].sourceNode.ID != 0) {  // is NOT ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check
                    List<AddonJointMotor> jointMotorList = CheckForAddonJointMotor(currentBuildSegmentList[i].sourceNode.ID);
                    for (int j = 0; j < jointMotorList.Count; j++) {
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
                }
                List<AddonThrusterEffector1D> thrusterEffector1DList = CheckForAddonThrusterEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < thrusterEffector1DList.Count; j++) {
                    numOutputs++;
                }
                List<AddonThrusterEffector3D> thrusterEffector3DList = CheckForAddonThrusterEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < thrusterEffector3DList.Count; j++) {
                    numOutputs = numOutputs + 3;
                }
                List<AddonTorqueEffector1D> torqueEffector1DList = CheckForAddonTorqueEffector1D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < torqueEffector1DList.Count; j++) {
                    numOutputs++;
                }
                List<AddonTorqueEffector3D> torqueEffector3DList = CheckForAddonTorqueEffector3D(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < torqueEffector3DList.Count; j++) {
                    numOutputs = numOutputs + 3;
                }
                List<AddonMouthBasic> mouthBasicList = CheckForAddonMouthBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < mouthBasicList.Count; j++) {
                    // BLANK FOR NOW!!!
                }
                List<AddonNoiseMakerBasic> noiseMakerBasicList = CheckForAddonNoiseMakerBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < noiseMakerBasicList.Count; j++) {
                    numOutputs++;
                }
                List<AddonSticky> stickyList = CheckForAddonSticky(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < stickyList.Count; j++) {
                    numOutputs++;
                }
                List<AddonWeaponBasic> weaponBasicList = CheckForAddonWeaponBasic(currentBuildSegmentList[i].sourceNode.ID);
                for (int j = 0; j < weaponBasicList.Count; j++) {
                    numOutputs++;
                }
                #endregion
                
                
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
        newSegmentGO.transform.rotation = newSegmentRotation;
        newSegmentGO.transform.position = newSegmentPos;
        newSegmentGO.transform.localScale = newSegmentDimensions;
        //Debug.Log("attachPosWorld = " + attachPosWorld.ToString() + ", attachPosition = " + ") newSegmentPos = " + newSegmentPos.ToString());
        //Debug.Log("a= " + a.ToString() + ", normalDirection = " + normalDirection.ToString() + ") attachDirWorld= " + attachDirWorld.ToString() + ", parentPos " + newSegment.parentSegment.gameObject.transform.position.ToString());
    }

    public void PreBuildCritter(float variableMass) {
        //Debug.Log("RebuildCritterFromGenomeRecursive: " + masterCritterGenome.CritterNodeList.Count.ToString());

        // interpret Genome and construct critter in its bind pose
        bool isPendingChildren = true;
        int currentDepth = 0; // start with RootNode
        int maxDepth = 20;  // safeguard to prevent while loop lock
        int nextSegmentID = 0;
        List<CritterSegment> builtSegmentsList = new List<CritterSegment>();  // keep track of segments that have been built - linear in-order array 0-n segments
        List<BuildSegmentInfo> currentBuildSegmentList = new List<BuildSegmentInfo>();  // keeps track of all current-depth segment build-requests, and holds important metadata
        List<BuildSegmentInfo> nextBuildSegmentList = new List<BuildSegmentInfo>();  // used to keep track of next childSegments that need to be built
        float critterTotalVolume = 0f; // keep track of total volume

        // ***********  Will attempt to traverse the Segments to be created, keeping track of where on each graph (nodes# & segment#) the current build is on.
        BuildSegmentInfo rootSegmentBuildInfo = new BuildSegmentInfo();
        rootSegmentBuildInfo.sourceNode = CritterNodeList[0];
        currentBuildSegmentList.Add(rootSegmentBuildInfo);  // ROOT NODE IS SPECIAL!        

        // Do a Breadth-first traversal??
        #region First WhileLoop
        while (isPendingChildren) {
            //int numberOfChildNodes = masterCritterGenome.CritterNodeList[currentNode].attachedJointLinkList.Count;

            for (int i = 0; i < currentBuildSegmentList.Count; i++) {
                //Debug.Log("currentDepth: " + currentDepth.ToString() + "builtNodesQueue.Count: " + builtSegmentsList.Count.ToString() + ", pendingNodes: " + currentBuildSegmentList.Count.ToString() + ", i: " + i.ToString());
                // Iterate through pending nodes
                // Build current node --> Segment
                GameObject newGO = new GameObject("Node" + nextSegmentID.ToString());
                CritterSegment newSegment = newGO.AddComponent<CritterSegment>();
                builtSegmentsList.Add(newSegment);

                if(TempCritterConstructionGroup.tempCritterConstructionGroup != null) {
                    newGO.transform.SetParent(TempCritterConstructionGroup.tempCritterConstructionGroup.gameObject.transform);
                }
                
                //newGO.AddComponent<BoxCollider>().isTrigger = false;                
                //critterSegmentList.Add(newGO);  // Add to master Linear list of Segments
                //newSegment.InitGamePiece();  // create the mesh and some other initialization stuff
                newSegment.sourceNode = currentBuildSegmentList[i].sourceNode;
                newSegment.id = nextSegmentID;
                nextSegmentID++;

                if (currentBuildSegmentList[i].sourceNode.ID == 0) {  // is ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check
                    newGO.transform.position = Vector3.zero;
                    newGO.transform.localPosition = Vector3.zero;
                    newGO.transform.rotation = Quaternion.identity;
                    newSegment.scalingFactor = newSegment.sourceNode.jointLink.recursionScalingFactor;
                    newGO.transform.localScale = currentBuildSegmentList[i].sourceNode.dimensions * newSegment.scalingFactor;
                    //newSegment.surfaceArea = new Vector3(newGO.transform.localScale.y * newGO.transform.localScale.z * 2f, newGO.transform.localScale.x * newGO.transform.localScale.z * 2f, newGO.transform.localScale.x * newGO.transform.localScale.y * 2f);

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
                        if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorX) {
                            // Invert the X-axis  (this will propagate down to all this segment's children
                            newSegment.mirrorX = !newSegment.mirrorX;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorY) {
                            newSegment.mirrorY = !newSegment.mirrorY;
                        }
                        //else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorZ) {
                        //    newSegment.mirrorZ = !newSegment.mirrorZ;
                        //}
                    }
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
                    if (CritterNodeList[childID].jointLink.onlyAttachToTailNode) {
                        if (currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions > 0) {
                            if (newSegment.recursionNumber > newSegment.sourceNode.jointLink.numberOfRecursions) {
                                // Only build segment if it is on the end of a recursion chain:
                                BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                                newSegmentInfo.sourceNode = CritterNodeList[childID];
                                newSegmentInfo.parentSegment = newSegment;
                                nextBuildSegmentList.Add(newSegmentInfo);

                                if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                    // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                    BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                    newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                                    newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                    newSegmentInfoMirror.parentSegment = newSegment;  // 
                                    nextBuildSegmentList.Add(newSegmentInfoMirror);
                                }
                            }
                        }
                        else {
                            // It only attaches to End nodes, but is parented to a Non-recursive segment, so proceed normally!!!
                            BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                            newSegmentInfo.sourceNode = CritterNodeList[childID];
                            newSegmentInfo.parentSegment = newSegment;
                            nextBuildSegmentList.Add(newSegmentInfo);

                            if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                                newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                newSegmentInfoMirror.parentSegment = newSegment;  // 
                                nextBuildSegmentList.Add(newSegmentInfoMirror);
                            }
                        }
                    }
                    else {  // proceed normally:
                        BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                        newSegmentInfo.sourceNode = CritterNodeList[childID];
                        newSegmentInfo.parentSegment = newSegment;
                        nextBuildSegmentList.Add(newSegmentInfo);

                        if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                            // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                            BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                            newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                            newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                            newSegmentInfoMirror.parentSegment = newSegment;  // 
                            nextBuildSegmentList.Add(newSegmentInfoMirror);
                        }
                    }
                }

                // PositionSEGMENT GameObject!!!
                if (newSegment.id == 0) {  // if ROOT NODE:

                }
                else {
                    SetSegmentTransform(newGO);  // Properly position the SegmentGO where it should be  && scale!
                }
                critterTotalVolume += newGO.transform.localScale.x * newGO.transform.localScale.y * newGO.transform.localScale.z;
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
        #endregion
        //Debug.Log("RebuildCritterFromGenomeRecursive " + inputChannelsList.Count.ToString() + ", " + outputChannelsList.Count.ToString());

        // Re-Size Critter based on target volume:
        float targetTotalVolume = 4f;
        float linearScaling = Mathf.Pow(targetTotalVolume / critterTotalVolume, 1f / 3f);
        for (int i = 0; i < CritterNodeList.Count; i++) {
            CritterNodeList[i].dimensions.x *= linearScaling;
            CritterNodeList[i].dimensions.y *= linearScaling;
            CritterNodeList[i].dimensions.z *= linearScaling;
        }

        // interpret Genome and construct critter in its bind pose
        isPendingChildren = true;
        currentDepth = 0; // start with RootNode
        maxDepth = 20;  // safeguard to prevent while loop lock
        nextSegmentID = 0;
        builtSegmentsList.Clear();
        currentBuildSegmentList.Clear();
        nextBuildSegmentList.Clear();
        // ***********  Will attempt to traverse the Segments to be created, keeping track of where on each graph (nodes# & segment#) the current build is on.
        rootSegmentBuildInfo = new BuildSegmentInfo();
        rootSegmentBuildInfo.sourceNode = CritterNodeList[0];
        currentBuildSegmentList.Add(rootSegmentBuildInfo);  // ROOT NODE IS SPECIAL!        

        // Do a Breadth-first traversal??
        #region Second WhileLoop
        while (isPendingChildren) {
            //int numberOfChildNodes = masterCritterGenome.CritterNodeList[currentNode].attachedJointLinkList.Count;

            for (int i = 0; i < currentBuildSegmentList.Count; i++) {
                //Debug.Log("currentDepth: " + currentDepth.ToString() + "builtNodesQueue.Count: " + builtSegmentsList.Count.ToString() + ", pendingNodes: " + currentBuildSegmentList.Count.ToString() + ", i: " + i.ToString());
                // Iterate through pending nodes
                // Build current node --> Segment
                GameObject newGO = new GameObject("Node" + nextSegmentID.ToString());
                CritterSegment newSegment = newGO.AddComponent<CritterSegment>();
                builtSegmentsList.Add(newSegment);
                
                newGO.transform.SetParent(TempCritterConstructionGroup.tempCritterConstructionGroup.gameObject.transform);
                //newGO.AddComponent<BoxCollider>().isTrigger = false;                
                //critterSegmentList.Add(newGO);  // Add to master Linear list of Segments
                //newSegment.InitGamePiece();  // create the mesh and some other initialization stuff
                newSegment.sourceNode = currentBuildSegmentList[i].sourceNode;
                newSegment.id = nextSegmentID;
                nextSegmentID++;

                if (currentBuildSegmentList[i].sourceNode.ID == 0) {  // is ROOT segment  -- Look into doing Root build BEFORE for loop to avoid the need to do this check
                    newGO.transform.position = Vector3.zero;
                    newGO.transform.rotation = Quaternion.identity;
                    newSegment.scalingFactor = newSegment.sourceNode.jointLink.recursionScalingFactor;
                    newGO.transform.localScale = currentBuildSegmentList[i].sourceNode.dimensions * newSegment.scalingFactor;
                    //newSegment.surfaceArea = new Vector3(newGO.transform.localScale.y * newGO.transform.localScale.z * 2f, newGO.transform.localScale.x * newGO.transform.localScale.z * 2f, newGO.transform.localScale.x * newGO.transform.localScale.y * 2f);

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
                        if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorX) {
                            // Invert the X-axis  (this will propagate down to all this segment's children
                            newSegment.mirrorX = !newSegment.mirrorX;
                        }
                        else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorY) {
                            newSegment.mirrorY = !newSegment.mirrorY;
                        }
                        //else if (currentBuildSegmentList[i].sourceNode.jointLink.symmetryType == CritterJointLink.SymmetryType.MirrorZ) {
                        //    newSegment.mirrorZ = !newSegment.mirrorZ;
                        //}
                    }
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
                    if (CritterNodeList[childID].jointLink.onlyAttachToTailNode) {
                        if (currentBuildSegmentList[i].sourceNode.jointLink.numberOfRecursions > 0) {
                            if (newSegment.recursionNumber > newSegment.sourceNode.jointLink.numberOfRecursions) {
                                // Only build segment if it is on the end of a recursion chain:
                                BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                                newSegmentInfo.sourceNode = CritterNodeList[childID];
                                newSegmentInfo.parentSegment = newSegment;
                                nextBuildSegmentList.Add(newSegmentInfo);

                                if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                    // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                    BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                    newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                                    newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                    newSegmentInfoMirror.parentSegment = newSegment;  // 
                                    nextBuildSegmentList.Add(newSegmentInfoMirror);
                                }
                            }
                        }
                        else {
                            // It only attaches to End nodes, but is parented to a Non-recursive segment, so proceed normally!!!
                            BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                            newSegmentInfo.sourceNode = CritterNodeList[childID];
                            newSegmentInfo.parentSegment = newSegment;
                            nextBuildSegmentList.Add(newSegmentInfo);

                            if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                                // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                                BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                                newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                                newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                                newSegmentInfoMirror.parentSegment = newSegment;  // 
                                nextBuildSegmentList.Add(newSegmentInfoMirror);
                            }
                        }
                    }
                    else {  // proceed normally:
                        BuildSegmentInfo newSegmentInfo = new BuildSegmentInfo();
                        newSegmentInfo.sourceNode = CritterNodeList[childID];
                        newSegmentInfo.parentSegment = newSegment;
                        nextBuildSegmentList.Add(newSegmentInfo);

                        if (CritterNodeList[childID].jointLink.symmetryType != CritterJointLink.SymmetryType.None) {
                            // the child node has some type of symmetry, so add a buildOrder for a mirrored Segment:
                            BuildSegmentInfo newSegmentInfoMirror = new BuildSegmentInfo();
                            newSegmentInfoMirror.sourceNode = CritterNodeList[childID];
                            newSegmentInfoMirror.isMirror = true;  // This segment is the COPY, not the original
                            newSegmentInfoMirror.parentSegment = newSegment;  // 
                            nextBuildSegmentList.Add(newSegmentInfoMirror);
                        }
                    }
                }

                // PositionSEGMENT GameObject!!!
                if (newSegment.id == 0) {  // if ROOT NODE:

                }
                else {
                    SetSegmentTransform(newGO);  // Properly position the SegmentGO where it should be  && scale!                    
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
        #endregion
        //Debug.Log("RebuildCritterFromGenomeRecursive " + inputChannelsList.Count.ToString() + ", " + outputChannelsList.Count.ToString());

        centerOfMassOffset.x = 0f;
        centerOfMassOffset.y = 0f;
        centerOfMassOffset.z = 0f;
        Vector3 avgPos = new Vector3(0f, 0f, 0f);
        List<float> segmentMassesList = new List<float>();
        float critterTotalMass = 0f;        
        
        // Fill data arrays with Critter stats for CritterSEGMENTS:
        for (int i = 0; i < builtSegmentsList.Count; i++) {  // iterate through every segment            
            float segmentVolume = builtSegmentsList[i].transform.localScale.x * builtSegmentsList[i].transform.localScale.y * builtSegmentsList[i].transform.localScale.z;            
            float segmentMass = Mathf.Lerp(1f, segmentVolume, variableMass);
            segmentMassesList.Add(segmentMass);
            critterTotalMass += segmentMass;
        }
        float avgMass = critterTotalMass / builtSegmentsList.Count;  //
        string segmentPositions = "CritterPreBuild SegmentPositions: \n";
        critterTotalMass /= avgMass;  // Adjust to center around 1f;
        for (int j = 0; j < builtSegmentsList.Count; j++) {
            // Finalize Segment Mass Value:
            segmentMassesList[j] /= avgMass;  // Center the segment Masses around 1f, to avoid precision errors
            //critterBeingTested.critterSegmentList[j].GetComponent<Rigidbody>().mass = wormSegmentArray_Mass[j];
            float shareOfTotalMass = segmentMassesList[j] / critterTotalMass;
            avgPos.x += builtSegmentsList[j].transform.position.x * shareOfTotalMass;  // multiply position by proportional share of total mass
            avgPos.y += builtSegmentsList[j].transform.position.y * shareOfTotalMass;
            avgPos.z += builtSegmentsList[j].transform.position.z * shareOfTotalMass;
            segmentPositions += "(" + builtSegmentsList[j].transform.position.x.ToString() + ", " + builtSegmentsList[j].transform.position.y.ToString() + ", " + builtSegmentsList[j].transform.position.z.ToString() + ")\n";
        }        
        centerOfMassOffset.x = -avgPos.x;
        centerOfMassOffset.y = -avgPos.y;
        centerOfMassOffset.z = -avgPos.z;
        //Debug.Log(segmentPositions + "\nC.O.M: (" + avgPos.x.ToString() + ", " + avgPos.y.ToString() + ", " + avgPos.z.ToString() + ")");        
        // Delete created Segment GameObjects:
        // Do I need to delete Components first?
        TempCritterConstructionGroup.tempCritterConstructionGroup.DeleteSegments();
    }

    public List<AddonPhysicalAttributes> CheckForAddonPhysicalAttributes(int nodeID) {
        List<AddonPhysicalAttributes> addonList = new List<AddonPhysicalAttributes>();
        for (int physicalAttributesIndex = 0; physicalAttributesIndex < addonPhysicalAttributesList.Count; physicalAttributesIndex++) {
            if (addonPhysicalAttributesList[physicalAttributesIndex].critterNodeID == nodeID) {
                addonList.Add(addonPhysicalAttributesList[physicalAttributesIndex]);
            }
        }
        return addonList;
    }
    public List<AddonJointAngleSensor> CheckForAddonJointAngleSensor(int nodeID) {
        List<AddonJointAngleSensor> addonList = new List<AddonJointAngleSensor>();
        for (int i = 0; i < addonJointAngleSensorList.Count; i++) {
            if (addonJointAngleSensorList[i].critterNodeID == nodeID) {
                addonList.Add(addonJointAngleSensorList[i]);
            }
        }
        return addonList;
    }
    public List<AddonContactSensor> CheckForAddonContactSensor(int nodeID) {
        List<AddonContactSensor> addonList = new List<AddonContactSensor>();
        for (int contactSensorIndex = 0; contactSensorIndex < addonContactSensorList.Count; contactSensorIndex++) {
            if (addonContactSensorList[contactSensorIndex].critterNodeID == nodeID) {
                addonList.Add(addonContactSensorList[contactSensorIndex]);
            }
        }
        return addonList;
    }
    public List<AddonRaycastSensor> CheckForAddonRaycastSensor(int nodeID) {
        List<AddonRaycastSensor> addonList = new List<AddonRaycastSensor>();
        for (int i = 0; i < addonRaycastSensorList.Count; i++) {
            if (addonRaycastSensorList[i].critterNodeID == nodeID) {
                addonList.Add(addonRaycastSensorList[i]);
            }
        }
        return addonList;
    }
    public List<AddonCompassSensor1D> CheckForAddonCompassSensor1D(int nodeID) {
        List<AddonCompassSensor1D> addonList = new List<AddonCompassSensor1D>();
        for (int i = 0; i < addonCompassSensor1DList.Count; i++) {
            if (addonCompassSensor1DList[i].critterNodeID == nodeID) {
                addonList.Add(addonCompassSensor1DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonCompassSensor3D> CheckForAddonCompassSensor3D(int nodeID) {
        List<AddonCompassSensor3D> addonList = new List<AddonCompassSensor3D>();
        for (int i = 0; i < addonCompassSensor3DList.Count; i++) {
            if (addonCompassSensor3DList[i].critterNodeID == nodeID) {
                addonList.Add(addonCompassSensor3DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonPositionSensor1D> CheckForAddonPositionSensor1D(int nodeID) {
        List<AddonPositionSensor1D> addonList = new List<AddonPositionSensor1D>();
        for (int i = 0; i < addonPositionSensor1DList.Count; i++) {
            if (addonPositionSensor1DList[i].critterNodeID == nodeID) {
                addonList.Add(addonPositionSensor1DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonPositionSensor3D> CheckForAddonPositionSensor3D(int nodeID) {
        List<AddonPositionSensor3D> addonList = new List<AddonPositionSensor3D>();
        for (int i = 0; i < addonPositionSensor3DList.Count; i++) {
            if (addonPositionSensor3DList[i].critterNodeID == nodeID) {
                addonList.Add(addonPositionSensor3DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonRotationSensor1D> CheckForAddonRotationSensor1D(int nodeID) {
        List<AddonRotationSensor1D> addonList = new List<AddonRotationSensor1D>();
        for (int i = 0; i < addonRotationSensor1DList.Count; i++) {
            if (addonRotationSensor1DList[i].critterNodeID == nodeID) {
                addonList.Add(addonRotationSensor1DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonRotationSensor3D> CheckForAddonRotationSensor3D(int nodeID) {
        List<AddonRotationSensor3D> addonList = new List<AddonRotationSensor3D>();
        for (int i = 0; i < addonRotationSensor3DList.Count; i++) {
            if (addonRotationSensor3DList[i].critterNodeID == nodeID) {
                addonList.Add(addonRotationSensor3DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonVelocitySensor1D> CheckForAddonVelocitySensor1D(int nodeID) {
        List<AddonVelocitySensor1D> addonList = new List<AddonVelocitySensor1D>();
        for (int i = 0; i < addonVelocitySensor1DList.Count; i++) {
            if (addonVelocitySensor1DList[i].critterNodeID == nodeID) {
                addonList.Add(addonVelocitySensor1DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonVelocitySensor3D> CheckForAddonVelocitySensor3D(int nodeID) {
        List<AddonVelocitySensor3D> addonList = new List<AddonVelocitySensor3D>();
        for (int i = 0; i < addonVelocitySensor3DList.Count; i++) {
            if (addonVelocitySensor3DList[i].critterNodeID == nodeID) {
                addonList.Add(addonVelocitySensor3DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonAltimeter> CheckForAddonAltimeter(int nodeID) {
        List<AddonAltimeter> addonList = new List<AddonAltimeter>();
        for (int i = 0; i < addonAltimeterList.Count; i++) {
            if (addonAltimeterList[i].critterNodeID == nodeID) {
                addonList.Add(addonAltimeterList[i]);
            }
        }
        return addonList;
    }
    public List<AddonEarBasic> CheckForAddonEarBasic(int nodeID) {
        List<AddonEarBasic> addonList = new List<AddonEarBasic>();
        for (int i = 0; i < addonEarBasicList.Count; i++) {
            if (addonEarBasicList[i].critterNodeID == nodeID) {
                addonList.Add(addonEarBasicList[i]);
            }
        }
        return addonList;
    }
    public List<AddonGravitySensor> CheckForAddonGravitySensor(int nodeID) {
        List<AddonGravitySensor> addonList = new List<AddonGravitySensor>();
        for (int i = 0; i < addonGravitySensorList.Count; i++) {
            if (addonGravitySensorList[i].critterNodeID == nodeID) {
                addonList.Add(addonGravitySensorList[i]);
            }
        }
        return addonList;
    }
    public List<AddonOscillatorInput> CheckForAddonOscillatorInput(int nodeID) {
        List<AddonOscillatorInput> addonList = new List<AddonOscillatorInput>();
        for (int i = 0; i < addonOscillatorInputList.Count; i++) {
            if (addonOscillatorInputList[i].critterNodeID == nodeID) {
                addonList.Add(addonOscillatorInputList[i]);
            }
        }
        return addonList;
    }
    public List<AddonValueInput> CheckForAddonValueInput(int nodeID) {
        List<AddonValueInput> addonList = new List<AddonValueInput>();
        for (int i = 0; i < addonValueInputList.Count; i++) {
            if (addonValueInputList[i].critterNodeID == nodeID) {
                addonList.Add(addonValueInputList[i]);
            }
        }
        return addonList;
    }
    public List<AddonTimerInput> CheckForAddonTimerInput(int nodeID) {
        List<AddonTimerInput> addonList = new List<AddonTimerInput>();
        for (int i = 0; i < addonTimerInputList.Count; i++) {
            if (addonTimerInputList[i].critterNodeID == nodeID) {
                addonList.Add(addonTimerInputList[i]);
            }
        }
        return addonList;
    }
    public List<AddonJointMotor> CheckForAddonJointMotor(int nodeID) {
        List<AddonJointMotor> addonList = new List<AddonJointMotor>();
        for (int i = 0; i < addonJointMotorList.Count; i++) {
            if (addonJointMotorList[i].critterNodeID == nodeID) {
                addonList.Add(addonJointMotorList[i]);
            }
        }
        return addonList;
    }
    public List<AddonThrusterEffector1D> CheckForAddonThrusterEffector1D(int nodeID) {
        List<AddonThrusterEffector1D> addonList = new List<AddonThrusterEffector1D>();
        for (int i = 0; i < addonThrusterEffector1DList.Count; i++) {
            if (addonThrusterEffector1DList[i].critterNodeID == nodeID) {
                addonList.Add(addonThrusterEffector1DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonThrusterEffector3D> CheckForAddonThrusterEffector3D(int nodeID) {
        List<AddonThrusterEffector3D> addonList = new List<AddonThrusterEffector3D>();
        for (int i = 0; i < addonThrusterEffector3DList.Count; i++) {
            if (addonThrusterEffector3DList[i].critterNodeID == nodeID) {
                addonList.Add(addonThrusterEffector3DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonTorqueEffector1D> CheckForAddonTorqueEffector1D(int nodeID) {
        List<AddonTorqueEffector1D> addonList = new List<AddonTorqueEffector1D>();
        for (int i = 0; i < addonTorqueEffector1DList.Count; i++) {
            if (addonTorqueEffector1DList[i].critterNodeID == nodeID) {
                addonList.Add(addonTorqueEffector1DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonTorqueEffector3D> CheckForAddonTorqueEffector3D(int nodeID) {
        List<AddonTorqueEffector3D> addonList = new List<AddonTorqueEffector3D>();
        for (int i = 0; i < addonTorqueEffector3DList.Count; i++) {
            if (addonTorqueEffector3DList[i].critterNodeID == nodeID) {
                addonList.Add(addonTorqueEffector3DList[i]);
            }
        }
        return addonList;
    }
    public List<AddonMouthBasic> CheckForAddonMouthBasic(int nodeID) {
        List<AddonMouthBasic> addonList = new List<AddonMouthBasic>();
        for (int i = 0; i < addonMouthBasicList.Count; i++) {
            if (addonMouthBasicList[i].critterNodeID == nodeID) {
                addonList.Add(addonMouthBasicList[i]);
            }
        }
        return addonList;
    }
    public List<AddonNoiseMakerBasic> CheckForAddonNoiseMakerBasic(int nodeID) {
        List<AddonNoiseMakerBasic> addonList = new List<AddonNoiseMakerBasic>();
        for (int i = 0; i < addonNoiseMakerBasicList.Count; i++) {
            if (addonNoiseMakerBasicList[i].critterNodeID == nodeID) {
                addonList.Add(addonNoiseMakerBasicList[i]);
            }
        }
        return addonList;
    }
    public List<AddonSticky> CheckForAddonSticky(int nodeID) {
        List<AddonSticky> addonList = new List<AddonSticky>();
        for (int i = 0; i < addonStickyList.Count; i++) {
            if (addonStickyList[i].critterNodeID == nodeID) {
                addonList.Add(addonStickyList[i]);
            }
        }
        return addonList;
    }
    public List<AddonWeaponBasic> CheckForAddonWeaponBasic(int nodeID) {
        List<AddonWeaponBasic> addonList = new List<AddonWeaponBasic>();
        for (int i = 0; i < addonWeaponBasicList.Count; i++) {
            if (addonWeaponBasicList[i].critterNodeID == nodeID) {
                addonList.Add(addonWeaponBasicList[i]);
            }
        }
        return addonList;
    }    
}
