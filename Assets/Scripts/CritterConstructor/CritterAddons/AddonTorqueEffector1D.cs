using UnityEngine;
using System.Collections;

public class AddonTorqueEffector1D {

    public int critterNodeID;
    public int innov;
    public Vector3[] axis;
    public float[] maxTorque;

    public AddonTorqueEffector1D() {
        Debug.Log("Constructor AddonTorqueEffector1D()");
        axis = new Vector3[1];
        axis[0] = new Vector3(0f, 0f, 1f);
        maxTorque = new float[1];
        maxTorque[0] = 10f;
    }

    public AddonTorqueEffector1D(int id, int inno) {
        Debug.Log("Constructor AddonTorqueEffector1D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        axis = new Vector3[1];
        axis[0] = new Vector3(0f, 0f, 1f);
        maxTorque = new float[1];
        maxTorque[0] = 10f;
    }
}
