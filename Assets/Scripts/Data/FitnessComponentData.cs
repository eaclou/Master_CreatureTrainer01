using UnityEngine;
using System.Collections;

public class FitnessComponentData {

    // FitnessComponentData
    //     Contains Both Raw and Modified Scores for this component (again, 0-1, or percentage of total weight, actual value?
    public AgentData[] agentDataArray;
    //public float rawValueTotal = 0f;
    //public float weightedValueTotal = 0f;
    //public float rawValueAvg = 0f;
    //public float weightedValueAvg = 0f;
    //public float rawValue01 = 0f;
    //public float[] rawValuesArray;
    //public float[] weightedValuesArray;
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
