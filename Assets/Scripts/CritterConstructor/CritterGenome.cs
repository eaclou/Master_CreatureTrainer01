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
        CritterNodeList = new List<CritterNode>();
        CritterNode critterRootNode = new CritterNode(0); // create root node
        CritterNodeList.Add(critterRootNode); // set it to first member of the list.
    }

    public void AddNewNode(CritterNode parentNode) {
        Debug.Log("AddNewNode(CritterNode parentNode)");

    }
    public void AddNewNode(CritterNode parentNode, CritterJointLink jointLink) {
        Debug.Log("AddNewNode(CritterNode parentNode, CritterJointLink jointLink)");

    }
    public void AddNewNode(CritterNode parentNode, Vector3 attachCoords) {
        Debug.Log("AddNewNode(CritterNode parentNode = " + parentNode.ToString() + ", Vector3 attachCoords = " + attachCoords.ToString() + ")");
        CritterNode newCritterNode = new CritterNode();
        newCritterNode.parentJointLink.parentNode = parentNode;
        newCritterNode.parentJointLink.attachCoords = attachCoords;
        CritterNodeList.Add(newCritterNode);        
            
    }

    public void DeleteNode(CritterNode node) {  // Removes the specified node from the genome -- its orphan nodes are attached to the ParentNode of the deleted node.

    }

    public void DeleteBranch(CritterNode node) {  // Removes the specified nodes as well as all of its children / mirrors

    }

    public void RenumberNodes() {  // After deletion of a node, this cleans up the ID's of all critterNodes to ensure consecutive ID#s without any 'holes'

    }
}
