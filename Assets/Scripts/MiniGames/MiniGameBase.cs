using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameBase {
	public static bool debugFunctionCalls = true; // turns debug messages on/off

	public List<BrainInputChannel> inputChannelsList;
	public List<BrainOutputChannel> outputChannelsList;

	public List<FitnessComponent> fitnessComponentList;

	public List<GameOptionChannel> gameOptionsList;

	public CritterGenome agentBodyBeingTested; // !!!!! RE-EVALUATE if instead, Trainer should pass agent into Reset() and Tick() functions, rather than storing a ref

	public bool gameEndStateReached = false;
	public bool piecesBuilt = false;
	public bool gameInitialized = false;
	public bool gameTicked = false;
	public bool gameUpdatedFromPhysX = false;
    public bool gameCleared = false;  // delete critterSegments -- wait for reset
    public bool waitingForReset = false;
	public int gameCurrentTimeStep = 0;



	//public float fitnessScore = 0f;

	// Constructor!!
	public MiniGameBase() {
		// Brain Inputs!:
		inputChannelsList = new List<BrainInputChannel>();
		// Brain Outputs!:		
		outputChannelsList = new List<BrainOutputChannel>();

		fitnessComponentList = new List<FitnessComponent>();

		gameOptionsList = new List<GameOptionChannel>();
	}

	public virtual void Tick() {  // Runs the mini-game for a single evaluation step.
		DebugBot.DebugFunctionCall("MiniGameBase; Tick();", debugFunctionCalls);
	}

	public virtual void Reset() {

	}

	public virtual void UpdateGameStateFromPhysX() {
		gameUpdatedFromPhysX = true;
		SetNonPhysicsGamePieceTransformsFromData();  // debug objects that rely on PhysX object positions
	}

	public virtual void InstantiateGamePieces() {
		
	}

	public virtual void UninstantiateGamePieces() {
		
	}

	public virtual void EnablePhysicsGamePieceComponents() {
		piecesBuilt = true;
	}

	public virtual void DisablePhysicsGamePieceComponents() {
		piecesBuilt = false;
	}

	public virtual void SetPhysicsGamePieceTransformsFromData() {

	}

	public virtual void SetNonPhysicsGamePieceTransformsFromData() {
		
	}

	public virtual void GameTimeStepCompleted() {
		gameTicked = false;
		gameUpdatedFromPhysX = false;
		gameCurrentTimeStep++;  // reset to 0
	}

    public virtual void ClearGame() {
        gameCleared = true;        
    }
}
