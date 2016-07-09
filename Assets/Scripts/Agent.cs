using UnityEngine;
using System.Collections;

public class Agent {

	public bool debugFunctionCalls = false; // turns debug messages on/off

	public float fitnessScore;
    public float parentFitnessScoreA;
    public float fitnessScoreSpecies;

	public BrainNEAT brain; // This is where the magic happens
	//public Genome genome; // encodes the brain network in long single-dimension arrays
    public GenomeNEAT brainGenome;
	public CritterGenome bodyGenome;  // encodes the body shape of the creature in a list of SegmentGenomes

    public int speciesID;
    public int fitnessRank = -1;

	// Constructor Methods:
	public Agent() {
		DebugBot.DebugFunctionCall("Agent; Agent() Constructor!; ", debugFunctionCalls);
	}

	public void InitializeRandomGenome(int numInputs, int numOutputs) {
		brain.InitializeRandomBrain(numInputs, numOutputs);
	}

    public void InitializeBlankGenome(int numInputs, int numOutputs) {
        brain.InitializeBlankBrain(numInputs, numOutputs);
    }

    public Agent RefAgent() {
		Agent agentRef = new Agent();
		agentRef = this;
		return agentRef;
	}

	public Agent CopyAgent() {
		Agent agentCopy = new Agent();

		return agentCopy;
	}
}
