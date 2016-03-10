using UnityEngine;
using System.Collections;

public class AddonRaycastSensor : CritterNodeAddonBase {

    public int blah;

	public AddonRaycastSensor() {
        Debug.Log("Constructor AddonRaycastSensor()");
        addonType = CritterNodeAddonTypes.RaycastSensor;
    }
}
