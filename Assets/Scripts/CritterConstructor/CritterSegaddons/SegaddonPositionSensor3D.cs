using UnityEngine;
using System.Collections;

public class SegaddonPositionSensor3D {

    public int segmentID;
    public bool relative;
    public float sensitivity;
    public Vector3[] targetPos;
    public float[] distanceRight;
    public float[] distanceUp;
    public float[] distanceForward;
    public float[] fitnessDistance;

    public SegaddonPositionSensor3D() {
        distanceRight = new float[1];
        distanceRight[0] = 0f;
        distanceUp = new float[1];
        distanceUp[0] = 0f;
        distanceForward = new float[1];
        distanceForward[0] = 0f;
        fitnessDistance = new float[1];
        fitnessDistance[0] = 0f;
        targetPos = new Vector3[1];
        sensitivity = 1f;
    }

    public SegaddonPositionSensor3D(AddonPositionSensor3D sourceNode) {
        sensitivity = sourceNode.sensitivity[0];
        distanceRight = new float[1];
        distanceRight[0] = 0f;
        distanceUp = new float[1];
        distanceUp[0] = 0f;
        distanceForward = new float[1];
        distanceForward[0] = 0f;
        fitnessDistance = new float[1];
        fitnessDistance[0] = 0f;
        targetPos = new Vector3[1];
    }
}
