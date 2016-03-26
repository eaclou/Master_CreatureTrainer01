using UnityEngine;
using System.Collections;

public class AddonTimerInput {

    public int critterNodeID;
    //public float[] value;

    public AddonTimerInput() {
        Debug.Log("Constructor AddonTimerInput()");
        //value = new float[1];
        //value[0] = 1f;
    }

    public AddonTimerInput(int id) {
        Debug.Log("Constructor AddonTimerInput(" + id.ToString() + ")");
        critterNodeID = id;
        //value = new float[1];
        //value[0] = 1f;
    }
}
