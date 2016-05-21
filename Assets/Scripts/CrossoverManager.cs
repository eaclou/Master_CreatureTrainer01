using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossoverManager {

	// SETTINGS:
	public string tempName = "name!";

    public bool useMutation = true;
    public bool useCrossover = true;
    public bool useSpeciation = true;

	public float masterMutationRate = 0.18f;
	public float maximumWeightMagnitude = 3.5f;
	public float mutationDriftScale = 0.5f;
	public float mutationRemoveLinkChance = 0.05f;
	public float mutationAddLinkChance = 0.1f;
	public float mutationAddNodeChance = 0.0f; // temp?
    public float mutationRemoveNodeChance = 0f;
	public float mutationBodyChance = 0.5f;
	public float maxBodyMutationFactor = 1.25f;

	public int numSwapPositions = 1;
	public int numFactions = 1;
	public int minNumParents = 2;
	public int maxNumParents = 2;
	public bool breedWithSimilar = false;

	public float survivalRate = 0.05f;
	public bool survivalByRank = true;
	public bool survivalStochastic = false;
	public bool survivalByRaffle = false;

	public float breedingRate = 0.65f;
	public bool breedingByRank = false;
	public bool breedingStochastic = false;
	public bool breedingByRaffle = true;

    public float largeBrainPenalty = 0.05f;
    public float adoptionRate = 0.05f;
    public float interspeciesBreedingRate = 0.01f;
    public float speciesSimilarityThreshold = 1.5f;
    public float largeSpeciesPenalty = 0.04f;

    //empty constructor
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
		mutationAddNodeChance = sourceManager.mutationAddNodeChance;

        largeBrainPenalty = sourceManager.largeBrainPenalty;
        speciesSimilarityThreshold = sourceManager.speciesSimilarityThreshold;
        adoptionRate = sourceManager.adoptionRate;
        largeSpeciesPenalty = sourceManager.largeSpeciesPenalty;
        interspeciesBreedingRate = sourceManager.interspeciesBreedingRate;

		numSwapPositions = sourceManager.numSwapPositions;
		numFactions = sourceManager.numFactions;
		minNumParents = sourceManager.minNumParents;
		maxNumParents = sourceManager.maxNumParents;
		breedWithSimilar = sourceManager.breedWithSimilar;

		survivalRate = sourceManager.survivalRate;
		survivalByRank = sourceManager.survivalByRank;
		survivalStochastic = sourceManager.survivalStochastic;
		survivalByRaffle = sourceManager.survivalByRaffle;

		breedingRate = sourceManager.breedingRate;
		breedingByRank = sourceManager.breedingByRank;
		breedingStochastic = sourceManager.breedingStochastic;
		breedingByRaffle = sourceManager.breedingByRaffle;
	}

	public void PerformCrossover(ref Population sourcePopulation) {
		//Population newPop = sourcePopulation.CopyPopulationSettings();
        BreedPopulation(ref sourcePopulation);

        /*if(numFactions > 1) {

			Population[] sourceFactions = sourcePopulation.SplitPopulation(numFactions);
			Population[] newFactions = new Population[numFactions];
			for(int i = 0; i < numFactions; i++) {
				// Make a Genome array of each faction
				// Then BreedAgentPool on each Array?
				// Then Add those genomes to new Population masterAgentArray?
				//newFactions[i] = sourceFactions[i].CopyPopulationSettings();
				Debug.Log ("FactionSize: " + sourceFactions[i].populationMaxSize.ToString());
				newFactions[i] = BreedPopulation(ref sourceFactions[i]);
			}
			// Add them back together!
			newPop.SetToCombinedPopulations(newFactions);

		}*/
        //else {
        //newPop = BreedPopulation(ref sourcePopulation);
        //}
        //newPop = BreedPopulation(ref sourcePopulation);
        //sourcePopulation = newPop;

        //Debug.Log("sourcePopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x: " + sourcePopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x.ToString());
    }

	public float[][] MixFloatChromosomes(float[][] parentFloatGenes, int numOffspring) {  // takes A number of Genomes and returns new mixed up versions
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

	

    /*public void SortNewAgentIntoSpecies(Agent agent, List<Species> speciesList) {
        bool speciesInList = false;
        for (int s = 0; s < speciesList.Count; s++) {
            if(agent.species.id == speciesList[s].id) {
                speciesInList = true;
                speciesList[s].AddNewMember(agent);
            }
        }
        if(!speciesInList) {
            // create new species:
            Species newSpecies = new Species(agent);
            Debug.Log("SortNewAgentIntoSpecies(Agent agent, List<Species> speciesList) ID: " + newSpecies.id.ToString() + ", members: " + newSpecies.currentMemberCount.ToString());
            speciesList.Add(newSpecies);
        }
    }*/

    /*public SpeciesBreedingPool FindMatchingBreedingPool(List<SpeciesBreedingPool> poolList, Species species) {
        for(int p = 0; p < poolList.Count; p++) {
            if(poolList[p].speciesID == species.id) {
                return poolList[p];
            }
        }
        Debug.Log("FindMatchingBreedPool FAILED! " + species.id.ToString());
        return new SpeciesBreedingPool(species);
    }*/

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

    

    public Population BreedPopulation(ref Population sourcePopulation) {
        
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
        for (int s = 0; s < sourcePopulation.speciesBreedingPoolList.Count; s++) {
            int index = 0;
            int failsafe = 0;
            int numAgents = sourcePopulation.speciesBreedingPoolList[s].agentList.Count;
            while (index < numAgents) {   
                if(index < sourcePopulation.speciesBreedingPoolList[s].agentList.Count) {
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
                if(failsafe > 500) {
                    Debug.Log("INFINITE LOOP! hit failsafe 500 iters -- Trimming BreedingPools!");
                    break;
                }
            }
            //Debug.Log("BreedPopulation -- TRIMSpeciesPool# " + s.ToString() + ", id: " + sourcePopulation.speciesBreedingPoolList[s].speciesID.ToString() + ", Count: " + sourcePopulation.speciesBreedingPoolList[s].agentList.Count.ToString());
            //
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

            SpeciesBreedingPool parentAgentBreedingPool = sourcePopulation.GetBreedingPoolByID(sourcePopulation.speciesBreedingPoolList, firstParentAgent.speciesID);

            //Debug.Log("NewFirstParent--Species: " + firstParentAgent.speciesID.ToString() + ", parentAgentBreedingPoolID: " + parentAgentBreedingPool.speciesID.ToString() + ", Count: " + parentAgentBreedingPool.agentList.Count.ToString() + ", fitRank: " + firstParentAgent.fitnessRank.ToString());

            Agent secondParentAgent;
            float randBreedOutsideSpecies = UnityEngine.Random.Range(0f, 1f);
            if (randBreedOutsideSpecies < interspeciesBreedingRate) { // Attempts to Found a new species
                // allowed to breed outside its own species:
                secondParentAgent = SelectAgentFromPopForBreeding(sourcePopulation, numEligibleBreederAgents, ref currentRankIndex);
            }
            else {
                // Selects mate only from within its own species:
                secondParentAgent = SelectAgentFromPoolForBreeding(parentAgentBreedingPool);
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
                    float fitSliceA = 0.01f * parentAgentsArray[0].fitnessScoreSpecies / (parentAgentsArray[0].fitnessScoreSpecies + parentAgentsArray[1].fitnessScoreSpecies);
                    float fitSliceB = 0.01f * parentAgentsArray[1].fitnessScoreSpecies / (parentAgentsArray[0].fitnessScoreSpecies + parentAgentsArray[1].fitnessScoreSpecies);
                    if (parentAgentsArray[0].fitnessScoreSpecies < parentAgentsArray[1].fitnessScoreSpecies) {
                        moreFitParent = 1;
                    }
                    else if (parentAgentsArray[0].fitnessScoreSpecies == parentAgentsArray[1].fitnessScoreSpecies) {
                        moreFitParent = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
                    }

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
                                GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, parentLinkListArray[0][parentListIndexA].weight, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov);
                                childLinkList.Add(newChildLink);
                            }
                            else {
                                /*if(fitSliceA < UnityEngine.Random.Range(0f, 1f)) {  // was less fit parent, but still passed on a gene!:
                                    GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, parentLinkListArray[0][parentListIndexA].weight, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov);
                                    childLinkList.Add(newChildLink);
                                }*/
                            }
                            parentListIndexA++;
                        }
                        if (innoDelta > 0) {  // Parent B has an earlier link mutation
                            //Debug.Log("newChildIndex: " + newChildIndex.ToString() + ", IndexA: " + parentListIndexA.ToString() + ", IndexB: " + parentListIndexB.ToString() + ", innoDelta > 0 (" + innoDelta.ToString() + ") --  moreFitP: " + moreFitParent.ToString() + ", nextLinkInnoA: " + nextLinkInnoA.ToString() + ", nextLinkInnoB: " + nextLinkInnoB.ToString());
                            if (moreFitParent == 1) {  // Parent B is more fit:
                                GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[1][parentListIndexB].fromNodeID, parentLinkListArray[1][parentListIndexB].toNodeID, parentLinkListArray[1][parentListIndexB].weight, parentLinkListArray[1][parentListIndexB].enabled, parentLinkListArray[1][parentListIndexB].innov);
                                childLinkList.Add(newChildLink);
                            }
                            else {
                                /*if (fitSliceB < UnityEngine.Random.Range(0f, 1f)) {  // was less fit parent, but still passed on a gene!:
                                    GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[1][parentListIndexB].fromNodeID, parentLinkListArray[1][parentListIndexB].toNodeID, parentLinkListArray[1][parentListIndexB].weight, parentLinkListArray[1][parentListIndexB].enabled, parentLinkListArray[1][parentListIndexB].innov);
                                    childLinkList.Add(newChildLink);
                                }*/
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
                            GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, newWeightValue, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov);
                            childLinkList.Add(newChildLink);

                            parentListIndexA++;
                            parentListIndexB++;
                        }

                    }
                    // once childLinkList is built -- use nodes of the moreFit parent:
                    for (int i = 0; i < parentNodeListArray[moreFitParent].Count; i++) { 
                        // iterate through all nodes in the parent List and copy them into fresh new geneNodes:
                        GeneNodeNEAT clonedNode = new GeneNodeNEAT(parentNodeListArray[moreFitParent][i].id, parentNodeListArray[moreFitParent][i].nodeType);
                        childNodeList.Add(clonedNode);
                    }

                    if (useMutation) {
                        for (int k = 0; k < childLinkList.Count; k++) {
                            if (CheckForMutation(masterMutationRate)) {  // Weight Mutation!
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
                            childGenome.AddNewRandomNode();
                        }
                        if (CheckForMutation(mutationAddLinkChance)) { // Adds new connection
                            //Debug.Log("Add Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            childGenome.AddNewRandomLink();
                        }
                    }
                    else {
                        Debug.Log("Mutation Disabled!");
                    }
                }
                else { // no crossover:                    
                    
                    for (int i = 0; i < parentNodeListArray[0].Count; i++) {
                        // iterate through all nodes in the parent List and copy them into fresh new geneNodes:
                        GeneNodeNEAT clonedNode = new GeneNodeNEAT(parentNodeListArray[0][i].id, parentNodeListArray[0][i].nodeType);
                        childNodeList.Add(clonedNode);
                    }
                    for (int j = 0; j < parentLinkListArray[0].Count; j++) {
                        //same thing with connections
                        GeneLinkNEAT clonedLink = new GeneLinkNEAT(parentLinkListArray[0][j].fromNodeID, parentLinkListArray[0][j].toNodeID, parentLinkListArray[0][j].weight, parentLinkListArray[0][j].enabled, parentLinkListArray[0][j].innov);
                        childLinkList.Add(clonedLink);
                    }
                    // MUTATION:
                    if (useMutation) {
                        for (int k = 0; k < childLinkList.Count; k++) {
                            if (CheckForMutation(masterMutationRate)) {  // Weight Mutation!
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
                            childGenome.AddNewRandomNode();
                        }
                        if (CheckForMutation(mutationAddLinkChance)) { // Adds new connection
                            //Debug.Log("Add Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            childGenome.AddNewRandomLink();
                        }
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
                            float geneticDistance = GenomeNEAT.MeasureGeneticDistance(newChildAgent.brainGenome, childSpeciesPoolsList[s].templateGenome);

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

    /*public Population BreedPopulation(ref Population sourcePopulation) {
		for(int m = 0; m < sourcePopulation.masterAgentArray.Length; m++) {
			//sourcePopulation.masterAgentArray[m].brain.genome.PrintBiases("sourcePop " + sourcePopulation.masterAgentArray[m].fitnessScore.ToString() + ", " + m.ToString() + ", ");
			//newPop.masterAgentArray[m].brain.genome.PrintBiases("newPop " + m.ToString() + ", ");
		}
		// rank sourcePop by fitness score // maybe do this as a method of Population class?
		sourcePopulation.RankAgentArray();

		// Create the Population that will hold the next Generation agentArray:
		Population newPopulation = new Population();
		newPopulation = sourcePopulation.CopyPopulationSettings();

		// Calculate total fitness score:
		float totalScore = 0f;
		if(survivalByRaffle) {
			for(int a = 0; a < sourcePopulation.populationMaxSize; a++) { // iterate through all agents
				totalScore += sourcePopulation.masterAgentArray[a].fitnessScore;
			}			
		}

		// Figure out How many Agents survive
		int numSurvivors = Mathf.RoundToInt(survivalRate * (float)newPopulation.populationMaxSize);
		//Depending on method, one at a time, select an Agent to survive until the max Number is reached
		int newChildIndex = 0;

		// For ( num Agents ) {
		for(int i = 0; i < numSurvivors; i++) {
			// If survival is by fitness score ranking:
			if(survivalByRank) {
				// Pop should already be ranked, so just traverse from top (best) to bottom (worst)
				newPopulation.masterAgentArray[newChildIndex] = sourcePopulation.masterAgentArray[newChildIndex];
				newChildIndex++;
			}
			// if survival is completely random, as a control:
			if(survivalStochastic) {
				int randomAgent = UnityEngine.Random.Range (0, numSurvivors-1);
				// Set next newChild slot to a randomly-chosen agent within the survivor faction -- change to full random?
				newPopulation.masterAgentArray[newChildIndex] = sourcePopulation.masterAgentArray[randomAgent];
				newChildIndex++;
			}
			// if survival is based on a fitness lottery:
			if(survivalByRaffle) {  // Try when Fitness is normalized from 0-1
				float randomSlicePosition = UnityEngine.Random.Range(0f, totalScore);
				float accumulatedFitness = 0f;
				for(int a = 0; a < sourcePopulation.populationMaxSize; a++) { // iterate through all agents
					accumulatedFitness += sourcePopulation.masterAgentArray[a].fitnessScore;
					// if accum fitness is on slicePosition, copy this Agent
					Debug.Log ("NumSurvivors: " + numSurvivors.ToString() + ", Surviving Agent " + a.ToString() + ": AccumFitness: " + accumulatedFitness.ToString() + ", RafflePos: " + randomSlicePosition.ToString() + ", TotalScore: " + totalScore.ToString() + ", newChildIndex: " + newChildIndex.ToString());
					if(accumulatedFitness >= randomSlicePosition) {
						newPopulation.masterAgentArray[newChildIndex] = sourcePopulation.masterAgentArray[a];
						newChildIndex++;
					}

				}
			}
		//		set newPop Agent to lucky sourcePop index
		//////////	Agent survivingAgent = sourcePopulation.Select
		// Fill up newPop agentArray with the surviving Agents
		// Keep track of Index, as that will be needed for new agents
		}

		// Figure out how many new agents must be created to fill up the new population:
		int numNewChildAgents = newPopulation.populationMaxSize - numSurvivors;
		int numEligibleBreederAgents = Mathf.RoundToInt(breedingRate * (float)newPopulation.populationMaxSize);
		int currentRankIndex = 0;

		float totalScoreBreeders = 0f;
		if(breedingByRaffle) {  // calculate total fitness scores to determine lottery weights
			for(int a = 0; a < numEligibleBreederAgents; a++) { // iterate through all agents
				totalScoreBreeders += sourcePopulation.masterAgentArray[a].fitnessScore;
			}			
		}

		// Iterate over numAgentsToCreate :
		int newChildrenCreated = 0;
		while(newChildrenCreated < numNewChildAgents) {
		//		Find how many parents random number btw min/max:
			int numParentAgents = UnityEngine.Random.Range (minNumParents, maxNumParents);
			int numChildAgents = 1; // defaults to one child, but:
			if(numNewChildAgents - newChildrenCreated >= 2) {  // room for two more!
				numChildAgents = 2;
				//Debug.Log ("numNewChildAgents: " + numNewChildAgents.ToString() + " - newChildrenCreated: " + newChildrenCreated.ToString() + " = numChildAgents: " + numChildAgents.ToString());
			}
			float[][] parentAgentBiases = new float[numParentAgents][];
			float[][] parentAgentWeights = new float[numParentAgents][];
			// %%%%%%%%%% hacky BodyGenome stuff:  -- to eventually be replaced by dynamic NEAT crossover -- for now only proportions/values:
			//CritterGenome[] parentBodyGenomes = new CritterGenome[numParentAgents]; // will hold referneces to parent Agent's bodyGenomes

			for(int p = 0; p < numParentAgents; p++) {
		//		Iterate over numberOfParents :
		//			Depending on method, select suitable agents' genome.Arrays until the numberOfPArents is reached, collect them in an array of arrays
				// If breeding is by fitness score ranking:
				if(breedingByRank) {
					// Pop should already be ranked, so just traverse from top (best) to bottom (worst) to select parentAgents
					if(currentRankIndex >= numEligibleBreederAgents) { // if current rank index is greater than the num of eligible breeders, then restart the index to 0;
						currentRankIndex = 0;
					}
					//parentAgentChromosomes[p] = new float[sourcePopulation.masterAgentArray[currentRankIndex].genome.genomeBiases.Length];
					parentAgentBiases[p] = sourcePopulation.masterAgentArray[currentRankIndex].genome.genomeBiases;
					parentAgentWeights[p] = sourcePopulation.masterAgentArray[currentRankIndex].genome.genomeWeights;
					//parentBodyGenomes[p] = sourcePopulation.masterAgentArray[currentRankIndex].bodyGenome;
					currentRankIndex++;
				}
				// if survival is completely random, as a control:
				if(breedingStochastic) {
					int randomAgent = UnityEngine.Random.Range (0, numEligibleBreederAgents-1); // check if minus 1 is needed
					// Set next newChild slot to a completely randomly-chosen agent
					parentAgentBiases[p] = sourcePopulation.masterAgentArray[randomAgent].genome.genomeBiases;
					parentAgentWeights[p] = sourcePopulation.masterAgentArray[randomAgent].genome.genomeWeights;
					//parentBodyGenomes[p] = sourcePopulation.masterAgentArray[randomAgent].bodyGenome;
				}
				// if survival is based on a fitness lottery:
				if(breedingByRaffle) {
					float randomSlicePosition = UnityEngine.Random.Range(0f, totalScoreBreeders);
					float accumulatedFitness = 0f;
					for(int a = 0; a < numEligibleBreederAgents; a++) { // iterate through all agents
						accumulatedFitness += sourcePopulation.masterAgentArray[a].fitnessScore;
						// if accum fitness is on slicePosition, copy this Agent
						Debug.Log ("Breeding Agent " + a.ToString() + ": AccumFitness: " + accumulatedFitness.ToString() + ", RafflePos: " + randomSlicePosition.ToString() + ", totalScoreBreeders: " + totalScoreBreeders.ToString() + ", numEligibleBreederAgents: " + numEligibleBreederAgents.ToString());
						if(accumulatedFitness >= randomSlicePosition) {
							parentAgentBiases[p] = sourcePopulation.masterAgentArray[a].genome.genomeBiases;
							parentAgentWeights[p] = sourcePopulation.masterAgentArray[a].genome.genomeWeights;
							//parentBodyGenomes[p] = sourcePopulation.masterAgentArray[a].bodyGenome;
						}
					}
				}
			}
			// Combine the genes in the parentArrays and return the specified number of children genomes
		//		Pass that array of parentAgent genome.Arrays into the float-based MixFloatChromosomes() function,
			float[][] childAgentBiases = MixFloatChromosomes(parentAgentBiases, numChildAgents);
			float[][] childAgentWeights = MixFloatChromosomes(parentAgentWeights, numChildAgents);
            // BODY hack:
            //CritterGenome[] childBodyGenomes = MixBodyChromosomes(parentBodyGenomes, numChildAgents);

		//		It can return an Array of Arrays (of new childAgent genome.Arrays) 
		//		Iterate over ChildArray.Length :  // how many newAgents created
			for(int c = 0; c < numChildAgents; c++) { // for number of child Agents in floatArray[][]:
				for(int b = 0; b < sourcePopulation.masterAgentArray[0].genome.genomeBiases.Length; b++) {
					//Debug.Log ("ChildNumber: " + c.ToString() + ", BiasIndex: " + b.ToString() + ", biasValue: " + childAgentBiases[c][b].ToString () + ", newChildIndex: " + newChildIndex.ToString() + ", numNewChildren: " + numNewChildAgents.ToString() + ", numChildAgents: " + numChildAgents.ToString() + ", newChildrenCreated: " + newChildrenCreated.ToString());
					newPopulation.masterAgentArray[newChildIndex].genome.genomeBiases[b] = childAgentBiases[c][b];
					// weights and functions and more!
				}
				for(int w = 0; w < sourcePopulation.masterAgentArray[0].genome.genomeWeights.Length; w++) {
					//Debug.Log ("ChildNumber: " + c.ToString() + ", BiasIndex: " + b.ToString() + ", biasValue: " + childAgentBiases[c][b].ToString () + ", newChildIndex: " + newChildIndex.ToString() + ", numNewChildren: " + numNewChildAgents.ToString() + ", numChildAgents: " + numChildAgents.ToString() + ", newChildrenCreated: " + newChildrenCreated.ToString());
					newPopulation.masterAgentArray[newChildIndex].genome.genomeWeights[w] = childAgentWeights[c][w];
					// weights and functions and more!
				}
				//newPopulation.masterAgentArray[newChildIndex].bodyGenome = childBodyGenomes[c];

                // IMPORTANT!!!! v v v v v v
				//newPopulation.masterAgentArray[newChildIndex].brain.SetBrainFromGenome(newPopulation.masterAgentArray[newChildIndex].genome);
				newChildIndex++;  // new child created!
				newChildrenCreated++;
			}
			
		}
        //newPop.isFunctional = true;
        //Debug.Log("newPopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x: " + newPopulation.masterAgentArray[0].bodyGenome.creatureBodySegmentGenomeList[0].size.x.ToString());
        return newPopulation;
	}*/



	/*
	public void PerformCrossover(ref Population sourcePopulation) {
		for(int m = 0; m < sourcePopulation.masterAgentArray.Length; m++) {
			//sourcePopulation.masterAgentArray[m].brain.genome.PrintBiases("sourcePop " + sourcePopulation.masterAgentArray[m].fitnessScore.ToString() + ", " + m.ToString() + ", ");
			//newPop.masterAgentArray[m].brain.genome.PrintBiases("newPop " + m.ToString() + ", ");
		}
		// rank sourcePop by fitness score // maybe do this as a method of Population class?
		// So for now, assume it is already ranked
		sourcePopulation.RankAgentArray();

		// create pending newPopulation = sourcePop
		Population newPop = new Population();
		newPop.SetMaxPopulationSize(sourcePopulation.populationMaxSize);
		newPop.brainType = sourcePopulation.brainType;
		newPop.templateBrain = sourcePopulation.templateBrain;
		newPop.numInputNodes = sourcePopulation.numInputNodes;
		newPop.numOutputNodes = sourcePopulation.numOutputNodes;
		newPop.numAgents = sourcePopulation.numAgents;
		newPop.InitializeMasterAgentArray();  // TEMP
		// set newPop maxSize (and a few other settings maybe) equal to sourcePop settings;

		// Depending on Crossover Method, mix and match Agents until the newPop is full of new Agents
		int spliceBias;
		int spliceWeight;
		int biasLength = sourcePopulation.masterAgentArray[0].genome.genomeBiases.Length;
		int weightLength = sourcePopulation.masterAgentArray[0].genome.genomeWeights.Length;
		int numMutations = 0;

		// OLD CROSSOVER CODE::::::::::
		for(int i = 0; i < (sourcePopulation.populationMaxSize / 2); i ++)
		{
			spliceBias = UnityEngine.Random.Range(0, biasLength);
			spliceWeight = UnityEngine.Random.Range(0, weightLength);
			//Debug.Log ("splice location: " + spliceBias + ", " + spliceWeight);
			
			float[] tempBias1 = new float[biasLength];
			float[] tempBias2 = new float[biasLength];
			TransferFunctions.TransferFunction[] tempFunctions1 = new TransferFunctions.TransferFunction[biasLength];
			TransferFunctions.TransferFunction[] tempFunctions2 = new TransferFunctions.TransferFunction[biasLength];
			float[] tempWeight1 = new float[weightLength];
			float[] tempWeight2 = new float[weightLength];


			string biasString1 = "WeightsString1: ";
			for(int g = 0; g < weightLength; g++) {
				biasString1 += sourcePopulation.masterAgentArray[i].brain.genome.genomeWeights[g] + ", ";
			}
			Debug.Log (biasString1);
			string biasString2 = "WeightsString2: ";
			for(int g = 0; g < weightLength; g++) {
				biasString2 += sourcePopulation.masterAgentArray[i+1].brain.genome.genomeWeights[g] + ", ";
			}
			Debug.Log (biasString2);

	
	// BIAS CROSSOVER
	for( int b = 0; b < biasLength; b++) {
		if( b < spliceBias)	{
			float mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempBias1[b] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else {
				tempBias1[b] = sourcePopulation.masterAgentArray[i].genome.genomeBiases[b];
			}
			mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempBias2[b] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else {
				tempBias2[b] = sourcePopulation.masterAgentArray[i+1].genome.genomeBiases[b];
			}
		}
		else {
			float mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempBias1[b] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else {
				tempBias1[b] = sourcePopulation.masterAgentArray[i+1].genome.genomeBiases[b];
			}
			mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempBias2[b] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else {
				tempBias2[b] = sourcePopulation.masterAgentArray[i].genome.genomeBiases[b];
			}
		}
	}
	// WEIGHT CROSSOVER
	for( int w = 0; w < weightLength; w++) {
		if( w < spliceWeight) {
			// Offspring A:
			float mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempWeight1[w] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else {
				tempWeight1[w] = sourcePopulation.masterAgentArray[i].genome.genomeWeights[w];
			}
			// Offspring B:
			mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempWeight2[w] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else {
				tempWeight2[w] = sourcePopulation.masterAgentArray[i+1].genome.genomeWeights[w];
			}
		}
		else {
			// Offspring A:
			float mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempWeight1[w] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else {
				tempWeight1[w] = sourcePopulation.masterAgentArray[i+1].genome.genomeWeights[w];
			}
			// Offspring B:
			mutateRoll = UnityEngine.Random.Range(0f, 1f);
			if(mutateRoll < mutationRate) {
				tempWeight2[w] = Gaussian.GetRandomGaussian();
				numMutations++;
			}
			else
			{
				tempWeight2[w] = sourcePopulation.masterAgentArray[i].genome.genomeWeights[w];
			}
		}
	}			
	//Debug.Log ("numMutations: " + numMutations);			
	//Add to new Genome Array:			
	newPop.masterAgentArray[2*i].genome.genomeBiases = tempBias1;
	//newPop.masterAgentArray[2*i].brain.genome.geneFunctions = tempFunctions1;
	newPop.masterAgentArray[2*i].genome.genomeWeights = tempWeight1;
	newPop.masterAgentArray[2*i].brain.SetBrainFromGenome(newPop.masterAgentArray[2*i].genome);
	//newPop.masterAgentArray[2*i].brain.genome.PrintBiases();
	
	newPop.masterAgentArray[2*i+1].genome.genomeBiases = tempBias2;
	//newPop.masterAgentArray[2*i+1].brain.genome.geneFunctions = tempFunctions2;
	newPop.masterAgentArray[2*i+1].genome.genomeWeights = tempWeight2;
	newPop.masterAgentArray[2*i+1].brain.SetBrainFromGenome(newPop.masterAgentArray[2*i+1].genome);
	//newPop.masterAgentArray[2*i+1].brain.genome.PrintBiases();
	

			string newBiasString1 = "newWeightsString1: ";
			for(int g = 0; g < weightLength; g++) {
				newBiasString1 += newPop.masterAgentArray[2*i].brain.genome.genomeWeights[g] + ", ";
			}
			Debug.Log (newBiasString1);
			string newBiasString2 = "newWeightsString2: ";
			for(int g = 0; g < weightLength; g++) {
				newBiasString2 += newPop.masterAgentArray[2*i+1].brain.genome.genomeWeights[g] + ", ";
			}
			Debug.Log (newBiasString2);

}

//Debug.Log ("NumMutations: " + numMutations.ToString ());

newPop.isFunctional = true;
sourcePopulation = newPop;
// Back the other way!!
//sourcePopulation.masterAgentArray = newPop.masterAgentArray;
//for(int x = 0; x < sourcePopulation.masterAgentArray.Length; x++) {
//
//}

		string rankedAfterString = "RankedAgentArrayAfter: ";
		for(int h = 0; h < sourcePopulation.masterAgentArray.Length; h++) {
			rankedAfterString += "Fit: " + sourcePopulation.masterAgentArray[h].fitnessScore.ToString () + ", " + sourcePopulation.masterAgentArray[h].brain.genome.genomeWeights[0].ToString() + ", ";
		}
		Debug.Log (rankedAfterString);

}
*/

	/*void Crossover()
	{
		int spliceBias;
		int spliceWeight;
		int biasLength = genePool [0].genomeBiases.Length;
		int weightLength = genePool [0].genomeWeights.Length;
		int numMutations = 0;
		
		
		
		Genome[] newPop = new Genome[numAgents];
		
		for(int i = 0; i < (numAgents / 2); i ++)
		{
			spliceBias = rand.Next(0, biasLength);
			spliceWeight = rand.Next(0, weightLength);
			//Debug.Log ("splice location: " + spliceBias + ", " + spliceWeight);
			
			double[] tempBias1 = new double[biasLength];
			double[] tempBias2 = new double[biasLength];
			double[] tempWeight1 = new double[weightLength];
			double[] tempWeight2 = new double[weightLength];
			
			// BIAS CROSSOVER
			for( int b = 0; b < biasLength; b++)
			{
				if( b < spliceBias)
				{
					double mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						tempBias1[b] = (genePool[i].genomeBiases[b] * (1.0 - mutationDriftScale)) + (MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyBias) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempBias1[b] = 0;
						}
						//tempBias2[b] = (genePool[i+1].genomeBiases[b] * (1.0 - mutationDriftScale)) + (Gaussian.GetRandomGaussian() * mutationDriftScale);
					}
					else
					{
						tempBias1[b] = genePool[i].genomeBiases[b];
						//tempBias2[b] = genePool[i+1].genomeBiases[b];
					}
					mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						//tempBias1[b] = (genePool[i].genomeBiases[b] * (1.0 - mutationDriftScale)) + (Gaussian.GetRandomGaussian() * mutationDriftScale);
						tempBias2[b] = (genePool[i+1].genomeBiases[b] * (1.0 - mutationDriftScale)) + (MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyBias) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempBias2[b] = 0;
						}
					}
					else
					{
						//tempBias1[b] = genePool[i].genomeBiases[b];
						tempBias2[b] = genePool[i+1].genomeBiases[b];
					}
				}
				else
				{
					double mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						tempBias1[b] = (genePool[i+1].genomeBiases[b] * (1.0 - mutationDriftScale)) + (MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyBias) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempBias1[b] = 0;
						}
						//tempBias2[b] = (genePool[i].genomeBiases[b] * (1.0 - mutationDriftScale)) + (Gaussian.GetRandomGaussian() * mutationDriftScale);
					}
					else
					{
						tempBias1[b] = genePool[i+1].genomeBiases[b];
						//tempBias2[b] = genePool[i].genomeBiases[b];
					}
					mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						//tempBias1[b] = (genePool[i+1].genomeBiases[b] * (1.0 - mutationDriftScale)) + (Gaussian.GetRandomGaussian() * mutationDriftScale);
						tempBias2[b] = (genePool[i].genomeBiases[b] * (1.0 - mutationDriftScale)) + (MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyBias) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempBias2[b] = 0;
						}
					}
					else
					{
						//tempBias1[b] = genePool[i+1].genomeBiases[b];
						tempBias2[b] = genePool[i].genomeBiases[b];
					}
				}
			}
			// WEIGHT CROSSOVER
			for( int w = 0; w < weightLength; w++)
			{
				
				
				if( w < spliceWeight)
				{
					// Offspring A:
					double mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						// weighted avg of current value and random weight, so it can't move it too far
						tempWeight1[w] = (genePool[i].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
							(MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyWeight) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempWeight1[w] = 0;
						}
						//tempWeight2[w] = (genePool[i+1].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
						//	(Gaussian.GetRandomGaussian() * mutationDriftScale);
					}
					else
					{
						tempWeight1[w] = genePool[i].genomeWeights[w];
						//tempWeight2[w] = genePool[i+1].genomeWeights[w];
					}
					// Offspring B:
					mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						// weighted avg of current value and random weight, so it can't move it too far
						//tempWeight1[w] = (genePool[i].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
						//	(Gaussian.GetRandomGaussian() * mutationDriftScale);
						tempWeight2[w] = (genePool[i+1].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
							(MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyWeight) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempWeight2[w] = 0;
						}
					}
					else
					{
						//tempWeight1[w] = genePool[i].genomeWeights[w];
						tempWeight2[w] = genePool[i+1].genomeWeights[w];
					}
				}
				else
				{
					// Offspring A:
					double mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						// weighted avg of current value and random weight, so it can't move it too far
						tempWeight1[w] = (genePool[i+1].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
							(MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyWeight) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempWeight1[w] = 0;
						}
						//tempWeight2[w] = (genePool[i].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
						//				   (Gaussian.GetRandomGaussian() * mutationDriftScale);
					}
					else
					{
						tempWeight1[w] = genePool[i+1].genomeWeights[w];
						//tempWeight2[w] = genePool[i].genomeWeights[w];
					}
					// Offspring B:
					mutateRoll = rand.NextDouble();
					if(mutateRoll < mutationRate)
					{
						numMutations++;
						// weighted avg of current value and random weight, so it can't move it too far
						//tempWeight1[w] = (genePool[i+1].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
						//	(Gaussian.GetRandomGaussian() * mutationDriftScale);
						tempWeight2[w] = (genePool[i].genomeWeights[w] * (1.0 - mutationDriftScale)) + 
							(MutateZeroBias(Gaussian.GetRandomGaussian()*maxPreyWeight) * mutationDriftScale);
						double zeroRoll = rand.NextDouble();
						if(zeroRoll < mutationZeroBias) {
							tempWeight2[w] = 0;
						}
					}
					else
					{
						//tempWeight1[w] = genePool[i+1].genomeWeights[w];
						tempWeight2[w] = genePool[i].genomeWeights[w];
					}
				}
			}
			
			//Debug.Log ("numMutations: " + numMutations);
			
			//Add to new Genome Array:
			newPop[2*i] = new Genome(layerSizes);
			newPop[2*i+1] = new Genome(layerSizes);
			
			newPop[2*i].genomeBiases = tempBias1;
			newPop[2*i].genomeWeights = tempWeight1;
			newPop[2*i+1].genomeBiases = tempBias2;
			newPop[2*i+1].genomeWeights = tempWeight2;
		}
		
		//set last member of the population to the genome of the current record-holder
		//newPop[numAgents - 1].genomeBiases = recordGenome.genomeBiases;
		//newPop[numAgents - 1].genomeWeights = recordGenome.genomeWeights;
		
		//Update genePool!!!!!!!! FINALLY!
		genePool = newPop;
	}
	*/
}
