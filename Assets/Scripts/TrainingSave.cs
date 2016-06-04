using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainingSave {

    // Used as a wrapper to hold all the data from a training run, so that it can be loaded at a later date or used for analysis re: what settings are effective
    public CrossoverManager savedCrossoverManager;
    public Population savedPopulation;
    public int beginGeneration;
    public int endGeneration;
    // begin fitness scores (look up dataManager to find these)
    public TrialData savedTrialDataBegin;
    // end fitness scores (at time of save)
    public TrialData savedTrialDataEnd;
    public List<FitnessComponent> savedFitnessComponentList;   // used to look up individual weights and bigIsBetter in order to interpret raw scores
    // Minigame settings:
    public MiniGameSettingsSaves savedMiniGameSettings;  // custom class to circumvent issues with saving Inherited Classes        

    public TrainingSave() {
        // empty constructor
    }
}
