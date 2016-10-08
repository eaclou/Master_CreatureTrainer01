using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameCritterWalkBasic : MiniGameBase
{

    public static bool debugFunctionCalls = false; // turns debug messages on/off

    public float[][] wormSegmentArray_PosX;
    public float[][] wormSegmentArray_PosY;
    public float[][] wormSegmentArray_PosZ;    
    public float[][] wormSegmentArray_ScaleX;
    public float[][] wormSegmentArray_ScaleY;
    public float[][] wormSegmentArray_ScaleZ;
    
    public Quaternion[] segmentArrayRestRotation;

    public float[] wormSegmentArray_Mass;
    public float wormTotalMass = 1f;

    public float[] targetPosX = new float[1];
    public float[] targetPosY = new float[1];
    public float[] targetPosZ = new float[1];
    private Vector3 targetPos;
    public float[] targetDirX = new float[1];
    public float[] targetDirY = new float[1];
    public float[] targetDirZ = new float[1];

    public List<Vector3> targetPositionList;  // will hold the target positions for each gameRound -- re-randomized each generation rather than per-agent
    public List<int> endGameTimesList;  // holds the percentage of the Maximum #timeSteps for each round of play, to create variable testing durations
    public List<Vector3> initialForceList;
    
    // Game Settings:
    public int numberOfSegments = 12; // clean this up!!!
    
    // Debug & Diagnostic:
    public float debugVectorLength = 0.7f;
    public float debugVectorThickness = 0.0125f;
    public float debugCOMradius = 0.075f;
    public Vector3 wormCOM = new Vector3(0f, 0f, 0f);

    // Fitness Component Scores:
    public float[] fitDistFromOrigin = new float[1];
    public float[] fitEnergySpent = new float[1];
    public float[] fitDistToTarget = new float[1];
    public float[] fitTimeInTarget = new float[1];
    public float[] fitMoveToTarget = new float[1];
    public float[] fitMoveSpeed = new float[1];
    public float[] fitCustomTarget = new float[1];
    public float[] fitCustomPole = new float[1];
    public float[] fitCustomBody = new float[1];

    public List<float[]> fitnessCompassSensor1DList;
    public List<float[]> fitnessCompassSensor3DListRight;
    public List<float[]> fitnessCompassSensor3DListUp;
    public List<float[]> fitnessCompassSensor3DListForward;
    public List<float[]> fitnessPositionSensor1DList;
    public List<float[]> fitnessPositionSensor3DList;
    public List<float[]> fitnessPositionSensor3DTargetList;
    public List<float[]> fitnessRotationSensor1DList;
    public List<float[]> fitnessRotationSensor3DList;
    public List<float[]> fitnessVelocitySensor3DList;
    public List<float[]> fitnessRaycastSensorList;
    public List<float[]> fitnessAltimeterList;
    public List<float[]> fitnessGravitySensorList;
    public List<float[]> fitnessContactSensorList;
    public List<float[]> fitnessMouthBasicList;

    public MiniGameCritterWalkBasicSettings customSettings;  // shitty hacky workaround for saving/loading

    // Game Pieces!    
    private GameObject critterBeingTestedGO;
    private GameObject GOgroundPlane;
    private GameObject GOtargetSphere;
    private GameObject GOheadToTargetVector;
    private GameObject GOheadToTargetVectorHor;
    private GameObject GOheadToTargetVectorVer;
    private GameObject GOheadFacingVector;
    private GameObject GOwormCenterOfMass;

    Material targetSphereMat = new Material(Shader.Find("Standard"));    
    Material groundPlaneMat = new Material(Shader.Find("Diffuse"));
    Material headToTargetVecMat = new Material(Shader.Find("Diffuse"));
    Material headToTargetVecHorMat = new Material(Shader.Find("Diffuse"));
    Material headToTargetVecVerMat = new Material(Shader.Find("Diffuse"));
    Material headFacingVecMat = new Material(Shader.Find("Diffuse"));
    Material wormCenterOfMassMat = new Material(Shader.Find("Diffuse"));
    public Material segmentMaterial = new Material(Shader.Find("Diffuse"));

    // Surface Areas for each pair of faces (neg x will be same as pos x): can't initialize because numberOfSegments is also declared in parallel
    private float[] sa_x;
    private float[] sa_y;
    private float[] sa_z;

    private bool preWarm = true;
    
    public MiniGameCritterWalkBasic(CritterGenome templateBodyGenome)
    {
        Debug.Log("MiniGameCritterWalkBasic(CritterGenome templateBodyGenome) CONSTRUCTOR!!!");
        piecesBuilt = false;  // This refers to if the pieces have all their components and are ready for Simulation, not simply instantiated empty GO's
        gameInitialized = false;  // Reset() is Initialization
        gameTicked = false;  // Has the game been ticked on its current TimeStep
        gameUpdatedFromPhysX = false;  // Did the game just get updated from PhysX Simulation?
        gameCurrentTimeStep = 0;  // Should only be able to increment this if the above are true (which means it went through gameLoop for this timeStep)
        preWarm = true;

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //agentBodyBeingTested = templateBodyGenome;
        if (critterBeingTestedGO == null) {
            critterBeingTestedGO = new GameObject("critterGO");
            critterBeingTestedGO.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
            critterBeingTested = critterBeingTestedGO.AddComponent<Critter>();
            critterBeingTested.InitializeCritterFromGenome(templateBodyGenome); // creates segmentList List<> and sets genome            
        }
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                
        targetSphereMat.SetFloat("_Mode", 3.0f);
        //Debug.Log(targetSphereMat.GetFloat("_Mode"));
        targetSphereMat.color = new Color(0.1f, 1f, 0.3f, 0.4f);
        groundPlaneMat.color = new Color(0.75f, 0.75f, 0.75f);
        headToTargetVecMat.color = new Color(1f, 1f, 0.25f);
        headToTargetVecHorMat.color = new Color(0.75f, 0f, 0f);
        headToTargetVecVerMat.color = new Color(0f, 0.75f, 0f);
        headFacingVecMat.color = new Color(0f, 0f, 0.75f);
        wormCenterOfMassMat.color = new Color(0f, 0f, 0f);

        // GAME OPTIONS INIT:
        //gameSettings = new MiniGameCritterWalkBasicSettings();
        //gameSettings.InitGameOptionsList();        
        //InitializeGame();        
    }

    public void UseSettings(MiniGameCritterWalkBasicSettings newSettings) {
        customSettings = (MiniGameCritterWalkBasicSettings)newSettings;
        gameSettings = (MiniGameCritterWalkBasicSettings)newSettings;
    }

    public void InitializeGame() {  // run the first time a miniGame is started to be played, to avoid the UI miniGame instance from being run
        //ResetTargetPositions(0);  // create new List<Vector3>()

        // Build Critter
        critterBeingTested.RebuildCritterFromGenomeRecursive(true, true, customSettings.variableMass[0], false);  //  REVISIT THIS!! UGGGGLLLYYYYYY!!!!
        numberOfSegments = critterBeingTested.critterSegmentList.Count;

        InitializeGameDataArrays();
        SetupInputOutputChannelLists();
        SetupFitnessComponentList();
        //SetupGameOptionsList();
        
        critterBeingTested.DeleteSegments();
    }
    
    private void InitializeGameDataArrays()
    {  // set up arrays based on Agent Genome -- uses templateGenome at Construction

        sa_x = new float[numberOfSegments];
        sa_y = new float[numberOfSegments];
        sa_z = new float[numberOfSegments];
        
        segmentArrayRestRotation = new Quaternion[numberOfSegments]; // relative to parent rotation
        wormSegmentArray_PosX = new float[numberOfSegments][];
        wormSegmentArray_PosY = new float[numberOfSegments][];
        wormSegmentArray_PosZ = new float[numberOfSegments][];
        wormSegmentArray_ScaleX = new float[numberOfSegments][];
        wormSegmentArray_ScaleY = new float[numberOfSegments][];
        wormSegmentArray_ScaleZ = new float[numberOfSegments][];
        wormSegmentArray_Mass = new float[numberOfSegments];

        // For EACH SEGMENT INIT:
        for (int i = 0; i < numberOfSegments; i++)
        {            
            // store bind-pose rotation:
            if (i != 0) {
                //segmentArrayRestRotation[i] = Quaternion.FromToRotation(critterBeingTested.critterSegmentList[i].GetComponent<CritterSegment>().parentSegment.gameObject.transform.forward,
                //                                                   critterBeingTested.critterSegmentList[i].transform.forward);
                segmentArrayRestRotation[i] = Quaternion.Inverse(critterBeingTested.critterSegmentList[i].GetComponent<CritterSegment>().parentSegment.gameObject.transform.rotation) * critterBeingTested.critterSegmentList[i].GetComponent<CritterSegment>().gameObject.transform.rotation;
            }
            wormSegmentArray_PosX[i] = new float[1];
            wormSegmentArray_PosY[i] = new float[1];
            wormSegmentArray_PosZ[i] = new float[1];
            wormSegmentArray_ScaleX[i] = new float[1];
            wormSegmentArray_ScaleY[i] = new float[1];
            wormSegmentArray_ScaleZ[i] = new float[1];
        }
    }

    public void ResetCritter() {
        critterBeingTested.RebuildCritterFromGenomeRecursive(false, false, customSettings.variableMass[0], false); // builds critter     and segment addons  
        critterBeingTested.RebuildCritterFromGenomeRecursive(true, true, customSettings.variableMass[0], false); // builds critter     and segment addons   
         
        //Debug.Log("ResetCritter() " + critterBeingTested.inputChannelsList.Count.ToString());
    }
    
    public override void ResetTargetPositions(int numRounds, int numTimeStepsMin, int numTimeStepsMax) {
        float ratio = (float)numTimeStepsMin / (float)numTimeStepsMax;

        if(targetPositionList == null) {
            targetPositionList = new List<Vector3>();
        }
        else {
            targetPositionList.Clear();
        }
        if(endGameTimesList == null) {
            endGameTimesList = new List<int>();
        }
        else {
            endGameTimesList.Clear();
        }
        if(initialForceList == null) {
            initialForceList = new List<Vector3>();
        }
        else {
            initialForceList.Clear();
        }

        for(int i = 0; i < numRounds; i++) {            
            // Using maxScoreDistance as an overall multiplier
            // Using minScoreDistance as a cap to prevent target from being too close
            float randX = customSettings.maxScoreDistance[0] * UnityEngine.Random.Range(Mathf.Min(customSettings.minTargetX[0], customSettings.maxTargetX[0]), Mathf.Max(customSettings.minTargetX[0], customSettings.maxTargetX[0]));
            float randY = customSettings.maxScoreDistance[0] * UnityEngine.Random.Range(Mathf.Min(customSettings.minTargetY[0], customSettings.maxTargetY[0]), Mathf.Max(customSettings.minTargetY[0], customSettings.maxTargetY[0]));
            float randZ = customSettings.maxScoreDistance[0] * UnityEngine.Random.Range(Mathf.Min(customSettings.minTargetZ[0], customSettings.maxTargetZ[0]), Mathf.Max(customSettings.minTargetZ[0], customSettings.maxTargetZ[0]));
            
            Vector3 randDir = new Vector3(randX, randY, randZ);
            float randMag = randDir.magnitude;
            randDir.Normalize();
            if(randMag < customSettings.minScoreDistance[0]) {
                targetPos = new Vector3(randDir.x * customSettings.minScoreDistance[0], randDir.y * customSettings.minScoreDistance[0], randDir.z * customSettings.minScoreDistance[0]);
            }
            else {
                targetPos = new Vector3(randX, randY, randZ);
            }
            
            targetPositionList.Add(targetPos);

            int randEndTime = Mathf.RoundToInt(UnityEngine.Random.Range(ratio, 1f) * numTimeStepsMax); // gameplay round will end after this percentage of maxTimeSteps
            endGameTimesList.Add(randEndTime);

            Vector3 forceDir = UnityEngine.Random.onUnitSphere;
            float forceMag = UnityEngine.Random.Range(customSettings.initForceMin[0], customSettings.initForceMax[0]);
            initialForceList.Add(new Vector3(forceDir.x * forceMag, forceDir.y * forceMag, forceDir.z * forceMag));
        }  
        
    }

    public override void Reset()
    {
        // Delete Critter Segments
        // Build Critter
        critterBeingTested.masterCritterGenome = agentBodyGenomeBeingTested;        
        ResetCritter();
        numberOfSegments = critterBeingTested.critterSegmentList.Count;
        InitializeGameDataArrays();
        // Update channel Lists:
        //Debug.Log("Reset() pre: " + critterBeingTested.inputChannelsList.Count.ToString());
        SetupInputOutputChannelLists();
        //Debug.Log("Reset() post: " + critterBeingTested.inputChannelsList.Count.ToString());

        //Debug.Log("Reset " + gameCurrentTimeStep.ToString() + ", angularVelocity: " + critterBeingTested.critterSegmentList[1].GetComponent<Rigidbody>().angularVelocity.ToString());
        Physics.gravity = new Vector3(0f, customSettings.gravityStrength[0], 0f);
                
        ResetFitnessComponentValues(); // Fitness component list currently does not change based on Agent, so no need to regenerate list, only reset values
        
        if(customSettings.useRandomTargetPos[0] < 0.5f) {
            targetPos = targetPositionList[gameCurrentRound];            
        }
        else {
            float randX = customSettings.maxScoreDistance[0] * UnityEngine.Random.Range(Mathf.Min(customSettings.minTargetX[0], customSettings.maxTargetX[0]), Mathf.Max(customSettings.minTargetX[0], customSettings.maxTargetX[0]));
            float randY = customSettings.maxScoreDistance[0] * UnityEngine.Random.Range(Mathf.Min(customSettings.minTargetY[0], customSettings.maxTargetY[0]), Mathf.Max(customSettings.minTargetY[0], customSettings.maxTargetY[0]));
            float randZ = customSettings.maxScoreDistance[0] * UnityEngine.Random.Range(Mathf.Min(customSettings.minTargetZ[0], customSettings.maxTargetZ[0]), Mathf.Max(customSettings.minTargetZ[0], customSettings.maxTargetZ[0]));
            
            Vector3 randDir = new Vector3(randX, randY, randZ);
            float randMag = randDir.magnitude;
            randDir.Normalize();
            if (randMag < customSettings.minScoreDistance[0]) {
                targetPos = new Vector3(randDir.x * customSettings.minScoreDistance[0], randDir.y * customSettings.minScoreDistance[0], randDir.z * customSettings.minScoreDistance[0]);
            }
            else {
                targetPos = new Vector3(randX, randY, randZ);
            }
        }
        targetPosX[0] = targetPos.x;
        targetPosY[0] = targetPos.y;
        targetPosZ[0] = targetPos.z;

        Vector3 avgPos = new Vector3(0f, 0f, 0f);
        wormTotalMass = 0f;

        // Fill data arrays with Critter stats for CritterSEGMENTS:
        for (int i = 0; i < numberOfSegments; i++)
        {  // iterate through every segment
            CritterSegment currentSegment = critterBeingTested.critterSegmentList[i].GetComponent<CritterSegment>();
            // SHARED:
            wormSegmentArray_ScaleX[i][0] = currentSegment.scalingFactor * currentSegment.sourceNode.dimensions.x;
            wormSegmentArray_ScaleY[i][0] = currentSegment.scalingFactor * currentSegment.sourceNode.dimensions.y;
            wormSegmentArray_ScaleZ[i][0] = currentSegment.scalingFactor * currentSegment.sourceNode.dimensions.z;

            // Temporariliy diasbled VARIABLE MASS *****************************************************************************************************
            //wormSegmentArray_Mass[i] = 1f;
            wormSegmentArray_Mass[i] = Mathf.Lerp(1f, wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleY[i][0] * wormSegmentArray_ScaleZ[i][0], customSettings.variableMass[0]);
            wormTotalMass += wormSegmentArray_Mass[i];

            // Modify Surface-Area!
            float xRatio = currentSegment.surfaceArea.x / (wormSegmentArray_ScaleY[i][0] * wormSegmentArray_ScaleZ[i][0] * 2f);
            if (xRatio < 0.25f) {
                // if adjusted surface area is less than % of what it should be, assume there is no overlap
                currentSegment.surfaceArea.x = 0f;
            }
            float yRatio = currentSegment.surfaceArea.y / (wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleZ[i][0] * 2f);
            if (yRatio < 0.25f) {
                // if adjusted surface area is less than % of what it should be, assume there is no overlap
                currentSegment.surfaceArea.y = 0f;
            }
            float zRatio = currentSegment.surfaceArea.z / (wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleY[i][0] * 2f);
            if (zRatio < 0.25f) {
                // if adjusted surface area is less than % of what it should be, assume there is no overlap
                currentSegment.surfaceArea.z = 0f;
            }

            sa_x[i] = currentSegment.surfaceArea.x;
            sa_y[i] = currentSegment.surfaceArea.y;
            sa_z[i] = currentSegment.surfaceArea.z;
            //Debug.Log("surface Area [" + i.ToString() + "]: " + currentSegment.surfaceArea.ToString());
            if (i == 0)
            {  // If Root Node:
                wormSegmentArray_PosX[0][0] = 0f;
                wormSegmentArray_PosY[0][0] = 0f;
                wormSegmentArray_PosZ[0][0] = 0f;
            }
            else
            {  // NOT the ROOT segment:
                wormSegmentArray_PosX[i][0] = currentSegment.gameObject.transform.localPosition.x;
                wormSegmentArray_PosY[i][0] = currentSegment.gameObject.transform.localPosition.y;
                wormSegmentArray_PosZ[i][0] = currentSegment.gameObject.transform.localPosition.z;
            }
        }
        float avgMass = wormTotalMass / numberOfSegments;  //
        wormCOM.x = 0f;
        wormCOM.y = 0f;
        wormCOM.z = 0f;

        wormTotalMass /= avgMass;  // Adjust to center around 1f;
        for (int j = 0; j < numberOfSegments; j++)
        {
            // Finalize Segment Mass Value:
            wormSegmentArray_Mass[j] /= avgMass;  // Center the segment Masses around 1f, to avoid precision errors
            critterBeingTested.critterSegmentList[j].GetComponent<Rigidbody>().mass = wormSegmentArray_Mass[j];

            float shareOfTotalMass = wormSegmentArray_Mass[j] / wormTotalMass;
            wormCOM.x += wormSegmentArray_PosX[j][0] * shareOfTotalMass;  // multiply position by proportional share of total mass
            wormCOM.y += wormSegmentArray_PosY[j][0] * shareOfTotalMass;
            wormCOM.z += wormSegmentArray_PosZ[j][0] * shareOfTotalMass;            
        }
        
        // INITIAL PUSH!!!  (currently only on root segment)
        critterBeingTested.critterSegmentList[0].GetComponent<Rigidbody>().AddForce(initialForceList[gameCurrentRound], ForceMode.Impulse);
        //critterBeingTested.critterSegmentList[4].GetComponent<Rigidbody>().AddForce(initialForceList[gameCurrentRound], ForceMode.Impulse);
        //critterBeingTested.critterSegmentList[6].GetComponent<Rigidbody>().AddForce(initialForceList[gameCurrentRound], ForceMode.Impulse);
        //critterBeingTested.critterSegmentList[7].GetComponent<Rigidbody>().AddForce(initialForceList[gameCurrentRound], ForceMode.Impulse);

        SetNonPhysicsGamePieceTransformsFromData();

        gameEndStateReached = false;
        gameInitialized = true;
        gameTicked = false;
        gameUpdatedFromPhysX = false;
        gameCleared = false;
        waitingForReset = false;
        gameCurrentTimeStep = 0;  // reset to 0
        preWarm = true;
    }

    public override void ClearGame() {
        critterBeingTested.DeleteSegments();
        gameCleared = true;
    }

    public void SetShaderTextures(BrainNetworkVisualizer visualizer) {
        for(int i = 0; i < critterBeingTested.critterSegmentList.Count; i++) {
            //critterBeingTested.critterSegmentList[i].GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0f, 0.5f, 0.1f));
            //Debug.Log("Set Color!");
            critterBeingTested.critterSegmentList[i].GetComponent<MeshRenderer>().material.SetTexture("_NeuronPosTex", visualizer.neuronPositionsTexture);
            //Debug.Log("Set Color!" + visualizer.neuronPositionsTexture.width.ToString());
        }
        //critterBeingTested.critterSegmentMaterial.SetTexture("_NeuronPosTex", networkVisualizer.neuronPositionsTexture);
    }

    public override void Tick()
    {  // Runs the mini-game for a single evaluation step.
       //Debug.Log ("Tick()");
       // THIS IS ALL PRE- PHYS-X!!! ::
       //Debug.Log("Tick-- angularVelocity: " + critterBeingTested.critterSegmentList[1].GetComponent<Rigidbody>().angularVelocity.ToString());
       //clockValue[0] = Mathf.Sin(Time.fixedTime * clockFrequency[0]);

        /*if(gameCurrentTimeStep == 1) {  // initial force // temporary hacky!!!! (for double pole-balancing)
            float randRoll = UnityEngine.Random.Range(0f, 1f);
            float randDir = 1f;
            if(randRoll < 0.5f) {
                randDir *= -1f;
            }
            Vector3 directionOfForce = new Vector3(0f, 0f, randDir);
            float forceMagnitude = 6f;
            Vector3 forceVector = new Vector3(directionOfForce.x * forceMagnitude, directionOfForce.y * forceMagnitude, directionOfForce.z * forceMagnitude);
            critterBeingTested.critterSegmentList[0].GetComponent<Rigidbody>().AddForce(forceVector);
        }*/

        
        // Inputs!!
        // Contact Sensor:
        for (int contactSensorIndex = 0; contactSensorIndex < critterBeingTested.segaddonContactSensorList.Count; contactSensorIndex++) {
            //SegaddonContactSensor contactSensor = critterBeingTested.segaddonContactSensorList[contactSensorIndex];
            // Need to add a script onto the Segment that handles OnCollision calls! maybe put that in the Critter Rebuild() function?            
        }
        // Raycast Sensor:
        for (int raycastSensorIndex = 0; raycastSensorIndex < critterBeingTested.segaddonRaycastSensorList.Count; raycastSensorIndex++) {
            SegaddonRaycastSensor raycastSensor = critterBeingTested.segaddonRaycastSensorList[raycastSensorIndex];
            Vector3 rayDirection = critterBeingTested.critterSegmentList[raycastSensor.segmentID].transform.rotation * raycastSensor.forwardVector[0].normalized;
            RaycastHit raycastHit = new RaycastHit();
            Ray raycast = new Ray(critterBeingTested.critterSegmentList[raycastSensor.segmentID].transform.position, rayDirection);
            bool hit = Physics.Raycast(raycast, out raycastHit, raycastSensor.maxDistance[0]);
            if (hit) {
                raycastSensor.hit[0] = true;
                raycastSensor.distance[0] = raycastHit.distance;
            }
            else {
                raycastSensor.hit[0] = false;
                raycastSensor.distance[0] = raycastSensor.maxDistance[0];  // -1 or maxDistance??
            }
        }
        // Compass Sensor 1D:
        for (int compassSensor1DIndex = 0; compassSensor1DIndex < critterBeingTested.segaddonCompassSensor1DList.Count; compassSensor1DIndex++) {
            SegaddonCompassSensor1D compassSensor1D = critterBeingTested.segaddonCompassSensor1DList[compassSensor1DIndex];
            Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[compassSensor1D.segmentID][0], targetPosY[0] - wormSegmentArray_PosY[compassSensor1D.segmentID][0], targetPosZ[0] - wormSegmentArray_PosZ[compassSensor1D.segmentID][0]).normalized;
            Vector3 compassVector = compassSensor1D.forwardVector[0].normalized;
            compassSensor1D.dotProduct[0] = Vector3.Dot(segmentToTargetVect, critterBeingTested.critterSegmentList[compassSensor1D.segmentID].transform.forward);
            //compassSensor1D.fitnessDotProduct[0] += compassSensor1D.dotProduct[0];
            fitnessCompassSensor1DList[compassSensor1DIndex][0] += compassSensor1D.dotProduct[0];
        }
        // Compass Sensor 3D:
        for (int compassSensor3DIndex = 0; compassSensor3DIndex < critterBeingTested.segaddonCompassSensor3DList.Count; compassSensor3DIndex++) {
            SegaddonCompassSensor3D compassSensor3D = critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex];
            Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[compassSensor3D.segmentID][0], targetPosY[0] - wormSegmentArray_PosY[compassSensor3D.segmentID][0], targetPosZ[0] - wormSegmentArray_PosZ[compassSensor3D.segmentID][0]).normalized;
            //Vector3 compassVectorRight = compassSensor3D.forwardVector[0].normalized;
            //Vector3 compassVectorForward = compassSensor3D.forwardVector[0].normalized;
            //Vector3 compassVectorForward = compassSensor3D.forwardVector[0].normalized;
            compassSensor3D.dotProductRight[0] = Vector3.Dot(segmentToTargetVect, critterBeingTested.critterSegmentList[compassSensor3D.segmentID].transform.right);
            compassSensor3D.dotProductUp[0] = Vector3.Dot(segmentToTargetVect, critterBeingTested.critterSegmentList[compassSensor3D.segmentID].transform.up);
            compassSensor3D.dotProductForward[0] = Vector3.Dot(segmentToTargetVect, critterBeingTested.critterSegmentList[compassSensor3D.segmentID].transform.forward);
            //compassSensor1D.fitnessDotProduct[0] += compassSensor1D.dotProduct[0];
            fitnessCompassSensor3DListRight[compassSensor3DIndex][0] += Mathf.Abs(compassSensor3D.dotProductRight[0]);  // 0 -> 1
            fitnessCompassSensor3DListUp[compassSensor3DIndex][0] += Mathf.Abs(compassSensor3D.dotProductUp[0]);
            fitnessCompassSensor3DListForward[compassSensor3DIndex][0] += compassSensor3D.dotProductForward[0];  // -1 -> 1
        }
        // Position Sensor 1D:
        for (int positionSensor1DIndex = 0; positionSensor1DIndex < critterBeingTested.segaddonPositionSensor1DList.Count; positionSensor1DIndex++) {
            SegaddonPositionSensor1D positionSensor1D = critterBeingTested.segaddonPositionSensor1DList[positionSensor1DIndex];
            Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[positionSensor1D.segmentID][0], targetPosY[0] - wormSegmentArray_PosY[positionSensor1D.segmentID][0], targetPosZ[0] - wormSegmentArray_PosZ[positionSensor1D.segmentID][0]);
            Vector3 forwardVector;
            if(positionSensor1D.relative) {
                forwardVector = critterBeingTested.critterSegmentList[positionSensor1D.segmentID].transform.rotation * positionSensor1D.forwardVector[0].normalized;
            }
            else {
                forwardVector = positionSensor1D.forwardVector[0].normalized;
            }
            //Debug.Log("positionSensor1D relative: " + positionSensor1D.relative.ToString() + ", forVec: " + forwardVector.ToString() + ", quat: " + critterBeingTested.critterSegmentList[positionSensor1D.segmentID].transform.rotation.ToString());
            float dot = Vector3.Dot(segmentToTargetVect, forwardVector) * positionSensor1D.sensitivity;  // should equal magnitude
            positionSensor1D.linearDistance[0] = dot;
            positionSensor1D.fitnessDistance[0] = Mathf.Abs(dot);
        }
        // Position Sensor 3D:
        for (int positionSensor3DIndex = 0; positionSensor3DIndex < critterBeingTested.segaddonPositionSensor3DList.Count; positionSensor3DIndex++) {
            SegaddonPositionSensor3D positionSensor3D = critterBeingTested.segaddonPositionSensor3DList[positionSensor3DIndex];
            //Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[positionSensor3D.segmentID][0], targetPosY[0] - wormSegmentArray_PosY[positionSensor3D.segmentID][0], targetPosZ[0] - wormSegmentArray_PosZ[positionSensor3D.segmentID][0]);
            Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - critterBeingTested.critterSegmentList[positionSensor3D.segmentID].transform.position.x, targetPosY[0] - critterBeingTested.critterSegmentList[positionSensor3D.segmentID].transform.position.y, targetPosZ[0] - critterBeingTested.critterSegmentList[positionSensor3D.segmentID].transform.position.z);
            Vector3 rightVector;
            Vector3 upVector;
            Vector3 forwardVector;
            if (positionSensor3D.relative) {
                rightVector = critterBeingTested.critterSegmentList[positionSensor3D.segmentID].transform.right;
                upVector = critterBeingTested.critterSegmentList[positionSensor3D.segmentID].transform.up;
                forwardVector = critterBeingTested.critterSegmentList[positionSensor3D.segmentID].transform.forward;
            }
            else {
                rightVector = new Vector3(1f, 0f, 0f);
                upVector = new Vector3(0f, 1f, 0f);
                forwardVector = new Vector3(0f, 0f, 1f);
            }
            float dotRight = Vector3.Dot(segmentToTargetVect, rightVector) * positionSensor3D.sensitivity;
            float dotUp = Vector3.Dot(segmentToTargetVect, upVector) * positionSensor3D.sensitivity;
            float dotForward = Vector3.Dot(segmentToTargetVect, forwardVector) * positionSensor3D.sensitivity;
            positionSensor3D.distanceRight[0] = dotRight;
            positionSensor3D.distanceUp[0] = dotUp;
            positionSensor3D.distanceForward[0] = dotForward;
            positionSensor3D.distanceRightDouble[0] = dotRight * 2f;
            positionSensor3D.distanceUpDouble[0] = dotUp * 2f;
            positionSensor3D.distanceForwardDouble[0] = dotForward * 2f;
            positionSensor3D.distanceRightHalf[0] = dotRight * 0.5f;
            positionSensor3D.distanceUpHalf[0] = dotUp * 0.5f;
            positionSensor3D.distanceForwardHalf[0] = dotForward * 0.5f;
            float distance = new Vector3(positionSensor3D.distanceRight[0], positionSensor3D.distanceUp[0], positionSensor3D.distanceForward[0]).magnitude;
            fitnessPositionSensor3DList[positionSensor3DIndex][0] += distance;
            positionSensor3D.fitnessDistance[0] = distance * positionSensor3D.sensitivity;
            positionSensor3D.invDistance[0] = 1f / distance;
            positionSensor3D.angle[0] = Mathf.Acos(dotForward / distance) * (dotUp / Mathf.Abs(dotUp));

            float customTarget = 0f;
            //bool insideTarget = false;
            if (distance < customSettings.targetRadius[0]) {                
                //insideTarget = true;
                customTarget += 5f; // + for being IN target
            }
            else {
                customTarget -= 0.1f; // - for being Outside target (small penalty)
            }
            fitnessPositionSensor3DTargetList[positionSensor3DIndex][0] += customTarget;
            //Debug.Log("Segment " + critterBeingTested.segaddonPositionSensor3DList[positionSensor3DIndex].segmentID.ToString() + " Distance: " + distance.ToString() + ", score: " + fitnessPositionSensor3DTargetList[positionSensor3DIndex][0].ToString());
        }
        // Rotation Sensor 1D:
        for (int rotationSensor1DIndex = 0; rotationSensor1DIndex < critterBeingTested.segaddonRotationSensor1DList.Count; rotationSensor1DIndex++) {
            SegaddonRotationSensor1D rotationSensor1D = critterBeingTested.segaddonRotationSensor1DList[rotationSensor1DIndex];
            //Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[rotationSensor1D.segmentID][0], targetPosY[0] - wormSegmentArray_PosY[rotationSensor1D.segmentID][0], targetPosZ[0] - wormSegmentArray_PosZ[rotationSensor1D.segmentID][0]);
            Vector3 angVel = critterBeingTested.critterSegmentList[rotationSensor1D.segmentID].GetComponent<Rigidbody>().angularVelocity;
            Vector3 axis = critterBeingTested.critterSegmentList[rotationSensor1D.segmentID].transform.rotation * rotationSensor1D.localAxis[0].normalized;  // world space??
            rotationSensor1D.rotationRate[0] = Vector3.Dot(angVel, axis) * rotationSensor1D.sensitivity;
            rotationSensor1D.fitnessRotationRate[0] = Mathf.Abs(rotationSensor1D.rotationRate[0]);
            // Vector3 localAngularVelocity = target.transform.InverseTransformDirection(target.rigidbody.angularVelocity); // use if needed!!
        }
        // Rotation Sensor 3D:
        for (int rotationSensor3DIndex = 0; rotationSensor3DIndex < critterBeingTested.segaddonRotationSensor3DList.Count; rotationSensor3DIndex++) {
            SegaddonRotationSensor3D rotationSensor3D = critterBeingTested.segaddonRotationSensor3DList[rotationSensor3DIndex];
            Vector3 localAngularVelocity = critterBeingTested.critterSegmentList[rotationSensor3D.segmentID].transform.InverseTransformDirection(critterBeingTested.critterSegmentList[rotationSensor3D.segmentID].GetComponent<Rigidbody>().angularVelocity);
            //Vector3 localAngularVelocity = critterBeingTested.critterSegmentList[rotationSensor3D.segmentID].GetComponent<Rigidbody>().angularVelocity;

            rotationSensor3D.rotationRateX[0] = localAngularVelocity.x * rotationSensor3D.sensitivity;
            rotationSensor3D.rotationRateY[0] = localAngularVelocity.y * rotationSensor3D.sensitivity;
            rotationSensor3D.rotationRateZ[0] = localAngularVelocity.z * rotationSensor3D.sensitivity; 
            fitnessRotationSensor3DList[rotationSensor3DIndex][0] += new Vector3(Mathf.Abs(rotationSensor3D.rotationRateX[0]), Mathf.Abs(rotationSensor3D.rotationRateY[0]), Mathf.Abs(rotationSensor3D.rotationRateZ[0])).magnitude;
            //rotationSensor3D.fitnessRotationRate[0] = new Vector3(Mathf.Abs(rotationSensor3D.rotationRateX[0]), Mathf.Abs(rotationSensor3D.rotationRateY[0]), Mathf.Abs(rotationSensor3D.rotationRateZ[0])).magnitude;
            //Debug.Log("rotationSensor3D fitnessRotationRate[0]: " + rotationSensor3D.fitnessRotationRate[0].ToString());
            // Vector3 localAngularVelocity = target.transform.InverseTransformDirection(target.rigidbody.angularVelocity); // use if needed!!
        }
        // Velocity Sensor 1D:
        for (int velocitySensor1DIndex = 0; velocitySensor1DIndex < critterBeingTested.segaddonVelocitySensor1DList.Count; velocitySensor1DIndex++) {
            SegaddonVelocitySensor1D velocitySensor1D = critterBeingTested.segaddonVelocitySensor1DList[velocitySensor1DIndex];
            // Eventually this should take into account target's velocity, but target is currently always stationary!
            Vector3 velocityWorld = critterBeingTested.critterSegmentList[velocitySensor1D.segmentID].GetComponent<Rigidbody>().velocity;
            Vector3 forwardVector;
            if (velocitySensor1D.relative) {
                forwardVector = critterBeingTested.critterSegmentList[velocitySensor1D.segmentID].transform.rotation * velocitySensor1D.forwardVector[0].normalized;
            }
            else {
                forwardVector = velocitySensor1D.forwardVector[0].normalized;
            }
            float dot = Vector3.Dot(velocityWorld, forwardVector) * velocitySensor1D.sensitivity;  // should equal magnitude
            velocitySensor1D.componentVelocity[0] = dot;
        }
        // Velocity Sensor 3D:
        for (int velocitySensor3DIndex = 0; velocitySensor3DIndex < critterBeingTested.segaddonVelocitySensor3DList.Count; velocitySensor3DIndex++) {
            SegaddonVelocitySensor3D velocitySensor3D = critterBeingTested.segaddonVelocitySensor3DList[velocitySensor3DIndex];
            // Eventually this should take into account target's velocity, but target is currently always stationary!
            Vector3 velocityWorld = critterBeingTested.critterSegmentList[velocitySensor3D.segmentID].GetComponent<Rigidbody>().velocity;
            Vector3 rightVector;
            Vector3 upVector;
            Vector3 forwardVector;
            if (velocitySensor3D.relative) {
                rightVector = critterBeingTested.critterSegmentList[velocitySensor3D.segmentID].transform.right;
                upVector = critterBeingTested.critterSegmentList[velocitySensor3D.segmentID].transform.up;
                forwardVector = critterBeingTested.critterSegmentList[velocitySensor3D.segmentID].transform.forward;
            }
            else {
                rightVector = new Vector3(1f, 0f, 0f);
                upVector = new Vector3(0f, 1f, 0f);
                forwardVector = new Vector3(0f, 0f, 1f);
            }
            float dotRight = Vector3.Dot(velocityWorld, rightVector) * velocitySensor3D.sensitivity;
            float dotUp = Vector3.Dot(velocityWorld, upVector) * velocitySensor3D.sensitivity;
            float dotForward = Vector3.Dot(velocityWorld, forwardVector) * velocitySensor3D.sensitivity;
            velocitySensor3D.velocityRight[0] = dotRight;
            velocitySensor3D.velocityUp[0] = dotUp;
            velocitySensor3D.velocityForward[0] = dotForward;

            Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - critterBeingTested.critterSegmentList[velocitySensor3D.segmentID].transform.position.x, targetPosY[0] - critterBeingTested.critterSegmentList[velocitySensor3D.segmentID].transform.position.y, targetPosZ[0] - critterBeingTested.critterSegmentList[velocitySensor3D.segmentID].transform.position.z);

            float distance = segmentToTargetVect.magnitude;
            float vel = Mathf.Min(0.0001f, velocityWorld.sqrMagnitude * 10f);
            float maxScore = 25f;
            float dot = Vector3.Dot(velocityWorld.normalized, segmentToTargetVect.normalized);
            //bool insideTarget = false;
            if (distance < customSettings.targetRadius[0]) {
                //insideTarget = true;
                fitnessVelocitySensor3DList[velocitySensor3DIndex][0] -= vel;
                fitnessVelocitySensor3DList[velocitySensor3DIndex][0] += Mathf.Min(1f/vel, maxScore) * 10f;
            }
            else {
                fitnessVelocitySensor3DList[velocitySensor3DIndex][0] += (vel * dot) * 1f;
            }
            //fitnessVelocitySensor3DList[velocitySensor3DIndex][0] += customTarget;            
        }
        // Altimeter:
        for (int altimeterIndex = 0; altimeterIndex < critterBeingTested.segaddonAltimeterList.Count; altimeterIndex++) {
            SegaddonAltimeter altimeter = critterBeingTested.segaddonAltimeterList[altimeterIndex];
            altimeter.altitude[0] = critterBeingTested.critterSegmentList[altimeter.segmentID].transform.position.y - customSettings.groundPositionY[0];
            fitnessAltimeterList[altimeterIndex][0] += altimeter.altitude[0];        
        }
        // EarBasic:
        for (int earBasicIndex = 0; earBasicIndex < critterBeingTested.segaddonEarBasicList.Count; earBasicIndex++) {
            SegaddonEarBasic earBasic = critterBeingTested.segaddonEarBasicList[earBasicIndex];
            earBasic.activation[0] = 0f * earBasic.sensitivity; //critterBeingTested.critterSegmentList[earBasic.segmentID].transform.position.y - customSettings.groundPositionY[0];            
        }
        // GravitySensor:
        for (int gravitySensorIndex = 0; gravitySensorIndex < critterBeingTested.segaddonGravitySensorList.Count; gravitySensorIndex++) {
            SegaddonGravitySensor gravitySensor = critterBeingTested.segaddonGravitySensorList[gravitySensorIndex];
            gravitySensor.gravityDot[0] = Mathf.Abs(Vector3.Dot(critterBeingTested.critterSegmentList[gravitySensor.segmentID].transform.forward, Physics.gravity.normalized));
            //fitnessAltimeterList[gravitySensorIndex][0] += gravitySensor.altitude[0];
            fitnessGravitySensorList[gravitySensorIndex][0] += gravitySensor.gravityDot[0];
        }
        // Oscillator Input:
        for (int oscillatorInputIndex = 0; oscillatorInputIndex < critterBeingTested.segaddonOscillatorInputList.Count; oscillatorInputIndex++) {
            SegaddonOscillatorInput oscillatorInput = critterBeingTested.segaddonOscillatorInputList[oscillatorInputIndex];
            oscillatorInput.value[0] = Mathf.Sin(Time.fixedTime * oscillatorInput.frequency[0] + oscillatorInput.offset[0]) * oscillatorInput.amplitude[0];
        }
        // Value input is constant
        // Timer Input:
        for (int timerInputIndex = 0; timerInputIndex < critterBeingTested.segaddonTimerInputList.Count; timerInputIndex++) {
            SegaddonTimerInput timerInput = critterBeingTested.segaddonTimerInputList[timerInputIndex];
            timerInput.value[0] = Time.fixedTime;  // This will eventually be useful when transfer function include trigonometric functions!
            // Might want to make the timer start at the beginning of the minigame though?
        }

        // Setup Joint MOTORS:  ..... was created within RebuildCritterFromGenomeRecursive...
        for (int motorIndex = 0; motorIndex < critterBeingTested.segaddonJointMotorList.Count; motorIndex++) {
            SegaddonJointMotor motor = critterBeingTested.segaddonJointMotorList[motorIndex];

            if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>() != null) {
                if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                    JointDrive drive = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularXDrive;
                    drive.positionSpring = motor.motorForce[0] * customSettings.jointMotorForce[0]; // TEMP!
                    drive.positionDamper = motor.motorForce[0] * customSettings.jointMotorForce[0] / 2f; // TEMP!
                    //drive.maximumForce = motor.motorForce[0] * customSettings.jointMotorForce[0];  // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularXDrive = drive;
                    Vector3 targetVel = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity;

                    float prevTarget = targetVel.x;

                    targetVel.x = motor.targetAngularX[0] * customSettings.jointMotorSpeed[0]; // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;

                    //Quaternion targetRot = Quaternion.Euler(motor.targetAngularX[0] * customSettings.jointMotorSpeed[0] * 180f / Mathf.PI, 0f, 0f);
                    //Debug.Log("targetRot: " + targetRot.ToString());
                    //critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetRotation = targetRot;

                    fitEnergySpent[0] += Mathf.Abs(targetVel.x - prevTarget);
                }
                else if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                    JointDrive drive = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularYZDrive;
                    drive.positionSpring = motor.motorForce[0] * customSettings.jointMotorForce[0]; // TEMP!
                    drive.positionDamper = motor.motorForce[0] * customSettings.jointMotorForce[0] / 2f; // TEMP!
                    //drive.maximumForce = motor.motorForce[0] * customSettings.jointMotorForce[0];  // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularYZDrive = drive;
                    Vector3 targetVel = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity;

                    float prevTarget = targetVel.y;

                    targetVel.y = motor.targetAngularY[0] * motor.motorSpeed[0] * customSettings.jointMotorSpeed[0]; // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;

                    fitEnergySpent[0] += Mathf.Abs(targetVel.y - prevTarget);
                }
                else if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                    JointDrive drive = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularYZDrive;
                    drive.positionSpring = motor.motorForce[0] * customSettings.jointMotorForce[0]; // TEMP!
                    drive.positionDamper = motor.motorForce[0] * customSettings.jointMotorForce[0] / 2f; // TEMP!
                    //drive.maximumForce = motor.motorForce[0] * customSettings.jointMotorForce[0];  // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularYZDrive = drive;
                    Vector3 targetVel = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity;

                    float prevTarget = targetVel.z;

                    targetVel.z = motor.targetAngularZ[0] * motor.motorSpeed[0] * customSettings.jointMotorSpeed[0]; // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;

                    fitEnergySpent[0] += Mathf.Abs(targetVel.z - prevTarget);
                }
                else if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                    JointDrive drive = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularXDrive;
                    drive.positionSpring = motor.motorForce[0] * customSettings.jointMotorForce[0]; // TEMP!
                    drive.positionDamper = motor.motorForce[0] * customSettings.jointMotorForce[0] / 2f; // TEMP!
                    //drive.maximumForce = motor.motorForce[0] * customSettings.jointMotorForce[0];  // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularXDrive = drive;
                    Vector3 targetVel = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity;

                    float prevTargetX = targetVel.x;
                    float prevTargetY = targetVel.y;

                    targetVel.x = motor.targetAngularX[0] * motor.motorSpeed[0] * customSettings.jointMotorSpeed[0]; // game option is a global multiplier on force/speed
                    //critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;

                    drive = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularYZDrive;
                    drive.maximumForce = motor.motorForce[0] * customSettings.jointMotorForce[0];  // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().angularYZDrive = drive;
                    //Vector3 targetVel = critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity;
                    targetVel.y = motor.targetAngularY[0] * motor.motorSpeed[0] * customSettings.jointMotorSpeed[0]; // game option is a global multiplier on force/speed
                    critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;

                    fitEnergySpent[0] += Mathf.Abs(targetVel.x - prevTargetX);
                    fitEnergySpent[0] += Mathf.Abs(targetVel.y - prevTargetY);
                }
            }
            else {
                Debug.Log("ERROR no Configurable Joint!");
            }
        }
        // ThrusterEffector1D:
        for (int thrusterEffector1DIndex = 0; thrusterEffector1DIndex < critterBeingTested.segaddonThrusterEffector1DList.Count; thrusterEffector1DIndex++) {
            SegaddonThrusterEffector1D thrusterEffector1D = critterBeingTested.segaddonThrusterEffector1DList[thrusterEffector1DIndex];
            Vector3 directionOfForce = critterBeingTested.critterSegmentList[thrusterEffector1D.segmentID].GetComponent<Rigidbody>().transform.rotation * thrusterEffector1D.forwardVector[0].normalized;
            float forceMagnitude = thrusterEffector1D.maxForce[0] * thrusterEffector1D.throttle[0];
            Vector3 forceVector = new Vector3(directionOfForce.x * forceMagnitude, directionOfForce.y * forceMagnitude, directionOfForce.z * forceMagnitude);
            critterBeingTested.critterSegmentList[thrusterEffector1D.segmentID].GetComponent<Rigidbody>().AddForce(forceVector);
        }
        // ThrusterEffector3D:
        for (int thrusterEffector3DIndex = 0; thrusterEffector3DIndex < critterBeingTested.segaddonThrusterEffector3DList.Count; thrusterEffector3DIndex++) {
            SegaddonThrusterEffector3D thrusterEffector3D = critterBeingTested.segaddonThrusterEffector3DList[thrusterEffector3DIndex];
            Vector3 directionOfForceX = critterBeingTested.critterSegmentList[thrusterEffector3D.segmentID].GetComponent<Rigidbody>().transform.right.normalized;
            float forceMagnitudeX = thrusterEffector3D.maxForce[0] * thrusterEffector3D.throttleX[0];
            Vector3 forceVectorX = new Vector3(directionOfForceX.x * forceMagnitudeX, directionOfForceX.y * forceMagnitudeX, directionOfForceX.z * forceMagnitudeX);
            critterBeingTested.critterSegmentList[thrusterEffector3D.segmentID].GetComponent<Rigidbody>().AddForce(forceVectorX);
            Vector3 directionOfForceY = critterBeingTested.critterSegmentList[thrusterEffector3D.segmentID].GetComponent<Rigidbody>().transform.up.normalized;
            float forceMagnitudeY = thrusterEffector3D.maxForce[0] * thrusterEffector3D.throttleY[0];
            Vector3 forceVectorY = new Vector3(directionOfForceY.x * forceMagnitudeY, directionOfForceY.y * forceMagnitudeY, directionOfForceY.z * forceMagnitudeY);
            critterBeingTested.critterSegmentList[thrusterEffector3D.segmentID].GetComponent<Rigidbody>().AddForce(forceVectorY);
            Vector3 directionOfForceZ = critterBeingTested.critterSegmentList[thrusterEffector3D.segmentID].GetComponent<Rigidbody>().transform.forward.normalized;
            float forceMagnitudeZ = thrusterEffector3D.maxForce[0] * thrusterEffector3D.throttleZ[0];
            Vector3 forceVectorZ = new Vector3(directionOfForceZ.x * forceMagnitudeZ, directionOfForceZ.y * forceMagnitudeZ, directionOfForceZ.z * forceMagnitudeZ);
            critterBeingTested.critterSegmentList[thrusterEffector3D.segmentID].GetComponent<Rigidbody>().AddForce(forceVectorZ);
        }
        // TorqueEffector1D:
        for (int torqueEffector1DIndex = 0; torqueEffector1DIndex < critterBeingTested.segaddonTorqueEffector1DList.Count; torqueEffector1DIndex++) {
            SegaddonTorqueEffector1D torqueEffector1D = critterBeingTested.segaddonTorqueEffector1DList[torqueEffector1DIndex];
            Vector3 directionOfTorque = torqueEffector1D.axis[0].normalized;
            float torqueMagnitude = torqueEffector1D.maxTorque[0] * torqueEffector1D.throttle[0];
            Vector3 torqueVector = new Vector3(directionOfTorque.x * torqueMagnitude, directionOfTorque.y * torqueMagnitude, directionOfTorque.z * torqueMagnitude);
            critterBeingTested.critterSegmentList[torqueEffector1D.segmentID].GetComponent<Rigidbody>().AddRelativeTorque(torqueVector);
        }
        // TorqueEffector3D:
        for (int torqueEffector3DIndex = 0; torqueEffector3DIndex < critterBeingTested.segaddonTorqueEffector3DList.Count; torqueEffector3DIndex++) {
            SegaddonTorqueEffector3D torqueEffector3D = critterBeingTested.segaddonTorqueEffector3DList[torqueEffector3DIndex];
            Vector3 directionOfTorqueX = new Vector3(1f, 0f, 0f);
            float torqueMagnitudeX = torqueEffector3D.maxTorque[0] * torqueEffector3D.throttleX[0];
            Vector3 torqueVectorX = new Vector3(directionOfTorqueX.x * torqueMagnitudeX, directionOfTorqueX.y * torqueMagnitudeX, directionOfTorqueX.z * torqueMagnitudeX);
            critterBeingTested.critterSegmentList[torqueEffector3D.segmentID].GetComponent<Rigidbody>().AddRelativeTorque(torqueVectorX);
            Vector3 directionOfTorqueY = new Vector3(0f, 1f, 0f);
            float torqueMagnitudeY = torqueEffector3D.maxTorque[0] * torqueEffector3D.throttleY[0];
            Vector3 torqueVectorY = new Vector3(directionOfTorqueY.x * torqueMagnitudeY, directionOfTorqueY.y * torqueMagnitudeY, directionOfTorqueY.z * torqueMagnitudeY);
            critterBeingTested.critterSegmentList[torqueEffector3D.segmentID].GetComponent<Rigidbody>().AddRelativeTorque(torqueVectorY);
            Vector3 directionOfTorqueZ = new Vector3(0f, 0f, 1f);
            float torqueMagnitudeZ = torqueEffector3D.maxTorque[0] * torqueEffector3D.throttleZ[0];
            Vector3 torqueVectorZ = new Vector3(directionOfTorqueZ.x * torqueMagnitudeZ, directionOfTorqueZ.y * torqueMagnitudeZ, directionOfTorqueZ.z * torqueMagnitudeZ);
            critterBeingTested.critterSegmentList[torqueEffector3D.segmentID].GetComponent<Rigidbody>().AddRelativeTorque(torqueVectorZ);
        }
        // MouthBasic:
        for (int mouthBasicIndex = 0; mouthBasicIndex < critterBeingTested.segaddonMouthBasicList.Count; mouthBasicIndex++) {
            SegaddonMouthBasic mouthBasic3D = critterBeingTested.segaddonMouthBasicList[mouthBasicIndex];
            //Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[positionSensor3D.segmentID][0], targetPosY[0] - wormSegmentArray_PosY[positionSensor3D.segmentID][0], targetPosZ[0] - wormSegmentArray_PosZ[positionSensor3D.segmentID][0]);
            Vector3 segmentToTargetVect = new Vector3(targetPosX[0] - critterBeingTested.critterSegmentList[mouthBasic3D.segmentID].transform.position.x, targetPosY[0] - critterBeingTested.critterSegmentList[mouthBasic3D.segmentID].transform.position.y, targetPosZ[0] - critterBeingTested.critterSegmentList[mouthBasic3D.segmentID].transform.position.z);
            Vector3 rightVector;
            Vector3 upVector;
            Vector3 forwardVector;
                        
            rightVector = new Vector3(1f, 0f, 0f);
            upVector = new Vector3(0f, 1f, 0f);
            forwardVector = new Vector3(0f, 0f, 1f);
            
            float dotRight = Vector3.Dot(segmentToTargetVect, rightVector);
            float dotUp = Vector3.Dot(segmentToTargetVect, upVector);
            float dotForward = Vector3.Dot(segmentToTargetVect, forwardVector);
            //mouthBasic3D.distanceRight[0] = dotRight;
            //mouthBasic3D.distanceUp[0] = dotUp;
            //mouthBasic3D.distanceForward[0] = dotForward;
            float distance = new Vector3(segmentToTargetVect.x, segmentToTargetVect.y, segmentToTargetVect.z).sqrMagnitude;
            fitnessMouthBasicList[mouthBasicIndex][0] += distance * 1f;

            float customTarget = 0f;
            //bool insideTarget = false;
            if (distance < customSettings.targetRadius[0]) {
                //insideTarget = true;
                customTarget -= 5f; // + for being IN target
            }
            else {
                customTarget = 1f; // - for being Outside target (small penalty)
            }
            fitnessMouthBasicList[mouthBasicIndex][0] += customTarget;
            //fitnessPositionSensor3DTargetList[positionSensor3DIndex][0] += customTarget;
            //Debug.Log("Segment " + critterBeingTested.segaddonPositionSensor3DList[positionSensor3DIndex].segmentID.ToString() + " Distance: " + distance.ToString() + ", score: " + fitnessPositionSensor3DTargetList[positionSensor3DIndex][0].ToString());
        }
        // NoiseMakerBasic:
        for (int noiseMakerBasicIndex = 0; noiseMakerBasicIndex < critterBeingTested.segaddonNoiseMakerBasicList.Count; noiseMakerBasicIndex++) {
            //SegaddonNoiseMakerBasic noiseMakerBasic = critterBeingTested.segaddonNoiseMakerBasicList[noiseMakerBasicIndex];
        }
        // Sticky:
        for (int stickyIndex = 0; stickyIndex < critterBeingTested.segaddonStickyList.Count; stickyIndex++) {
            //SegaddonSticky sticky = critterBeingTested.segaddonStickyList[stickyIndex];
            //critterBeingTested.critterSegmentList[sticky.segmentID].GetComponent<Collider>().material.;
        }
        // WeaponBasic:
        for (int weaponBasicIndex = 0; weaponBasicIndex < critterBeingTested.segaddonWeaponBasicList.Count; weaponBasicIndex++) {
            //SegaddonWeaponBasic weaponBasic = critterBeingTested.segaddonWeaponBasicList[weaponBasicIndex];
        }
        
        // FITNESS COMPONENTS!
        float avgMass = wormTotalMass / numberOfSegments;  //
        Vector3 prevFrameWormCOM = wormCOM;
        wormCOM.x = 0f;
        wormCOM.y = 0f;
        wormCOM.z = 0f;
        
        for (int e = 0; e < numberOfSegments; e++)
        {
            float shareOfTotalMass = wormSegmentArray_Mass[e] / wormTotalMass;
            wormCOM.x += wormSegmentArray_PosX[e][0] * shareOfTotalMass;  // multiply position by proportional share of total mass
            wormCOM.y += wormSegmentArray_PosY[e][0] * shareOfTotalMass;
            wormCOM.z += wormSegmentArray_PosZ[e][0] * shareOfTotalMass;
            
            //fitEnergySpent[0] += Mathf.Abs(wormSegmentArray_MotorTargetX[e][0]) / 3f;
            //fitEnergySpent[0] += Mathf.Abs(wormSegmentArray_MotorTargetY[e][0]) / 3f;
            //fitEnergySpent[0] += Mathf.Abs(wormSegmentArray_MotorTargetZ[e][0]) / 3f;
        }
        Vector3 comVel = wormCOM - prevFrameWormCOM;
        
            

        ArenaCameraController.arenaCameraControllerStatic.focusPosition = wormCOM;
        Vector3 targetDirection = new Vector3(targetPosX[0] - wormCOM.x, targetPosY[0] - wormCOM.y, targetPosZ[0] - wormCOM.z);
        float distToTarget = targetDirection.magnitude;
        //if(preWarm == false) {
        
        //}

        Vector3 headToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[0][0], targetPosY[0] - wormSegmentArray_PosY[0][0], targetPosZ[0] - wormSegmentArray_PosZ[0][0]).normalized;

        

        bool inTarget = false;
        if (distToTarget < customSettings.targetRadius[0])
        {
            fitTimeInTarget[0] += 1f;
            inTarget = true;

            fitCustomTarget[0] += 4f; // + for being IN target
            fitCustomTarget[0] -= comVel.magnitude;  // encourage slower movement inside target
            fitCustomTarget[0] += Vector3.Dot(comVel, targetDirection.normalized);  // move quickly towards target
            //fitCustomTarget[0]
        }
        else
        {
            fitTimeInTarget[0] += 0f;

            fitCustomTarget[0] -= 0.01f; // - for being Outside target (small penalty)
            fitCustomTarget[0] += comVel.magnitude * 0.25f;  // encourage faster movement outside target
            fitCustomTarget[0] += Vector3.Dot(comVel, targetDirection.normalized);  // move quickly towards target
        }

        if (gameCurrentTimeStep > 2) {
            fitMoveToTarget[0] += Vector3.Dot(comVel.normalized, targetDirection.normalized);
            fitMoveSpeed[0] += comVel.magnitude;
            fitDistToTarget[0] += distToTarget;
            fitDistFromOrigin[0] += wormCOM.magnitude;
        }

        if (Mathf.Abs(wormCOM.z) < 6f) {  // can't move more than this
            //fitCustomPole[0] += 1f; 
        }
        else {
            //fitCustomPole[0] -= 250f;
            //gameEndStateReached = true;
        }

        for (int w = 0; w < numberOfSegments; w++) {
            // VISCOSITY!!!!!!!!!!! ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            if (critterBeingTested.critterSegmentList[w].GetComponent<CritterSegment>().broken) {
                fitCustomBody[0] = -1000;
                gameEndStateReached = true;
                agentBodyGenomeBeingTested.degenerate = true;
            }
            if (critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().velocity.magnitude > 30f) {
                fitCustomBody[0] = -1000;
                gameEndStateReached = true;
                Debug.Log("PHYS-X EXPLOSION! velocity.magnitude > 30f: " + gameCurrentTimeStep.ToString() + ", w: " + w.ToString());
                agentBodyGenomeBeingTested.degenerate = true;
            }
            if (gameCurrentTimeStep < 2) {
                if (critterBeingTested.critterSegmentList[w].GetComponent<CritterSegment>().colliding) {
                    fitCustomBody[0] = -1000;
                    gameEndStateReached = true;
                    //Debug.Log("COLLISION! gameCurrentTimeStep: " + gameCurrentTimeStep.ToString() + ", w: " + w.ToString());
                    agentBodyGenomeBeingTested.degenerate = true;
                }
            }
            ApplyViscosityForces(critterBeingTested.critterSegmentList[w], w, customSettings.viscosityDrag[0]);
        }

        //fitCustomTarget[0] += 1f;

        //SetNonPhysicsGamePieceTransformsFromData();  // debug objects that rely on PhysX object positions
        SetNonPhysicsGamePieceTransformsFromData();

        gameTicked = true;
        //gameCurrentTimeStep++;  This is updated in base class function: GameTimeStepCompleted()

        if(gameCurrentTimeStep >= endGameTimesList[gameCurrentRound] ) {
            gameEndStateReached = true;
            //Debug.Log("GameEndStateReached! gameCurrentTimeStep: " + gameCurrentTimeStep.ToString() + " endGameTimesList[gameCurrentRound]: " + endGameTimesList[gameCurrentRound].ToString());
        }
    }

    #region SETUP METHODS:
    private void SetupInputOutputChannelLists()
    {
        // TESTING:
        // Brain Inputs!:
        if (inputChannelsList == null) {
            inputChannelsList = new List<BrainInputChannel>();
        }
        else {
            //inputChannelsList.Clear();
        }
        // Brain Outputs!:		
        if (outputChannelsList == null) {
            outputChannelsList = new List<BrainOutputChannel>();
        }
        else {
            //outputChannelsList.Clear();
        }

        //critterBeingTested.SetInputOutputLists(ref inputChannelsList, ref outputChannelsList);
        inputChannelsList = critterBeingTested.inputChannelsList;
        outputChannelsList = critterBeingTested.outputChannelsList;

        /*
        // Joint ANGLE SENSORS:
        for (int angleSensorIndex = 0; angleSensorIndex < critterBeingTested.segaddonJointAngleSensorList.Count; angleSensorIndex++) {
            SegaddonJointAngleSensor angleSensor = critterBeingTested.segaddonJointAngleSensorList[angleSensorIndex];
            //Debug.Log("angleSensorIndex " + angleSensorIndex.ToString() + ", angleSensor.angleX: " + angleSensor.angleX.ToString());
            if (critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                string inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleX";
                BrainInputChannel BIC_SegmentAngle = new BrainInputChannel(ref angleSensor.angleX, true, inputChannelName);
                inputChannelsList.Add(BIC_SegmentAngle);

                if(angleSensor.measureVel) {
                    inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleVelX";
                    BrainInputChannel BIC_SegmentAngleVel = new BrainInputChannel(ref angleSensor.angleVelX, true, inputChannelName);
                    inputChannelsList.Add(BIC_SegmentAngleVel);
                }                
            }
            else if (critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                string inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleY";
                BrainInputChannel BIC_SegmentAngle = new BrainInputChannel(ref angleSensor.angleY, true, inputChannelName);
                inputChannelsList.Add(BIC_SegmentAngle);

                if (angleSensor.measureVel) {
                    inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleVelY";
                    BrainInputChannel BIC_SegmentAngleVel = new BrainInputChannel(ref angleSensor.angleVelY, true, inputChannelName);
                    inputChannelsList.Add(BIC_SegmentAngleVel);
                }
            }
            else if (critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                string inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleZ";
                BrainInputChannel BIC_SegmentAngle = new BrainInputChannel(ref angleSensor.angleZ, true, inputChannelName);
                inputChannelsList.Add(BIC_SegmentAngle);

                if (angleSensor.measureVel) {
                    inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleVelZ";
                    BrainInputChannel BIC_SegmentAngleVel = new BrainInputChannel(ref angleSensor.angleVelZ, true, inputChannelName);
                    inputChannelsList.Add(BIC_SegmentAngleVel);
                }
            }
            else if (critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                string inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleX";
                BrainInputChannel BIC_SegmentAngleX = new BrainInputChannel(ref angleSensor.angleX, true, inputChannelName);
                inputChannelsList.Add(BIC_SegmentAngleX);

                inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleY";
                BrainInputChannel BIC_SegmentAngleY = new BrainInputChannel(ref angleSensor.angleY, true, inputChannelName);
                inputChannelsList.Add(BIC_SegmentAngleY);

                if (angleSensor.measureVel) {
                    inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleVelX";
                    BrainInputChannel BIC_SegmentAngleVelX = new BrainInputChannel(ref angleSensor.angleVelX, true, inputChannelName);
                    inputChannelsList.Add(BIC_SegmentAngleVelX);

                    inputChannelName = "Segment " + angleSensor.segmentID.ToString() + " AngleVelY";
                    BrainInputChannel BIC_SegmentAngleVelY = new BrainInputChannel(ref angleSensor.angleVelY, true, inputChannelName);
                    inputChannelsList.Add(BIC_SegmentAngleVelY);
                }
            }
        }
        // Contact Sensor:
        for (int contactSensorIndex = 0; contactSensorIndex < critterBeingTested.segaddonContactSensorList.Count; contactSensorIndex++) {
            SegaddonContactSensor contactSensor = critterBeingTested.segaddonContactSensorList[contactSensorIndex];
            string inputChannelName = "Segment " + contactSensor.segmentID.ToString() + " Contact";
            BrainInputChannel BIC_SegmentContact = new BrainInputChannel(ref contactSensor.contactStatus, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentContact);
        }
        // Raycast Sensor:
        for (int raycastSensorIndex = 0; raycastSensorIndex < critterBeingTested.segaddonRaycastSensorList.Count; raycastSensorIndex++) {
            SegaddonRaycastSensor raycastSensor = critterBeingTested.segaddonRaycastSensorList[raycastSensorIndex];
            string inputChannelName = "Segment " + raycastSensor.segmentID.ToString() + " Raycast";
            BrainInputChannel BIC_SegmentRaycast = new BrainInputChannel(ref raycastSensor.distance, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentRaycast);
        }
        // Compass Sensor 1D:
        for (int compassSensor1DIndex = 0; compassSensor1DIndex < critterBeingTested.segaddonCompassSensor1DList.Count; compassSensor1DIndex++) {
            SegaddonCompassSensor1D compassSensor1D = critterBeingTested.segaddonCompassSensor1DList[compassSensor1DIndex];
            string inputChannelName = "Segment " + compassSensor1D.segmentID.ToString() + " Compass1D";
            BrainInputChannel BIC_SegmentCompass1D = new BrainInputChannel(ref compassSensor1D.dotProduct, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentCompass1D);
        }
        // Compass Sensor 3D:
        for (int compassSensor3DIndex = 0; compassSensor3DIndex < critterBeingTested.segaddonCompassSensor3DList.Count; compassSensor3DIndex++) {
            SegaddonCompassSensor3D compassSensor3D = critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex];
            string inputChannelName = "Segment " + compassSensor3D.segmentID.ToString() + " Compass3D Right";
            BrainInputChannel BIC_SegmentCompass3DRight = new BrainInputChannel(ref compassSensor3D.dotProductRight, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentCompass3DRight);
            inputChannelName = "Segment " + compassSensor3D.segmentID.ToString() + " Compass3D Up";
            BrainInputChannel BIC_SegmentCompass3DUp = new BrainInputChannel(ref compassSensor3D.dotProductUp, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentCompass3DUp);
            inputChannelName = "Segment " + compassSensor3D.segmentID.ToString() + " Compass3D Forward";
            BrainInputChannel BIC_SegmentCompass3DForward = new BrainInputChannel(ref compassSensor3D.dotProductForward, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentCompass3DForward);
        }
        // Position Sensor 1D:
        for (int positionSensor1DIndex = 0; positionSensor1DIndex < critterBeingTested.segaddonPositionSensor1DList.Count; positionSensor1DIndex++) {
            SegaddonPositionSensor1D positionSensor1D = critterBeingTested.segaddonPositionSensor1DList[positionSensor1DIndex];
            string inputChannelName = "Segment " + positionSensor1D.segmentID.ToString() + " Position1D";
            BrainInputChannel BIC_SegmentPosition1D = new BrainInputChannel(ref positionSensor1D.linearDistance, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentPosition1D);
        }
        // Position Sensor 3D:
        for (int positionSensor3DIndex = 0; positionSensor3DIndex < critterBeingTested.segaddonPositionSensor3DList.Count; positionSensor3DIndex++) {
            SegaddonPositionSensor3D positionSensor3D = critterBeingTested.segaddonPositionSensor3DList[positionSensor3DIndex];
            string inputChannelName = "Segment " + positionSensor3D.segmentID.ToString() + " Pos3D Distance";
            BrainInputChannel BIC_SegmentPosition3DDist = new BrainInputChannel(ref positionSensor3D.fitnessDistance, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentPosition3DDist);
            inputChannelName = "Segment " + positionSensor3D.segmentID.ToString() + " Pos3D Inv. Distance";
            BrainInputChannel BIC_SegmentPosition3DInvDist = new BrainInputChannel(ref positionSensor3D.invDistance, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentPosition3DInvDist);
            inputChannelName = "Segment " + positionSensor3D.segmentID.ToString() + " Pos3D Angle";
            BrainInputChannel BIC_SegmentPosition3DAngle = new BrainInputChannel(ref positionSensor3D.angle, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentPosition3DAngle);
            //string inputChannelName = "Segment " + positionSensor3D.segmentID.ToString() + " Position3D Right";
            //BrainInputChannel BIC_SegmentPosition3DRight = new BrainInputChannel(ref positionSensor3D.distanceRight, true, inputChannelName);
            //inputChannelsList.Add(BIC_SegmentPosition3DRight);
            inputChannelName = "Segment " + positionSensor3D.segmentID.ToString() + " Position3D Up";
            BrainInputChannel BIC_SegmentPosition3DUp = new BrainInputChannel(ref positionSensor3D.distanceUp, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentPosition3DUp);
            inputChannelName = "Segment " + positionSensor3D.segmentID.ToString() + " Position3D Forward";
            BrainInputChannel BIC_SegmentPosition3DForward = new BrainInputChannel(ref positionSensor3D.distanceForward, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentPosition3DForward);
            
        }        
        // Rotation Sensor 1D:
        for (int rotationSensor1DIndex = 0; rotationSensor1DIndex < critterBeingTested.segaddonRotationSensor1DList.Count; rotationSensor1DIndex++) {
            SegaddonRotationSensor1D rotationSensor1D = critterBeingTested.segaddonRotationSensor1DList[rotationSensor1DIndex];
            string inputChannelName = "Segment " + rotationSensor1D.segmentID.ToString() + " Rotation1D";
            BrainInputChannel BIC_SegmentRotation1D = new BrainInputChannel(ref rotationSensor1D.rotationRate, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentRotation1D);
        }
        // Rotation Sensor 3D:
        for (int rotationSensor3DIndex = 0; rotationSensor3DIndex < critterBeingTested.segaddonRotationSensor3DList.Count; rotationSensor3DIndex++) {
            SegaddonRotationSensor3D rotationSensor3D = critterBeingTested.segaddonRotationSensor3DList[rotationSensor3DIndex];
            string inputChannelName = "Segment " + rotationSensor3D.segmentID.ToString() + " Rotation3D X";
            BrainInputChannel BIC_SegmentRotation3DRight = new BrainInputChannel(ref rotationSensor3D.rotationRateX, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentRotation3DRight);
            inputChannelName = "Segment " + rotationSensor3D.segmentID.ToString() + " Rotation3D Y";
            BrainInputChannel BIC_SegmentRotation3DUp = new BrainInputChannel(ref rotationSensor3D.rotationRateY, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentRotation3DUp);
            inputChannelName = "Segment " + rotationSensor3D.segmentID.ToString() + " Rotation3D Z";
            BrainInputChannel BIC_SegmentRotation3DForward = new BrainInputChannel(ref rotationSensor3D.rotationRateZ, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentRotation3DForward);
        }
        // Velocity Sensor 1D:
        for (int velocitySensor1DIndex = 0; velocitySensor1DIndex < critterBeingTested.segaddonVelocitySensor1DList.Count; velocitySensor1DIndex++) {
            SegaddonVelocitySensor1D velocitySensor1D = critterBeingTested.segaddonVelocitySensor1DList[velocitySensor1DIndex];
            string inputChannelName = "Segment " + velocitySensor1D.segmentID.ToString() + " Velocity1D";
            BrainInputChannel BIC_SegmentVelocity1D = new BrainInputChannel(ref velocitySensor1D.componentVelocity, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentVelocity1D);
        }
        // Velocity Sensor 3D:
        for (int velocitySensor3DIndex = 0; velocitySensor3DIndex < critterBeingTested.segaddonVelocitySensor3DList.Count; velocitySensor3DIndex++) {
            SegaddonVelocitySensor3D velocitySensor3D = critterBeingTested.segaddonVelocitySensor3DList[velocitySensor3DIndex];
            string inputChannelName = "Segment " + velocitySensor3D.segmentID.ToString() + " Velocity3D Right";
            BrainInputChannel BIC_SegmentVelocity3DRight = new BrainInputChannel(ref velocitySensor3D.velocityRight, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentVelocity3DRight);
            inputChannelName = "Segment " + velocitySensor3D.segmentID.ToString() + " Velocity3D Up";
            BrainInputChannel BIC_SegmentVelocity3DUp = new BrainInputChannel(ref velocitySensor3D.velocityUp, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentVelocity3DUp);
            inputChannelName = "Segment " + velocitySensor3D.segmentID.ToString() + " Velocity3D Forward";
            BrainInputChannel BIC_SegmentVelocity3DForward = new BrainInputChannel(ref velocitySensor3D.velocityForward, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentVelocity3DForward);
        }
        // Altimeter:
        for (int altimeterIndex = 0; altimeterIndex < critterBeingTested.segaddonAltimeterList.Count; altimeterIndex++) {
            SegaddonAltimeter altimeter = critterBeingTested.segaddonAltimeterList[altimeterIndex];
            string inputChannelName = "Segment " + altimeter.segmentID.ToString() + " Altimeter";
            BrainInputChannel BIC_SegmentAltimeter = new BrainInputChannel(ref altimeter.altitude, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentAltimeter);
        }
        // EarBasic:
        for (int earBasicIndex = 0; earBasicIndex < critterBeingTested.segaddonEarBasicList.Count; earBasicIndex++) {
            SegaddonEarBasic earBasic = critterBeingTested.segaddonEarBasicList[earBasicIndex];
            string inputChannelName = "Segment " + earBasic.segmentID.ToString() + " EarBasic";
            BrainInputChannel BIC_SegmentEarBasic = new BrainInputChannel(ref earBasic.activation, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentEarBasic);
        }
        // GravitySensor:
        for (int gravitySensorIndex = 0; gravitySensorIndex < critterBeingTested.segaddonGravitySensorList.Count; gravitySensorIndex++) {
            SegaddonGravitySensor gravitySensor = critterBeingTested.segaddonGravitySensorList[gravitySensorIndex];
            string inputChannelName = "Segment " + gravitySensor.segmentID.ToString() + " GravitySensor";
            BrainInputChannel BIC_SegmentGravitySensor = new BrainInputChannel(ref gravitySensor.gravityDot, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentGravitySensor);
        }
        // Oscillator Input:
        for (int oscillatorInputIndex = 0; oscillatorInputIndex < critterBeingTested.segaddonOscillatorInputList.Count; oscillatorInputIndex++) {
            SegaddonOscillatorInput oscillatorInput = critterBeingTested.segaddonOscillatorInputList[oscillatorInputIndex];
            string inputChannelName = "Segment " + oscillatorInput.segmentID.ToString() + " Oscillator Input";
            BrainInputChannel BIC_SegmentOscillatorInput = new BrainInputChannel(ref oscillatorInput.value, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentOscillatorInput);
        }
        // Value Input:
        for (int valueInputIndex = 0; valueInputIndex < critterBeingTested.segaddonValueInputList.Count; valueInputIndex++) {
            SegaddonValueInput valueInput = critterBeingTested.segaddonValueInputList[valueInputIndex];
            string inputChannelName = "Segment " + valueInput.segmentID.ToString() + " Value Input";
            BrainInputChannel BIC_SegmentValueInput = new BrainInputChannel(ref valueInput.value, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentValueInput);
        }
        // Timer Input:
        for (int timerInputIndex = 0; timerInputIndex < critterBeingTested.segaddonTimerInputList.Count; timerInputIndex++) {
            SegaddonTimerInput timerInput = critterBeingTested.segaddonTimerInputList[timerInputIndex];
            string inputChannelName = "Segment " + timerInput.segmentID.ToString() + " Timer Input";
            BrainInputChannel BIC_SegmentTimerInput = new BrainInputChannel(ref timerInput.value, true, inputChannelName);
            inputChannelsList.Add(BIC_SegmentTimerInput);
        }


        // JOINT MOTORS:
        for (int motorIndex = 0; motorIndex < critterBeingTested.segaddonJointMotorList.Count; motorIndex++) {
            SegaddonJointMotor motor = critterBeingTested.segaddonJointMotorList[motorIndex];
            if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeX) {
                string outputChannelName = "Segment " + motor.segmentID.ToString() + " Motor Target X";
                BrainOutputChannel BOC_SegmentAngleVel = new BrainOutputChannel(ref motor.targetAngularX, true, outputChannelName);
                outputChannelsList.Add(BOC_SegmentAngleVel);
            }
            else if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeY) {
                string outputChannelName = "Segment " + motor.segmentID.ToString() + " Motor Target Y";
                BrainOutputChannel BOC_SegmentAngleVel = new BrainOutputChannel(ref motor.targetAngularY, true, outputChannelName);
                outputChannelsList.Add(BOC_SegmentAngleVel);
            }
            else if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.HingeZ) {
                string outputChannelName = "Segment " + motor.segmentID.ToString() + " Motor Target Z";
                BrainOutputChannel BOC_SegmentAngleVel = new BrainOutputChannel(ref motor.targetAngularZ, true, outputChannelName);
                outputChannelsList.Add(BOC_SegmentAngleVel);
            }
            else if (critterBeingTested.critterSegmentList[motor.segmentID].GetComponent<CritterSegment>().sourceNode.jointLink.jointType == CritterJointLink.JointType.DualXY) {
                string outputChannelName = "Segment " + motor.segmentID.ToString() + " Motor Target X";
                BrainOutputChannel BOC_SegmentAngleVelX = new BrainOutputChannel(ref motor.targetAngularX, true, outputChannelName);
                outputChannelsList.Add(BOC_SegmentAngleVelX);

                outputChannelName = "Segment " + motor.segmentID.ToString() + " Motor Target Y";
                BrainOutputChannel BOC_SegmentAngleVelY = new BrainOutputChannel(ref motor.targetAngularY, true, outputChannelName);
                outputChannelsList.Add(BOC_SegmentAngleVelY);
            }
        }
        // Thruster Effector 1D:
        for (int thrusterEffector1DIndex = 0; thrusterEffector1DIndex < critterBeingTested.segaddonThrusterEffector1DList.Count; thrusterEffector1DIndex++) {
            SegaddonThrusterEffector1D thrusterEffector1D = critterBeingTested.segaddonThrusterEffector1DList[thrusterEffector1DIndex];
            string outputChannelName = "Segment " + thrusterEffector1D.segmentID.ToString() + " ThrusterEffector1D";
            BrainOutputChannel BOC_SegmentThrusterEffector1D = new BrainOutputChannel(ref thrusterEffector1D.throttle, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentThrusterEffector1D);
        }
        // Thruster Effector 3D:
        for (int thrusterEffector3DIndex = 0; thrusterEffector3DIndex < critterBeingTested.segaddonThrusterEffector3DList.Count; thrusterEffector3DIndex++) {
            SegaddonThrusterEffector3D thrusterEffector3D = critterBeingTested.segaddonThrusterEffector3DList[thrusterEffector3DIndex];
            string outputChannelName = "Segment " + thrusterEffector3D.segmentID.ToString() + " ThrusterEffector3D X";
            BrainOutputChannel BOC_SegmentThrusterEffector3DX = new BrainOutputChannel(ref thrusterEffector3D.throttleX, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentThrusterEffector3DX);
            outputChannelName = "Segment " + thrusterEffector3D.segmentID.ToString() + " ThrusterEffector3D Y";
            BrainOutputChannel BOC_SegmentThrusterEffector3DY = new BrainOutputChannel(ref thrusterEffector3D.throttleY, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentThrusterEffector3DY);
            outputChannelName = "Segment " + thrusterEffector3D.segmentID.ToString() + " ThrusterEffector3D Z";
            BrainOutputChannel BOC_SegmentThrusterEffector3DZ = new BrainOutputChannel(ref thrusterEffector3D.throttleZ, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentThrusterEffector3DZ);
        }
        // Torque Effector 1D:
        for (int torqueEffector1DIndex = 0; torqueEffector1DIndex < critterBeingTested.segaddonTorqueEffector1DList.Count; torqueEffector1DIndex++) {
            SegaddonTorqueEffector1D torqueEffector1D = critterBeingTested.segaddonTorqueEffector1DList[torqueEffector1DIndex];
            string outputChannelName = "Segment " + torqueEffector1D.segmentID.ToString() + " TorqueEffector1D";
            BrainOutputChannel BOC_SegmentTorqueEffector1D = new BrainOutputChannel(ref torqueEffector1D.throttle, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentTorqueEffector1D);
        }
        // Torque Effector 3D:
        for (int torqueEffector3DIndex = 0; torqueEffector3DIndex < critterBeingTested.segaddonTorqueEffector3DList.Count; torqueEffector3DIndex++) {
            SegaddonTorqueEffector3D torqueEffector3D = critterBeingTested.segaddonTorqueEffector3DList[torqueEffector3DIndex];
            string outputChannelName = "Segment " + torqueEffector3D.segmentID.ToString() + " TorqueEffector3D X";
            BrainOutputChannel BOC_SegmentTorqueEffector3DX = new BrainOutputChannel(ref torqueEffector3D.throttleX, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentTorqueEffector3DX);
            outputChannelName = "Segment " + torqueEffector3D.segmentID.ToString() + " TorqueEffector3D Y";
            BrainOutputChannel BOC_SegmentTorqueEffector3DY = new BrainOutputChannel(ref torqueEffector3D.throttleY, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentTorqueEffector3DY);
            outputChannelName = "Segment " + torqueEffector3D.segmentID.ToString() + " TorqueEffector3D Z";
            BrainOutputChannel BOC_SegmentTorqueEffector3DZ = new BrainOutputChannel(ref torqueEffector3D.throttleZ, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentTorqueEffector3DZ);
        }
        // MouthBasic:
        for (int mouthBasicIndex = 0; mouthBasicIndex < critterBeingTested.segaddonMouthBasicList.Count; mouthBasicIndex++) {
            SegaddonMouthBasic mouthBasic = critterBeingTested.segaddonMouthBasicList[mouthBasicIndex];

            //string inputChannelName = "Segment " + mouthBasic.segmentID.ToString() + " MouthSensor";
            //BrainInputChannel BIC_SegmentMouthBasic = new BrainInputChannel(ref mouthBasic.contactStatus, true, inputChannelName);
            //inputChannelsList.Add(BIC_SegmentMouthBasic);

            //string outputChannelName = "Segment " + mouthBasic.segmentID.ToString() + " MouthBite";
            //BrainOutputChannel BOC_SegmentMouthBasic = new BrainOutputChannel(ref mouthBasic.biteStrength, true, outputChannelName);
            //outputChannelsList.Add(BOC_SegmentMouthBasic);
            
        }
        // NoiseMakerBasic:
        for (int noiseMakerBasicIndex = 0; noiseMakerBasicIndex < critterBeingTested.segaddonNoiseMakerBasicList.Count; noiseMakerBasicIndex++) {
            SegaddonNoiseMakerBasic noiseMakerBasic = critterBeingTested.segaddonNoiseMakerBasicList[noiseMakerBasicIndex];            
            string outputChannelName = "Segment " + noiseMakerBasic.segmentID.ToString() + " NoiseMaker";
            BrainOutputChannel BOC_SegmentNoiseMakerBasic = new BrainOutputChannel(ref noiseMakerBasic.volume, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentNoiseMakerBasic);
        }
        // Sticky:
        for (int stickyIndex = 0; stickyIndex < critterBeingTested.segaddonStickyList.Count; stickyIndex++) {
            SegaddonSticky sticky = critterBeingTested.segaddonStickyList[stickyIndex];
            string outputChannelName = "Segment " + sticky.segmentID.ToString() + " Sticky";
            BrainOutputChannel BOC_SegmentSticky = new BrainOutputChannel(ref sticky.stickiness, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentSticky);
        }
        // WeaponBasic:
        for (int weaponBasicIndex = 0; weaponBasicIndex < critterBeingTested.segaddonWeaponBasicList.Count; weaponBasicIndex++) {
            SegaddonWeaponBasic weaponBasic = critterBeingTested.segaddonWeaponBasicList[weaponBasicIndex];
            string outputChannelName = "Segment " + weaponBasic.segmentID.ToString() + " Weapon";
            BrainOutputChannel BOC_SegmentWeaponBasic = new BrainOutputChannel(ref weaponBasic.strength, true, outputChannelName);
            outputChannelsList.Add(BOC_SegmentWeaponBasic);
        }
        */
        string list = critterBeingTested.inputChannelsList.Count + "Minigame SetupInputOutputChannelLists:\n";
        for(int i = 0; i < inputChannelsList.Count; i++) {
            list += "in# " + i.ToString() + " " + inputChannelsList[i].channelName + "\n";
        }
        for (int i = 0; i < outputChannelsList.Count; i++) {
            list += "out# " + i.ToString() + " " + outputChannelsList[i].channelName + "\n";
        }
        //Debug.Log(list);
    }

    private void SetupFitnessComponentList()
    {
        // Fitness Component List:
        fitnessComponentList = new List<FitnessComponent>();
        FitnessComponent FC_distFromOrigin = new FitnessComponent(ref fitDistFromOrigin, true, true, 1f, 0f, "Distance From Origin", true);
        fitnessComponentList.Add(FC_distFromOrigin); // 0
        FitnessComponent FC_energySpent = new FitnessComponent(ref fitEnergySpent, true, false, 1f, 1f, "Energy Spent", true);
        fitnessComponentList.Add(FC_energySpent); // 1
        FitnessComponent FC_distToTarget = new FitnessComponent(ref fitDistToTarget, true, false, 1f, 1f, "Distance To Target", true);
        fitnessComponentList.Add(FC_distToTarget); // 2
        //FitnessComponent FC_timeToTarget = new FitnessComponent(ref fitTimeInTarget, true, true, 1f, 1f, "Time In Target", true);
        //fitnessComponentList.Add(FC_timeToTarget); // 3
        //FitnessComponent FC_moveToTarget = new FitnessComponent(ref fitMoveToTarget, true, true, 1f, 1f, "Move Towards Target", true);
        //fitnessComponentList.Add(FC_moveToTarget); // 7
        FitnessComponent FC_moveSpeed = new FitnessComponent(ref fitMoveSpeed, true, true, 1f, 0f, "Average Speed", true);
        fitnessComponentList.Add(FC_moveSpeed); // 8
        FitnessComponent FC_customBody = new FitnessComponent(ref fitCustomBody, true, true, 1f, 1f, "Custom Body", false);
        fitnessComponentList.Add(FC_customBody); // 8
        //FitnessComponent FC_customPole = new FitnessComponent(ref fitCustomPole, true, true, 1f, 1f, "Custom Pole", false);
        //fitnessComponentList.Add(FC_customPole); // 8

        fitnessContactSensorList = new List<float[]>();
        for (int contactSensorIndex = 0; contactSensorIndex < critterBeingTested.segaddonContactSensorList.Count; contactSensorIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonContactSensorList[contactSensorIndex].segmentID.ToString() + " Contact";
            float[] fitnessContact = new float[1];
            fitnessContact[0] = 0f;
            fitnessContactSensorList.Add(fitnessContact);
            FitnessComponent FC_segmentContact = new FitnessComponent(ref fitnessContact, true, true, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentContact);
        }
        fitnessRaycastSensorList = new List<float[]>();
        for (int raycastSensorIndex = 0; raycastSensorIndex < critterBeingTested.segaddonRaycastSensorList.Count; raycastSensorIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonRaycastSensorList[raycastSensorIndex].segmentID.ToString() + " Raycast";
            float[] fitnessDistance = new float[1];
            fitnessDistance[0] = 0f;
            fitnessRaycastSensorList.Add(fitnessDistance);
            FitnessComponent FC_segmentRaycast = new FitnessComponent(ref fitnessDistance, true, true, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentRaycast);
        }
        fitnessCompassSensor1DList = new List<float[]>();
        for (int compassSensor1DIndex = 0; compassSensor1DIndex < critterBeingTested.segaddonCompassSensor1DList.Count; compassSensor1DIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonCompassSensor1DList[compassSensor1DIndex].segmentID.ToString() + " Compass1D";
            float[] fitnessDotProduct = new float[1];
            fitnessDotProduct[0] = 0f;
            fitnessCompassSensor1DList.Add(fitnessDotProduct);
            FitnessComponent FC_segmentCompass1D = new FitnessComponent(ref fitnessDotProduct, true, true, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentCompass1D);
        }
        fitnessCompassSensor3DListRight = new List<float[]>();
        fitnessCompassSensor3DListUp = new List<float[]>();
        fitnessCompassSensor3DListForward = new List<float[]>();
        for (int compassSensor3DIndex = 0; compassSensor3DIndex < critterBeingTested.segaddonCompassSensor3DList.Count; compassSensor3DIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex].segmentID.ToString() + " Compass3D Right";
            float[] fitnessDotProductRight = new float[1];
            fitnessDotProductRight[0] = 0f;
            fitnessCompassSensor3DListRight.Add(fitnessDotProductRight);
            FitnessComponent FC_segmentCompass3DRight = new FitnessComponent(ref fitnessDotProductRight, true, false, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentCompass3DRight);

            fitnessComponentName = "Segment " + critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex].segmentID.ToString() + " Compass3D Up";
            float[] fitnessDotProductUp = new float[1];
            fitnessDotProductUp[0] = 0f;
            fitnessCompassSensor3DListUp.Add(fitnessDotProductUp);
            FitnessComponent FC_segmentCompass3DUp = new FitnessComponent(ref fitnessDotProductUp, true, false, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentCompass3DUp);

            fitnessComponentName = "Segment " + critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex].segmentID.ToString() + " Compass3D Forward";
            float[] fitnessDotProductForward = new float[1];
            fitnessDotProductForward[0] = 0f;
            fitnessCompassSensor3DListForward.Add(fitnessDotProductForward);
            FitnessComponent FC_segmentCompass3DForward = new FitnessComponent(ref fitnessDotProductForward, true, true, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentCompass3DForward);
        }
        fitnessPositionSensor1DList = new List<float[]>();
        for(int positionSensor1DIndex = 0; positionSensor1DIndex < critterBeingTested.segaddonPositionSensor1DList.Count; positionSensor1DIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonPositionSensor1DList[positionSensor1DIndex].segmentID.ToString() + " Position1D Distance";
            float[] fitnessDistance = new float[1];
            fitnessDistance[0] = 0f;
            fitnessPositionSensor1DList.Add(fitnessDistance);
            FitnessComponent FC_segmentPosition1DDistance = new FitnessComponent(ref fitnessDistance, true, false, 1f, 0.25f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentPosition1DDistance);
        }
        fitnessPositionSensor3DList = new List<float[]>();
        for (int positionSensor3DIndex = 0; positionSensor3DIndex < critterBeingTested.segaddonPositionSensor3DList.Count; positionSensor3DIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonPositionSensor3DList[positionSensor3DIndex].segmentID.ToString() + " Position3D Distance";
            float[] fitnessDistance = new float[1];
            fitnessDistance[0] = 0f;
            fitnessPositionSensor3DList.Add(fitnessDistance);
            FitnessComponent FC_segmentPosition3DDistance = new FitnessComponent(ref fitnessDistance, true, false, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentPosition3DDistance);
        }
        fitnessPositionSensor3DTargetList = new List<float[]>();
        for (int positionSensor3DTargetIndex = 0; positionSensor3DTargetIndex < critterBeingTested.segaddonPositionSensor3DList.Count; positionSensor3DTargetIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonPositionSensor3DList[positionSensor3DTargetIndex].segmentID.ToString() + " Pos3D Custom Target";
            float[] fitnessTarget = new float[1];
            fitnessTarget[0] = 0f;
            fitnessPositionSensor3DTargetList.Add(fitnessTarget);
            FitnessComponent FC_segmentPosition3DTarget = new FitnessComponent(ref fitnessTarget, true, true, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentPosition3DTarget);
        }
        fitnessRotationSensor1DList = new List<float[]>();
        for (int rotationSensor1DIndex = 0; rotationSensor1DIndex < critterBeingTested.segaddonRotationSensor1DList.Count; rotationSensor1DIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonRotationSensor1DList[rotationSensor1DIndex].segmentID.ToString() + " Rotation1D Rate";
            float[] fitnessRate = new float[1];
            fitnessRate[0] = 0f;
            fitnessRotationSensor1DList.Add(fitnessRate);
            FitnessComponent FC_segmentRotation1D = new FitnessComponent(ref fitnessRate, true, false, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentRotation1D);
        }
        fitnessRotationSensor3DList = new List<float[]>();
        for (int rotationSensor3DIndex = 0; rotationSensor3DIndex < critterBeingTested.segaddonRotationSensor3DList.Count; rotationSensor3DIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonRotationSensor3DList[rotationSensor3DIndex].segmentID.ToString() + " Rotation3D Rate";
            float[] fitnessRate = new float[1];
            fitnessRate[0] = 0f;
            fitnessRotationSensor3DList.Add(fitnessRate);
            //critterBeingTested.segaddonRotationSensor3DList[rotationSensor3DIndex];
            FitnessComponent FC_segmentRotation3D = new FitnessComponent(ref fitnessRate, true, false, 1f, 0.25f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentRotation3D);
        }
        fitnessVelocitySensor3DList = new List<float[]>();
        for(int velocitySensor3DIndex = 0; velocitySensor3DIndex < critterBeingTested.segaddonVelocitySensor3DList.Count; velocitySensor3DIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonVelocitySensor3DList[velocitySensor3DIndex].segmentID.ToString() + " Velocity3D";
            float[] fitnessRate = new float[1];
            fitnessRate[0] = 0f;
            fitnessVelocitySensor3DList.Add(fitnessRate);
            FitnessComponent FC_segmentVelocity3D = new FitnessComponent(ref fitnessRate, true, true, 1f, 0f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentVelocity3D);
        }
        fitnessAltimeterList = new List<float[]>();
        for (int altimeterIndex = 0; altimeterIndex < critterBeingTested.segaddonAltimeterList.Count; altimeterIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonAltimeterList[altimeterIndex].segmentID.ToString() + " Altimeter";
            float[] fitnessAltimeter = new float[1];
            fitnessAltimeter[0] = 0f;
            fitnessAltimeterList.Add(fitnessAltimeter);
            FitnessComponent FC_segmentAltimeter = new FitnessComponent(ref fitnessAltimeter, true, true, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentAltimeter);
        }
        fitnessGravitySensorList = new List<float[]>();
        for (int gravitySensorIndex = 0; gravitySensorIndex < critterBeingTested.segaddonGravitySensorList.Count; gravitySensorIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonGravitySensorList[gravitySensorIndex].segmentID.ToString() + " Gravity";
            float[] fitnessGravitySensor = new float[1];
            fitnessGravitySensor[0] = 0f;
            fitnessGravitySensorList.Add(fitnessGravitySensor);
            FitnessComponent FC_segmentGravitySensor = new FitnessComponent(ref fitnessGravitySensor, true, false, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentGravitySensor);
        }
        fitnessMouthBasicList = new List<float[]>();
        for (int mouthBasicIndex = 0; mouthBasicIndex < critterBeingTested.segaddonMouthBasicList.Count; mouthBasicIndex++) {
            string fitnessComponentName = "Segment " + critterBeingTested.segaddonMouthBasicList[mouthBasicIndex].segmentID.ToString() + " MouthBasic";
            float[] fitnessMouth = new float[1];
            fitnessMouth[0] = 0f;
            fitnessMouthBasicList.Add(fitnessMouth);
            FitnessComponent FC_segmentMouthBasic = new FitnessComponent(ref fitnessMouth, true, false, 1f, 1f, fitnessComponentName, true);
            fitnessComponentList.Add(FC_segmentMouthBasic);
        }

        ResetFitnessComponentValues();
    }

    private void ResetFitnessComponentValues()
    {
        // FITNESS COMPONENTS!:
        fitDistFromOrigin[0] = 0f;
        fitEnergySpent[0] = 0f;
        fitDistToTarget[0] = 0f;
        fitTimeInTarget[0] = 0f;
        fitMoveToTarget[0] = 0f;
        fitMoveSpeed[0] = 0f;
        fitCustomTarget[0] = 0f;
        fitCustomPole[0] = 0f;
        fitCustomBody[0] = 0f;

        for (int contactSensorIndex = 0; contactSensorIndex < critterBeingTested.segaddonContactSensorList.Count; contactSensorIndex++) {
            critterBeingTested.segaddonContactSensorList[contactSensorIndex].fitnessContact[0] = 0f;
            fitnessContactSensorList[contactSensorIndex][0] = 0f;
        }
        for (int raycastSensorIndex = 0; raycastSensorIndex < critterBeingTested.segaddonRaycastSensorList.Count; raycastSensorIndex++) {
            critterBeingTested.segaddonRaycastSensorList[raycastSensorIndex].fitnessDistance[0] = 0f;
            fitnessRaycastSensorList[raycastSensorIndex][0] = 0f;
        }
        for (int compassSensor1DIndex = 0; compassSensor1DIndex < critterBeingTested.segaddonCompassSensor1DList.Count; compassSensor1DIndex++) {
            critterBeingTested.segaddonCompassSensor1DList[compassSensor1DIndex].fitnessDotProduct[0] = 0f;
            fitnessCompassSensor1DList[compassSensor1DIndex][0] = 0f;
        }
        for (int compassSensor3DIndex = 0; compassSensor3DIndex < critterBeingTested.segaddonCompassSensor3DList.Count; compassSensor3DIndex++) {
            critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex].fitnessDotProductRight[0] = 0f;
            critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex].fitnessDotProductUp[0] = 0f;
            critterBeingTested.segaddonCompassSensor3DList[compassSensor3DIndex].fitnessDotProductForward[0] = 0f;
            fitnessCompassSensor3DListRight[compassSensor3DIndex][0] = 0f;
            fitnessCompassSensor3DListUp[compassSensor3DIndex][0] = 0f;
            fitnessCompassSensor3DListForward[compassSensor3DIndex][0] = 0f;
        }
        for (int positionSensor1DIndex = 0; positionSensor1DIndex < critterBeingTested.segaddonPositionSensor1DList.Count; positionSensor1DIndex++) {
            critterBeingTested.segaddonPositionSensor1DList[positionSensor1DIndex].fitnessDistance[0] = 0f;
            fitnessPositionSensor1DList[positionSensor1DIndex][0] = 0f;
        }
        for (int positionSensor3DIndex = 0; positionSensor3DIndex < critterBeingTested.segaddonPositionSensor3DList.Count; positionSensor3DIndex++) {
            critterBeingTested.segaddonPositionSensor3DList[positionSensor3DIndex].fitnessDistance[0] = 0f;
            fitnessPositionSensor3DList[positionSensor3DIndex][0] = 0f;
        }
        for (int positionSensor3DTargetIndex = 0; positionSensor3DTargetIndex < critterBeingTested.segaddonPositionSensor3DList.Count; positionSensor3DTargetIndex++) {
            //critterBeingTested.segaddonPositionSensor3DList[positionSensor3DTargetIndex].fitnessDistance[0] = 0f;
            fitnessPositionSensor3DTargetList[positionSensor3DTargetIndex][0] = 0f;
        }
        for (int rotationSensor1DIndex = 0; rotationSensor1DIndex < critterBeingTested.segaddonRotationSensor1DList.Count; rotationSensor1DIndex++) {
            critterBeingTested.segaddonRotationSensor1DList[rotationSensor1DIndex].fitnessRotationRate[0] = 0f;
            fitnessRotationSensor1DList[rotationSensor1DIndex][0] = 0f;
        }
        for (int rotationSensor3DIndex = 0; rotationSensor3DIndex < critterBeingTested.segaddonRotationSensor3DList.Count; rotationSensor3DIndex++) {
            critterBeingTested.segaddonRotationSensor3DList[rotationSensor3DIndex].fitnessRotationRate[0] = 0f;
            fitnessRotationSensor3DList[rotationSensor3DIndex][0] = 0f;
        }
        for (int velocitySensor3DIndex = 0; velocitySensor3DIndex < critterBeingTested.segaddonVelocitySensor3DList.Count; velocitySensor3DIndex++) {
            //critterBeingTested.segaddonVelocitySensor3DList[velocitySensor3DIndex][0] = 0f;
            fitnessVelocitySensor3DList[velocitySensor3DIndex][0] = 0f;
        }
        for (int altimeterIndex = 0; altimeterIndex < critterBeingTested.segaddonAltimeterList.Count; altimeterIndex++) {
            critterBeingTested.segaddonAltimeterList[altimeterIndex].fitnessAltitude[0] = 0f;
            fitnessAltimeterList[altimeterIndex][0] = 0f;
        }
        for (int gravitySensorIndex = 0; gravitySensorIndex < critterBeingTested.segaddonGravitySensorList.Count; gravitySensorIndex++) {
            //critterBeingTested.segaddonGravitySensorList[gravitySensorIndex].fitnessAltitude[0] = 0f;
            fitnessGravitySensorList[gravitySensorIndex][0] = 0f;
        }
        for (int mouthBasicIndex = 0; mouthBasicIndex < critterBeingTested.segaddonMouthBasicList.Count; mouthBasicIndex++) {            
            fitnessMouthBasicList[mouthBasicIndex][0] = 0f;
        }
    }
    #endregion

    public override void UpdateGameStateFromPhysX()
    {
        // PhysX simulation happened after miniGame Tick(), so before passing gameStateData to brain, need to update gameStateData from the rigidBodies
        // SO that the correct updated input values can be sent to the brain
        for (int w = 0; w < numberOfSegments; w++)
        {
            if (critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>() != null)
            {
                //Debug.Log("UpdateGameStateFromPhysX(): RigidBodPos: (" + critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().position.x.ToString() + ", " + critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().position.y.ToString() + ", " + critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().position.z.ToString() + ")");
                wormSegmentArray_PosX[w][0] = critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().position.x;
                wormSegmentArray_PosY[w][0] = critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().position.y;
                wormSegmentArray_PosZ[w][0] = critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().position.z;                
            }
        }

        for(int angleSensorIndex = 0; angleSensorIndex < critterBeingTested.segaddonJointAngleSensorList.Count; angleSensorIndex++) {
            MeasureJointAngles(angleSensorIndex);
        }
        
        gameUpdatedFromPhysX = true;
        
    }

    private void MeasureJointAngles(int addonIndex)
    {
        SegaddonJointAngleSensor angleSensor = critterBeingTested.segaddonJointAngleSensorList[addonIndex];

        float prevAngleX = angleSensor.angleX[0];
        float prevAngleY = angleSensor.angleY[0];
        float prevAngleZ = angleSensor.angleZ[0];

        Quaternion currentRotation = Quaternion.Inverse(critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().parentSegment.gameObject.transform.rotation) * critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().gameObject.transform.rotation;
        Quaternion DeltaRotation = Quaternion.Inverse(segmentArrayRestRotation[angleSensor.segmentID]) * currentRotation;
        Vector3 eulerAngles = DeltaRotation.eulerAngles;
        //Debug.Log("TimeStep: " + gameCurrentTimeStep.ToString() + " DeltaRotation: ( " + DeltaRotation.ToString() + " )");
        if (eulerAngles.x > 180f)
            eulerAngles.x -= 360f;
        else if (eulerAngles.x < -180f)
            eulerAngles.x += 360f;
        if (eulerAngles.y > 180f)
            eulerAngles.y -= 360f;
        else if (eulerAngles.y < -180f)
            eulerAngles.y += 360f;
        if (eulerAngles.z > 180f)
            eulerAngles.z -= 360f;
        else if (eulerAngles.z < -180f)
            eulerAngles.z += 360f;
        eulerAngles.x /= Mathf.Rad2Deg;
        eulerAngles.y /= Mathf.Rad2Deg;
        eulerAngles.z /= Mathf.Rad2Deg;
        //Debug.Log("TimeStep: " + gameCurrentTimeStep.ToString() + " JointAngles[" + angleSensor.segmentID.ToString() + "]: ( " + eulerAngles.x.ToString() + ", " + eulerAngles.y.ToString() + ", " + eulerAngles.z.ToString() + " )");

        angleSensor.angleX[0] = eulerAngles.x * angleSensor.angleSensitivity[0] * customSettings.angleSensorSensitivity[0];        
        angleSensor.angleY[0] = eulerAngles.y * angleSensor.angleSensitivity[0] * customSettings.angleSensorSensitivity[0];
        angleSensor.angleZ[0] = eulerAngles.z * angleSensor.angleSensitivity[0] * customSettings.angleSensorSensitivity[0];

        // Get angle delta:
        if(angleSensor.measureVel) {
            angleSensor.angleVelX[0] = (angleSensor.angleX[0] - prevAngleX) * 10f;
            angleSensor.angleVelY[0] = (angleSensor.angleY[0] - prevAngleY) * 10f;
            angleSensor.angleVelZ[0] = (angleSensor.angleZ[0] - prevAngleZ) * 10f;
        }

        //Vector3 bindPoseRightVector = segmentArrayRestRotation[angleSensor.segmentID] * critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().parentSegment.gameObject.transform.right;
        //Vector3 bindPoseUpVector = segmentArrayRestRotation[angleSensor.segmentID] * critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().parentSegment.gameObject.transform.up;
        //Vector3 bindPoseForwardVector = segmentArrayRestRotation[angleSensor.segmentID] * critterBeingTested.critterSegmentList[angleSensor.segmentID].GetComponent<CritterSegment>().parentSegment.gameObject.transform.forward;

        //angleSensor.angleX[0] = Mathf.Asin(Vector3.Dot(critterBeingTested.critterSegmentList[angleSensor.segmentID].transform.forward, bindPoseUpVector)) * angleSensor.angleSensitivity[0] * customSettings.angleSensorSensitivity[0];
        //if(Mathf.Abs(angleSensor.angleX[0]) > 75f) {
        //Debug.Log("Pole FELL!!! " + angleSensor.angleX[0].ToString());
        //fitCustomPole[0] -= 500f;
        //gameEndStateReached = true;
        //}
        //angleSensor.angleY[0] = Mathf.Asin(Vector3.Dot(critterBeingTested.critterSegmentList[angleSensor.segmentID].transform.right, bindPoseForwardVector)) * angleSensor.angleSensitivity[0] * customSettings.angleSensorSensitivity[0];
        //angleSensor.angleZ[0] = Mathf.Asin(Vector3.Dot(critterBeingTested.critterSegmentList[angleSensor.segmentID].transform.right, bindPoseUpVector)) * angleSensor.angleSensitivity[0] * customSettings.angleSensorSensitivity[0];

        //   OLD BELOW!!!!!
        //wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(critterBeingTested.critterSegmentList[w].transform.forward, segmentArrayRestUp[w])) * Mathf.Rad2Deg * angleSensorSensitivity[0];
        //wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(critterBeingTested.critterSegmentList[w].transform.right, segmentArrayRestForward[w])) * Mathf.Rad2Deg * angleSensorSensitivity[0];
        //wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(critterBeingTested.critterSegmentList[w].transform.right, segmentArrayRestUp[w])) * Mathf.Rad2Deg * angleSensorSensitivity[0];

        /*
        if (agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos)
        {
            wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
        }
        else if (agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg)
        {
            wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
        }
        else if (agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos)
        {
            wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
        }
        else if (agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg)
        {
            wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
        }
        else if (agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos)
        {
            wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
        }
        else if (agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg)
        {
            wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
            wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
        }
        */
        //Debug.Log ("Segment " + w.ToString() + ", AngleX= " + wormSegmentArray_AngleX[w][0].ToString() + ", AngleY= " + wormSegmentArray_AngleY[w][0].ToString() + ", AngleZ= " + wormSegmentArray_AngleZ[w][0].ToString());

    }

    public override void InstantiateGamePieces()  // CALLED FROM TRAINER script
    {
        //Debug.Log ("MiniGameCreatureSwimBasic InstantiateGamePieces();");
        //GOwormSegments = new GameObject[numberOfSegments]; // USED TO BE IN RESET -> InitializeGameDataArrays();

        /*for (int i = 0; i < numberOfSegments; i++)  // removed because critter segment GO's are created within the miniGame constructor right now
        {
            string name = "GOwormSegment" + i.ToString();
            GOwormSegments[i] = new GameObject(name);
            GOwormSegments[i].gameObject.layer = LayerMask.NameToLayer("worm");
            GOwormSegments[i].transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
            GOwormSegments[i].AddComponent<GamePieceCreatureV1Segment>().InitGamePiece();
            GOwormSegments[i].GetComponent<Rigidbody>().useGravity = true;
            GOwormSegments[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            if (agentBodyBeingTested.creatureBodySegmentGenomeList[i].addOn1 == CreatureBodySegmentGenome.AddOns.ContactSensor || agentBodyBeingTested.creatureBodySegmentGenomeList[i].addOn2 == CreatureBodySegmentGenome.AddOns.ContactSensor)
            {
                GOwormSegments[i].AddComponent<SphereCollider>().isTrigger = true;
            }
            GOwormSegments[i].GetComponent<Renderer>().material = segmentMaterial;
        }*/

        GOtargetSphere = new GameObject("GOtargetSphere");
        GOtargetSphere.transform.localScale = new Vector3(customSettings.targetRadius[0], customSettings.targetRadius[0], customSettings.targetRadius[0]);
        GOtargetSphere.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOtargetSphere.AddComponent<GamePieceCommonSphere>().InitGamePiece();
        GOtargetSphere.GetComponent<Renderer>().material = targetSphereMat;

        GOgroundPlane = new GameObject("GOgroundPlane");
        GOgroundPlane.transform.localScale = new Vector3(250f, 1f, 250f);
        GOgroundPlane.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOgroundPlane.transform.position = new Vector3(0f, customSettings.groundPositionY[0], 0f);
        GOgroundPlane.AddComponent<GamePieceCommonPlane>().InitGamePiece();
        Vector3 colliderCenter = new Vector3(0f, -0.5f, 0f);
        GOgroundPlane.AddComponent<BoxCollider>().center = colliderCenter;
        GOgroundPlane.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
        GOgroundPlane.GetComponent<Renderer>().material = groundPlaneMat;
        //GOgroundPlane.layer = LayerMask.NameToLayer("environment");
        
        /*GOheadToTargetVector = new GameObject("GOheadToTargetVector");
        GOheadToTargetVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, debugVectorLength);
        GOheadToTargetVector.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOheadToTargetVector.AddComponent<GamePieceCommonCube>().InitGamePiece();
        GOheadToTargetVector.GetComponent<Renderer>().material = headToTargetVecMat;

        GOheadToTargetVectorHor = new GameObject("GOheadToTargetVectorHor");
        GOheadToTargetVectorHor.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, debugVectorLength);
        GOheadToTargetVectorHor.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOheadToTargetVectorHor.AddComponent<GamePieceCommonCube>().InitGamePiece();
        GOheadToTargetVectorHor.GetComponent<Renderer>().material = headToTargetVecHorMat;

        GOheadToTargetVectorVer = new GameObject("GOheadToTargetVectorVer");
        GOheadToTargetVectorVer.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, debugVectorLength);
        GOheadToTargetVectorVer.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOheadToTargetVectorVer.AddComponent<GamePieceCommonCube>().InitGamePiece();
        GOheadToTargetVectorVer.GetComponent<Renderer>().material = headToTargetVecVerMat;

        GOheadFacingVector = new GameObject("GOheadFacingVector");
        GOheadFacingVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, debugVectorLength);
        GOheadFacingVector.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOheadFacingVector.AddComponent<GamePieceCommonCube>().InitGamePiece();
        GOheadFacingVector.GetComponent<Renderer>().material = headFacingVecMat;
        */
        
        GOwormCenterOfMass = new GameObject("GOwormCenterOfMass");
        GOwormCenterOfMass.transform.localScale = new Vector3(debugCOMradius, debugCOMradius, debugCOMradius);
        GOwormCenterOfMass.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOwormCenterOfMass.AddComponent<GamePieceCommonSphere>().InitGamePiece();
        GOwormCenterOfMass.GetComponent<Renderer>().material = wormCenterOfMassMat;
    }

    public override void UninstantiateGamePieces()
    {

    }

    public override void EnablePhysicsGamePieceComponents()  // No longer needed?
    {
        //Debug.Log ("BuildGamePieceComponents()");

        //ConfigurableJoint joint = segmentListGO[i].AddComponent<ConfigurableJoint>();
        //joint.connectedBody = segmentListGO[workingNodeList[i].parentID].GetComponent<Rigidbody>();  // Set parent
        //joint.anchor = GetJointAnchor(workingNodeList[i]);
        //joint.connectedAnchor = GetJointConnectedAnchor(workingNodeList[i]); // <-- Might be Unnecessary
        //ConfigureJointSettings(workingNodeList[i], ref joint);

        /*for (int w = 0; w < numberOfSegments; w++)
        {
            //GOwormSegments[w].GetComponent<Rigidbody>().useGravity = false;
            GOwormSegments[w].GetComponent<Rigidbody>().isKinematic = false;  //////////////////////////////////
            GOwormSegments[w].GetComponent<Rigidbody>().mass = wormSegmentArray_Mass[w];
            if (w < (numberOfSegments - 1))
            {  // if not the final 'Tail' segment:

            }
            if (w > 0)
            {  // if not the first root segment:
                ConfigurableJoint configJoint;
                if (GOwormSegments[w].GetComponent<ConfigurableJoint>() == null)
                {
                    configJoint = GOwormSegments[w].AddComponent<ConfigurableJoint>();
                }
                else
                {
                    configJoint = GOwormSegments[w].GetComponent<ConfigurableJoint>();
                }
                configJoint.autoConfigureConnectedAnchor = false;
                //Debug.Log ("EnablePhysicsGamePieceComponents(); id= " + agentBodyBeingTested.creatureBodySegmentGenomeList[w].id.ToString() + ", parentID= " + agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentID.ToString());
                configJoint.connectedBody = GOwormSegments[agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentID].GetComponent<Rigidbody>();  // Set parent
                configJoint.anchor = GetJointAnchor(agentBodyBeingTested.creatureBodySegmentGenomeList[w]);
                configJoint.connectedAnchor = GetJointConnectedAnchor(agentBodyBeingTested.creatureBodySegmentGenomeList[w]); // <-- Might be Unnecessary
                ConfigureJointSettings(agentBodyBeingTested.creatureBodySegmentGenomeList[w], ref configJoint);
            }
        }*/
        piecesBuilt = true;
    }

    public override void DisablePhysicsGamePieceComponents()
    { // See if I can just sleep them?
      //Debug.Log ("DestroyGamePieceComponents()");
        for (int w = 0; w < numberOfSegments; w++)
        {
            if (critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>() != null)
            {
                critterBeingTested.critterSegmentList[w].GetComponent<Rigidbody>().isKinematic = true;
                if (critterBeingTested.critterSegmentList[w].GetComponent<ConfigurableJoint>() != null)
                {
                    critterBeingTested.critterSegmentList[w].GetComponent<ConfigurableJoint>().connectedBody = null;
                }
            }
        }
        piecesBuilt = false;
    }

    public override void SetPhysicsGamePieceTransformsFromData()  // RE-Visit the overall structure of turning on/off and not just letting physX run
    {
        //Debug.Log ("SetPhysicsGamePieceTransformsFromData()");
        for (int w = 0; w < numberOfSegments; w++)
        {
            critterBeingTested.critterSegmentList[w].transform.localScale = new Vector3(wormSegmentArray_ScaleX[w][0], wormSegmentArray_ScaleY[w][0], wormSegmentArray_ScaleZ[w][0]);
            critterBeingTested.critterSegmentList[w].transform.position = new Vector3(wormSegmentArray_PosX[w][0], wormSegmentArray_PosY[w][0], wormSegmentArray_PosZ[w][0]); // RE-EVALUATE!!!
            critterBeingTested.critterSegmentList[w].transform.localRotation = Quaternion.identity;
            //Debug.Log("wormSegmentArray_ScaleX[" + w.ToString() + "][0]: " + wormSegmentArray_ScaleX[w][0].ToString());
        }
    }

    public override void SetNonPhysicsGamePieceTransformsFromData()
    {
        //Vector3 headToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[0][0], targetPosY[0] - wormSegmentArray_PosY[0][0], targetPosZ[0] - wormSegmentArray_PosZ[0][0]);
        // POSITION!
        //GOheadToTargetVector.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + (headToTargetVect / 2f); // halfway between head center and target center
        //GOheadToTargetVectorHor.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.right * (debugVectorLength / 2f);
        //GOheadToTargetVectorVer.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.up * (debugVectorLength / 2f);
        //GOheadFacingVector.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.forward * (debugVectorLength / 2f); ;
        GOwormCenterOfMass.transform.position = wormCOM; // average center of mass
        // ROTATION!
        //GOheadToTargetVector.transform.rotation = Quaternion.LookRotation(headToTargetVect);
        //GOheadToTargetVectorHor.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.right); // Figure this out!
        //GOheadToTargetVectorVer.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.up);
        //GOheadFacingVector.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.forward);  // grab direction from head segment
        // SCALE!
        //float lengthScale = headToTargetVect.magnitude;
        //GOheadToTargetVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, lengthScale);
        
        //GOgroundPlane.transform.localScale = new Vector3(20f, 1f, 20f);
        //GOgroundPlane.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
        GOgroundPlane.transform.position = new Vector3(0f, customSettings.groundPositionY[0], 0f);
        //Vector3 colliderCenter = new Vector3(0f, 0f, 0f);
        //GOgroundPlane.GetComponent<BoxCollider>().center = colliderCenter;

        GOtargetSphere.transform.localPosition = new Vector3(targetPosX[0], targetPosY[0], targetPosZ[0]);
        GOtargetSphere.transform.localScale = new Vector3(customSettings.targetRadius[0], customSettings.targetRadius[0], customSettings.targetRadius[0]);
    }

    private void ApplyViscosityForces(GameObject body, int segmentIndex, float drag)
    {
        Rigidbody rigidBod = body.GetComponent<Rigidbody>();
        // Cache positive axis vectors:
        Vector3 forward = body.transform.forward;
        Vector3 up = body.transform.up;
        Vector3 right = body.transform.right;
        // Find centers of each of box's faces
        Vector3 xpos_face_center = (right * body.transform.localScale.x / 2f) + body.transform.position;
        Vector3 ypos_face_center = (up * body.transform.localScale.y / 2f) + body.transform.position;
        Vector3 zpos_face_center = (forward * body.transform.localScale.z / 2f) + body.transform.position;
        //Vector3 xneg_face_center = -(right * body.transform.localScale.x / 2f) + body.transform.position;
        //Vector3 yneg_face_center = -(up * body.transform.localScale.y / 2f) + body.transform.position;
        //Vector3 zneg_face_center = -(forward * body.transform.localScale.z / 2f) + body.transform.position;

        // FRONT (posZ):
        if(sa_z[segmentIndex] > 0f) {
            Vector3 pointVelPosZ = rigidBod.GetPointVelocity(zpos_face_center); // Get velocity of face's center (doesn't catch torque around center of mass)
            float dotZ = Vector3.Dot(forward, pointVelPosZ);
            Vector3 fluidDragVecPosZ = -forward *    // in the direction opposite the face's normal
                dotZ * Mathf.Abs(dotZ) *   // 
                    sa_z[segmentIndex] * customSettings.viscosityDrag[0];  // multiplied by face's surface area, and user-defined multiplier
            rigidBod.AddForceAtPosition(fluidDragVecPosZ * 2f, body.transform.position);  // Apply force at face's center, in the direction opposite the face normal
        }
        // TOP (posY):
        if(sa_y[segmentIndex] > 0f) {
            Vector3 pointVelPosY = rigidBod.GetPointVelocity(ypos_face_center);
            float dotY = Vector3.Dot(up, pointVelPosY);
            Vector3 fluidDragVecPosY = -up * dotY * Mathf.Abs(dotY) * sa_y[segmentIndex] * customSettings.viscosityDrag[0];
            rigidBod.AddForceAtPosition(fluidDragVecPosY * 2f, body.transform.position);
        }
        // RIGHT (posX):
        if (sa_x[segmentIndex] > 0f) {
            Vector3 pointVelPosX = rigidBod.GetPointVelocity(xpos_face_center);
            float dotX = Vector3.Dot(right, pointVelPosX);
            Vector3 fluidDragVecPosX = -right * dotX * Mathf.Abs(dotX) * sa_x[segmentIndex] * customSettings.viscosityDrag[0];
            rigidBod.AddForceAtPosition(fluidDragVecPosX, body.transform.position);
        }        

        // OLD RIGHT (posX):
        //Vector3 pointVelPosX = rigidBod.GetPointVelocity(xpos_face_center);
        //Vector3 fluidDragVecPosX = -right * Vector3.Dot(right, pointVelPosX) * sa_x[segmentIndex] * customSettings.viscosityDrag[0];
        //rigidBod.AddForceAtPosition(fluidDragVecPosX, xpos_face_center);
    }
    /*
    #region RebuildCreature()

    void ConfigureJointSettings(CreatureBodySegmentGenome node, ref ConfigurableJoint joint)
    {
        /*if(armSegmentBendTwoAxis[0] > 0.5f) {  // if two-axis bend enabled
					if(w % 2 == 0) {
						configJoint.axis = new Vector3(0f, 0f, 1f);
					}
					else {
						
						configJoint.axis = new Vector3(0f, 1f, 0f);
					}
				}
				else {
					configJoint.axis = new Vector3(0f, 0f, 1f);
				}
				configJoint.connectedAnchor = new Vector3(-0.5f, 0f, 0f);
				JointLimits jointLimits = new JointLimits();
				jointLimits.max = armSegmentMaxBend[0];
				jointLimits.min = -armSegmentMaxBend[0];
				configJoint.limits = jointLimits;
				configJoint.useLimits = true;
				configJoint.useMotor = true;
				JointMotor motor = new JointMotor();
				motor.force = jointMotorForce[0];  // needed???
				configJoint.motor = motor;
				*/
        /*        
        if (node.jointType == CreatureBodySegmentGenome.JointType.Fixed)
        { // Fixed Joint
          // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
        }
        else if (node.jointType == CreatureBodySegmentGenome.JointType.HingeX)
        { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(1f, 0f, 0f);
            joint.secondaryAxis = new Vector3(0f, 1f, 0f);
            JointDrive jointDrive = joint.angularXDrive;
            jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularXDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            // Joint Limits:
            SoftJointLimit limitXMin = joint.lowAngularXLimit;
            limitXMin.limit = node.jointLimitsMin.x;
            joint.lowAngularXLimit = limitXMin;
            SoftJointLimit limitXMax = joint.highAngularXLimit;
            limitXMax.limit = node.jointLimitsMax.x;
            joint.highAngularXLimit = limitXMax;
        }
        else if (node.jointType == CreatureBodySegmentGenome.JointType.HingeY)
        { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(1f, 0f, 0f);
            joint.secondaryAxis = new Vector3(0f, 1f, 0f);
            JointDrive jointDrive = joint.angularYZDrive;
            jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularYZDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            // Joint Limits:
            SoftJointLimit limitY = joint.angularYLimit;
            limitY.limit = node.jointLimitsMax.y;
            joint.angularYLimit = limitY;
        }
        else if (node.jointType == CreatureBodySegmentGenome.JointType.HingeZ)
        { // Uni-Axis Hinge Joint
            joint.axis = new Vector3(0f, 1f, 0f);
            joint.secondaryAxis = new Vector3(1f, 0f, 0f);
            JointDrive jointDrive = joint.angularYZDrive;
            jointDrive.mode = JointDriveMode.Velocity;
            jointDrive.positionDamper = 1f;
            jointDrive.positionSpring = 1f;
            joint.angularYZDrive = jointDrive;
            // Lock mobility:
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Limited;
            // Joint Limits:
            SoftJointLimit limitZ = joint.angularZLimit;
            limitZ.limit = node.jointLimitsMax.z;
            joint.angularZLimit = limitZ;
        }
    }

    private Vector3 GetJointAnchor(CreatureBodySegmentGenome node)
    {
        Vector3 retVec = new Vector3(0f, 0f, 0f);

        if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg)
        {  // Neg X
            retVec.x = 0.5f;
            retVec.y = node.attachPointChild.y;
            retVec.z = node.attachPointChild.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg)
        {  // Neg Y
            retVec.x = node.attachPointChild.x;
            retVec.y = 0.5f;
            retVec.z = node.attachPointChild.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg)
        { // Neg Z
            retVec.x = node.attachPointChild.x;
            retVec.y = node.attachPointChild.y;
            retVec.z = 0.5f;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos)
        {  // Pos X
            retVec.x = -0.5f;
            retVec.y = node.attachPointChild.y;
            retVec.z = node.attachPointChild.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos)
        {  // Pos Y
            retVec.x = node.attachPointChild.x;
            retVec.y = -0.5f;
            retVec.z = node.attachPointChild.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos)
        {  // Pos Z
            retVec.x = node.attachPointChild.x;
            retVec.y = node.attachPointChild.y;
            retVec.z = -0.5f;
        }

        return retVec;
    }

    private Vector3 GetJointConnectedAnchor(CreatureBodySegmentGenome node)
    {
        Vector3 retVec = new Vector3(0f, 0f, 0f);

        if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg)
        {  // Neg X
            retVec.x = -0.5f;
            retVec.y = node.attachPointParent.y;
            retVec.z = node.attachPointParent.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg)
        {  // Neg Y
            retVec.x = node.attachPointParent.x;
            retVec.y = -0.5f;
            retVec.z = node.attachPointParent.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg)
        { // Neg Z
            retVec.x = node.attachPointParent.x;
            retVec.y = node.attachPointParent.y;
            retVec.z = -0.5f;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos)
        {  // Pos X
            retVec.x = 0.5f;
            retVec.y = node.attachPointParent.y;
            retVec.z = node.attachPointParent.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos)
        {  // Pos Y
            retVec.x = node.attachPointParent.x;
            retVec.y = 0.5f;
            retVec.z = node.attachPointParent.z;
        }
        else if (node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos)
        {  // Pos Z
            retVec.x = node.attachPointParent.x;
            retVec.y = node.attachPointParent.y;
            retVec.z = 0.5f;
        }

        return retVec;
    }

    private Vector3 GetBlockPosition(Vector3 parentCenter, Vector3 parentSize, Vector3 attachParent, Vector3 attachChild, Vector3 ownSize, CreatureBodySegmentGenome.ParentAttachSide attachAxis)
    {

        float xOffset = parentCenter.x;
        float yOffset = parentCenter.y;
        float zOffset = parentCenter.z;


        if (attachAxis == CreatureBodySegmentGenome.ParentAttachSide.xNeg)
        {  // Neg X
           // Adjusting Offset For Connection Axis, each segment is centered on the other
            xOffset = parentCenter.x - ((parentSize.x / 2f) + (ownSize.x / 2f));
            // Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
            yOffset += attachParent.y * parentSize.y;
            zOffset += attachParent.z * parentSize.z;
            // Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
            yOffset -= attachChild.y * ownSize.y;
            zOffset -= attachChild.z * ownSize.z;
        }
        else if (attachAxis == CreatureBodySegmentGenome.ParentAttachSide.yNeg)
        {  // Neg Y
            yOffset = parentCenter.y - ((parentSize.y / 2f) + (ownSize.y / 2f));
            // Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
            xOffset += attachParent.x * parentSize.x;
            zOffset += attachParent.z * parentSize.z;
            // Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
            xOffset -= attachChild.x * ownSize.x;
            zOffset -= attachChild.z * ownSize.z;
        }
        else if (attachAxis == CreatureBodySegmentGenome.ParentAttachSide.zNeg)
        { // Neg Z
            zOffset = parentCenter.z - ((parentSize.z / 2f) + (ownSize.z / 2f));
            // Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
            xOffset += attachParent.x * parentSize.x;
            yOffset += attachParent.y * parentSize.y;
            // Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
            xOffset -= attachChild.x * ownSize.x;
            yOffset -= attachChild.y * ownSize.y;
        }
        else if (attachAxis == CreatureBodySegmentGenome.ParentAttachSide.xPos)
        {  // Pos X
            xOffset = parentCenter.x + ((parentSize.x / 2f) + (ownSize.x / 2f));
            // Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
            yOffset += attachParent.y * parentSize.y;
            zOffset += attachParent.z * parentSize.z;
            // Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
            yOffset -= attachChild.y * ownSize.y;
            zOffset -= attachChild.z * ownSize.z;
        }
        else if (attachAxis == CreatureBodySegmentGenome.ParentAttachSide.yPos)
        {  // Pos Y
            yOffset = parentCenter.y + ((parentSize.y / 2f) + (ownSize.y / 2f));
            // Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
            xOffset += attachParent.x * parentSize.x;
            zOffset += attachParent.z * parentSize.z;
            // Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
            xOffset -= attachChild.x * ownSize.x;
            zOffset -= attachChild.z * ownSize.z;
        }
        else if (attachAxis == CreatureBodySegmentGenome.ParentAttachSide.zPos)
        {  // Pos Z
            zOffset = parentCenter.z + ((parentSize.z / 2f) + (ownSize.z / 2f));
            // Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
            xOffset += attachParent.x * parentSize.x;
            yOffset += attachParent.y * parentSize.y;
            // Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
            xOffset -= attachChild.x * ownSize.x;
            yOffset -= attachChild.y * ownSize.y;
        }

        Vector3 retVec = new Vector3(xOffset, yOffset, zOffset);
        return retVec;
    }
    #endregion
    */
}
