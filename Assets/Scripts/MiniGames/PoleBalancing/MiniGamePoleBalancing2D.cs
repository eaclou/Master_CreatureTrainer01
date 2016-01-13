using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//==============================================================================================
//  Very Simple minigame to be used as a benchmark
//  Consists of a cart with a number of vertical poles that it must try to keep upright for as long as possible by moving the cart
//==============================================================================================

public class MiniGamePoleBalancing2D : MiniGameBase {

	public static bool debugFunctionCalls = true; // turns debug messages on/off

	// generally Inputs:
	public float[] poleAngle1 = new float[1];
	public float[] poleAngle2 = new float[1];
	public float[] poleAngle3 = new float[1];
	public float[] poleAngularVel1 = new float[1];  // optional inputs
	public float[] poleAngularVel2 = new float[1];
	public float[] poleAngularVel3 = new float[1];
	public float[] cartPosition = new float[1];
	public float[] cartVelocity = new float[1];

	// generally outputs:
	public float[] cartForce = new float[1]; // the output value, what force to apply to the cart per tick()

	// Game Option Settings:
	public float[] numberOfPoles = new float[1];  // 1-3
	public float[] poleLength1 = new float[1];  // length of first pole
	public float[] poleLength2 = new float[1];  // length of second pole
	public float[] poleLength3 = new float[1];  // length of third pole
	public float[] maxInitialPoleOffset = new float[1];  // maximum angular velocity that the pole will be initialized with, to prevent cart from winning by staying motionless
	public float[] maxCartPushForce = new float[1];  // maximum amount of force that the NN can apply to the cart
	public float[] maxCartDistance = new float[1];  // prevents cart from flying off in one direction forever?
	public float[] gravityStrength = new float[1];

	public float cartWidth = 2f;
	public float cartHeight = 0.2f;
	public float poleWidth = 0.06f;
	public bool poleUpright1 = true;
	public bool poleUpright2 = true;
	public bool poleUpright3 = true;
	// Debug & Diagnostic:

	// Fitness Component Scores:
	public float[] fitTimeUpright = new float[1];
	public float[] fitEnergySpent = new float[1];
	public float[] fitPoleVerticality1 = new float[1];
	public float[] fitPoleVerticality2 = new float[1];
	public float[] fitPoleVerticality3 = new float[1];

	// Game Pieces!
	private GameObject GOcartBase;
	private GameObject GOpole1;
	private GameObject GOpole2;
	private GameObject GOpole3;
	private GameObject GOgroundPlane;

	// Materials!
	Material cartMat = new Material (Shader.Find("Standard"));
	Material poleMat1 = new Material (Shader.Find("Standard"));
	Material poleMat2 = new Material (Shader.Find("Standard"));
	Material poleMat3 = new Material (Shader.Find("Standard"));
	Material groundMat = new Material (Shader.Find("Standard"));

	// Constructor!!
	public MiniGamePoleBalancing2D() {
		piecesBuilt = false;  // This refers to if the pieces have all their components and are ready for Simulation, not simply instantiated empty GO's
		gameInitialized = false;  // Reset() is Initialization
		gameTicked = false;  // Has the game been ticked on its current TimeStep
		gameUpdatedFromPhysX = false;  // Did the game just get updated from PhysX Simulation?
		gameCurrentTimeStep = 0;  // Should only be able to increment this if the above are true (which means it went through gameLoop for this timeStep)

		cartMat.color = new Color(1f, 1f, 1f);
		poleMat1.color = new Color(1f, 0.25f, 0.25f);
		poleMat2.color = new Color(0.25f, 1f, 0.25f);
		poleMat3.color = new Color(0.25f, 0.25f, 1f);
		groundMat.color = new Color(0.25f, 0.25f, 0.25f);

		// generally Inputs:
		poleAngle1[0] = 0f;
		poleAngle2[0] = 0f;
		poleAngle3[0] = 0f;
		poleAngularVel1[0] = 0f;
		poleAngularVel2[0] = 0f;
		poleAngularVel3[0] = 0f;
		cartPosition[0] = 0f;
		cartVelocity[0] = 0f;		
		// generally outputs:
		cartForce[0] = 0f;

		// GAME OPTIONS INIT:
		numberOfPoles[0] = 2f;  // 1-3
		poleLength1[0] = 2f;  // length of first pole
		poleLength2[0] = 0.5f;  // length of second pole
		poleLength3[0] = 0.2f;  // length of third pole
		maxInitialPoleOffset[0] = 0.2f;  // sets poles off-center so they begin falling
		maxCartPushForce[0] = 360f;  // maximum amount of force that the NN can apply to the cart
		maxCartDistance[0] = 5f;  // prevents cart from flying off in one direction forever?
		gravityStrength[0] = 9.8f;

		// Fitness Component Scores:
		fitTimeUpright[0] = 0f;
		fitEnergySpent[0] = 0f;
		fitPoleVerticality1[0] = 0f;
		fitPoleVerticality2[0] = 0f;
		fitPoleVerticality3[0] = 0f;

		// Brain Inputs!:
		inputChannelsList = new List<BrainInputChannel>();
		BrainInputChannel BIC_poleAngle1 = new BrainInputChannel(ref poleAngle1, false, "Pole 1 Angle");
		inputChannelsList.Add (BIC_poleAngle1);
		BrainInputChannel BIC_poleAngle2 = new BrainInputChannel(ref poleAngle2, false, "Pole 2 Angle");
		inputChannelsList.Add (BIC_poleAngle2);
		BrainInputChannel BIC_poleAngle3 = new BrainInputChannel(ref poleAngle3, false, "Pole 3 Angle");
		inputChannelsList.Add (BIC_poleAngle3);
		BrainInputChannel BIC_poleAngularVel1 = new BrainInputChannel(ref poleAngularVel1, false, "Pole 1 Angular Vel");
		inputChannelsList.Add (BIC_poleAngularVel1);
		BrainInputChannel BIC_poleAngularVel2 = new BrainInputChannel(ref poleAngularVel2, false, "Pole 2 Angular Vel");
		inputChannelsList.Add (BIC_poleAngularVel2);
		BrainInputChannel BIC_poleAngularVel3 = new BrainInputChannel(ref poleAngularVel3, false, "Pole 3 Angular Vel");
		inputChannelsList.Add (BIC_poleAngularVel3);
		BrainInputChannel BIC_cartPosition = new BrainInputChannel(ref cartPosition, false, "Cart Position");
		inputChannelsList.Add (BIC_cartPosition);
		BrainInputChannel BIC_cartVelocity = new BrainInputChannel(ref cartVelocity, false, "Cart Velocity");
		inputChannelsList.Add (BIC_cartVelocity);

		// Brain Outputs!:		
		outputChannelsList = new List<BrainOutputChannel>();
		BrainOutputChannel BOC_cartForce = new BrainOutputChannel(ref cartForce, false, "Cart Force");
		outputChannelsList.Add (BOC_cartForce);

		// Fitness Component List:
		fitnessComponentList = new List<FitnessComponent>();
		FitnessComponent FC_timeUpright = new FitnessComponent(ref fitTimeUpright, true, true, 1f, 1f, "Time Upright", true);
		fitnessComponentList.Add (FC_timeUpright); // 0
		FitnessComponent FC_energySpent = new FitnessComponent(ref fitEnergySpent, true, false, 1f, 1f, "Energy Spent", true);
		fitnessComponentList.Add (FC_energySpent); // 1
		FitnessComponent FC_poleVerticality1 = new FitnessComponent(ref fitPoleVerticality1, true, true, 1f, 1f, "Pole 1 Verticality", true);
		fitnessComponentList.Add (FC_poleVerticality1); // 2
		FitnessComponent FC_poleVerticality2 = new FitnessComponent(ref fitPoleVerticality2, true, true, 1f, 1f, "Pole 2 Verticality", true);
		fitnessComponentList.Add (FC_poleVerticality2); // 3
		FitnessComponent FC_poleVerticality3 = new FitnessComponent(ref fitPoleVerticality3, true, true, 1f, 1f, "Pole 3 Verticality", true);
		fitnessComponentList.Add (FC_poleVerticality3); // 4
		
		// Game Options List:
		gameOptionsList = new List<GameOptionChannel>();
		GameOptionChannel GOC_numberOfPoles = new GameOptionChannel(ref numberOfPoles, 1f, 3f, "Number Of Poles");
		gameOptionsList.Add (GOC_numberOfPoles); // 0
		GameOptionChannel GOC_poleLength1 = new GameOptionChannel(ref poleLength1, 0.05f, 10f, "Pole 1 Length");
		gameOptionsList.Add (GOC_poleLength1); // 1
		GameOptionChannel GOC_poleLength2 = new GameOptionChannel(ref poleLength2, 0.05f, 10f, "Pole 2 Length");
		gameOptionsList.Add (GOC_poleLength2); // 2
		GameOptionChannel GOC_poleLength3 = new GameOptionChannel(ref poleLength3, 0.05f, 10f, "Pole 3 Length");
		gameOptionsList.Add (GOC_poleLength3); // 3
		GameOptionChannel GOC_maxInitialPoleOffset = new GameOptionChannel(ref maxInitialPoleOffset, 0f, 10f, "Max Initial Pole Offset");
		gameOptionsList.Add (GOC_maxInitialPoleOffset); // 4
		GameOptionChannel GOC_maxCartPushForce = new GameOptionChannel(ref maxCartPushForce, 0.01f, 500f, "Max Cart Push Force");
		gameOptionsList.Add (GOC_maxCartPushForce); // 5
		GameOptionChannel GOC_maxCartDistance = new GameOptionChannel(ref maxCartDistance, 1f, 20f, "Max Cart Distance");
		gameOptionsList.Add (GOC_maxCartDistance); // 6
		GameOptionChannel GOC_gravityStrength = new GameOptionChannel(ref gravityStrength, 0.01f, 50f, "Gravity Strength");
		gameOptionsList.Add (GOC_gravityStrength); // 7

	}

	public override void Tick() {  // Runs the mini-game for a single evaluation step.
		//Debug.Log ("Tick()");
		// THIS IS ALL PRE- PHYS-X!!! ::

		GOcartBase.GetComponent<Rigidbody>().AddForce(Vector3.right * cartForce[0] * maxCartPushForce[0]);
		//

		// FITNESS COMPONENTS!

		fitEnergySpent[0] += Mathf.Abs (cartForce[0]);


		if(Mathf.Abs (poleAngle1[0]) >= 90f) {
			poleUpright1 = false;
			//Debug.Log ("poleUpright1 FALSE: " + gameCurrentTimeStep.ToString());
		}
		if(Mathf.Abs (poleAngle2[0]) >= 90f) {
			poleUpright2 = false;
		}
		if(Mathf.Abs (poleAngle3[0]) >= 90f) {
			poleUpright3 = false;
		}
		if(Mathf.Abs (cartPosition[0]) >= maxCartDistance[0]) {
			poleUpright1 = false;
			poleUpright2 = false;
			poleUpright3 = false;
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) == 1) {
			if(poleUpright1) { fitTimeUpright[0] += 1f; }
			else { gameEndStateReached = true; }
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) == 2) {
			if(poleUpright1 && poleUpright2) { fitTimeUpright[0] += 1f; }
			else { gameEndStateReached = true; }
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) == 3) {
			if(poleUpright1 && poleUpright2 && poleUpright3) { fitTimeUpright[0] += 1f; }
			else { gameEndStateReached = true; }
		}

		if(poleUpright1) {
			fitPoleVerticality1[0] += (90f - Mathf.Abs (poleAngle1[0])) / 90f; // MODIFY LATER!!!!!!!!!!!!!!!!
		}
		if(poleUpright1 && poleUpright2) {
			fitPoleVerticality2[0] += (90f - Mathf.Abs (poleAngle2[0])) / 90f; // MODIFY LATER!!!!!!!!!!!!!!!!
		}
		if(poleUpright1 && poleUpright2 && poleUpright3) {
			fitPoleVerticality3[0] += (90f - Mathf.Abs (poleAngle3[0])) / 90f; // MODIFY LATER!!!!!!!!!!!!!!!!
		}

		ArenaCameraController.arenaCameraControllerStatic.focusPosition = new Vector3(cartPosition[0], 0f, 0f);

		gameTicked = true;
	}

	public override void Reset() {
		//Debug.Log ("Reset()");
		//Debug.Log ("Last upright score: " + fitTimeUpright[0].ToString());
		cartPosition[0] = 0f;
		cartVelocity[0] = 0f;
		cartForce[0] = 0f;

		poleAngle1[0] = 0f;
		poleAngle2[0] = 0f;
		poleAngle3[0] = 0f;
		poleAngularVel1[0] = 0f;
		poleAngularVel2[0] = 0f;
		poleAngularVel3[0] = 0f;

		// FITNESS COMPONENTS!:
		fitTimeUpright[0] = 0f;
		fitEnergySpent[0] = 0f;
		fitPoleVerticality1[0] = 0f;
		fitPoleVerticality2[0] = 0f;
		fitPoleVerticality3[0] = 0f;

		poleUpright1 = true;
		poleUpright2 = true;
		poleUpright3 = true;

		gameInitialized = true;
		gameTicked = false;
		gameUpdatedFromPhysX = false;
		gameCurrentTimeStep = 0;  // reset to 0
	}

	public override void UpdateGameStateFromPhysX() {
		//Debug.Log ("UpdateGameStateFromPhysX()");
		// PhysX simulation happened after miniGame Tick(), so before passing gameStateData to brain, need to update gameStateData from the rigidBodies
		// SO that the correct updated input values can be sent to the brain
		cartPosition[0] = GOcartBase.GetComponent<Rigidbody>().position.x;
		cartVelocity[0] = GOcartBase.GetComponent<Rigidbody>().velocity.x;

		poleAngle1[0] = GOpole1.GetComponent<HingeJoint>().angle;
		poleAngularVel1[0] = GOpole1.GetComponent<HingeJoint>().velocity;  // MAKE SURE THIS GIVES CORRECT VALUE!!!
		if(Mathf.RoundToInt(numberOfPoles[0]) >= 2) {
			poleAngle2[0] = GOpole2.GetComponent<HingeJoint>().angle;
			poleAngularVel2[0] = GOpole2.GetComponent<HingeJoint>().velocity;  // MAKE SURE THIS GIVES CORRECT VALUE!!!
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) >= 3) {
			poleAngle3[0] = GOpole3.GetComponent<HingeJoint>().angle;
			poleAngularVel3[0] = GOpole3.GetComponent<HingeJoint>().velocity;  // MAKE SURE THIS GIVES CORRECT VALUE!!!
		}

		gameUpdatedFromPhysX = true;
		SetNonPhysicsGamePieceTransformsFromData();  // debug objects that rely on PhysX object positions
	}

	public override void InstantiateGamePieces() {

		GOcartBase = new GameObject("GOcartBase");
		GOcartBase.transform.localScale = new Vector3(cartWidth, cartHeight, cartWidth);
		GOcartBase.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOcartBase.AddComponent<GamePieceCommonPhysicsCube>().InitGamePiece();
		GOcartBase.GetComponent<Rigidbody>().mass = 5f;
		GOcartBase.GetComponent<Renderer>().material = cartMat;
		GOcartBase.AddComponent<BoxCollider>().enabled = false;
		GOcartBase.GetComponent<BoxCollider>().material.staticFriction = 0f;
		GOcartBase.GetComponent<BoxCollider>().material.dynamicFriction = 0f;
		GOcartBase.GetComponent<BoxCollider>().material.frictionCombine = PhysicMaterialCombine.Minimum;
		GOcartBase.GetComponent<BoxCollider>().material.bounciness = 0f;
		GOcartBase.GetComponent<BoxCollider>().material.bounceCombine = PhysicMaterialCombine.Minimum;

		GOpole1 = new GameObject("GOpole1");
		GOpole1.transform.localScale = new Vector3(poleWidth, poleLength1[0], poleWidth);
		GOpole1.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOpole1.AddComponent<GamePieceCommonPhysicsCube>().InitGamePiece();
		GOpole1.GetComponent<Rigidbody>().mass = 0.5f;
		GOpole1.GetComponent<Rigidbody>().angularDrag = 0f;
		GOpole1.GetComponent<Renderer>().material = poleMat1;

		if(Mathf.RoundToInt(numberOfPoles[0]) >= 2) {
			GOpole2 = new GameObject("GOpole2");
			GOpole2.transform.localScale = new Vector3(poleWidth, poleLength2[0], poleWidth);
			GOpole2.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
			GOpole2.AddComponent<GamePieceCommonPhysicsCube>().InitGamePiece();
			GOpole2.GetComponent<Rigidbody>().mass = 0.5f;
			GOpole2.GetComponent<Rigidbody>().angularDrag = 0f;
			GOpole2.GetComponent<Renderer>().material = poleMat2;
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) >= 3) {
			GOpole3 = new GameObject("GOpole3");
			GOpole3.transform.localScale = new Vector3(poleWidth, poleLength3[0], poleWidth);
			GOpole3.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
			GOpole3.AddComponent<GamePieceCommonPhysicsCube>().InitGamePiece();
			GOpole3.GetComponent<Rigidbody>().mass = 0.5f;
			GOpole3.GetComponent<Rigidbody>().angularDrag = 0f;
			GOpole3.GetComponent<Renderer>().material = poleMat3;
		}

		GOgroundPlane = new GameObject("GOgroundPlane");
		GOgroundPlane.transform.localScale = new Vector3(maxCartDistance[0]*2f, 1f, maxCartDistance[0]*2f);
		GOgroundPlane.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOgroundPlane.AddComponent<GamePieceCommonPlane>().InitGamePiece();
		//GOgroundPlane.AddComponent<MeshCollider>().sharedMesh = GOgroundPlane.GetComponent<MeshFilter>().sharedMesh;
		GOgroundPlane.AddComponent<MeshCollider>().sharedMesh = GOgroundPlane.GetComponent<MeshFilter>().sharedMesh;
		GOgroundPlane.GetComponent<Renderer>().material = groundMat;
	}
	
	public override void UninstantiateGamePieces() {
		
	}
	
	public override void EnablePhysicsGamePieceComponents() {
		//Debug.Log ("EnablePhysicsGamePieceComponents()");

		// Ground doesn't have a rigidBody, only Collision, since it never moves

		HingeJoint hingeJointPole1;
		if(GOpole1.GetComponent<HingeJoint>() == null) {
			hingeJointPole1 = GOpole1.AddComponent<HingeJoint>();
		}
		else {
			hingeJointPole1 = GOpole1.GetComponent<HingeJoint>();
		}
		hingeJointPole1.autoConfigureConnectedAnchor = false;
		hingeJointPole1.connectedBody = GOcartBase.GetComponent<Rigidbody>();
		hingeJointPole1.anchor = new Vector3(-0.5f, -0.5f, 0f);
		hingeJointPole1.axis = new Vector3(0f, 0f, 1f);
		hingeJointPole1.connectedAnchor = new Vector3(-0.5f, 0.5f, 0f);

		if(Mathf.RoundToInt(numberOfPoles[0]) >= 2) {
			HingeJoint hingeJointPole2;
			if(GOpole2.GetComponent<HingeJoint>() == null) {
				hingeJointPole2 = GOpole2.AddComponent<HingeJoint>();
			}
			else {
				hingeJointPole2 = GOpole2.GetComponent<HingeJoint>();
			}
			hingeJointPole2.autoConfigureConnectedAnchor = false;
			hingeJointPole2.connectedBody = GOcartBase.GetComponent<Rigidbody>();
			hingeJointPole2.anchor = new Vector3(0f, -0.5f, 0f);
			hingeJointPole2.axis = new Vector3(0f, 0f, 1f);
			hingeJointPole2.connectedAnchor = new Vector3(0f, 0.5f, 0f);
			
			GOpole2.GetComponent<Rigidbody>().isKinematic = false;
		}

		if(Mathf.RoundToInt(numberOfPoles[0]) >= 3) {
			HingeJoint hingeJointPole3;
			if(GOpole3.GetComponent<HingeJoint>() == null) {
				hingeJointPole3 = GOpole3.AddComponent<HingeJoint>();
			}
			else {
				hingeJointPole3 = GOpole3.GetComponent<HingeJoint>();
			}
			hingeJointPole3.autoConfigureConnectedAnchor = false;
			hingeJointPole3.connectedBody = GOcartBase.GetComponent<Rigidbody>();
			hingeJointPole3.anchor = new Vector3(0.5f, -0.5f, 0f);
			hingeJointPole3.axis = new Vector3(0f, 0f, 1f);
			hingeJointPole3.connectedAnchor = new Vector3(0.5f, 0.5f, 0f);

			GOpole3.GetComponent<Rigidbody>().isKinematic = false;
		}

		//GOcartBase.AddComponent<BoxCollider>();
		GOcartBase.GetComponent<Rigidbody>().isKinematic = false;
		GOpole1.GetComponent<Rigidbody>().isKinematic = false;			
		GOcartBase.GetComponent<BoxCollider>().enabled = true;

		piecesBuilt = true;
	}
	
	public override void DisablePhysicsGamePieceComponents() { // See if I can just sleep them?
		//Debug.Log ("DisablePhysicsGamePieceComponents()");

		GOcartBase.GetComponent<BoxCollider>().enabled = false;
		GOcartBase.GetComponent<Rigidbody>().isKinematic = true;

		GOpole1.GetComponent<Rigidbody>().isKinematic = true;
		if(GOpole1.GetComponent<HingeJoint>() != null) {
			GOpole1.GetComponent<HingeJoint>().connectedBody = null;
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) >= 2) {
			GOpole2.GetComponent<Rigidbody>().isKinematic = true;
			if(GOpole2.GetComponent<HingeJoint>() != null) {
				GOpole2.GetComponent<HingeJoint>().connectedBody = null;
			}
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) >= 3) {
			GOpole3.GetComponent<Rigidbody>().isKinematic = true;
			if(GOpole3.GetComponent<HingeJoint>() != null) {
				GOpole3.GetComponent<HingeJoint>().connectedBody = null;
			}
		}

		piecesBuilt = false;
	}

	public override void SetPhysicsGamePieceTransformsFromData() {
		//Debug.Log ("SetPhysicsGamePieceTransformsFromData() cartPosX: " + cartPosition[0].ToString() + ", rigidBodPos: " + GOcartBase.GetComponent<Rigidbody>().transform.position.x.ToString());
		float randOffset1 = UnityEngine.Random.Range(-maxInitialPoleOffset[0], maxInitialPoleOffset[0]);
		float randOffset2 = UnityEngine.Random.Range(-maxInitialPoleOffset[0], maxInitialPoleOffset[0]);
		float randOffset3 = UnityEngine.Random.Range(-maxInitialPoleOffset[0], maxInitialPoleOffset[0]);

		GOcartBase.transform.localScale = new Vector3(cartWidth, cartHeight, cartWidth);
		GOcartBase.transform.position = new Vector3(cartPosition[0], cartHeight*0.5f, 0f);
		GOcartBase.transform.localRotation = Quaternion.identity;

		GOpole1.transform.localScale = new Vector3(poleWidth, poleLength1[0], cartWidth);
		GOpole1.transform.position = new Vector3(cartPosition[0] - cartWidth/2f + randOffset1, cartHeight + poleLength1[0]/2f, 0f);
		///////// REVEISIT THE ROTATION SETTING!!!!!!!!!!!!!!!!!!!!!
		GOpole1.transform.localRotation = Quaternion.identity;

		if(Mathf.RoundToInt(numberOfPoles[0]) >= 2) {
			GOpole2.transform.localScale = new Vector3(poleWidth, poleLength2[0], cartWidth);
			GOpole2.transform.position = new Vector3(cartPosition[0] + randOffset2, cartHeight + poleLength2[0]/2f, 0f);
			GOpole2.transform.localRotation = Quaternion.identity;
		}
		if(Mathf.RoundToInt(numberOfPoles[0]) >= 3) {
			GOpole3.transform.localScale = new Vector3(poleWidth, poleLength3[0], cartWidth);
			GOpole3.transform.position = new Vector3(cartPosition[0] + cartWidth/2f + randOffset3, cartHeight + poleLength3[0]/2f, 0f);
			GOpole3.transform.localRotation = Quaternion.identity;
		}

		//GOpole1.GetComponent<Rigidbody>().AddForce(Vector3.right * poleAngularVel1[0]);
		//GOpole2.GetComponent<Rigidbody>().AddForce(Vector3.right * poleAngularVel2[0]);
		//GOpole3.GetComponent<Rigidbody>().AddForce(Vector3.right * poleAngularVel3[0]);
	}

	public override void SetNonPhysicsGamePieceTransformsFromData() {

	}

}
