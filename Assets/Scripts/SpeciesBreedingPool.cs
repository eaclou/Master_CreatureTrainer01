using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeciesBreedingPool {

    public List<Agent> agentList;
    public int speciesID;
    public GenomeNEAT templateGenome;
    public int nextAgentIndex = 0;

    public SpeciesBreedingPool() {
        // empty constructor for EasySave2 to work
    }
    public SpeciesBreedingPool(GenomeNEAT genome, int id) {
        agentList = new List<Agent>();
        templateGenome = genome;
        speciesID = id;
        nextAgentIndex = 0;
    }

    public void AddNewAgent(Agent newAgent) {
        newAgent.speciesID = speciesID;
        agentList.Add(newAgent);
    }
}
