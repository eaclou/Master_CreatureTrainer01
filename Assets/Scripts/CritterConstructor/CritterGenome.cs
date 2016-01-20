using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterGenome {

    private List<CritterNode> CritterNodeList;

    public CritterGenome() {
        Debug.Log("CritterGenome Constructor()!");

        CritterNodeList = new List<CritterNode>();
        CritterNode critterRootNode = new CritterNode(0); // create root node
        CritterNodeList.Add(critterRootNode); // set it to first member of the list.
    }
}
