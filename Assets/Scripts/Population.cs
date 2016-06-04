using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Population {
	public bool debugFunctionCalls = false; // turns debug messages on/off
    
    public BrainSettings brainSettings;
    public CritterGenome templateGenome;
    public int numInputNodes = 0;
	public int numOutputNodes = 0;
	public int populationMaxSize = 4; // default value
	public int numAgents = 0;
	public Agent[] masterAgentArray;
    public List<SpeciesBreedingPool> speciesBreedingPoolList;
    public int nextAvailableSpeciesID = 0;

    public int trainingGenerations = 0; // ONLY UPDATE ON SAVE
    
	public bool initRandom = false;
	public bool isFunctional = false;

	// Constructor Methods:
	public Population() {
		DebugBot.DebugFunctionCall("Population; Population() Constructor!; ", debugFunctionCalls);
        if(speciesBreedingPoolList == null) {
            speciesBreedingPoolList = new List<SpeciesBreedingPool>();
        }
        brainSettings = new BrainSettings(); // CHANGE THIS LATER!!!!!
	}

    public int GetNextSpeciesID() {
        nextAvailableSpeciesID++;
        return nextAvailableSpeciesID;
    }
    
	public void InitializeMasterAgentArray(CritterGenome bodyGenome) {  // Creates a new population for the FIRST TIME!!!
		DebugBot.DebugFunctionCall("Population; InitializeMasterAgentArray(CritterGenome); ", debugFunctionCalls);
		templateGenome = bodyGenome;
        
        int[] critterData = templateGenome.CalculateNumberOfSegmentsInputsOutputs(); // just to check number of segments, inputs, and outputs
        int numSegments = critterData[0];
        numInputNodes = critterData[1];
        numOutputNodes = critterData[2];
        Debug.Log("Critter Stats [0]: " + numSegments.ToString() + ", [1]: " + numInputNodes.ToString() + ", [2]: " + numOutputNodes.ToString());
                
        masterAgentArray = new Agent[populationMaxSize];
		for(int i = 0; i < populationMaxSize; i++) {
			Agent newAgent = new Agent();
            //newAgent.fitnessRank =
			InitializeAgentBrainAndBody(newAgent, bodyGenome);  // create Agent's brain as proper type, and copies over templateBrain's settings
			masterAgentArray[i] = newAgent;
			numAgents++;
		}

        InitializeSpeciesPoolsAndAgents(); // assigns agents to a species and populates the breeding pools    
    }
    public void InitializeSpeciesPoolsAndAgents() {
        for (int i = 0; i < populationMaxSize; i++) {
            SortAgentIntoBreedingPool(masterAgentArray[i]);
        }
    }

    public void SortAgentIntoBreedingPool(Agent agent) {
        bool speciesGenomeMatch = false;
        for(int s = 0; s < speciesBreedingPoolList.Count; s++) {
            float geneticDistance = GenomeNEAT.MeasureGeneticDistance(agent.brainGenome, speciesBreedingPoolList[s].templateGenome, 0.425f, 0.425f, 0.15f, 0f, 0f, 1f);
            
            if (geneticDistance < 1f) { //speciesSimilarityThreshold) {  // !!! figure out how/where to place this attribute or get a ref from crossoverManager
                speciesGenomeMatch = true;
                //agent.speciesID = speciesBreedingPoolList[s].speciesID; // this is done inside the AddNewAgent method below v v v 
                speciesBreedingPoolList[s].AddNewAgent(agent);
                //Debug.Log("SortAgentIntoBreedingPool dist: " + geneticDistance.ToString() + ", speciesIDs: " + agent.speciesID.ToString() + ", " + speciesBreedingPoolList[s].speciesID.ToString());
                break;
            }
        }
        if(!speciesGenomeMatch) {
            //Debug.Log("POPULATION SortAgentIntoBreedingPool NO MATCH!!! -- creating new BreedingPool ");
            SpeciesBreedingPool newSpeciesBreedingPool = new SpeciesBreedingPool(agent.brainGenome, GetNextSpeciesID()); // creates new speciesPool modeled on this agent's genome
            newSpeciesBreedingPool.AddNewAgent(agent);  // add this agent to breeding pool
            speciesBreedingPoolList.Add(newSpeciesBreedingPool);  // add new speciesPool to the population's list of all active species
        }
    }

    public SpeciesBreedingPool GetBreedingPoolByID(List<SpeciesBreedingPool> poolsList, int id) {
        for (int s = 0; s < poolsList.Count; s++) {
            
            if (poolsList[s].speciesID == id) {
                //Debug.Log("GetBreedingPoolByID MATCH!: id: " + id.ToString() + ", speciesIDs: " + poolsList[s].speciesID.ToString());

                return poolsList[s];
            }
        }
        Debug.Log("GetBreedingPoolByID FAILURE!!!: id: " + id.ToString());
        return null;
    }

    /*public void AssignAgentToSpecies(Agent agent) {
        bool speciesMatch = false;
        for (int s = 0; s < speciesList.Count; s++) {
            // check for match
            //if()
            float geneticDistance = GenomeNEAT.MeasureGeneticDistance(speciesList[s].templateGenome, agent.brainGenome);
            float threshold = 0.8f;

            if (geneticDistance < threshold) {
                // Join that species!
                speciesMatch = true;
                Debug.Log("AssignAgentToSpecies - MATCH! speciesID: " + speciesList[s].id.ToString());
                speciesList[s].AddNewMember(agent);
            }
        }
        if (!speciesMatch) { // no matching species!
                             // create a new species
            
            Species newSpecies = new Species(agent);  // adds member automagically
            speciesList.Add(newSpecies);
            Debug.Log("AssignAgentToSpecies - UNIQUE! speciesID: " + newSpecies.id.ToString() + ", listCount: " + speciesList.Count.ToString());
        }
    }*/

    public void InitializeLoadedMasterAgentArray() {
		for(int i = 0; i < populationMaxSize; i++) {
			InitializeAgentBrainsFromGenome(masterAgentArray[i]);  // create Agent's brain as proper type, and copies over templateBrain's settings
		}
		numAgents = populationMaxSize;
		isFunctional = true;
	}

	public void TempResizeMasterAgentArray() {
		Agent[] newMasterAgentArray = new Agent[populationMaxSize];
		for(int i = 0; i < populationMaxSize; i++) {
			if(i < masterAgentArray.Length) {
				newMasterAgentArray[i] = masterAgentArray[i];
			}
			else {// No agent at this index
				Agent newAgent = new Agent();
				//InitializeAgentBrainOnly(newAgent);  // create Agent's brain as proper type, and copies over templateBrain's settings
				newMasterAgentArray[i] = newAgent;
			}
		}
		numAgents = populationMaxSize;
		masterAgentArray = null;
		masterAgentArray = newMasterAgentArray;
	}

	private void InitializeAgentBrainAndBody(Agent newAgent, CritterGenome bodyGenome) {  /// Configure newly-created Agent (brain + body) for the FIRST TIME!! to change settings on an existing agent, use a different method.
		DebugBot.DebugFunctionCall("Population; InitializeAgentInstance(); ", debugFunctionCalls);
		// Figure out Agent Body HERE:		
		newAgent.bodyGenome = bodyGenome; // set as this agent's body Genome
        
        // BRAIN BELOW:
        // Initialize Brain:
		newAgent.brain = new BrainNEAT();        
		
        GenomeNEAT brainGenome;
		if(initRandom) {
            brainGenome = newAgent.brain.InitializeRandomBrain(numInputNodes, numOutputNodes); // 'builds' the brain and spits out a Genome
		}
		else {
            brainGenome = newAgent.brain.InitializeBlankBrain(numInputNodes, numOutputNodes);
		}
        
        newAgent.brainGenome = brainGenome;
		newAgent.brain.BuildBrainNetwork();  // constructs the brain from its sourceGenome
        //AssignAgentToSpecies(newAgent);
        isFunctional = true;        
	}

    private void InitializeAgentBrainsFromGenome(Agent agent) {  // set up agent's brain in the case of a loaded population		

        //agent.brain.InitializeBrainFromGenome(agent.brainGenome);
        agent.brain = new BrainNEAT(agent.brainGenome);
        agent.brain.BuildBrainNetwork();  // constructs the brain from its sourceGenome
    }

	public void SetMaxPopulationSize(int size) {
		DebugBot.DebugFunctionCall("Population; SetMaxPopulationSize(); ", debugFunctionCalls);
		populationMaxSize = size;
		// Come in here and add restrictions later for shrinking pop size safely
	}

	public void RankAgentArray() { // brute force sort
        
		Agent[] rankedAgentArray = new Agent[populationMaxSize];
		rankedAgentArray = masterAgentArray;
		Agent swapAgentA = new Agent();
		Agent swapAgentB = new Agent();

		for(int i = 0; i < populationMaxSize - 1; i++)
		{
			for(int j = 0; j < populationMaxSize - 1; j++)
			{
				if(rankedAgentArray[j].fitnessScoreSpecies < rankedAgentArray[j+1].fitnessScoreSpecies)  // if lower index holds a smaller time, swap places
				{
					swapAgentA = rankedAgentArray[j];
					swapAgentB = rankedAgentArray[j+1];
					rankedAgentArray[j] = swapAgentB;
					rankedAgentArray[j+1] = swapAgentA;
				}
			}
		}	
		
		string rankedAfterString = "RankedAgentArrayAfter: ";
		for(int h = 0; h < populationMaxSize; h++) {
            rankedAgentArray[h].fitnessRank = h;
            rankedAfterString += "Fit: " + rankedAgentArray[h].fitnessScoreSpecies.ToString () + ", h: " + h.ToString() + ", ";
		}
		Debug.Log (rankedAfterString);
		
		masterAgentArray = rankedAgentArray;
	}

	public Population RefPopulation() {  // returns a copy (same ref memory) of the current Population instance calling the function i.e. Population newPop = new Population(); newPop = sourcePop.CopyPopulation();
		Population populationRef = new Population();
		populationRef.PrintSettings();
		populationRef = this;
		return populationRef;
	}

	public Population CopyPopulationSettings() {  // returns a copy of the current Population instance calling the function i.e. Population newPop = new Population(); newPop = sourcePop.CopyPopulation();
		Population populationCopy = new Population();
		populationCopy.SetMaxPopulationSize(populationMaxSize);
		//populationCopy.brainType = brainType;
		//populationCopy.templateBrain = templateBrain;
		populationCopy.templateGenome = templateGenome;
		populationCopy.numInputNodes = numInputNodes;
		populationCopy.numOutputNodes = numOutputNodes;
		populationCopy.numAgents = numAgents;
		//populationCopy.InitializeMasterAgentArray(templateGenome); // CONFIRM THIS SHOULD BE USED!! MIGHT WANT TO JUST COPY AGENTS!!!!
		// ^ ^ ^ Looks like this is used in the CrossoverManager to break the reference connection to the AgentArray when copying the main Population
		// ....  Seems like this is just needed to assign new memmory for the swapPopulation agentArray, to allow for proper data transfer
		// ....  between the sourcePopulation and the new Copy (which will hold the next generation). New agents are setup at birth time.

		//populationCopy.masterAgentArray = masterAgentArray;
		//populationCopy = this;
		return populationCopy;
	}

	public Population[] SplitPopulation(int numFactions) {
		if(numFactions > masterAgentArray.Length) { // safety
			numFactions = masterAgentArray.Length;
		}
		Population[] populationArray = new Population[numFactions];
		float factionSize = (float)masterAgentArray.Length / (float)numFactions;
		float incrementSize = factionSize;  // start at ideal length
		float currentSplitPosition = 0f;
		int curAgentIndex = 0;
		for(int i = 0; i < numFactions; i++) {   // 4/3:
			currentSplitPosition += incrementSize;  // 1.333, 3.0, 3.67
			int blockSize = Mathf.RoundToInt(incrementSize);  // 1, 2, 1
			incrementSize += (factionSize - (float)blockSize);  // 1.67, -0.67,
			//Debug.Log ("Faction #" + (i+1).ToString () + ": " + blockSize.ToString() + " Agents, IncrementSize= " + incrementSize.ToString() + ", CurPos: " + currentSplitPosition.ToString());

			// Create new Population:
			Population factionPop = new Population();
			factionPop.populationMaxSize = blockSize;
			//factionPop.brainType = brainType;
			//factionPop.templateBrain = templateBrain;
			factionPop.numInputNodes = numInputNodes;
			factionPop.numOutputNodes = numOutputNodes;
			//factionPop.InitializeMasterAgentArray();

			for(int j = 0; j < blockSize; j++) {  // copy over Agents from source Pop.
				factionPop.masterAgentArray[j] = masterAgentArray[curAgentIndex];
				curAgentIndex++;
			}

			populationArray[i] = factionPop;
		}

		return populationArray;
	}

	public static Population AddPopulations(Population[] sourcePopulations) {
		// Find target population size
		return sourcePopulations[0];
	}

	public void SetToCombinedPopulations(Population[] sourcePopulation) {
		int newSize = 0;
		for(int i = 0; i < sourcePopulation.Length; i++) {
			newSize += sourcePopulation[i].masterAgentArray.Length;
		}
		this.ResizePopulation(newSize);
		int masterIndex = 0;
		for(int j = 0; j < sourcePopulation.Length; j++) {
			for(int k = 0; k < sourcePopulation[j].masterAgentArray.Length; k++) {
				masterAgentArray[masterIndex] = sourcePopulation[j].masterAgentArray[k];
				masterIndex++;
			}
		}
	}

	public void ResizePopulation(int newSize) {
		this.populationMaxSize = newSize;
		Agent[] newMasterAgentArray = new Agent[populationMaxSize];
		for(int i = 0; i < populationMaxSize; i++) {
			if(i < masterAgentArray.Length) {
				newMasterAgentArray[i] = masterAgentArray[i];
			}
			else {// No agent at this index
				Agent newAgent = new Agent();
				//InitializeAgentBrainOnly(newAgent);  // create Agent's brain as proper type, and copies over templateBrain's settings
				newMasterAgentArray[i] = newAgent;
			}
		}
		numAgents = populationMaxSize;
		masterAgentArray = null;
		masterAgentArray = newMasterAgentArray;
	}

	public void PrintSettings() {
		string currentSettingsLog = "";		
		currentSettingsLog += ", Input/Output: " + numInputNodes.ToString() + " / " + numOutputNodes.ToString();
		currentSettingsLog += ", popMaxSize: " + populationMaxSize.ToString();
		currentSettingsLog += ", numAgents: " + numAgents.ToString();
		currentSettingsLog += ", isFunc: " + isFunctional.ToString();
		Debug.Log (currentSettingsLog);

		string agentListLog = "AgentArray! ";
		if(masterAgentArray != null) {
			agentListLog += "Length: " + masterAgentArray.Length;

			for(int i = 0; i < masterAgentArray.Length; i++) {
				agentListLog += ", A:" + i.ToString() + " Fit: " + masterAgentArray[i].fitnessScore;
				if(masterAgentArray[i].brain != null) {
					//agentListLog += ", brain: " + masterAgentArray[i].brain.Name;
				} else {
					agentListLog += ", brain: null";
				}
				if(masterAgentArray[i].genome != null) {
					agentListLog += ", genome: " + masterAgentArray[i].genome.genomeWeights[0].ToString();
				} else {
					agentListLog += ", genome: null";
				}
			}

		} else {
			agentListLog += "null";
		}
		Debug.Log (agentListLog);


		//public BrainBase templateBrain; // do I need this? would be used as currently selected brain for use in displaying/choosing brain settings		
		//public Agent[] masterAgentArray;
	}

}
