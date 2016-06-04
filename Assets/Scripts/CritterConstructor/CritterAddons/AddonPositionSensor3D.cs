﻿using UnityEngine;
using System.Collections;

public class AddonPositionSensor3D {

    public int critterNodeID;
    public bool[] relative;
    public float[] sensitivity;

    public AddonPositionSensor3D() {
        //Debug.Log("Constructor AddonPositionSensor3D()");
        relative = new bool[1];
        relative[0] = true;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonPositionSensor3D(int id) {
        //Debug.Log("Constructor AddonPositionSensor3D(" + id.ToString() + ")");
        critterNodeID = id;
        relative = new bool[1];
        relative[0] = true;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
