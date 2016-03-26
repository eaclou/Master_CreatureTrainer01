using UnityEngine;
using System.Collections;

public class AddonThrusterEffector3D {

    public int critterNodeID;
    //public Vector3[] directionVector;
    public float[] maxForce;

    public AddonThrusterEffector3D() {
        Debug.Log("Constructor AddonThrusterEffector3D()");
        //directionVector = new Vector3[1];
        //directionVector[0] = new Vector3(0f, 0f, 1f);
        maxForce = new float[1];
        maxForce[0] = 10f;
    }

    public AddonThrusterEffector3D(int id) {
        Debug.Log("Constructor AddonThrusterEffector3D(" + id.ToString() + ")");
        critterNodeID = id;
        //directionVector = new Vector3[1];
        //directionVector[0] = new Vector3(0f, 0f, 1f);
        maxForce = new float[1];
        maxForce[0] = 10f;
    }
}
