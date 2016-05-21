using UnityEngine;
using System.Collections;

public class AddonPositionSensor1D {

    public int critterNodeID;
    public bool[] relative;
    public Vector3[] forwardVector;
    public float[] sensitivity;

	public AddonPositionSensor1D() {
        Debug.Log("Constructor AddonPositionSensor1D()");
        relative = new bool[1];
        relative[0] = true;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonPositionSensor1D(int id) {
        Debug.Log("Constructor AddonPositionSensor1D(" + id.ToString() + ")");
        // Defaults:
        critterNodeID = id;
        relative = new bool[1];
        relative[0] = true;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
