using UnityEngine;
using System.Collections;

public class AddonTimerInput {

    public int critterNodeID;
    public int innov;
    //public float[] value;

    public AddonTimerInput() {
        Debug.Log("Constructor AddonTimerInput()");
        //value = new float[1];
        //value[0] = 1f;
    }

    public AddonTimerInput(int id, int inno) {
        Debug.Log("Constructor AddonTimerInput(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        //value = new float[1];
        //value[0] = 1f;
    }

    public AddonTimerInput CloneThisAddon() {
        AddonTimerInput clonedAddon = new AddonTimerInput(this.critterNodeID, this.innov);
        return clonedAddon;
    }
}
