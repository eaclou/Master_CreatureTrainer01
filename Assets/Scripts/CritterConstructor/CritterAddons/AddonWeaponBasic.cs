using UnityEngine;
using System.Collections;

public class AddonWeaponBasic {

    public int critterNodeID;
    public int innov;
    public float[] strength;

    public AddonWeaponBasic() {
        Debug.Log("Constructor AddonWeaponBasic()");
        strength = new float[1];
        strength[0] = 1f;
    }

    public AddonWeaponBasic(int id, int inno) {
        Debug.Log("Constructor AddonWeaponBasic(" + id.ToString() + ")");
        critterNodeID = id;
        innov = inno;
        strength = new float[1];
        strength[0] = 1f;
    }
}
