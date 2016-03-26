using UnityEngine;
using System.Collections;

public class AddonJointAngleSensor {

    public int critterNodeID;
    public float[] sensitivity;

    public AddonJointAngleSensor() {
        Debug.Log("Constructor AddonJointAngleSensor()");
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonJointAngleSensor(int id) {
        Debug.Log("Constructor AddonJointAngleSensor(" + id.ToString() + ")");
        critterNodeID = id;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
