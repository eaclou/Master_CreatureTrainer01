using UnityEngine;
using System.Collections;

public class SegaddonRotationSensor1D {

    public int segmentID;  // move to base class?
    public Vector3[] localAxis;
    public float sensitivity;
    public float[] rotationRate;
    public float[] fitnessRotationRate;

    public SegaddonRotationSensor1D() {
        sensitivity = 1f;
        rotationRate = new float[1];
        rotationRate[0] = 0f;
        fitnessRotationRate = new float[1];
        fitnessRotationRate[0] = 0f;
        localAxis = new Vector3[1];
    }

    public SegaddonRotationSensor1D(AddonRotationSensor1D sourceNode) {
        sensitivity = sourceNode.sensitivity[0];
        rotationRate = new float[1];
        rotationRate[0] = 0f;
        fitnessRotationRate = new float[1];
        fitnessRotationRate[0] = 0f;
        localAxis[0] = sourceNode.localAxis[0];
    }
}
