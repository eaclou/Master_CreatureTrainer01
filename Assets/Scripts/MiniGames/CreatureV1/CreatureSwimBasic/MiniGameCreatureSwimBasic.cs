using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameCreatureSwimBasic : MiniGameBase {

	public static bool debugFunctionCalls = false; // turns debug messages on/off

	public float[][] wormSegmentArray_PosX;
	public float[][] wormSegmentArray_PosY;
	public float[][] wormSegmentArray_PosZ;
	public float[][] wormSegmentArray_AngleX;
	public float[][] wormSegmentArray_MotorTargetX;
	public float[][] wormSegmentArray_AngleY;
	public float[][] wormSegmentArray_MotorTargetY;
	public float[][] wormSegmentArray_AngleZ;
	public float[][] wormSegmentArray_MotorTargetZ;
	public float[][] wormSegmentArray_ScaleX;
	public float[][] wormSegmentArray_ScaleY;
	public float[][] wormSegmentArray_ScaleZ;

	public float[] wormSegmentArray_Mass;
	public float wormTotalMass = 1f;

	public float[] targetPosX = new float[1];
	public float[] targetPosY = new float[1];
	public float[] targetPosZ = new float[1];
	private Vector3 targetPos;
	public float[] targetDirX = new float[1];
	public float[] targetDirY = new float[1];
	public float[] targetDirZ = new float[1];

	public float[] clockValue = new float[1];
	public float[] targetHeadDot = new float[1];
	public float[] targetHeadDotHor = new float[1];
	public float[] targetHeadDotVer = new float[1];

	// Game Settings:
	public float[] clockFrequency = new float[1];
	//public float[] armTotalLength = new float[1];
	//public float[] armSegmentMaxBend = new float[1];
	//public float[] armSegmentBendTwoAxis = new float[1];
	public int numberOfSegments = 12;
	//public float[] wormSegmentThicknessHorMax = new float[1];
	//public float[] wormSegmentThicknessVerMax = new float[1];
	//public float[][] wormSegmentArray_ThicknessHor;
	//public float[][] wormSegmentArray_ThicknessVer;

	// GameOptions:
	public float[] viscosityDrag = new float[1]; //50f;
	public float[] jointMotorForce = new float[1]; //100f;
	public float[] jointMotorSpeed = new float[1]; //500f;
	public float[] variableMass = new float[1]; // 0 means all segments have mass of 1.  // 1 means segment's mass is proportional to its volume
	public float[] targetX = new float[1];
	public float[] targetY = new float[1];
	public float[] targetZ = new float[1];
	public float[] targetPosAxis = new float[1];
	public float[] moveSpeedMaxFit = new float[1];
	public float[] maxScoreDistance = new float[1];
	public float[] targetRadius = new float[1];
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
	public float[] fitTargetHeadDot = new float[1];
	public float[] fitTargetHeadDotHor = new float[1];
	public float[] fitTargetHeadDotVer = new float[1];
	public float[] fitMoveToTarget = new float[1];
	public float[] fitMoveSpeed = new float[1];

	// Game Pieces!
	private GameObject[] GOwormSegments;
	private GameObject GOtargetSphere;
	private GameObject GOheadToTargetVector;
	private GameObject GOheadToTargetVectorHor;
	private GameObject GOheadToTargetVectorVer;
	private GameObject GOheadFacingVector;
	private GameObject GOwormCenterOfMass;

	Material targetSphereMat = new Material (Shader.Find("Diffuse"));
	Material headToTargetVecMat = new Material (Shader.Find("Diffuse"));
	Material headToTargetVecHorMat = new Material (Shader.Find("Diffuse"));
	Material headToTargetVecVerMat = new Material (Shader.Find("Diffuse"));
	Material headFacingVecMat = new Material (Shader.Find("Diffuse"));
	Material wormCenterOfMassMat = new Material (Shader.Find("Diffuse"));
	Material segmentMaterial = new Material (Shader.Find("Diffuse"));
	
	// Surface Areas for each pair of faces (neg x will be same as pos x): can't initialize because numberOfSegments is also declared in parallel
	private float[] sa_x;
	private float[] sa_y;
	private float[] sa_z;

	private bool preWarm = true;

	// Constructor Methods!!
	/*public MiniGameCreatureSwimBasic() {  // DEPRECATED!
		piecesBuilt = false;  // This refers to if the pieces have all their components and are ready for Simulation, not simply instantiated empty GO's
		gameInitialized = false;  // Reset() is Initialization
		gameTicked = false;  // Has the game been ticked on its current TimeStep
		gameUpdatedFromPhysX = false;  // Did the game just get updated from PhysX Simulation?
		gameCurrentTimeStep = 0;  // Should only be able to increment this if the above are true (which means it went through gameLoop for this timeStep)

		targetSphereMat.color = new Color(1f, 1f, 0.25f);
		headToTargetVecMat.color = new Color(1f, 1f, 0.25f);
		headToTargetVecHorMat.color = new Color(0.75f, 0f, 0f);
		headToTargetVecVerMat.color = new Color(0f, 0.75f, 0f);
		headFacingVecMat.color = new Color(0f, 0f, 0.75f);
		wormCenterOfMassMat.color = new Color(0f, 0f, 0f);

		// GAME OPTIONS INIT:
		//clockFrequency[0] = 12f;
		viscosityDrag[0] = 35f;
		jointMotorForce[0] = 600f;
		jointMotorSpeed[0] = 400f;
		variableMass[0] = 1f;
		targetX[0] = 1f;
		targetY[0] = 0f;
		targetZ[0] = 0f;
		targetPosAxis[0] = 1f;
		maxScoreDistance[0] = 3f;
		moveSpeedMaxFit[0] = 0.03f;
		targetRadius[0] = 1f;
		//armTotalLength[0] = 2.0f;
		//armSegmentMaxBend[0] = 45f;
		//armSegmentBendTwoAxis[0] = 1f;
		//wormSegmentThicknessHorMax[0] = 0.4f;
		//wormSegmentThicknessVerMax[0] = 0.1f;

		InitializeGameDataArrays();

		SetupInputOutputChannelLists();
		SetupFitnessComponentList();
		SetupGameOptionsList();
	}*/

	public MiniGameCreatureSwimBasic(CreatureBodyGenome templateBodyGenome) {
		//Debug.Log ("MiniGameCreatureSwimBasic(CreatureBodyGenome templateBodyGenome)");

		piecesBuilt = false;  // This refers to if the pieces have all their components and are ready for Simulation, not simply instantiated empty GO's
		gameInitialized = false;  // Reset() is Initialization
		gameTicked = false;  // Has the game been ticked on its current TimeStep
		gameUpdatedFromPhysX = false;  // Did the game just get updated from PhysX Simulation?
		gameCurrentTimeStep = 0;  // Should only be able to increment this if the above are true (which means it went through gameLoop for this timeStep)
		preWarm = true;

		// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		agentBodyBeingTested = templateBodyGenome;
		numberOfSegments = templateBodyGenome.creatureBodySegmentGenomeList.Count;
		// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

		targetSphereMat.color = new Color(1f, 1f, 0.25f);
		headToTargetVecMat.color = new Color(1f, 1f, 0.25f);
		headToTargetVecHorMat.color = new Color(0.75f, 0f, 0f);
		headToTargetVecVerMat.color = new Color(0f, 0.75f, 0f);
		headFacingVecMat.color = new Color(0f, 0f, 0.75f);
		wormCenterOfMassMat.color = new Color(0f, 0f, 0f);
		
		// GAME OPTIONS INIT:
		clockFrequency[0] = 10f;
		viscosityDrag[0] = 100f;
		jointMotorForce[0] = 600f;
		jointMotorSpeed[0] = 100f;
		variableMass[0] = 0.2f;
		targetX[0] = 1f;
		targetY[0] = 0f;
		targetZ[0] = 0f;
		targetPosAxis[0] = 1f;
		maxScoreDistance[0] = 3f;
		moveSpeedMaxFit[0] = 0.03f;
		targetRadius[0] = 1f;
		//armTotalLength[0] = 2.0f;
		//armSegmentMaxBend[0] = 45f;
		//armSegmentBendTwoAxis[0] = 1f;
		//wormSegmentThicknessHorMax[0] = 0.4f;
		//wormSegmentThicknessVerMax[0] = 0.1f;

		InitializeGameDataArrays();
		
		SetupInputOutputChannelLists();
		SetupFitnessComponentList();
		SetupGameOptionsList();
	}

	private void InitializeGameDataArrays() {  // set up arrays based on Agent Genome -- uses templateGenome at Construction
		//Debug.Log ("MiniGameCreatureSwimBasic InitializeGameDataArrays();");
		targetDirX[0] = 0f;
		targetDirY[0] = 0f;
		targetDirZ[0] = 0f;
		//targetPosAxis[0] = 1f;
		clockValue[0] = 0f;
		
		sa_x = new float[numberOfSegments];
		sa_y = new float[numberOfSegments];
		sa_z = new float[numberOfSegments];
		
		GOwormSegments = new GameObject[numberOfSegments];
		wormSegmentArray_PosX = new float[numberOfSegments][];
		wormSegmentArray_PosY = new float[numberOfSegments][];
		wormSegmentArray_PosZ = new float[numberOfSegments][];
		wormSegmentArray_AngleX = new float[numberOfSegments][];
		wormSegmentArray_MotorTargetX = new float[numberOfSegments][];
		wormSegmentArray_AngleY = new float[numberOfSegments][];
		wormSegmentArray_MotorTargetY = new float[numberOfSegments][];
		wormSegmentArray_AngleZ = new float[numberOfSegments][];
		wormSegmentArray_MotorTargetZ = new float[numberOfSegments][];
		//wormSegmentArray_Length = new float[numberOfSegments][];
		//wormSegmentArray_ThicknessHor = new float[numberOfSegments][];
		//wormSegmentArray_ThicknessVer = new float[numberOfSegments][];
		wormSegmentArray_ScaleX = new float[numberOfSegments][];
		wormSegmentArray_ScaleY = new float[numberOfSegments][];
		wormSegmentArray_ScaleZ = new float[numberOfSegments][];

		wormSegmentArray_Mass = new float[numberOfSegments];
		
		// For EACH SEGMENT INIT:
		for(int i = 0; i < numberOfSegments; i++) {
			//string name = "GOwormSegment" + i.ToString();
			//GOwormSegments[i] = new GameObject(name);
			//GOwormSegments[i].transform.localPosition = new Vector3(0f, 0f, 0f); // RE-EVALUATE!!!
			wormSegmentArray_PosX[i] = new float[1];
			wormSegmentArray_PosY[i] = new float[1];
			wormSegmentArray_PosZ[i] = new float[1];
			wormSegmentArray_AngleX[i] = new float[1];
			wormSegmentArray_MotorTargetX[i] = new float[1];
			wormSegmentArray_AngleY[i] = new float[1];
			wormSegmentArray_MotorTargetY[i] = new float[1];
			wormSegmentArray_AngleZ[i] = new float[1];
			wormSegmentArray_MotorTargetZ[i] = new float[1];
			//wormSegmentArray_Length[i] = new float[1];
			//wormSegmentArray_ThicknessHor[i] = new float[1];
			//wormSegmentArray_ThicknessVer[i] = new float[1];
			wormSegmentArray_ScaleX[i] = new float[1];
			wormSegmentArray_ScaleY[i] = new float[1];
			wormSegmentArray_ScaleZ[i] = new float[1];
			/*
			wormSegmentArray_Length[i][0] = armTotalLength[0]/(float)numberOfSegments;
			float indexRatio = (i+1f) / numberOfSegments;
			wormSegmentArray_ThicknessHor[i][0] = wormSegmentThicknessHorMax[0]; // * Mathf.Pow (indexRatio, 1.4f);
			wormSegmentArray_ThicknessVer[i][0] = wormSegmentThicknessVerMax[0]; // * Mathf.Pow ((1f - indexRatio) + 1f/numberOfSegments, 1.4f);
			// Calculate surface areas for each face:
			sa_x[i] = wormSegmentArray_ThicknessHor[i][0] * wormSegmentArray_ThicknessVer[i][0];  // Y * Z
			sa_y[i] = wormSegmentArray_Length[i][0] * wormSegmentArray_ThicknessHor[i][0];  // X * Z
			sa_z[i] = wormSegmentArray_Length[i][0] * wormSegmentArray_ThicknessVer[i][0]; // X * Y
			*/
		}
	}

	public override void Reset() {
		
		if(agentBodyBeingTested != null) {
			//Debug.Log ("MiniGameCreatureSwimBasic; Reset() Agent: " + agentBodyBeingTested.creatureBodySegmentGenomeList.Count.ToString());
		}
		else {
			//Debug.Log ("MiniGameCreatureSwimBasic; Reset() Agent NULL!!");
		}

		//InitializeGameDataArrays(); // Re-Evaluate when creature topologies can change
		targetDirX[0] = 0f;
		targetDirY[0] = 0f;
		targetDirZ[0] = 0f;
		//targetPosAxis[0] = 0f;
		//clockValue[0] = 0f;
		
		//SetupInputOutputChannelLists();
		ResetFitnessComponentValues(); // Fitness component list currently does not change based on Agent, so no need to regenerate list, only reset values
				
		float randX = UnityEngine.Random.Range(-1f, 1f);
		float randY = UnityEngine.Random.Range(-1f, 1f);
		float randZ = UnityEngine.Random.Range(-1f, 1f);
		float delta = targetPosAxis[0] - 0.5f;
		if(targetX[0] <= 0.5f) {  // if x-axis is enabled, use randX, else set to 0;
			randX = 0f;
		}
		else {
			if(delta > 0.16667f) {
				randX = Mathf.Abs (randX);
			}
			else if(delta < -0.16667f) {
				randX = -Mathf.Abs (randX);
			}
			else { // btw 0.333 and 0.667 ==> use full random

			}
		}
		if(targetY[0] < 0.5f) {  
			randY = 0f;
		}
		else {
			if(delta > 0.16667f) {
				randY = Mathf.Abs (randY);
			}
			else if(delta < -0.16667f) {
				randY = -Mathf.Abs (randY);
			}
		}
		if(targetZ[0] < 0.5f) {  // if x-axis is enabled, use randX, else set to 0;
			randZ = 0f;
		}
		else {
			if(delta > 0.16667f) {
				randZ = Mathf.Abs (randZ);
			}
			else if(delta < -0.16667f) {
				randZ = -Mathf.Abs (randZ);
			}
		}
		
		Vector3 randDir = new Vector3(randX, randY, randZ).normalized;		
		clockValue[0] = 0f;
		targetPos = new Vector3(randDir.x * maxScoreDistance[0], randDir.y * maxScoreDistance[0], randDir.z * maxScoreDistance[0]);
		targetPosX[0] = targetPos.x;
		targetPosY[0] = targetPos.y;
		targetPosZ[0] = targetPos.z;

		//float xOffset = armTotalLength[0] * 0.5f; // Revisit later, make Vector3, = -COM
		//wormSegmentArray_PosX[0][0] = 0f; // origin of arm at (0, 0); // will change later
		//wormSegmentArray_PosY[0][0] = 0f;
		//wormSegmentArray_PosZ[0][0] = 0f;
		
		// Game Pieces!!!!! // maybe put into their own function eventually?
		/*
		for(int w = 0; w < numberOfSegments; w++) {
			// ##### Segment SCALE:
			wormSegmentArray_ScaleX[w] = agentBeingTested.bodyGenome.creatureBodySegmentGenomeList[w].size.x;
			wormSegmentArray_ScaleY[w] = agentBeingTested.bodyGenome.creatureBodySegmentGenomeList[w].size.y;
			wormSegmentArray_ScaleZ[w] = agentBeingTested.bodyGenome.creatureBodySegmentGenomeList[w].size.z;
			//wormSegmentArray_ThicknessHor[w][0] = wormSegmentThicknessHorMax[0]; // * Mathf.Pow (indexRatio, 1.4f);
			//wormSegmentArray_ThicknessVer[w][0] = wormSegmentThicknessVerMax[0];

			//wormSegmentArray_Length[w][0] = armTotalLength[0]/(float)numberOfSegments;
			wormSegmentArray_Angle[w][0] = 0f;
			wormSegmentArray_MotorTarget[w][0] = 0f;
			
			if(w != 0) { // if not the root segment:
				wormSegmentArray_PosX[w][0] = wormSegmentArray_PosX[w-1][0] + wormSegmentArray_Length[w-1][0]*0.5f + wormSegmentArray_Length[w][0]*0.5f; // - (armTotalLength[0] / 2f);
			}
			wormSegmentArray_PosY[w][0] = 0f;
			wormSegmentArray_PosZ[w][0] = 0f;
			
			sa_x[w] = wormSegmentArray_ScaleY[w][0] * wormSegmentArray_ScaleZ[w][0];  // Y * Z
			sa_y[w] = wormSegmentArray_ScaleX[w][0] * wormSegmentArray_ScaleZ[w][0];  // X * Z
			sa_z[w] = wormSegmentArray_ScaleX[w][0] * wormSegmentArray_ScaleY[w][0]; // X * Y
		}
		*/

		Vector3 avgPos = new Vector3(0f, 0f, 0f);
		wormTotalMass = 0f;

		// Create Creature:
		//CreatureBodySegmentGenome parentSegmentNode;
		for(int i = 0; i < numberOfSegments; i++) {  // iterate through every node

			// SHARED:
			wormSegmentArray_ScaleX[i][0] = agentBodyBeingTested.creatureBodySegmentGenomeList[i].size.x;
			wormSegmentArray_ScaleY[i][0] = agentBodyBeingTested.creatureBodySegmentGenomeList[i].size.y;
			wormSegmentArray_ScaleZ[i][0] = agentBodyBeingTested.creatureBodySegmentGenomeList[i].size.z;

			wormSegmentArray_Mass[i] = Mathf.Lerp (1f, wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleY[i][0] * wormSegmentArray_ScaleZ[i][0], variableMass[0]);
			wormTotalMass += wormSegmentArray_Mass[i];

			if(i == 0) {  // If Root Node:
				wormSegmentArray_PosX[0][0] = 0f;
				wormSegmentArray_PosY[0][0] = 0f;
				wormSegmentArray_PosZ[0][0] = 0f;
				sa_x[i] = wormSegmentArray_ScaleY[i][0] * wormSegmentArray_ScaleZ[i][0];  // Y * Z
				sa_y[i] = wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleZ[i][0];  // X * Z
				sa_z[i] = wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleY[i][0]; // X * Y
			}
			else {  // NOT the ROOT segment:
				// FIGURE OUT BIND POSITION:
				int parentID = agentBodyBeingTested.creatureBodySegmentGenomeList[i].parentID;
				Vector3 parentPos = new Vector3(wormSegmentArray_PosX[parentID][0],
				                                wormSegmentArray_PosY[parentID][0],
				                                wormSegmentArray_PosZ[parentID][0]);
				Vector3 parentSize = new Vector3(wormSegmentArray_ScaleX[parentID][0],
				                                 wormSegmentArray_ScaleY[parentID][0],
				                                 wormSegmentArray_ScaleZ[parentID][0]);
				Vector3 centerPos = GetBlockPosition(parentPos, 
				                                     parentSize, 
				                                     agentBodyBeingTested.creatureBodySegmentGenomeList[i].attachPointParent, 
				                                     agentBodyBeingTested.creatureBodySegmentGenomeList[i].attachPointChild, 
				                                     agentBodyBeingTested.creatureBodySegmentGenomeList[i].size, 
				                                     agentBodyBeingTested.creatureBodySegmentGenomeList[i].parentAttachSide);
				Vector3 ownSize = agentBodyBeingTested.creatureBodySegmentGenomeList[i].size;
				// Subtract SurfaceArea for shared sides:
				sa_x[i] = wormSegmentArray_ScaleY[i][0] * wormSegmentArray_ScaleZ[i][0];  // Y * Z
				sa_y[i] = wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleZ[i][0];  // X * Z
				sa_z[i] = wormSegmentArray_ScaleX[i][0] * wormSegmentArray_ScaleY[i][0]; // X * Y
				CreatureBodySegmentGenome.ParentAttachSide attachSide = agentBodyBeingTested.creatureBodySegmentGenomeList[i].parentAttachSide; 
				if(attachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg || attachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos) {
					// Find how much the affected side is shared by both segments, divide by two when applying because sa_x is an average of its pos/neg faces;
					float overlap = (Mathf.Min (ownSize.y, parentSize.y) * Mathf.Min (ownSize.z, parentSize.z)) / 2f; 
					sa_x[i] -= overlap;  // subtract the shared region from this segment that should not be in contact with the water
					sa_x[parentID] -= overlap;  // subtract the shared region from the parent segment that should not be in contact with the water
				}
				else if(attachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg || attachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos) {
					float overlap = (Mathf.Min (ownSize.x, parentSize.x) * Mathf.Min (ownSize.z, parentSize.z)) / 2f; 
					sa_y[i] -= overlap;  // subtract the shared region from this segment that should not be in contact with the water
					sa_y[parentID] -= overlap;
				}
				else if(attachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg || attachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos) {
					float overlap = (Mathf.Min (ownSize.x, parentSize.x) * Mathf.Min (ownSize.y, parentSize.y)) / 2f; 
					sa_z[i] -= overlap;  // subtract the shared region from this segment that should not be in contact with the water
					sa_z[parentID] -= overlap;
				}

				wormSegmentArray_PosX[i][0] = centerPos.x;
				wormSegmentArray_PosY[i][0] = centerPos.y;
				wormSegmentArray_PosZ[i][0] = centerPos.z;
				// Initialize Joint Settings:
				wormSegmentArray_AngleX[i][0] = 0f;
				wormSegmentArray_MotorTargetX[i][0] = 0f;
				wormSegmentArray_AngleY[i][0] = 0f;
				wormSegmentArray_MotorTargetY[i][0] = 0f;
				wormSegmentArray_AngleZ[i][0] = 0f;
				wormSegmentArray_MotorTargetZ[i][0] = 0f;
			}
			//avgPos += new Vector3(wormSegmentArray_PosX[i][0], wormSegmentArray_PosY[i][0], wormSegmentArray_PosZ[i][0]);
		}

		float avgMass = wormTotalMass / numberOfSegments;  //
		wormCOM.x = 0f;
		wormCOM.y = 0f;
		wormCOM.z = 0f;

		wormTotalMass /= avgMass;  // Adjust to center around 1f;
		for(int j = 0; j < numberOfSegments; j++) { 
			// Finalize Segment Mass Value:
			wormSegmentArray_Mass[j] /= avgMass;  // Center the segment Masses around 1f, to avoid precision errors

			float shareOfTotalMass = wormSegmentArray_Mass[j] / wormTotalMass;
			wormCOM.x += wormSegmentArray_PosX[j][0] * shareOfTotalMass;  // multiply position by proportional share of total mass
			wormCOM.y += wormSegmentArray_PosY[j][0] * shareOfTotalMass;
			wormCOM.z += wormSegmentArray_PosZ[j][0] * shareOfTotalMass;
		}
		
		for(int k = 0; k < numberOfSegments; k++) { 
			// Center the creature so that its center of mass is at the origin, to avoid initial position bias
			wormSegmentArray_PosX[k][0] -= wormCOM.x;
			wormSegmentArray_PosY[k][0] -= wormCOM.y;
			wormSegmentArray_PosZ[k][0] -= wormCOM.z;
		}

		// Reset wormCOM to 0f, now that it's been moved:
		wormCOM.x = 0f;
		wormCOM.y = 0f;
		wormCOM.z = 0f;
		
		gameInitialized = true;
		gameTicked = false;
		gameUpdatedFromPhysX = false;
		gameCurrentTimeStep = 0;  // reset to 0
		preWarm = true;
	}
	
	public override void Tick() {  // Runs the mini-game for a single evaluation step.
		//Debug.Log ("Tick()");
		// THIS IS ALL PRE- PHYS-X!!! ::
		if(gameCurrentTimeStep % 480 == 0) {
			preWarm = false;
			//Debug.Log ("PreWarm OFF");
			float randX = UnityEngine.Random.Range(-1f, 1f);
			float randY = UnityEngine.Random.Range(-1f, 1f);
			float randZ = UnityEngine.Random.Range(-1f, 1f);
			float delta = targetPosAxis[0] - 0.5f;
			if(targetX[0] <= 0.5f) {  // if x-axis is enabled, use randX, else set to 0;
				randX = 0f;
			}
			else {
				if(delta > 0.16667f) {
					randX = Mathf.Abs (randX);
				}
				else if(delta < -0.16667f) {
					randX = -Mathf.Abs (randX);
				}
				else { // btw 0.333 and 0.667 ==> use full random
					
				}
			}
			if(targetY[0] < 0.5f) {  
				randY = 0f;
			}
			else {
				if(delta > 0.16667f) {
					randY = Mathf.Abs (randY);
				}
				else if(delta < -0.16667f) {
					randY = -Mathf.Abs (randY);
				}
			}
			if(targetZ[0] < 0.5f) {  // if x-axis is enabled, use randX, else set to 0;
				randZ = 0f;
			}
			else {
				if(delta > 0.16667f) {
					randZ = Mathf.Abs (randZ);
				}
				else if(delta < -0.16667f) {
					randZ = -Mathf.Abs (randZ);
				}
			}			
			Vector3 randDir = new Vector3(randX, randY, randZ).normalized;
			targetPos = new Vector3(randDir.x * maxScoreDistance[0], randDir.y * maxScoreDistance[0], randDir.z * maxScoreDistance[0]);
			targetPosX[0] = targetPos.x;
			targetPosY[0] = targetPos.y;
			targetPosZ[0] = targetPos.z;
		}

		clockValue[0] = Mathf.Sin (Time.fixedTime * clockFrequency[0]);

		for(int w = 0; w < numberOfSegments; w++) {
			if(GOwormSegments[w].GetComponent<ConfigurableJoint>() != null) {
				if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].jointType == CreatureBodySegmentGenome.JointType.HingeX) {
					JointDrive drive = GOwormSegments[w].GetComponent<ConfigurableJoint>().angularXDrive;
					drive.maximumForce = jointMotorForce[0];
					GOwormSegments[w].GetComponent<ConfigurableJoint>().angularXDrive = drive;
					Vector3 targetVel = GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity;
					targetVel.x = wormSegmentArray_MotorTargetX[w][0] * jointMotorSpeed[0];
					GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;
					//GOwormSegments[w].GetComponent<ConfigurableJoint>().angularXDrive.maximumForce = jointMotorForce[0];
					//GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity.x = wormSegmentArray_MotorTargetX[w][0] * jointMotorSpeed[0];
				}
				else if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].jointType == CreatureBodySegmentGenome.JointType.HingeY) {
					JointDrive drive = GOwormSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive;
					drive.maximumForce = jointMotorForce[0];
					GOwormSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive = drive;
					Vector3 targetVel = GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity;
					targetVel.y = wormSegmentArray_MotorTargetY[w][0] * jointMotorSpeed[0];
					GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;
				}
				else if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].jointType == CreatureBodySegmentGenome.JointType.HingeZ) {
					JointDrive drive = GOwormSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive;
					drive.maximumForce = jointMotorForce[0];
					GOwormSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive = drive;
					Vector3 targetVel = GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity;
					targetVel.z = wormSegmentArray_MotorTargetZ[w][0] * jointMotorSpeed[0];
					GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;
				}

				//JointMotor motor = new JointMotor();
				//motor.force = jointMotorForce[0];
				//motor.targetVelocity = wormSegmentArray_MotorTarget[w][0] * jointMotorSpeed[0];
				//Debug.Log ("Tick, TargetVel= " + wormSegmentArray_MotorTarget[w][0].ToString());
				//GOwormSegments[w].GetComponent<ConfigurableJoint>().motor = motor;


			}
			ApplyViscosityForces(GOwormSegments[w], w, viscosityDrag[0]);
		}

		// FITNESS COMPONENTS!
		float avgMass = wormTotalMass / numberOfSegments;  //
		Vector3 prevFrameWormCOM = wormCOM;
		wormCOM.x = 0f;
		wormCOM.y = 0f;
		wormCOM.z = 0f;

		//Vector3 avgPos = new Vector3(0f, 0f, 0f);
		for(int e = 0; e < numberOfSegments; e++) {
			float shareOfTotalMass = wormSegmentArray_Mass[e] / wormTotalMass;
			wormCOM.x += wormSegmentArray_PosX[e][0] * shareOfTotalMass;  // multiply position by proportional share of total mass
			wormCOM.y += wormSegmentArray_PosY[e][0] * shareOfTotalMass;
			wormCOM.z += wormSegmentArray_PosZ[e][0] * shareOfTotalMass;

			//avgPos += new Vector3(wormSegmentArray_PosX[e][0], wormSegmentArray_PosY[e][0], wormSegmentArray_PosZ[e][0]);
			fitEnergySpent[0] += Mathf.Abs (wormSegmentArray_MotorTargetX[e][0]) / 3f;
			fitEnergySpent[0] += Mathf.Abs (wormSegmentArray_MotorTargetY[e][0]) / 3f;
			fitEnergySpent[0] += Mathf.Abs (wormSegmentArray_MotorTargetZ[e][0]) / 3f;
		}
		//avgPos /= (float)numberOfSegments;
		Vector3 comVel = wormCOM - prevFrameWormCOM; // this frame minus last frame
		//Debug.Log ("comVel= " + comVel.ToString());
		//wormCOM = avgPos;
		//targetPosX[0] = targetPos.x + wormCOM.x;
		//targetPosY[0] = targetPos.y + wormCOM.y;
		//targetPosZ[0] = targetPos.z + wormCOM.z;
		ArenaCameraController.arenaCameraControllerStatic.focusPosition = wormCOM;
		Vector3 targetDirection = new Vector3(targetPosX[0] - wormCOM.x, targetPosY[0] - wormCOM.y, targetPosZ[0] - wormCOM.z);
		float distToTarget = targetDirection.magnitude;
		//if(preWarm == false) {
		fitDistToTarget[0] += distToTarget / maxScoreDistance[0];
		fitDistFromOrigin[0] += wormCOM.magnitude / maxScoreDistance[0];
		//}

		Vector3 headToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[0][0], targetPosY[0] - wormSegmentArray_PosY[0][0], targetPosZ[0] - wormSegmentArray_PosZ[0][0]).normalized;
		targetHeadDot[0] = Vector3.Dot (headToTargetVect, GOwormSegments[0].transform.forward);
		targetHeadDotHor[0] = Vector3.Dot (headToTargetVect, GOwormSegments[0].transform.right);
		targetHeadDotVer[0] = Vector3.Dot (headToTargetVect, GOwormSegments[0].transform.up);
		//if(preWarm == false) {
		fitTargetHeadDot[0] += targetHeadDot[0] * 0.5f + 0.5f;  
		fitTargetHeadDotHor[0] += Mathf.Abs (targetHeadDotHor[0]); // need to be perpendicular to target, so 0 is the desired value -- come back to this later (biggeris better should become 'target value'?)
		fitTargetHeadDotVer[0] += Mathf.Abs (targetHeadDotVer[0]);

		fitMoveToTarget[0] += Vector3.Dot (comVel.normalized, targetDirection.normalized) * 0.5f + 0.5f;
		fitMoveSpeed[0] += comVel.magnitude / moveSpeedMaxFit[0];

		if(distToTarget < targetRadius[0]) {
			fitTimeInTarget[0] += 1f;
		}
		else {
			fitTimeInTarget[0] += 0f;
		}
		//}

		targetDirX[0] = targetDirection.x;
		targetDirY[0] = targetDirection.y;
		targetDirZ[0] = targetDirection.z;

		gameTicked = true;
		gameCurrentTimeStep++;
	}

	#region SETUP METHODS:
	private void SetupInputOutputChannelLists() {
		// TESTING:
		// Brain Inputs!:
		inputChannelsList = new List<BrainInputChannel>();
		// Brain Outputs!:		
		outputChannelsList = new List<BrainOutputChannel>();
		
		for(int bc = 0; bc < numberOfSegments; bc++) {
			if(bc != 0) {  // if not Root Segment:
				//Debug.Log ("SetupInputOutputChannelLists: " + agentBodyBeingTested.creatureBodySegmentGenomeList[bc].jointType.ToString());
				if(agentBodyBeingTested.creatureBodySegmentGenomeList[bc].jointType == CreatureBodySegmentGenome.JointType.HingeX) {
					string inputChannelName = "Worm Segment " + bc.ToString() + " Angle";
					BrainInputChannel BIC_wormSegmentAngle = new BrainInputChannel(ref wormSegmentArray_AngleX[bc], false, inputChannelName);
					inputChannelsList.Add (BIC_wormSegmentAngle);

					string outputChannelName = "Worm Segment " + bc.ToString() + " Motor Target";
					BrainOutputChannel BOC_wormSegmentAngleVel = new BrainOutputChannel(ref wormSegmentArray_MotorTargetX[bc], false, outputChannelName);
					outputChannelsList.Add (BOC_wormSegmentAngleVel);
				}
				else if(agentBodyBeingTested.creatureBodySegmentGenomeList[bc].jointType == CreatureBodySegmentGenome.JointType.HingeY) {
					string inputChannelName = "Worm Segment " + bc.ToString() + " Angle";
					BrainInputChannel BIC_wormSegmentAngle = new BrainInputChannel(ref wormSegmentArray_AngleY[bc], false, inputChannelName);
					inputChannelsList.Add (BIC_wormSegmentAngle);

					string outputChannelName = "Worm Segment " + bc.ToString() + " Motor Target";
					BrainOutputChannel BOC_wormSegmentAngleVel = new BrainOutputChannel(ref wormSegmentArray_MotorTargetY[bc], false, outputChannelName);
					outputChannelsList.Add (BOC_wormSegmentAngleVel);
				}
				else if(agentBodyBeingTested.creatureBodySegmentGenomeList[bc].jointType == CreatureBodySegmentGenome.JointType.HingeZ) {
					string inputChannelName = "Worm Segment " + bc.ToString() + " Angle";
					BrainInputChannel BIC_wormSegmentAngle = new BrainInputChannel(ref wormSegmentArray_AngleZ[bc], false, inputChannelName);
					inputChannelsList.Add (BIC_wormSegmentAngle);

					string outputChannelName = "Worm Segment " + bc.ToString() + " Motor Target";
					BrainOutputChannel BOC_wormSegmentAngleVel = new BrainOutputChannel(ref wormSegmentArray_MotorTargetZ[bc], false, outputChannelName);
					outputChannelsList.Add (BOC_wormSegmentAngleVel);
				}
			}
			//string inputChannelName = "Worm Segment " + bc.ToString() + " Angle";
			//BrainInputChannel BIC_wormSegmentAngle = new BrainInputChannel(ref wormSegmentArray_Angle[bc], false, inputChannelName);
			//inputChannelsList.Add (BIC_wormSegmentAngle);
			
			//string outputChannelName = "Worm Segment " + bc.ToString() + " Motor Target";
			//BrainOutputChannel BOC_wormSegmentAngleVel = new BrainOutputChannel(ref wormSegmentArray_MotorTarget[bc], false, outputChannelName);
			//outputChannelsList.Add (BOC_wormSegmentAngleVel);
		}
		BrainInputChannel BIC_targetHeadDot = new BrainInputChannel(ref targetHeadDot, false, "Target Head Dot");
		inputChannelsList.Add (BIC_targetHeadDot);
		BrainInputChannel BIC_targetHeadDotHor = new BrainInputChannel(ref targetHeadDotHor, false, "Target Head Dot Hor");
		inputChannelsList.Add (BIC_targetHeadDotHor);
		BrainInputChannel BIC_targetHeadDotVer = new BrainInputChannel(ref targetHeadDotVer, false, "Target Head Dot Ver");
		inputChannelsList.Add (BIC_targetHeadDotVer);
		BrainInputChannel BIC_clockValue = new BrainInputChannel(ref clockValue, false, "Clock Value");
		inputChannelsList.Add (BIC_clockValue);
		BrainInputChannel BIC_targetDirX = new BrainInputChannel(ref targetDirX, false, "TargetDir X");
		inputChannelsList.Add (BIC_targetDirX);
		BrainInputChannel BIC_targetDirY = new BrainInputChannel(ref targetDirY, false, "TargetDir Y");
		inputChannelsList.Add (BIC_targetDirY);
		BrainInputChannel BIC_targetDirZ = new BrainInputChannel(ref targetDirZ, false, "TargetDir Z");
		inputChannelsList.Add (BIC_targetDirZ);
	}

	private void SetupFitnessComponentList() {
		// Fitness Component List:
		fitnessComponentList = new List<FitnessComponent>();
		FitnessComponent FC_distFromOrigin = new FitnessComponent(ref fitDistFromOrigin, true, true, 1f, 1f, "Distance From Origin", true);
		fitnessComponentList.Add (FC_distFromOrigin); // 0
		FitnessComponent FC_energySpent = new FitnessComponent(ref fitEnergySpent, true, false, 1f, 1f, "Energy Spent", true);
		fitnessComponentList.Add (FC_energySpent); // 1
		FitnessComponent FC_distToTarget = new FitnessComponent(ref fitDistToTarget, true, false, 1f, 1f, "Distance To Target", true);
		fitnessComponentList.Add (FC_distToTarget); // 2
		FitnessComponent FC_timeToTarget = new FitnessComponent(ref fitTimeInTarget, true, false, 1f, 1f, "Time To Target", true);
		fitnessComponentList.Add (FC_timeToTarget); // 3
		FitnessComponent FC_targetHeadDot = new FitnessComponent(ref fitTargetHeadDot, true, true, 1f, 1f, "Target Head Dot", true);
		fitnessComponentList.Add (FC_targetHeadDot); // 4
		FitnessComponent FC_targetHeadDotHor = new FitnessComponent(ref fitTargetHeadDotHor, true, false, 1f, 1f, "Target Head Dot Hor", true);
		fitnessComponentList.Add (FC_targetHeadDotHor); // 5
		FitnessComponent FC_targetHeadDotVer = new FitnessComponent(ref fitTargetHeadDotVer, true, false, 1f, 1f, "Target Head Dot Ver", true);
		fitnessComponentList.Add (FC_targetHeadDotVer); // 6
		FitnessComponent FC_moveToTarget = new FitnessComponent(ref fitMoveToTarget, true, true, 1f, 1f, "Move Towards Target", true);
		fitnessComponentList.Add (FC_moveToTarget); // 7
		FitnessComponent FC_moveSpeed = new FitnessComponent(ref fitMoveSpeed, true, true, 1f, 1f, "Average Speed", true);
		fitnessComponentList.Add (FC_moveSpeed); // 8

		ResetFitnessComponentValues();
	}

	private void ResetFitnessComponentValues() {
		// FITNESS COMPONENTS!:
		fitDistFromOrigin[0] = 0f;
		fitEnergySpent[0] = 0f;
		fitDistToTarget[0] = 0f;
		fitTimeInTarget[0] = 0f;
		fitTargetHeadDot[0] = 0f;
		fitTargetHeadDotHor[0] = 0f;
		fitTargetHeadDotVer[0] = 0f;
		fitMoveToTarget[0] = 0f;
		fitMoveSpeed[0] = 0f;
	}

	private void SetupGameOptionsList() {
		// Game Options List:
		gameOptionsList = new List<GameOptionChannel>();
		GameOptionChannel GOC_clockFrequency = new GameOptionChannel(ref clockFrequency, 0.04f, 32f, "Clock Frequency");
		gameOptionsList.Add (GOC_clockFrequency); // 0
		GameOptionChannel GOC_viscosityDrag = new GameOptionChannel(ref viscosityDrag, 0.1f, 1000f, "Viscosity Drag");
		gameOptionsList.Add (GOC_viscosityDrag); // 0
		GameOptionChannel GOC_jointMotorForce = new GameOptionChannel(ref jointMotorForce, 1f, 1000f, "Joint Motor Force");
		gameOptionsList.Add (GOC_jointMotorForce); // 1
		GameOptionChannel GOC_jointMotorSpeed = new GameOptionChannel(ref jointMotorSpeed, 1f, 1000f, "Joint Motor Speed");
		gameOptionsList.Add (GOC_jointMotorSpeed); // 2
		GameOptionChannel GOC_variableMass = new GameOptionChannel(ref variableMass, 0f, 1f, "VariableMass");
		gameOptionsList.Add (GOC_variableMass); // 2
		GameOptionChannel GOC_targetRadius = new GameOptionChannel(ref targetRadius, 0.01f, 25f, "Target Size");
		gameOptionsList.Add (GOC_targetRadius); // 8
		GameOptionChannel GOC_targetX = new GameOptionChannel(ref targetX, 0f, 1f, "Target Use X-Axis");
		gameOptionsList.Add (GOC_targetX); // 3
		GameOptionChannel GOC_targetY = new GameOptionChannel(ref targetY, 0f, 1f, "Target Use Y-Axis");
		gameOptionsList.Add (GOC_targetY); // 4
		GameOptionChannel GOC_targetZ = new GameOptionChannel(ref targetZ, 0f, 1f, "Target Use Z-Axis");
		gameOptionsList.Add (GOC_targetZ); // 5
		GameOptionChannel GOC_targetPosAxis = new GameOptionChannel(ref targetPosAxis, 0f, 1f, "Target Only Positive Axis");
		gameOptionsList.Add (GOC_targetPosAxis); // 5
		GameOptionChannel GOC_moveSpeedMaxFit = new GameOptionChannel(ref moveSpeedMaxFit, 0.001f, 1f, "Move Speed Max Score");
		gameOptionsList.Add (GOC_moveSpeedMaxFit); // 6
		GameOptionChannel GOC_maxScoreDistance = new GameOptionChannel(ref maxScoreDistance, 0.001f, 50f, "Move Score Distance");
		gameOptionsList.Add (GOC_maxScoreDistance); // 7
		//GameOptionChannel GOC_wormTotalLength = new GameOptionChannel(ref armTotalLength, 0.01f, 5f, "Worm Total Length");
		//gameOptionsList.Add (GOC_wormTotalLength); // 8
		//GameOptionChannel GOC_wormThicknessHor = new GameOptionChannel(ref wormSegmentThicknessHorMax, 0.01f, 1f, "Worm Thickness Hor");
		//gameOptionsList.Add (GOC_wormThicknessHor); // 8
		//GameOptionChannel GOC_wormThicknessVer = new GameOptionChannel(ref wormSegmentThicknessVerMax, 0.01f, 1f, "Worm Thickness Ver");
		//gameOptionsList.Add (GOC_wormThicknessVer); // 8
		//GameOptionChannel GOC_wormBendTwoAxis = new GameOptionChannel(ref armSegmentBendTwoAxis, 0f, 1f, "Bend Two Axis");
		//gameOptionsList.Add (GOC_wormBendTwoAxis); // 8
	}
	#endregion
	
	public override void UpdateGameStateFromPhysX() {
		//Debug.Log ("UpdateGameStateFromPhysX()");
		// PhysX simulation happened after miniGame Tick(), so before passing gameStateData to brain, need to update gameStateData from the rigidBodies
		// SO that the correct updated input values can be sent to the brain
		for(int w = 0; w < numberOfSegments; w++) {
			if(GOwormSegments[w].GetComponent<Rigidbody>() != null) {
				wormSegmentArray_PosX[w][0] = GOwormSegments[w].GetComponent<Rigidbody>().position.x;
				wormSegmentArray_PosY[w][0] = GOwormSegments[w].GetComponent<Rigidbody>().position.y;
				wormSegmentArray_PosZ[w][0] = GOwormSegments[w].GetComponent<Rigidbody>().position.z;
				//Debug.Log ("UpdateGameStateFromPhysX() rigidBodPos: " + new Vector3(wormSegmentArray_PosX[w][0], wormSegmentArray_PosY[w][0], wormSegmentArray_PosZ[w][0]));

				if(w > 0) {  // if not the ROOT segment:
					//wormSegmentArray_AngleX[w][0] = Quaternion.FromToRotation(GOwormSegments[w].GetComponent<ConfigurableJoint>().axis, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.transform.rotation.eulerAngles).eulerAngles.x;
					//wormSegmentArray_AngleY[w][0] = Quaternion.FromToRotation(GOwormSegments[w].GetComponent<ConfigurableJoint>().axis, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.transform.rotation.eulerAngles).eulerAngles.y;
					//wormSegmentArray_AngleZ[w][0] = Quaternion.FromToRotation(GOwormSegments[w].GetComponent<ConfigurableJoint>().axis, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.transform.rotation.eulerAngles).eulerAngles.z;
					MeasureJointAngles(w);
					//Debug.Log ("Segment " + w.ToString() + ", AngleX= " + wormSegmentArray_AngleX[w][0].ToString() + ", AngleY= " + wormSegmentArray_AngleY[w][0].ToString() + ", AngleZ= " + wormSegmentArray_AngleZ[w][0].ToString());
				}
			}
		}
		gameUpdatedFromPhysX = true;
		SetNonPhysicsGamePieceTransformsFromData();  // debug objects that rely on PhysX object positions
	}

	private void MeasureJointAngles(int w) {
		if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos) {
			wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
		}
		else if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg) {
			wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
		}
		else if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos) {
			wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
		}
		else if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg) {
			wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
		}
		else if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos) {
			wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.right, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
		}
		else if(agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg) {
			wormSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.forward, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
			wormSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOwormSegments[w].transform.up, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
		}

		//Debug.Log ("Segment " + w.ToString() + ", AngleX= " + wormSegmentArray_AngleX[w][0].ToString() + ", AngleY= " + wormSegmentArray_AngleY[w][0].ToString() + ", AngleZ= " + wormSegmentArray_AngleZ[w][0].ToString());

	}

	public override void InstantiateGamePieces() {
		//Debug.Log ("MiniGameCreatureSwimBasic InstantiateGamePieces();");
		//GOwormSegments = new GameObject[numberOfSegments]; // USED TO BE IN RESET -> InitializeGameDataArrays();

		for(int i = 0; i < numberOfSegments; i++) {
			string name = "GOwormSegment" + i.ToString();
			GOwormSegments[i] = new GameObject(name);
			GOwormSegments[i].transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
			GOwormSegments[i].AddComponent<GamePiecePhysXWormSegment>().InitGamePiece();
			GOwormSegments[i].GetComponent<Renderer>().material = segmentMaterial;
		}

		GOtargetSphere = new GameObject("GOtargetSphere");
		GOtargetSphere.transform.localScale = new Vector3(targetRadius[0], targetRadius[0], targetRadius[0]);
		GOtargetSphere.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOtargetSphere.AddComponent<GamePieceCommonSphere>().InitGamePiece();
		GOtargetSphere.GetComponent<Renderer>().material = targetSphereMat;

		GOheadToTargetVector = new GameObject("GOheadToTargetVector");
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

		GOwormCenterOfMass = new GameObject("GOwormCenterOfMass");
		GOwormCenterOfMass.transform.localScale = new Vector3(debugCOMradius, debugCOMradius, debugCOMradius);
		GOwormCenterOfMass.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOwormCenterOfMass.AddComponent<GamePieceCommonSphere>().InitGamePiece();
		GOwormCenterOfMass.GetComponent<Renderer>().material = wormCenterOfMassMat;
	}
	
	public override void UninstantiateGamePieces() {
		
	}
	
	public override void EnablePhysicsGamePieceComponents() {
		//Debug.Log ("BuildGamePieceComponents()");

		//ConfigurableJoint joint = segmentListGO[i].AddComponent<ConfigurableJoint>();
		//joint.connectedBody = segmentListGO[workingNodeList[i].parentID].GetComponent<Rigidbody>();  // Set parent
		//joint.anchor = GetJointAnchor(workingNodeList[i]);
		//joint.connectedAnchor = GetJointConnectedAnchor(workingNodeList[i]); // <-- Might be Unnecessary
		//ConfigureJointSettings(workingNodeList[i], ref joint);

		for(int w = 0; w < numberOfSegments; w++) {
			GOwormSegments[w].GetComponent<Rigidbody>().useGravity = false;
			GOwormSegments[w].GetComponent<Rigidbody>().isKinematic = false;  //////////////////////////////////
			GOwormSegments[w].GetComponent<Rigidbody>().mass = wormSegmentArray_Mass[w];
			if(w < (numberOfSegments - 1)) {  // if not the final 'Tail' segment:
				
			}
			if(w > 0) {  // if not the first root segment:
				ConfigurableJoint configJoint;
				if(GOwormSegments[w].GetComponent<ConfigurableJoint>() == null) {
					configJoint = GOwormSegments[w].AddComponent<ConfigurableJoint>();
				}
				else {
					configJoint = GOwormSegments[w].GetComponent<ConfigurableJoint>();
				}
				configJoint.autoConfigureConnectedAnchor = false;
				//Debug.Log ("EnablePhysicsGamePieceComponents(); id= " + agentBodyBeingTested.creatureBodySegmentGenomeList[w].id.ToString() + ", parentID= " + agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentID.ToString());
				configJoint.connectedBody = GOwormSegments[agentBodyBeingTested.creatureBodySegmentGenomeList[w].parentID].GetComponent<Rigidbody>();  // Set parent
				configJoint.anchor = GetJointAnchor(agentBodyBeingTested.creatureBodySegmentGenomeList[w]);
				configJoint.connectedAnchor = GetJointConnectedAnchor(agentBodyBeingTested.creatureBodySegmentGenomeList[w]); // <-- Might be Unnecessary
				ConfigureJointSettings(agentBodyBeingTested.creatureBodySegmentGenomeList[w], ref configJoint);
				//configJoint.connectedBody = GOwormSegments[w].GetComponent<Rigidbody>(); // connected bod of previous segment is THIS segment
				//configJoint.anchor = new Vector3(0.5f, 0f, 0f);
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
			}
		}		
		piecesBuilt = true;
	}
	
	public override void DisablePhysicsGamePieceComponents() { // See if I can just sleep them?
		//Debug.Log ("DestroyGamePieceComponents()");
		for(int w = 0; w < numberOfSegments; w++) {
			if(GOwormSegments[w].GetComponent<Rigidbody>() != null) {
				GOwormSegments[w].GetComponent<Rigidbody>().isKinematic = true;
				if(GOwormSegments[w].GetComponent<ConfigurableJoint>() != null) {
					GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody = null;
				}
			}	
		}
		piecesBuilt = false;
	}

	public override void SetPhysicsGamePieceTransformsFromData() {
		//Debug.Log ("SetPhysicsGamePieceTransformsFromData()");
		for(int w = 0; w < numberOfSegments; w++) {
			GOwormSegments[w].transform.localScale = new Vector3(wormSegmentArray_ScaleX[w][0], wormSegmentArray_ScaleY[w][0], wormSegmentArray_ScaleZ[w][0]);
			GOwormSegments[w].transform.position = new Vector3(wormSegmentArray_PosX[w][0], wormSegmentArray_PosY[w][0], wormSegmentArray_PosZ[w][0]); // RE-EVALUATE!!!
			GOwormSegments[w].transform.localRotation = Quaternion.identity;
		}
	}

	public override void SetNonPhysicsGamePieceTransformsFromData() {
		Vector3 headToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[0][0], targetPosY[0] - wormSegmentArray_PosY[0][0], targetPosZ[0] - wormSegmentArray_PosZ[0][0]);
		// POSITION!
		GOheadToTargetVector.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + (headToTargetVect / 2f);	// halfway between head center and target center
		GOheadToTargetVectorHor.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.right * (debugVectorLength / 2f);
		GOheadToTargetVectorVer.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.up * (debugVectorLength / 2f);	
		GOheadFacingVector.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.forward * (debugVectorLength / 2f);;
		GOwormCenterOfMass.transform.position = wormCOM; // average center of mass
		// ROTATION!
		GOheadToTargetVector.transform.rotation = Quaternion.LookRotation(headToTargetVect);
		GOheadToTargetVectorHor.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.right); // Figure this out!
		GOheadToTargetVectorVer.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.up);
		GOheadFacingVector.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.forward);  // grab direction from head segment
		// SCALE!
		float lengthScale = headToTargetVect.magnitude;
		GOheadToTargetVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, lengthScale);

		GOtargetSphere.transform.localPosition = new Vector3(targetPosX[0], targetPosY[0], targetPosZ[0]);
		GOtargetSphere.transform.localScale = new Vector3(targetRadius[0], targetRadius[0], targetRadius[0]);
	}

	private void ApplyViscosityForces(GameObject body, int segmentIndex, float drag) {
		Rigidbody rigidBod = body.GetComponent<Rigidbody>();
		// Cache positive axis vectors:
		Vector3 forward = body.transform.forward;
		Vector3 up = body.transform.up;
		Vector3 right = body.transform.right;
		// Find centers of each of box's faces
		Vector3 xpos_face_center = (right * body.transform.localScale.x / 2f) + body.transform.position;
		Vector3 ypos_face_center = (up * body.transform.localScale.y / 2f) + body.transform.position;
		Vector3 zpos_face_center = (forward * body.transform.localScale.z / 2f) + body.transform.position;
		Vector3 xneg_face_center = -(right * body.transform.localScale.x / 2f) + body.transform.position;
		Vector3 yneg_face_center = -(up * body.transform.localScale.y / 2f) + body.transform.position;
		Vector3 zneg_face_center = -(forward * body.transform.localScale.z / 2f) + body.transform.position;
		
		// FRONT (posZ):
		Vector3 pointVelPosZ = rigidBod.GetPointVelocity (zpos_face_center); // Get velocity of face's center (doesn't catch torque around center of mass)
		Vector3 fluidDragVecPosZ = -forward *    // in the direction opposite the face's normal
			Vector3.Dot (forward, pointVelPosZ) *    // 
				sa_z[segmentIndex] * viscosityDrag[0];  // multiplied by face's surface area, and user-defined multiplier
		rigidBod.AddForceAtPosition (fluidDragVecPosZ*2f, zpos_face_center);  // Apply force at face's center, in the direction opposite the face normal
		
		// TOP (posY):
		Vector3 pointVelPosY = rigidBod.GetPointVelocity (ypos_face_center);
		Vector3 fluidDragVecPosY = -up * Vector3.Dot (up, pointVelPosY) * sa_y[segmentIndex] * viscosityDrag[0];  
		rigidBod.AddForceAtPosition (fluidDragVecPosY*2f, ypos_face_center);		

		// RIGHT (posX):
		Vector3 pointVelPosX = rigidBod.GetPointVelocity (xpos_face_center);
		Vector3 fluidDragVecPosX = -right * Vector3.Dot (right, pointVelPosX) * sa_x[segmentIndex] * viscosityDrag[0];  
		rigidBod.AddForceAtPosition (fluidDragVecPosX, xpos_face_center);
	}

	#region RebuildCreature()
	
	void ConfigureJointSettings(CreatureBodySegmentGenome node, ref ConfigurableJoint joint) {
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

		if(node.jointType == CreatureBodySegmentGenome.JointType.Fixed) { // Fixed Joint
			// Lock mobility:
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Locked;
			joint.angularYMotion = ConfigurableJointMotion.Locked;
			joint.angularZMotion = ConfigurableJointMotion.Locked;
		}
		else if(node.jointType == CreatureBodySegmentGenome.JointType.HingeX) { // Uni-Axis Hinge Joint
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
		else if(node.jointType == CreatureBodySegmentGenome.JointType.HingeY) { // Uni-Axis Hinge Joint
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
		else if(node.jointType == CreatureBodySegmentGenome.JointType.HingeZ) { // Uni-Axis Hinge Joint
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
	
	private Vector3 GetJointAnchor(CreatureBodySegmentGenome node) {
		Vector3 retVec = new Vector3(0f, 0f, 0f);
		
		if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg) {  // Neg X
			retVec.x = 0.5f;
			retVec.y = node.attachPointChild.y;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg) {  // Neg Y
			retVec.x = node.attachPointChild.x;
			retVec.y = 0.5f;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg) { // Neg Z
			retVec.x = node.attachPointChild.x;
			retVec.y = node.attachPointChild.y;
			retVec.z = 0.5f;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos) {  // Pos X
			retVec.x = -0.5f;
			retVec.y = node.attachPointChild.y;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos) {  // Pos Y
			retVec.x = node.attachPointChild.x;
			retVec.y = -0.5f;
			retVec.z = node.attachPointChild.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos) {  // Pos Z
			retVec.x = node.attachPointChild.x;
			retVec.y = node.attachPointChild.y;
			retVec.z = -0.5f;
		}
		
		return retVec;
	}
	
	private Vector3 GetJointConnectedAnchor(CreatureBodySegmentGenome node) {
		Vector3 retVec = new Vector3(0f, 0f, 0f);
		
		if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg) {  // Neg X
			retVec.x = -0.5f;
			retVec.y = node.attachPointParent.y;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg) {  // Neg Y
			retVec.x = node.attachPointParent.x;
			retVec.y = -0.5f;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg) { // Neg Z
			retVec.x = node.attachPointParent.x;
			retVec.y = node.attachPointParent.y;
			retVec.z = -0.5f;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos) {  // Pos X
			retVec.x = 0.5f;
			retVec.y = node.attachPointParent.y;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos) {  // Pos Y
			retVec.x = node.attachPointParent.x;
			retVec.y = 0.5f;
			retVec.z = node.attachPointParent.z;
		}
		else if(node.parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos) {  // Pos Z
			retVec.x = node.attachPointParent.x;
			retVec.y = node.attachPointParent.y;
			retVec.z = 0.5f;
		}
		
		return retVec;
	}
	
	private Vector3 GetBlockPosition(Vector3 parentCenter, Vector3 parentSize, Vector3 attachParent, Vector3 attachChild, Vector3 ownSize, CreatureBodySegmentGenome.ParentAttachSide attachAxis) {
		
		float xOffset = parentCenter.x;
		float yOffset = parentCenter.y;
		float zOffset = parentCenter.z;
		
		
		if(attachAxis == CreatureBodySegmentGenome.ParentAttachSide.xNeg) {  // Neg X
			// Adjusting Offset For Connection Axis, each segment is centered on the other
			xOffset = parentCenter.x - ((parentSize.x / 2f) + (ownSize.x / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			yOffset += attachParent.y * parentSize.y;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			yOffset -= attachChild.y * ownSize.y;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureBodySegmentGenome.ParentAttachSide.yNeg) {  // Neg Y
			yOffset = parentCenter.y - ((parentSize.y / 2f) + (ownSize.y / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			xOffset += attachParent.x * parentSize.x;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			xOffset -= attachChild.x * ownSize.x;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureBodySegmentGenome.ParentAttachSide.zNeg) { // Neg Z
			zOffset = parentCenter.z - ((parentSize.z / 2f) + (ownSize.z / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			xOffset += attachParent.x * parentSize.x;
			yOffset += attachParent.y * parentSize.y;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			xOffset -= attachChild.x * ownSize.x;
			yOffset -= attachChild.y * ownSize.y;
		}
		else if(attachAxis == CreatureBodySegmentGenome.ParentAttachSide.xPos) {  // Pos X
			xOffset = parentCenter.x + ((parentSize.x / 2f) + (ownSize.x / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			yOffset += attachParent.y * parentSize.y;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			yOffset -= attachChild.y * ownSize.y;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureBodySegmentGenome.ParentAttachSide.yPos) {  // Pos Y
			yOffset = parentCenter.y + ((parentSize.y / 2f) + (ownSize.y / 2f));
			// Adjust Offset due to Attach Point Parent  ( the location on the parent segment where the joint hinges ):
			xOffset += attachParent.x * parentSize.x;
			zOffset += attachParent.z * parentSize.z;
			// Adjust Offset due to own Attach Point  ( the location on this segment where the joint hinges ):
			xOffset -= attachChild.x * ownSize.x;
			zOffset -= attachChild.z * ownSize.z;
		}
		else if(attachAxis == CreatureBodySegmentGenome.ParentAttachSide.zPos) {  // Pos Z
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
}
