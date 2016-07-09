using UnityEngine;
using System.Collections;

public class AddonPositionSensor3D {

    public int critterNodeID;
    public int innov;
    public bool[] relative;
    public float[] sensitivity;

    public AddonPositionSensor3D() {
        //Debug.Log("Constructor AddonPositionSensor3D()");
        relative = new bool[1];
        relative[0] = true;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonPositionSensor3D(int id, int inno) {
        //Debug.Log("Constructor AddonPositionSensor3D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        relative = new bool[1];
        relative[0] = true;
        sensitivity = new float[1];
        sensitivity[0] = 1f;
    }

    public AddonPositionSensor3D CloneThisAddon() {
        AddonPositionSensor3D clonedAddon = new AddonPositionSensor3D(this.critterNodeID, this.innov);
        clonedAddon.relative[0] = this.relative[0];
        clonedAddon.sensitivity[0] = this.sensitivity[0];
        return clonedAddon;
    }
}
