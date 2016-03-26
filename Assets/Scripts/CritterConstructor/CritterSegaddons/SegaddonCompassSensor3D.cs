using UnityEngine;
using System.Collections;

public class SegaddonCompassSensor3D {

    public int segmentID;
    //public Vector3[] forwardVector;
    public Vector3[] targetVector;
    public float[] dotProductRight;
    public float[] dotProductUp;
    public float[] dotProductForward;
    public float[] fitnessDotProductRight;
    public float[] fitnessDotProductUp;
    public float[] fitnessDotProductForward;

    public SegaddonCompassSensor3D() {
        dotProductRight = new float[1];
        dotProductRight[0] = 0f;
        dotProductUp = new float[1];
        dotProductUp[0] = 0f;
        dotProductForward = new float[1];
        dotProductForward[0] = 0f;
        fitnessDotProductRight = new float[1];
        fitnessDotProductRight[0] = 0f;
        fitnessDotProductUp = new float[1];
        fitnessDotProductUp[0] = 0f;
        fitnessDotProductForward = new float[1];
        fitnessDotProductForward[0] = 0f;
        //forwardVector = new Vector3[1];
        targetVector = new Vector3[1];
    }

    public SegaddonCompassSensor3D(AddonCompassSensor3D sourceNode) {
        dotProductRight = new float[1];
        dotProductRight[0] = 0f;
        dotProductUp = new float[1];
        dotProductUp[0] = 0f;
        dotProductForward = new float[1];
        dotProductForward[0] = 0f;
        fitnessDotProductRight = new float[1];
        fitnessDotProductRight[0] = 0f;
        fitnessDotProductUp = new float[1];
        fitnessDotProductUp[0] = 0f;
        fitnessDotProductForward = new float[1];
        fitnessDotProductForward[0] = 0f;
        //forwardVector = new Vector3[1];
        //forwardVector[0] = sourceNode.forwardVector[0];
        targetVector = new Vector3[1];
    }
}
