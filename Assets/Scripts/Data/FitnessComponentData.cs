using UnityEngine;
using System.Collections;

public class FitnessComponentData {

    // FitnessComponentData
    //     Contains Both Raw Scores for this component
    public AgentData[] agentDataArray;  // collection of all agents in the population, with their component score for this Fitness Component
    public float lowestScore = 0f;  // These are the RAW score values, does not take into account BiggerIsBetter!!!!
    public float highestScore = 0f;

    public float totalRawScore = 0f; // total combined score of all Agents for this fitnessComponent

    public FitnessComponentData() {
        //empty constructor to prevent memory allocation of AgentData Array
    }
	
	public FitnessComponentData(int numAgents) {
        agentDataArray = new AgentData[numAgents];
        //rawValuesArray = new float[numGameRounds];
		//weightedValuesArray = new float[numGameRounds];
	}
}
