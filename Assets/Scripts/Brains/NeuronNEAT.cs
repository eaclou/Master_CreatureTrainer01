using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuronNEAT {

    public GeneNodeNEAT.GeneNodeType nodeType;
    public int id;
    public float[] currentValue;
    public float previousValue;
    public List<ConnectionNEAT> incomingConnectionsList;  // used for more easily traversing the network forward
    public int segmentID;
    public Vector3 worldPos;

    // Constructor
    public NeuronNEAT() {

    }
    public NeuronNEAT(int nodeID, GeneNodeNEAT.GeneNodeType type) {
        id = nodeID;
        nodeType = type;
        currentValue = new float[1];
        incomingConnectionsList = new List<ConnectionNEAT>();
    }
}
