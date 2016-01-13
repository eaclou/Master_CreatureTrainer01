using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameObstacleNavigation : MiniGameBase {

	public static bool debugFunctionCalls = true; // turns debug messages on/off

	public float[][] wormSegmentArray_PosX;
	public float[][] wormSegmentArray_PosY;
	public float[][] wormSegmentArray_PosZ;
	public float[][] wormSegmentArray_Angle;
	public float[][] wormSegmentArray_MotorTarget;
	public float[][] wormSegmentArray_Length;

	public float[] targetPosX = new float[1];
	public float[] targetPosY = new float[1];
	public float[] targetPosZ = new float[1];
	public float[] targetDirX = new float[1];
	public float[] targetDirY = new float[1];
	public float[] targetDirZ = new float[1];

	public float[] clockValue = new float[1];
	public float[] targetHeadDot = new float[1];
	public float[] targetHeadDotHor = new float[1];
	public float[] targetHeadDotVer = new float[1];

	// Game Settings:
	public float[] clockFrequency = new float[1];
	public float[] armTotalLength = new float[1];
	public float[] armSegmentMaxBend = new float[1];
	public float[] armSegmentBendTwoAxis = new float[1];
	public int numberOfSegments = 17;
	public float[] wormSegmentThicknessHorMax = new float[1];
	public float[] wormSegmentThicknessVerMax = new float[1];
	public float[][] wormSegmentArray_ThicknessHor;
	public float[][] wormSegmentArray_ThicknessVer;

	public float[] viscosityDrag = new float[1]; //50f;
	public float[] jointMotorForce = new float[1]; //100f;
	public float[] jointMotorSpeed = new float[1]; //500f;
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
	public float[] fitTimeToTarget = new float[1];
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

	// Constructor!!
	public MiniGameObstacleNavigation() {
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

		sa_x = new float[numberOfSegments];
		sa_y = new float[numberOfSegments];
		sa_z = new float[numberOfSegments];

		targetDirX[0] = 0f;
		targetDirY[0] = 0f;
		targetDirZ[0] = 0f;
		targetPosAxis[0] = 0f;
		clockValue[0] = 0f;

		// GAME OPTIONS INIT:
		clockFrequency[0] = 12f;
		viscosityDrag[0] = 35f;
		jointMotorForce[0] = 600f;
		jointMotorSpeed[0] = 400f;
		targetX[0] = 1f;
		targetY[0] = 0f;
		targetZ[0] = 0f;
		maxScoreDistance[0] = 3f;
		moveSpeedMaxFit[0] = 0.03f;
		targetRadius[0] = 1f;
		armTotalLength[0] = 2.0f;
		armSegmentMaxBend[0] = 45f;
		armSegmentBendTwoAxis[0] = 1f;
		wormSegmentThicknessHorMax[0] = 0.25f;
		wormSegmentThicknessVerMax[0] = 0.25f;

		GOwormSegments = new GameObject[numberOfSegments];
		wormSegmentArray_PosX = new float[numberOfSegments][];
		wormSegmentArray_PosY = new float[numberOfSegments][];
		wormSegmentArray_PosZ = new float[numberOfSegments][];
		wormSegmentArray_Angle = new float[numberOfSegments][];
		wormSegmentArray_MotorTarget = new float[numberOfSegments][];
		wormSegmentArray_Length = new float[numberOfSegments][];
		wormSegmentArray_ThicknessHor = new float[numberOfSegments][];
		wormSegmentArray_ThicknessVer = new float[numberOfSegments][];

		//GOtargetSphere = new GameObject("GOtargetSphere");
		//GOtargetSphere.transform.localScale = new Vector3(targetRadius, targetRadius, targetRadius);

		// For EACH SEGMENT INIT:
		for(int i = 0; i < numberOfSegments; i++) {
			//string name = "GOwormSegment" + i.ToString();
			//GOwormSegments[i] = new GameObject(name);
			//GOwormSegments[i].transform.localPosition = new Vector3(0f, 0f, 0f); // RE-EVALUATE!!!
			wormSegmentArray_PosX[i] = new float[1];
			wormSegmentArray_PosY[i] = new float[1];
			wormSegmentArray_PosZ[i] = new float[1];
			wormSegmentArray_Angle[i] = new float[1];
			wormSegmentArray_MotorTarget[i] = new float[1];
			wormSegmentArray_Length[i] = new float[1];
			wormSegmentArray_ThicknessHor[i] = new float[1];
			wormSegmentArray_ThicknessVer[i] = new float[1];
			wormSegmentArray_Length[i][0] = armTotalLength[0]/(float)numberOfSegments;
			float indexRatio = (i+1f) / numberOfSegments;
			wormSegmentArray_ThicknessHor[i][0] = wormSegmentThicknessHorMax[0]; // * Mathf.Pow (indexRatio, 1.4f);
			wormSegmentArray_ThicknessVer[i][0] = wormSegmentThicknessVerMax[0]; // * Mathf.Pow ((1f - indexRatio) + 1f/numberOfSegments, 1.4f);
			// Calculate surface areas for each face:
			sa_x[i] = wormSegmentArray_ThicknessHor[i][0] * wormSegmentArray_ThicknessVer[i][0];  // Y * Z
			sa_y[i] = wormSegmentArray_Length[i][0] * wormSegmentArray_ThicknessHor[i][0];  // X * Z
			sa_z[i] = wormSegmentArray_Length[i][0] * wormSegmentArray_ThicknessVer[i][0]; // X * Y
		}

		fitDistFromOrigin[0] = 0f;
		fitEnergySpent[0] = 0f;
		fitDistToTarget[0] = 0f;
		fitTimeToTarget[0] = 0f;
		fitTargetHeadDot[0] = 0f;
		fitTargetHeadDotHor[0] = 0f;
		fitTargetHeadDotVer[0] = 0f;
		fitMoveToTarget[0] = 0f;
		fitMoveSpeed[0] = 0f;

		// Brain Inputs!:
		inputChannelsList = new List<BrainInputChannel>();
		// Brain Outputs!:		
		outputChannelsList = new List<BrainOutputChannel>();
		
		for(int bc = 0; bc < numberOfSegments; bc++) {
			string inputChannelName = "Worm Segment " + bc.ToString() + " Angle";
			BrainInputChannel BIC_wormSegmentAngle = new BrainInputChannel(ref wormSegmentArray_Angle[bc], false, inputChannelName);
			inputChannelsList.Add (BIC_wormSegmentAngle);
			
			string outputChannelName = "Worm Segment " + bc.ToString() + " Motor Target";
			BrainOutputChannel BOC_wormSegmentAngleVel = new BrainOutputChannel(ref wormSegmentArray_MotorTarget[bc], false, outputChannelName);
			outputChannelsList.Add (BOC_wormSegmentAngleVel);
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

		// Fitness Component List:
		fitnessComponentList = new List<FitnessComponent>();
		FitnessComponent FC_distFromOrigin = new FitnessComponent(ref fitDistFromOrigin, true, true, 1f, 1f, "Distance From Origin", true);
		fitnessComponentList.Add (FC_distFromOrigin); // 0
		FitnessComponent FC_energySpent = new FitnessComponent(ref fitEnergySpent, true, false, 1f, 1f, "Energy Spent", true);
		fitnessComponentList.Add (FC_energySpent); // 1
		FitnessComponent FC_distToTarget = new FitnessComponent(ref fitDistToTarget, true, false, 1f, 1f, "Distance To Target", false);
		fitnessComponentList.Add (FC_distToTarget); // 2
		FitnessComponent FC_timeToTarget = new FitnessComponent(ref fitTimeToTarget, true, false, 1f, 1f, "Time To Target", true);
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
		GameOptionChannel GOC_jointBendMaxAngle = new GameOptionChannel(ref armSegmentMaxBend, 1f, 150f, "Joint Max Bend Angle");
		gameOptionsList.Add (GOC_jointBendMaxAngle); // 2
		GameOptionChannel GOC_targetRadius = new GameOptionChannel(ref targetRadius, 0.01f, 5f, "Target Size");
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
		GameOptionChannel GOC_maxScoreDistance = new GameOptionChannel(ref maxScoreDistance, 0.001f, 12f, "Move Score Distance");
		gameOptionsList.Add (GOC_maxScoreDistance); // 7
		GameOptionChannel GOC_wormTotalLength = new GameOptionChannel(ref armTotalLength, 0.01f, 5f, "Worm Total Length");
		gameOptionsList.Add (GOC_wormTotalLength); // 8
		GameOptionChannel GOC_wormThicknessHor = new GameOptionChannel(ref wormSegmentThicknessHorMax, 0.01f, 1f, "Worm Thickness Hor");
		gameOptionsList.Add (GOC_wormThicknessHor); // 8
		GameOptionChannel GOC_wormThicknessVer = new GameOptionChannel(ref wormSegmentThicknessVerMax, 0.01f, 1f, "Worm Thickness Ver");
		gameOptionsList.Add (GOC_wormThicknessVer); // 8
		GameOptionChannel GOC_wormBendTwoAxis = new GameOptionChannel(ref armSegmentBendTwoAxis, 0f, 1f, "Bend Two Axis");
		gameOptionsList.Add (GOC_wormBendTwoAxis); // 8

	}

	public void ApplyViscosityForces(GameObject body, int segmentIndex, float drag) {
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

		if(segmentIndex ==0 || segmentIndex >= numberOfSegments) {   // if first or last segment, calculate forces for the one exposed face at front and back
			// RIGHT (posX):
			Vector3 pointVelPosX = rigidBod.GetPointVelocity (xpos_face_center);
			Vector3 fluidDragVecPosX = -right * Vector3.Dot (right, pointVelPosX) * sa_x[segmentIndex] * viscosityDrag[0];  
			rigidBod.AddForceAtPosition (fluidDragVecPosX, xpos_face_center);
		}


		// SQUARED:
		// TOP (posY):
		//Vector3 pointVelPosY = rigidBod.GetPointVelocity (ypos_face_center);
		//float velPosY = Vector3.Dot (up, pointVelPosY) * pointVelPosY.sqrMagnitude;   // get the proportion of the velocity vector in the direction of face's normal (0 - 1) times magnitude squared
		//Vector3 fluidDragVecPosY = -up * velPosY * sa_y[segmentIndex] * viscosityDrag;  
		//rigidBod.AddForceAtPosition (fluidDragVecPosY*2f, ypos_face_center);
	}
	
	public override void Tick() {  // Runs the mini-game for a single evaluation step.
		//Debug.Log ("Tick()");
		// THIS IS ALL PRE- PHYS-X!!! ::

		clockValue[0] = Mathf.Sin (Time.fixedTime * clockFrequency[0]);

		for(int w = 0; w < numberOfSegments; w++) {
			if(GOwormSegments[w].GetComponent<HingeJoint>() != null) {
				JointMotor motor = new JointMotor();
				motor.force = jointMotorForce[0];
				motor.targetVelocity = wormSegmentArray_MotorTarget[w][0] * jointMotorSpeed[0];
				GOwormSegments[w].GetComponent<HingeJoint>().motor = motor;
			}
			ApplyViscosityForces(GOwormSegments[w], w, viscosityDrag[0]);
		}

		// FITNESS COMPONENTS!
		Vector3 avgPos = new Vector3(0f, 0f, 0f);
		for(int e = 0; e < numberOfSegments; e++) {
			avgPos += new Vector3(wormSegmentArray_PosX[e][0], wormSegmentArray_PosY[e][0], wormSegmentArray_PosZ[e][0]);
			fitEnergySpent[0] += Mathf.Abs (wormSegmentArray_MotorTarget[e][0])/(float)numberOfSegments;
		}
		avgPos /= (float)numberOfSegments;
		Vector3 comVel = avgPos - wormCOM; // this frame minus last frame
		wormCOM = avgPos;
		ArenaCameraController.arenaCameraControllerStatic.focusPosition = avgPos;
		Vector3 targetDirection = new Vector3(targetPosX[0] - avgPos.x, targetPosY[0] - avgPos.y, targetPosZ[0] - avgPos.z);
		float distToTarget = targetDirection.magnitude;
		fitDistToTarget[0] = distToTarget / maxScoreDistance[0];
		fitDistFromOrigin[0] += avgPos.magnitude / maxScoreDistance[0];

		Vector3 headToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[0][0], targetPosY[0] - wormSegmentArray_PosY[0][0], targetPosZ[0] - wormSegmentArray_PosZ[0][0]).normalized;
		targetHeadDot[0] = Vector3.Dot (headToTargetVect, -GOwormSegments[0].transform.right);
		targetHeadDotHor[0] = Vector3.Dot (headToTargetVect, GOwormSegments[0].transform.forward);
		targetHeadDotVer[0] = Vector3.Dot (headToTargetVect, GOwormSegments[0].transform.up);
		fitTargetHeadDot[0] += (targetHeadDot[0] * 0.5f + 0.5f);  // (get in 0-1 range) fitnessHeadDot is an average so it is accumulated
		fitTargetHeadDotHor[0] += Mathf.Abs (targetHeadDotHor[0]);
		fitTargetHeadDotVer[0] += Mathf.Abs (targetHeadDotVer[0]);

		fitMoveToTarget[0] += Vector3.Dot (comVel.normalized, targetDirection.normalized) * 0.5f + 0.5f;
		fitMoveSpeed[0] += comVel.magnitude / moveSpeedMaxFit[0];

		targetDirX[0] = targetDirection.x;
		targetDirY[0] = targetDirection.y;
		targetDirZ[0] = targetDirection.z;

		if(distToTarget < targetRadius[0]) {
			fitTimeToTarget[0] += 0f;
		}
		else {
			fitTimeToTarget[0] += 1f;
		}

		gameTicked = true;
	}

	public override void Reset() {
		//Debug.Log ("Reset()");
		float xOffset = armTotalLength[0] * 0.5f;
		wormSegmentArray_PosX[0][0] = 0f - (armTotalLength[0] / 2f) + (armTotalLength[0]/(float)numberOfSegments)*0.5f; // origin of arm at (0, 0);
		wormSegmentArray_PosY[0][0] = 0f;
		wormSegmentArray_PosZ[0][0] = 0f;

		float randX = UnityEngine.Random.Range(-1f, 1f);
		float randY = UnityEngine.Random.Range(-1f, 1f);
		float randZ = UnityEngine.Random.Range(-1f, 1f);
		if(targetX[0] <= 0.5f) {  // if x-axis is enabled, use randX, else set to 0;
			randX = 0f;
		}
		else {
			if(targetPosAxis[0] > 0.5f) {
				randX = Mathf.Abs (randX);
			}
		}
		if(targetY[0] < 0.5f) {  
			randY = 0f;
		}
		else {
			if(targetPosAxis[0] > 0.5f) {
				randY = Mathf.Abs (randY);
			}
		}
		if(targetZ[0] < 0.5f) {  // if x-axis is enabled, use randX, else set to 0;
			randZ = 0f;
		}
		else {
			if(targetPosAxis[0] > 0.5f) {
				randZ = Mathf.Abs (randZ);
			}
		}

		//Vector3 randDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
		Vector3 randDir = new Vector3(randX, randY, randZ).normalized;
		//randDir = (randDir * 2f) / randDir.magnitude;

		clockValue[0] = 0f;
		targetPosX[0] = randDir.x * maxScoreDistance[0];
		targetPosY[0] = randDir.y * maxScoreDistance[0];
		targetPosZ[0] = randDir.z * maxScoreDistance[0];
		//targetPosX[0] = UnityEngine.Random.Range(targetX[0], targetMaxX[0]);
		//targetPosY[0] = UnityEngine.Random.Range(targetMinY[0], targetMaxY[0]);
		//targetPosZ[0] = UnityEngine.Random.Range(targetMinZ[0], targetMaxZ[0]);

		// Game Pieces!!!!! // maybe put into their own function eventually?
		for(int w = 0; w < numberOfSegments; w++) {
			wormSegmentArray_Length[w][0] = armTotalLength[0]/(float)numberOfSegments;
			wormSegmentArray_Angle[w][0] = 0f;
			wormSegmentArray_MotorTarget[w][0] = 0f;

			if(w != 0) { // if not the root segment:
				wormSegmentArray_PosX[w][0] = wormSegmentArray_PosX[w-1][0] + wormSegmentArray_Length[w-1][0]*0.5f + wormSegmentArray_Length[w][0]*0.5f; // - (armTotalLength[0] / 2f);
			}
			wormSegmentArray_PosY[w][0] = 0f;
			wormSegmentArray_PosZ[w][0] = 0f;

			//Debug.Log ("Reset() segmentPos: " + new Vector3(wormSegmentArray_PosX[w][0], wormSegmentArray_PosY[w][0], wormSegmentArray_PosZ[w][0]).ToString());
		
			//GOwormSegments[i].transform.localScale = new Vector3(wormSegmentArray_Length[i][0], wormSegmentThickness, wormSegmentThickness*2f);
			sa_x[w] = wormSegmentArray_ThicknessHor[w][0] * wormSegmentArray_ThicknessVer[w][0];  // Y * Z
			sa_y[w] = wormSegmentArray_Length[w][0] * wormSegmentArray_ThicknessHor[w][0];  // X * Z
			sa_z[w] = wormSegmentArray_Length[w][0] * wormSegmentArray_ThicknessVer[w][0]; // X * Y
		}

		wormCOM.x = 0f;
		wormCOM.y = 0f;
		wormCOM.z = 0f;

		// FITNESS COMPONENTS!:
		fitDistFromOrigin[0] = 0f;
		fitEnergySpent[0] = 0f;
		fitDistToTarget[0] = 0f;
		fitTimeToTarget[0] = 0f;
		fitTargetHeadDot[0] = 0f;
		fitTargetHeadDotHor[0] = 0f;
		fitTargetHeadDotVer[0] = 0f;
		fitMoveToTarget[0] = 0f;
		fitMoveSpeed[0] = 0f;

		gameInitialized = true;
		gameTicked = false;
		gameUpdatedFromPhysX = false;
		gameCurrentTimeStep = 0;  // reset to 0
	}

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

				if(w < (numberOfSegments - 1)) {  // if not the final 'Tail' segment:
					wormSegmentArray_Angle[w][0] = GOwormSegments[w].GetComponent<HingeJoint>().angle;
					// Motor force? think that should be a one-way path, set from gameData
				}
			}
		}
		gameUpdatedFromPhysX = true;
		SetNonPhysicsGamePieceTransformsFromData();  // debug objects that rely on PhysX object positions
	}

	public override void InstantiateGamePieces() {
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
		
		for(int i = 0; i < numberOfSegments; i++) {
			string name = "GOwormSegment" + i.ToString();
			GOwormSegments[i] = new GameObject(name);
			GOwormSegments[i].transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
			GOwormSegments[i].AddComponent<GamePiecePhysXWormSegment>().InitGamePiece();
			GOwormSegments[i].GetComponent<Renderer>().material = segmentMaterial;
		}
	}
	
	public override void UninstantiateGamePieces() {
		
	}
	
	public override void EnablePhysicsGamePieceComponents() {
		//Debug.Log ("BuildGamePieceComponents()");

		for(int w = 0; w < numberOfSegments; w++) {
			//if(GOwormSegments[w].GetComponent<GamePiecePhysXWormSegment>() == null) {
			//	GOwormSegments[w].AddComponent<Rigidbody>().useGravity = false;
			//}
			GOwormSegments[w].GetComponent<Rigidbody>().useGravity = false;
			GOwormSegments[w].GetComponent<Rigidbody>().isKinematic = false;  //////////////////////////////////
			if(w < (numberOfSegments - 1)) {  // if not the final 'Tail' segment:
				
			}
			if(w > 0) {  // if not the first root segment:
				HingeJoint hingeJoint;
				if(GOwormSegments[w-1].GetComponent<HingeJoint>() == null) {
					hingeJoint = GOwormSegments[w-1].AddComponent<HingeJoint>();
				}
				else {
					hingeJoint = GOwormSegments[w-1].GetComponent<HingeJoint>();
				}
				hingeJoint.autoConfigureConnectedAnchor = false;
				hingeJoint.connectedBody = GOwormSegments[w].GetComponent<Rigidbody>(); // connected bod of previous segment is THIS segment
				hingeJoint.anchor = new Vector3(0.5f, 0f, 0f);
				if(armSegmentBendTwoAxis[0] > 0.5f) {  // if two-axis bend enabled
					if(w % 2 == 0) {
						hingeJoint.axis = new Vector3(0f, 0f, 1f);
					}
					else {
						
						hingeJoint.axis = new Vector3(0f, 1f, 0f);
					}
				}
				else {
					hingeJoint.axis = new Vector3(0f, 0f, 1f);
				}
				hingeJoint.connectedAnchor = new Vector3(-0.5f, 0f, 0f);
				JointLimits jointLimits = new JointLimits();
				jointLimits.max = armSegmentMaxBend[0];
				jointLimits.min = -armSegmentMaxBend[0];
				hingeJoint.limits = jointLimits;
				hingeJoint.useLimits = true;
				hingeJoint.useMotor = true;
				JointMotor motor = new JointMotor();
				motor.force = jointMotorForce[0];  // needed???
				hingeJoint.motor = motor;				
			}
		}		
		piecesBuilt = true;
	}
	
	public override void DisablePhysicsGamePieceComponents() { // See if I can just sleep them?
		//Debug.Log ("DestroyGamePieceComponents()");
		for(int w = 0; w < numberOfSegments; w++) {
			if(GOwormSegments[w].GetComponent<Rigidbody>() != null) {
				GOwormSegments[w].GetComponent<Rigidbody>().isKinematic = true;
				if(GOwormSegments[w].GetComponent<HingeJoint>() != null) {
					GOwormSegments[w].GetComponent<HingeJoint>().connectedBody = null;
				}
			}	
		}
		piecesBuilt = false;
	}

	public override void SetPhysicsGamePieceTransformsFromData() {
		//Debug.Log ("SetPhysicsGamePieceTransformsFromData()");
		for(int w = 0; w < numberOfSegments; w++) {
			GOwormSegments[w].transform.localScale = new Vector3(wormSegmentArray_Length[w][0], wormSegmentArray_ThicknessVer[w][0], wormSegmentArray_ThicknessHor[w][0]);
			GOwormSegments[w].transform.position = new Vector3(wormSegmentArray_PosX[w][0], wormSegmentArray_PosY[w][0], wormSegmentArray_PosZ[w][0]); // RE-EVALUATE!!!
			GOwormSegments[w].transform.localRotation = Quaternion.identity;
		}
	}

	public override void SetNonPhysicsGamePieceTransformsFromData() {
		Vector3 headToTargetVect = new Vector3(targetPosX[0] - wormSegmentArray_PosX[0][0], targetPosY[0] - wormSegmentArray_PosY[0][0], targetPosZ[0] - wormSegmentArray_PosZ[0][0]);
		// POSITION!
		GOheadToTargetVector.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + (headToTargetVect / 2f);	// halfway between head center and target center
		GOheadToTargetVectorHor.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.forward * (debugVectorLength / 2f);
		GOheadToTargetVectorVer.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + GOwormSegments[0].transform.up * (debugVectorLength / 2f);	
		GOheadFacingVector.transform.position = new Vector3(wormSegmentArray_PosX[0][0], wormSegmentArray_PosY[0][0], wormSegmentArray_PosZ[0][0]) + -1f*GOwormSegments[0].transform.right * (debugVectorLength / 2f);;
		GOwormCenterOfMass.transform.position = wormCOM; // average center of mass
		// ROTATION!
		GOheadToTargetVector.transform.rotation = Quaternion.LookRotation(headToTargetVect);
		GOheadToTargetVectorHor.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.forward); // Figure this out!
		GOheadToTargetVectorVer.transform.rotation = Quaternion.LookRotation(GOwormSegments[0].transform.up);
		GOheadFacingVector.transform.rotation = Quaternion.LookRotation(-GOwormSegments[0].transform.right);  // grab direction from head segment
		// SCALE!
		float lengthScale = headToTargetVect.magnitude;
		GOheadToTargetVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, lengthScale);

		GOtargetSphere.transform.localPosition = new Vector3(targetPosX[0], targetPosY[0], targetPosZ[0]);
		GOtargetSphere.transform.localScale = new Vector3(targetRadius[0], targetRadius[0], targetRadius[0]);
	}

}
