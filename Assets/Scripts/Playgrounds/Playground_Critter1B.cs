using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Playground_Critter1B : MonoBehaviour {

	public bool reset = true;
	public string sourceFileName;
	public int agentIndex;
	public bool fileIsPopulation = true;
	public Transform targetTransform;
	public float masterScale = 1f;

	private string fileAddressRoot = "E:/Unity Projects/GitHub/ANNTrainer/CreatureTrainer/Assets/TrainingSaves/";
	private string fileAddressFilename;
	private string fileAddressExt = ".txt";
	private bool critterSourceIsValid;
	private bool critterInitialized = false;

	// CRITTER:
	private Agent sourceAgent;
	private int numInputNodes;
	private int numOutputNodes;
	private CreatureBodyGenome critterBodyGenome;
	private int critterNumberOfSegments;
	private GameObject[] GOcritterSegments;
	private Material critterDefaultMat;
	private List<BrainInputChannel> inputChannelsList;
	private List<BrainOutputChannel> outputChannelsList;
	private List<GameOptionChannel> gameOptionsList;
	private float[][] brainInput;  //  Maybe Revisit This
	private float[][] brainOutput;

	private float[][] critterSegmentArray_PosX;
	private float[][] critterSegmentArray_PosY;
	private float[][] critterSegmentArray_PosZ;
	private float[][] critterSegmentArray_AngleX;
	private float[][] critterSegmentArray_MotorTargetX;
	private float[][] critterSegmentArray_AngleY;
	private float[][] critterSegmentArray_MotorTargetY;
	private float[][] critterSegmentArray_AngleZ;
	private float[][] critterSegmentArray_MotorTargetZ;
	private float[][] critterSegmentArray_ScaleX;
	private float[][] critterSegmentArray_ScaleY;
	private float[][] critterSegmentArray_ScaleZ;	
	private float[] critterSegmentArray_Mass;
	private float critterTotalMass = 1f;	
	private Vector3 critterCOM = new Vector3(0f, 0f, 0f);
	private float[] targetPosX = new float[1];
	private float[] targetPosY = new float[1];
	private float[] targetPosZ = new float[1];
	private float[] clockValue = new float[1];
	private float[] clockFrequency = new float[1];
	private float[] targetHeadDot = new float[1];
	private float[] targetHeadDotHor = new float[1];
	private float[] targetHeadDotVer = new float[1];
	// GameOptions:
	private float[] viscosityDrag = new float[1]; //50f;
	public float jointMotorForce = 1000f;
	public float jointMotorSpeed = 500f;
	private float[] variableMass = new float[1]; // 0 means all segments have mass of 1.  // 1 means segment's mass is proportional to its volume
	private float[] sa_x;
	private float[] sa_y;
	private float[] sa_z;

	// GAMELOOP:
	private bool gameInitialized;
	private bool gameTicked;
	private bool gameUpdatedFromPhysX;
	private bool gamePiecesBuilt;

	void Awake() {
		InitializeCritter();
	}

	// Use this for initialization
	void Start () {

		if(critterSourceIsValid) {
			Debug.Log("critterSourceIsValid!");
			InitializeGameLoopModel();
			InstantiateGamePieces();
			Reset();
			SetPhysicsGamePieceTransformsFromData();
			EnablePhysicsGamePieceComponents();
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(critterSourceIsValid) {
			viscosityDrag[0] = Playground_Controller.playgroundControllerStatic.physicsViscosity;
			if(gameTicked) {
				UpdateGameStateFromPhysX();
				GameTimeStepCompleted();
			}
			sourceAgent.brain.BrainMasterFunction(ref brainInput, ref brainOutput);
			// Run the game for one timeStep: (Note that this will only modify non-physX variables -- the actual movement and physX sim happens just afterward -- so keep that in mind)
			Tick();
		}
	}

	#region Initialization Functions:
	private void InitializeCritter() {
		// Determine source filename is valid
		critterSourceIsValid = false;
		
		if(fileIsPopulation) {  // if loading a Population
			fileAddressRoot += "Populations/";
		}
		else {  // else if a single Agent
			fileAddressRoot += "Agents/";
		}
		fileAddressFilename = sourceFileName; // grab filename value from UI field
		if(fileAddressFilename != "") { // if field is filled in
			if(System.IO.File.Exists (fileAddressRoot + fileAddressFilename + fileAddressExt)) {  // check if file exists
				Debug.Log (fileAddressRoot + fileAddressFilename + fileAddressExt + " Exists!!!");
				
				Population populationToLoad = ES2.Load<Population>(fileAddressRoot + fileAddressFilename + fileAddressExt);				
				//Debug.Log ("genomeBiases.Length: " + populationToLoad.masterAgentArray[0].genome.genomeBiases.Length.ToString());
				if(populationToLoad.masterAgentArray[agentIndex] != null) {
					sourceAgent = populationToLoad.masterAgentArray[agentIndex];
					numInputNodes = populationToLoad.numInputNodes;
					numOutputNodes = populationToLoad.numOutputNodes;
					sourceAgent.brain = new BrainANN_FF_Layers_A2A() as BrainANN_FF_Layers_A2A;		
					sourceAgent.brain.InitializeBrainFromGenome(sourceAgent.genome);
					critterSourceIsValid = true;
				}
				else {
					Debug.Log ("Agent " + agentIndex + " object is NULL!");
				}
			}
			else {
				Debug.LogError("No Source File Exists!");
			}
		}
	}

	private void InitializeGameLoopModel() {
		//Debug.Log ("MiniGameCreatureSwimBasic(CreatureBodyGenome templateBodyGenome)");
		
		gamePiecesBuilt = false;  // This refers to if the pieces have all their components and are ready for Simulation, not simply instantiated empty GO's
		gameInitialized = false;  // Reset() is Initialization
		gameTicked = false;  // Has the game been ticked on its current TimeStep
		gameUpdatedFromPhysX = false;  // Did the game just get updated from PhysX Simulation?

		critterBodyGenome = sourceAgent.bodyGenome;
		critterNumberOfSegments = critterBodyGenome.creatureBodySegmentGenomeList.Count;
		critterDefaultMat = new Material (Shader.Find("Diffuse"));
				
		// GAME OPTIONS INIT:
		clockFrequency[0] = 10f;
		viscosityDrag[0] = Playground_Controller.playgroundControllerStatic.physicsViscosity;
		//jointMotorForce[0] = 600f;
		//jointMotorSpeed[0] = 100f;
		variableMass[0] = 0.0f;
		
		InitializeGameDataArrays();		
		SetupInputOutputChannelLists();
		SetInputOutputArrays();
		//SetupGameOptionsList();
	}

	private void InitializeGameDataArrays() {  // set up arrays based on Agent Genome -- uses templateGenome at Construction
		sa_x = new float[critterNumberOfSegments];
		sa_y = new float[critterNumberOfSegments];
		sa_z = new float[critterNumberOfSegments];		
		GOcritterSegments = new GameObject[critterNumberOfSegments];
		critterSegmentArray_PosX = new float[critterNumberOfSegments][];
		critterSegmentArray_PosY = new float[critterNumberOfSegments][];
		critterSegmentArray_PosZ = new float[critterNumberOfSegments][];
		critterSegmentArray_AngleX = new float[critterNumberOfSegments][];
		critterSegmentArray_MotorTargetX = new float[critterNumberOfSegments][];
		critterSegmentArray_AngleY = new float[critterNumberOfSegments][];
		critterSegmentArray_MotorTargetY = new float[critterNumberOfSegments][];
		critterSegmentArray_AngleZ = new float[critterNumberOfSegments][];
		critterSegmentArray_MotorTargetZ = new float[critterNumberOfSegments][];
		critterSegmentArray_ScaleX = new float[critterNumberOfSegments][];
		critterSegmentArray_ScaleY = new float[critterNumberOfSegments][];
		critterSegmentArray_ScaleZ = new float[critterNumberOfSegments][];		
		critterSegmentArray_Mass = new float[critterNumberOfSegments];
		
		// For EACH SEGMENT INIT:
		for(int i = 0; i < critterNumberOfSegments; i++) {
			critterSegmentArray_PosX[i] = new float[1];
			critterSegmentArray_PosY[i] = new float[1];
			critterSegmentArray_PosZ[i] = new float[1];
			critterSegmentArray_AngleX[i] = new float[1];
			critterSegmentArray_MotorTargetX[i] = new float[1];
			critterSegmentArray_AngleY[i] = new float[1];
			critterSegmentArray_MotorTargetY[i] = new float[1];
			critterSegmentArray_AngleZ[i] = new float[1];
			critterSegmentArray_MotorTargetZ[i] = new float[1];
			critterSegmentArray_ScaleX[i] = new float[1];
			critterSegmentArray_ScaleY[i] = new float[1];
			critterSegmentArray_ScaleZ[i] = new float[1];
		}
	}
	#endregion

	#region SETUP METHODS:
	private void SetupInputOutputChannelLists() {
		inputChannelsList = new List<BrainInputChannel>();	
		outputChannelsList = new List<BrainOutputChannel>();
		
		for(int bc = 0; bc < critterNumberOfSegments; bc++) {
			if(bc != 0) {  // if not Root Segment:
				if(critterBodyGenome.creatureBodySegmentGenomeList[bc].jointType == CreatureBodySegmentGenome.JointType.HingeX) {
					string inputChannelName = "Critter Segment " + bc.ToString() + " Angle";
					BrainInputChannel BIC_critterSegmentAngle = new BrainInputChannel(ref critterSegmentArray_AngleX[bc], true, inputChannelName);
					inputChannelsList.Add (BIC_critterSegmentAngle);
					
					string outputChannelName = "Critter Segment " + bc.ToString() + " Motor Target";
					BrainOutputChannel BOC_critterSegmentAngleVel = new BrainOutputChannel(ref critterSegmentArray_MotorTargetX[bc], true, outputChannelName);
					outputChannelsList.Add (BOC_critterSegmentAngleVel);
				}
				else if(critterBodyGenome.creatureBodySegmentGenomeList[bc].jointType == CreatureBodySegmentGenome.JointType.HingeY) {
					string inputChannelName = "Critter Segment " + bc.ToString() + " Angle";
					BrainInputChannel BIC_critterSegmentAngle = new BrainInputChannel(ref critterSegmentArray_AngleY[bc], true, inputChannelName);
					inputChannelsList.Add (BIC_critterSegmentAngle);
					
					string outputChannelName = "Critter Segment " + bc.ToString() + " Motor Target";
					BrainOutputChannel BOC_critterSegmentAngleVel = new BrainOutputChannel(ref critterSegmentArray_MotorTargetY[bc], true, outputChannelName);
					outputChannelsList.Add (BOC_critterSegmentAngleVel);
				}
				else if(critterBodyGenome.creatureBodySegmentGenomeList[bc].jointType == CreatureBodySegmentGenome.JointType.HingeZ) {
					string inputChannelName = "Critter Segment " + bc.ToString() + " Angle";
					BrainInputChannel BIC_critterSegmentAngle = new BrainInputChannel(ref critterSegmentArray_AngleZ[bc], true, inputChannelName);
					inputChannelsList.Add (BIC_critterSegmentAngle);
					
					string outputChannelName = "Critter Segment " + bc.ToString() + " Motor Target";
					BrainOutputChannel BOC_critterSegmentAngleVel = new BrainOutputChannel(ref critterSegmentArray_MotorTargetZ[bc], true, outputChannelName);
					outputChannelsList.Add (BOC_critterSegmentAngleVel);
				}
			}
		}
		BrainInputChannel BIC_targetHeadDot = new BrainInputChannel(ref targetHeadDot, true, "Target Head Dot");
		inputChannelsList.Add (BIC_targetHeadDot);
		BrainInputChannel BIC_targetHeadDotHor = new BrainInputChannel(ref targetHeadDotHor, true, "Target Head Dot Hor");
		inputChannelsList.Add (BIC_targetHeadDotHor);
		BrainInputChannel BIC_targetHeadDotVer = new BrainInputChannel(ref targetHeadDotVer, true, "Target Head Dot Ver");
		inputChannelsList.Add (BIC_targetHeadDotVer);
		BrainInputChannel BIC_clockValue = new BrainInputChannel(ref clockValue, true, "Clock Value");
		inputChannelsList.Add (BIC_clockValue);
	}

	private void SetInputOutputArrays() {
		// Prep the input data for the brain by making the array dimensions match the number of input/output Neurons in the current brain:
		brainInput = new float[numInputNodes][];
		brainOutput = new float[numOutputNodes][];
		// Find length of Channel Lists
		int numInputChannels = inputChannelsList.Count;
		int numOutputChannels = outputChannelsList.Count;
		// Loop through original Channel Lists, and if a channel is selected, pass a ref of its value to the next Index in the brainDataArrays
		int currentInputArrayIndex = 0;
		int currentOutputArrayIndex = 0;
		for(int i = 0; i < numInputChannels; i++) {
			if(inputChannelsList[i].on) {
				brainInput[currentInputArrayIndex] = inputChannelsList[i].channelValue; // send reference of channel value to current brainInputArray Index
				currentInputArrayIndex++; // increment current brainInput Index
			}
		}
		for(int o = 0; o < numOutputChannels; o++) {
			if(outputChannelsList[o].on) {
				brainOutput[currentOutputArrayIndex] = outputChannelsList[o].channelValue; // send reference of channel value to current brainOutputArray Index
				currentOutputArrayIndex++; // increment current brainOutput Index
			}
		}
		// Fill any remaining indices with value of zero ( this will happen if the brain has more nodes than the number of selected Channels )
		while(currentInputArrayIndex < brainInput.Length) {
			float[] zeroArray = new float[1];
			zeroArray[0] = 0f;
			brainInput[currentInputArrayIndex] = zeroArray; // zero out extra indices
			currentInputArrayIndex++; // increment current brainInput Index
		}
		while(currentOutputArrayIndex < brainOutput.Length) {
			float[] zeroArray = new float[1];
			zeroArray[0] = 0f;
			brainOutput[currentOutputArrayIndex] = zeroArray; // zero out extra indices
			currentOutputArrayIndex++; // increment current brainOutput Index
		}
	}
	
	/*private void SetupGameOptionsList() {
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
	}*/
	#endregion

	#region Game Pieces:
	private void InstantiateGamePieces() {		
		for(int i = 0; i < critterNumberOfSegments; i++) {
			string name = "GOcritterSegment" + i.ToString();
			GOcritterSegments[i] = new GameObject(name);
			GOcritterSegments[i].transform.SetParent(this.gameObject.transform);
			GOcritterSegments[i].AddComponent<GamePiecePhysXWormSegment>().InitGamePiece();
			//GOcritterSegments[i].AddComponent<BoxCollider>().enabled = false;
			GOcritterSegments[i].GetComponent<Renderer>().material = critterDefaultMat;
		}
	}

	private void DisablePhysicsGamePieceComponents() { 
		for(int w = 0; w < critterNumberOfSegments; w++) {
			if(GOcritterSegments[w].GetComponent<BoxCollider>() != null) {
				GOcritterSegments[w].GetComponent<BoxCollider>().enabled = false;
			}
			if(GOcritterSegments[w].GetComponent<Rigidbody>() != null) {
				GOcritterSegments[w].GetComponent<Rigidbody>().isKinematic = true;
				if(GOcritterSegments[w].GetComponent<ConfigurableJoint>() != null) {
					GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody = null;
				}
			}	
		}
		gamePiecesBuilt = false;
	}

	private void SetPhysicsGamePieceTransformsFromData() {
		for(int w = 0; w < critterNumberOfSegments; w++) {
			GOcritterSegments[w].transform.localScale = new Vector3(critterSegmentArray_ScaleX[w][0], critterSegmentArray_ScaleY[w][0], critterSegmentArray_ScaleZ[w][0]);
			GOcritterSegments[w].transform.position = new Vector3(critterSegmentArray_PosX[w][0], critterSegmentArray_PosY[w][0], critterSegmentArray_PosZ[w][0]);
			GOcritterSegments[w].transform.localRotation = Quaternion.identity;
		}
	}

	private void EnablePhysicsGamePieceComponents() {
		for(int w = 0; w < critterNumberOfSegments; w++) {
			GOcritterSegments[w].GetComponent<Rigidbody>().useGravity = true;
			GOcritterSegments[w].GetComponent<Rigidbody>().isKinematic = false;
			GOcritterSegments[w].GetComponent<Rigidbody>().mass = critterSegmentArray_Mass[w];

			if(w < (critterNumberOfSegments - 1)) {  // if not the final 'Tail' segment:
				
			}
			if(w > 0) {  // if not the first root segment:
				ConfigurableJoint configJoint;
				if(GOcritterSegments[w].GetComponent<ConfigurableJoint>() == null) {
					configJoint = GOcritterSegments[w].AddComponent<ConfigurableJoint>();
				}
				else {
					configJoint = GOcritterSegments[w].GetComponent<ConfigurableJoint>();
				}
				configJoint.autoConfigureConnectedAnchor = false;
				configJoint.connectedBody = GOcritterSegments[critterBodyGenome.creatureBodySegmentGenomeList[w].parentID].GetComponent<Rigidbody>();  // Set parent
				configJoint.anchor = GetJointAnchor(critterBodyGenome.creatureBodySegmentGenomeList[w]);
				configJoint.connectedAnchor = GetJointConnectedAnchor(critterBodyGenome.creatureBodySegmentGenomeList[w]); // <-- Might be Unnecessary
				ConfigureJointSettings(critterBodyGenome.creatureBodySegmentGenomeList[w], ref configJoint);
			}

		}		
		gamePiecesBuilt = true;
	}

	private void UpdateGameStateFromPhysX() {
		// PhysX simulation happened after miniGame Tick(), so before passing gameStateData to brain, need to update gameStateData from the rigidBodies
		// SO that the correct updated input values can be sent to the brain
		for(int w = 0; w < critterNumberOfSegments; w++) {
			if(GOcritterSegments[w].GetComponent<Rigidbody>() != null) {
				critterSegmentArray_PosX[w][0] = GOcritterSegments[w].GetComponent<Rigidbody>().position.x;
				critterSegmentArray_PosY[w][0] = GOcritterSegments[w].GetComponent<Rigidbody>().position.y;
				critterSegmentArray_PosZ[w][0] = GOcritterSegments[w].GetComponent<Rigidbody>().position.z;
				//Debug.Log ("UpdateGameStateFromPhysX() rigidBodPos: " + new Vector3(wormSegmentArray_PosX[w][0], wormSegmentArray_PosY[w][0], wormSegmentArray_PosZ[w][0]));
				
				if(w > 0) {  // if not the ROOT segment:
					//wormSegmentArray_AngleX[w][0] = Quaternion.FromToRotation(GOwormSegments[w].GetComponent<ConfigurableJoint>().axis, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.transform.rotation.eulerAngles).eulerAngles.x;
					//wormSegmentArray_AngleY[w][0] = Quaternion.FromToRotation(GOwormSegments[w].GetComponent<ConfigurableJoint>().axis, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.transform.rotation.eulerAngles).eulerAngles.y;
					//wormSegmentArray_AngleZ[w][0] = Quaternion.FromToRotation(GOwormSegments[w].GetComponent<ConfigurableJoint>().axis, GOwormSegments[w].GetComponent<ConfigurableJoint>().connectedBody.transform.rotation.eulerAngles).eulerAngles.z;
					MeasureJointAngles(w);
					//Debug.Log ("Segment " + w.ToString() + ", AngleX= " + wormSegmentArray_AngleX[w][0].ToString() + ", AngleY= " + wormSegmentArray_AngleY[w][0].ToString() + ", AngleZ= " + wormSegmentArray_AngleZ[w][0].ToString());
				}
			}
			if(GOcritterSegments[w].GetComponent<BoxCollider>() != null) {
				//GOcritterSegments[w].GetComponent<BoxCollider>().enabled = true;
			}
		}
		gameUpdatedFromPhysX = true;
		SetNonPhysicsGamePieceTransformsFromData();  // debug objects that rely on PhysX object positions
	}

	private void SetNonPhysicsGamePieceTransformsFromData() {
		/*
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
		*/
	}

	private void MeasureJointAngles(int w) {
		if(critterBodyGenome.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xPos) {
			critterSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.forward, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.right, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.right, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
		}
		else if(critterBodyGenome.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.xNeg) {
			critterSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.up, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.forward, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.up, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
		}
		else if(critterBodyGenome.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yPos) {
			critterSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.forward, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.right, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.right, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
		}
		else if(critterBodyGenome.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.yNeg) {
			critterSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.up, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.forward, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.up, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
		}
		else if(critterBodyGenome.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zPos) {
			critterSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.forward, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.right, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.right, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.up)) * Mathf.Rad2Deg;
		}
		else if(critterBodyGenome.creatureBodySegmentGenomeList[w].parentAttachSide == CreatureBodySegmentGenome.ParentAttachSide.zNeg) {
			critterSegmentArray_AngleX[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.up, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.forward)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleY[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.forward, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
			critterSegmentArray_AngleZ[w][0] = Mathf.Asin(Vector3.Dot(GOcritterSegments[w].transform.up, GOcritterSegments[w].GetComponent<ConfigurableJoint>().connectedBody.GetComponent<Rigidbody>().transform.right)) * Mathf.Rad2Deg;
		}
	}
	#endregion

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
				sa_z[segmentIndex] * viscosityDrag[0] * critterSegmentArray_Mass[segmentIndex];  // multiplied by face's surface area, and user-defined multiplier
		rigidBod.AddForceAtPosition (fluidDragVecPosZ*2f, zpos_face_center);  // Apply force at face's center, in the direction opposite the face normal
		
		// TOP (posY):
		Vector3 pointVelPosY = rigidBod.GetPointVelocity (ypos_face_center);
		Vector3 fluidDragVecPosY = -up * Vector3.Dot (up, pointVelPosY) * sa_y[segmentIndex] * viscosityDrag[0] * critterSegmentArray_Mass[segmentIndex];  
		rigidBod.AddForceAtPosition (fluidDragVecPosY*2f, ypos_face_center);		
		
		// RIGHT (posX):
		Vector3 pointVelPosX = rigidBod.GetPointVelocity (xpos_face_center);
		Vector3 fluidDragVecPosX = -right * Vector3.Dot (right, pointVelPosX) * sa_x[segmentIndex] * viscosityDrag[0] * critterSegmentArray_Mass[segmentIndex];  
		rigidBod.AddForceAtPosition (fluidDragVecPosX, xpos_face_center);
	}

	private void Reset() {	

		// Right place for these??		
		clockValue[0] = 0f;
		Vector3 avgPos = new Vector3(0f, 0f, 0f);

		critterTotalMass = 0f;
		
		// Create Critter:
		for(int i = 0; i < critterNumberOfSegments; i++) {  // iterate through every node			
			// SHARED:
			critterSegmentArray_ScaleX[i][0] = critterBodyGenome.creatureBodySegmentGenomeList[i].size.x * masterScale;
			critterSegmentArray_ScaleY[i][0] = critterBodyGenome.creatureBodySegmentGenomeList[i].size.y * masterScale;
			critterSegmentArray_ScaleZ[i][0] = critterBodyGenome.creatureBodySegmentGenomeList[i].size.z * masterScale;
			
			critterSegmentArray_Mass[i] = Mathf.Lerp (1f, critterSegmentArray_ScaleX[i][0] * critterSegmentArray_ScaleY[i][0] * critterSegmentArray_ScaleZ[i][0], variableMass[0]);
			critterTotalMass += critterSegmentArray_Mass[i];
			
			if(i == 0) {  // If Root Node:
				critterSegmentArray_PosX[0][0] = 0f;
				critterSegmentArray_PosY[0][0] = 0f;
				critterSegmentArray_PosZ[0][0] = 0f;
				sa_x[i] = critterSegmentArray_ScaleY[i][0] * critterSegmentArray_ScaleZ[i][0];  // Y * Z
				sa_y[i] = critterSegmentArray_ScaleX[i][0] * critterSegmentArray_ScaleZ[i][0];  // X * Z
				sa_z[i] = critterSegmentArray_ScaleX[i][0] * critterSegmentArray_ScaleY[i][0]; // X * Y
			}
			else {  // NOT the ROOT segment:
				// FIGURE OUT BIND POSITION:
				int parentID = critterBodyGenome.creatureBodySegmentGenomeList[i].parentID;
				Vector3 parentPos = new Vector3(critterSegmentArray_PosX[parentID][0],
				                                critterSegmentArray_PosY[parentID][0],
				                                critterSegmentArray_PosZ[parentID][0]);
				Vector3 parentSize = new Vector3(critterSegmentArray_ScaleX[parentID][0],
				                                 critterSegmentArray_ScaleY[parentID][0],
				                                 critterSegmentArray_ScaleZ[parentID][0]);
				Vector3 centerPos = GetBlockPosition(parentPos, 
				                                     parentSize, 
				                                     critterBodyGenome.creatureBodySegmentGenomeList[i].attachPointParent, 
				                                     critterBodyGenome.creatureBodySegmentGenomeList[i].attachPointChild, 
				                                     critterBodyGenome.creatureBodySegmentGenomeList[i].size, 
				                                     critterBodyGenome.creatureBodySegmentGenomeList[i].parentAttachSide);
				Vector3 ownSize = critterBodyGenome.creatureBodySegmentGenomeList[i].size * masterScale;
				// Subtract SurfaceArea for shared sides:
				sa_x[i] = critterSegmentArray_ScaleY[i][0] * critterSegmentArray_ScaleZ[i][0];  // Y * Z
				sa_y[i] = critterSegmentArray_ScaleX[i][0] * critterSegmentArray_ScaleZ[i][0];  // X * Z
				sa_z[i] = critterSegmentArray_ScaleX[i][0] * critterSegmentArray_ScaleY[i][0]; // X * Y
				CreatureBodySegmentGenome.ParentAttachSide attachSide = critterBodyGenome.creatureBodySegmentGenomeList[i].parentAttachSide; 
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
				
				critterSegmentArray_PosX[i][0] = centerPos.x;
				critterSegmentArray_PosY[i][0] = centerPos.y;
				critterSegmentArray_PosZ[i][0] = centerPos.z;
				// Initialize Joint Settings:
				critterSegmentArray_AngleX[i][0] = 0f;
				critterSegmentArray_MotorTargetX[i][0] = 0f;
				critterSegmentArray_AngleY[i][0] = 0f;
				critterSegmentArray_MotorTargetY[i][0] = 0f;
				critterSegmentArray_AngleZ[i][0] = 0f;
				critterSegmentArray_MotorTargetZ[i][0] = 0f;
			}
		}
		
		float avgMass = critterTotalMass / critterNumberOfSegments;  //
		critterCOM.x = 0f;
		critterCOM.y = 0f;
		critterCOM.z = 0f;
		
		critterTotalMass /= avgMass;  // Adjust to center around 1f;
		for(int j = 0; j < critterNumberOfSegments; j++) { 
			// Finalize Segment Mass Value:
			critterSegmentArray_Mass[j] /= avgMass;  // Center the segment Masses around 1f, to avoid precision errors
			
			float shareOfTotalMass = critterSegmentArray_Mass[j] / critterTotalMass;
			critterCOM.x += critterSegmentArray_PosX[j][0] * shareOfTotalMass;  // multiply position by proportional share of total mass
			critterCOM.y += critterSegmentArray_PosY[j][0] * shareOfTotalMass;
			critterCOM.z += critterSegmentArray_PosZ[j][0] * shareOfTotalMass;
		}
		
		for(int k = 0; k < critterNumberOfSegments; k++) { 
			// Center the creature so that its center of mass is at the origin, to avoid initial position bias
			critterSegmentArray_PosX[k][0] -= critterCOM.x;
			critterSegmentArray_PosY[k][0] -= critterCOM.y;
			critterSegmentArray_PosZ[k][0] -= critterCOM.z;
		}
		
		// Reset wormCOM to 0f, now that it's been moved:
		critterCOM.x = 0f;
		critterCOM.y = 0f;
		critterCOM.z = 0f;
		
		gameInitialized = true;
		gameTicked = false;
		gameUpdatedFromPhysX = false;
		reset = true;
	}

	private void Tick() {  // Runs the mini-game for a single evaluation step.
		//Debug.Log ("Tick()");
		// THIS IS ALL PRE- PHYS-X!!! :		
		clockValue[0] = Mathf.Sin (Time.fixedTime * clockFrequency[0]);
		targetPosX[0] = targetTransform.localPosition.x;
		targetPosY[0] = targetTransform.localPosition.y;
		targetPosZ[0] = targetTransform.localPosition.z;
		
		for(int w = 0; w < critterNumberOfSegments; w++) {
			if(GOcritterSegments[w].GetComponent<ConfigurableJoint>() != null) {
				if(critterBodyGenome.creatureBodySegmentGenomeList[w].jointType == CreatureBodySegmentGenome.JointType.HingeX) {
					JointDrive drive = GOcritterSegments[w].GetComponent<ConfigurableJoint>().angularXDrive;
					drive.maximumForce = jointMotorForce;
					GOcritterSegments[w].GetComponent<ConfigurableJoint>().angularXDrive = drive;
					Vector3 targetVel = GOcritterSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity;
					targetVel.x = critterSegmentArray_MotorTargetX[w][0] * jointMotorSpeed;
					GOcritterSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;
					//GOwormSegments[w].GetComponent<ConfigurableJoint>().angularXDrive.maximumForce = jointMotorForce[0];
					//GOwormSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity.x = wormSegmentArray_MotorTargetX[w][0] * jointMotorSpeed[0];
				}
				else if(critterBodyGenome.creatureBodySegmentGenomeList[w].jointType == CreatureBodySegmentGenome.JointType.HingeY) {
					JointDrive drive = GOcritterSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive;
					drive.maximumForce = jointMotorForce;
					GOcritterSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive = drive;
					Vector3 targetVel = GOcritterSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity;
					targetVel.y = critterSegmentArray_MotorTargetY[w][0] * jointMotorSpeed;
					GOcritterSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;
				}
				else if(critterBodyGenome.creatureBodySegmentGenomeList[w].jointType == CreatureBodySegmentGenome.JointType.HingeZ) {
					JointDrive drive = GOcritterSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive;
					drive.maximumForce = jointMotorForce;
					GOcritterSegments[w].GetComponent<ConfigurableJoint>().angularYZDrive = drive;
					Vector3 targetVel = GOcritterSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity;
					targetVel.z = critterSegmentArray_MotorTargetZ[w][0] * jointMotorSpeed;
					GOcritterSegments[w].GetComponent<ConfigurableJoint>().targetAngularVelocity = targetVel;
				}
			}
			ApplyViscosityForces(GOcritterSegments[w], w, viscosityDrag[0]);
		}
		
		// Update Game Data:
		//float avgMass = critterTotalMass / critterNumberOfSegments;
		critterCOM.x = 0f;
		critterCOM.y = 0f;
		critterCOM.z = 0f;

		for(int e = 0; e < critterNumberOfSegments; e++) {
			float shareOfTotalMass = critterSegmentArray_Mass[e] / critterTotalMass;
			critterCOM.x += critterSegmentArray_PosX[e][0] * shareOfTotalMass;  // multiply position by proportional share of total mass
			critterCOM.y += critterSegmentArray_PosY[e][0] * shareOfTotalMass;
			critterCOM.z += critterSegmentArray_PosZ[e][0] * shareOfTotalMass;
		}
		//Vector3 targetDirection = new Vector3(targetPosX[0] - critterCOM.x, targetPosY[0] - critterCOM.y, targetPosZ[0] - critterCOM.z);
		//float distToTarget = targetDirection.magnitude;		
		Vector3 headToTargetVect = new Vector3(targetPosX[0] - critterSegmentArray_PosX[0][0], targetPosY[0] - critterSegmentArray_PosY[0][0], targetPosZ[0] - critterSegmentArray_PosZ[0][0]).normalized;
		targetHeadDot[0] = Vector3.Dot (headToTargetVect, GOcritterSegments[0].transform.forward);
		targetHeadDotHor[0] = Vector3.Dot (headToTargetVect, GOcritterSegments[0].transform.right);
		targetHeadDotVer[0] = Vector3.Dot (headToTargetVect, GOcritterSegments[0].transform.up);		

		gameTicked = true;
	}

	private void GameTimeStepCompleted() {
		gameTicked = false;
		gameUpdatedFromPhysX = false;
	}

	#region RebuildCreature()	
	private void ConfigureJointSettings(CreatureBodySegmentGenome node, ref ConfigurableJoint joint) {
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
