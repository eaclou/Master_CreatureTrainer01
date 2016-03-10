using UnityEngine;
using System.Collections;

public class CritterJointLink {

    //public CritterNode thisNode;
    public int thisNodeID;
    //public CritterNode parentNode;
    public int parentNodeID = -1;  // defaults to -1 means root, other joints' parentID's should be set later

    public Vector3 attachDir;
    public Vector3 restAngleDir;
    public float jointLimitPrimary = 45f;
    public float jointLimitSecondary = 45f;
    public int numberOfRecursions = 0;
    public float recursionScalingFactor = 1f;
    public float recursionForward = 1f;  // might get rid of this?? only needed when using recursion and symmetry at the same time, which i might remove
    public bool onlyAttachToTailNode = true;
    //public int recursionInstances = 0;  // deprecated

    public JointType jointType;
    public enum JointType {
        Fixed,
        HingeX,
        HingeY,
        HingeZ,
        DualXY,
        DualYZ,
        DualXZ,
        Full
    };

    public SymmetryType symmetryType;
    public enum SymmetryType {
        None,
        MirrorX,
        MirrorY,
        MirrorZ
    }

    public CritterJointLink() {
        Debug.Log("CritterJointLink Constructor()! NO ID NO ID NO ID NO ID");
        jointType = JointType.HingeX;  // Default!
        symmetryType = SymmetryType.None;
    }
    public CritterJointLink(int thisID) {
        Debug.Log("CritterJointLink Constructor( " + thisID.ToString() + " )!");
        this.thisNodeID = thisID;
        jointType = JointType.HingeX;  // Default!
        symmetryType = SymmetryType.None;
    }
    /*public CritterJointLink(CritterNode thisNode) {
        Debug.Log("CritterJointLink Constructor()!");
        this.thisNode = thisNode;
        jointType = JointType.HingeX;  // Default!
        symmetryType = SymmetryType.None;
    }*/

    public void MoveAttachCoords(Vector3 newAttachCoords) { // sets the position where this joint attaches to its parent

    }
}
