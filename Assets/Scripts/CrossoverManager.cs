using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossoverManager {

	// SETTINGS:
	public string tempName = "name!";

    public bool useMutation = true;
    public bool useCrossover = true;
    public bool useSpeciation = false;

    public static int nextNodeInnov;
    public static int nextAddonInnov;

    // Body OLD:
    //public float mutationBodyChance = 0.5f;
    //public float maxBodyMutationFactor = 1.25f;
    // MUTATION:
    public float masterMutationRate = 0.5f;
	public float maximumWeightMagnitude = 5f;
	public float mutationDriftScale = 0.8f;
	public float mutationRemoveLinkChance = 0.1f;
	public float mutationAddLinkChance = 0.24f;	
    public float mutationRemoveNodeChance = 0.0f;
    public float mutationAddNodeChance = 0.08f;
    public float mutationActivationFunctionChance = 0.0f;
    public float largeBrainPenalty = 0.04f;
    public float newLinkMutateBonus = 1.3f;  // 1 = does nothing 
    public int newLinkBonusDuration = 25;
    public float existingNetworkBias = 0.01f;
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
    // BODY!!!:
    public float maxAttributeValueChange = 1.25f;
    public float newSegmentChance = 0f;
    public float removeSegmentChance = 0f;
    public float segmentProportionChance = 0f;
    public float segmentAttachSettingsChance = 0f;
    public float jointSettingsChance = 0f;
    public float newAddonChance = 0f;
    public float removeAddonChance = 0f;
    public float addonSettingsChance = 0f;
    public float recursionChance = 0f;
    public float symmetryChance = 0f;
    // COMMON!!!
    public float survivalRate = 0.03f;   // top-performers are copied into the next generation
	public bool survivalByRank = true;
	public bool survivalStochastic = false;
	public bool survivalByRaffle = false;

	public float breedingRate = 0.6f;   // percentage of population that performed well enough to breed
	public bool breedingByRank = false;
	public bool breedingStochastic = false;
	public bool breedingByRaffle = true;

    public float mutationBlastModifier = 1f;

    //empty constructor for easySave2
    public CrossoverManager() {

	}

    public int GetNextNodeInnov() {
        nextNodeInnov++;
        return nextNodeInnov;
    }
    public int GetNextAddonInnov() {
        return nextAddonInnov++;
    }

    public void CopyFromSourceCrossoverManager(CrossoverManager sourceManager) {

		tempName = sourceManager.tempName;

        useMutation = sourceManager.useMutation;
        useCrossover = sourceManager.useCrossover;
        useSpeciation = sourceManager.useSpeciation;
        //Mutation
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
        //Crossover
        crossoverRandomLinkChance = sourceManager.crossoverRandomLinkChance;
        //Species
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
        // Body
        maxAttributeValueChange = sourceManager.maxAttributeValueChange;
        newSegmentChance = sourceManager.newSegmentChance;
        removeSegmentChance = sourceManager.removeSegmentChance;
        segmentProportionChance = sourceManager.segmentProportionChance;
        segmentAttachSettingsChance = sourceManager.segmentAttachSettingsChance;
        jointSettingsChance = sourceManager.jointSettingsChance;
        newAddonChance = sourceManager.newAddonChance;
        removeAddonChance = sourceManager.removeAddonChance;
        addonSettingsChance = sourceManager.addonSettingsChance;
        recursionChance = sourceManager.recursionChance;
        symmetryChance = sourceManager.symmetryChance;
        // Common
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

        // Fill out Gene History stats:
        if (sourcePopulation.geneHistoryDict == null) {
            sourcePopulation.geneHistoryDict = new Dictionary<int, GeneHistoryInfo>();
        }
        else {
            sourcePopulation.geneHistoryDict.Clear();
        }
        for (int i = 0; i < sourcePopulation.masterAgentArray.Length; i++) {
            
            for(int j = 0; j < sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList.Count; j++) {                
                if(sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList[j].enabled) {
                    int inno = sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList[j].innov;
                    if (sourcePopulation.geneHistoryDict.ContainsKey(inno)) {
                        GeneHistoryInfo info = sourcePopulation.geneHistoryDict[inno];
                        //info.innov = inno; // should have been set when it was created
                        info.numCopies++;
                        info.totalWeight += sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList[j].weight;
                        info.avgWeight = info.totalWeight / (float)info.numCopies;
                    }
                    else {
                        GeneHistoryInfo geneInfo = new GeneHistoryInfo();
                        geneInfo.innov = inno;
                        geneInfo.numCopies++;
                        geneInfo.totalWeight += sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList[j].weight;
                        geneInfo.avgWeight = geneInfo.totalWeight / (float)geneInfo.numCopies;
                        geneInfo.fromNode = sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList[j].fromNodeID;
                        geneInfo.toNode = sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList[j].toNodeID;
                        geneInfo.gen = sourcePopulation.masterAgentArray[i].brainGenome.linkNEATList[j].birthGen;
                        sourcePopulation.geneHistoryDict.Add(inno, geneInfo);
                    }
                }                
            }
        }        
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

    /*public CritterGenome[] MixBodyChromosomes(CritterGenome[] parentGenomes, int numOffspring) {  // takes A number of Genomes and returns new mixed up versions
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
	}*/

    public bool CheckForMutation(float rate) {
        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand < rate) {
            return true;
        }
        return false;
    }

	/*public bool CheckForBodyMutation() {
		float rand = UnityEngine.Random.Range(0f, 1f);
		if(rand < masterMutationRate) { // change to custom body mutation rate at some point
			return true;
		}
		return false;
	}*/

	public float MutateFloat(float sourceFloat) {
		float newFloat;
		
		newFloat = (sourceFloat * (1.0f - mutationDriftScale)) + 
			(Gaussian.GetRandomGaussian()*maximumWeightMagnitude) * mutationDriftScale * mutationBlastModifier;
		
		return newFloat;
	}

	public float MutateBodyFloat(float sourceFloat) {
        float newFloat = 0f;
		if(sourceFloat != 0f) {
			//newFloat = sourceFloat * UnityEngine.Random.Range(1f/maxBodyMutationFactor, maxBodyMutationFactor);
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

    public void BreedCritterAddons(ref CritterGenome childBodyGenome, ref CritterGenome sourceBodyGenomeMoreFit, ref CritterGenome sourceBodyGenomeLessFit) {

        // Iterate over the Addons of the more fit parent:
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonAltimeterList.Count; i++) {
            AddonAltimeter clonedAddon = sourceBodyGenomeMoreFit.addonAltimeterList[i].CloneThisAddon();
            childBodyGenome.addonAltimeterList.Add(clonedAddon);

            if(useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonAltimeterList.Count; a++) {
                    if(sourceBodyGenomeLessFit.addonAltimeterList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonCompassSensor1DList.Count; i++) {
            AddonCompassSensor1D clonedAddon = sourceBodyGenomeMoreFit.addonCompassSensor1DList[i].CloneThisAddon();
            childBodyGenome.addonCompassSensor1DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonCompassSensor1DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonCompassSensor1DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonCompassSensor3DList.Count; i++) {
            AddonCompassSensor3D clonedAddon = sourceBodyGenomeMoreFit.addonCompassSensor3DList[i].CloneThisAddon();
            childBodyGenome.addonCompassSensor3DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonCompassSensor3DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonCompassSensor3DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonContactSensorList.Count; i++) {
            AddonContactSensor clonedAddon = sourceBodyGenomeMoreFit.addonContactSensorList[i].CloneThisAddon();
            childBodyGenome.addonContactSensorList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonContactSensorList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonContactSensorList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonEarBasicList.Count; i++) {
            AddonEarBasic clonedAddon = sourceBodyGenomeMoreFit.addonEarBasicList[i].CloneThisAddon();
            childBodyGenome.addonEarBasicList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonEarBasicList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonEarBasicList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonGravitySensorList.Count; i++) {
            AddonGravitySensor clonedAddon = sourceBodyGenomeMoreFit.addonGravitySensorList[i].CloneThisAddon();
            childBodyGenome.addonGravitySensorList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonGravitySensorList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonGravitySensorList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonJointAngleSensorList.Count; i++) {
            AddonJointAngleSensor clonedAddon = sourceBodyGenomeMoreFit.addonJointAngleSensorList[i].CloneThisAddon();
            childBodyGenome.addonJointAngleSensorList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonJointAngleSensorList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonJointAngleSensorList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonJointMotorList.Count; i++) {
            AddonJointMotor clonedAddon = sourceBodyGenomeMoreFit.addonJointMotorList[i].CloneThisAddon();
            childBodyGenome.addonJointMotorList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonJointMotorList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonJointMotorList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonMouthBasicList.Count; i++) {
            AddonMouthBasic clonedAddon = sourceBodyGenomeMoreFit.addonMouthBasicList[i].CloneThisAddon();
            childBodyGenome.addonMouthBasicList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonMouthBasicList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonMouthBasicList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonNoiseMakerBasicList.Count; i++) {
            AddonNoiseMakerBasic clonedAddon = sourceBodyGenomeMoreFit.addonNoiseMakerBasicList[i].CloneThisAddon();
            childBodyGenome.addonNoiseMakerBasicList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonNoiseMakerBasicList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonNoiseMakerBasicList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonOscillatorInputList.Count; i++) {
            AddonOscillatorInput clonedAddon = sourceBodyGenomeMoreFit.addonOscillatorInputList[i].CloneThisAddon();
            childBodyGenome.addonOscillatorInputList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonOscillatorInputList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonOscillatorInputList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonPhysicalAttributesList.Count; i++) {
            AddonPhysicalAttributes clonedAddon = sourceBodyGenomeMoreFit.addonPhysicalAttributesList[i].CloneThisAddon();
            childBodyGenome.addonPhysicalAttributesList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonPhysicalAttributesList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonPositionSensor1DList.Count; i++) {
            AddonPositionSensor1D clonedAddon = sourceBodyGenomeMoreFit.addonPositionSensor1DList[i].CloneThisAddon();
            childBodyGenome.addonPositionSensor1DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonPositionSensor1DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonPositionSensor1DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonPositionSensor3DList.Count; i++) {
            AddonPositionSensor3D clonedAddon = sourceBodyGenomeMoreFit.addonPositionSensor3DList[i].CloneThisAddon();
            childBodyGenome.addonPositionSensor3DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonPositionSensor3DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonPositionSensor3DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonRaycastSensorList.Count; i++) {
            AddonRaycastSensor clonedAddon = sourceBodyGenomeMoreFit.addonRaycastSensorList[i].CloneThisAddon();
            childBodyGenome.addonRaycastSensorList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonRaycastSensorList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonRaycastSensorList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonRotationSensor1DList.Count; i++) {
            AddonRotationSensor1D clonedAddon = sourceBodyGenomeMoreFit.addonRotationSensor1DList[i].CloneThisAddon();
            childBodyGenome.addonRotationSensor1DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonRotationSensor1DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonRotationSensor1DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonRotationSensor3DList.Count; i++) {
            AddonRotationSensor3D clonedAddon = sourceBodyGenomeMoreFit.addonRotationSensor3DList[i].CloneThisAddon();
            childBodyGenome.addonRotationSensor3DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonRotationSensor3DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonRotationSensor3DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonStickyList.Count; i++) {
            AddonSticky clonedAddon = sourceBodyGenomeMoreFit.addonStickyList[i].CloneThisAddon();
            childBodyGenome.addonStickyList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonStickyList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonStickyList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonThrusterEffector1DList.Count; i++) {
            AddonThrusterEffector1D clonedAddon = sourceBodyGenomeMoreFit.addonThrusterEffector1DList[i].CloneThisAddon();
            childBodyGenome.addonThrusterEffector1DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonThrusterEffector1DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonThrusterEffector1DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonThrusterEffector3DList.Count; i++) {
            AddonThrusterEffector3D clonedAddon = sourceBodyGenomeMoreFit.addonThrusterEffector3DList[i].CloneThisAddon();
            childBodyGenome.addonThrusterEffector3DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonThrusterEffector3DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonThrusterEffector3DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonTimerInputList.Count; i++) {
            AddonTimerInput clonedAddon = sourceBodyGenomeMoreFit.addonTimerInputList[i].CloneThisAddon();
            childBodyGenome.addonTimerInputList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonTimerInputList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonTimerInputList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonTorqueEffector1DList.Count; i++) {
            AddonTorqueEffector1D clonedAddon = sourceBodyGenomeMoreFit.addonTorqueEffector1DList[i].CloneThisAddon();
            childBodyGenome.addonTorqueEffector1DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonTorqueEffector1DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonTorqueEffector1DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonTorqueEffector3DList.Count; i++) {
            AddonTorqueEffector3D clonedAddon = sourceBodyGenomeMoreFit.addonTorqueEffector3DList[i].CloneThisAddon();
            childBodyGenome.addonTorqueEffector3DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonTorqueEffector3DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonTorqueEffector3DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonValueInputList.Count; i++) {
            AddonValueInput clonedAddon = sourceBodyGenomeMoreFit.addonValueInputList[i].CloneThisAddon();
            childBodyGenome.addonValueInputList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonValueInputList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonValueInputList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonVelocitySensor1DList.Count; i++) {
            AddonVelocitySensor1D clonedAddon = sourceBodyGenomeMoreFit.addonVelocitySensor1DList[i].CloneThisAddon();
            childBodyGenome.addonVelocitySensor1DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonVelocitySensor1DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonVelocitySensor1DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonVelocitySensor3DList.Count; i++) {
            AddonVelocitySensor3D clonedAddon = sourceBodyGenomeMoreFit.addonVelocitySensor3DList[i].CloneThisAddon();
            childBodyGenome.addonVelocitySensor3DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonVelocitySensor3DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonVelocitySensor3DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonWeaponBasicList.Count; i++) {
            AddonWeaponBasic clonedAddon = sourceBodyGenomeMoreFit.addonWeaponBasicList[i].CloneThisAddon();
            childBodyGenome.addonWeaponBasicList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonWeaponBasicList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonWeaponBasicList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                    }
                }
            }
        }
    }

    public void PerformBodyMutation(ref CritterGenome bodyGenome) {
        int numBodyMutations = 0;
        for (int i = 0; i < bodyGenome.CritterNodeList.Count; i++) {
            // Segment Proportions:
            if(CheckForMutation(segmentProportionChance)) {  // X
                bodyGenome.CritterNodeList[i].dimensions.x *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                numBodyMutations++;
            }
            if (CheckForMutation(segmentProportionChance)) {  // Y
                bodyGenome.CritterNodeList[i].dimensions.y *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                numBodyMutations++;
            }
            if (CheckForMutation(segmentProportionChance)) {  // Z
                bodyGenome.CritterNodeList[i].dimensions.z *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                numBodyMutations++;
            }
            if(i > 0) {   // NOT THE ROOT SEGMENT!:
                // Segment Attach Settings:
                if (CheckForMutation(segmentAttachSettingsChance)) {  // Position X
                    bodyGenome.CritterNodeList[i].jointLink.attachDir.x *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                    numBodyMutations++;
                }
                if (CheckForMutation(segmentAttachSettingsChance)) {  // Position Y
                    bodyGenome.CritterNodeList[i].jointLink.attachDir.y *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                    numBodyMutations++;
                }
                if (CheckForMutation(segmentAttachSettingsChance)) {  // Position Z
                    bodyGenome.CritterNodeList[i].jointLink.attachDir.z *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                    numBodyMutations++;
                }
                if (CheckForMutation(segmentAttachSettingsChance)) {  // Orientation X
                    bodyGenome.CritterNodeList[i].jointLink.restAngleDir.x *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                    numBodyMutations++;
                }
                if (CheckForMutation(segmentAttachSettingsChance)) {  // Orientation Y
                    bodyGenome.CritterNodeList[i].jointLink.restAngleDir.y *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                    numBodyMutations++;
                }
                if (CheckForMutation(segmentAttachSettingsChance)) {  // Orientation Z
                    bodyGenome.CritterNodeList[i].jointLink.restAngleDir.z *= UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
                    numBodyMutations++;
                }

                // RECURSION:
                if (CheckForMutation(recursionChance)) {
                    int addRemove = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f)) * 2 - 1;  // either -1 or 1
                    bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions += addRemove;
                    if (bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions < 0)
                        bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions = 0;
                    if (bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions > 8)
                        bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions = 8;
                    numBodyMutations++;
                    // NEED TO FIX BRAIN!!! -- how to preserve existing wiring while adding new neurons that may re-order?
                    // Do I need to save a reference to segment/nodes within each input/output neuron?
                }
            }            
        }
        //Debug.Log("NumBodyMutations: " + numBodyMutations.ToString());
    }

    public Population BreedPopulation(ref Population sourcePopulation, int currentGeneration) {

        #region Pre-Crossover, Figuring out how many agents to breed etc.
        int LifetimeGeneration = currentGeneration + sourcePopulation.trainingGenerations;
        int totalNumWeightMutations = 0;
        //float totalWeightChangeValue = 0f;
        // go through species list and adjust fitness
        List<SpeciesBreedingPool> childSpeciesPoolsList = new List<SpeciesBreedingPool>(); // will hold agents in an internal list to facilitate crossover
        
        for (int s = 0; s < sourcePopulation.speciesBreedingPoolList.Count; s++) {            
            SpeciesBreedingPool newChildSpeciesPool = new SpeciesBreedingPool(sourcePopulation.speciesBreedingPoolList[s].templateGenome, sourcePopulation.speciesBreedingPoolList[s].speciesID);  // create Breeding Pools
            // copies the existing breeding pools but leaves their agentLists empty for future children
            childSpeciesPoolsList.Add(newChildSpeciesPool);            // Add to list of pools          
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
        #endregion

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
            List<GeneNodeNEAT> firstParentNodeList = firstParentAgent.brainGenome.nodeNEATList;
            List<GeneLinkNEAT> firstParentLinkList = firstParentAgent.brainGenome.linkNEATList;
            //List<GeneNodeNEAT> firstParentNodeList = new List<GeneNodeNEAT>();
            //List<GeneLinkNEAT> firstParentLinkList = new List<GeneLinkNEAT>();
            //firstParentNodeList = firstParentAgent.brainGenome.nodeNEATList;
            //firstParentLinkList = firstParentAgent.brainGenome.linkNEATList;
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
            List<GeneNodeNEAT> secondParentNodeList = secondParentAgent.brainGenome.nodeNEATList;
            List<GeneLinkNEAT> secondParentLinkList = secondParentAgent.brainGenome.linkNEATList;
            //List<GeneNodeNEAT> secondParentNodeList = new List<GeneNodeNEAT>();
            //List<GeneLinkNEAT> secondParentLinkList = new List<GeneLinkNEAT>();
            //secondParentNodeList = secondParentAgent.brainGenome.nodeNEATList;
            //secondParentLinkList = secondParentAgent.brainGenome.linkNEATList;
            parentNodeListArray[1] = secondParentNodeList;
            parentLinkListArray[1] = secondParentLinkList;
           
            //		Iterate over ChildArray.Length :  // how many newAgents created
            for (int c = 0; c < numChildAgents; c++) { // for number of child Agents in floatArray[][]:
                Agent newChildAgent = new Agent();
                
                List<GeneNodeNEAT> childNodeList = new List<GeneNodeNEAT>();
                List<GeneLinkNEAT> childLinkList = new List<GeneLinkNEAT>();
                
                GenomeNEAT childBrainGenome = new GenomeNEAT();
                childBrainGenome.nodeNEATList = childNodeList;
                childBrainGenome.linkNEATList = childLinkList;

                int numEnabledLinkGenes = 0;

                if (useCrossover) {                    
                    int nextLinkInnoA = 0;
                    int nextLinkInnoB = 0;
                    //int nextBodyNodeInno = 0;
                    //int nextBodyAddonInno = 0;

                    int failsafeMax = 500;
                    int failsafe = 0;
                    int parentListIndexA = 0;
                    int parentListIndexB = 0;
                    //int parentBodyNodeIndex = 0;
                    bool parentDoneA = false;
                    bool parentDoneB = false;
                    bool endReached = false;

                    int moreFitParent = 0;  // which parent is more Fit
                    if (parentAgentsArray[0].fitnessScoreSpecies < parentAgentsArray[1].fitnessScoreSpecies) {
                        moreFitParent = 1;
                    }
                    else if (parentAgentsArray[0].fitnessScoreSpecies == parentAgentsArray[1].fitnessScoreSpecies) {
                        moreFitParent = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
                    }

                    CritterGenome childBodyGenome = new CritterGenome();  // create new body genome for Child
                    // This creates the ROOT NODE!!!!
                    // Clone Nodes & Addons from more fit parent to create new child body genome
                    // crossover is on, so check for matching Nodes and Add-ons (based on Inno#'s) to determine when to mix Settings/Attributes:                    
                    // Iterate over the nodes of the more fit parent:
                    for (int i = 0; i < parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList.Count; i++) {
                        int currentNodeInno = parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList[i].innov;
                        if(i == 0) {  // if this is the ROOT NODE:
                            childBodyGenome.CritterNodeList[0].CopySettingsFromNode(parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList[0]);
                            // The root node was already created during the Constructor method of the CritterGenome,
                            // ... so instead of creating a new one, just copy the settings
                        }
                        else {  // NOT the root node, proceed normally:
                                // Create new cloned node defaulted to the settings of the source( more-fit parent's) Node:
                            CritterNode clonedCritterNode = parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList[i].CloneThisCritterNode();
                            childBodyGenome.CritterNodeList.Add(clonedCritterNode);
                        }

                        // Check other parent for same node:
                        for (int j = 0; j < parentAgentsArray[1 - moreFitParent].bodyGenome.CritterNodeList.Count; j++) {
                            if (parentAgentsArray[1 - moreFitParent].bodyGenome.CritterNodeList[j].innov == currentNodeInno) {
                                // CROSSOVER NODE SETTINGS HERE!!!  ---- If random dice roll > 0.5, use less fit parent's settings, otherwise leave as default
                                // Node dimensions
                                // Joint Settings - placement, orientation
                                // Recursion
                                // Symmetry
                                // Scale Factors, forward Bias, Attach only to Last, etc.
                            }
                        }
                    }
                    // ADD-ONS!!!!!!!!!!!!!!!!!!!!!!
                    BreedCritterAddons(ref childBodyGenome, ref parentAgentsArray[moreFitParent].bodyGenome, ref parentAgentsArray[1 - moreFitParent].bodyGenome);
                    newChildAgent.bodyGenome = childBodyGenome;  // ?????
                    // ##################  ^ ^ ^  this method will clone the Add-Ons of the more-fit parent, then, if the less-fit parent ALSO has that add-on,
                    // ... its settings will be mixed up between the two parents ( IF Crossover=true )

                    //  MATCH UP Links between both agents, if they have a gene with matching Inno#, then mixing can occur                    
                    while (!endReached) {
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
                                GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, parentLinkListArray[0][parentListIndexA].weight, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov, parentLinkListArray[0][parentListIndexA].birthGen);
                                childLinkList.Add(newChildLink);
                                if (parentLinkListArray[0][parentListIndexA].enabled)
                                    numEnabledLinkGenes++;
                            }
                            else {
                                if(CheckForMutation(crossoverRandomLinkChance)) {  // was less fit parent, but still passed on a gene!:
                                    GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, parentLinkListArray[0][parentListIndexA].weight, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov, parentLinkListArray[0][parentListIndexA].birthGen);
                                    childLinkList.Add(newChildLink);
                                }
                            }
                            parentListIndexA++;
                        }
                        if (innoDelta > 0) {  // Parent B has an earlier link mutation
                            //Debug.Log("newChildIndex: " + newChildIndex.ToString() + ", IndexA: " + parentListIndexA.ToString() + ", IndexB: " + parentListIndexB.ToString() + ", innoDelta > 0 (" + innoDelta.ToString() + ") --  moreFitP: " + moreFitParent.ToString() + ", nextLinkInnoA: " + nextLinkInnoA.ToString() + ", nextLinkInnoB: " + nextLinkInnoB.ToString());
                            if (moreFitParent == 1) {  // Parent B is more fit:
                                GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[1][parentListIndexB].fromNodeID, parentLinkListArray[1][parentListIndexB].toNodeID, parentLinkListArray[1][parentListIndexB].weight, parentLinkListArray[1][parentListIndexB].enabled, parentLinkListArray[1][parentListIndexB].innov, parentLinkListArray[1][parentListIndexB].birthGen);
                                childLinkList.Add(newChildLink);
                                if (parentLinkListArray[1][parentListIndexB].enabled)
                                    numEnabledLinkGenes++;
                            }
                            else {
                                if (CheckForMutation(crossoverRandomLinkChance)) {  // was less fit parent, but still passed on a gene!:
                                    GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[1][parentListIndexB].fromNodeID, parentLinkListArray[1][parentListIndexB].toNodeID, parentLinkListArray[1][parentListIndexB].weight, parentLinkListArray[1][parentListIndexB].enabled, parentLinkListArray[1][parentListIndexB].innov, parentLinkListArray[1][parentListIndexB].birthGen);
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
                            GeneLinkNEAT newChildLink = new GeneLinkNEAT(parentLinkListArray[0][parentListIndexA].fromNodeID, parentLinkListArray[0][parentListIndexA].toNodeID, newWeightValue, parentLinkListArray[0][parentListIndexA].enabled, parentLinkListArray[0][parentListIndexA].innov, parentLinkListArray[0][parentListIndexA].birthGen);
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
                        // BODY MUTATION:
                        PerformBodyMutation(ref childBodyGenome);
                        // NEED TO ADJUST BRAINS IF MUTATION CHANGES #NODES!!!!

                        // BRAIN MUTATION:
                        if (numEnabledLinkGenes < 1)
                            numEnabledLinkGenes = 1;
                        for (int k = 0; k < childLinkList.Count; k++) {
                            float mutateChance = mutationBlastModifier * masterMutationRate / (1f + (float)numEnabledLinkGenes * 0.15f);
                            if (LifetimeGeneration - childLinkList[k].birthGen < newLinkBonusDuration) {
                                float t = 1 - ((LifetimeGeneration - childLinkList[k].birthGen) / (float)newLinkBonusDuration);
                                // t=0 means age of gene is same as bonusDuration, t=1 means it is brand new
                                mutateChance = Mathf.Lerp(mutateChance, mutateChance * newLinkMutateBonus, t);
                            }
                            if (CheckForMutation(mutateChance)) {  // Weight Mutation!
                                //Debug.Log("Weight Mutation Link[" + k.ToString() + "] weight: " + childLinkList[k].weight.ToString() + ", mutate: " + MutateFloat(childLinkList[k].weight).ToString());
                                childLinkList[k].weight = MutateFloat(childLinkList[k].weight);
                                totalNumWeightMutations++;
                            }
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationRemoveLinkChance)) {
                            //Debug.Log("Remove Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            childBrainGenome.RemoveRandomLink();
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationAddNodeChance)) {   // Adds a new node
                            //Debug.Log("Add Node Mutation Agent[" + newChildIndex.ToString() + "]");
                            childBrainGenome.AddNewRandomNode(LifetimeGeneration);
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationAddLinkChance)) { // Adds new connection
                            //Debug.Log("Add Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            if (CheckForMutation(existingNetworkBias)) {
                                childBrainGenome.AddNewExtraLink(existingFromNodeBias, LifetimeGeneration);
                            }
                            else {
                                childBrainGenome.AddNewRandomLink(LifetimeGeneration);
                            }
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationActivationFunctionChance)) {
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
                    // OLD! //newChildAgent.bodyGenome = parentAgentsArray[0].bodyGenome; // Do I need to use the 'new' keyword to prevent dual-referencing?
                    CritterGenome childBodyGenome = new CritterGenome();  // create new body genome for Child
                    // This creates the ROOT NODE!!!!
                    // Clone Nodes & Addons from more fit parent to create new child body genome                   
                    // Iterate over the nodes of the more fit parent:
                    for (int i = 0; i < parentAgentsArray[0].bodyGenome.CritterNodeList.Count; i++) {
                        int currentNodeInno = parentAgentsArray[0].bodyGenome.CritterNodeList[i].innov;
                        if (i == 0) {  // if this is the ROOT NODE:
                            childBodyGenome.CritterNodeList[0].CopySettingsFromNode(parentAgentsArray[0].bodyGenome.CritterNodeList[0]);
                            // The root node was already created during the Constructor method of the CritterGenome,
                            // ... so instead of creating a new one, just copy the settings
                        }
                        else {  // NOT the root node, proceed normally:
                                // Create new cloned node defaulted to the settings of the source( more-fit parent's) Node:
                            CritterNode clonedCritterNode = parentAgentsArray[0].bodyGenome.CritterNodeList[i].CloneThisCritterNode();
                            childBodyGenome.CritterNodeList.Add(clonedCritterNode);
                        }                        
                    }
                    // ADD-ONS!!!!!!!!!!!!!!!!!!!!!!
                    BreedCritterAddons(ref childBodyGenome, ref parentAgentsArray[0].bodyGenome, ref parentAgentsArray[0].bodyGenome);
                    newChildAgent.bodyGenome = childBodyGenome;
                    //===============================================================================================

                    for (int i = 0; i < parentNodeListArray[0].Count; i++) {
                        // iterate through all nodes in the parent List and copy them into fresh new geneNodes:
                        GeneNodeNEAT clonedNode = new GeneNodeNEAT(parentNodeListArray[0][i].id, parentNodeListArray[0][i].nodeType, parentNodeListArray[0][i].activationFunction);
                        childNodeList.Add(clonedNode);
                    }
                    for (int j = 0; j < parentLinkListArray[0].Count; j++) {
                        //same thing with connections
                        GeneLinkNEAT clonedLink = new GeneLinkNEAT(parentLinkListArray[0][j].fromNodeID, parentLinkListArray[0][j].toNodeID, parentLinkListArray[0][j].weight, parentLinkListArray[0][j].enabled, parentLinkListArray[0][j].innov, parentLinkListArray[0][j].birthGen);
                        childLinkList.Add(clonedLink);
                        if (parentLinkListArray[0][j].enabled)
                            numEnabledLinkGenes++;
                    }
                    // MUTATION:
                    if (useMutation) {
                        // BODY MUTATION:
                        PerformBodyMutation(ref childBodyGenome);

                        // BRAIN MUTATION:
                        if (numEnabledLinkGenes < 1)
                            numEnabledLinkGenes = 1;
                        for (int k = 0; k < childLinkList.Count; k++) {
                            float mutateChance = mutationBlastModifier * masterMutationRate / (1f + (float)numEnabledLinkGenes * 0.15f);
                            if (LifetimeGeneration - childLinkList[k].birthGen < newLinkBonusDuration) {
                                float t = 1 - ((LifetimeGeneration - childLinkList[k].birthGen) / (float)newLinkBonusDuration);
                                // t=0 means age of gene is same as bonusDuration, t=1 means it is brand new
                                mutateChance = Mathf.Lerp(mutateChance, mutateChance * newLinkMutateBonus, t);
                            }
                            if (CheckForMutation(mutateChance)) {  // Weight Mutation!
                                //Debug.Log("Weight Mutation Link[" + k.ToString() + "] weight: " + childLinkList[k].weight.ToString() + ", mutate: " + MutateFloat(childLinkList[k].weight).ToString());
                                childLinkList[k].weight = MutateFloat(childLinkList[k].weight);
                                totalNumWeightMutations++;
                            }
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationRemoveLinkChance)) {
                            //Debug.Log("Remove Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            childBrainGenome.RemoveRandomLink();
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationAddNodeChance)) {   // Adds a new node
                            //Debug.Log("Add Node Mutation Agent[" + newChildIndex.ToString() + "]");
                            childBrainGenome.AddNewRandomNode(LifetimeGeneration);
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationAddLinkChance)) { // Adds new connection
                            //Debug.Log("Add Link Mutation Agent[" + newChildIndex.ToString() + "]");
                            if(CheckForMutation(existingNetworkBias)) {
                                childBrainGenome.AddNewExtraLink(existingFromNodeBias, LifetimeGeneration);
                            }
                            else {
                                childBrainGenome.AddNewRandomLink(LifetimeGeneration);
                            }
                        }
                        if (CheckForMutation(mutationBlastModifier * mutationActivationFunctionChance)) {
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
                
                newChildAgent.brainGenome = childBrainGenome;
                newChildAgent.brainGenome.nodeNEATList = childNodeList;
                newChildAgent.brainGenome.linkNEATList = childLinkList;
                BrainNEAT childBrain = new BrainNEAT(childBrainGenome);
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
        
        Debug.Log("Finished Crossover! totalNumWeightMutations: " + totalNumWeightMutations.ToString() + ", mutationBlastModifier: " + mutationBlastModifier.ToString() + ", LifetimeGeneration: " + LifetimeGeneration.ToString() + ", currentGeneration: " + currentGeneration.ToString() + ", sourcePopulation.trainingGenerations: " + sourcePopulation.trainingGenerations.ToString());
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
