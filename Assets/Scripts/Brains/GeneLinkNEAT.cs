using UnityEngine;
using System.Collections;

public class GeneLinkNEAT {

    public int fromNodeID;  // id of node from which this connection originates
    public int toNodeID;  // id of node to which this connection flows
    public float weight;  // multiplier on signal
    public bool enabled; 
    public int innov;  // innovation number
    public int birthGen = 0;

	public GeneLinkNEAT() {

    }

    public GeneLinkNEAT(int fromID, int toID, float weight, bool enabled, int inno, int gen) {
        fromNodeID = fromID;
        toNodeID = toID;
        this.weight = weight;
        this.enabled = enabled;
        innov = inno;
        birthGen = gen;
    }
}
