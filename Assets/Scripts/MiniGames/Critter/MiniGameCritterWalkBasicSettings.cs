using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameCritterWalkBasicSettings : MiniGameSettingsBase {

    // GameOptions:
    public float[] viscosityDrag = new float[1]; //50f;
    public float[] gravityStrength = new float[1];
    public float[] jointMotorForce = new float[1]; //100f;
    public float[] jointMotorSpeed = new float[1]; //500f;
    //public float[] variableMass = new float[1]; // 0 means all segments have mass of 1.  // 1 means segment's mass is proportional to its volume
    public float[] minTargetX = new float[1];
    public float[] maxTargetX = new float[1];
    public float[] minTargetY = new float[1];
    public float[] maxTargetY = new float[1];
    public float[] minTargetZ = new float[1];
    public float[] maxTargetZ = new float[1];
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
        viscosityDrag[0] = 1f;
        gravityStrength[0] = 0f;
        jointMotorForce[0] = 2f; // global multipliers on individual joint motor settings
        jointMotorSpeed[0] = 2f;
        //variableMass[0] = 0.0f;
        minTargetX[0] = -5f;
        maxTargetX[0] = 5f;
        minTargetY[0] = -5f;
        maxTargetY[0] = 5f;
        minTargetZ[0] = 10f;
        maxTargetZ[0] = 10f;
        minScoreDistance[0] = 15f;
        maxScoreDistance[0] = 1f;
        //moveSpeedMaxFit[0] = 0.1f;
        targetRadius[0] = 5f;
        groundPositionY[0] = -25f;
        angleSensorSensitivity[0] = 0.1f;

        gameOptionsList = new List<GameOptionChannel>();
    }

    public override void InitGameOptionsList() {
        GameOptionChannel GOC_viscosityDrag = new GameOptionChannel(ref viscosityDrag, 0.0f, 200f, "Viscosity Drag");
        gameOptionsList.Add(GOC_viscosityDrag); // 
        GameOptionChannel GOC_gravityStrength = new GameOptionChannel(ref gravityStrength, -36.0f, 4f, "Gravity Strength");
        gameOptionsList.Add(GOC_gravityStrength); // 
        GameOptionChannel GOC_jointMotorForce = new GameOptionChannel(ref jointMotorForce, 0f, 10f, "Joint Motor Force");
        gameOptionsList.Add(GOC_jointMotorForce); // 
        GameOptionChannel GOC_jointMotorSpeed = new GameOptionChannel(ref jointMotorSpeed, 0f, 10f, "Joint Motor Speed");
        gameOptionsList.Add(GOC_jointMotorSpeed); // 
        //GameOptionChannel GOC_variableMass = new GameOptionChannel(ref variableMass, 0f, 1f, "VariableMass");
        //gameOptionsList.Add(GOC_variableMass); // 
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
        //GameOptionChannel GOC_targetPosAxis = new GameOptionChannel(ref targetPosAxis, 0f, 1f, "Target Only Positive Axis");
        //gameOptionsList.Add(GOC_targetPosAxis); // 5
        //GameOptionChannel GOC_moveSpeedMaxFit = new GameOptionChannel(ref moveSpeedMaxFit, 0.001f, 1f, "Move Speed Max Score");
        //gameOptionsList.Add(GOC_moveSpeedMaxFit); // 6
        GameOptionChannel GOC_minScoreDistance = new GameOptionChannel(ref minScoreDistance, 0.01f, 50f, "Min Target Distance");
        gameOptionsList.Add(GOC_minScoreDistance); // 7
        GameOptionChannel GOC_maxScoreDistance = new GameOptionChannel(ref maxScoreDistance, 0.01f, 50f, "Max Target Distance");
        gameOptionsList.Add(GOC_maxScoreDistance); // 7
        GameOptionChannel GOC_groundPositionY = new GameOptionChannel(ref groundPositionY, -20f, 0f, "Ground Position Y");
        gameOptionsList.Add(GOC_groundPositionY); // 7    
        GameOptionChannel GOC_angleSensorSensitivity = new GameOptionChannel(ref angleSensorSensitivity, 0f, 1f, "Angle Sensor Sensitivity");
        gameOptionsList.Add(GOC_angleSensorSensitivity); // 7
    }    
}
