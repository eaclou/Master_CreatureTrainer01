﻿using UnityEngine;
using System.Collections;

public class AddonCompassSensor1D {

    public int critterNodeID;
    public int innov;
    public Vector3[] forwardVector;

	public AddonCompassSensor1D() {
        Debug.Log("Constructor AddonCompassSensor1D()");
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
    }

    public AddonCompassSensor1D(int id, int inno) {
        Debug.Log("Constructor AddonCompassSensor1D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
    }

    public AddonCompassSensor1D CloneThisAddon() {
        AddonCompassSensor1D clonedAddon = new AddonCompassSensor1D(this.critterNodeID, this.innov);
        clonedAddon.forwardVector[0] = this.forwardVector[0];
        return clonedAddon;
    }
}
