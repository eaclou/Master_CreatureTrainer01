using UnityEngine;
using System.Collections;

public class SegaddonRaycastSensor {

    public int segmentID;  // move to base class?
    public Vector3[] forwardVector;
    public float[] maxDistance;
    public bool[] hit;
    public float[] distance;
    public float[] fitnessDistance;

    public SegaddonRaycastSensor() {
        forwardVector = new Vector3[1];
        maxDistance = new float[1];
        maxDistance[0] = 0f;
        hit = new bool[1];
        hit[0] = false;
        distance = new float[1];
        distance[0] = 0f;
        fitnessDistance = new float[1];
        fitnessDistance[0] = 0f;
    }

    public SegaddonRaycastSensor(AddonRaycastSensor sourceNode) {
        forwardVector = new Vector3[1];
        forwardVector[0] = sourceNode.forwardVector[0];
        maxDistance = new float[1];
        maxDistance[0] = sourceNode.maxDistance[0];
        hit = new bool[1];
        hit[0] = false;
        distance = new float[1];
        distance[0] = 0f;
        fitnessDistance = new float[1];
        fitnessDistance[0] = 0f;
    }
}
