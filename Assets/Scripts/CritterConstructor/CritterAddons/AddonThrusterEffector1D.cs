using UnityEngine;
using System.Collections;

public class AddonThrusterEffector1D : CritterNodeAddonBase {

    public int blah;

	public AddonThrusterEffector1D() {
        Debug.Log("Constructor AddonThrusterEffector1D()");
        addonType = CritterNodeAddonTypes.ThrusterEffector1D;
    }
}
