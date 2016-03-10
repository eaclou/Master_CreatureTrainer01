using UnityEngine;
using System.Collections;

public class AddonThrusterEffector3D : CritterNodeAddonBase {

    public int blah;

	public AddonThrusterEffector3D() {
        Debug.Log("Constructor AddonThrusterEffector3D()");
        addonType = CritterNodeAddonTypes.ThrusterEffector3D;
    }
}
