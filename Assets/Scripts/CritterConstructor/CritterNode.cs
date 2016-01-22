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
    private List<CritterJointLink> attachedJointLinkList;
    private CritterJointLink parentJointLink;

    public Vector3 dimensions = new Vector3(1f, 1f, 1f);

    public CritterNode() {
        Debug.Log("CritterNode Constructor()!");

        attachedJointLinkList = new List<CritterJointLink>();
        
    }

    public CritterNode(int i) {
        Debug.Log("CritterNode Constructor(int id " + i.ToString() + ")!");

        this.iD = i;
        attachedJointLinkList = new List<CritterJointLink>();

    }
}
