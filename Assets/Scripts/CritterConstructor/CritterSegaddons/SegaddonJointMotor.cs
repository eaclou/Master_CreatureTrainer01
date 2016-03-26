using UnityEngine;
using System.Collections;

public class SegaddonJointMotor {

    public int segmentID;  // move to base class?
    public float[] motorForce;
    public float[] motorSpeed;
    public float[] targetAngularX;
    public float[] targetAngularY;
    public float[] targetAngularZ;

    public SegaddonJointMotor() {
        motorForce = new float[1];
        motorForce[0] = 100f;
        motorSpeed = new float[1];
        motorSpeed[0] = 100f;

        targetAngularX = new float[1];
        targetAngularX[0] = 0f;
        targetAngularY = new float[1];
        targetAngularY[0] = 0f;
        targetAngularZ = new float[1];
        targetAngularZ[0] = 0f;
    }

    public SegaddonJointMotor(AddonJointMotor sourceNode) {
        motorForce = new float[1];
        motorForce[0] = sourceNode.motorForce[0];
        motorSpeed = new float[1];
        motorSpeed[0] = sourceNode.motorSpeed[0];

        targetAngularX = new float[1];
        targetAngularX[0] = 0f;
        targetAngularY = new float[1];
        targetAngularY[0] = 0f;
        targetAngularZ = new float[1];
        targetAngularZ[0] = 0f;
    }
}
