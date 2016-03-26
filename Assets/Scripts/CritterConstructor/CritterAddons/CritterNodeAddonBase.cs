using UnityEngine;
using System.Collections;

[System.Serializable]
public class CritterNodeAddonBase {

    public CritterNodeAddonTypes addonType;
    public enum CritterNodeAddonTypes {   
        PhysicalAttributes,  // physicMaterial and rigidBody settings

        JointAngleSensor,    // Sensors v v v
        ContactSensor,       //
        RaycastSensor,       //
        CompassSensor1D,     //
        CompassSensor3D,     //
        PositionSensor1D,     //
        PositionSensor3D,     //
        RotationSensor1D,     //
        RotationSensor3D,     //
        VelocitySensor1D,     //
        VelocitySensor3D,     //
        Altimeter,           // Sensors ^ ^ ^

        JointMotor,          // Effectors v v v
        ThrusterEffector1D,  //
        ThrusterEffector3D,  //
        TorqueEffector1D,    //
        TorqueEffector3D,    // Effectors ^ ^ ^

        OscillatorInput,     // Inputs v v v
        ValueInput,          //
        TimerInput           // Inputs ^ ^ ^
    };

	public CritterNodeAddonBase() {
        Debug.Log("Constructor CritterNodeAddonBase()");
    }
}
