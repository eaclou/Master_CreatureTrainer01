using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterGenome {

    public List<CritterNode> CritterNodeList;

    public CritterGenome() {
        Debug.Log("CritterGenome Constructor()!");

        CritterNodeList = new List<CritterNode>();
        CritterNode critterRootNode = new CritterNode(0); // create root node
        CritterNodeList.Add(critterRootNode); // set it to first member of the list.
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
}
