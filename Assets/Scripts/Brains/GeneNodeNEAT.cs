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
    public int sourceAddonInno;
    public int sourceAddonRecursionNum;
    public int sourceAddonChannelNum;  // if an addon has multiple Channels (i.e. compass3D -- keeps track of which is which)

	public GeneNodeNEAT() {

    }

    public GeneNodeNEAT(int id, GeneNodeType nodeType, TransferFunctions.TransferFunction function) {
        this.id = id;
        this.nodeType = nodeType;
        this.activationFunction = function;
    }

    public GeneNodeNEAT(int id, GeneNodeType nodeType, TransferFunctions.TransferFunction function, int inno, int recurse, bool mirror, int channelNum) {
        this.id = id;
        this.nodeType = nodeType;
        this.activationFunction = function;
        sourceAddonInno = inno;
        sourceAddonRecursionNum = recurse;
        if (mirror) {
            sourceAddonRecursionNum += 10;
        }
        sourceAddonChannelNum = channelNum;
    }
}
