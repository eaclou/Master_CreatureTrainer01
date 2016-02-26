using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterNode {

    private int iD;
    public int ID {        
        get {
            return iD;
        }
        set { }
    }
    public List<CritterJointLink> attachedJointLinkList;
    public List<CritterNodeAddonBase> addonsList;
    public CritterJointLink parentJointLink;

    public Vector3 dimensions = new Vector3(1f, 1f, 1f);

    public CritterNode() {
        Debug.Log("CritterNode Constructor()!");
        parentJointLink = new CritterJointLink();
        attachedJointLinkList = new List<CritterJointLink>();
        addonsList = new List<CritterNodeAddonBase>();        
    }

    public CritterNode(int i) {
        Debug.Log("CritterNode Constructor(int id " + i.ToString() + ")!");

        this.iD = i;
        parentJointLink = new CritterJointLink();
        attachedJointLinkList = new List<CritterJointLink>();
        addonsList = new List<CritterNodeAddonBase>();
    }

    public void ResizeNode(Vector3 newDimensions) {

    }

    public void RenumberNodeID(int newID) {
        iD = newID;
    }


}
