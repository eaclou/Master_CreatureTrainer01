using UnityEngine;
using System.Collections;

public class SegaddonVelocitySensor1D {

    public int segmentID;  // move to base class?
    public bool relative;
    public float sensitivity;
    public Vector3[] forwardVector;
    public Vector3[] targetVelocity;
    public float[] componentVelocity;
    public float[] fitnessComponentVelocity;

    public SegaddonVelocitySensor1D() {
        relative = true;
        sensitivity = 1f;
        componentVelocity = new float[1];
        componentVelocity[0] = 0f;
        fitnessComponentVelocity = new float[1];
        fitnessComponentVelocity[0] = 0f;
        forwardVector = new Vector3[1];
        targetVelocity = new Vector3[1];
    }

    public SegaddonVelocitySensor1D(AddonVelocitySensor1D sourceNode) {
        relative = sourceNode.relative[0];
        sensitivity = sourceNode.sensitivity[0];
        componentVelocity = new float[1];
        componentVelocity[0] = 0f;
        fitnessComponentVelocity = new float[1];
        fitnessComponentVelocity[0] = 0f;
        forwardVector = new Vector3[1];
        forwardVector[0] = sourceNode.forwardVector[0];
        targetVelocity = new Vector3[1];
    }
}
