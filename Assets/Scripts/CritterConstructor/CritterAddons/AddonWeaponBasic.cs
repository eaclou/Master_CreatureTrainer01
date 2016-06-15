using UnityEngine;
using System.Collections;

public class AddonWeaponBasic {

    public int critterNodeID;
    public float[] strength;

    public AddonWeaponBasic() {
        Debug.Log("Constructor AddonWeaponBasic()");
        strength = new float[1];
        strength[0] = 1f;
    }

    public AddonWeaponBasic(int id) {
        Debug.Log("Constructor AddonWeaponBasic(" + id.ToString() + ")");
        critterNodeID = id;
        strength = new float[1];
        strength[0] = 1f;
    }
}
