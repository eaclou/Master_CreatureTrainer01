using UnityEngine;
using System.Collections;

public class SegaddonMouthBasic {

    public int segmentID;
    public float[] contactStatus;
    public float[] biteStrength;

    public SegaddonMouthBasic() {
        contactStatus = new float[1];
        contactStatus[0] = 0f;
        biteStrength = new float[1];
        biteStrength[0] = 0f;
    }

    public SegaddonMouthBasic(AddonMouthBasic sourceNode) {
        contactStatus = new float[1];
        contactStatus[0] = 0f;
        biteStrength = new float[1];
        biteStrength[0] = 0f;
    }
}
