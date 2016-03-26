using UnityEngine;
using System.Collections;

public class SegaddonVelocitySensor3D {

    public int segmentID;
    public bool relative;
    public float sensitivity;
    public Vector3[] targetVelocity;
    public float[] velocityRight;
    public float[] velocityUp;
    public float[] velocityForward;
    public float[] fitnessVelocityRight;
    public float[] fitnessVelocityUp;
    public float[] fitnessVelocityForward;

    public SegaddonVelocitySensor3D() {
        velocityRight = new float[1];
        velocityRight[0] = 0f;
        velocityUp = new float[1];
        velocityUp[0] = 0f;
        velocityForward = new float[1];
        velocityForward[0] = 0f;
        fitnessVelocityRight = new float[1];
        fitnessVelocityRight[0] = 0f;
        fitnessVelocityUp = new float[1];
        fitnessVelocityUp[0] = 0f;
        fitnessVelocityForward = new float[1];
        fitnessVelocityForward[0] = 0f;
        targetVelocity = new Vector3[1];
    }

    public SegaddonVelocitySensor3D(AddonVelocitySensor3D sourceNode) {
        relative = sourceNode.relative[0];
        sensitivity = sourceNode.sensitivity[0];
        velocityRight = new float[1];
        velocityRight[0] = 0f;
        velocityUp = new float[1];
        velocityUp[0] = 0f;
        velocityForward = new float[1];
        velocityForward[0] = 0f;
        fitnessVelocityRight = new float[1];
        fitnessVelocityRight[0] = 0f;
        fitnessVelocityUp = new float[1];
        fitnessVelocityUp[0] = 0f;
        fitnessVelocityForward = new float[1];
        fitnessVelocityForward[0] = 0f;
        targetVelocity = new Vector3[1];
    }
}
