using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//=======================================================================================
//  Designed to test propulsion-based movement and resource management
//  Spaceship has body, engines, shield, sensor, and weapon -- all share a common energy pool that must be distributed optimally
//  Ship gets points for shooting enemy ships, dodging asteroids, and staying alive for as long as possible
//
//=======================================================================================

public class MiniGameSpaceshipShooter : MiniGameBase {

	public static bool debugFunctionCalls = true; // turns debug messages on/off

	// Generally Inputs:
	public float[] ownAngVelDotX = new float[1];  // component of angular velocity relative to ship's orientation, intended to keep track of ship rotation/spin
	public float[] ownAngVelDotY = new float[1];
	public float[] ownAngVelDotZ = new float[1];
	public float[] ownEnergyPool = new float[1]; 
	//public float[] ownEnergyWeapon = new float[1]; 
	//public float[] ownEnergySight = new float[1]; 
	//public float[] ownEnergyShield = new float[1];
	//public float[] ownEnergyThrusters = new float[1]; 
	public float[] ownHealth = new float[1]; 
	public float[] enemyDotX = new float[1];  // 0-1 value for each relative axis to keep track of where enemy is
	public float[] enemyDotY = new float[1];
	public float[] enemyDotZ = new float[1];
	//public float[] enemyVelDotX = new float[1];  // 0-1 value for each relative axis to keep track of how enemy's movement compares to own orientation
	//public float[] enemyVelDotY = new float[1];
	//public float[] enemyVelDotZ = new float[1];
	//public float[] enemyWeaponDotX = new float[1];  // 0-1 value for how enemy weapon is orientated
	//public float[] enemyWeaponDotY = new float[1];  // 0-1 value for how enemy weapon is orientated
	//public float[] enemyWeaponDotZ = new float[1];  // 0-1 value for how enemy weapon is orientated

	// Generally Outputs:
	public float[] transferEnergyToThrusters = new float[1];
	public float[] transferEnergyToWeapon = new float[1];
	//public float[] transferEnergyToSight = new float[1];
	public float[] transferEnergyToShield = new float[1];
	//public float[] ownWeaponDirX = new float[1];  // orientation of own weapon relative to Primary Spaceship Orientation
	//public float[] ownWeaponDirY = new float[1];
	//public float[] ownWeaponDirZ = new float[1];
	public float[][] ownThrusterArrayPower;  // how much power to distribute to each of the thrusters

	// Game Data:
	private Vector3 ownShipWorldPos = new Vector3(0f, 0f, 0f);
	private Vector3 enemyShipWorldPos = new Vector3(0f, 4f, 0f);


	// Game Settings:
	public float[] maxThrusterForce = new float[1];
	public float[] maxFuelBurnWeapon = new float[1];
	public float[] maxFuelBurnShield = new float[1];
	public float[] maxFuelBurnThrusters = new float[1];
	public float[] energyPoolRegenRate = new float[1];
	public float[] ownMaxHealth = new float[1];

	public float ownShipScaleX = 0.5f;
	public float ownShipScaleY = 0.5f;
	public float ownShipScaleZ = 0.25f;
	public int numThrusterRows = 2;
	public int numThrusterColumns = 2;
	public float debugVectorLength = 4f;
	public float debugVectorThickness = 0.02f;
	private Vector3 ownWeaponScale = new Vector3(0.05f, 0.05f, 0.5f);
	private float ownThrusterMaxLength = 0.8f;
	private float ownThrusterWidthMultiplier = 0.35f;
	private float ownShieldRadius = 1f;

	// Fitness Component Scores:
	public float[] fitTimeAlive = new float[1];
	public float[] fitEnemyDot = new float[1];
	public float[] fitEnergySpent = new float[1];

	// Game Pieces!
	private GameObject GOownShipBody;
	private GameObject GOownWeapon;
	private GameObject GOownShield;
	private GameObject[] GOownThrusterArray;
	private GameObject GOownShipForwardVector;
	private GameObject GOownShipUpVector;
	private GameObject GOownShipRightVector;
	private GameObject GOenemyShipBody;

	Material ownShipMat = new Material (Shader.Find("Standard"));
	Material ownWeaponMat = new Material (Shader.Find("Standard"));
	Material ownShieldMat = new Material (Shader.Find("Standard"));
	Material ownThrustersMat = new Material (Shader.Find("Standard"));
	Material enemyShipMat = new Material (Shader.Find("Standard"));
	Material debugForwardVecMat = new Material (Shader.Find("Standard"));
	Material debugUpVecMat = new Material (Shader.Find("Standard"));
	Material debugRightVecMat = new Material (Shader.Find("Standard"));

	#region Constructor!!
	public MiniGameSpaceshipShooter() {
		piecesBuilt = false;  // This refers to if the pieces have all their components and are ready for Simulation, not simply instantiated empty GO's
		gameInitialized = false;  // Reset() is Initialization
		gameTicked = false;  // Has the game been ticked on its current TimeStep
		gameUpdatedFromPhysX = false;  // Did the game just get updated from PhysX Simulation?
		gameCurrentTimeStep = 0;  // Should only be able to increment this if the above are true (which means it went through gameLoop for this timeStep)

		ownShipMat.color = new Color(0.85f, 1f, 0.85f);
		//ownWeaponMat
		ownShieldMat.SetFloat("_Mode", 3);
		ownShieldMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		ownShieldMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		ownShieldMat.SetInt("_ZWrite", 0);
		ownShieldMat.DisableKeyword("_ALPHATEST_ON");
		ownShieldMat.EnableKeyword("_ALPHABLEND_ON");
		ownShieldMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		ownShieldMat.renderQueue = 3000;
		ownShieldMat.color = new Color(0.65f, 1f, 0.25f, 0.3f);

		enemyShipMat.color = new Color(1f, 0.5f, 0.45f);
		debugForwardVecMat.color = new Color(0f, 0f, 0.75f);
		debugUpVecMat.color = new Color(0f, 0.75f, 0f);
		debugRightVecMat.color = new Color(0.75f, 0f, 0f);

		ownEnergyPool[0] = 1f;
		transferEnergyToShield[0] = 0f;
		transferEnergyToWeapon[0] = 0f;

		// GAME OPTIONS INIT:
		maxThrusterForce[0] = 4f;
		maxFuelBurnWeapon[0] = 0.1f;
		maxFuelBurnShield[0] = 0.2f;
		maxFuelBurnThrusters[0] = 0.02f;
		energyPoolRegenRate[0] = 0.015f;
		ownMaxHealth[0] = 1f;

		// Fitness Components!
		fitTimeAlive[0] = 0f;
		fitEnemyDot[0] = 0f;
		fitEnergySpent[0] = 0f;

		// Brain Inputs!:
		inputChannelsList = new List<BrainInputChannel>();
		BrainInputChannel BIC_enemyDotX = new BrainInputChannel(ref enemyDotX, false, "Enemy Dot X");
		inputChannelsList.Add (BIC_enemyDotX);
		BrainInputChannel BIC_enemyDotY = new BrainInputChannel(ref enemyDotY, false, "Enemy Dot Y");
		inputChannelsList.Add (BIC_enemyDotY);
		BrainInputChannel BIC_enemyDotZ = new BrainInputChannel(ref enemyDotZ, false, "Enemy Dot Z");
		inputChannelsList.Add (BIC_enemyDotZ);	
		BrainInputChannel BIC_ownEnergyPool = new BrainInputChannel(ref ownEnergyPool, false, "Own Energy Pool");
		inputChannelsList.Add (BIC_ownEnergyPool);
		//BrainInputChannel BIC_ownHealth = new BrainInputChannel(ref ownHealth, false, "Own Health");
		//inputChannelsList.Add (BIC_ownHealth);
		BrainInputChannel BIC_ownAngVelDotX = new BrainInputChannel(ref ownAngVelDotX, false, "Own AngVel Dot X");
		inputChannelsList.Add (BIC_ownAngVelDotX);
		BrainInputChannel BIC_ownAngVelDotY = new BrainInputChannel(ref ownAngVelDotY, false, "Own AngVel Dot Y");
		inputChannelsList.Add (BIC_ownAngVelDotY);
		BrainInputChannel BIC_ownAngVelDotZ = new BrainInputChannel(ref ownAngVelDotZ, false, "Own AngVel Dot Z");
		inputChannelsList.Add (BIC_ownAngVelDotZ);	

		// Brain Outputs!:		
		outputChannelsList = new List<BrainOutputChannel>();
		ownThrusterArrayPower = new float[numThrusterRows * numThrusterColumns][];
		for(int i = 0; i < ownThrusterArrayPower.Length; i++) {
			ownThrusterArrayPower[i] = new float[1];
			ownThrusterArrayPower[i][0] = 0f;
			string outputChannelName = "Thruster " + i.ToString() + " Power";
			BrainOutputChannel BOC_ownThrusterPower = new BrainOutputChannel(ref ownThrusterArrayPower[i], false, outputChannelName);
			outputChannelsList.Add (BOC_ownThrusterPower);
		}
		//BrainOutputChannel BOC_transferEnergyToThrusters = new BrainOutputChannel(ref transferEnergyToThrusters, false, "Energy To Thrusters");
		//outputChannelsList.Add (BOC_transferEnergyToThrusters);
		BrainOutputChannel BOC_transferEnergyToShield = new BrainOutputChannel(ref transferEnergyToShield, false, "Energy To Shield");
		outputChannelsList.Add (BOC_transferEnergyToShield);
		BrainOutputChannel BOC_transferEnergyToWeapon = new BrainOutputChannel(ref transferEnergyToWeapon, false, "Energy To Weapon");
		outputChannelsList.Add (BOC_transferEnergyToWeapon);


		// Fitness Component List:
		fitnessComponentList = new List<FitnessComponent>();
		FitnessComponent FC_timeAlive = new FitnessComponent(ref fitTimeAlive, true, true, 1f, 1f, "Time Alive", true);
		fitnessComponentList.Add (FC_timeAlive); // 0
		FitnessComponent FC_enemyDot = new FitnessComponent(ref fitEnemyDot, true, true, 1f, 1f, "Enemy Dot Product", true);
		fitnessComponentList.Add (FC_enemyDot); // 0
		FitnessComponent FC_energySpent = new FitnessComponent(ref fitEnergySpent, true, false, 1f, 1f, "Energy Spent", true);
		fitnessComponentList.Add (FC_energySpent); // 0
		
		// Game Options List:
		gameOptionsList = new List<GameOptionChannel>();
		GameOptionChannel GOC_ownMaxHealth = new GameOptionChannel(ref ownMaxHealth, 0.04f, 32f, "Own Max Health");
		gameOptionsList.Add (GOC_ownMaxHealth); // 0
		GameOptionChannel GOC_maxThrusterForce = new GameOptionChannel(ref maxThrusterForce, 0.04f, 32f, "maxThrusterForce");
		gameOptionsList.Add (GOC_maxThrusterForce); // 1
		GameOptionChannel GOC_maxFuelBurnWeapon = new GameOptionChannel(ref maxFuelBurnWeapon, 0.001f, 1f, "maxFuelBurnWeapon");
		gameOptionsList.Add (GOC_maxFuelBurnWeapon); // 2
		GameOptionChannel GOC_maxFuelBurnShield = new GameOptionChannel(ref maxFuelBurnShield, 0.001f, 1f, "maxFuelBurnShield");
		gameOptionsList.Add (GOC_maxFuelBurnShield); // 3
		GameOptionChannel GOC_maxFuelBurnThrusters = new GameOptionChannel(ref maxFuelBurnThrusters, 0.001f, 1f, "maxFuelBurnThrusters");
		gameOptionsList.Add (GOC_maxFuelBurnThrusters); // 4
		GameOptionChannel GOC_energyPoolRegenRate = new GameOptionChannel(ref energyPoolRegenRate, 0.001f, 1f, "energyPoolRegenRate");
		gameOptionsList.Add (GOC_energyPoolRegenRate); // 5

	}
	#endregion

	#region Tick()!
	public override void Tick() {  // Runs the mini-game for a single evaluation step.
		//Debug.Log ("Tick()");
		// THIS IS ALL PRE- PHYS-X!!! ::

		Vector3 forward = GOownShipBody.GetComponent<Rigidbody>().transform.forward;
		Vector3 right = GOownShipBody.GetComponent<Rigidbody>().transform.right;
		Vector3 up = GOownShipBody.GetComponent<Rigidbody>().transform.up;

		// ENERGY DISTRIBUTIONS:
		ownEnergyPool[0] += energyPoolRegenRate[0];

		float fuelBurned = 0f;

		// Apply THRUSTER FORCES:
		for(int x = 0; x < numThrusterRows; x++) {
			for(int y = 0; y < numThrusterColumns; y++) {
				int index = x * numThrusterRows + y;
				Vector3 position = new Vector3();
				position = GOownShipBody.GetComponent<Rigidbody>().transform.position + 
									//(forward * (-ownShipScaleZ / 2f)) +
									(right * ((float)x / ((float)numThrusterRows - 1f) * ownShipScaleX - (ownShipScaleX / 2f))) +
						 			(up * ((float)y / ((float)numThrusterColumns - 1f) * ownShipScaleY - (ownShipScaleY / 2f)));
				float fuelCost = Mathf.Abs (ownThrusterArrayPower[index][0]) * maxFuelBurnThrusters[0];
				float thrust = ownThrusterArrayPower[index][0];
				if(fuelCost >= ownEnergyPool[0]) {  // if fuel cost exceeds Fuel Storage, use rest of energyPool
					//thrust = thrust * (ownEnergyPool[0] / fuelCost);
					fuelCost = ownEnergyPool[0];
				}
				ownEnergyPool[0] -= fuelCost;
				fuelBurned += fuelCost;
				GOownShipBody.GetComponent<Rigidbody>().AddForceAtPosition(forward * thrust * maxThrusterForce[0], position);
				GOownThrusterArray[index].transform.localScale = new Vector3(ownShipScaleX * ownThrusterWidthMultiplier / numThrusterRows, ownShipScaleY * ownThrusterWidthMultiplier / numThrusterColumns, ownThrusterMaxLength * thrust);
			}
		}

		// SHIELD!!!!!!
		float shieldCost = Mathf.Clamp01(transferEnergyToShield[0]) * maxFuelBurnShield[0];
		if(shieldCost >= ownEnergyPool[0]) {  // if fuel cost exceeds Fuel Storage, use rest of energyPool
			shieldCost = ownEnergyPool[0];
		}
		ownEnergyPool[0] -= shieldCost;



		Vector3 enemyShipDir = new Vector3(enemyShipWorldPos.x - ownShipWorldPos.x, enemyShipWorldPos.y - ownShipWorldPos.y, enemyShipWorldPos.z - ownShipWorldPos.z).normalized;
		enemyDotX[0] = Vector3.Dot(enemyShipDir, right);  // 0-1 value for each relative axis to keep track of where enemy is
		enemyDotY[0] = Vector3.Dot(enemyShipDir, up);
		enemyDotZ[0] = Vector3.Dot(enemyShipDir, forward);

		// FITNESS COMPONENTS!
		if(ownHealth[0] > 0f) {
			fitTimeAlive[0] += 1f;
		}
		fitEnemyDot[0] += (enemyDotZ[0] + 1f) * 0.5f;  // get in 0-1 range
		fitEnergySpent[0] += (1f - ownEnergyPool[0]);

		gameTicked = true;
	}
	#endregion

	#region Reset()!
	public override void Reset() {
		//Debug.Log ("Reset()");
		ownShipWorldPos.x = 0f;
		ownShipWorldPos.y = 0f;
		ownShipWorldPos.z = 0f;

		enemyShipWorldPos = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));

		ownHealth[0] = ownMaxHealth[0]; // reset healthbar
		ownEnergyPool[0] = 1f;

		// Fitness Components!
		fitTimeAlive[0] = 0f;
		fitEnemyDot[0] = 0f;
		fitEnergySpent[0] = 0f;

		gameInitialized = true;
		gameTicked = false;
		gameUpdatedFromPhysX = false;
		gameCurrentTimeStep = 0;  // reset to 0
	}
	#endregion

	#region UpdateGameStateFromPhysX()
	public override void UpdateGameStateFromPhysX() {
		//Debug.Log ("UpdateGameStateFromPhysX()");
		// PhysX simulation happened after miniGame Tick(), so before passing gameStateData to brain, need to update gameStateData from the rigidBodies
		// SO that the correct updated input values can be sent to the brain
		ownShipWorldPos = GOownShipBody.GetComponent<Rigidbody>().transform.position;
		//enemyShipWorldPos = GOenemyShipBody.transform.position;

		gameUpdatedFromPhysX = true;
		SetNonPhysicsGamePieceTransformsFromData();  // debug objects that rely on PhysX object positions
	}
	#endregion

	#region InstantiateGamePieces()
	public override void InstantiateGamePieces() {
		GOownShipBody = new GameObject("GOownShipBody");
		//GOownShipBody.transform.localScale = new Vector3(ownShipScaleX, ownShipScaleY, ownShipScaleZ);
		GOownShipBody.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOownShipBody.AddComponent<GamePieceSpaceshipBody>().SetScale(ownShipScaleX, ownShipScaleY, ownShipScaleZ);
		GOownShipBody.GetComponent<GamePieceSpaceshipBody>().InitGamePiece();
		GOownShipBody.GetComponent<Renderer>().material = ownShipMat;
		GOownShipBody.GetComponent<Rigidbody>().useGravity = false;

		GOownWeapon = new GameObject("GOownWeapon");
		GOownWeapon.transform.localScale = new Vector3(ownWeaponScale.x, ownWeaponScale.y, ownWeaponScale.z);
		GOownWeapon.transform.SetParent(GOownShipBody.transform);
		GOownWeapon.AddComponent<GamePieceSpaceshipWeapon>().InitGamePiece();	
		GOownWeapon.GetComponent<Renderer>().material = ownWeaponMat;

		GOownShield = new GameObject("GOownShield");
		GOownShield.transform.localScale = new Vector3(ownShieldRadius, ownShieldRadius, ownShieldRadius);
		GOownShield.transform.SetParent(GOownShipBody.transform);
		GOownShield.AddComponent<GamePieceSpaceshipShield>().InitGamePiece();	
		GOownShield.GetComponent<Renderer>().material = ownShieldMat;

		GOownThrusterArray = new GameObject[numThrusterRows * numThrusterColumns];
		for(int x = 0; x < numThrusterRows; x++) {
			for(int y = 0; y < numThrusterColumns; y++) {
				int index = x * numThrusterRows + y;
				string name = "GOownThrusterArray" + index.ToString();
				GOownThrusterArray[index] = new GameObject(name);
				Vector3 position = new Vector3(((float)x / ((float)numThrusterRows - 1f) * ownShipScaleX - (ownShipScaleX / 2f)), 
				                               ((float)y / ((float)numThrusterColumns - 1f) * ownShipScaleY - (ownShipScaleY / 2f)), 
				                               0f);
				GOownThrusterArray[index].transform.position = position;
				//GOownThrusterArray[index].transform.position = new Vector3((x * ownShipScaleX / (numThrusterRows * 1f)) - (ownShipScaleX / 4f), (y * ownShipScaleY / (numThrusterColumns * 1f)) - (ownShipScaleY / 4f), -ownShipScaleZ / 2f);
				GOownThrusterArray[index].transform.SetParent(GOownShipBody.transform);
				GOownThrusterArray[index].AddComponent<GamePieceSpaceshipThruster>().InitGamePiece();
				GOownThrusterArray[index].GetComponent<Renderer>().material = ownThrustersMat;
			}
		}

		GOownShipForwardVector = new GameObject("GOownShipForwardVector");
		GOownShipForwardVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, debugVectorLength);
		GOownShipForwardVector.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOownShipForwardVector.AddComponent<GamePieceCommonCube>().InitGamePiece();	
		GOownShipForwardVector.GetComponent<Renderer>().material = debugForwardVecMat;

		GOownShipUpVector = new GameObject("GOownShipUpVector");
		GOownShipUpVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, debugVectorLength);
		GOownShipUpVector.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOownShipUpVector.AddComponent<GamePieceCommonCube>().InitGamePiece();	
		GOownShipUpVector.GetComponent<Renderer>().material = debugUpVecMat;

		GOownShipRightVector = new GameObject("GOownShipRightVector");
		GOownShipRightVector.transform.localScale = new Vector3(debugVectorThickness, debugVectorThickness, debugVectorLength);
		GOownShipRightVector.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOownShipRightVector.AddComponent<GamePieceCommonCube>().InitGamePiece();	
		GOownShipRightVector.GetComponent<Renderer>().material = debugRightVecMat;

		GOenemyShipBody = new GameObject("GOenemyShipBody");
		GOenemyShipBody.transform.localScale = new Vector3(1f, 1f, 1f);
		GOenemyShipBody.transform.SetParent(ArenaGroup.arenaGroupStatic.gameObject.transform);
		GOenemyShipBody.AddComponent<GamePieceCommonCube>().InitGamePiece();	
		GOenemyShipBody.GetComponent<Renderer>().material = enemyShipMat;

	}
	#endregion
	public override void UninstantiateGamePieces() {
		
	}
	#region EnablePhysicsGamePieceComponents()
	public override void EnablePhysicsGamePieceComponents() {
		//Debug.Log ("BuildGamePieceComponents()");


		GOownShipBody.GetComponent<Rigidbody>().isKinematic = false; 

				
		piecesBuilt = true;
	}
	#endregion

	#region DisablePhysicsGamePieceComponents()
	public override void DisablePhysicsGamePieceComponents() { // See if I can just sleep them?
		//Debug.Log ("DestroyGamePieceComponents()");

		if(GOownShipBody.GetComponent<Rigidbody>() != null) {
			GOownShipBody.GetComponent<Rigidbody>().isKinematic = true;

		}	

		piecesBuilt = false;
	}
	#endregion

	#region SetPhysicsGamePieceTransformsFromData()
	public override void SetPhysicsGamePieceTransformsFromData() {
		//Debug.Log ("SetPhysicsGamePieceTransformsFromData()");
		//GOownShipBody.transform.localScale = new Vector3(ownShipScaleX, ownShipScaleY, ownShipScaleZ);  // done within InstantiateGamePieces()
		GOownShipBody.transform.position = ownShipWorldPos;
		GOownShipBody.transform.localRotation = Quaternion.identity;

	}
	#endregion

	#region SetNonPhysicsGamePieceTransformsFromData()
	public override void SetNonPhysicsGamePieceTransformsFromData() {
		Vector3 SelfToEnemyVect = new Vector3(enemyShipWorldPos.x - ownShipWorldPos.x, enemyShipWorldPos.y - ownShipWorldPos.y, enemyShipWorldPos.z - ownShipWorldPos.z);
		// POSITION!
		GOownShipForwardVector.transform.position = ownShipWorldPos + GOownShipBody.transform.forward * (debugVectorLength / 2f);	
		GOownShipUpVector.transform.position = ownShipWorldPos + GOownShipBody.transform.up * (debugVectorLength / 2f);
		GOownShipRightVector.transform.position = ownShipWorldPos + GOownShipBody.transform.right * (debugVectorLength / 2f);

		GOenemyShipBody.transform.position = enemyShipWorldPos;

		// ROTATION!
		GOownShipForwardVector.transform.rotation = Quaternion.LookRotation(GOownShipBody.transform.forward);
		GOownShipUpVector.transform.rotation = Quaternion.LookRotation(GOownShipBody.transform.up); // Figure this out!
		GOownShipRightVector.transform.rotation = Quaternion.LookRotation(GOownShipBody.transform.right);

		//for(int i = 0; i < numThrusterRows * numThrusterColumns; i++) {
			//string name = "GOownThrusterArray" + i.ToString();
			//GOownThrusterArray[i] = new GameObject(name);
			//GOownThrusterArray[i].transform.localScale = new Vector3(ownShipScaleX * ownThrusterWidthMultiplier / numThrusterRows, ownShipScaleY * ownThrusterWidthMultiplier / numThrusterColumns, ownThrusterMaxLength * ownThrusterArrayPower[i][0]);
			//GOownThrusterArray[i].AddComponent<GamePieceSpaceshipThruster>().InitGamePiece();
			//GOownThrusterArray[i].GetComponent<Renderer>().material = ownThrustersMat;
		//}
		float shieldRad = ownShieldRadius * Mathf.Clamp01(transferEnergyToShield[0]);
		GOownShield.transform.localScale = new Vector3(shieldRad, shieldRad, shieldRad);

	}
	#endregion

}
