using UnityEngine;
using System.Collections;

public class SegaddonPositionSensor1D {

    public int segmentID;  // move to base class?
    public bool relative;
    public float sensitivity;
    public Vector3[] forwardVector;
    public Vector3[] targetPos;
    public float[] linearDistance;
    public float[] fitnessDistance;

    public SegaddonPositionSensor1D() {
        sensitivity = 1f;
        linearDistance = new float[1];
        linearDistance[0] = 0f;
        fitnessDistance = new float[1];
        fitnessDistance[0] = 0f;
        forwardVector = new Vector3[1];
        targetPos = new Vector3[1];
    }

    public SegaddonPositionSensor1D(AddonPositionSensor1D sourceNode) {
        sensitivity = sourceNode.sensitivity[0];
        linearDistance = new float[1];
        linearDistance[0] = 0f;
        fitnessDistance = new float[1];
        fitnessDistance[0] = 0f;
        forwardVector = new Vector3[1];
        forwardVector[0] = sourceNode.forwardVector[0];
        targetPos = new Vector3[1];
        relative = sourceNode.relative[0];
    }
}
