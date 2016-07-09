using UnityEngine;
using System.Collections;

public class AddonGravitySensor {

    public int critterNodeID;
    public int innov;

    public AddonGravitySensor() {
        Debug.Log("Constructor AddonGravitySensor()");
    }

    public AddonGravitySensor(int id, int inno) {
        Debug.Log("Constructor AddonGravitySensor(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
    }

    public AddonGravitySensor CloneThisAddon() {
        AddonGravitySensor clonedAddon = new AddonGravitySensor(this.critterNodeID, this.innov);
        return clonedAddon;
    }
}
