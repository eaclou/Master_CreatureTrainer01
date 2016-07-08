using UnityEngine;
using System.Collections;

public class AddonVelocitySensor3D {

    public int critterNodeID;
    public int innov;
    public bool[] relative;
    public float[] sensitivity;

    public AddonVelocitySensor3D() {
        Debug.Log("Constructor AddonVelocitySensor3D()");
        relative = new bool[1];
        relative[0] = true;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonVelocitySensor3D(int id, int inno) {
        Debug.Log("Constructor AddonVelocitySensor3D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        relative = new bool[1];
        relative[0] = true;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
