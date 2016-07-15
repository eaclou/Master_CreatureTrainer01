using UnityEngine;
using System.Collections;

public class AddonValueInput {

    public int critterNodeID;
    public int innov;
    public float[] value;

    public AddonValueInput() {
        Debug.Log("Constructor AddonValueInput()");
        value = new float[1];
        value[0] = 1f;
    }

    public AddonValueInput(int id, int inno) {
        Debug.Log("Constructor AddonValueInput(" + id.ToString() + ") inno: " + inno.ToString());
        critterNodeID = id;
        innov = inno;
        value = new float[1];
        value[0] = 1f;
    }

    public AddonValueInput CloneThisAddon() {
        AddonValueInput clonedAddon = new AddonValueInput(this.critterNodeID, this.innov);
        clonedAddon.value[0] = this.value[0];
        return clonedAddon;
    }
}
