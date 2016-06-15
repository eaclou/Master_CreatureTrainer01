using UnityEngine;
using System.Collections;

public class SegaddonWeaponBasic {

    public int segmentID;
    public float[] strength;

    public SegaddonWeaponBasic() {
        strength = new float[1];
        strength[0] = 0f;
    }

    public SegaddonWeaponBasic(AddonWeaponBasic sourceNode) {
        strength = new float[1];
        strength[0] = sourceNode.strength[0];
    }
}
