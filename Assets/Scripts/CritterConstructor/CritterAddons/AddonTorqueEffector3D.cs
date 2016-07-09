using UnityEngine;
using System.Collections;

public class AddonTorqueEffector3D {

    public int critterNodeID;
    public int innov;
    public float[] maxTorque;
    //public float[] throttle;

    public AddonTorqueEffector3D() {
        Debug.Log("Constructor AddonTorqueEffector3D()");
        //directionVector = new Vector3[1];
        //directionVector[0] = new Vector3(0f, 0f, 1f);
        maxTorque = new float[1];
        maxTorque[0] = 10f;
        //throttle = new float[1];
        //throttle[0] = 0f;
    }

    public AddonTorqueEffector3D(int id, int inno) {
        Debug.Log("Constructor AddonTorqueEffector3D(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        //directionVector = new Vector3[1];
        //directionVector[0] = new Vector3(0f, 0f, 1f);
        maxTorque = new float[1];
        maxTorque[0] = 10f;
        //throttle = new float[1];
        //throttle[0] = 0f;
    }

    public AddonTorqueEffector3D CloneThisAddon() {
        AddonTorqueEffector3D clonedAddon = new AddonTorqueEffector3D(this.critterNodeID, this.innov);
        clonedAddon.maxTorque[0] = this.maxTorque[0];
        return clonedAddon;
    }
}
