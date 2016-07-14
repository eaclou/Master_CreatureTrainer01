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

    public void ApplyTrainingModifierEffectsTarget(Trainer trainer, MiniGameBase minigame) {
        int currentGen = trainer.PlayingCurGeneration; // + trainer.PlayerList[0].masterPopulation.trainingGenerations;
        if (trainer.loadedTrainingSave != null) {
            currentGen += trainer.loadedTrainingSave.endGeneration;
        }
        int numModifiers = activeTrainingModifierList.Count;
        int numRounds = 0;
        int gameRoundIndex = 0;

        // v v v SUPER HACKY!!! v v v 
        MiniGameCritterWalkBasic game = (MiniGameCritterWalkBasic)minigame;
        // Clear existing Lists:
        if (game.targetPositionList == null) {
            game.targetPositionList = new List<Vector3>();
        }
        else {
            game.targetPositionList.Clear();
        }
        if (game.endGameTimesList == null) {
            game.endGameTimesList = new List<int>();
        }
        else {
            game.endGameTimesList.Clear();
        }
        if (game.initialForceList == null) {
            game.initialForceList = new List<Vector3>();
        }
        else {
            game.initialForceList.Clear();
        }

        if (numModifiers > 0) {
            int numTargetModifiers = 0;
            for (int i = numModifiers - 1; i >= 0; i--) {  // iterate through all modifiers
                float t = 0f;
                switch (activeTrainingModifierList[i].modifierType) {
                    case TrainingModifier.TrainingModifierType.LinkExplosion:

                        break;

                    case TrainingModifier.TrainingModifierType.MutationBlast:
                        
                        break;

                    case TrainingModifier.TrainingModifierType.PruneBrain:
                        
                        break;

                    case TrainingModifier.TrainingModifierType.TargetCone:
                        numTargetModifiers++;
                        numRounds += activeTrainingModifierList[i].numRounds;

                        t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                        if(t > 1f) {
                            t = 1f;
                        }
                        
                        for (int c = 0; c < activeTrainingModifierList[i].numRounds; c++) {
                            float horAngle = 0f;
                            float verAngle = 0f;
                            float minAngle = Mathf.Lerp(activeTrainingModifierList[i].beginMinAngle, activeTrainingModifierList[i].endMinAngle, t);
                            float maxAngle = Mathf.Lerp(activeTrainingModifierList[i].beginMaxAngle, activeTrainingModifierList[i].endMaxAngle, t);
                            if (activeTrainingModifierList[i].horizontal) {
                                horAngle = UnityEngine.Random.Range(minAngle, maxAngle) * Mathf.PI / 180f;
                                // Flip to negative axis check
                                if (UnityEngine.Random.Range(0f, 1f) < 0.5f) {
                                    horAngle *= -1f;
                                }
                            }
                            if (activeTrainingModifierList[i].vertical) {
                                verAngle = UnityEngine.Random.Range(minAngle, maxAngle) * Mathf.PI / 180f;
                                if (UnityEngine.Random.Range(0f, 1f) < 0.5f) {
                                    verAngle *= -1f;
                                }
                            }
                            
                            float minDist = Mathf.Lerp(activeTrainingModifierList[i].beginMinDistance, activeTrainingModifierList[i].endMinDistance, t);
                            float maxDist = Mathf.Lerp(activeTrainingModifierList[i].beginMaxDistance, activeTrainingModifierList[i].endMaxDistance, t);
                            float dist = UnityEngine.Random.Range(minDist, maxDist);

                            Vector2 hor = new Vector2(Mathf.Sin(horAngle) * dist, Mathf.Cos(horAngle) * dist).normalized;
                            float x = Mathf.Sin(horAngle) * dist * Mathf.Cos(verAngle);
                            float z = Mathf.Cos(horAngle) * dist * Mathf.Cos(verAngle);
                            float y = Mathf.Sin(verAngle) * dist;

                            game.targetPositionList.Add(new Vector3(x, y, z));
                            int randEndTime = Mathf.RoundToInt(UnityEngine.Random.Range(trainer.PlayerList[0].masterTrialsList[0].minEvaluationTimeSteps, trainer.PlayerList[0].masterTrialsList[0].maxEvaluationTimeSteps)); 
                            game.endGameTimesList.Add(randEndTime);
                            gameRoundIndex++;

                            Vector3 forceDir = UnityEngine.Random.onUnitSphere;
                            float forceMag = UnityEngine.Random.Range(game.customSettings.initForceMin[0], game.customSettings.initForceMax[0]);
                            game.initialForceList.Add(new Vector3(forceDir.x * forceMag, forceDir.y * forceMag, forceDir.z * forceMag));
                        }

                        break;

                    case TrainingModifier.TrainingModifierType.TargetForward:
                        numTargetModifiers++;
                        numRounds += activeTrainingModifierList[i].numRounds;

                        t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                        if (t > 1f) {
                            t = 1f;
                        }

                        for (int c = 0; c < activeTrainingModifierList[i].numRounds; c++) {
                            
                            float minDist = Mathf.Lerp(activeTrainingModifierList[i].beginMinDistance, activeTrainingModifierList[i].endMinDistance, t);
                            float maxDist = Mathf.Lerp(activeTrainingModifierList[i].beginMaxDistance, activeTrainingModifierList[i].endMaxDistance, t);
                            float dist = UnityEngine.Random.Range(minDist, maxDist);

                            game.targetPositionList.Add(new Vector3(0f, 0f, dist));
                            int randEndTime = Mathf.RoundToInt(UnityEngine.Random.Range(trainer.PlayerList[0].masterTrialsList[0].minEvaluationTimeSteps, trainer.PlayerList[0].masterTrialsList[0].maxEvaluationTimeSteps));
                            game.endGameTimesList.Add(randEndTime);
                            gameRoundIndex++;

                            Vector3 forceDir = UnityEngine.Random.onUnitSphere;
                            float forceMag = UnityEngine.Random.Range(game.customSettings.initForceMin[0], game.customSettings.initForceMax[0]);
                            game.initialForceList.Add(new Vector3(forceDir.x * forceMag, forceDir.y * forceMag, forceDir.z * forceMag));
                        }

                        break;

                    case TrainingModifier.TrainingModifierType.TargetOmni:
                        numTargetModifiers++;
                        numRounds += activeTrainingModifierList[i].numRounds;

                        t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                        if (t > 1f) {
                            t = 1f;
                        }

                        for (int c = 0; c < activeTrainingModifierList[i].numRounds; c++) {

                            float randX = 0f;
                            float randY = 0f;
                            float randZ = 0f;
                            if(activeTrainingModifierList[i].horizontal) {
                                randX = UnityEngine.Random.Range(-1f, 1f);
                            }
                            if (activeTrainingModifierList[i].vertical) {
                                randY = UnityEngine.Random.Range(-1f, 1f);
                            }
                            if (activeTrainingModifierList[i].forward) {
                                randZ = UnityEngine.Random.Range(-1f, 1f);
                            }                         
                            Vector3 randDir = new Vector3(randX, randY, randZ).normalized;
                            
                            float minDist = Mathf.Lerp(activeTrainingModifierList[i].beginMinDistance, activeTrainingModifierList[i].endMinDistance, t);
                            float maxDist = Mathf.Lerp(activeTrainingModifierList[i].beginMaxDistance, activeTrainingModifierList[i].endMaxDistance, t);
                            float dist = UnityEngine.Random.Range(minDist, maxDist);
                            
                            game.targetPositionList.Add(new Vector3(randDir.x * dist, randDir.y * dist, randDir.z * dist));
                            int randEndTime = Mathf.RoundToInt(UnityEngine.Random.Range(trainer.PlayerList[0].masterTrialsList[0].minEvaluationTimeSteps, trainer.PlayerList[0].masterTrialsList[0].maxEvaluationTimeSteps));
                            game.endGameTimesList.Add(randEndTime);
                            gameRoundIndex++;

                            Vector3 forceDir = UnityEngine.Random.onUnitSphere;
                            float forceMag = UnityEngine.Random.Range(game.customSettings.initForceMin[0], game.customSettings.initForceMax[0]);
                            game.initialForceList.Add(new Vector3(forceDir.x * forceMag, forceDir.y * forceMag, forceDir.z * forceMag));
                        }

                        break;

                    case TrainingModifier.TrainingModifierType.VariableTrialTimes:
                        
                        break;

                    case TrainingModifier.TrainingModifierType.WideSearch:
                        
                        break;

                    default:
                        Debug.Log("Modifier type not found!!! SWITCH DEFAULT CASE");
                        break;
                }
            }
            if(numTargetModifiers == 0) {
                // proceed as default
                minigame.ResetTargetPositions(trainer.PlayerList[0].masterTrialsList[0].numberOfPlays, trainer.PlayerList[0].masterTrialsList[0].minEvaluationTimeSteps, trainer.PlayerList[0].masterTrialsList[0].maxEvaluationTimeSteps);
            }
            else {
                trainer.PlayerList[0].masterTrialsList[0].numberOfPlays = numRounds;
            }            
        }
        else {  // No Modifiers!
            minigame.ResetTargetPositions(trainer.PlayerList[0].masterTrialsList[0].numberOfPlays, trainer.PlayerList[0].masterTrialsList[0].minEvaluationTimeSteps, trainer.PlayerList[0].masterTrialsList[0].maxEvaluationTimeSteps);
        }
    }

    public void ApplyTrainingModifierEffects(Trainer trainer) {

        int currentGen = trainer.PlayingCurGeneration;
        if(trainer.loadedTrainingSave != null) {
            currentGen += trainer.loadedTrainingSave.endGeneration;
        }
        //Debug.Log("ApplyTrainingModifierEffects  currentGen: " + currentGen.ToString());
        CrossoverManager crossoverManager = trainer.PlayerList[0].masterCupid;
        int numModifiers = activeTrainingModifierList.Count;
        crossoverManager.mutationBlastModifier = 1f;
        if (numModifiers > 0) {
            for (int i = numModifiers - 1; i >= 0; i--) {
                float t = 0f;
                switch (activeTrainingModifierList[i].modifierType) {
                    case TrainingModifier.TrainingModifierType.LinkExplosion:

                        // go through all agents and pump them up -- THIS WILL NEED IMPROVEMENTS!!!!
                        for (int a = 0; a < trainer.PlayerList[0].masterPopulation.masterAgentArray.Length; a++) {
                            GenomeNEAT genome = trainer.PlayerList[0].masterPopulation.masterAgentArray[a].brainGenome;
                            int numNodes = genome.nodeNEATList.Count;
                            int numNewLinks = (int)((float)numNodes * activeTrainingModifierList[i].linksPerNode);
                            for (int n = 0; n < numNewLinks; n++) {
                                genome.AddNewRandomLink(currentGen);
                            }
                            int numLinks = genome.linkNEATList.Count;
                            int numNewNodes = (int)((float)numLinks * activeTrainingModifierList[i].nodesPerLink);
                            for (int b = 0; b < numNewNodes; b++) {
                                genome.AddNewRandomNode(currentGen);
                            }
                        }
                        activeTrainingModifierList.RemoveAt(i);
                        break;

                    case TrainingModifier.TrainingModifierType.MutationBlast:
                        t = ((float)currentGen - (float)activeTrainingModifierList[i].startGen) / (float)activeTrainingModifierList[i].duration;
                        crossoverManager = trainer.PlayerList[0].masterCupid;
                        if (t > 1f) {
                            if(activeTrainingModifierList[i].liveForever) {
                                t = 1f;
                            }
                            else {
                                // Duration has expired, and the live forever flag if false, so remove this modifier
                                crossoverManager.mutationBlastModifier = 1f;
                                activeTrainingModifierList.RemoveAt(i);
                                break;
                            }
                        }
                        crossoverManager.mutationBlastModifier = Mathf.Lerp(1f, activeTrainingModifierList[i].minMultiplier, t);
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
                            if(activeTrainingModifierList[i].liveForever) {
                                t = 1f;
                            }
                            else {
                                // Remove this modifier
                                activeTrainingModifierList.RemoveAt(i);
                            }
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
}
