using UnityEngine;
using System.Collections;

public class AddonCompassSensor3D : CritterNodeAddonBase {

    public int blah;

	public AddonCompassSensor3D() {
        Debug.Log("Constructor AddonCompassSensor3D()");
        addonType = CritterNodeAddonTypes.CompassSensor3D;
    }
}
