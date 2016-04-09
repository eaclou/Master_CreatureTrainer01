using UnityEngine;
using System.Collections;

public class GeneNodeNEAT {

    public GeneNodeType nodeType;
    public enum GeneNodeType {
        In,
        Hid,
        Out
    }
    public int id;

	public GeneNodeNEAT() {

    }

    public GeneNodeNEAT(int id, GeneNodeType nodeType) {
        this.id = id;
        this.nodeType = nodeType;
    }
}
