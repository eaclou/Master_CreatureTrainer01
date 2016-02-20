using UnityEngine;
using System.Collections;

public class CritterJointLink {

    public CritterNode childNode;
    public CritterNode parentNode;

    public Vector3 attachDir;
    public Vector3 restAngleDir;
    public float jointLimitMaxTemp = 60f;
    public int numberOfRecursions = 0;
    public float recursionScalingFactor = 0.8f;
    public float recursionForward = 1.0f;
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
