using UnityEngine;
using System.Collections;

public class CritterJointLink {

    public CritterNode childNode;
    public CritterNode parentNode;

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
        Debug.Log("CritterJointLink Constructor()!");
        jointType = JointType.HingeX;  // Default!
        symmetryType = SymmetryType.None;
    }

    public void MoveAttachCoords(Vector3 newAttachCoords) { // sets the position where this joint attaches to its parent

    }
}
