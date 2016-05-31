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
    public TransferFunctions.TransferFunction activationFunction = TransferFunctions.TransferFunction.RationalSigmoid;

	public GeneNodeNEAT() {

    }

    public GeneNodeNEAT(int id, GeneNodeType nodeType, TransferFunctions.TransferFunction function) {
        this.id = id;
        this.nodeType = nodeType;
        this.activationFunction = function;
    }
}
