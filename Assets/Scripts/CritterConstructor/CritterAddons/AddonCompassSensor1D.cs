using UnityEngine;
using System.Collections;

public class AddonCompassSensor1D {

    public int critterNodeID;
    public Vector3[] forwardVector;

	public AddonCompassSensor1D() {
        Debug.Log("Constructor AddonCompassSensor1D()");
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
    }

    public AddonCompassSensor1D(int id) {
        Debug.Log("Constructor AddonCompassSensor1D(" + id.ToString() + ")");
        critterNodeID = id;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
    }
}
