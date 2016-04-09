using UnityEngine;
using System.Collections;

public class ConnectionNEAT {

    public int fromNodeID;  // id of node from which this connection originates
    public int toNodeID;  // id of node to which this connection flows
    public float[] weight;  // multiplier on signal -- 1-member array to make a reference type  !#!$@#$!@$#! DOES this need to be an array?????

    //public bool enabled;
    //public int innov;  // innovation number

    public ConnectionNEAT() {

    }

    public ConnectionNEAT(int fromID, int toID, float initWeight) {
        weight = new float[1];
        weight[0] = initWeight;
        fromNodeID = fromID;
        toNodeID = toID;
    }
}
