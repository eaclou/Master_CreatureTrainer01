using UnityEngine;
using System.Collections;

public class AddonRotationSensor1D {

    public int critterNodeID;
    public Vector3[] localAxis;
    public float[] sensitivity;

	public AddonRotationSensor1D() {
        Debug.Log("Constructor AddonRotationSensor1D()");
        localAxis = new Vector3[1];
        localAxis[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonRotationSensor1D(int id) {
        Debug.Log("Constructor AddonRotationSensor1D(" + id.ToString() + ")");
        critterNodeID = id;
        localAxis = new Vector3[1];
        localAxis[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }
}
