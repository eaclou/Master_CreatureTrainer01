using UnityEngine;
using System.Collections;

public class SegaddonCompassSensor1D {

    public int segmentID;  // move to base class?
    public Vector3[] forwardVector;
    public Vector3[] targetVector;
    public float[] dotProduct;
    public float[] fitnessDotProduct;

    public SegaddonCompassSensor1D() {
        dotProduct = new float[1];
        dotProduct[0] = 0f;
        fitnessDotProduct = new float[1];
        fitnessDotProduct[0] = 0f;
        forwardVector = new Vector3[1];
        targetVector = new Vector3[1];
    }

    public SegaddonCompassSensor1D(AddonCompassSensor1D sourceNode) {
        dotProduct = new float[1];
        dotProduct[0] = 0f;
        fitnessDotProduct = new float[1];
        fitnessDotProduct[0] = 0f;
        forwardVector = new Vector3[1];
        forwardVector[0] = sourceNode.forwardVector[0];
        targetVector = new Vector3[1];
    }
}
