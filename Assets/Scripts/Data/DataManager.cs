using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager {

	// This class is in charge of storing all the data for the Current Player over the course of its training run
	private Player playerRef;

	// Playing Game States! 
	private int writingCurGeneration = -1;
	private int writingCurPlayer = -1;
	//private int writingNumPlayers = 0;
	private int writingCurAgent = -1;
	//private int writingNumAgents = 0;
	private int writingCurTrialIndex = -1;
	//private int writingNumTrials = 0; 
	private int writingCurGameRound = -1;
	//private int writingNumGameRounds = 0;

	// VARIABLES FOR TEMP DATA:



	// DATA:

	//public FitnessData playerFitnessData; // Holds all the training data for the player that owns this DataManager
	public List<GenerationData> generationDataList;  // Holds all the training data for the player that owns this DataManager

	// Average Fitness Scores for all generations
	// For each Trial:
	//     For each Fitness Component:
	//         Store Raw FitComponent Value (only adjust all to bigger is better)
	//         Store Modified Fitness Score value - After power & weight

	// FitnessData
	//     Holds all stored fitness data for this player - Array/List of GenerationData
	//     Holds Number of Generations
	/*public struct FitnessData {
		private List<GenerationData> generationDataList;
		
		public FitnessData() {
			generationDataList = new List<GenerationData>();
		}
	}*/
	// GameRoundFitnessComponentData  // MIGHT NOT BE NEEDED !!!!
	//     Contains a raw and weighted score for one round of a minigame trial, for one fitness component
	/*public struct GameRoundComponentData {
		private float rawValue;
		private float weightedValue;

		public GameRoundComponentData() {

		}
	}*/

	// Array/List/1D Texture:  Index --> Generation#, and Value = 0-1 Total Score
	// or
	// Array/List/2D Texture: X-Index --> Generation#, Y-Index --> [0]=Raw or [1]=Modified, and Value[X,Y] = 0-1 Total Score
	// Array/List/1-2D Texture: X-Index --> Agent#, Value --> 0-1 Total Score


	public DataManager(Player player) {
		playerRef = player;

		generationDataList = new List<GenerationData>();
		//Debug.Log (generationDataList.Count.ToString());
		//playerFitnessData = new FitnessData();
	}

	// 

	public void InitializeNewGenerationDataArrays(int generationIndex) {
		// For This Player:
		if(generationIndex >= generationDataList.Count) { // if new generation needs to be created
			//Debug.Log ("InitializeNewGenerationDataArrays" + generationIndex.ToString());

			int numAgents = playerRef.masterPopulation.populationMaxSize;
			GenerationData newGenerationData = new GenerationData(numAgents); // Now I have a generationData wrapper and an array of AgentData's

			//     Get number of Trials
			int numActiveTrials = 0;
			for(int i = 0; i < playerRef.masterTrialsList.Count; i++) { // First loop needed just to Count
				if(playerRef.masterTrialsList[i].miniGameManager.gameType != MiniGameManager.MiniGameType.None) {  // if active trial
					numActiveTrials++;
				}
			}
			//Debug.Log ("NumActiveTrials= " + numActiveTrials.ToString() + ", NumAgents= " + numAgents.ToString());
			// Loop over all agents?
			for(int j = 0; j < numAgents; j++) {
				AgentData newAgentData = new AgentData(numActiveTrials);

				// Fill the AgentData's TrialsList:		
				int curTrialIndex = 0;
				int totalNumComponents = 0;
				for(int i = 0; i < playerRef.masterTrialsList.Count; i++) {
					if(playerRef.masterTrialsList[i].miniGameManager.gameType != MiniGameManager.MiniGameType.None) {  // if active trial
						//     For each Trial, Get number of FitnessComponents & GameRounds
						int numGameRounds = playerRef.masterTrialsList[i].numberOfPlays;
						int totalFitnessComponents = playerRef.masterTrialsList[i].fitnessManager.brainFitnessComponentList.Count + 
														playerRef.masterTrialsList[i].fitnessManager.gameFitnessComponentList.Count;
						int numActiveFitnessComponents = 0;
						// Brain Components:
						for(int b = 0; b < playerRef.masterTrialsList[i].fitnessManager.brainFitnessComponentList.Count; b++) {
							if(playerRef.masterTrialsList[i].fitnessManager.brainFitnessComponentList[b].on) {
								numActiveFitnessComponents++;
								totalNumComponents++;
							}
						}
						// Game Components:
						for(int g = 0; g < playerRef.masterTrialsList[i].fitnessManager.gameFitnessComponentList.Count; g++) {
							if(playerRef.masterTrialsList[i].fitnessManager.gameFitnessComponentList[g].on) {
								numActiveFitnessComponents++;
								totalNumComponents++;
							}
						}
						
						int curFitnessComponentIndex = 0;
						TrialData newTrialData = new TrialData(numActiveFitnessComponents);
						for(int b = 0; b < playerRef.masterTrialsList[i].fitnessManager.brainFitnessComponentList.Count; b++) {
							if(playerRef.masterTrialsList[i].fitnessManager.brainFitnessComponentList[b].on) {
								newTrialData.fitnessComponentDataArray[curFitnessComponentIndex] = new FitnessComponentData(numGameRounds);
								curFitnessComponentIndex++;
							}
						}
						for(int g = 0; g < playerRef.masterTrialsList[i].fitnessManager.gameFitnessComponentList.Count; g++) {
							if(playerRef.masterTrialsList[i].fitnessManager.gameFitnessComponentList[g].on) {
								newTrialData.fitnessComponentDataArray[curFitnessComponentIndex] = new FitnessComponentData(numGameRounds);
								curFitnessComponentIndex++;
							}
						}
						newAgentData.trialDataArray[curTrialIndex] = newTrialData; // Set AgentData for this agent.
						//Debug.Log ("curTrialIndex= " + curTrialIndex.ToString() + ", numGameRounds= " + numGameRounds.ToString() + ", numFitnessComponents= " + totalFitnessComponents.ToString() + ", numActiveFitnessComponents= " + numActiveFitnessComponents.ToString());
						curTrialIndex++; // before or after?
							
					}
				}
				newGenerationData.agentDataArray[j] = newAgentData; // actually assign the new AgentData instance to generationData
				newGenerationData.totalNumFitnessComponents = totalNumComponents;
			}	
			//
			generationDataList.Add (newGenerationData); // Add the newly created generation data to the master list
		}               
	}

	public void StoreGameRoundRawData(float rawScore, int curGeneration, int curAgent, int curTrial, int curFitComp, int curGameRound) { // One play of a game has been completed
		generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[curFitComp].rawValuesArray[curGameRound] = rawScore;
		generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[curFitComp].rawValueTotal += rawScore;
	}
	public void StoreGameRoundWeightedData(float weightedScore, int curGeneration, int curAgent, int curTrial, int curFitComp, int curGameRound) { // One play of a game has been completed
		generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[curFitComp].weightedValuesArray[curGameRound] = weightedScore;
		generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[curFitComp].weightedValueTotal += weightedScore;
	}
	// Trying to remove this method:
	public void AverageTrialDataPerFitnessComponent(int curGeneration, int curAgent, int curTrial) {
		int numFitnessComponents = generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray.Length;
		for(int i = 0; i < numFitnessComponents; i++) {
			// Calculate Average Scores per raw fitness Component:
			generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].rawValueAvg = 
				generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].rawValueTotal / 
					(float)generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].rawValuesArray.Length; // Divided by numGameRounds

			// Add total score for this TrialIndex to total agent scores ( to later be averaged over numTrials)
			generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].rawValueTotal += generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].rawValueTotal;
			// Calculate Average Scores per weighted fitness Component:
			generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].weightedValueAvg = 
				generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].weightedValueTotal / 
					(float)generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].weightedValuesArray.Length;
			// Add score for this TrialIndex to total agent scores ( to later be averaged over numTrials)
			generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].weightedValueTotal += generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].fitnessComponentDataArray[i].weightedValueAvg;
		}
		generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].rawValueAvg = generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].rawValueTotal / (float)numFitnessComponents;
		generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].weightedValueAvg = generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].weightedValueTotal / (float)numFitnessComponents;
		// Once totals for this specific Trial are calculated, add this value to total AgentScore so it can be avged by numTrials Later.
		generationDataList[curGeneration].agentDataArray[curAgent].rawValueTotal += generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].rawValueAvg;
		generationDataList[curGeneration].agentDataArray[curAgent].weightedValueTotal += generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray[curTrial].weightedValueAvg;
	}

	public void AverageTrialScoresPerAgent(int curGeneration, int curAgent) {
		/*
		// Iterate through each TrialIndex
		int numTrials = playerRef.masterTrialsList.Count - 1;
		for(int i = 0; i < numTrials; i++) {
			// for each trial, iterate through that Trial's FitnessComponentList
			int numFitnessComponents = generationDataList[curGeneration].agentDataArray[0].trialDataArray[i].fitnessComponentDataArray.Length;
			for(int j = 0; j < numFitnessComponents; j++) {
				// Find Min & Max scores for each fitness component by
				// Iterating through all agents and checking their raw scores
				int numAgents = generationDataList[curGeneration].agentDataArray.Length;
				float minValue = generationDataList[curGeneration].agentDataArray[0].trialDataArray[i].fitnessComponentDataArray[j].rawValueTotal;
				float maxValue = generationDataList[curGeneration].agentDataArray[0].trialDataArray[i].fitnessComponentDataArray[j].rawValueTotal;
				for(int k = 0; k < numAgents; k++) {
					// check score for this agent, for this trial, this fitnessComp, accumulated from all game rounds
					float score = generationDataList[curGeneration].agentDataArray[k].trialDataArray[i].fitnessComponentDataArray[j].rawValueTotal;
					if(score < minValue) {
						minValue = score; // update new record minimum
					}
					if(score > maxValue) {
						maxValue = score; // update new record maximum value
					}
				}

				// Iterate through agents again and remap all values to 0-1 range, now that the range is known:
				for(int a = 0; a < numAgents; a++) {
					float originalRange = maxValue - minValue;
					float remappedScore;
					if(originalRange != 0f) {
						remappedScore = (generationDataList[curGeneration].agentDataArray[a].trialDataArray[i].fitnessComponentDataArray[j].rawValueTotal - minValue) / originalRange;
					}
					else {
						remappedScore = 0f;
					}
					// biggerIsBetter:
					FitnessManager fitnessManagerRef = playerRef.masterTrialsList[i].fitnessManager;
					if(fitnessManagerRef.masterFitnessCompList[j].bigIsBetter == false) {
						//remappedScore = 1f - remappedScore;
					}
					// Keep track of weight amounts:
					generationDataList[curGeneration].agentDataArray[a].trialDataArray[i].totalSumOfWeights += fitnessManagerRef.masterFitnessCompList[j].weight;
					// set 0-1 range score:
					generationDataList[curGeneration].agentDataArray[a].trialDataArray[i].fitnessComponentDataArray[j].rawValue01 = remappedScore;
					// Add score for this TrialIndex to total agent scores ( to later be averaged over numTrials)
					//generationDataList[curGeneration].agentDataArray[a].trialDataArray[i].rawValueTotal += remappedScore;
					if(a == 0) {
						//Debug.Log ("AverageTrialScoresPerAgent(); 01: " + remappedScore.ToString() + ", min: " + minValue.ToString() + ", max: " + maxValue.ToString());
					}
				}
			}
			// for each Trial, keep track of its weight value for later:
			generationDataList[curGeneration].totalSumOfWeights += playerRef.masterTrialsList[i].weight;
		}
		// At this point, each fitness component raw score is stored, along with total weight values for averaging purposes;

		// Iterate through data for each agent AGAIN
		int agentCount = generationDataList[curGeneration].agentDataArray.Length;
		for(int agentID = 0; agentID < agentCount; agentID++) {
			// For each Trial:
			for(int trialIndex = 0; trialIndex < numTrials; trialIndex++) {
				// Total up raw scores and average by number of fitness components
				// Get weighted score by multiplying by share of totalWeights for that Trial
				FitnessManager fitnessManagerRef = playerRef.masterTrialsList[trialIndex].fitnessManager;
				// For each fitness component:
				int numFitnessComponents = generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].fitnessComponentDataArray.Length;
				for(int fitID = 0; fitID < numFitnessComponents; fitID++) {
					generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].rawValueAvg += 
						generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].fitnessComponentDataArray[fitID].rawValue01 / numFitnessComponents;
					// calculate proportion of total weights for fitness components:
					float weightShare = fitnessManagerRef.masterFitnessCompList[fitID].weight / generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].totalSumOfWeights;
					generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].weightedValueAvg +=
						Mathf.Pow (generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].fitnessComponentDataArray[fitID].rawValue01, fitnessManagerRef.masterFitnessCompList[fitID].power) * weightShare;
				}
				// Set Agent's total raw score by tallying up its scores for each TrialIndex
				generationDataList[curGeneration].agentDataArray[agentID].rawValueAvg += generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].rawValueAvg / numTrials;
				float trialWeightShare = playerRef.masterTrialsList[trialIndex].weight / generationDataList[curGeneration].totalSumOfWeights;
				generationDataList[curGeneration].agentDataArray[agentID].weightedValueAvg += 
					Mathf.Pow (generationDataList[curGeneration].agentDataArray[agentID].trialDataArray[trialIndex].weightedValueAvg, playerRef.masterTrialsList[trialIndex].power) * trialWeightShare;
			}
			//Debug.Log ("AverageTrialScoresPerAgent(); " + agentID.ToString() + ", raw: " + generationDataList[curGeneration].agentDataArray[agentID].rawValueAvg.ToString() + ", weighted: " + generationDataList[curGeneration].agentDataArray[agentID].weightedValueAvg.ToString());
			generationDataList[curGeneration].totalAgentScoresRaw += generationDataList[curGeneration].agentDataArray[agentID].rawValueAvg;
			generationDataList[curGeneration].totalAgentScoresWeighted += generationDataList[curGeneration].agentDataArray[agentID].weightedValueAvg;
		}
		// Store Raw Value
		// modify raw value by power and weight
		// Store Weighted Value
		*/



		// OLD OLD OLD OLD !!!! OLD OLD OLD OLD !!!! OLD OLD OLD OLD !!!! OLD OLD OLD OLD !!!! OLD OLD OLD OLD !!!! OLD OLD OLD OLD !!!!

		// Set value for total fitness score for this Agent (Over all Trials):
		generationDataList[curGeneration].agentDataArray[curAgent].rawValueAvg = generationDataList[curGeneration].agentDataArray[curAgent].rawValueTotal / 
			(float)generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray.Length; // Divided by number of Trials
		//Debug.Log ("AverageTrialScoresPerAgent(); avgRawScore: " + generationDataList[curGeneration].agentDataArray[curAgent].rawValueAvg.ToString());
		// Add this agent's score to the generation Total:
		generationDataList[curGeneration].totalAgentScoresRaw += generationDataList[curGeneration].agentDataArray[curAgent].rawValueAvg;

		generationDataList[curGeneration].agentDataArray[curAgent].weightedValueAvg = generationDataList[curGeneration].agentDataArray[curAgent].weightedValueTotal / 
			(float)generationDataList[curGeneration].agentDataArray[curAgent].trialDataArray.Length; // Divided by number of Trials
		generationDataList[curGeneration].totalAgentScoresWeighted += generationDataList[curGeneration].agentDataArray[curAgent].weightedValueAvg;

	}

	public void AverageTrialScoresPerGen(int curGeneration) {
		// Average Values over the entire Generation
		//Debug.Log ("AverageTrialScoresPerGen(); avgRawScore: " + generationDataList[curGeneration].avgAgentScoreRaw.ToString());
		generationDataList[curGeneration].avgAgentScoreRaw = generationDataList[curGeneration].totalAgentScoresRaw / (float)generationDataList[curGeneration].agentDataArray.Length;
		generationDataList[curGeneration].avgAgentScoreWeighted = generationDataList[curGeneration].totalAgentScoresWeighted / (float)generationDataList[curGeneration].agentDataArray.Length;

		// Calculate Generation average genome:
		int numAgents = generationDataList[curGeneration].agentDataArray.Length;
		Genome avgGenome = new Genome();
		avgGenome.genomeBiases = new float[playerRef.masterPopulation.masterAgentArray[0].genome.genomeBiases.Length];
		avgGenome.genomeWeights = new float[playerRef.masterPopulation.masterAgentArray[0].genome.genomeWeights.Length];
		avgGenome.ZeroGenome(); // set all values to 0f;
		for(int i = 0; i < numAgents; i++) {
			// iterate through Bias Arrays and add each agent's bias value to the avg
			for(int b = 0; b < avgGenome.genomeBiases.Length; b++) {
				avgGenome.genomeBiases[b] += playerRef.masterPopulation.masterAgentArray[i].genome.genomeBiases[b] / (float)numAgents;
			}
			// iterate through Weight Arrays and add each agent's weight value to the avg
			for(int w = 0; w < avgGenome.genomeWeights.Length; w++) {
				avgGenome.genomeWeights[w] += playerRef.masterPopulation.masterAgentArray[i].genome.genomeWeights[w] / (float)numAgents;
			}
		}
		// Save the genome values to this generation Data:
		generationDataList[curGeneration].genAvgGenome.genomeBiases = avgGenome.genomeBiases;
		generationDataList[curGeneration].genAvgGenome.genomeWeights = avgGenome.genomeWeights;
	}
}
