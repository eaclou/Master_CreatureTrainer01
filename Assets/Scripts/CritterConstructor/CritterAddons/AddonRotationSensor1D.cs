using UnityEngine;
using System.Collections;

public class AddonRotationSensor1D {

    public int critterNodeID;
    public int innov;
    public Vector3[] localAxis;
    public float[] sensitivity;

	public AddonRotationSensor1D() {
        Debug.Log("Constructor AddonRotationSensor1D()");
        localAxis = new Vector3[1];
        localAxis[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonRotationSensor1D(int id, int inno) {
        Debug.Log("Constructor AddonRotationSensor1D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        localAxis = new Vector3[1];
        localAxis[0] = new Vector3(0f, 0f, 1f);
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonRotationSensor1D CloneThisAddon() {
        AddonRotationSensor1D clonedAddon = new AddonRotationSensor1D(this.critterNodeID, this.innov);
        clonedAddon.localAxis[0] = this.localAxis[0];
        clonedAddon.sensitivity[0] = this.sensitivity[0];
        return clonedAddon;
    }
}
