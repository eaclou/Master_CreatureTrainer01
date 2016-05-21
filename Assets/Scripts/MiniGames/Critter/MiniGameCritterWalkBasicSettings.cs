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
    public float[] targetX = new float[1];
    public float[] targetY = new float[1];
    public float[] targetZ = new float[1];
    public float[] targetPosAxis = new float[1];
    //public float[] moveSpeedMaxFit = new float[1];
    public float[] minScoreDistance = new float[1];
    public float[] maxScoreDistance = new float[1];
    public float[] targetRadius = new float[1];
    public float[] groundPositionY = new float[1];
    public float[] groundBounce = new float[1];
    public float[] groundFriction = new float[1];
    public float[] angleSensorSensitivity = new float[1];

    public MiniGameCritterWalkBasicSettings() {
        // DEFAULTS:
        viscosityDrag[0] = 0f;
        gravityStrength[0] = 0f;
        jointMotorForce[0] = 1f; // global multipliers on individual joint motor settings
        jointMotorSpeed[0] = 1f;
        variableMass[0] = 0.0f;
        targetX[0] = 0f;
        targetY[0] = 0f;
        targetZ[0] = 1f;
        targetPosAxis[0] = 0.5f;
        minScoreDistance[0] = 3f;
        maxScoreDistance[0] = 3.1f;
        //moveSpeedMaxFit[0] = 0.1f;
        targetRadius[0] = 1f;
        groundPositionY[0] = -20f;
        angleSensorSensitivity[0] = 1f;

        gameOptionsList = new List<GameOptionChannel>();
    }

    public override void InitGameOptionsList() {
        GameOptionChannel GOC_viscosityDrag = new GameOptionChannel(ref viscosityDrag, 0.0f, 200f, "Viscosity Drag");
        gameOptionsList.Add(GOC_viscosityDrag); // 0
        GameOptionChannel GOC_gravityStrength = new GameOptionChannel(ref gravityStrength, -36.0f, 4f, "Gravity Strength");
        gameOptionsList.Add(GOC_gravityStrength); // 0
        GameOptionChannel GOC_jointMotorForce = new GameOptionChannel(ref jointMotorForce, 0f, 10f, "Joint Motor Force");
        gameOptionsList.Add(GOC_jointMotorForce); // 1
        GameOptionChannel GOC_jointMotorSpeed = new GameOptionChannel(ref jointMotorSpeed, 0f, 10f, "Joint Motor Speed");
        gameOptionsList.Add(GOC_jointMotorSpeed); // 2
        GameOptionChannel GOC_variableMass = new GameOptionChannel(ref variableMass, 0f, 1f, "VariableMass");
        gameOptionsList.Add(GOC_variableMass); // 2
        GameOptionChannel GOC_targetRadius = new GameOptionChannel(ref targetRadius, 0.01f, 25f, "Target Size");
        gameOptionsList.Add(GOC_targetRadius); // 8
        GameOptionChannel GOC_targetX = new GameOptionChannel(ref targetX, 0f, 1f, "Target Use X-Axis");
        gameOptionsList.Add(GOC_targetX); // 3
        GameOptionChannel GOC_targetY = new GameOptionChannel(ref targetY, 0f, 1f, "Target Use Y-Axis");
        gameOptionsList.Add(GOC_targetY); // 4
        GameOptionChannel GOC_targetZ = new GameOptionChannel(ref targetZ, 0f, 1f, "Target Use Z-Axis");
        gameOptionsList.Add(GOC_targetZ); // 5
        GameOptionChannel GOC_targetPosAxis = new GameOptionChannel(ref targetPosAxis, 0f, 1f, "Target Only Positive Axis");
        gameOptionsList.Add(GOC_targetPosAxis); // 5
        //GameOptionChannel GOC_moveSpeedMaxFit = new GameOptionChannel(ref moveSpeedMaxFit, 0.001f, 1f, "Move Speed Max Score");
        //gameOptionsList.Add(GOC_moveSpeedMaxFit); // 6
        GameOptionChannel GOC_minScoreDistance = new GameOptionChannel(ref minScoreDistance, 0.001f, 50f, "Min Target Distance");
        gameOptionsList.Add(GOC_minScoreDistance); // 7
        GameOptionChannel GOC_maxScoreDistance = new GameOptionChannel(ref maxScoreDistance, 0.001f, 50f, "Max Target Distance");
        gameOptionsList.Add(GOC_maxScoreDistance); // 7
        GameOptionChannel GOC_groundPositionY = new GameOptionChannel(ref groundPositionY, -20f, 0f, "Ground Position Y");
        gameOptionsList.Add(GOC_groundPositionY); // 7    
        GameOptionChannel GOC_angleSensorSensitivity = new GameOptionChannel(ref angleSensorSensitivity, 0f, 1f, "Angle Sensor Sensitivity");
        gameOptionsList.Add(GOC_angleSensorSensitivity); // 7
    }    
}
