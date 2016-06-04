using UnityEngine;
using System.Collections;

public class TrainingModifier {

    public TrainingModifierType modifierType;
    public enum TrainingModifierType {
        LinkExplosion,
        MutationBlast,
        PruneBrain,
        TargetConeUI,
        TargetForwardUI,
        TargetOmniUI,
        VariableTrialTimes,
        WideSearch
    };

    // Variables for all modifier types (should make saving/loading slightly easier)
    public int duration;
    public int numRounds;
    public bool liveForever;
    public bool decayEffectOverDuration;
    public float beginMinDistance;
    public float beginMaxDistance;
    public float endMinDistance;
    public float endMaxDistance;
    public float beginMinAngle;
    public float beginMaxAngle;
    public float endMinAngle;
    public float endMaxAngle;
    public float linksPerNode;
    public float nodesPerLink;
    public float largeBrainPenalty;
    public float removeLinkChance;
    public float removeNodeChance;
    public bool forward;
    public bool horizontal;
    public bool vertical;
    public float beginMinTime;
    public float beginMaxTime;
    public float endMinTime;
    public float endMaxTime;
    public float speciesSimilarityThreshold;
    public float adoptionRate;
    public float largeSpeciesPenalty;
    
}
