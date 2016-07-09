using UnityEngine;
using System.Collections;

public class AddonContactSensor {

    public int critterNodeID;
    public int innov;
    public float[] contactSensitivity;

    public AddonContactSensor() {
        Debug.Log("Constructor AddonContactSensor()");
        contactSensitivity = new float[1];
        contactSensitivity[0] = 1f;
    }

    public AddonContactSensor(int id, int inno) {
        Debug.Log("Constructor AddonContactSensor(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        contactSensitivity = new float[1];
        contactSensitivity[0] = 1f;
    }

    public AddonContactSensor CloneThisAddon() {
        AddonContactSensor clonedAddon = new AddonContactSensor(this.critterNodeID, this.innov);
        clonedAddon.contactSensitivity[0] = this.contactSensitivity[0];
        return clonedAddon;
    }
}
