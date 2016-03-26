using UnityEngine;
using System.Collections;

public class AddonValueInput {

    public int critterNodeID;
    public float[] value;

    public AddonValueInput() {
        Debug.Log("Constructor AddonValueInput()");
        value = new float[1];
        value[0] = 1f;
    }

    public AddonValueInput(int id) {
        Debug.Log("Constructor AddonValueInput(" + id.ToString() + ")");
        critterNodeID = id;
        value = new float[1];
        value[0] = 1f;
    }
}
