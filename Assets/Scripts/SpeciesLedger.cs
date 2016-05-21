using UnityEngine;
using System.Collections;

public class SpeciesLedger {
    // Keeps track of current species?
    public int nextID = -1;
    
    public SpeciesLedger() {

    }

    public int GetNextID() {
        nextID++;
        return nextID;
    }
}
