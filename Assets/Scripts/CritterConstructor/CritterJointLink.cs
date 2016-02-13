using UnityEngine;
using System.Collections;

public class CritterJointLink {

    public CritterNode childNode;
    public CritterNode parentNode;

    public Vector3 attachDir;
    public float jointLimitMaxTemp = 60f;

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

    public CritterJointLink() {
        Debug.Log("CritterJointLink Constructor()!");
        jointType = JointType.HingeX;
    }

    public void MoveAttachCoords(Vector3 newAttachCoords) { // sets the position where this joint attaches to its parent

    }
}
