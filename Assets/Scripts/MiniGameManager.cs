using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameManager {

	//public List<MiniGameInputChannel> selectedInputChannels = new List<MiniGameInputChannel>();

	public bool debugFunctionCalls = false; // turns debug messages on/off
	public delegate void DelegateMiniGameTick();
	DelegateMiniGameTick delegateMiniGameTick;

	public enum MiniGameType {
		None,
		//BallCatch,
		//CreatureSwimBasic,
		CritterWalkBasic
		//MoveToTarget3D,
		//ObstacleNavigation,
		//PhysXWorm,
		//PoleBalancing2D,
		//RobotArmSimple,
		//SpaceshipShooter
	};

	public Player playerRef;
	// determines class type of miniGameInstance
	public MiniGameType gameType = MiniGameType.None;
	// The actual miniGame instance object:
	public MiniGameBase miniGameInstance;

	// CHANGE THESE TO BETTER/GENERIC DATA TYPES LATER!!!!!!!!!!!!!!!!!
	// The list of potential Channels is stored inside the miniGameInstance Object,
	// The arrays of paramater references for the Brain (only consists of the selected channels) lives here in the MiniGameManager
	public List<float[]> brainInput;  //  Come back to this and see if I should use a BrainInputChannel class instead of float;
	public List<float[]> brainOutput;  // !! Or if this should live inside the miniGameInstance


	public MiniGameManager(Player playerReference) {
		DebugBot.DebugFunctionCall("MiniGameManager; MiniGameManager(); Constructor", debugFunctionCalls);
		playerRef = playerReference;
		//SetMiniGameType(gameType);
		//inputSourceList = new string[0];
		//outputActionsList = new string[0];
	}

	public void SetInputOutputArrays() {
        //DebugBot.DebugFunctionCall("MiniGameManager; SetInputOutputArrays(); " + playerRef.currentTrialForEdit.ToString(), debugFunctionCalls);
        // ++++++ NEW APPROACH !! ++++++++++++++++++++++++++++++++++++++++++++++++
        // re-initialize data arrays in case population has changed settings
        //brainInput = null;
        //brainOutput = null;
        // Prep the input data for the brain by making the array dimensions match the number of input/output Neurons in the current brain:
        brainInput = new List<float[]>(); // playerRef.masterPopulation.numInputNodes][];
        brainOutput = new List<float[]>(); // playerRef.masterPopulation.numOutputNodes][];
		// Find length of Channel Lists
		int numInputChannels = miniGameInstance.inputChannelsList.Count; // playerRef.masterPopulation.numOutputNodes;
        int numOutputChannels = miniGameInstance.outputChannelsList.Count;
        //DebugBot.DebugFunctionCall("MiniGameManager; SetInputOutputArrays(); inputCount: " + miniGameInstance.inputChannelsList.Count.ToString() + ", outputCount: " + miniGameInstance.outputChannelsList.Count.ToString(), debugFunctionCalls);
        // Loop through original Channel Lists, and if a channel is selected, pass a ref of its value to the next Index in the brainDataArrays
        int currentInputArrayIndex = 0;
		int currentOutputArrayIndex = 0;
		for(int i = 0; i < numInputChannels; i++) {
            //Debug.Log("SetInputOutputArrays... i: " + i.ToString() + ", numInputChannels: " + numInputChannels.ToString() + ", inputChannelsListCount: " + miniGameInstance.inputChannelsList.Count.ToString() + ", " + miniGameInstance.inputChannelsList[i].ToString() + ", " + miniGameInstance.inputChannelsList[i].channelName.ToString());
			if(miniGameInstance.inputChannelsList[i].on) {
				//brainInput[currentInputArrayIndex] = miniGameInstance.inputChannelsList[i].channelValue; // send reference of channel value to current brainInputArray Index
                brainInput.Add(miniGameInstance.inputChannelsList[i].channelValue);
                currentInputArrayIndex++; // increment current brainInput Index
			}
		}
		for(int o = 0; o < numOutputChannels; o++) {
			if(miniGameInstance.outputChannelsList[o].on) {
                //brainOutput[currentOutputArrayIndex] = miniGameInstance.outputChannelsList[o].channelValue; // send reference of channel value to current brainOutputArray Index
                brainOutput.Add(miniGameInstance.outputChannelsList[o].channelValue);
				currentOutputArrayIndex++; // increment current brainOutput Index
			}
		}
	}

	public void SetMiniGameType(MiniGameManager.MiniGameType newGameType, MiniGameSettingsBase gameSettings) {  // Change game type and re-instantiate miniGameInstance
		DebugBot.DebugFunctionCall("Trial; SetMiniGameType(); " + gameType.ToString(), debugFunctionCalls);
		miniGameInstance = null;
		gameType = newGameType;  // Update GameType to new Type!
		
		// CREATE mini-game instance!  !!!!!!!!!!!!! Come back to this for Improvement Later!!!!!!!
		if(newGameType == MiniGameType.None) {
			miniGameInstance = new MiniGameBase();
			//miniGameInstance = newGameInstance as MiniGame;
		}
		/*else if(newGameType == MiniGameType.MoveToTarget3D) {
			MiniGameMoveToTarget3D newGameInstance = new MiniGameMoveToTarget3D();
			miniGameInstance = newGameInstance;
		}
		else if(newGameType == MiniGameType.CreatureSwimBasic) {
            // SHOULD the input/output Lists depend on the Agent's full Genome, or should the minigame have control over hookups?
            // ... Maybe eventually the Agent's genome should dictate, but for now, to avoid a complete re-write, will leave the ability
            // ... of miniGame's to choose which input/output neurons to hook up. Save that change for the big NEAT update where creature
            // ... topologies can morph and differ within a population.
            Debug.Log("BROKEN!! MiniGameManager public void SetMiniGameType(MiniGameManager.MiniGameType newGameType)");
			CritterGenome templateBody = playerRef.masterPopulation.templateGenome;
			//MiniGameCreatureSwimBasic newGameInstance = new MiniGameCreatureSwimBasic(templateBody);
			//miniGameInstance = newGameInstance;
		}*/
		else if(newGameType == MiniGameType.CritterWalkBasic) {
            CritterGenome templateBody = playerRef.masterPopulation.templateGenome;
            //Debug.Log("playerRef.masterPopulation.templateBodyGenome: " + playerRef.masterPopulation.templateBodyGenome.creatureBodySegmentGenomeList[0].addOn1.ToString());
            //Debug.Log("templateBody: " + templateBody.creatureBodySegmentGenomeList[0].addOn1.ToString());
			MiniGameCritterWalkBasic newGameInstance = new MiniGameCritterWalkBasic(templateBody);
            newGameInstance.UseSettings((MiniGameCritterWalkBasicSettings)gameSettings);
            newGameInstance.InitializeGame();
			miniGameInstance = newGameInstance;
            Debug.Log("miniGameInstance.inputChannelsList.Count: " + miniGameInstance.inputChannelsList.Count.ToString());
		}
		/*else if(newGameType == MiniGameType.RobotArmSimple) {
			MiniGameRobotArm newGameInstance = new MiniGameRobotArm();
			miniGameInstance = newGameInstance;
		}
		else if(newGameType == MiniGameType.PhysXWorm) {
			MiniGamePhysXWorm newGameInstance = new MiniGamePhysXWorm();
			miniGameInstance = newGameInstance;
		}
		else if(newGameType == MiniGameType.BallCatch) {
			MiniGameBallCatch newGameInstance = new MiniGameBallCatch();
			miniGameInstance = newGameInstance;
		}
		else if(newGameType == MiniGameType.ObstacleNavigation) {
			MiniGameObstacleNavigation newGameInstance = new MiniGameObstacleNavigation();
			miniGameInstance = newGameInstance;
		}
		else if(newGameType == MiniGameType.PoleBalancing2D) {
			MiniGamePoleBalancing2D newGameInstance = new MiniGamePoleBalancing2D();
			miniGameInstance = newGameInstance;
		}
		else if(newGameType == MiniGameType.SpaceshipShooter) {
			MiniGameSpaceshipShooter newGameInstance = new MiniGameSpaceshipShooter();
			miniGameInstance = newGameInstance;
		}*/

		SetInputOutputArrays(); // Haven't set input/output lists for the minigame yet....
	}
}
