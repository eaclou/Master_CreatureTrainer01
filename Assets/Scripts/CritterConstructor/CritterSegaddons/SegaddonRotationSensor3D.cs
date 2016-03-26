using UnityEngine;
using System.Collections;

public class SegaddonRotationSensor3D {

    public int segmentID;
    public float sensitivity;
    public float[] rotationRateX;
    public float[] rotationRateY;
    public float[] rotationRateZ;
    public float[] fitnessRotationRate;

    public SegaddonRotationSensor3D() {
        sensitivity = 1f;
        rotationRateX = new float[1];
        rotationRateX[0] = 0f;
        rotationRateY = new float[1];
        rotationRateY[0] = 0f;
        rotationRateZ = new float[1];
        rotationRateZ[0] = 0f;
        fitnessRotationRate = new float[1];
        fitnessRotationRate[0] = 0f;
    }

    public SegaddonRotationSensor3D(AddonRotationSensor3D sourceNode) {
        sensitivity = sourceNode.sensitivity[0];
        rotationRateX = new float[1];
        rotationRateX[0] = 0f;
        rotationRateY = new float[1];
        rotationRateY[0] = 0f;
        rotationRateZ = new float[1];
        rotationRateZ[0] = 0f;
        fitnessRotationRate = new float[1];
        fitnessRotationRate[0] = 0f;
    }
}
