using UnityEngine;
using System.Collections;

public class AddonStickyEffector : CritterNodeAddonBase {

    public int blah;

	public AddonStickyEffector() {
        Debug.Log("Constructor AddonStickyEffector()");
        addonType = CritterNodeAddonTypes.StickyEffector;
    }
}
