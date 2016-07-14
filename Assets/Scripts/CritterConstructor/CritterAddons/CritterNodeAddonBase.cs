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
        Altimeter,           
        EarBasic,
        Gravity,             // Sensors ^ ^ ^
        OscillatorInput,     // Inputs v v v
        ValueInput,          //
        TimerInput,          // Inputs ^ ^ ^

        JointMotor,          // Effectors v v v
        ThrusterEffector1D,  //
        ThrusterEffector3D,  //
        TorqueEffector1D,    //
        TorqueEffector3D,    
        MouthBasic,
        NoiseMakerBasic,
        Sticky,
        WeaponBasic         // Effectors ^ ^ ^
    };

	public CritterNodeAddonBase() {
        Debug.Log("Constructor CritterNodeAddonBase()");
    }
}
