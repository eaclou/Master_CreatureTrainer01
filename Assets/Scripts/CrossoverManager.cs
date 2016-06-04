﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossoverManager {

	// SETTINGS:
	public string tempName = "name!";

    public bool useMutation = true;
    public bool useCrossover = true;
    public bool useSpeciation = true;

    // Body OLD:
    public float mutationBodyChance = 0.5f;
    public float maxBodyMutationFactor = 1.25f;
    // MUTATION:
    public float masterMutationRate = 0.2f;
	public float maximumWeightMagnitude = 4f;
	public float mutationDriftScale = 0.45f;
	public float mutationRemoveLinkChance = 0.1f;
	public float mutationAddLinkChance = 0.2f;	
    public float mutationRemoveNodeChance = 0.0f;
    public float mutationAddNodeChance = 0.0f;
    public float mutationActivationFunctionChance = 0.0f;
    public float largeBrainPenalty = 0.05f;
    public float newLinkMutateBonus = 1f;  // 1 = does nothing 
    public int newLinkBonusDuration = 20;
    public float existingNetworkBias = 0.05f;
    public float existingFromNodeBias = 0.5f;
    // CROSSOVER!!:
    public float crossoverRandomLinkChance = 0f;
    // SPECIES!!!:
    public float speciesSimilarityThreshold = 1.5f;
    public float excessLinkWeight = 0.425f;
    public float disjointLinkWeight = 0.425f;
    public float linkWeightWeight = 0.15f;
    public float normalizeExcess = 0f;
    public float normalizeDisjoint = 0f;
    public float normalizeLinkWeight = 1f;
    public float adoptionRate = 0.05f;
    public float largeSpeciesPenalty = 0.04f;
    public float interspeciesBreedingRate = 0.01f; 

    public float survivalRate = 0.03f;   // top-performers are copied into the next generation
	public bool survivalByRank = true;
	public bool survivalStochastic = false;
	public bool survivalByRaffle = false;

	public float breedingRate = 0.6f;   // percentage of population that performed well enough to breed
	public bool breedingByRank = false;
	public bool breedingStochastic = false;
	public bool breedingByRaffle = true;

    //empty constructor for easySave2
    public CrossoverManager() {

	}

	public void CopyFromSourceCrossoverManager(CrossoverManager sourceManager) {

		tempName = sourceManager.tempName;

        useMutation = sourceManager.useMutation;
        useCrossover = sourceManager.useCrossover;
        useSpeciation = sourceManager.useSpeciation;

        masterMutationRate = sourceManager.masterMutationRate;
		maximumWeightMagnitude = sourceManager.maximumWeightMagnitude;
		mutationDriftScale = sourceManager.mutationDriftScale;
		mutationRemoveLinkChance = sourceManager.mutationRemoveLinkChance;
		mutationAddLinkChance = sourceManager.mutationAddLinkChance;		
        mutationRemoveNodeChance = sourceManager.mutationRemoveNodeChance;
        mutationAddNodeChance = sourceManager.mutationAddNodeChance;
        mutationActivationFunctionChance = sourceManager.mutationActivationFunctionChance;
        largeBrainPenalty = sourceManager.largeBrainPenalty;
        newLinkMutateBonus = sourceManager.newLinkMutateBonus;
        newLinkBonusDuration = sourceManager.newLinkBonusDuration;
        existingNetworkBias = sourceManager.existingNetworkBias;
        existingFromNodeBias = sourceManager.existingFromNodeBias;

        crossoverRandomLinkChance = sourceManager.crossoverRandomLinkChance;

        speciesSimilarityThreshold = sourceManager.speciesSimilarityThreshold;
        excessLinkWeight = sourceManager.excessLinkWeight;
        disjointLinkWeight = sourceManager.disjointLinkWeight;
        linkWeightWeight = sourceManager.linkWeightWeight;
        normalizeExcess = sourceManager.normalizeExcess;
        normalizeDisjoint = sourceManager.normalizeDisjoint;
        normalizeLinkWeight = sourceManager.normalizeLinkWeight;
        adoptionRate = sourceManager.adoptionRate;
        largeSpeciesPenalty = sourceManager.largeSpeciesPenalty;
        interspeciesBreedingRate = sourceManager.interspeciesBreedingRate;
        
		survivalRate = sourceManager.survivalRate;
		survivalByRank = sourceManager.survivalByRank;
		survivalStochastic = sourceManager.survivalStochastic;
		survivalByRaffle = sourceManager.survivalByRaffle;

		breedingRate = sourceManager.breedingRate;
		breedingByRank = sourceManager.breedingByRank;
		breedingStochastic = sourceManager.breedingStochastic;
		breedingByRaffle = sourceManager.breedingByRaffle;
	}

	public void PerformCrossover(ref Population sourcePopulation, int gen) {
		//Population newPop = sourcePopulation.CopyPopulationSettings();
        BreedPopulation(ref sourcePopulation, gen);
                
        //Debug.Log("sourcePopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x: " + sourcePopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x.ToString());
    }

    /*public float[][] MixFloatChromosomes(float[][] parentFloatGenes, int numOffspring) {  // takes A number of Genomes and returns new mixed up versions
		int geneArrayLength = parentFloatGenes[0].Length;
		float[][] childFloatGenes = new float[numOffspring][];

		List<int> swapPositionsList = new List<int>();
		for(int s = 0; s < numSwapPositions; s++) {
			swapPositionsList.Add(UnityEngine.Random.Range (0,geneArrayLength));
			//Debug.Log ("swapPositionsList[" + s.ToString() + "]: " + swapPositionsList[s].ToString());
		}
		swapPositionsList.Sort(); // Ordered list of indices where the parent changes

		if(parentFloatGenes.Length == 1) {  // SINGLE PARENT
			for(int c = 0; c < numOffspring; c++) {  // for each childAgent:
				childFloatGenes[c] = new float[geneArrayLength]; // set child geneArray to proper length
				for(int i = 0; i < geneArrayLength; i++) { // iterate through genes in geneArray
					childFloatGenes[c][i] = parentFloatGenes[0][i]; // only one parent, hence index=0
					if(CheckForFloatMutation()) {
					// MMMUUUUTTTTTAAAAATTTTIIIOOOOOOONNNNNNNN!!!!!!!!!!!!
						childFloatGenes[c][i] = MutateFloat(childFloatGenes[c][i]);
					}
				}
			}
		}
		if(parentFloatGenes.Length == 2) {  // TWO PARENTS
			int currentParentIndex = UnityEngine.Random.Range (0,2); // 2 parents
			for(int c = 0; c < numOffspring; c++) {  // for each childAgent:
				childFloatGenes[c] = new float[geneArrayLength];
				//currentParentIndex = 1 - currentParentIndex; // 2 parents, swaps at beginning of geneArray
				//Debug.Log ("currentParentIndex: " + currentParentIndex.ToString());
				for(int i = 0; i < geneArrayLength; i++) { // iterate through genes in geneArray
					// Do Crossover of Array HERE: !!!!!
					if(swapPositionsList.Contains(i)) { // if current array index is a swap position
						currentParentIndex = 1 - currentParentIndex; // 2 parents, so just swaps
						//Debug.Log ("SWAP: currentParentIndex: " + currentParentIndex.ToString() + ", biasIndex: " + i.ToString());
					}
					childFloatGenes[c][i] = parentFloatGenes[currentParentIndex][i]; // 
					if(CheckForFloatMutation()) {
						// MMMUUUUTTTTTAAAAATTTTIIIOOOOOOONNNNNNNN!!!!!!!!!!!!
						childFloatGenes[c][i] = MutateFloat(childFloatGenes[c][i]);
					}
				}
			}
		}
		if(parentFloatGenes.Length > 2) {  // THREE OR MORE PARENTS
			int currentParentIndex = UnityEngine.Random.Range (0,parentFloatGenes.Length);
			for(int c = 0; c < numOffspring; c++) {  // for each childAgent:
				childFloatGenes[c] = new float[geneArrayLength];
				//currentParentIndex = 1 - currentParentIndex; // 2 parents, swaps at beginning of geneArray
				//Debug.Log ("currentParentIndex: " + currentParentIndex.ToString());
				for(int i = 0; i < geneArrayLength; i++) { // iterate through genes in geneArray
					// Do Crossover of Array HERE: !!!!!
					if(swapPositionsList.Contains(i)) { // if current array index is a swap position
						currentParentIndex = UnityEngine.Random.Range (0,parentFloatGenes.Length); // 3 parents, so picks next Parent Randomly
						//Debug.Log ("SWAP: currentParentIndex: " + currentParentIndex.ToString() + ", biasIndex: " + i.ToString());
					}
					childFloatGenes[c][i] = parentFloatGenes[currentParentIndex][i]; // 
					if(CheckForFloatMutation()) {
						// MMMUUUUTTTTTAAAAATTTTIIIOOOOOOONNNNNNNN!!!!!!!!!!!!
						childFloatGenes[c][i] = MutateFloat(childFloatGenes[c][i]);
					}
				}
			}
		}
		return childFloatGenes;
	}
    */

    public CritterGenome[] MixBodyChromosomes(CritterGenome[] parentGenomes, int numOffspring) {  // takes A number of Genomes and returns new mixed up versions
                                                                                                  //int geneArrayLength = parentGenomes.Length;
        CritterGenome[] childGenomes = new CritterGenome[numOffspring];
        Debug.Log("CURRENTLY BROKEN! public CritterGenome[] MixBodyChromosomes(CritterGenome[] parentGenomes, int numOffspring)");
        
		return childGenomes;
	}

	public bool CheckForFloatMutation() {
		float rand = UnityEngine.Random.Range(0f, 1f);
		if(rand < masterMutationRate) {
			return true;
		}
		return false;
	}
    public bool CheckForMutation(float rate) {
        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand < rate) {
            return true;
        }
        return false;
    }

	public bool CheckForBodyMutation() {
		float rand = UnityEngine.Random.Range(0f, 1f);
		if(rand < masterMutationRate) { // change to custom body mutation rate at some point
			return true;
		}
		return false;
	}

	public float MutateFloat(float sourceFloat) {
		float newFloat;
		
		newFloat = (sourceFloat * (1.0f - mutationDriftScale)) + 
			(Gaussian.GetRandomGaussian()*maximumWeightMagnitude) * mutationDriftScale;
		
		return newFloat;
	}

	public float MutateBodyFloat(float sourceFloat) {
		float newFloat;
		if(sourceFloat != 0f) {
			newFloat = sourceFloat * UnityEngine.Random.Range(1f/maxBodyMutationFactor, maxBodyMutationFactor);
		}
		else {
			newFloat = sourceFloat + UnityEngine.Random.Range(-0.1f, 0.1f);
		}

		return newFloat;
	}

    public Agent SelectAgentFromPopForBreeding(Population breedingPop, int numEligibleBreederAgents, ref int currentRankIndex) {
        //		Iterate over numberOfParents :
        //			Depending on method, select suitable agents' genome.Arrays until the numberOfPArents is reached, collect them in an array of arrays
        Agent newParentAgent = new Agent();
        // If breeding is by fitness score ranking:
        if (breedingByRank) {
            // Pop should already be ranked, so just traverse from top (best) to bottom (worst) to select parentAgents
            if (currentRankIndex >= numEligibleBreederAgents) { // if current rank index is greater than the num of eligible breeders, then restart the index to 0;
                currentRankIndex = 0;
            }
            newParentAgent = breedingPop.masterAgentArray[currentRankIndex];
            currentRankIndex++;
        }
        // if survival is completely random, as a control:
        if (breedingStochastic) {
            int randomAgent = UnityEngine.Random.Range(0, numEligibleBreederAgents - 1); // check if minus 1 is needed
                                                                                         // Set next newChild slot to a completely randomly-chosen agent
            newParentAgent = breedingPop.masterAgentArray[randomAgent];
        }
        // if survival is based on a fitness lottery:
        if (breedingByRaffle) {
            float totalScoreBreeders = 0f;
            for (int a = 0; a < numEligibleBreederAgents; a++) { // iterate through all agents
                totalScoreBreeders += breedingPop.masterAgentArray[a].fitnessScoreSpecies;
            }
            float randomSlicePosition = UnityEngine.Random.Range(0f, totalScoreBreeders);
            float accumulatedFitness = 0f;
            for (int a = 0; a < numEligibleBreederAgents; a++) { // iterate through all agents
                accumulatedFitness += breedingPop.masterAgentArray[a].fitnessScoreSpecies;
                // if accum fitness is on slicePosition, copy this Agent
                //Debug.Log("Breeding Agent " + a.ToString() + ": AccumFitness: " + accumulatedFitness.ToString() + ", RafflePos: " + randomSlicePosition.ToString() + ", totalScoreBreeders: " + totalScoreBreeders.ToString() + ", numEligibleBreederAgents: " + numEligibleBreederAgents.ToString());
                if (accumulatedFitness >= randomSlicePosition) {
                    //Debug.Log("Breeding Agent " + a.ToString() + "( " + sourcePopulation.masterAgentArray[a].fitnessScore.ToString() + "): AccumFitness: " + accumulatedFitness.ToString() + ", RafflePos: " + randomSlicePosition.ToString() + ", totalScoreBreeders: " + totalScoreBreeders.ToString() + ", numEligibleBreederAgents: " + numEligibleBreederAgents.ToString());
                    newParentAgent = breedingPop.masterAgentArray[a];
                    break;
                }
            }
        }
        return newParentAgent;
    }

    public Agent SelectAgentFromPoolForBreeding(SpeciesBreedingPool breedingPool) {
        //			Depending on method, select suitable agents' genome.Arrays until the numberOfPArents is reached, collect them in an array of arrays
        //Debug.Log("SelectAgentFromPoolForBreeding(): #Agents: " + breedingPool.agentList.Count.ToString() + ", nextAgent#: " + breedingPool.nextAgentIndex.ToString() + ", speciesID: " + breedingPool.speciesID.ToString());
        Agent newParentAgent = new Agent();
        // If breeding is by fitness score ranking:
        if (breedingByRank) {
            // Pop should already be ranked, so just traverse from top (best) to bottom (worst) to select parentAgents
            if (breedingPool.nextAgentIndex >= breedingPool.agentList.Count) { // if current rank index is greater than the num of eligible breeders, then restart the index to 0;
                breedingPool.nextAgentIndex = 0;
            }
            newParentAgent = breedingPool.agentList[breedingPool.nextAgentIndex];
            breedingPool.nextAgentIndex++;
            
        }
        // if survival is completely random, as a control:
        if (breedingStochastic) {
            int randomAgent = UnityEngine.Random.Range(0, breedingPool.agentList.Count); // check if minus 1 is needed
                                                                                         // Set next newChild slot to a completely randomly-chosen agent
            newParentAgent = breedingPool.agentList[randomAgent];
           
        }        
        // if survival is based on a fitness lottery:
        if (breedingByRaffle) {
            float totalScoreBreeders = 0f;
            for (int a = 0; a < breedingPool.agentList.Count; a++) { // iterate through all agents
                totalScoreBreeders += breedingPool.agentList[a].fitnessScoreSpecies;
            }
            float randomSlicePosition = UnityEngine.Random.Range(0f, totalScoreBreeders);
            float accumulatedFitness = 0f;
            for (int a = 0; a < breedingPool.agentList.Count; a++) { // iterate through all agents
                accumulatedFitness += breedingPool.agentList[a].fitnessScoreSpecies;
                // if accum fitness is on slicePosition, copy this Agent
                //Debug.Log("Breeding Agent " + a.ToString() + ": AccumFitness: " + accumulatedFitness.ToString() + ", RafflePos: " + randomSlicePosition.ToString() + ", totalScoreBreeders: " + totalScoreBreeders.ToString() + ", numEligibleBreederAgents: " + numEligibleBreederAgents.ToString());
                if (accumulatedFitness >= randomSlicePosition) {
                    //Debug.Log("Breeding Agent " + a.ToString() + "( " + sourcePopulation.masterAgentArray[a].fitnessScore.ToString() + "): AccumFitness: " + accumulatedFitness.ToString() + ", RafflePos: " + randomSlicePosition.ToString() + ", totalScoreBreeders: " + totalScoreBreeders.ToString() + ", numEligibleBreederAgents: " + numEligibleBreederAgents.ToString());
                    newParentAgent = breedingPool.agentList[a];
                    
                    break;
                }
            }
        }
        return newParentAgent;
    }

    

    public Population BreedPopulation(ref Population sourcePopulation, int currentGeneration) {
        
        // go through species list and adjust fitness
        List<SpeciesBreedingPool> childSpeciesPoolsList = new List<SpeciesBreedingPool>(); // will hold agents in an internal list to facilitate crossover
        //List<Species> childSpeciesList = new List<Species>();  // the new species of the next generation
        //Debug.Log("BreedPopulation -- SpeciesCount: " + sourcePopulation.speciesBreedingPoolList.Count.ToString());
        for (int s = 0; s < sourcePopulation.speciesBreedingPoolList.Count; s++) {            
            SpeciesBreedingPool newChildSpeciesPool = new SpeciesBreedingPool(sourcePopulation.speciesBreedingPoolList[s].templateGenome, sourcePopulation.speciesBreedingPoolList[s].speciesID);  // create Breeding Pools
            // copies the existing breeding pools but leaves their agentLists empty for future children
            childSpeciesPoolsList.Add(newChildSpeciesPool);            // Add to list of pools
            //Debug.Log("BreedPopulation -- newChildSpeciesPool# " + s.ToString() + ", id: " + newChildSpeciesPool.speciesID.ToString());
            //            
        }

        sourcePopulation.RankAgentArray(); // based on modified species fitness score, so compensated for species sizes
        
        Agent[] newAgentArray = new Agent[sourcePopulation.masterAgentArray.Length];

        // Calculate total fitness score:
        float totalScore = 0f;
        if (survivalByRaffle) {
            for (int a = 0; a < sourcePopulation.populationMaxSize; a++) { // iterate through all agents
                totalScore += sourcePopulation.masterAgentArray[a].fitnessScoreSpecies;
            }
        }

        // Figure out How many Agents survive
        int numSurvivors = Mathf.RoundToInt(survivalRate * (float)sourcePopulation.populationMaxSize);
        //Depending on method, one at a time, select an Agent to survive until the max Number is reached
        int newChildIndex = 0;
        // For ( num Agents ) {
        for (int i = 0; i < numSurvivors; i++) {
            // If survival is by fitness score ranking:
            if (survivalByRank) {
                // Pop should already be ranked, so just traverse from top (best) to bottom (worst)
                newAgentArray[newChildIndex] = sourcePopulation.masterAgentArray[newChildIndex];
                SpeciesBreedingPool survivingAgentBreedingPool = sourcePopulation.GetBreedingPoolByID(childSpeciesPoolsList, newAgentArray[newChildIndex].speciesID);
                survivingAgentBreedingPool.AddNewAgent(newAgentArray[newChildIndex]);
                //SortNewAgentIntoSpecies(newAgentArray[newChildIndex], childSpeciesList); // sorts this surviving agent into next generation's species'
                newChildIndex++;
            }
            // if survival is completely random, as a control:
            if (survivalStochastic) {
                int randomAgent = UnityEngine.Random.Range(0, numSurvivors - 1);
                // Set next newChild slot to a randomly-chosen agent within the survivor faction -- change to full random?
                newAgentArray[newChildIndex] = sourcePopulation.masterAgentArray[randomAgent];
                SpeciesBreedingPool survivingAgentBreedingPool = sourcePopulation.GetBreedingPoolByID(childSpeciesPoolsList, newAgentArray[newChildIndex].speciesID);
                survivingAgentBreedingPool.AddNewAgent(newAgentArray[newChildIndex]);
                //SortNewAgentIntoSpecies(newAgentArray[newChildIndex], childSpeciesList); // sorts this surviving agent into next generation's species'
                newChildIndex++;
            }
            // if survival is based on a fitness lottery:
            if (survivalByRaffle) {  // Try when Fitness is normalized from 0-1
                float randomSlicePosition = UnityEngine.Random.Range(0f, totalScore);
                float accumulatedFitness = 0f;
                for (int a = 0; a < sourcePopulation.populationMaxSize; a++) { // iterate through all agents
                    accumulatedFitness += sourcePopulation.masterAgentArray[a].fitnessScoreSpecies;
                    // if accum fitness is on slicePosition, copy this Agent
                    //Debug.Log("NumSurvivors: " + numSurvivors.ToString() + ", Surviving Agent " + a.ToString() + ": AccumFitness: " + accumulatedFitness.ToString() + ", RafflePos: " + randomSlicePosition.ToString() + ", TotalScore: " + totalScore.ToString() + ", newChildIndex: " + newChildIndex.ToString());
                    if (accumulatedFitness >= randomSlicePosition) {
                        newAgentArray[newChildIndex] = sourcePopulation.masterAgentArray[a]; // add to next gen's list of agents
                        SpeciesBreedingPool survivingAgentBreedingPool = sourcePopulation.GetBreedingPoolByID(childSpeciesPoolsList, newAgentArray[newChildIndex].speciesID);
                        survivingAgentBreedingPool.AddNewAgent(newAgentArray[newChildIndex]);
                        //SortNewAgentIntoSpecies(newAgentArray[newChildIndex], childSpeciesList); // sorts this surviving agent into next generation's species'
                        newChildIndex++;
                        break;
                    }
                }
            }
        }

        // Figure out how many new agents must be created to fill up the new population:
        int numNewChildAgents = sourcePopulation.populationMaxSize - numSurvivors;
        int numEligibleBreederAgents = Mathf.RoundToInt(breedingRate * (float)sourcePopulation.populationMaxSize);
        int currentRankIndex = 0;

        // Once the agents are ranked, trim the BreedingPools of agents that didn't make the cut for mating:
        if(useSpeciation) {
            for (int s = 0; s < sourcePopulation.speciesBreedingPoolList.Count; s++) {
                int index = 0;
                int failsafe = 0;
                int numAgents = sourcePopulation.speciesBreedingPoolList[s].agentList.Count;
                while (index < numAgents) {
                    if (index < sourcePopulation.speciesBreedingPoolList[s].agentList.Count) {
                        if (sourcePopulation.speciesBreedingPoolList[s].agentList[index].fitnessRank >= numEligibleBreederAgents) {
                            sourcePopulation.speciesBreedingPoolList[s].agentList.RemoveAt(index);
                        }
                        else {
                            index++;
                        }
                    }
                    else {
                        break;
                    }
                    failsafe++;
                    if (failsafe > 500) {
                        Debug.Log("INFINITE LOOP! hit failsafe 500 iters -- Trimming BreedingPools!");
                        break;
                    }
                }
                //Debug.Log("BreedPopulation -- TRIMSpeciesPool# " + s.ToString() + ", id: " + sourcePopulation.speciesBreedingPoolList[s].speciesID.ToString() + ", Count: " + sourcePopulation.speciesBreedingPoolList[s].agentList.Count.ToString());
                //
            }
        }        

        float totalScoreBreeders = 0f;
        if (breedingByRaffle) {  // calculate total fitness scores to determine lottery weights
            for (int a = 0; a < numEligibleBreederAgents; a++) { // iterate through all agents
                totalScoreBreeders += sourcePopulation.masterAgentArray[a].fitnessScoreSpecies;
            }
        }

        // Iterate over numAgentsToCreate :
        int newChildrenCreated = 0;
        while (newChildrenCreated < numNewChildAgents) {
            //		Find how many parents random number btw min/max:
            int numParentAgents = 2; // UnityEngine.Random.Range(minNumParents, maxNumParents);
            int numChildAgents = 1; // defaults to one child, but:
            if (numNewChildAgents - newChildrenCreated >= 2) {  // room for two more!
                numChildAgents = 2;                
            }

            Agent[] parentAgentsArray = new Agent[numParentAgents]; // assume 2 for now? yes, so far....
            
            List<GeneNodeNEAT>[] parentNodeListArray = new List<GeneNodeNEAT>[numParentAgents];
            List<GeneLinkNEAT>[] parentLinkListArray = new List<GeneLinkNEAT>[numParentAgents];

            Agent firstParentAgent = SelectAgentFromPopForBreeding(sourcePopulation, numEligibleBreederAgents, ref currentRankIndex);
            parentAgentsArray[0] = firstParentAgent;
            List<GeneNodeNEAT> firstParentNodeList = new List<GeneNodeNEAT>();
            List<GeneLinkNEAT> firstParentLinkList = new List<GeneLinkNEAT>();
            firstParentNodeList = firstParentAgent.brainGenome.nodeNEATList;
            firstParentLinkList = firstParentAgent.brainGenome.linkNEATList;
            parentNodeListArray[0] = firstParentNodeList;
            parentLinkListArray[0] = firstParentLinkList;

            Agent secondParentAgent;
            SpeciesBreedingPool parentAgentBreedingPool = sourcePopulation.GetBreedingPoolByID(sourcePopulation.speciesBreedingPoolList, firstParentAgent.speciesID);
            if (useSpeciation) {
                //parentAgentBreedingPool
                float randBreedOutsideSpecies = UnityEngine.Random.Range(0f, 1f);
                if (randBreedOutsideSpecies < interspeciesBreedingRate) { // Attempts to Found a new species
                                                                          // allowed to breed outside its own species:
                    secondParentAgent = SelectAgentFromPopForBreeding(sourcePopulation, numEligibleBreederAgents, ref currentRankIndex);
                }
                else {
                    // Selects mate only from within its own species:
                    secondParentAgent = SelectAgentFromPoolForBreeding(parentAgentBreedingPool);
                }
            }
            else {
                secondParentAgent = SelectAgentFromPopForBreeding(sourcePopulation, numEligibleBreederAgents, ref currentRankIndex);
            }           
            
            parentAgentsArray[1] = secondParentAgent;
            List<GeneNodeNEAT> secondParentNodeList = new List<GeneNodeNEAT>();
            List<GeneLinkNEAT> secondParentLinkList = new List<GeneLinkNEAT>();
            secondParentNodeList = secondParentAgent.brainGenome.nodeNEATList;
            secondParentLinkList = secondParentAgent.brainGenome.linkNEATList;
            parentNodeListArray[1] = secondParentNodeList;
            parentLinkListArray[1] = secondParentLinkList;

            //Debug.Log("NewParentPair--Species: " + firstParentAgent.speciesID.ToString() + ", parent1: " + firstParentAgent.fitnessRank.ToString() + ", parent2: " + secondParentAgent.fitnessRank.ToString());
                        
            //		Iterate over ChildArray.Length :  // how many newAgents created
            for (int c = 0; c < numChildAgents; c++) { // for number of child Agents in floatArray[][]:
                Agent newChildAgent = new Agent();
                
                List<GeneNodeNEAT> childNodeList = new List<GeneNodeNEAT>();
                List<GeneLinkNEAT> childLinkList = new List<GeneLinkNEAT>();
                //Debug.Log("newChildIndex: " + newChildIndex.ToString() + " parentAgentsArray[0].brainGenome.ReadMaxInno(): " + parentAgentsArray[0].brainGenome.ReadMaxInno().ToString());

                GenomeNEAT childGenome = new GenomeNEAT();
                childGenome.nodeNEATList = childNodeList;
                childGenome.linkNEATList = childLinkList;

                int numEnabledLinkGenes = 0;

                if (useCrossover) {
                    //float dist = GenomeNEAT.MeasureGeneticDistance(parentAgentsArray[0].brainGenome, parentAgentsArray[1].brainGenome);
                    //Debug.Log("Parent Distance: " + dist.ToString());

                    int nextLinkInnoA = 0;
                    int nextLinkInnoB = 0;

                    int failsafeMax = 50; // parentAgentsArray[0].brainGenome.ReadMaxInno();
                    int failsafe = 0;
                    //int currentInno = 0;
                    int parentListIndexA = 0;
                    int parentListIndexB = 0;
                    bool parentDoneA = false;
                    bool parentDoneB = false;
                    bool endReached = false;

                    int moreFitParent = 0;  // which parent is more Fit
                    //float fitSliceA = 0.01f * parentAgentsArray[0].fitnessScoreSpecies / (parentAgentsArray[0].fitnessScoreSpecies + parentAgentsArray[1].fitnessScoreSpecies);
                    //float fitSliceB = 0.01f * parentAgentsArray[1].fitnessScoreSpecies / (parentAgentsArray[0].fitnessScoreSpecies + parentAgentsArray[1].fitnessScoreSpecies);
                    if (parentAgentsArray[0].fitnessScoreSpecies < parentAgentsArray[1].fitnessScoreSpecies) {
                        moreFitParent = 1;
                    }
                    else if (parentAgentsArray[0].fitnessScoreSpecies == parentAgentsArray[1].fitnessScoreSpecies) {
                        moreFitParent = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
                    }

                    if (moreFitParent < 0.5f)
                        newChildAgent.bodyGenome = parentAgentsArray[0].bodyGenome;
                    else
                        newChildAgent.bodyGenome = parentAgentsArray[1].bodyGenome;

                    while (!endReached) {
                    //for(int i = 0; i < parentAgentsArray[0].brainGenome.ReadMaxInno(); i++) { 
                        failsafe++;
                        if(failsafe > failsafeMax) {
                            Debug.Log("failsafe reached!");
                            break;
                        }
                        // inno# of next links:
                        if(parentLinkListArray[0].Count > parentListIndexA) {
                            nextLinkInnoA = parentLinkListArray[0][parentListIndexA].innov;
                        }
                        else {
                            parentDoneA = true;
                        }
                        if (parentLinkListArray[1].Count > parentListIndexB) {
                            nextLinkInnoB = parentLinkListArray[1][parentListIndexB].innov;
                        }
                        else {
                            parentDoneB = true;
                        }

                        int innoDelta = nextLinkInnoA - nextLinkInnoB;  // 0=match, neg= Aextra, pos= Bextra
                        if (parentDoneA && !parentDoneB) {
                            innoDelta = 1;
                        }
                        if (parentDoneB && !parentDoneA) {
                            innoDelta = -1;
                        }
                        if (parentDoneA && parentDoneB) {  // reached end of both parent's linkLists
                            endReached = true;
                            break;
                        }


                        if (innoDelta < 0) {  // Parent A has an earlier link mutation
                            //Debug.Log("newChildIndex: " + newChildIndex.ToString() + ", IndexA: " + parentListIndexA.ToString() + ", IndexB: " + parentListIndexB.ToString() + ", innoDelta < 0 (" + innoDelta.ToString() + ") --  moreFitP: " + moreFitParent.ToString() + ", nextLinkInnoA: " + nextLinkInnoA.ToString() + ", nextLinkInnoB: " + nextLinkInnoB.ToString());
                            if (moreFitParent == 0) {  // Parent A is more fit:
                                GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, parentLinkListArray[0][parentListIndexA].weight, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov, currentGeneration);
                                childLinkList.Add(newChildLink);
                                if (parentLinkListArray[0][parentListIndexA].enabled)
                                    numEnabledLinkGenes++;
                            }
                            else {
                                if(CheckForMutation(crossoverRandomLinkChance)) {  // was less fit parent, but still passed on a gene!:
                                    GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, parentLinkListArray[0][parentListIndexA].weight, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov, currentGeneration);
                                    childLinkList.Add(newChildLink);
                                }
                            }
                            parentListIndexA++;
                        }
                        if (innoDelta > 0) {  // Parent B has an earlier link mutation
                            //Debug.Log("newChildIndex: " + newChildIndex.ToString() + ", IndexA: " + parentListIndexA.ToString() + ", IndexB: " + parentListIndexB.ToString() + ", innoDelta > 0 (" + innoDelta.ToString() + ") --  moreFitP: " + moreFitParent.ToString() + ", nextLinkInnoA: " + nextLinkInnoA.ToString() + ", nextLinkInnoB: " + nextLinkInnoB.ToString());
                            if (moreFitParent == 1) {  // Parent B is more fit:
                                GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[1][parentListIndexB].fromNodeID, parentLinkListArray[1][parentListIndexB].toNodeID, parentLinkListArray[1][parentListIndexB].weight, parentLinkListArray[1][parentListIndexB].enabled, parentLinkListArray[1][parentListIndexB].innov, currentGeneration);
                                childLinkList.Add(newChildLink);
                                if (parentLinkListArray[1][parentListIndexB].enabled)
                                    numEnabledLinkGenes++;
                            }
                            else {
                                if (CheckForMutation(crossoverRandomLinkChance)) {  // was less fit parent, but still passed on a gene!:
                                    GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[1][parentListIndexB].fromNodeID, parentLinkListArray[1][parentListIndexB].toNodeID, parentLinkListArray[1][parentListIndexB].weight, parentLinkListArray[1][parentListIndexB].enabled, parentLinkListArray[1][parentListIndexB].innov, currentGeneration);
                                    childLinkList.Add(newChildLink);
                                }
                            }
                            parentListIndexB++;
                        }
                        if (innoDelta == 0) {  // Match!
                            float randParentIndex = UnityEngine.Random.Range(0f, 1f);
                            float newWeightValue;
                            if (randParentIndex < 0.5) {
                                // ParentA wins:
                                newWeightValue = parentLinkListArray[0][parentListIndexA].weight;
                            }
                            else {  // ParentB wins:
                                newWeightValue = parentLinkListArray[1][parentListIndexB].weight;
                            }
                            //Debug.Log("newChildIndex: " + newChildIndex.ToString() + ", IndexA: " + parentListIndexA.ToString() + ", IndexB: " + parentListIndexB.ToString() + ", innoDelta == 0 (" + innoDelta.ToString() + ") --  moreFitP: " + moreFitParent.ToString() + ", nextLinkInnoA: " + nextLinkInnoA.ToString() + ", nextLinkInnoB: " + nextLinkInnoB.ToString() + ", randParent: " + randParentIndex.ToString() + ", weight: " + newWeightValue.ToString());
                            GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, newWeightValue, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov, currentGeneration);
                            childLinkList.Add(newChildLink);
                            if (parentLinkListArray[0][parentListIndexA].enabled)
                                numEnabledLinkGenes++;

                            parentListIndexA++;
                            parentListIndexB++;
                        }

                    }
                    // once childLinkList is built -- use nodes of the moreFit parent:
                    for (int i = 0; i < parentNodeListArray[moreFitParent].Count; i++) { 
                        // iterate through all nodes in the parent List and copy them into fresh new geneNodes:
                        GeneNodeNEAT clonedNode = new GeneNodeNEAT(parentNodeListArray[moreFitParent][i].id, parentNodeListArray[moreFitParent][i].nodeType, parentNodeListArray[moreFitParent][i].activationFunction);
                        childNodeList.Add(clonedNode);
                    }

                    if (useMutation) {
                        if (numEnabledLinkGenes < 1)
                            numEnabledLinkGenes = 1;
                        for (int k = 0; k < childLinkList.Count; k++) {
                            float mutateChance = masterMutationRate / numEnabledLinkGenes;
                            if (currentGeneration - childLinkList[k].birthGen < newLinkBonusDuration) {
                                float t = 1 - ((currentGeneration - childLinkList[k].birthGen) / (float)newLinkBonusDuration);
                                // t=0 means age of gene is same as bonusDuration, t=1 means it is brand new
                                mutateChance *= Mathf.Lerp(mutateChance, mutateChance * newLinkMutateBonus, t);
                            }
                            if (CheckForMutation(mutateChance)) {  // Weight Mutation!
                                //Debug.Log("Weight Mutation Link[" + k.ToString() + "] weight: " + childLinkList[k].weight.ToString() + ", mutate: " + MutateFloat(childLinkList[k].weight).ToString());
                                childLinkList[k].weight = MutateFloat(childLinkList[k].weight);
                            }
                        }
                        if (CheckForMutation(mutationRemoveLinkChance)) {
                            //Debug.Log("Remove Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            childGenome.RemoveRandomLink();
                        }
                        if (CheckForMutation(mutationAddNodeChance)) {   // Adds a new node
                            //Debug.Log("Add Node Mutation Agent[" + newChildIndex.ToString() + "]");
                            childGenome.AddNewRandomNode(currentGeneration);
                        }
                        if (CheckForMutation(mutationAddLinkChance)) { // Adds new connection
                            //Debug.Log("Add Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            if (CheckForMutation(existingNetworkBias)) {
                                childGenome.AddNewExtraLink(existingFromNodeBias, currentGeneration);
                            }
                            else {
                                childGenome.AddNewRandomLink(currentGeneration);
                            }
                        }
                        if (CheckForMutation(mutationActivationFunctionChance)) {
                            TransferFunctions.TransferFunction newFunction;
                            int randIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0f, childNodeList.Count - 1));
                            int randomTF = (int)UnityEngine.Random.Range(0f, 12f);

                            switch (randomTF) {
                                case 0:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                case 1:
                                    newFunction = TransferFunctions.TransferFunction.Linear;
                                    break;
                                case 2:
                                    newFunction = TransferFunctions.TransferFunction.Gaussian;
                                    break;
                                case 3:
                                    newFunction = TransferFunctions.TransferFunction.Abs;
                                    break;
                                case 4:
                                    newFunction = TransferFunctions.TransferFunction.Cos;
                                    break;
                                case 5:
                                    newFunction = TransferFunctions.TransferFunction.Sin;
                                    break;
                                case 6:
                                    newFunction = TransferFunctions.TransferFunction.Tan;
                                    break;
                                case 7:
                                    newFunction = TransferFunctions.TransferFunction.Square;
                                    break;
                                case 8:
                                    newFunction = TransferFunctions.TransferFunction.Threshold01;
                                    break;
                                case 9:
                                    newFunction = TransferFunctions.TransferFunction.ThresholdNegPos;
                                    break;
                                case 10:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                case 11:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                case 12:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                default:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                            }
                            if (childNodeList[randIndex].nodeType != GeneNodeNEAT.GeneNodeType.Out) {  // keep outputs -1 to 1 range
                                Debug.Log("ActivationFunction Mutation Node[" + randIndex.ToString() + "] prev: " + childNodeList[randIndex].activationFunction.ToString() + ", new: " + newFunction.ToString());
                                childNodeList[randIndex].activationFunction = newFunction;
                            }
                        }
                    }
                    else {
                        Debug.Log("Mutation Disabled!");
                    }
                }
                else { // no crossover:                    
                    newChildAgent.bodyGenome = parentAgentsArray[0].bodyGenome;

                    for (int i = 0; i < parentNodeListArray[0].Count; i++) {
                        // iterate through all nodes in the parent List and copy them into fresh new geneNodes:
                        GeneNodeNEAT clonedNode = new GeneNodeNEAT(parentNodeListArray[0][i].id, parentNodeListArray[0][i].nodeType, parentNodeListArray[0][i].activationFunction);
                        childNodeList.Add(clonedNode);
                    }
                    for (int j = 0; j < parentLinkListArray[0].Count; j++) {
                        //same thing with connections
                        GeneLinkNEAT clonedLink = new GeneLinkNEAT(parentLinkListArray[0][j].fromNodeID, parentLinkListArray[0][j].toNodeID, parentLinkListArray[0][j].weight, parentLinkListArray[0][j].enabled, parentLinkListArray[0][j].innov, currentGeneration);
                        childLinkList.Add(clonedLink);
                        if (parentLinkListArray[0][j].enabled)
                            numEnabledLinkGenes++;
                    }
                    // MUTATION:
                    if (useMutation) {
                        if (numEnabledLinkGenes < 1)
                            numEnabledLinkGenes = 1;
                        for (int k = 0; k < childLinkList.Count; k++) {
                            float mutateChance = masterMutationRate / numEnabledLinkGenes;
                            if (currentGeneration - childLinkList[k].birthGen < newLinkBonusDuration) {
                                float t = 1 - ((currentGeneration - childLinkList[k].birthGen) / (float)newLinkBonusDuration);
                                // t=0 means age of gene is same as bonusDuration, t=1 means it is brand new
                                mutateChance *= Mathf.Lerp(mutateChance, mutateChance * newLinkMutateBonus, t);
                            }
                            if (CheckForMutation(mutateChance)) {  // Weight Mutation!
                                //Debug.Log("Weight Mutation Link[" + k.ToString() + "] weight: " + childLinkList[k].weight.ToString() + ", mutate: " + MutateFloat(childLinkList[k].weight).ToString());
                                childLinkList[k].weight = MutateFloat(childLinkList[k].weight);
                            }
                        }
                        if (CheckForMutation(mutationRemoveLinkChance)) {
                            //Debug.Log("Remove Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            childGenome.RemoveRandomLink();
                        }
                        if (CheckForMutation(mutationAddNodeChance)) {   // Adds a new node
                            //Debug.Log("Add Node Mutation Agent[" + newChildIndex.ToString() + "]");
                            childGenome.AddNewRandomNode(currentGeneration);
                        }
                        if (CheckForMutation(mutationAddLinkChance)) { // Adds new connection
                            //Debug.Log("Add Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            if(CheckForMutation(existingNetworkBias)) {
                                childGenome.AddNewExtraLink(existingFromNodeBias, currentGeneration);
                            }
                            else {
                                childGenome.AddNewRandomLink(currentGeneration);
                            }
                        }
                        if (CheckForMutation(mutationActivationFunctionChance)) {
                            TransferFunctions.TransferFunction newFunction;
                            int randIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0f, childNodeList.Count - 1));
                            int randomTF = (int)UnityEngine.Random.Range(0f, 12f);

                            switch (randomTF) {
                                case 0:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                case 1:
                                    newFunction = TransferFunctions.TransferFunction.Linear;
                                    break;
                                case 2:
                                    newFunction = TransferFunctions.TransferFunction.Gaussian;
                                    break;
                                case 3:
                                    newFunction = TransferFunctions.TransferFunction.Abs;
                                    break;
                                case 4:
                                    newFunction = TransferFunctions.TransferFunction.Cos;
                                    break;
                                case 5:
                                    newFunction = TransferFunctions.TransferFunction.Sin;
                                    break;
                                case 6:
                                    newFunction = TransferFunctions.TransferFunction.Tan;
                                    break;
                                case 7:
                                    newFunction = TransferFunctions.TransferFunction.Square;
                                    break;
                                case 8:
                                    newFunction = TransferFunctions.TransferFunction.Threshold01;
                                    break;
                                case 9:
                                    newFunction = TransferFunctions.TransferFunction.ThresholdNegPos;
                                    break;
                                case 10:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                case 11:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                case 12:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                                default:
                                    newFunction = TransferFunctions.TransferFunction.RationalSigmoid;
                                    break;
                            }
                            if (childNodeList[randIndex].nodeType != GeneNodeNEAT.GeneNodeType.Out) {  // keep outputs -1 to 1 range
                                Debug.Log("ActivationFunction Mutation Node[" + randIndex.ToString() + "] prev: " + childNodeList[randIndex].activationFunction.ToString() + ", new: " + newFunction.ToString());
                                childNodeList[randIndex].activationFunction = newFunction;
                            }
                        }
                        //for (int t = 0; t < childNodeList.Count; t++) {
                            
                        //}
                    }
                    else {
                        Debug.Log("Mutation Disabled!");
                    }
                }
                
                newChildAgent.brainGenome = childGenome;
                newChildAgent.brainGenome.nodeNEATList = childNodeList;
                newChildAgent.brainGenome.linkNEATList = childLinkList;
                BrainNEAT childBrain = new BrainNEAT(childGenome);
                childBrain.BuildBrainNetwork();
                newChildAgent.brain = childBrain;
                
                // Species:
                if(useSpeciation) {
                    float randAdoption = UnityEngine.Random.Range(0f, 1f);
                    if (randAdoption < adoptionRate) { // Attempts to Found a new species
                        bool speciesGenomeMatch = false;
                        for (int s = 0; s < childSpeciesPoolsList.Count; s++) {
                            float geneticDistance = GenomeNEAT.MeasureGeneticDistance(newChildAgent.brainGenome, childSpeciesPoolsList[s].templateGenome, excessLinkWeight, disjointLinkWeight, linkWeightWeight, normalizeExcess, normalizeDisjoint, normalizeLinkWeight);

                            if (geneticDistance < speciesSimilarityThreshold) {
                                speciesGenomeMatch = true;
                                //agent.speciesID = speciesBreedingPoolList[s].speciesID; // this is done inside the AddNewAgent method below v v v 
                                childSpeciesPoolsList[s].AddNewAgent(newChildAgent);
                                //Debug.Log(" NEW CHILD (" + newChildIndex.ToString() + ") SortAgentIntoBreedingPool dist: " + geneticDistance.ToString() + ", speciesIDs: " + newChildAgent.speciesID.ToString() + ", " + childSpeciesPoolsList[s].speciesID.ToString() + ", speciesCount: " + childSpeciesPoolsList[s].agentList.Count.ToString());
                                break;
                            }
                        }
                        if (!speciesGenomeMatch) {

                            SpeciesBreedingPool newSpeciesBreedingPool = new SpeciesBreedingPool(newChildAgent.brainGenome, sourcePopulation.GetNextSpeciesID()); // creates new speciesPool modeled on this agent's genome

                            newSpeciesBreedingPool.AddNewAgent(newChildAgent);  // add this agent to breeding pool
                            childSpeciesPoolsList.Add(newSpeciesBreedingPool);  // add new speciesPool to the population's list of all active species

                            Debug.Log(" NEW CHILD (" + newChildIndex.ToString() + ") SortAgentIntoBreedingPool NO MATCH!!! -- creating new BreedingPool " + newSpeciesBreedingPool.speciesID.ToString() + ", newChildAgentSpeciesID: " + newChildAgent.speciesID.ToString());
                        }
                    }
                    else {  // joins parent species automatically:
                        SpeciesBreedingPool newSpeciesBreedingPool = sourcePopulation.GetBreedingPoolByID(childSpeciesPoolsList, parentAgentBreedingPool.speciesID);
                        newSpeciesBreedingPool.AddNewAgent(newChildAgent);  // add this agent to breeding pool
                                                                            //Debug.Log(" NEW CHILD (" + newChildIndex.ToString() + ") NO ADOPTION SortAgentIntoBreedingPool speciesIDs: " + newChildAgent.speciesID.ToString() + ", " + newSpeciesBreedingPool.speciesID.ToString() + ", speciesCount: " + newSpeciesBreedingPool.agentList.Count.ToString());
                    }
                }
                else {  // joins parent species automatically:
                    SpeciesBreedingPool newSpeciesBreedingPool = sourcePopulation.GetBreedingPoolByID(childSpeciesPoolsList, parentAgentBreedingPool.speciesID);
                    newSpeciesBreedingPool.AddNewAgent(newChildAgent);  // add this agent to breeding pool                                                                        
                }

                newChildAgent.parentFitnessScoreA = sourcePopulation.masterAgentArray[newChildIndex].fitnessScore;
                newAgentArray[newChildIndex] = newChildAgent;

                newChildIndex++;  // new child created!
                newChildrenCreated++;
            }
        }

        /*Debug.Log("Finished Crossover! childSpeciesPoolsList:");
        for (int i = 0; i < sourcePopulation.speciesBreedingPoolList.Count; i++) {
            string poolString = " Child Species ID: " + sourcePopulation.speciesBreedingPoolList[i].speciesID.ToString();
            for (int j = 0; j < sourcePopulation.speciesBreedingPoolList[i].agentList.Count; j++) {
                poolString += ", member# " + j.ToString() + ", species: " + sourcePopulation.speciesBreedingPoolList[i].agentList[j].speciesID.ToString() + ", fitRank: " + sourcePopulation.speciesBreedingPoolList[i].agentList[j].fitnessRank.ToString();
            }
            Debug.Log(poolString);
        }*/

        // Clear out extinct species:
        int listIndex = 0;
        for (int s = 0; s < childSpeciesPoolsList.Count; s++) {
            if (listIndex >= childSpeciesPoolsList.Count) {
                Debug.Log("end childSpeciesPoolsList " + childSpeciesPoolsList.Count.ToString() + ", index= " + listIndex.ToString());
                break;
            }
            else {
                if (childSpeciesPoolsList[listIndex].agentList.Count == 0) {  // if empty:
                    Debug.Log("Species " + childSpeciesPoolsList[listIndex].speciesID.ToString() + " WENT EXTINCT!!! --- childSpeciesPoolsList[" + listIndex.ToString() + "] old Count: " + childSpeciesPoolsList.Count.ToString() + ", s: " + s.ToString());
                    childSpeciesPoolsList.RemoveAt(listIndex);
                    //s--;  // see if this works                    
                }
                else {
                    listIndex++;
                }
            }
        }

        //Debug.Log("Finished Crossover! numChildSpeciesPools: " + childSpeciesPoolsList.Count.ToString() + " species0size: " + childSpeciesPoolsList[0].agentList.Count.ToString());
        sourcePopulation.masterAgentArray = newAgentArray;
        sourcePopulation.speciesBreedingPoolList = childSpeciesPoolsList;

        /*Debug.Log("Finished Crossover! sourcePopulation.speciesBreedingPoolList:");
        for (int i = 0; i < sourcePopulation.speciesBreedingPoolList.Count; i++) {
            string poolString = "New Species ID: " + sourcePopulation.speciesBreedingPoolList[i].speciesID.ToString();
            for (int j = 0; j < sourcePopulation.speciesBreedingPoolList[i].agentList.Count; j++) {
                poolString += ", member# " + j.ToString() + ", species: " + sourcePopulation.speciesBreedingPoolList[i].agentList[j].speciesID.ToString() + ", fitRank: " + sourcePopulation.speciesBreedingPoolList[i].agentList[j].fitnessRank.ToString();
            }
            Debug.Log(poolString);
        }*/

        return sourcePopulation;
    }
}
