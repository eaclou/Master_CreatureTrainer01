using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrainerDataViewUI : MonoBehaviour {

	public TrainerModuleUI trainerModuleScript;
	public Image bgImage;

    public Text textBox;
    public Text textPageTitle;
    public Button buttonPrevPage;
    public Button buttonNextPage;
    public Button buttonPrevAgent;
    public Button buttonNextAgent;

    public int currentAgent = 0;
    public int numAgents = 1;

    public enum CurrentPage {
        EachAgentBrain,
        EachAgentBrainConnections,
        GeneLinks,
        AllAgentsBrains,        
        FitnessRealtime,
        EachAgentFitnessPrev,
        AllAgentsFitnessPrev,
        AllAgentsFitnessPrevNormalized,
        SpeciesFitness
    }
    public CurrentPage currentPage = CurrentPage.EachAgentBrain;

    public Population populationRef;
    public MiniGameBase minigameRef;
    public DataManager dataManagerRef;

    public void SetCurrentAgentID(int id) {
        currentAgent = id;
    }

    public void UpdateDataText() {
        textPageTitle.text = currentPage.ToString();
        if (currentAgent >= populationRef.masterAgentArray.Length) {
            currentAgent = 0;
        }
        if(currentAgent < 0) {
            currentAgent = populationRef.masterAgentArray.Length - 1;
        }
        numAgents = populationRef.masterAgentArray.Length;

        string bodyText = "";
        
        switch(currentPage) 
        {
            case CurrentPage.EachAgentBrain:
                //+++++++++++++++++++++  EachAgentBrain ++++++++++++++++++++++++++++++++++++++++
                Agent sourceAgent = populationRef.masterAgentArray[currentAgent];
                // Intro Info:
                bodyText += "Agent #" + currentAgent.ToString() + ", speciesID: " + sourceAgent.speciesID.ToString() + "\n";
                // High-level stats:
                bodyText += sourceAgent.brain.neuronList.Count.ToString() + " nodes, " + sourceAgent.brain.connectionList.Count + " connections\n";
                // Go through all nodes:
                int curInput = 0;
                int curOutput = 0;
                for (int n = 0; n < sourceAgent.brain.neuronList.Count; n++) {
                    string neuronName = "";
                    if (sourceAgent.brain.neuronList[n].nodeType == GeneNodeNEAT.GeneNodeType.In) {
                        if (curInput <= minigameRef.inputChannelsList.Count) {
                            neuronName += minigameRef.inputChannelsList[curInput].channelName;
                            curInput++;
                        }
                    }
                    if (sourceAgent.brain.neuronList[n].nodeType == GeneNodeNEAT.GeneNodeType.Out) {
                        if (curOutput <= minigameRef.outputChannelsList.Count) {
                            neuronName += minigameRef.outputChannelsList[curOutput].channelName;
                            curOutput++;
                        }
                    }
                    bodyText += "Neuron #" + n.ToString() + " (" + sourceAgent.brain.neuronList[n].nodeType.ToString() + " - " + neuronName + ") f: " + sourceAgent.brain.neuronList[n].activationFunction.ToString() + ", pVal: "
                                + sourceAgent.brain.neuronList[n].previousValue.ToString() + ", curVal: " + sourceAgent.brain.neuronList[n].currentValue[0].ToString() + "\n";
                }
                // Go through all Connections:
                for (int c = 0; c < sourceAgent.brain.connectionList.Count; c++) {
                    bodyText += "Connection #" + c.ToString() + ", From: " + sourceAgent.brain.connectionList[c].fromNodeID.ToString() + ", To: "
                                 + sourceAgent.brain.connectionList[c].toNodeID.ToString() + ", Weight: " + sourceAgent.brain.connectionList[c].weight[0].ToString() + "\n";
                }
                break;

            case CurrentPage.EachAgentBrainConnections:
                //+++++++++++++++++++++  EachAgentBrainConnections ++++++++++++++++++++++++++++++++++++++++
                sourceAgent = populationRef.masterAgentArray[currentAgent];
                // Intro Info:
                bodyText += "Agent #" + currentAgent.ToString() + ", speciesID: " + sourceAgent.speciesID.ToString() + "\n";
                // High-level stats:
                bodyText += sourceAgent.brain.neuronList.Count.ToString() + " nodes, " + sourceAgent.brain.connectionList.Count + " connections\n";
                // Go through all Connections:
                for (int c = 0; c < sourceAgent.brain.connectionList.Count; c++) {
                    bodyText += "Connection #" + c.ToString() + ", From: " + sourceAgent.brain.connectionList[c].fromNodeID.ToString() + ", To: "
                                 + sourceAgent.brain.connectionList[c].toNodeID.ToString() + ", Weight: " + sourceAgent.brain.connectionList[c].weight[0].ToString() + "\n";
                }
                break;

            case CurrentPage.GeneLinks:
                //populationRef.geneHistoryDict
                bodyText += "GENE LINKS" + "\n";
                if(populationRef.geneHistoryDict != null) {
                    int failsafe = 0;
                    var enumerator = populationRef.geneHistoryDict.GetEnumerator();
                    while (enumerator.MoveNext()) {
                        // Access value with enumerator.Current.Value;
                        //bodyText += enumerator.Current.Value.ToString() + "\n";
                        GeneHistoryInfo geneLink = enumerator.Current.Value;
                        bodyText += "Gene Link #" + geneLink.innov.ToString() + "  numCopies: " + geneLink.numCopies.ToString() + ", from: " + geneLink.fromNode.ToString() + ", to: " + geneLink.toNode.ToString() + ", gen: " + geneLink.gen.ToString()  + ", avgWeight: " + geneLink.avgWeight.ToString() + "\n";
                        failsafe++;
                        if (failsafe > 5000) {
                            break;
                        }
                    }
                }  
                else {
                    bodyText += "Dict = NULL!";
                }              
                break;

            case CurrentPage.AllAgentsBrains:
                //+++++++++++++++++++++  AllAgentsBrains ++++++++++++++++++++++++++++++++++++++++
                // Intro Info:
                bodyText += populationRef.masterAgentArray.Length.ToString() + " Agents\n";
                
                for(int a = 0; a < populationRef.masterAgentArray.Length; a++) {
                    bodyText += "Agent #" + a.ToString();
                    int numNodes = populationRef.masterAgentArray[a].brain.neuronList.Count;
                    int numInputNodes = populationRef.masterAgentArray[a].brain.inputNeuronList.Count;
                    int numOutputNodes = populationRef.masterAgentArray[a].brain.outputNeuronList.Count;
                    int numHiddenNodes = numNodes - numInputNodes - numOutputNodes;
                    int numConnections = populationRef.masterAgentArray[a].brain.connectionList.Count;
                    bodyText += ":   TotalNodes: " + numNodes.ToString() + "   InNodes: " + numInputNodes.ToString() + ",   OutNodes: " + numOutputNodes.ToString() + ",   HidNodes: " + numHiddenNodes.ToString() + ",   Links: " + numConnections.ToString() + ", sID: " + populationRef.masterAgentArray[a].speciesID.ToString() + "\n";
                }
                break;

            case CurrentPage.FitnessRealtime:
                //+++++++++++++++++++++  EachAgentFitnessLive ++++++++++++++++++++++++++++++++++++++++
                // Intro Info:
                bodyText += minigameRef.fitnessComponentList.Count + " Fitness Components:\n";

                for (int f = 0; f < minigameRef.fitnessComponentList.Count; f++) {
                    float score = minigameRef.fitnessComponentList[f].componentScore[0];
                    if(minigameRef.gameCurrentTimeStep > 0) {
                        if (minigameRef.fitnessComponentList[f].divideByTimeSteps) {
                            score /= ((float)minigameRef.gameCurrentTimeStep + 1f);
                        }
                    }                    
                    bodyText += minigameRef.fitnessComponentList[f].componentName + ": " + score.ToString() + "\n";
                }
                break;

            case CurrentPage.EachAgentFitnessPrev:
                //+++++++++++++++++++++  EachAgentFitnessPrev ++++++++++++++++++++++++++++++++++++++++
                if(dataManagerRef.generationDataList.Count >= 2) {
                    int fitCompLength = dataManagerRef.generationDataList[dataManagerRef.generationDataList.Count - 1].trialDataArray[0].fitnessComponentDataArray.Length;
                    bodyText += "Agent #" + currentAgent.ToString() + ":\n";
                    for (int f = 0; f < fitCompLength; f++) {
                        FitnessComponentData fitData = dataManagerRef.generationDataList[dataManagerRef.generationDataList.Count - 2].trialDataArray[0].fitnessComponentDataArray[f];
                        bodyText += minigameRef.fitnessComponentList[f].componentName + " rawValueTotal: " + fitData.agentDataArray[currentAgent].rawValueTotal.ToString() + "\n";
                    }
                }                
                break;

            case CurrentPage.AllAgentsFitnessPrev:
                //+++++++++++++++++++++  AllAgentsFitnessPrev ++++++++++++++++++++++++++++++++++++++++
                if (dataManagerRef.generationDataList.Count >= 2) {
                    int fitCompLength = dataManagerRef.generationDataList[dataManagerRef.generationDataList.Count - 1].trialDataArray[0].fitnessComponentDataArray.Length;
                    //bodyText += "Agent #" + currentAgent.ToString() + ":\n";
                    //List<float> agentFitCompList = new List<float>();
                    
                        
                    for (int f = 0; f < fitCompLength; f++) {
                        float fitCompTotal = 0f;
                        for (int a = 0; a < numAgents; a++) {
                            FitnessComponentData fitData = dataManagerRef.generationDataList[dataManagerRef.generationDataList.Count - 2].trialDataArray[0].fitnessComponentDataArray[f];
                            fitCompTotal += fitData.agentDataArray[a].rawValueTotal;
                        }
                        bodyText += minigameRef.fitnessComponentList[f].componentName + " rawValueTotal: " + fitCompTotal.ToString() + "\n";
                    }                    
                }
                break;

            case CurrentPage.AllAgentsFitnessPrevNormalized:
                //+++++++++++++++++++++  AllAgentsFitnessPrevNormalized ++++++++++++++++++++++++++++++++++++++++
                if (dataManagerRef.generationDataList.Count >= 2) {                    
                    for (int a = 0; a < numAgents; a++) {
                        
                        bodyText += "Agent #" + a.ToString() + " Fitness Score: " + populationRef.masterAgentArray[a].parentFitnessScoreA + "\n";
                    }
                }
                break;

            case CurrentPage.SpeciesFitness:
                //+++++++++++++++++++++  SpeciesFitness ++++++++++++++++++++++++++++++++++++++++
                for(int i = 0; i < populationRef.speciesBreedingPoolList.Count; i++) {
                    bodyText += "Species #: " + populationRef.speciesBreedingPoolList[i].speciesID.ToString() + ", SpeciesSize: " + populationRef.speciesBreedingPoolList[i].agentList.Count.ToString() + " Avg Fitness: ";
                    if(populationRef.speciesBreedingPoolList[i].agentList.Count > 0) {
                        float speciesAvgFitness = 0f;
                        for (int j = 0; j < populationRef.speciesBreedingPoolList[i].agentList.Count; j++) {
                            speciesAvgFitness += populationRef.speciesBreedingPoolList[i].agentList[j].parentFitnessScoreA;
                        }
                        speciesAvgFitness /= populationRef.speciesBreedingPoolList[i].agentList.Count;
                        bodyText += speciesAvgFitness.ToString() + "\n";
                    }
                    else {
                        bodyText += "(((EXTINCT)))\n";
                    }
                    
                }                
                break;

            default:
                Debug.Log("Page type not found!!!");
                break;
        }
        

        // Apply to textField:
        textBox.text = bodyText;
    }

    public void ClickPrevPage() {
        int numPages = System.Enum.GetNames(typeof(CurrentPage)).Length;
        int curPage = (int)currentPage;
        int newPage = curPage - 1;
        if(newPage < 0) {
            newPage = numPages - 1;
        }
        currentPage = (CurrentPage)newPage;
        //textPageTitle.text = currentPage.ToString();
        UpdateDataText();
    }
    public void ClickNextPage() {
        int numPages = System.Enum.GetNames(typeof(CurrentPage)).Length;
        int curPage = (int)currentPage;
        int newPage = curPage + 1;
        if (newPage >= numPages) {
            newPage = 0;
        }
        currentPage = (CurrentPage)newPage;
        //textPageTitle.text = currentPage.ToString();
        UpdateDataText();
    }
    public void ClickPrevAgent() {
        currentAgent -= 1;
        if (currentAgent < 0) {
            currentAgent = numAgents;
        }
        UpdateDataText();
    }
    public void ClickNextAgent() {
        currentAgent += 1;
        if (currentAgent >= numAgents) {
            currentAgent = 0;
        }
        UpdateDataText();
    }
}
