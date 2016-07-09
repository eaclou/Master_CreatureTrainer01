using UnityEngine;
using System.Collections;

public class AddonSticky {

    public int critterNodeID;
    public int innov;

    public AddonSticky() {
        Debug.Log("Constructor AddonSticky()");
    }

    public AddonSticky(int id, int inno) {
        Debug.Log("Constructor AddonSticky(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
    }

    public AddonSticky CloneThisAddon() {
        AddonSticky clonedAddon = new AddonSticky(this.critterNodeID, this.innov);
        return clonedAddon;
    }
}
