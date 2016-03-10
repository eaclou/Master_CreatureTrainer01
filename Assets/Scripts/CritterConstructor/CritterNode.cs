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
    //public List<CritterJointLink> attachedJointLinkList;
    public List<int> attachedChildNodesIdList;
    public List<CritterNodeAddonBase> addonsList;
    
    public CritterJointLink jointLink;

    public Vector3 dimensions = new Vector3(1f, 1f, 1f);

    public CritterNode() {
        Debug.Log("CritterNode Constructor()!  NO ID NO ID NO ID NO ID");
        jointLink = new CritterJointLink();
        //attachedJointLinkList = new List<CritterJointLink>();
        attachedChildNodesIdList = new List<int>();
        addonsList = new List<CritterNodeAddonBase>();        
    }

    public CritterNode(int i) {
        Debug.Log("CritterNode Constructor(int id " + i.ToString() + ")!");

        this.iD = i;
        jointLink = new CritterJointLink(i);
        //attachedJointLinkList = new List<CritterJointLink>();
        attachedChildNodesIdList = new List<int>();
        addonsList = new List<CritterNodeAddonBase>();
    }

    public void ResizeNode(Vector3 newDimensions) {

    }

    public void RenumberNodeID(int newID) {
        iD = newID;
    }
}
