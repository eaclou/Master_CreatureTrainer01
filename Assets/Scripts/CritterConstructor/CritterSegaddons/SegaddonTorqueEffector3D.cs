using UnityEngine;
using System.Collections;

public class SegaddonTorqueEffector3D {

    public int segmentID;  // move to base class?
    public float[] maxTorque;
    public float[] throttleX;
    public float[] throttleY;
    public float[] throttleZ;

    public SegaddonTorqueEffector3D() {
        maxTorque = new float[1];
        maxTorque[0] = 0f;
        throttleX = new float[1];
        throttleX[0] = 0f;
        throttleY = new float[1];
        throttleY[0] = 0f;
        throttleZ = new float[1];
        throttleZ[0] = 0f;
    }

    public SegaddonTorqueEffector3D(AddonTorqueEffector3D sourceNode) {
        maxTorque = new float[1];
        maxTorque[0] = sourceNode.maxTorque[0];
        throttleX = new float[1];
        throttleX[0] = 0f;
        throttleY = new float[1];
        throttleY[0] = 0f;
        throttleZ = new float[1];
        throttleZ[0] = 0f;
    }
}
