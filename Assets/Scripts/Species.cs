using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Species {
    public static int nextID = 0;
    public int id;

    public int currentMemberCount = 0;

    public GenomeNEAT templateGenome;


    public Species(Species baseSpecies) {
        id = baseSpecies.id;
        currentMemberCount = 0;
        templateGenome = baseSpecies.templateGenome;
    }
    public Species(Agent newAgent) {
        id = nextID;
        nextID++;
        templateGenome = newAgent.brainGenome;
        AddNewMember(newAgent);
        
        
    }

    public Species CreateExtensionSpecies() {
        Species extensionSpecies = new Species(this);
        Debug.Log("CreateExtensionSpecies() ID: " + extensionSpecies.id.ToString() + ", members: " + extensionSpecies.currentMemberCount.ToString());
        return extensionSpecies;
    }

    public void ResetMemberCount() {
        currentMemberCount = 0;
    }
    public void AddNewMember(Agent newAgent) { // when a new agent is created, assign it to a species and keep track of how many members
        //newAgent.species = this;
        
        currentMemberCount++;
    }
}
