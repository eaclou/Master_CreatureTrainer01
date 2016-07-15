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
    public float masterMutationRate = 0.05f;
	public float maximumWeightMagnitude = 4f;
	public float mutationDriftScale = 0.75f;
	public float mutationRemoveLinkChance = 0.05f;
	public float mutationAddLinkChance = 0.2f;	
    public float mutationRemoveNodeChance = 0.0f;
    public float mutationAddNodeChance = 0f;
    public float mutationActivationFunctionChance = 0.0f;
    public float largeBrainPenalty = 0f;
    public float newLinkMutateBonus = 1.3f;  // 1 = does nothing 
    public int newLinkBonusDuration = 25;
    public float existingNetworkBias = 0f;
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

	public float breedingRate = 0.8f;   // percentage of population that performed well enough to breed
	public bool breedingByRank = false;
	public bool breedingStochastic = false;
	public bool breedingByRaffle = true;

    public float mutationBlastModifier = 1f;
    public float bodyMutationBlastModifier = 1f;

    //empty constructor for easySave2
    public CrossoverManager() {

	}

    public int GetNextNodeInnov() {
        nextNodeInnov++;
        return nextNodeInnov;
    }
    public int GetNextAddonInnov() {
        //nextAddonInnov++;
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

	public float MutateBodyFloatAdd(float sourceFloat, float maxDrift) {        
		return sourceFloat + UnityEngine.Random.Range(-maxDrift, maxDrift);
	}
    public float MutateBodyFloatAdd(float sourceFloat, float maxAmount, float min, float max) {
        return sourceFloat + Mathf.Max(Mathf.Min(UnityEngine.Random.Range(-maxAmount, maxAmount), max), min);
    }
    public float MutateBodyFloatMult(float sourceFloat) {
        return sourceFloat * UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange);
    }
    public float MutateBodyFloatMult(float sourceFloat, float min, float max) {
        return sourceFloat * Mathf.Max(Mathf.Min(UnityEngine.Random.Range(1f / maxAttributeValueChange, maxAttributeValueChange), max), min);
    }
    public bool MutateBodyBool(bool sourceBool) {        
        if(UnityEngine.Random.Range(0f, 1f) < 0.5f) {
            sourceBool = !sourceBool;
        }
        return sourceBool;
    }

    //public Vector3 MutateBodyVector3(Vector3 sourceVector) {
        //return sourceVector;
    //}
    public Vector3 MutateBodyVector3Normalized(Vector3 sourceVector) {        
        return Vector3.Slerp(sourceVector, UnityEngine.Random.onUnitSphere, maxAttributeValueChange - 1f).normalized;
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
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonPhysicalAttributesList.Count; i++) {
            AddonPhysicalAttributes clonedAddon = sourceBodyGenomeMoreFit.addonPhysicalAttributesList[i].CloneThisAddon();
            childBodyGenome.addonPhysicalAttributesList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonPhysicalAttributesList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.bounciness[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].bounciness[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.dynamicFriction[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].dynamicFriction[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.staticFriction[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].staticFriction[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.freezePositionX[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].freezePositionX[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.freezePositionY[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].freezePositionY[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.freezePositionZ[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].freezePositionZ[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.freezeRotationX[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].freezeRotationX[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.freezeRotationY[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].freezeRotationY[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.freezeRotationZ[0] = sourceBodyGenomeLessFit.addonPhysicalAttributesList[a].freezeRotationZ[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonJointAngleSensorList[a].sensitivity[0];
                        }
                        // #################  How to handle MEasureVel Checkbox?? It adds a Neuron!!!!!!!!!
                        // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.contactSensitivity[0] = sourceBodyGenomeLessFit.addonContactSensorList[a].contactSensitivity[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.forwardVector[0] = sourceBodyGenomeLessFit.addonRaycastSensorList[a].forwardVector[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.maxDistance[0] = sourceBodyGenomeLessFit.addonRaycastSensorList[a].maxDistance[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.forwardVector[0] = sourceBodyGenomeLessFit.addonCompassSensor1DList[a].forwardVector[0];
                        }
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
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonPositionSensor1DList.Count; i++) {
            AddonPositionSensor1D clonedAddon = sourceBodyGenomeMoreFit.addonPositionSensor1DList[i].CloneThisAddon();
            childBodyGenome.addonPositionSensor1DList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonPositionSensor1DList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonPositionSensor1DList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.forwardVector[0] = sourceBodyGenomeLessFit.addonPositionSensor1DList[a].forwardVector[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.relative[0] = sourceBodyGenomeLessFit.addonPositionSensor1DList[a].relative[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonPositionSensor1DList[a].sensitivity[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonPositionSensor3DList[a].sensitivity[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.relative[0] = sourceBodyGenomeLessFit.addonPositionSensor3DList[a].relative[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.localAxis[0] = sourceBodyGenomeLessFit.addonRotationSensor1DList[a].localAxis[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonRotationSensor1DList[a].sensitivity[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonRotationSensor3DList[a].sensitivity[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonVelocitySensor1DList[a].sensitivity[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.forwardVector[0] = sourceBodyGenomeLessFit.addonVelocitySensor1DList[a].forwardVector[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.relative[0] = sourceBodyGenomeLessFit.addonVelocitySensor1DList[a].relative[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonVelocitySensor3DList[a].sensitivity[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.relative[0] = sourceBodyGenomeLessFit.addonVelocitySensor3DList[a].relative[0];
                        }
                    }
                }
            }
        }
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonAltimeterList.Count; i++) {
            AddonAltimeter clonedAddon = sourceBodyGenomeMoreFit.addonAltimeterList[i].CloneThisAddon();
            childBodyGenome.addonAltimeterList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonAltimeterList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonAltimeterList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.sensitivity[0] = sourceBodyGenomeLessFit.addonEarBasicList[a].sensitivity[0];
                        }
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
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonOscillatorInputList.Count; i++) {
            AddonOscillatorInput clonedAddon = sourceBodyGenomeMoreFit.addonOscillatorInputList[i].CloneThisAddon();
            childBodyGenome.addonOscillatorInputList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonOscillatorInputList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonOscillatorInputList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.amplitude[0] = sourceBodyGenomeLessFit.addonOscillatorInputList[a].amplitude[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.frequency[0] = sourceBodyGenomeLessFit.addonOscillatorInputList[a].frequency[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.offset[0] = sourceBodyGenomeLessFit.addonOscillatorInputList[a].offset[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.value[0] = sourceBodyGenomeLessFit.addonValueInputList[a].value[0];
                        }
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

        for (int i = 0; i < sourceBodyGenomeMoreFit.addonJointMotorList.Count; i++) {
            AddonJointMotor clonedAddon = sourceBodyGenomeMoreFit.addonJointMotorList[i].CloneThisAddon();
            childBodyGenome.addonJointMotorList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonJointMotorList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonJointMotorList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.motorForce[0] = sourceBodyGenomeLessFit.addonJointMotorList[a].motorForce[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.motorSpeed[0] = sourceBodyGenomeLessFit.addonJointMotorList[a].motorSpeed[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.forwardVector[0] = sourceBodyGenomeLessFit.addonThrusterEffector1DList[a].forwardVector[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.maxForce[0] = sourceBodyGenomeLessFit.addonThrusterEffector1DList[a].maxForce[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.maxForce[0] = sourceBodyGenomeLessFit.addonThrusterEffector3DList[a].maxForce[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.maxTorque[0] = sourceBodyGenomeLessFit.addonTorqueEffector1DList[a].maxTorque[0];
                        }
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.axis[0] = sourceBodyGenomeLessFit.addonTorqueEffector1DList[a].axis[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.maxTorque[0] = sourceBodyGenomeLessFit.addonTorqueEffector3DList[a].maxTorque[0];
                        }
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
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.amplitude[0] = sourceBodyGenomeLessFit.addonNoiseMakerBasicList[a].amplitude[0];
                        }
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
        for (int i = 0; i < sourceBodyGenomeMoreFit.addonWeaponBasicList.Count; i++) {
            AddonWeaponBasic clonedAddon = sourceBodyGenomeMoreFit.addonWeaponBasicList[i].CloneThisAddon();
            childBodyGenome.addonWeaponBasicList.Add(clonedAddon);

            if (useCrossover) {
                int sourceInno = clonedAddon.innov;
                for (int a = 0; a < sourceBodyGenomeLessFit.addonWeaponBasicList.Count; a++) {
                    if (sourceBodyGenomeLessFit.addonWeaponBasicList[a].innov == sourceInno) {  // if the LESS-fit parent also has this Add-On:
                        // MIX SETTINGS HERE!!!
                        if (CheckForMutation(0.5f)) {
                            clonedAddon.strength[0] = sourceBodyGenomeLessFit.addonWeaponBasicList[a].strength[0];
                        }
                    }
                }
            }
        }
    }

    public void PerformBodyMutation(ref CritterGenome bodyGenome, ref GenomeNEAT brainGenome) {
        int numBodyMutations = 0;
        //float attachDirMutationMultiplier = 0.1f;
        //float restAngleDirMutationMultiplier = 0.1f;
        float jointAngleTypeMultiplier = 0.5f;
        List<int> nodesToDelete = new List<int>();        
        for (int i = 0; i < bodyGenome.CritterNodeList.Count; i++) {
            
            // Segment Proportions:
            if(CheckForMutation(segmentProportionChance * bodyMutationBlastModifier)) {  // X
                bodyGenome.CritterNodeList[i].dimensions.x = MutateBodyFloatMult(bodyGenome.CritterNodeList[i].dimensions.x);
                numBodyMutations++;
            }
            if (CheckForMutation(segmentProportionChance * bodyMutationBlastModifier)) {  // Y
                bodyGenome.CritterNodeList[i].dimensions.y = MutateBodyFloatMult(bodyGenome.CritterNodeList[i].dimensions.y);
                numBodyMutations++;
            }
            if (CheckForMutation(segmentProportionChance * bodyMutationBlastModifier)) {  // Z
                bodyGenome.CritterNodeList[i].dimensions.z = MutateBodyFloatMult(bodyGenome.CritterNodeList[i].dimensions.z);
                numBodyMutations++;
            }
            if(i > 0) {   // NOT THE ROOT SEGMENT!:
                // REMOVE SEGMENT!!!!
                if(CheckForMutation(removeSegmentChance * bodyMutationBlastModifier)) {  // Can't delete Root Segment
                    // Add this nodeID into queue for deletion at the end of mutation, to avoid shortening NodeList while traversing it:
                    nodesToDelete.Add(i);                    
                }
                else {
                    // Segment Attach Settings:
                    if (CheckForMutation(segmentAttachSettingsChance * bodyMutationBlastModifier)) {  // Position X
                        bodyGenome.CritterNodeList[i].jointLink.attachDir = MutateBodyVector3Normalized(bodyGenome.CritterNodeList[i].jointLink.attachDir);
                        numBodyMutations++;
                    }
                    if (CheckForMutation(segmentAttachSettingsChance * bodyMutationBlastModifier)) {  // Orientation X
                        bodyGenome.CritterNodeList[i].jointLink.restAngleDir = MutateBodyVector3Normalized(bodyGenome.CritterNodeList[i].jointLink.restAngleDir);
                        numBodyMutations++;
                    }

                    // Joint Settings:
                    if (CheckForMutation(jointSettingsChance * bodyMutationBlastModifier)) {
                        bodyGenome.CritterNodeList[i].jointLink.jointLimitPrimary = MutateBodyFloatMult(bodyGenome.CritterNodeList[i].jointLink.jointLimitPrimary);
                    }
                    if (CheckForMutation(jointSettingsChance * bodyMutationBlastModifier)) {
                        bodyGenome.CritterNodeList[i].jointLink.jointLimitSecondary = MutateBodyFloatMult(bodyGenome.CritterNodeList[i].jointLink.jointLimitSecondary);
                    }
                    if (CheckForMutation(jointSettingsChance * bodyMutationBlastModifier)) {
                        bodyGenome.CritterNodeList[i].jointLink.recursionScalingFactor = MutateBodyFloatMult(bodyGenome.CritterNodeList[i].jointLink.recursionScalingFactor);
                    }
                    // Joint Hinge TYPE!!!!  --- how to handle this???
                    if (CheckForMutation(jointSettingsChance * jointAngleTypeMultiplier * bodyMutationBlastModifier)) {
                        int jointTypeInt = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
                        bodyGenome.CritterNodeList[i].jointLink.jointType = (CritterJointLink.JointType)jointTypeInt;
                        Debug.Log(i.ToString() + " Mutated to JointType: " + bodyGenome.CritterNodeList[i].jointLink.jointType);
                    }

                    // RECURSION:
                    if (CheckForMutation(recursionChance * bodyMutationBlastModifier)) {
                        int addRemove = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f)) * 2 - 1;  // either -1 or 1
                                                                                                     //addRemove = 1; // TEMPORARY!!! testing add recursion
                        if (bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions + addRemove < 0 || bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions + addRemove > 3) {
                            // Do Nothing, tried to change numRecursions beyond the CAP
                        }
                        else {  // It's all good -- PROCEED:
                            bodyGenome.CritterNodeList[i].jointLink.numberOfRecursions += addRemove;
                            numBodyMutations++;

                            if (addRemove > 0) {  // Created an additional Recursion -- Need to ADD new BrainNodes:
                                                  //int numNodesBefore = brainGenome.nodeNEATList.Count;                            
                                                  //brainGenome.AdjustBrainAfterBodyChange(bodyGenome);
                                                  //int numNodesAfter = brainGenome.nodeNEATList.Count;
                                                  //Debug.Log("MUTATION RECURSION! b4#: " + numNodesBefore.ToString() + ", after#: " + numNodesAfter.ToString());
                            }
                            else {  // REMOVED a Recursion -- Need to REMOVE BrainNodes:
                                    //brainGenome.AdjustBrainRemovedRecursion(bodyGenome, i);
                            }
                            
                        }
                        // NEED TO FIX BRAIN!!! -- how to preserve existing wiring while adding new neurons that may re-order?
                        // Do I need to save a reference to segment/nodes within each input/output neuron?
                    }

                    // SYMMETRY

                    // EXTRA -- recursion forward, only attach to tail
                    
                }

                // CREATE NEW ADD-ON!!  On this segment
                if (CheckForMutation(newAddonChance * bodyMutationBlastModifier)) {
                    // Which types can be created automatically??
                    // 
                    int randomAddon = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
                    switch (randomAddon) {
                        case 0:
                            AddonOscillatorInput newOscillatorInput = new AddonOscillatorInput(i, GetNextAddonInnov());
                            bodyGenome.addonOscillatorInputList.Add(newOscillatorInput);
                            break;
                        case 1:
                            AddonValueInput newValueInput = new AddonValueInput(i, GetNextAddonInnov());
                            bodyGenome.addonValueInputList.Add(newValueInput);
                            break;
                        default:
                            break;
                    }
                    Debug.Log("NEW ADDON! " + randomAddon.ToString() + ", segmentNode: " + i.ToString());
                }
            }            
        }

        for(int i = 0; i < nodesToDelete.Count; i++) {
            Debug.Log("DELETE SEGMENT NODE! " + i.ToString());
            bodyGenome.DeleteNode(nodesToDelete[i]);
            bodyGenome.RenumberNodes();
        }
        List<int> nodesToAddTo = new List<int>(); // After Deleting nodes, check all remaining nodes for a chance to have a child segment added:
        for (int i = 0; i < bodyGenome.CritterNodeList.Count; i++) {
            if (CheckForMutation(newSegmentChance * bodyMutationBlastModifier)) {
                nodesToAddTo.Add(i);
            }
        }
        for (int i = 0; i < nodesToAddTo.Count; i++) {
            Vector3 attachDir = Vector3.Slerp(new Vector3(0f, 0f, 1f), UnityEngine.Random.onUnitSphere, UnityEngine.Random.Range(0f, 1f)); //ConvertWorldSpaceToAttachDir(selectedSegment, rightClickWorldPosition);
            int nextID = bodyGenome.CritterNodeList.Count;
            bodyGenome.AddNewNode(bodyGenome.CritterNodeList[nodesToAddTo[i]], attachDir, new Vector3(0f, 0f, 0f), nextID, GetNextNodeInnov());
            
            // Auto-Add-ons: -- New Segment starts with a joint angle sensor and joint Motor by default:
            AddonJointAngleSensor newJointAngleSensor = new AddonJointAngleSensor(nextID, GetNextAddonInnov());
            bodyGenome.addonJointAngleSensorList.Add(newJointAngleSensor);
            // Motor:
            AddonJointMotor newJointMotor = new AddonJointMotor(nextID, GetNextAddonInnov());
            bodyGenome.addonJointMotorList.Add(newJointMotor);

            Debug.Log("New SEGMENT! : " + nodesToAddTo[i].ToString());
        }        

        // Addon Mutation:
        PerformAddonMutation(ref bodyGenome, ref brainGenome);
        //Debug.Log("NumBodyMutations: " + numBodyMutations.ToString());
    }

    public void PerformAddonMutation(ref CritterGenome bodyGenome, ref GenomeNEAT brainGenome) {
             
        // Chance to Remove an existing Add-on:
        // Chance to Mutate Add-On Settings:
        for (int i = bodyGenome.addonPhysicalAttributesList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonPhysicalAttributesList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPhysicalAttributesList[i].bounciness[0] = MutateBodyFloatMult(bodyGenome.addonPhysicalAttributesList[i].bounciness[0], 0f, 1f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPhysicalAttributesList[i].dynamicFriction[0] = MutateBodyFloatMult(bodyGenome.addonPhysicalAttributesList[i].dynamicFriction[0], 0f, 1f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPhysicalAttributesList[i].staticFriction[0] = MutateBodyFloatMult(bodyGenome.addonPhysicalAttributesList[i].staticFriction[0], 0f, 1f);
                }
                /*if (CheckForMutation(addonSettingsChance)) {
                    bodyGenome.addonPhysicalAttributesList[i].freezePositionX[0] = MutateBodyBool(bodyGenome.addonPhysicalAttributesList[i].freezePositionX[0]);
                }
                if (CheckForMutation(addonSettingsChance)) {
                    bodyGenome.addonPhysicalAttributesList[i].freezePositionY[0] = MutateBodyBool(bodyGenome.addonPhysicalAttributesList[i].freezePositionY[0]);
                }
                if (CheckForMutation(addonSettingsChance)) {
                    bodyGenome.addonPhysicalAttributesList[i].freezePositionZ[0] = MutateBodyBool(bodyGenome.addonPhysicalAttributesList[i].freezePositionZ[0]);
                }
                if (CheckForMutation(addonSettingsChance)) {
                    bodyGenome.addonPhysicalAttributesList[i].freezeRotationX[0] = MutateBodyBool(bodyGenome.addonPhysicalAttributesList[i].freezeRotationX[0]);
                }
                if (CheckForMutation(addonSettingsChance)) {
                    bodyGenome.addonPhysicalAttributesList[i].freezeRotationY[0] = MutateBodyBool(bodyGenome.addonPhysicalAttributesList[i].freezeRotationY[0]);
                }
                if (CheckForMutation(addonSettingsChance)) {
                    bodyGenome.addonPhysicalAttributesList[i].freezeRotationZ[0] = MutateBodyBool(bodyGenome.addonPhysicalAttributesList[i].freezeRotationZ[0]);
                }*/
            }
        }
        for (int i = bodyGenome.addonJointAngleSensorList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonJointAngleSensorList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonJointAngleSensorList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonJointAngleSensorList[i].sensitivity[0], 0.001f, 100f);
                }
                // #################  How to handle MEasureVel Checkbox?? It adds a Neuron!!!!!!!!!
                // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%   
            }
        }
        for (int i = bodyGenome.addonContactSensorList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonContactSensorList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonContactSensorList[i].contactSensitivity[0] = MutateBodyFloatMult(bodyGenome.addonContactSensorList[i].contactSensitivity[0], 0.01f, 10f);
                }
            }            
        }
        for (int i = bodyGenome.addonRaycastSensorList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonRaycastSensorList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonRaycastSensorList[i].forwardVector[0] = MutateBodyVector3Normalized(bodyGenome.addonRaycastSensorList[i].forwardVector[0]);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonRaycastSensorList[i].maxDistance[0] = MutateBodyFloatMult(bodyGenome.addonRaycastSensorList[i].maxDistance[0], 0.01f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonCompassSensor1DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonCompassSensor1DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonCompassSensor1DList[i].forwardVector[0] = MutateBodyVector3Normalized(bodyGenome.addonCompassSensor1DList[i].forwardVector[0]);
                }
            }                               
        }
        for (int i = bodyGenome.addonCompassSensor3DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonCompassSensor3DList.RemoveAt(i);
            }
            else {

            }

        }
        for (int i = bodyGenome.addonPositionSensor1DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonPositionSensor1DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPositionSensor1DList[i].forwardVector[0] = MutateBodyVector3Normalized(bodyGenome.addonPositionSensor1DList[i].forwardVector[0]);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPositionSensor1DList[i].relative[0] = MutateBodyBool(bodyGenome.addonPositionSensor1DList[i].relative[0]);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPositionSensor1DList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonPositionSensor1DList[i].sensitivity[0], 0.01f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonPositionSensor3DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonPositionSensor3DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPositionSensor3DList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonPositionSensor3DList[i].sensitivity[0], 0.01f, 100f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonPositionSensor3DList[i].relative[0] = MutateBodyBool(bodyGenome.addonPositionSensor3DList[i].relative[0]);
                }
            }            
        }
        for (int i = bodyGenome.addonRotationSensor1DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonRotationSensor1DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonRotationSensor1DList[i].localAxis[0] = MutateBodyVector3Normalized(bodyGenome.addonRotationSensor1DList[i].localAxis[0]);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonRotationSensor1DList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonRotationSensor1DList[i].sensitivity[0], 0.01f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonRotationSensor3DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonRotationSensor3DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonRotationSensor3DList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonRotationSensor3DList[i].sensitivity[0], 0.01f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonVelocitySensor1DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonVelocitySensor1DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonVelocitySensor1DList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonVelocitySensor1DList[i].sensitivity[0], 0.01f, 100f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonVelocitySensor1DList[i].forwardVector[0] = MutateBodyVector3Normalized(bodyGenome.addonVelocitySensor1DList[i].forwardVector[0]);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonVelocitySensor1DList[i].relative[0] = MutateBodyBool(bodyGenome.addonVelocitySensor1DList[i].relative[0]);
                }
            }            
        }
        for (int i = bodyGenome.addonVelocitySensor3DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonVelocitySensor3DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonVelocitySensor3DList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonVelocitySensor3DList[i].sensitivity[0], 0.01f, 100f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonVelocitySensor3DList[i].relative[0] = MutateBodyBool(bodyGenome.addonVelocitySensor3DList[i].relative[0]);
                }
            }            
        }
        for (int i = bodyGenome.addonAltimeterList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonAltimeterList.RemoveAt(i);
            }
            else {

            }
        }
        for (int i = bodyGenome.addonEarBasicList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonEarBasicList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonEarBasicList[i].sensitivity[0] = MutateBodyFloatMult(bodyGenome.addonEarBasicList[i].sensitivity[0], 0.01f, 100f);
                }
            }                              
        }
        for (int i = bodyGenome.addonGravitySensorList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonGravitySensorList.RemoveAt(i);
            }
            else {

            }
        }
        for (int i = bodyGenome.addonOscillatorInputList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonOscillatorInputList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonOscillatorInputList[i].amplitude[0] = MutateBodyFloatMult(bodyGenome.addonOscillatorInputList[i].amplitude[0], 0.01f, 100f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonOscillatorInputList[i].frequency[0] = MutateBodyFloatMult(bodyGenome.addonOscillatorInputList[i].frequency[0], 0.01f, 50f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonOscillatorInputList[i].offset[0] = MutateBodyFloatMult(bodyGenome.addonOscillatorInputList[i].offset[0], -50f, 50f);
                }
            }            
        }
        for (int i = bodyGenome.addonValueInputList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonValueInputList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonValueInputList[i].value[0] = MutateBodyFloatMult(bodyGenome.addonValueInputList[i].value[0], -100f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonTimerInputList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonTimerInputList.RemoveAt(i);
            }
            else {

            }
        }
        //=======================  OUTPUTS ===================== OUTPUTS ======================= OUTPUTS =====================================================
        for (int i = bodyGenome.addonJointMotorList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonJointMotorList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonJointMotorList[i].motorForce[0] = MutateBodyFloatMult(bodyGenome.addonJointMotorList[i].motorForce[0], 0.01f, 1000f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonJointMotorList[i].motorSpeed[0] = MutateBodyFloatMult(bodyGenome.addonJointMotorList[i].motorSpeed[0], 0.01f, 1000f);
                }
            }            
        }
        for (int i = bodyGenome.addonThrusterEffector1DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonThrusterEffector1DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonThrusterEffector1DList[i].forwardVector[0] = MutateBodyVector3Normalized(bodyGenome.addonThrusterEffector1DList[i].forwardVector[0]);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonThrusterEffector1DList[i].maxForce[0] = MutateBodyFloatMult(bodyGenome.addonThrusterEffector1DList[i].maxForce[0], 0.01f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonThrusterEffector3DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonThrusterEffector3DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonThrusterEffector3DList[i].maxForce[0] = MutateBodyFloatMult(bodyGenome.addonThrusterEffector3DList[i].maxForce[0], 0.01f, 100f);
                }
            }            
        }        
        for (int i = bodyGenome.addonTorqueEffector1DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonTorqueEffector1DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonTorqueEffector1DList[i].maxTorque[0] = MutateBodyFloatMult(bodyGenome.addonTorqueEffector1DList[i].maxTorque[0], 0.01f, 100f);
                }
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonTorqueEffector1DList[i].axis[0] = MutateBodyVector3Normalized(bodyGenome.addonTorqueEffector1DList[i].axis[0]);
                }
            }            
        }
        for (int i = bodyGenome.addonTorqueEffector3DList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonTorqueEffector3DList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonTorqueEffector3DList[i].maxTorque[0] = MutateBodyFloatMult(bodyGenome.addonTorqueEffector3DList[i].maxTorque[0], 0.01f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonMouthBasicList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonMouthBasicList.RemoveAt(i);
            }
            else {

            }
        }
        for (int i = bodyGenome.addonNoiseMakerBasicList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonNoiseMakerBasicList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonNoiseMakerBasicList[i].amplitude[0] = MutateBodyFloatMult(bodyGenome.addonNoiseMakerBasicList[i].amplitude[0], 0.01f, 100f);
                }
            }            
        }
        for (int i = bodyGenome.addonStickyList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonStickyList.RemoveAt(i);
            }
            else {

            }
        }
        for (int i = bodyGenome.addonWeaponBasicList.Count - 1; i >= 0; i--) {
            if (CheckForMutation(removeAddonChance * bodyMutationBlastModifier)) {
                // DELETE THIS ADD-ON!!!
                bodyGenome.addonWeaponBasicList.RemoveAt(i);
            }
            else {
                if (CheckForMutation(addonSettingsChance * bodyMutationBlastModifier)) {
                    bodyGenome.addonWeaponBasicList[i].strength[0] = MutateBodyFloatMult(bodyGenome.addonWeaponBasicList[i].strength[0], 0.01f, 100f);
                }
            }            
        }

        // UPDATE BRAIN/BODY:
        brainGenome.AdjustBrainAfterBodyChange(ref bodyGenome);
    }

    public void BodyCrossover(ref CritterNode clonedCritterNode, CritterNode lessFitNode) {
        // Node dimensions
        if(CheckForMutation(0.5f)) {
            clonedCritterNode.dimensions = lessFitNode.dimensions;
        }
        // Joint Settings - placement, orientation
        if (CheckForMutation(0.5f)) {
            clonedCritterNode.jointLink.attachDir = lessFitNode.jointLink.attachDir;
        }
        if (CheckForMutation(0.5f)) {
            clonedCritterNode.jointLink.restAngleDir = lessFitNode.jointLink.restAngleDir;
        }
        if (CheckForMutation(0.5f)) {
            clonedCritterNode.jointLink.jointLimitPrimary = lessFitNode.jointLink.jointLimitPrimary;
            clonedCritterNode.jointLink.jointLimitSecondary = lessFitNode.jointLink.jointLimitSecondary;
            clonedCritterNode.jointLink.jointType = lessFitNode.jointLink.jointType;
        }
        // Recursion
        if (CheckForMutation(0.5f)) {
            clonedCritterNode.jointLink.numberOfRecursions = lessFitNode.jointLink.numberOfRecursions;
        }
        // Symmetry
        if (CheckForMutation(0.5f)) {
            clonedCritterNode.jointLink.symmetryType = lessFitNode.jointLink.symmetryType;
        }
        // Scale Factors, forward Bias, Attach only to Last, etc.
        if (CheckForMutation(0.5f)) {
            clonedCritterNode.jointLink.recursionScalingFactor = lessFitNode.jointLink.recursionScalingFactor;
            clonedCritterNode.jointLink.recursionForward = lessFitNode.jointLink.recursionForward;
            clonedCritterNode.jointLink.onlyAttachToTailNode = lessFitNode.jointLink.onlyAttachToTailNode;
        }
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
                        GeneNodeNEAT clonedNode = new GeneNodeNEAT(parentNodeListArray[moreFitParent][i].id, parentNodeListArray[moreFitParent][i].nodeType, parentNodeListArray[moreFitParent][i].activationFunction, parentNodeListArray[moreFitParent][i].sourceAddonInno, parentNodeListArray[moreFitParent][i].sourceAddonRecursionNum, parentNodeListArray[moreFitParent][i].sourceAddonChannelNum);
                        childNodeList.Add(clonedNode);
                    }

                    if (useMutation) {
                        // BODY MUTATION:
                        //PerformBodyMutation(ref childBodyGenome, ref childBrainGenome);
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

                    // THE BODY   ==========!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!======================================================================================
                    CritterGenome childBodyGenome = new CritterGenome();  // create new body genome for Child
                    // This creates the ROOT NODE!!!!
                    // Clone Nodes & Addons from more fit parent to create new child body genome
                    // crossover is on, so check for matching Nodes and Add-ons (based on Inno#'s) to determine when to mix Settings/Attributes:                    
                    // Iterate over the nodes of the more fit parent:
                    for (int i = 0; i < parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList.Count; i++) {
                        int currentNodeInno = parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList[i].innov;
                        if (i == 0) {  // if this is the ROOT NODE:
                            childBodyGenome.CritterNodeList[0].CopySettingsFromNode(parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList[0]);
                            // The root node was already created during the Constructor method of the CritterGenome,
                            // ... so instead of creating a new one, just copy the settings
                        }
                        else {  // NOT the root node, proceed normally:
                                // Create new cloned node defaulted to the settings of the source( more-fit parent's) Node:
                            CritterNode clonedCritterNode = parentAgentsArray[moreFitParent].bodyGenome.CritterNodeList[i].CloneThisCritterNode();
                            
                            // Check other parent for same node:
                            for (int j = 0; j < parentAgentsArray[1 - moreFitParent].bodyGenome.CritterNodeList.Count; j++) {
                                if (parentAgentsArray[1 - moreFitParent].bodyGenome.CritterNodeList[j].innov == currentNodeInno) {
                                    // CROSSOVER NODE SETTINGS HERE!!!  ---- If random dice roll > 0.5, use less fit parent's settings, otherwise leave as default
                                    BodyCrossover(ref clonedCritterNode, parentAgentsArray[1 - moreFitParent].bodyGenome.CritterNodeList[j]);                                    
                                }
                            }
                            childBodyGenome.CritterNodeList.Add(clonedCritterNode);
                        }                        
                    }
                    // ADD-ONS!!!!!!!!!!!!!!!!!!!!!!
                    BreedCritterAddons(ref childBodyGenome, ref parentAgentsArray[moreFitParent].bodyGenome, ref parentAgentsArray[1 - moreFitParent].bodyGenome);
                    newChildAgent.bodyGenome = childBodyGenome;  // ?????
                    if (useMutation) {
                        // BODY MUTATION:
                        PerformBodyMutation(ref childBodyGenome, ref childBrainGenome);
                    }
                }
                else { // no crossover:                    
                    
                    //===============================================================================================
                    for (int i = 0; i < parentNodeListArray[0].Count; i++) {
                        // iterate through all nodes in the parent List and copy them into fresh new geneNodes:
                        GeneNodeNEAT clonedNode = new GeneNodeNEAT(parentNodeListArray[0][i].id, parentNodeListArray[0][i].nodeType, parentNodeListArray[0][i].activationFunction, parentNodeListArray[0][i].sourceAddonInno, parentNodeListArray[0][i].sourceAddonRecursionNum, parentNodeListArray[0][i].sourceAddonChannelNum);
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
                        //childBrainGenome.nodeNEATList = childNodeList
                        //PerformBodyMutation(ref childBodyGenome, ref childBrainGenome);

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

                    //   THE BODY!!!!! ++++++++++++++++++++++================+++++++++++++++++++===============+++++++++++++++++++===================+++++++++++++++++==============
                    CritterGenome childBodyGenome = new CritterGenome();  // create new body genome for Child                 
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
                    if (useMutation) {
                        // BODY MUTATION:
                        PerformBodyMutation(ref childBodyGenome, ref childBrainGenome);
                    }
                }
                
                newChildAgent.brainGenome = childBrainGenome;
                //newChildAgent.brainGenome.nodeNEATList = childNodeList;
                //newChildAgent.brainGenome.linkNEATList = childLinkList;
                BrainNEAT childBrain = new BrainNEAT(newChildAgent.brainGenome);
                childBrain.BuildBrainNetwork();
                newChildAgent.brain = childBrain;
                //Debug.Log("NEW CHILD numNodes: " + newChildAgent.brainGenome.nodeNEATList.Count.ToString() + ", #Neurons: " + newChildAgent.brain.neuronList.Count.ToString());
                //newChildAgent.bodyGenome.PreBuildCritter(0.8f);
                // Species:
                if (useSpeciation) {
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
        
        Debug.Log("Finished Crossover! totalNumWeightMutations: " + totalNumWeightMutations.ToString() + ", mutationBlastModifier: " + mutationBlastModifier.ToString() + ", bodyMutationBlastModifier: " + bodyMutationBlastModifier.ToString() + ", LifetimeGeneration: " + LifetimeGeneration.ToString() + ", currentGeneration: " + currentGeneration.ToString() + ", sourcePopulation.trainingGenerations: " + sourcePopulation.trainingGenerations.ToString());
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
