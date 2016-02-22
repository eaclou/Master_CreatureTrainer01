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
        CritterNodeList.Add(critterRootNode); // set it to first member of the list.
    }

    public void AddNewNode(CritterNode parentNode) {
        Debug.Log("AddNewNode(CritterNode parentNode)");

    }
    public void AddNewNode(CritterNode parentNode, CritterJointLink jointLink) {
        Debug.Log("AddNewNode(CritterNode parentNode, CritterJointLink jointLink)");

    }
    public void AddNewNode(CritterNode parentNode, Vector3 attachDir, int id) {        
        CritterNode newCritterNode = new CritterNode(id);
        newCritterNode.parentJointLink.parentNode = parentNode;
        newCritterNode.parentJointLink.childNode = newCritterNode;
        newCritterNode.parentJointLink.numberOfRecursions = 0;
        Vector3 newSegmentDimensions = parentNode.dimensions;
        newCritterNode.dimensions = newSegmentDimensions;
        parentNode.attachedJointLinkList.Add(newCritterNode.parentJointLink);
        newCritterNode.parentJointLink.attachDir = attachDir.normalized;        
        CritterNodeList.Add(newCritterNode);
        Debug.Log("AddNewNode(CritterNode parentNode = " + parentNode.ID.ToString() + ", Vector3 attachDir = " + newCritterNode.parentJointLink.attachDir.ToString() + ")");
    }
    public void AddNewNode(CritterNode parentNode, Vector3 attachDir, Vector3 restAngleDir, int id) {
        CritterNode newCritterNode = new CritterNode(id);
        newCritterNode.parentJointLink.parentNode = parentNode;
        newCritterNode.parentJointLink.childNode = newCritterNode;
        newCritterNode.parentJointLink.numberOfRecursions = 0;
        Vector3 newSegmentDimensions = parentNode.dimensions;
        newCritterNode.dimensions = newSegmentDimensions;
        parentNode.attachedJointLinkList.Add(newCritterNode.parentJointLink);
        newCritterNode.parentJointLink.attachDir = attachDir.normalized;
        newCritterNode.parentJointLink.restAngleDir = restAngleDir.normalized;
        CritterNodeList.Add(newCritterNode);
        Debug.Log("AddNewNode(CritterNode parentNode = " + parentNode.ID.ToString() + ", attachDir = " + newCritterNode.parentJointLink.attachDir.ToString() + ")" + " restAngleDir: " + restAngleDir.ToString());
    }

    public void DeleteNode(CritterNode node) {  // Removes the specified node from the genome -- its orphan nodes are attached to the ParentNode of the deleted node.

    }

    public void DeleteBranch(CritterNode node) {  // Removes the specified nodes as well as all of its children / mirrors

    }

    public void RenumberNodes() {  // After deletion of a node, this cleans up the ID's of all critterNodes to ensure consecutive ID#s without any 'holes'

    }
}
