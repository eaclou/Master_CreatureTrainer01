using UnityEngine;
using System.Collections;

public class AddonThrusterEffector1D {

    public int critterNodeID;
    public Vector3[] forwardVector;
    public float[] maxForce;

    public AddonThrusterEffector1D() {
        Debug.Log("Constructor AddonThrusterEffector1D()");
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        maxForce = new float[1];
        maxForce[0] = 10f;
    }

    public AddonThrusterEffector1D(int id) {
        Debug.Log("Constructor AddonThrusterEffector1D(" + id.ToString() + ")");
        critterNodeID = id;
        forwardVector = new Vector3[1];
        forwardVector[0] = new Vector3(0f, 0f, 1f);
        maxForce = new float[1];
        maxForce[0] = 10f;
    }

}
