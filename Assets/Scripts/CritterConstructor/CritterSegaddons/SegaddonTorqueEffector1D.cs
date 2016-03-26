using UnityEngine;
using System.Collections;

public class SegaddonTorqueEffector1D : MonoBehaviour {

    public int segmentID;  // move to base class?
    public Vector3[] axis;
    public float[] maxTorque;
    public float[] throttle;

    public SegaddonTorqueEffector1D() {
        axis = new Vector3[1];
        maxTorque = new float[1];
        maxTorque[0] = 0f;
        throttle = new float[1];
        throttle[0] = 0f;
    }

    public SegaddonTorqueEffector1D(AddonTorqueEffector1D sourceNode) {

        axis = new Vector3[1];
        axis[0] = sourceNode.axis[0];
        maxTorque = new float[1];
        maxTorque[0] = sourceNode.maxTorque[0];
        throttle = new float[1];
        throttle[0] = 0f;
    }
}
