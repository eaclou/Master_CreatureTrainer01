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

    public float[] distanceRightDouble;
    public float[] distanceUpDouble;
    public float[] distanceForwardDouble;

    public float[] distanceRightHalf;
    public float[] distanceUpHalf;
    public float[] distanceForwardHalf;

    public float[] fitnessDistance;
    public float[] invDistance;
    public float[] angle;

    public SegaddonPositionSensor3D() {
        distanceRight = new float[1];
        distanceRight[0] = 0f;
        distanceUp = new float[1];
        distanceUp[0] = 0f;
        distanceForward = new float[1];
        distanceForward[0] = 0f;
        fitnessDistance = new float[1];
        fitnessDistance[0] = 0f;
        invDistance = new float[1];
        invDistance[0] = 0f;
        angle = new float[1];
        angle[0] = 0f;
        targetPos = new Vector3[1];
        sensitivity = 1f;

        distanceRightDouble = new float[1];
        distanceRightDouble[0] = 0f;
        distanceUpDouble = new float[1];
        distanceUpDouble[0] = 0f;
        distanceForwardDouble = new float[1];
        distanceForwardDouble[0] = 0f;

        distanceRightHalf = new float[1];
        distanceRightHalf[0] = 0f;
        distanceUpHalf = new float[1];
        distanceUpHalf[0] = 0f;
        distanceForwardHalf = new float[1];
        distanceForwardHalf[0] = 0f;
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
        invDistance = new float[1];
        invDistance[0] = 0f;
        angle = new float[1];
        angle[0] = 0f;
        targetPos = new Vector3[1];
        relative = sourceNode.relative[0];

        distanceRightDouble = new float[1];
        distanceRightDouble[0] = 0f;
        distanceUpDouble = new float[1];
        distanceUpDouble[0] = 0f;
        distanceForwardDouble = new float[1];
        distanceForwardDouble[0] = 0f;

        distanceRightHalf = new float[1];
        distanceRightHalf[0] = 0f;
        distanceUpHalf = new float[1];
        distanceUpHalf[0] = 0f;
        distanceForwardHalf = new float[1];
        distanceForwardHalf[0] = 0f;
    }
}
