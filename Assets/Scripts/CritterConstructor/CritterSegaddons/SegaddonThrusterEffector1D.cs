using UnityEngine;
using System.Collections;

public class SegaddonThrusterEffector1D {

    public int segmentID;  // move to base class?
    public Vector3[] forwardVector;
    public float[] maxForce;
    public float[] throttle;

    public SegaddonThrusterEffector1D() {
        forwardVector = new Vector3[1];
        maxForce = new float[1];
        maxForce[0] = 0f;
        throttle = new float[1];
        throttle[0] = 0f;
    }

    public SegaddonThrusterEffector1D(AddonThrusterEffector1D sourceNode) {

        forwardVector = new Vector3[1];
        forwardVector[0] = sourceNode.forwardVector[0];
        maxForce = new float[1];
        maxForce[0] = sourceNode.maxForce[0];
        throttle = new float[1];
        throttle[0] = 0f;
    }
}
