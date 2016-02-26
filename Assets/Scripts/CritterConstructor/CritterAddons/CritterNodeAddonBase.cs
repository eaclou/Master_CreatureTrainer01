using UnityEngine;
using System.Collections;

public class CritterNodeAddonBase {

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
        ValueInput        
    };

	public CritterNodeAddonBase() {
        Debug.Log("Constructor CritterNodeAddonBase()");
    }
}
