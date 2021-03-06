﻿using UnityEngine;
using System.Collections;

public class SegaddonJointAngleSensor {

    public int segmentID;  // move to base class?
    public float[] angleSensitivity;
    public float[] angleX;
    public float[] angleY;
    public float[] angleZ;
    public float[] angleVelX;
    public float[] angleVelY;
    public float[] angleVelZ;
    public bool measureVel;

    public SegaddonJointAngleSensor() {        
        angleSensitivity = new float[1];
        angleSensitivity[0] = 1f;

        angleX = new float[1];
        angleX[0] = 0f;
        angleY = new float[1];
        angleY[0] = 0f;
        angleZ = new float[1];
        angleZ[0] = 0f;

        angleVelX = new float[1];
        angleVelX[0] = 0f;
        angleVelY = new float[1];
        angleVelY[0] = 0f;
        angleVelZ = new float[1];
        angleVelZ[0] = 0f;
    }

    public SegaddonJointAngleSensor(AddonJointAngleSensor sourceNode) {
        angleSensitivity = new float[1];
        angleSensitivity[0] = sourceNode.sensitivity[0];
        measureVel = sourceNode.measureVel[0];

        angleX = new float[1];
        angleX[0] = 0f;
        angleY = new float[1];
        angleY[0] = 0f;
        angleZ = new float[1];
        angleZ[0] = 0f;

        angleVelX = new float[1];
        angleVelX[0] = 0f;
        angleVelY = new float[1];
        angleVelY[0] = 0f;
        angleVelZ = new float[1];
        angleVelZ[0] = 0f;
    }
}
