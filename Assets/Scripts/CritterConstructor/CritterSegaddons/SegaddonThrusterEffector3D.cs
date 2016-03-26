using UnityEngine;
using System.Collections;

public class SegaddonThrusterEffector3D {

    public int segmentID;  // move to base class?
    public float[] maxForce;
    public float[] throttleX;
    public float[] throttleY;
    public float[] throttleZ;

    public SegaddonThrusterEffector3D() {
        maxForce = new float[1];
        maxForce[0] = 0f;
        throttleX = new float[1];
        throttleX[0] = 0f;
        throttleY = new float[1];
        throttleY[0] = 0f;
        throttleZ = new float[1];
        throttleZ[0] = 0f;
    }

    public SegaddonThrusterEffector3D(AddonThrusterEffector3D sourceNode) {        
        maxForce = new float[1];
        maxForce[0] = sourceNode.maxForce[0];
        throttleX = new float[1];
        throttleX[0] = 0f;
        throttleY = new float[1];
        throttleY[0] = 0f;
        throttleZ = new float[1];
        throttleZ[0] = 0f;
    }
}
