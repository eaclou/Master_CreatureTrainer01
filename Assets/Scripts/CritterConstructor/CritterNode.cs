using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CritterNode {

    private int iD;
    public int ID {        
        get {
            return iD;
        }
        set {
            RenumberNodeID(value);
        }
    }
    public int innov;
    //public List<CritterJointLink> attachedJointLinkList;
    public List<int> attachedChildNodesIdList;
    //public List<CritterNodeAddonBase> addonsList;  // Deprecated

    public CritterJointLink jointLink;

    public Vector3 dimensions = new Vector3(1f, 1f, 1f);

    public CritterNode() {
        //Debug.Log("CritterNode Constructor()!  NO ID NO ID NO ID NO ID");
        jointLink = new CritterJointLink();
        //attachedJointLinkList = new List<CritterJointLink>();
        attachedChildNodesIdList = new List<int>();
        //addonsList = new List<CritterNodeAddonBase>();     // Deprecated     
    }

    public CritterNode(int i, int inno) {
        //Debug.Log("CritterNode Constructor(int id " + i.ToString() + ")!");

        this.iD = i;
        this.innov = inno;
        jointLink = new CritterJointLink(i);
        //attachedJointLinkList = new List<CritterJointLink>();
        attachedChildNodesIdList = new List<int>();
        //addonsList = new List<CritterNodeAddonBase>();  // Deprecated
    }

    public void ResizeNode(Vector3 newDimensions) {

    }

    public void RenumberNodeID(int newID) {
        iD = newID;
    }

    public CritterNode CloneThisCritterNode() {
        CritterNode clonedCritterNode = new CritterNode(this.ID, this.innov);
        clonedCritterNode.dimensions = this.dimensions;
        clonedCritterNode.jointLink.CopySettingsFromJointLink(this.jointLink);
        for(int i = 0; i < this.attachedChildNodesIdList.Count; i++) {
            // populate clonedNode's attachedChild list based on this Node
            clonedCritterNode.attachedChildNodesIdList.Add(this.attachedChildNodesIdList[i]);
        }
        return clonedCritterNode;
    }

    public void CopySettingsFromNode(CritterNode sourceNode) {
        this.iD = sourceNode.ID;
        this.innov = sourceNode.innov;
        this.dimensions = sourceNode.dimensions;
        this.jointLink.CopySettingsFromJointLink(sourceNode.jointLink);
        for (int i = 0; i < sourceNode.attachedChildNodesIdList.Count; i++) {
            // populate clonedNode's attachedChild list based on this Node
            this.attachedChildNodesIdList.Add(sourceNode.attachedChildNodesIdList[i]);
        }
    }
}
