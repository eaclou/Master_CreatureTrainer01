using UnityEngine;
using System.Collections;

public class MiniGameSettingsSaves {

    // GameOptions:
    public float viscosityDrag; 
    public float gravityStrength;
    public float jointMotorForce;
    public float jointMotorSpeed;
    public float variableMass;
    public float minTargetX;
    public float maxTargetX;
    public float minTargetY;
    public float maxTargetY;
    public float minTargetZ;
    public float maxTargetZ;
    public float minScoreDistance;
    public float maxScoreDistance;
    public float targetRadius;
    public float groundPositionY;
    public float groundBounce;
    public float groundFriction;
    public float angleSensorSensitivity;
    public float initForceMin;
    public float initForceMax;
    public float useRandomTargetPos;

    public MiniGameSettingsSaves() {
        //empty constructor for EasySave2
    }
}
