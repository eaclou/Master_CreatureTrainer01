using UnityEngine;
using System.Collections;

public class GenerationData {

    // GenerationData
    //     Holds data for all Agents for this generation
    //     Holds value for number of Agents in this gen
    //public AgentData[] agentDataArray;
    public TrialData[] trialDataArray;
	//public float totalAgentScoresRaw;
	//public float totalAgentScoresWeighted;
	//public float avgAgentScoreRaw;
	//public float avgAgentScoreWeighted;
	public int totalNumFitnessComponents;
	public float totalSumOfWeights = 0f; // keeps track of total of all TrialIndex weight values for this Generation
	//public Genome genAvgGenome;  // REVISIT!!!
    public float totalAllAgentsScore = 0f;

    // Keep track of each fitnessComponent avg score?
    // Keep track of generation to compare avg scores to?
	
	public GenerationData(int numTrials) {
        //agentDataArray = new AgentData[numAgents]; // old
        trialDataArray = new TrialData[numTrials];
		//genAvgGenome = new Genome();
	}
}
