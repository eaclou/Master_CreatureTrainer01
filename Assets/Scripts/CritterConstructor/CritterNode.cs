using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterNode {

    private List<CritterJointLink> attachedJointLinkList;
    private CritterJointLink parentJointLink;

    public CritterNode() {
        Debug.Log("CritterNode Constructor()!");
    }
}
