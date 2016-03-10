using UnityEngine;
using System.Collections;

[System.Serializable]
public class CritterNodeAddonBase {

    public CritterNodeAddonTypes addonType;
    public enum CritterNodeAddonTypes {        
        JointMotor,
        JointAngleSensor,
        ContactSensor,     // includes collider?
        RaycastSensor,
        CompassSensor1D,
        CompassSensor3D,
        ThrusterEffector1D,
        ThrusterEffector3D,
        StickyEffector,
        OscillatorInput,
        ValueInput,
        None
    };

	public CritterNodeAddonBase() {
        Debug.Log("Constructor CritterNodeAddonBase()");
        addonType = CritterNodeAddonTypes.None;
    }
}
