using UnityEngine;
using System.Collections;

public class AddonRotationSensor3D {

    public int critterNodeID;
    public int innov;
    public float[] sensitivity;

    public AddonRotationSensor3D() {
        Debug.Log("Constructor AddonRotationSensor3D()");
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonRotationSensor3D(int id, int inno) {
        Debug.Log("Constructor AddonRotationSensor3D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonRotationSensor3D CloneThisAddon() {
        AddonRotationSensor3D clonedAddon = new AddonRotationSensor3D(this.critterNodeID, this.innov);
        clonedAddon.sensitivity[0] = this.sensitivity[0];
        return clonedAddon;
    }
}
