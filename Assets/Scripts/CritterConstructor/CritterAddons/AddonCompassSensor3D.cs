﻿using UnityEngine;
using System.Collections;

public class AddonCompassSensor3D {

    public int critterNodeID;
    public int innov;
    //public Vector3[] forwardVector;

    public AddonCompassSensor3D() {
        //Debug.Log("Constructor AddonCompassSensor3D()");
        //forwardVector = new Vector3[1];
        //forwardVector[0] = new Vector3(0f, 0f, 1f);
    }

    public AddonCompassSensor3D(int id, int inno) {
        //Debug.Log("Constructor AddonCompassSensor3D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        //forwardVector = new Vector3[1];
        //forwardVector[0] = new Vector3(0f, 0f, 1f);
    }

    public AddonCompassSensor3D CloneThisAddon() {
        AddonCompassSensor3D clonedAddon = new AddonCompassSensor3D(this.critterNodeID, this.innov);
        return clonedAddon;
    }
}
