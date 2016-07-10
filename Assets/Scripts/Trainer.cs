using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trainer {

	#region Variables
	public bool debugFunctionCalls = false; // turns debug messages on/off
	public GameController gameControllerRef;

	// STATES:
	private bool isPlaying = false;  // When true, the mini-games are being played by the agents, when false, game is paused
	public bool IsPlaying {
		get {
			return isPlaying;
		}
		set {}
	}
	private bool crossoverOn = true; // Note that this refers to the "Training" button on the menuBar, names may have to change in the future.
	public bool CrossoverOn {
		get {
			return crossoverOn;
		}
		set {}
	}
	private bool fastModeOn = false; // simulate and watch in real-time, or crunch the numbers as fast as possible behind-the-scenes.
	public bool FastModeOn {
		get {
			return fastModeOn;
		}
		set {}
	}
	private bool manualOverrideOn = false;  // With this true, the user can play against the AI controllers
	public bool ManualOverrideOn {
		get {
			return manualOverrideOn;
		}
		set {}
	}

	// When true (needs to be paused, between gens), any pending values in the specified panel will be used to update their corresponding data:
	public bool applyPanelPlayers = false;   // basically, true means that the current state of the system allows changes to each of these panel's data
	public bool applyPanelPopulation = false;
	public bool applyPanelCrossover = true; 
	public bool applyPanelTrials = true; 
	public bool applyPanelFitness = false; 
	public bool applyPanelMiniGame = false; 

	private int maxPlayers = 4;
	public int MaxPlayers {
		get {
			return maxPlayers;
		}
		set {}
	}
	private int numPlayers = 1;
	public int NumPlayers {
		get {
			return numPlayers;
		}
		set {
			numPlayers = value;
		}
	}
	private int curPlayer = 1;
	public int CurPlayer {
		get {
			return curPlayer;
		}
		set {
			curPlayer = value;
		}
	}

	// PLAYER LIST:
	private List<Player> playerList;  // This holds the instanced Player classes, depending on numPlayers.  This holds all the information for training and populations!
	public List<Player> PlayerList {
		get {
			return playerList;
		}
		set {
			playerList = value;
		}
	}

	public float playbackSpeed = 1.0f;

	public bool hasValidPlayerList = false;

	// Playing Game States! 
	private int playingCurGeneration = 0;
    public int PlayingCurGeneration {
        get {
            return playingCurGeneration;
        }
        set { }
    }
    private int playingCurTrialIndex;
	private int playingNumTrials = 0; // keep track of highest index Trial for any player
	private int playingCurPlayer;
	private int playingNumPlayers = 0;
	private int playingCurTrialRound;
	private int playingNumTrialRounds = 0;
	private int playingCurAgent;
	private int playingNumAgents = 0;
	private int playingCurMiniGameTimeStep;
	private int playingNumMiniGameTimeSteps = 0;

	public bool betweenGenerations = true;

    public TrainingSave loadedTrainingSave;
    public TrainingModifierManager trainingModifierManager;

    // MOVE THIS LATER!!!
    public GameObject brainNetworkGO;
    public BrainNetworkVisualizer networkVisualizer;
    Material brainNetworkMat = new Material(Shader.Find("Custom/SimpleBrainNetworkShader"));
    #endregion
    
    // Constructor Method
    public Trainer() {
		DebugBot.DebugFunctionCall("Trainer; Trainer() Constructor!; ", debugFunctionCalls);
		//CheckPanelApplyCriteria();
		InitializePlayerList();
        if(trainingModifierManager == null) {
            trainingModifierManager = new TrainingModifierManager();
        }
	}

	public void InitializePlayerList() {
		playerList = new List<Player>();
		for(int i = 0; i < numPlayers; i++) {
			Player newPlayer = new Player();
			playerList.Add (newPlayer);
		}
		hasValidPlayerList = true;
		DebugBot.DebugFunctionCall("Trainer; InitializePlayerList; " + playerList.ToString(), debugFunctionCalls);
	}

	public void AddPlayer() {
		Player newPlayer = new Player();
		newPlayer.InitializeNewPopulation();
		playerList.Add (newPlayer);
		UpdatePlayingNumPlayers();
	}

	public void TogglePlayPause() { // !!!!!!!! NEEDS RE-FACTORING!!!
		Debug.Log ("Toggle Play/Pause!");
        //Debug.Log("PlayRealTimeStep()Trainer: " + playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager.miniGameInstance.agentBodyBeingTested.creatureBodySegmentGenomeList[0].addOn1.ToString());
        isPlaying = !isPlaying;
		if(isPlaying) {
			Time.timeScale = playbackSpeed;
		}
		else {
			Time.timeScale = 0.0f;
		}
		if(betweenGenerations == true && isPlaying == true) { // !!!! Re-Evaluate this way of zero-ing!
			playingCurTrialIndex = 0;
			playingCurPlayer = 0;
			playingCurTrialRound = 0;
			playingCurAgent = 0;
			playingCurMiniGameTimeStep = 0;
			UpdatePlayingNumTimeSteps();
			UpdatePlayingNumTrialRounds();
			UpdatePlayingNumAgents();
			UpdatePlayingNumTrials(); 
			UpdatePlayingNumPlayers();
			
            // For the very first time, generation zero:
			for(int p = 0; p < playerList.Count; p++) {
				playerList[p].dataManager.InitializeNewGenerationDataArrays(playingCurGeneration);
			}
			playerList[playingCurPlayer].graphKing.BuildTexturesFitnessPerGen(playerList[playingCurPlayer]);
			playerList[playingCurPlayer].graphKing.BuildTexturesCurAgentPerAgent(playerList[playingCurPlayer], playingCurAgent);
			gameControllerRef.trainerUI.SetAllPanelsFromTrainerData();
		}
	}

	public void ToggleTraining() {
		crossoverOn = !crossoverOn;
	}

    #region OLD CODE:
    public void ToggleFastMode() {
		fastModeOn = !fastModeOn;
	}
	public void PlayRealTimeStep() {
		//DebugBot.DebugFunctionCall("Trainer; PlayRealTimeStep; " + playingCurGeneration + ", " + playingCurTrialIndex + "/" + playingNumTrials + ", " + playingCurPlayer + "/" + playingNumPlayers + ", " + playingCurTrialRound + "/" + playingNumTrialRounds + ", " + playingCurAgent + "/" + playingNumAgents + ", " + playingCurMiniGameTimeStep + "/" + playingNumMiniGameTimeSteps, debugFunctionCalls);
		Debug.Log ("PlayRealTimeStep(): Before CalculateOneStep() & UpdatePlayingState()! game TimeStep: " + playingCurMiniGameTimeStep.ToString());
        // !!!!!!!!!!!!!!  NEED TO UPDATE THIS !!!! so that it properly detects when a mini-game instance has changed and deletes unneeded pieces
        //if(!playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager.miniGameInstance.piecesBuilt) { // if current miniGame instance doesn't have pieces built
        //	DebugBot.DebugFunctionCall("PIECES BUILT!! ", debugFunctionCalls);
        //	playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager.miniGameInstance.BuildGamePieces();
        //}
        //playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager.miniGameInstance.PrintTestTargetBallPos();
        // Do whatever it is that it does for one frame -- Main play loop -- Tick Brain, Tick MiniGame:
        //Debug.Log("PlayRealTimeStep()Trainer: " + playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager.miniGameInstance.agentBodyBeingTested.creatureBodySegmentGenomeList[0].addOn1.ToString());
        CalculateOneStep();
		// Increment Time Step!
		playingCurMiniGameTimeStep++;
		// Update playing State
		UpdatePlayingState();
	}
	public void PlayFastModeChunk(int numSteps) {
		int counter = 0;
		while(counter < numSteps) {
			CalculateOneStep();
			counter++;
			playingCurMiniGameTimeStep++;
			UpdatePlayingState();
		}
	}
    #endregion

    #region Fitness Stuff
    private void ProcessFitnessScoresEndRound() {
		// Agent X just finished a round of a game -- Figure out its score.
		// Find playingCurMiniGameTimeStep (i.e. how many timesteps the game was played
		int numTimeSteps = playingCurMiniGameTimeStep;
		// Look through Current player's Trial's fitness manager's Fitness Components:
		FitnessManager fitnessManagerRef = playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].fitnessManager;
		int numGameFitComponents = fitnessManagerRef.gameFitnessComponentList.Count;
		int numBrainFitComponents = fitnessManagerRef.brainFitnessComponentList.Count;
		int numSelectedComponents = 0;

        // Fitness Components from MINIGAME (TRIAL):
		for(int i = 0; i < numGameFitComponents; i++) {
			if(fitnessManagerRef.gameFitnessComponentList[i].on) {
				float score = 0f;
				score = fitnessManagerRef.gameFitnessComponentList[i].componentScore[0]; // get raw value from miniGame
				// If the score is an Average, divide by number of timeSteps -- only needed if miniGame numTimeSteps varies?? keeping it in just in case...
				if(fitnessManagerRef.gameFitnessComponentList[i].divideByTimeSteps) {
					score /= (float)numTimeSteps;
				}
				
                // Add this raw score from this round to the Agent's fitness raw values:
				playerList[playingCurPlayer].dataManager.StoreGameRoundRawData(score, playingCurGeneration, playingCurAgent, playingCurTrialIndex, numSelectedComponents);
				numSelectedComponents++;
			}		
		}
        // Fitness Components from BRAIN:
		for(int i = 0; i < numBrainFitComponents; i++) {
			if(fitnessManagerRef.brainFitnessComponentList[i].on) {
				float score = 0f;
				score = fitnessManagerRef.brainFitnessComponentList[i].componentScore[0];
				if(fitnessManagerRef.brainFitnessComponentList[i].divideByTimeSteps) {
					score /= (float)numTimeSteps;
				}
				playerList[playingCurPlayer].dataManager.StoreGameRoundRawData(score, playingCurGeneration, playingCurAgent, playingCurTrialIndex, numSelectedComponents);                
				numSelectedComponents++;
			}
		}
	}

    private void ProcessFitnessScoresEndAgent(int agentIndex) {
        // Check all FitnessComponent scores for lowest/highest generation scores:

        int populationSize = playerList[playingCurPlayer].masterPopulation.populationMaxSize;

        for (int fitCompIndex = 0; fitCompIndex < playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].fitnessManager.masterFitnessCompList.Count; fitCompIndex++) {
            float agentRawScore = playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].agentDataArray[agentIndex].rawValueTotal;
            float curLowestScore = playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore;
            float curHighestScore = playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].highestScore;
            string fitComponentName = playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].fitnessManager.masterFitnessCompList[fitCompIndex].componentName;
            //Debug.Log("ProcessFitnessScoresEndAgent(" + agentIndex.ToString() + ") " + fitComponentName + ", raw: " + agentRawScore.ToString() + ", low: " + curLowestScore.ToString() + ", high: " + curHighestScore.ToString());
            if(agentIndex == 0) {
                playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore = agentRawScore;
                playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].highestScore = agentRawScore;
            }
            else {
                if (agentRawScore < curLowestScore) {
                    playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore = agentRawScore;
                }
                if (agentRawScore > curHighestScore) {
                    playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].highestScore = agentRawScore;
                }
            }

            // Add Agent's raw score totals for this Trial (all game rounds):
            playerList[playingCurPlayer].dataManager.generationDataList[playingCurGeneration].trialDataArray[playingCurTrialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore += agentRawScore;
        }
    }

    private void ProcessFitnessScoresEndGeneration() {
		// Just before NextGeneration, need to tally up and combine scores from each of the Trials for each Player's Agents
		// Loop through Players
		for(int p = 0; p < playingNumPlayers; p++) {
            //		Get Population Size,
            int populationSize = playerList[p].masterPopulation.populationMaxSize;
            float[] agentScores01 = new float[populationSize];
            float mostLinks = 0f;
            for(int i = 0; i < populationSize; i++) { // Init Array:
                agentScores01[i] = 0f;
                // TEMPORARY!!!  -- adds a cost to having larger brains!!
                float numLinks = 0f;
                for(int b = 0; b < playerList[p].masterPopulation.masterAgentArray[i].brainGenome.linkNEATList.Count; b++) {
                    if(playerList[p].masterPopulation.masterAgentArray[i].brainGenome.linkNEATList[b].enabled) {
                        numLinks++;
                    }
                }
                if(numLinks > mostLinks) {
                    mostLinks = numLinks;
                }
            }
            if (mostLinks == 0f)
                mostLinks = 1f;
            // LOOP through all FITNESS COMPONENTS (by way of trials)
            for (int trialIndex = 0; trialIndex < playerList[p].masterTrialsList.Count; trialIndex++) {
                // CAN I RELY ON [fitnessManager.masterFitnessCompList] ?????
                for (int fitCompIndex = 0; fitCompIndex < playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList.Count + 1; fitCompIndex++) {
                    // Loop Through all AGENTS:
                    for (int a = 0; a < populationSize; a++) {
                        // TEMPORARY!!! for large brain penalty!!!
                        if(fitCompIndex == playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList.Count) {
                            float largeBrainCost = playerList[p].masterPopulation.masterAgentArray[a].brainGenome.linkNEATList.Count / mostLinks;
                            float largeBrainPenaltyMultiplier = playerList[p].masterCupid.largeBrainPenalty;
                            largeBrainCost *= largeBrainPenaltyMultiplier;
                            agentScores01[a] = Mathf.Max(0f, agentScores01[a] - largeBrainCost);  // don't allow negative
                            //Debug.Log("Brain Penalty Agent# " + a.ToString() + ": " + largeBrainCost.ToString() + ", fitness: " + agentScores01[a].ToString());
                        }
                        else {  // proceed normally:
                                // get agent's raw fitness score for this fitness component:
                                // requires re-arranging dataManager in a more FitnessComponent-centric way:
                            float rawComponentScore = playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].agentDataArray[a].rawValueTotal;
                            float lowestScore = playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore;
                            float highestScore = playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].highestScore;
                            float scoreRange = highestScore - lowestScore;
                            if (scoreRange == 0f) scoreRange = 1f; // PREVENT DIVIDE BY ZERO
                                                                   // remap to 0-1:
                            rawComponentScore -= lowestScore;
                            float score01 = rawComponentScore / scoreRange;
                            if (playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList[fitCompIndex].bigIsBetter == false) {
                                score01 = 1f - score01;
                            }
                            score01 = Mathf.Pow(score01, playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList[fitCompIndex].power);
                            // figure out this fitnessComponent's share of total
                            // should I store totalSumOfWeights, or calculate it at beginning of this function?
                            float trialPieSlice = playerList[p].masterTrialsList[trialIndex].weight / playerList[p].dataManager.generationDataList[playingCurGeneration].totalSumOfWeights;
                            float fitnessComponentPieSlice = playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList[fitCompIndex].weight / playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].totalSumOfWeights;
                            float weightMultiplier = trialPieSlice * fitnessComponentPieSlice;

                            // Add score to Agent's total 0-1 fitness score ... stored
                            agentScores01[a] += score01 * weightMultiplier;
                        }                        
                    }

                    if (fitCompIndex < playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList.Count) {  // TEMPORARY large brain penalty safeguard!
                        // Divide total scores by number of game rounds to stay normalized:
                        playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore /= playerList[p].masterTrialsList[trialIndex].numberOfPlays;
                        //Update all-time records:                    
                        if (playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore < playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore) {
                            playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore = playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore;
                        }
                        if (playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore > playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].highestScore) {
                            playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].highestScore = playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore;
                        }
                        if (playingCurGeneration == 0) {
                            playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore = playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore;
                            playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].highestScore = playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore;
                        }
                    }
                    
                    //Debug.Log("ProcessFitnessScoresEndGeneration(" + playingCurGeneration.ToString() + ") fitComp[" + fitCompIndex.ToString() + "], totalScore: " + playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore.ToString() + ", low: " + playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore.ToString() + ", high: " + playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].highestScore.ToString());
                }
            }
            // SAVE FINAL 0-1 SCORE FOR EACH AGENT HERE:   (this is the value that will be used to rank the agents during crossover!)
            for (int j = 0; j < populationSize; j++) { // Init Array:
                string fitComponentScores = "";
                for (int fitCompIndex = 0; fitCompIndex < playerList[p].masterTrialsList[0].fitnessManager.masterFitnessCompList.Count; fitCompIndex++) {
                    fitComponentScores += playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[0].fitnessComponentDataArray[fitCompIndex].agentDataArray[j].rawValueTotal.ToString() + " || ";
                }
                //Debug.Log("ProcessFitnessScoresEndGeneration(" + playingCurGeneration.ToString() + ") agent " + j.ToString() + " score01= " + agentScores01[j].ToString());
                //Debug.Log("ProcessFitnessScoresEndGeneration(" + playingCurGeneration.ToString() + ") agent " + j.ToString() + "|| " +  fitComponentScores);
                playerList[p].masterPopulation.masterAgentArray[j].fitnessScore = agentScores01[j];
                playerList[p].masterPopulation.masterAgentArray[j].parentFitnessScoreA = agentScores01[j];
                int numMembersOfSpecies = playerList[p].masterPopulation.GetBreedingPoolByID(playerList[p].masterPopulation.speciesBreedingPoolList, playerList[p].masterPopulation.masterAgentArray[j].speciesID).agentList.Count;
                //Debug.Log("Trainer fit -- numMembersOfSpecies: " + numMembersOfSpecies.ToString() + ", speciesID: " + playerList[p].masterPopulation.masterAgentArray[j].speciesID.ToString());               
                playerList[p].masterPopulation.masterAgentArray[j].fitnessScoreSpecies = agentScores01[j] / (1f + numMembersOfSpecies * playerList[p].masterCupid.largeSpeciesPenalty);
                              
            }


            // WILL HAVE TO: cycle through ALL generations and update their final 0-1 scores based on global record highs/lows per-fitnessComponent
            //This will give a 0-1 value per-fitnessComponent which will be weighted based on currentGen settings -- this is done to ALL gens to keep the graph accurate
            for(int genIndex = 0; genIndex < playingCurGeneration + 1; genIndex++) {
                // LOOP through all FITNESS COMPONENTS (by way of trials)
                playerList[p].dataManager.generationDataList[genIndex].totalAllAgentsScore = 0f; // zero it out to start
                for (int trialIndex = 0; trialIndex < playerList[p].masterTrialsList.Count; trialIndex++) {
                    // CAN I RELY ON [fitnessManager.masterFitnessCompList] ?????
                    for (int fitCompIndex = 0; fitCompIndex < playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList.Count; fitCompIndex++) {
                        
                        float rawTotalComponentScore = playerList[p].dataManager.generationDataList[genIndex].trialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].totalRawScore;
                        float lowestScore = playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].lowestScore;
                        float highestScore = playerList[p].dataManager.recordScoresTrialDataArray[trialIndex].fitnessComponentDataArray[fitCompIndex].highestScore;
                        float scoreRange = highestScore - lowestScore;
                        if (scoreRange == 0f) scoreRange = 1f; // PREVENT DIVIDE BY ZERO
                                                               // remap to 0-1:
                        rawTotalComponentScore -= lowestScore;
                        float genScore01 = rawTotalComponentScore / scoreRange;
                        if (playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList[fitCompIndex].bigIsBetter == false) {
                            genScore01 = 1f - genScore01;
                        }
                        genScore01 = Mathf.Pow(genScore01, playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList[fitCompIndex].power);
                        // figure out this fitnessComponent's share of total
                        // should I store totalSumOfWeights, or calculate it at beginning of this function?
                        float trialPieSlice = playerList[p].masterTrialsList[trialIndex].weight / playerList[p].dataManager.generationDataList[playingCurGeneration].totalSumOfWeights;
                        float fitnessComponentPieSlice = playerList[p].masterTrialsList[trialIndex].fitnessManager.masterFitnessCompList[fitCompIndex].weight / playerList[p].dataManager.generationDataList[playingCurGeneration].trialDataArray[trialIndex].totalSumOfWeights;
                        float weightMultiplier = trialPieSlice * fitnessComponentPieSlice;

                        //Debug.Log("ProcessFitnessScoresEndGeneration(" + genIndex.ToString() + ", genScore01: " + genScore01.ToString() + ", mult: " + weightMultiplier.ToString());
                        playerList[p].dataManager.generationDataList[genIndex].totalAllAgentsScore += genScore01 * weightMultiplier;

                    }
                }
                //Debug.Log("ProcessFitnessScoresEndGeneration(" + genIndex.ToString() + ") totalAllAgentsScore: " + playerList[p].dataManager.generationDataList[genIndex].totalAllAgentsScore.ToString());
            }
            //Debug.Log("ProcessFitnessScoresEndGeneration(" + playingCurGeneration.ToString() + ") base01: " + baseGenerationScore01.ToString() + ", new01: " + newGenerationScore01.ToString() + ", scoreRatio: " + newScoreRatio.ToString());
            
            //Debug.Log (agentFitnessTotals);
            playerList[p].graphKing.BuildTexturesFitnessPerGen(playerList[p]);
			//playerList[p].graphKing.BuildTexturesHistoryPerGen(playerList[p]);
		}
	}
	#endregion

    public void BuildBrainMesh(BrainNEAT brain) {
        MiniGameManager currentGameManager = PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager;
        if (brainNetworkGO == null) {
            brainNetworkGO = new GameObject("brainNetworkGO");
            brainNetworkGO.AddComponent<MeshFilter>();
            
            //Material brainNetworkMat = new Material(Shader.Find("Custom/SimpleBrainNetworkShader"));
            brainNetworkGO.AddComponent<MeshRenderer>().material = brainNetworkMat;
            
            networkVisualizer = brainNetworkGO.AddComponent<BrainNetworkVisualizer>();
            
            if (currentGameManager.miniGameInstance.critterBeingTested != null) {
                networkVisualizer.sourceCritter = currentGameManager.miniGameInstance.critterBeingTested;
            }
            
            //currentGameManager.miniGameInstance.critterBeingTested.critterSegmentMaterial.SetTexture("_NeuronPosTex", networkVisualizer.neuronPositionsTexture);
        }
        MiniGameCritterWalkBasic minigame = (MiniGameCritterWalkBasic)currentGameManager.miniGameInstance as MiniGameCritterWalkBasic;
        networkVisualizer.InitShaderTexture(brain);
        minigame.SetShaderTextures(networkVisualizer);
        brainNetworkGO.GetComponent<MeshFilter>().sharedMesh = networkVisualizer.BuildNewMesh(brain);
    }
        
	private void UpdatePlayingState() {  // increments current state variables, keeping track of when each should roll-over into the next round
		
		MiniGameManager currentGameManager = PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager;  // <-- to help readability
		betweenGenerations = false;
		if(playingCurMiniGameTimeStep >= playingNumMiniGameTimeSteps || currentGameManager.miniGameInstance.gameEndStateReached) {  // hit time limit of minigame or the game reported a finish State:
			ProcessFitnessScoresEndRound();  // divides accumulated fitness score by time-steps, to keep it in 0-1 range and stores it in AgentScoresArray
			playingCurMiniGameTimeStep = 0;
			playingCurTrialRound++;  // Same agent, next round playthrough
            
            currentGameManager.miniGameInstance.ClearGame();

            if (playingCurTrialRound >= playingNumTrialRounds) {   // finished all rounds of current Trial for current Agent
				ProcessFitnessScoresEndAgent(playingCurAgent);  // combines the raw scores from all game rounds to get the agent's Fitness score for that TrialIndex
				playingCurTrialRound = 0;
				playingCurAgent++;  // Move on to next agent                

                if (playingCurAgent >= playingNumAgents) {
					playingCurAgent = 0;
					playingCurPlayer++;  // Now that the current player has changed, update how many trial rounds for new player

					if(playingCurPlayer >= playingNumPlayers) {  // finished all active Players for this Trial Index
						playingCurPlayer = 0;
						playingCurTrialIndex++;  // Move on to next Trial Index

						if(playingCurTrialIndex >= playingNumTrials) {
							ProcessFitnessScoresEndGeneration(); // combines the scores for all agents' TrialIndex's to get final Agent Fitness Scores
							playingCurTrialIndex = 0;
							betweenGenerations = true;
							//=========================
							// THE NEXT GENERATION!!!!!
							//=========================
							NextGenerationStart(); // Pending Changes Applied & Crossover

                            playingCurGeneration++;
                            // Initialize dataManager structs for new generation:
                            for (int p = 0; p < playerList.Count; p++) {
                                playerList[p].dataManager.InitializeNewGenerationDataArrays(playingCurGeneration);
                            }
                            // TrainingModifiers!!!
                            trainingModifierManager.ApplyTrainingModifierEffects(this);                           
                            for (int p = 0; p < numPlayers; p++) {  // iterate through playerList
                                trainingModifierManager.ApplyTrainingModifierEffectsTarget(this, PlayerList[p].masterTrialsList[0].miniGameManager.miniGameInstance);
                            }

                            UpdatePlayingNumTimeSteps();
							UpdatePlayingNumTrialRounds();
							UpdatePlayingNumAgents();
							UpdatePlayingNumPlayers();
							UpdatePlayingNumTrials();							
                        }
						else{ // reset player back to 0, incremented Trial
							UpdatePlayingNumTrialRounds();  // new player means potentially different number of trial rounds
							UpdatePlayingNumAgents();
							UpdatePlayingNumTimeSteps();
						}
					}
					else {  // reset current agent (finished all trial Plays, next player or next Trial Index:
						UpdatePlayingNumTimeSteps(); // Might not be correct placement....
						UpdatePlayingNumTrialRounds();
					}
				}
				else { // current Agent is incremented: 

                    // ##########################################################
                    // Might need to do something here to handle new Agent and thus new AgentBody

					if(!fastModeOn) {
						// UPDATE texture for current agent brain diagram with genome for new agent
						playerList[playingCurPlayer].graphKing.BuildTexturesCurAgentPerAgent(playerList[playingCurPlayer], playingCurAgent);
                        
                    }
					UpdatePlayingNumTimeSteps();
				}
			}
		}
	}

    #region UpdatePlayingStates
    public void UpdatePlayingNumTimeSteps() {
		playingNumMiniGameTimeSteps = PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].maxEvaluationTimeSteps;
	}
	public void UpdatePlayingNumTrialRounds() {
		playingNumTrialRounds = PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].numberOfPlays;
	}
	public void UpdatePlayingNumPlayers() {
		playingNumPlayers = playerList.Count;
		// Might keep the number of players static and just use a check to see if there is an active Trial at the specified Index for the cur Player
	}
	public void UpdatePlayingNumAgents() {  // Check how many agents in the current population
		playingNumAgents = playerList[playingCurPlayer].masterPopulation.populationMaxSize;
	}
	public void UpdatePlayingNumTrials() { // highest numbered Trial Index for any player

		int highestNumTrials = 0;
		for(int i = 0; i < numPlayers; i++) { // iterate through all active players
			for(int j = 0; j < PlayerList[i].masterTrialsList.Count; j++) {  // Count how many Trials each player has, keep track of highest number;
				if(j > highestNumTrials) {
					highestNumTrials = j;
				}
			}
		}
		playingNumTrials = highestNumTrials; // set number of Trials to iterate through
	}
    #endregion

    public void CalculateOneStep() {  // This is where the magic happens -- feeds game data to current Brain and Tick's the current mini-game
                                      // Based on current playingState data, looks up current Trial Index,
                                      // Look up current Player being evaluated, and see what Mini-game type is selected for that player.
                                      // If the mini-game is a multi-player game, evaluate all players for that trial simultaneously (SPLIT CODE PATH)
                                      // Else it's a normal day at the office:  Evaluate time step for current playingTrial index, current playingPlayer index
                                      // Send gameStateData (InputChannels List) through inputArray to the current Agent's Brain instance.
        //Debug.Log("CalculateOneStep() Trainer: " + playingCurMiniGameTimeStep.ToString());
        // v v v TO BE REPLACED EVENTUALLY!!!
        BrainNEAT currentBrain = PlayerList[playingCurPlayer].masterPopulation.masterAgentArray[playingCurAgent].brain;  // <-- to help readability
		MiniGameManager currentGameManager = PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager;  // <-- to help readability
		Agent curAgent = PlayerList[playingCurPlayer].masterPopulation.masterAgentArray[playingCurAgent];
        // DIG INTO THIS -- see where it is lost on the Agent
		currentGameManager.miniGameInstance.agentBodyGenomeBeingTested = curAgent.bodyGenome; // !!! RE_EVALUATE!!
		// ^ ^ ^ ^ PhysX Simulation step from last CalculateOneStep() happens before the following code in this Function:

		if(!currentGameManager.miniGameInstance.gameInitialized) { // If the miniGame exists but has not been Initialized -- very first timeStep of whole training session
                                                                   // Then Initialize the game data:
                                                                   //currentGameManager.miniGameInstance.Reset();
            //Debug.Log("CalculateOneStep()Trainer BEFORE: " + playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager.miniGameInstance.agentBodyBeingTested.creatureBodySegmentGenomeList[0].addOn1.ToString());
            currentGameManager.miniGameInstance.InstantiateGamePieces();  // first time Game has been Reset, so instantiateGamePieces
            // Currently, this just includes the C.O.M, targetSphere, and groundPlane
            // TrainingModifiers!!!
            trainingModifierManager.ApplyTrainingModifierEffects(this);
            currentGameManager.miniGameInstance.ResetTargetPositions(PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].numberOfPlays, PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].minEvaluationTimeSteps, PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].maxEvaluationTimeSteps);
            //currentGameManager.miniGameInstance.DisablePhysicsGamePieceComponents();
            currentGameManager.miniGameInstance.Reset(); // <-- OLD PLACEMENT
            currentGameManager.SetInputOutputArrays();
                       
            //BuildBrainMesh(playerList[playingCurPlayer].masterPopulation.masterAgentArray[playingCurAgent].brain);
            //currentGameManager.miniGameInstance.SetPhysicsGamePieceTransformsFromData();  // set the PhysX gamePieces based on gameData
            //currentGameManager.miniGameInstance.EnablePhysicsGamePieceComponents(); // create RigidBody and HingeJoint etc. components on the empty GameObjects
            //Debug.Log("CalculateOneStep()Trainer AFTER: " + playerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager.miniGameInstance.agentBodyBeingTested.creatureBodySegmentGenomeList[0].addOn1.ToString());
        }

		// If the game Ran a timeStep during the last call of FixedUpdate/CalculateOneStep, and there is pending PhysX Simulation data yet to be integrated:
		if(currentGameManager.miniGameInstance.gameTicked) {
			//Debug.Log ("CalculateOneStep() currentGameManager.miniGameInstance.gameTicked: " + playingCurMiniGameTimeStep.ToString());
			// This should only update if game Ran before this function call, otherwise it's skipped and goes onto GameSimulation
			currentGameManager.miniGameInstance.UpdateGameStateFromPhysX(); // integrate pending PhysX stateData back into primary gameData
            currentGameManager.miniGameInstance.SetNonPhysicsGamePieceTransformsFromData();
            if (currentGameManager.miniGameInstance.gameEndStateReached) {  // If, after the end of its gameLoop, some endGame conditions have been met:
				// game ended for reason other than timeOut	
			}
			// =================================================================================================================================================================
			//      END OF GAME LOOP !!!!
			// =================================================================================================================================================================
            
			playingCurMiniGameTimeStep++;
			currentGameManager.miniGameInstance.GameTimeStepCompleted();  // sets gameTicked, gameUpdatedFromPhysX to 0, increments internal curGameTimeStep
			UpdatePlayingState ();
			// This should check to see if the next gameTimeStep results in a rollover,
			// ... in which case it handles the incrementing of stateData and if a game needs to be Reset, resets curTimeStep to 0
		}

        if(currentGameManager.miniGameInstance.waitingForReset) {
            //Debug.Log("CalculateOneStep() waitingForReset");
            currentGameManager.miniGameInstance.gameCurrentRound = playingCurTrialRound;
            currentGameManager.miniGameInstance.Reset();
            currentGameManager.SetInputOutputArrays();
            if (currentGameManager.miniGameInstance.critterBeingTested != null) {
                //networkVisualizer.sourceCritter = currentGameManager.miniGameInstance.critterBeingTested;
                playerList[playingCurPlayer].masterPopulation.masterAgentArray[playingCurAgent].brain.ClearBrainState();  // zeroes out all values
            }
            //BuildBrainMesh(playerList[playingCurPlayer].masterPopulation.masterAgentArray[playingCurAgent].brain);
            
        }
        else {
            if (currentGameManager.miniGameInstance.gameCleared) {
                currentGameManager.miniGameInstance.waitingForReset = true;
            }
            else {
                // Check if this mini-game tests agents in isolation, or drops them all in the same arena together
                if (true) {  //if(miniGame testsIsolated) { If it's isolated, then it can use the multi-agent parallel independent testing:
                             // Check how many agents to be tested (how many mini-Game instance -- agent pairs)
                             // For ( agents to be tested ) 
                    for (int i = 0; i < 1; i++) {
                        
                        // Now with the updated values for position/velocity etc., pass input values into brains
                        if(playingCurMiniGameTimeStep == 1) {
                            Debug.Log("playingCurMiniGameTimeStep currentGameManager.brainInput: " + currentGameManager.brainInput.Count.ToString() + " currentGameManager.brainOutput: " + currentGameManager.brainOutput.Count.ToString());
                        }
                        currentBrain.BrainMasterFunction(ref currentGameManager.brainInput, ref currentGameManager.brainOutput);
                        // Run the game for one timeStep: (Note that this will only modify non-physX variables -- the actual movement and physX sim happens just afterward -- so keep that in mind)
                        currentGameManager.miniGameInstance.Tick();

                        // Luxuries at Low Speeds: Visualizations & Data read-outs
                        if(playbackSpeed < 6f) {                            
                            // Update Data text view:
                            gameControllerRef.trainerUI.panelDataViewScript.populationRef = playerList[playingCurPlayer].masterPopulation;
                            gameControllerRef.trainerUI.panelDataViewScript.minigameRef = currentGameManager.miniGameInstance;
                            gameControllerRef.trainerUI.panelDataViewScript.dataManagerRef = playerList[playingCurPlayer].dataManager;
                            gameControllerRef.trainerUI.panelDataViewScript.SetCurrentAgentID(playingCurAgent);
                            gameControllerRef.trainerUI.panelDataViewScript.UpdateDataText();
                            //MiniGameCritterWalkBasic minigame = (MiniGameCritterWalkBasic)currentGameManager.miniGameInstance as MiniGameCritterWalkBasic;
                            //networkVisualizer.InitShaderTexture(brain);
                            //minigame.SetShaderTextures(networkVisualizer);
                            if(playbackSpeed < 3f) {
                                /*if (currentGameManager.miniGameInstance.critterBeingTested != null) {
                                    networkVisualizer.sourceCritter = currentGameManager.miniGameInstance.critterBeingTested;
                                    networkVisualizer.SetNeuronSegmentPositions(currentBrain);
                                }
                                networkVisualizer.UpdateVertexColors(currentBrain);
                                networkVisualizer.MoveNeurons(currentBrain, 1);
                                networkVisualizer.UpdateNodeVertexPositionsSphere(currentBrain);
                                networkVisualizer.UpdateConnectionVertexPositionsBezier(currentBrain);
                                */
                            }
                        }                        
                    }
                    // Update playingState now that multiple agents have been evaluated -- messing up the playingCurAgent
                }
                //else {  // if miniGame tests all agents together:
                //	// Do whatever...
                //}

                // .......................................
                // ... Next is PhysX Simulation (internal Unity code that I can't access)
            }
        }
    }

	public void NextGenerationStart() {
		gameControllerRef.trainerUI.CheckForAllPendingApply(); // apply any pending edits
		gameControllerRef.trainerUI.panelDataVisScript.InitializePanelWithTrainerData();

        // Update Data text view:
        MiniGameManager currentGameManager = PlayerList[playingCurPlayer].masterTrialsList[playingCurTrialIndex].miniGameManager;  // <-- to help readability
        gameControllerRef.trainerUI.panelDataViewScript.populationRef = playerList[playingCurPlayer].masterPopulation;
        gameControllerRef.trainerUI.panelDataViewScript.minigameRef = currentGameManager.miniGameInstance;
        gameControllerRef.trainerUI.panelDataViewScript.dataManagerRef = playerList[playingCurPlayer].dataManager;
        gameControllerRef.trainerUI.panelDataViewScript.SetCurrentAgentID(playingCurAgent);
        gameControllerRef.trainerUI.panelDataViewScript.UpdateDataText();

        // CROSSOVER!!!!!! ++++++++++++++++++
        if (crossoverOn) {
			for(int p = 0; p < numPlayers; p++) {  // iterate through playerList
				//Debug.Log ("crossover, player" + p.ToString () + ", " + playerList[p].MasterPopulation.brainType.ToString() + playerList[p].masterCupid.tempName);
				playerList[p].masterCupid.PerformCrossover(ref playerList[p].masterPopulation, playingCurGeneration);
                //Debug.Log("playerList[p].masterPopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x: " + playerList[p].masterPopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x.ToString());
                //for(int t = 0; t < playerList[p].masterTrialsList.Count; t++ ) { // Hacky zero out fitness Scores
                //playerList[p].masterTrialsList[t].fitnessManager = 0f;
                //}
                //for(int m = 0; m < playerList[p].MasterPopulation.masterAgentArray.Length; m++) {
                //	playerList[p].MasterPopulation.masterAgentArray[m].brain.genome.PrintBiases();
                //}
                // looks at player's Population instance and does Crossover based on masterCupid instances settings, and population's size/brain-type
                // Creates a new population and updates the player's masterPopulation to that new Agent population

                // Randomly generates the target positions in each minigame once now, and use those positions for all agents:
                //for (int t = 0; t < PlayerList[p].masterTrialsList.Count; t++) {
                //    Debug.Log("TEST@@" + PlayerList[p].masterTrialsList[t].miniGameManager.miniGameInstance.ToString());
                //    PlayerList[p].masterTrialsList[t].miniGameManager.miniGameInstance.ResetTargetPositions(PlayerList[p].masterTrialsList[t].numberOfPlays);
                //}                
            }
		}        
    }

	public string GetCurrentGamePlayingState() {
		string output = "Current Generation: " + playingCurGeneration + 
						"\nCurrent Trial Index: " + (playingCurTrialIndex + 1) + " / " + playingNumTrials + 
						"\nCurrent Player: " + (playingCurPlayer + 1) + " / " + playingNumPlayers + 
						"\nCurrent Game Play Count: " + (playingCurTrialRound + 1) + " / " + playingNumTrialRounds + 
						"\nCurrent Agent: " + (playingCurAgent + 1) + " / " + playingNumAgents + 
						"\nCurrent Game Time Step: " + (playingCurMiniGameTimeStep + 1) + "/" + playingNumMiniGameTimeSteps;
		return output;
	}
}
