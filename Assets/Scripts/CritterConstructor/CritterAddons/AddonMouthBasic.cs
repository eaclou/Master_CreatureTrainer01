using UnityEngine;
using System.Collections;

public class AddonMouthBasic {

    public int critterNodeID;
    public int innov;

    public AddonMouthBasic() {
        Debug.Log("Constructor AddonMouthBasic()");
    }

    public AddonMouthBasic(int id, int inno) {
        Debug.Log("Constructor AddonMouthBasic(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
    }

    public AddonMouthBasic CloneThisAddon() {
        AddonMouthBasic clonedAddon = new AddonMouthBasic(this.critterNodeID, this.innov);
        return clonedAddon;
    }
}
