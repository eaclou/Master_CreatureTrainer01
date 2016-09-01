using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameCritterWalkBasicSettings : MiniGameSettingsBase {

    // GameOptions:
    public float[] viscosityDrag = new float[1]; //50f;
    public float[] gravityStrength = new float[1];
    public float[] jointMotorForce = new float[1]; //100f;
    public float[] jointMotorSpeed = new float[1]; //500f;
    public float[] variableMass = new float[1]; // 0 means all segments have mass of 1.  // 1 means segment's mass is proportional to its volume
    public float[] minTargetX = new float[1];
    public float[] maxTargetX = new float[1];
    public float[] minTargetY = new float[1];
    public float[] maxTargetY = new float[1];
    public float[] minTargetZ = new float[1];
    public float[] maxTargetZ = new float[1];
    public float[] minScoreDistance = new float[1];
    public float[] maxScoreDistance = new float[1];
    public float[] targetRadius = new float[1];
    public float[] groundPositionY = new float[1];
    public float[] groundBounce = new float[1];
    public float[] groundFriction = new float[1];
    public float[] angleSensorSensitivity = new float[1];
    public float[] initForceMin = new float[1];
    public float[] initForceMax = new float[1];
    public float[] useRandomTargetPos = new float[1];

    public MiniGameCritterWalkBasicSettings() {
        // DEFAULTS:
        viscosityDrag[0] = 1f;
        gravityStrength[0] = 0f;
        jointMotorForce[0] = 0.1f; // global multipliers on individual joint motor settings
        jointMotorSpeed[0] = 0.2f;
        variableMass[0] = 0.8f;
        minTargetX[0] = 0f;
        maxTargetX[0] = 0f;
        minTargetY[0] = -5f;
        maxTargetY[0] = 5f;
        minTargetZ[0] = 0f;
        maxTargetZ[0] = 6f;
        minScoreDistance[0] = 1f;
        maxScoreDistance[0] = 0.75f;
        targetRadius[0] = 0.01f;
        groundPositionY[0] = -5f;
        angleSensorSensitivity[0] = 1f;
        initForceMin[0] = 0f;
        initForceMax[0] = 0.5f;
        groundBounce[0] = 0f;
        groundFriction[0] = 1f;
        useRandomTargetPos[0] = 0f;

        gameOptionsList = new List<GameOptionChannel>();
    }

    public override void InitGameOptionsList() {
        GameOptionChannel GOC_viscosityDrag = new GameOptionChannel(ref viscosityDrag, 0.0f, 15f, "Viscosity Drag");
        gameOptionsList.Add(GOC_viscosityDrag); // 
        GameOptionChannel GOC_gravityStrength = new GameOptionChannel(ref gravityStrength, -36.0f, 4f, "Gravity Strength");
        gameOptionsList.Add(GOC_gravityStrength); // 
        GameOptionChannel GOC_jointMotorForce = new GameOptionChannel(ref jointMotorForce, 0f, 10f, "Joint Motor Force");
        gameOptionsList.Add(GOC_jointMotorForce); // 
        GameOptionChannel GOC_jointMotorSpeed = new GameOptionChannel(ref jointMotorSpeed, 0f, 10f, "Joint Motor Speed");
        gameOptionsList.Add(GOC_jointMotorSpeed); // 
        GameOptionChannel GOC_variableMass = new GameOptionChannel(ref variableMass, 0f, 1f, "VariableMass");
        gameOptionsList.Add(GOC_variableMass); // 
        GameOptionChannel GOC_targetRadius = new GameOptionChannel(ref targetRadius, 0.01f, 25f, "Target Size");
        gameOptionsList.Add(GOC_targetRadius); //
        GameOptionChannel GOC_minTargetX = new GameOptionChannel(ref minTargetX, -10f, 10f, "X Min");
        gameOptionsList.Add(GOC_minTargetX); // 
        GameOptionChannel GOC_maxTargetX = new GameOptionChannel(ref maxTargetX, -10f, 10f, "X Max");
        gameOptionsList.Add(GOC_maxTargetX); // 
        GameOptionChannel GOC_minTargetY = new GameOptionChannel(ref minTargetY, -10f, 10f, "Y Min");
        gameOptionsList.Add(GOC_minTargetY); // 
        GameOptionChannel GOC_maxTargetY = new GameOptionChannel(ref maxTargetY, -10f, 10f, "Y Max");
        gameOptionsList.Add(GOC_maxTargetY); // 
        GameOptionChannel GOC_minTargetZ = new GameOptionChannel(ref minTargetZ, -10f, 10f, "Z Min");
        gameOptionsList.Add(GOC_minTargetZ); // 
        GameOptionChannel GOC_maxTargetZ = new GameOptionChannel(ref maxTargetZ, -10f, 10f, "Z Max");
        gameOptionsList.Add(GOC_maxTargetZ); // 
        GameOptionChannel GOC_minScoreDistance = new GameOptionChannel(ref minScoreDistance, 0.01f, 50f, "Min Target Distance");
        gameOptionsList.Add(GOC_minScoreDistance); // 7
        GameOptionChannel GOC_maxScoreDistance = new GameOptionChannel(ref maxScoreDistance, 0.01f, 20f, "Max Target Distance");
        gameOptionsList.Add(GOC_maxScoreDistance); // 7
        GameOptionChannel GOC_groundPositionY = new GameOptionChannel(ref groundPositionY, -50f, 0f, "Ground Position Y");
        gameOptionsList.Add(GOC_groundPositionY); // 7    
        GameOptionChannel GOC_angleSensorSensitivity = new GameOptionChannel(ref angleSensorSensitivity, 0f, 1f, "Angle Sensor Sensitivity");
        gameOptionsList.Add(GOC_angleSensorSensitivity); // 7
        GameOptionChannel GOC_initForceMin = new GameOptionChannel(ref initForceMin, 0f, 10f, "Init Force Min");
        gameOptionsList.Add(GOC_initForceMin); // 7
        GameOptionChannel GOC_initForceMax = new GameOptionChannel(ref initForceMax, 0f, 100f, "Init Force Max");
        gameOptionsList.Add(GOC_initForceMax); // 7
        GameOptionChannel GOC_useRandomTargetPos = new GameOptionChannel(ref useRandomTargetPos, 0f, 1f, "Use Random Target Pos");
        gameOptionsList.Add(GOC_useRandomTargetPos); // 7
    }

    public override void CopySettingsToSave(MiniGameSettingsSaves miniGameSettingsSaves) {
        miniGameSettingsSaves.angleSensorSensitivity = angleSensorSensitivity[0];
        miniGameSettingsSaves.gravityStrength = gravityStrength[0];
        miniGameSettingsSaves.groundBounce = groundBounce[0];
        miniGameSettingsSaves.groundFriction = groundFriction[0];
        miniGameSettingsSaves.groundPositionY = groundPositionY[0];
        miniGameSettingsSaves.jointMotorForce = jointMotorForce[0];
        miniGameSettingsSaves.jointMotorSpeed = jointMotorSpeed[0];
        miniGameSettingsSaves.maxScoreDistance = maxScoreDistance[0];
        miniGameSettingsSaves.maxTargetX = maxTargetX[0];
        miniGameSettingsSaves.maxTargetY = maxTargetY[0];
        miniGameSettingsSaves.maxTargetZ = maxTargetZ[0];
        miniGameSettingsSaves.minScoreDistance = minScoreDistance[0];
        miniGameSettingsSaves.minTargetX = minTargetX[0];
        miniGameSettingsSaves.minTargetY = minTargetY[0];
        miniGameSettingsSaves.minTargetZ = minTargetZ[0];
        miniGameSettingsSaves.targetRadius = targetRadius[0];
        miniGameSettingsSaves.viscosityDrag = viscosityDrag[0];
        miniGameSettingsSaves.initForceMin = initForceMin[0];
        miniGameSettingsSaves.initForceMax = initForceMax[0];
        miniGameSettingsSaves.useRandomTargetPos = useRandomTargetPos[0];
        miniGameSettingsSaves.variableMass = variableMass[0];
    }

    public override void CopySettingsFromLoad(MiniGameSettingsSaves miniGameSettingsSaves) {
        viscosityDrag[0] = miniGameSettingsSaves.viscosityDrag;
        gravityStrength[0] = miniGameSettingsSaves.gravityStrength;
        jointMotorForce[0] = miniGameSettingsSaves.jointMotorForce;
        jointMotorSpeed[0] = miniGameSettingsSaves.jointMotorSpeed;
        minTargetX[0] = miniGameSettingsSaves.minTargetX;
        maxTargetX[0] = miniGameSettingsSaves.maxTargetX;
        minTargetY[0] = miniGameSettingsSaves.minTargetY;
        maxTargetY[0] = miniGameSettingsSaves.maxTargetY;
        minTargetZ[0] = miniGameSettingsSaves.minTargetZ;
        maxTargetZ[0] = miniGameSettingsSaves.maxTargetZ;
        minScoreDistance[0] = miniGameSettingsSaves.minScoreDistance;
        maxScoreDistance[0] = miniGameSettingsSaves.maxScoreDistance;
        targetRadius[0] = miniGameSettingsSaves.targetRadius;
        groundPositionY[0] = miniGameSettingsSaves.groundPositionY;
        groundBounce[0] = miniGameSettingsSaves.groundBounce;
        groundFriction[0] = miniGameSettingsSaves.groundFriction;
        angleSensorSensitivity[0] = miniGameSettingsSaves.angleSensorSensitivity;
        initForceMin[0] = miniGameSettingsSaves.initForceMin;
        initForceMax[0] = miniGameSettingsSaves.initForceMax;
        useRandomTargetPos[0] = miniGameSettingsSaves.useRandomTargetPos;
        variableMass[0] = miniGameSettingsSaves.variableMass;
    }
}
