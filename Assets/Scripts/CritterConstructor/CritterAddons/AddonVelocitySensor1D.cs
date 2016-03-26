using UnityEngine;
using System.Collections;

public class AddonVelocitySensor1D {

    public int critterNodeID;
    public bool[] relative;
    public Vector3[] forwardVector;
    public float[] sensitivity;

	public AddonVelocitySensor1D() {
        Debug.Log("Constructor AddonVelocitySensor1D()");
        relative = new bool[1];
        relative[0] = true;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonVelocitySensor1D(int id) {
        Debug.Log("Constructor AddonVelocitySensor1D(" + id.ToString() + ")");
        critterNodeID = id;
        relative = new bool[1];
        relative[0] = true;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
