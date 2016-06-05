using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainingModifierManager {

    public List<TrainingModifier> activeTrainingModifierList;

	public TrainingModifierManager() {
        // empty constructor;
        if(activeTrainingModifierList == null) {
            activeTrainingModifierList = new List<TrainingModifier>();
        }        
    }

    public float GetMutationBlastParameter() {

        return 1f;
    }

    public void ApplyTrainingModifierEffects(Trainer trainer) {

        int currentGen = trainer.PlayingCurGeneration;
        CrossoverManager crossoverManager = trainer.PlayerList[0].masterCupid;
        for (int i = 0; i < activeTrainingModifierList.Count; i++) {
            float t = 0f;
            switch (activeTrainingModifierList[i].modifierType) {
                case TrainingModifier.TrainingModifierType.LinkExplosion:
                    
                    // go through all agents and pump them up -- THIS WILL NEED IMPROVEMENTS!!!!
                    for(int a = 0; a < trainer.PlayerList[0].masterPopulation.masterAgentArray.Length; a++) {
                        GenomeNEAT genome = trainer.PlayerList[0].masterPopulation.masterAgentArray[a].brainGenome;
                        int numNodes = genome.nodeNEATList.Count;
                        int numNewLinks = (int)((float)numNodes * activeTrainingModifierList[i].linksPerNode);
                        for(int n = 0; n < numNewLinks; n++) {
                            genome.AddNewRandomLink(currentGen);
                        }
                        int numLinks = genome.linkNEATList.Count;
                        int numNewNodes = (int)((float)numLinks * activeTrainingModifierList[i].nodesPerLink);
                        for(int b = 0; b < numNewNodes; b++) {
                            genome.AddNewRandomNode(currentGen);
                        }
                    }
                    break;

                case TrainingModifier.TrainingModifierType.MutationBlast:
                    t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                    crossoverManager = trainer.PlayerList[0].masterCupid;
                    if(t > 1f) {
                        t = 1f;
                    }
                    crossoverManager.mutationBlastModifier = 1f - t;
                    break;

                case TrainingModifier.TrainingModifierType.PruneBrain:
                    t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                    crossoverManager = trainer.PlayerList[0].masterCupid;
                    if (t > 1f) {
                        t = 1f;
                    }
                    t = 1f - t;
                    crossoverManager.largeBrainPenalty = activeTrainingModifierList[i].largeBrainPenalty * t;
                    crossoverManager.mutationRemoveLinkChance = activeTrainingModifierList[i].removeLinkChance * t;
                    crossoverManager.mutationRemoveNodeChance = activeTrainingModifierList[i].removeNodeChance * t;
                    break;

                case TrainingModifier.TrainingModifierType.TargetCone:
                    
                    break;

                case TrainingModifier.TrainingModifierType.TargetForward:
                    
                    break;

                case TrainingModifier.TrainingModifierType.TargetOmni:
                    
                    break;

                case TrainingModifier.TrainingModifierType.VariableTrialTimes:
                    t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                    Trial trial = trainer.PlayerList[0].masterTrialsList[0];
                    if (t > 1f) {
                        t = 1f;
                    }
                    int minSteps = (int)Mathf.Lerp(activeTrainingModifierList[i].beginMinTime, activeTrainingModifierList[i].endMinTime, t);
                    int maxSteps = (int)Mathf.Lerp(activeTrainingModifierList[i].beginMaxTime, activeTrainingModifierList[i].endMaxTime, t);
                    trial.maxEvaluationTimeSteps = maxSteps; // (int)UnityEngine.Random.Range(minSteps, maxSteps);
                    trial.minEvaluationTimeSteps = minSteps;
                    break;

                case TrainingModifier.TrainingModifierType.WideSearch:
                    t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                    crossoverManager = trainer.PlayerList[0].masterCupid;
                    if (t > 1f) {
                        t = 1f;
                    }
                    t = 1f - t;
                    //crossoverManager.largeBrainPenalty = activeTrainingModifierList[i].largeBrainPenalty * t;
                    //crossoverManager.mutationRemoveLinkChance = activeTrainingModifierList[i].removeLinkChance * t;
                    //crossoverManager.mutationRemoveNodeChance = activeTrainingModifierList[i].removeNodeChance * t;
                    break;

                default:
                    Debug.Log("Modifier type not found!!! SWITCH DEFAULT CASE");
                    break;
            }
        }
    }
}
