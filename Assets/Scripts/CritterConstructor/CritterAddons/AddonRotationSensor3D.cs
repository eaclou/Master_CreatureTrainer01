using UnityEngine;
using System.Collections;

public class AddonRotationSensor3D {

    public int critterNodeID;
    public float[] sensitivity;

    public AddonRotationSensor3D() {
        Debug.Log("Constructor AddonRotationSensor3D()");
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonRotationSensor3D(int id) {
        Debug.Log("Constructor AddonRotationSensor3D(" + id.ToString() + ")");
        critterNodeID = id;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
